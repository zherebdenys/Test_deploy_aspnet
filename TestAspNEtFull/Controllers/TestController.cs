using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestAspNEtFull.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult PublicEndpoint()
    {
        return Ok("Це відкритий ендпойнт.");
    }

    [Authorize]
    [HttpGet("private")]
    public IActionResult PrivateEndpoint()
    {
        var userEmail = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        return Ok($"Це приватний ендпойнт! Ваш email: {userEmail}");
    }
}