using Application.Softwares;
using Application.Softwares.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class SoftwareController(ISoftwareService softwareService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddSoftware([FromBody] CreateSoftwareRequest request, CancellationToken cancellationToken)
    {
        await softwareService.AddSoftwareAsync(request, cancellationToken);
        return Created();
    }
}