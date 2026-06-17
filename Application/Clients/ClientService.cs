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

    public async Task UpdateClientAsync(int id, UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var client = await dbContext.Clients
                         .Include(c => c.IndividualClient)
                         .Include(c => c.CompanyClient)
                         .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
                     ?? throw new NotFoundException("Client not found");

        client.Email = request.Email;
        client.Phone = request.Phone;
        client.Address = request.Address;

        if (client.IndividualClient != null)
        {
            if (client.IndividualClient.IsDeleted)
                throw new ConflictException("Editing deleted client is not allowed!");

            client.IndividualClient.FirstName = request.FirstName ?? client.IndividualClient.FirstName;
            client.IndividualClient.LastName = request.LastName ?? client.IndividualClient.LastName;
        }
        else if (client.CompanyClient != null)
        {
            client.CompanyClient.Name = request.CompanyName ?? client.CompanyClient.Name;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteClientAsync(int id, CancellationToken cancellationToken)
    {
        var client = await dbContext.Clients
                         .Include(c => c.IndividualClient)
                         .Include(c => c.CompanyClient)
                         .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
                     ?? throw new NotFoundException("Client not found");
        
        if (client.CompanyClient != null)
        {
            throw new ConflictException("Company data cannot be deleted!");
        }

        if (client.IndividualClient != null)
        {
            if (client.IndividualClient.IsDeleted)
            {
                throw new ConflictException("This client is already deleted!");
            }

            client.Address = "deleted";
            client.Email = "deleted";
            client.Phone = "000000000";
            
            client.IndividualClient.FirstName = "deleted";
            client.IndividualClient.LastName = "deleted";
            client.IndividualClient.IsDeleted = true;
            
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}