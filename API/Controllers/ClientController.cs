using Application.Clients;
using Application.Clients.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ClientController(IClientService clientService) : ControllerBase
{
    [HttpPost("individual")]
    public async Task<IActionResult> AddIndividualClient([FromBody] AddIndividualClientRequest request, CancellationToken cancellationToken)
    {
        var clientId = await clientService.AddIndividualClientAsync(request, cancellationToken);
        return Created(string.Empty, new { ClientId = clientId });
    }
    
    [HttpPost("company")]
    public async Task<IActionResult> AddCompanyClient([FromBody] AddCompanyClientRequest request, CancellationToken cancellationToken)
    {
        var clientId = await clientService.AddCompanyClientAsync(request, cancellationToken);
        return Created(string.Empty, new { ClientId = clientId });
    }
}