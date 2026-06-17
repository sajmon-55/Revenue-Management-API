using Application.Contracts.DTOs;

namespace Application.Contracts;

public interface IContractService
{
    Task<int> CreateContractAsync(CreateContractRequest request, CancellationToken cancellationToken);
}