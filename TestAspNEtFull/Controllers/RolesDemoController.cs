using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TestAspNEtFull.Entities;

namespace TestAspNEtFull.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesDemoController : ControllerBase
{
    // Лише користувачі з роллю Admin
    [Authorize(Roles = nameof(Roles.Admin))]
    [HttpGet("admin")]
    public IActionResult OnlyAdmins() => Ok("Admin access granted");

    // Admin або Manager
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
    [HttpGet("management")]
    public IActionResult ManagersAndAdmins() => Ok("Manager/Admin access granted");
    // Також можна в bitwise enum Roles додати нове значення AdminAndManager = Admin | Manager і використовувати його
    // [Authorize(Roles = Roles.AdminAndManager)]
    // це і є сила bitwise enum

    // Дивимось усі claims
    [Authorize]
    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        var userEmail = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);
        return Ok(new { userEmail, roles });
    }
}