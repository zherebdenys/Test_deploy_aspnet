using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAspNEtFull.Services;

namespace TestAspNEtFull.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(Roles.Admin))]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;


    [HttpPost("setRoles")]
    public async Task<IActionResult> SetRoles(string id, Roles roles)
    {
        var user = await _userService.GetAsync(id) ;
        if (user == null) return NotFound("User not found");

        user.Role = roles;
        await _userService.UpdateAsync(user);

        return Ok(new { username = user.Username, roles = user.Role, rolesValue = (int)user.Role });
    }

    [HttpPost("addRole")]
    public async Task<IActionResult> AddRole(string id, Roles role)
    {
        var user = await _userService.GetAsync(id);
        if (user == null) return NotFound();

        user.Role |= role; // додаємо прапорець
        await _userService.UpdateAsync(user);

        return Ok(new { username = user.Username, roles = user.Role, rolesValue = (int)user.Role });
    }

    [HttpPost("removeRole")]
    public async Task<IActionResult> RemoveRole(string id, Roles role)
    {
        var user = await _userService.GetAsync(id);
        if (user == null) return NotFound();

        user.Role &= ~role; // знімаємо прапорець
        await _userService.UpdateAsync(user);

        return Ok(new { username = user.Username, roles = user.Role, rolesValue = (int)user.Role });
    }
}

