using System;
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
        return Ok(new { Token = token });
    }
}