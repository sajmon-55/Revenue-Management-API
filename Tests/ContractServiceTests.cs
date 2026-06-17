using Application.Contracts;
using Application.Contracts.DTOs;
using Domain.Entities.Clients;
using Domain.Entities.Contracts;
using Domain.Entities.Softwares;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Application.Tests;

public class ContractServiceTests
{
    // pusta baza w RAM
    private DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var configuration = new ConfigurationBuilder().Build();

        return new DatabaseContext(options, configuration);
    }

    [Fact]
    public async Task CreateContractAsync_ForReturningClient_ShouldApply5PercentDiscount()
    {
        // ==========================================
        // 1. ARRANGE (Przygotowanie środowiska i danych)
        // ==========================================
        var dbContext = GetInMemoryDbContext();
        var service = new ContractService(dbContext);

        // add software
        var software = new Software { Id = 1, Name = "System ERP", CurrentVersion = "1.0", Category = "Finanse" };
        dbContext.Softwares.Add(software);

        // add client
        var client = new Client { Id = 1, Email = "test@test.pl", Phone = "123", Address = "Warszawa" };
        dbContext.Clients.Add(client);

        var oldContract = new Contract 
        { 
            Id = 99, ClientId = 1, SoftwareId = 1, Price = 5000, 
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-100)),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-70)),
            StatusId = 4
        };
        dbContext.Contracts.Add(oldContract);
        await dbContext.SaveChangesAsync();

        // Przygotowujemy zapytanie o NOWĄ umowę.
        // Cena bazowa to 10 000 PLN + 1 rok wsparcia (1000 PLN) = 11 000 PLN.
        var request = new CreateContractRequest(
            ClientId: 1,
            SoftwareId: 1,
            SoftwareVersion: "1.0",
            StartDate: DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
            Price: 10000m,
            YearsOfSupport: 1
        );

        // ==========================================
        // 2. ACT (Właściwe wykonanie logiki biznesowej)
        // ==========================================
        var newContractId = await service.CreateContractAsync(request, CancellationToken.None);

        // ==========================================
        // 3. ASSERT (Sprawdzenie, czy wynik jest poprawny)
        // ==========================================
        var savedContract = await dbContext.Contracts.FindAsync(newContractId);
        
        // Cena bazowa = 11000 PLN. Discount 5% z 11000 = 550 PLN. 
        // Oczekiwana cena końcowa = 10450 PLN.
        Assert.NotNull(savedContract);
        Assert.Equal(10450m, savedContract.Price); 
    }
}