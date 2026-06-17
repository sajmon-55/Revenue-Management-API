using Application.Exceptions;
using Application.Payments.DTOs;
using Domain.Entities.Contracts;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Payments;

public class PaymentService(DatabaseContext dbContext) : IPaymentService
{
    public async Task ProcessContractPaymentAsync(ProccesContractPaymentRequest request, CancellationToken cancellationToken)
    {
        var contract = await dbContext.Contracts
            .Include(c => c.ContractPayments)
            .FirstOrDefaultAsync(c => c.Id == request.ContractId, cancellationToken)
            ?? throw new NotFoundException("Contract not found");
        
        if (contract.StatusId == 4)
        {
            throw new ConflictException("Payment for contract is already done");
        }
        
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (currentDate > contract.EndDate)
        {
            contract.StatusId = 3;
            await dbContext.SaveChangesAsync(cancellationToken);
            throw new ConflictException("Time is over, this contract is cancelled");
        }
        
        var payment = new ContractPayment
        {
            ContractId = contract.Id,
            Amount = request.Amount,
            PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow),
        };
        
        contract.ContractPayments.Add(payment);

        var totalPaid = contract.ContractPayments.Sum(p => p.Amount);
        
        if (totalPaid >= contract.Price)
        {
            contract.StatusId = 2;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ProcessSubscriptionPaymentAsync(ProcessSubscriptionPaymentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}