using AxiomSysBL.IServices.ISalesService;
using AxiomSysBL.Services.SalesService;

namespace AxiomSysSL.Extensions;

public static class SalesModuleServiceCollectionExtensions
{
    public static IServiceCollection AddSalesModule(this IServiceCollection services)
    {
        services.AddScoped<ISaDeliveryTermService, SaDeliveryTermService>();
        return services;
    }
}
