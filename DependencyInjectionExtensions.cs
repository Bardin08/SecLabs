using FastEndpoints;
using SecurityLabs.Configurations;
using SecurityLabs.Services;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs;

internal static class DependencyInjectionExtensions
{
    internal static IServiceCollection AddPresentationLayerServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddFastEndpoints();
        builder.Services.AddSingleton<IApplicationTokenService, ApplicationTokenService>();

        return builder.Services
            .AddConfigurations(builder.Configuration);
    }

    private static IServiceCollection AddConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AppCredentialsConfiguration>(
            configuration.GetSection(AppCredentialsConfiguration.SectionName));
        services.Configure<ClientCredentialsConfiguration>(
            configuration.GetSection(ClientCredentialsConfiguration.SectionName));

        return services;
    }
}
