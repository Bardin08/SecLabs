using FastEndpoints;
using SecurityLabs.Configurations;
using SecurityLabs.Services;
using SecurityLabs.Services.Interfaces;
using SecurityLabs.Services.Proxies;
using SecurityLabs.Services.Proxies.Interfaces;

namespace SecurityLabs;

internal static class DependencyInjectionExtensions
{
    internal static IServiceCollection AddPresentationLayerServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddFastEndpoints();

        builder.Services.AddSingleton<IApplicationTokenService, ApplicationTokenService>();
        builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
        builder.Services.AddScoped<IUsersService, UsersService>();

        builder.Services.AddScoped<IAccessTokenServiceProxy, AccessTokenServiceProxy>();

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
        services.Configure<JwtTokenConfiguration>(
            configuration.GetSection(JwtTokenConfiguration.SectionName));

        return services;
    }
}