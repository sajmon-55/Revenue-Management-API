using Application.Revenue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // Finanse widzi tylko Admin
public class RevenueController(IRevenueService revenueService) : ControllerBase
{
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRevenue([FromQuery] int? softwareId, [FromQuery] string? currency, CancellationToken cancellationToken)
    {
        var revenue = await revenueService.CalculateCurrentRevenueAsync(softwareId, currency, cancellationToken);
        return Ok(new { Revenue = revenue, Currency = currency?.ToUpper() ?? "PLN" });
    }

    [HttpGet("predicted")]
    public async Task<IActionResult> GetPredictedRevenue([FromQuery] int? softwareId, [FromQuery] string? currency, CancellationToken cancellationToken)
    {
        var revenue = await revenueService.CalculatePredictedRevenueAsync(softwareId, currency, cancellationToken);
        return Ok(new { Revenue = revenue, Currency = currency?.ToUpper() ?? "PLN" });
    }
}