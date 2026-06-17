using Application.Clients.DTOs;

namespace Application.Clients;

public interface IClientService
{
    Task<int> AddIndividualClientAsync(AddIndividualClientRequest request, CancellationToken cancellationToken);
    Task<int> AddCompanyClientAsync(AddCompanyClientRequest request, CancellationToken cancellationToken);
    Task UpdateClientAsync(int id, UpdateClientRequest request, CancellationToken cancellationToken);
    Task DeleteClientAsync(int id, CancellationToken cancellationToken);
}