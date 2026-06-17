namespace Application.Payments.DTOs;

public record ProcessSubscriptionPaymentRequest (
    int SubscriptionId,
    decimal Amount
);