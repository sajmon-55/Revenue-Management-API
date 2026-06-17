using Application.Contracts;
using Application.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractController(IContractService contractService) : ControllerBase
{
    [HttpPost("create-contract")]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest request, CancellationToken cancellationToken)
    {
        await contractService.CreateContractAsync(request, cancellationToken);
        return Created();
    }
}