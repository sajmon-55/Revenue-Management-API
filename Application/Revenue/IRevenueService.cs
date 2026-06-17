namespace Application.Revenue;

public interface IRevenueService
{
    Task<decimal> CalculateCurrentRevenueAsync(int? softwareId, string? currency, CancellationToken cancellationToken);
    Task<decimal> CalculatePredictedRevenueAsync(int? softwareId, string? currency, CancellationToken cancellationToken);
}