using Application.Clients.DTOs;
using Application.Exceptions;
using Domain.Entities.Clients;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Clients;

public class ClientService(DatabaseContext dbContext) : IClientService
{
    public async Task<int> AddIndividualClientAsync(AddIndividualClientRequest request, CancellationToken cancellationToken)
    {
        if (await dbContext.IndividualClients.AnyAsync(c => c.Pesel == request.Pesel, cancellationToken))
        {
            throw new ConflictException("Client with the specified PESEL number already exists");
        }

        var client = new Client
        {
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            IndividualClient = new IndividualClient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Pesel = request.Pesel,
                IsDeleted = false,
            }
        };
        
        await dbContext.Clients.AddAsync(client, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return client.Id;
    }

    public async Task<int> AddCompanyClientAsync(AddCompanyClientRequest request, CancellationToken cancellationToken)
    {
        if (await dbContext.CompanyClients.AnyAsync(c => c.Krs == request.Krs, cancellationToken))
        {
            throw new ConflictException("Company with the specified KRS number already exists");
        }

        var client = new Client
        {
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            CompanyClient = new CompanyClient
            {
                Name = request.Name,
                Krs = request.Krs,
            }
        };
        
        await dbContext.Clients.AddAsync(client, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return client.Id;
    }
}