using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUserData()
    {
        return Ok("User is logged in!" + HttpContext.User.FindFirst(ClaimTypes.Name)!.Value);
    }
}