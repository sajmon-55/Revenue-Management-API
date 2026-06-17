using Application.Authentication;
using Application.Clients;
using Application.Contracts;
using Application.Payments;
using Application.Revenue;
using Application.Softwares;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ISoftwareService , SoftwareService>();
        services.AddHttpClient<IRevenueService, RevenueService>();
        
        return services;
    }
}