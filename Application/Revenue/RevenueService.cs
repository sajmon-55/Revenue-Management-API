using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace Application.Revenue;

public class RevenueService(DatabaseContext dbContext, HttpClient httpClient) : IRevenueService
{
    public async Task<decimal> CalculateCurrentRevenueAsync(int? softwareId, string? currency, CancellationToken cancellationToken)
    {
        var query = dbContext.Contracts.Where(c => c.StatusId == 2);
        
        if (softwareId.HasValue)
        {
            query = query.Where(c => c.SoftwareId == softwareId.Value);
        }

        var revenuePln = await query.SumAsync(c => c.Price, cancellationToken);

        return await ConvertFromPlnAsync(revenuePln, currency, cancellationToken);
    }

    public async Task<decimal> CalculatePredictedRevenueAsync(int? softwareId, string? currency, CancellationToken cancellationToken)
    {
        var query = dbContext.Contracts.Where(c => c.StatusId == 1 || c.StatusId == 2);
        
        if (softwareId.HasValue)
        {
            query = query.Where(c => c.SoftwareId == softwareId.Value);
        }

        var revenuePln = await query.SumAsync(c => c.Price, cancellationToken);

        return await ConvertFromPlnAsync(revenuePln, currency, cancellationToken);
    }

    private async Task<decimal> ConvertFromPlnAsync(decimal amountPln, string? targetCurrency, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(targetCurrency) || targetCurrency.Equals("PLN", StringComparison.OrdinalIgnoreCase))
        {
            return Math.Round(amountPln, 2);
        }

        try
        {
            var url = $"https://api.nbp.pl/api/exchangerates/rates/a/{targetCurrency}/?format=json";
            var response = await httpClient.GetFromJsonAsync<NbpResponse>(url, cancellationToken);
            
            if (response != null && response.Rates.Count != 0)
            {
                decimal exchangeRate = response.Rates.First().Mid;
                return Math.Round(amountPln / exchangeRate, 2);
            }
        }
        catch
        {
            throw new Exception($"Nie udało się pobrać aktualnego kursu dla waluty: {targetCurrency}. Sprawdź poprawność kodu waluty (np. USD, EUR, GBP).");
        }

        return Math.Round(amountPln, 2);
    }

    private class NbpResponse
    {
        public List<NbpRate> Rates { get; set; } = new();
    }
    private class NbpRate
    {
        public decimal Mid { get; set; }
    }
}