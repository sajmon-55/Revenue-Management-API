using System.Diagnostics.Contracts;
using Application.Contracts.DTOs;
using Application.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Contract = Domain.Entities.Contracts.Contract;

namespace Application.Contracts;

public class ContractService(DatabaseContext dbContext) : IContractService
{
    public async Task<int> CreateContractAsync(CreateContractRequest request, CancellationToken cancellationToken)
    {
        var hasActiveContract = await dbContext.Contracts
            .AnyAsync(c => c.ClientId == request.ClientId && c.SoftwareId == request.SoftwareId && c.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);
        
        if(hasActiveContract)
            throw new ConflictException("Client already have active contract on this software.");
        
        var software = await dbContext.Softwares
                           .Include(s => s.SoftwareDiscounts)
                           .ThenInclude(sd => sd.Discount)
                           .FirstOrDefaultAsync(s => s.Id == request.SoftwareId, cancellationToken) 
                       ?? throw new NotFoundException("Software not found.");
        
        decimal supportCost = request.YearsOfSupport * 1000m;
        decimal basePrice = request.Price + supportCost;
        
        decimal maxDiscountValue = software.SoftwareDiscounts
            .Where(sd => sd.Discount.DateFrom <= request.StartDate && sd.Discount.DateTo >= request.EndDate)
            .Select(sd => sd.Discount.Value)
            .DefaultIfEmpty(0)
            .Max();
        
        bool isReturning = await dbContext.Contracts.AnyAsync(c => c.ClientId == request.ClientId, cancellationToken)
                           || await dbContext.Subscriptions.AnyAsync(s => s.ClientId == request.ClientId, cancellationToken);
        
        decimal finalPrice = basePrice * (1 - maxDiscountValue);
        if (isReturning) finalPrice -= (basePrice * 0.05m);
        
        var duration = request.EndDate.DayNumber - request.StartDate.DayNumber;
        if (duration < 3 || duration > 30) throw new Exception("Kontrakt musi trwać od 3 do 30 dni.");

        var contract = new Contract
        {
            ClientId = request.ClientId,
            SoftwareId = request.SoftwareId,
            SoftwareVersion = request.SoftwareVersion,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Price = finalPrice,
            YearsOfSupport = request.YearsOfSupport,
            StatusId = 1
        };

        await dbContext.Contracts.AddAsync(contract, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return contract.Id;

    }
}