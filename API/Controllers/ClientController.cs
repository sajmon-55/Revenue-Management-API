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
        await clientService.AddIndividualClientAsync(request, cancellationToken);
        return Created();
    }
    
    [HttpPost("company")]
    public async Task<IActionResult> AddCompanyClient([FromBody] AddCompanyClientRequest request, CancellationToken cancellationToken)
    {
        await clientService.AddCompanyClientAsync(request, cancellationToken);
        return Created();
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
    {
        await clientService.UpdateClientAsync(id, request, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClient(int id, CancellationToken cancellationToken)
    {
        await clientService.DeleteClientAsync(id, cancellationToken);
        return NoContent();
    }
}