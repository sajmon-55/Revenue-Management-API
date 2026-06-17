namespace Application.Payments.DTOs;

public record ProccesContractPaymentRequest(
    int ContractId,
    decimal Amount
);