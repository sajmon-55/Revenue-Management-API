using Application.Payments;
using Application.Payments.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PayForContract([FromBody] ProccesContractPaymentRequest request, CancellationToken cancellationToken)
    {
        await paymentService.ProcessContractPaymentAsync(request, cancellationToken);
        return Ok(new { Message = "Płatność została przetworzona." });
    }
}