using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TestAspNEtFull.Entities;
using TestAspNEtFull.Helpers;
using TestAspNEtFull.Services;

namespace TestAspNEtFull.Controllers;

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public AuthController(IUserService userService, JwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        var existingUser = await _userService.GetByEmailAsync(user.Email);
        if (existingUser != null)
            return BadRequest("Користувач уже існує.");

        user.Password = _passwordHasher.HashPassword(user.Password);
        await _userService.CreateAsync(user);

        return Ok("Користувач зареєстрований.");
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userService.GetByEmailAsync(model.Email);

        if (user == null)
            return Unauthorized("Неправильна пошта або пароль.");

        if (user.Password != _passwordHasher.HashPassword(model.Password))
            return Unauthorized("Непраивльний пароль.");

        var token = _jwtTokenGenerator.Generate(user);
        // Генеруємо refresh token
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        // Зберігаємо refresh token у користувача
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userService.UpdateAsync(user);

        return Ok(new {
            Token = token,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromHeader(Name = "Authorization")] string authHeader, [FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return BadRequest("Missing or invalid Authorization header");

        var token = authHeader.Substring("Bearer ".Length);

        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(token);

        if (principal == null)
            return BadRequest("Invalid refresh token");

        var id = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (id == null) throw new Exception("Invalid token claims");

        var user = await _userService.GetAsync(id);

        if (user == null ||
            user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Unauthorized("Invalid refresh token");
        }

        var newAccessToken = _jwtTokenGenerator.Generate(user);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userService.UpdateAsync(user);

        return Ok(new
        {
            token = newAccessToken,
            refreshToken = newRefreshToken,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
        });
    }
}