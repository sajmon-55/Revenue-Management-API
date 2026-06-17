using Application.Payments.DTOs;

namespace Application.Payments;

public interface IPaymentService
{
    Task ProcessContractPaymentAsync(ProccesContractPaymentRequest request, CancellationToken cancellationToken);
    Task ProcessSubscriptionPaymentAsync(ProcessSubscriptionPaymentRequest request, CancellationToken cancellationToken);
}