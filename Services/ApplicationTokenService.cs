using ErrorOr;
using Microsoft.Extensions.Options;
using Refit;
using SecurityLabs.Configurations;
using SecurityLabs.Contracts.Api;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Services;

internal class ApplicationTokenService : IApplicationTokenService
{
    private AuthInfoResponse? _appCredentials;
    private AppCredentialsConfiguration _clientAuthConfiguration;
    private readonly ILogger<ApplicationTokenService> _logger;
    
    public ApplicationTokenService(
        IOptions<AppCredentialsConfiguration> configurationProvider,
        ILogger<ApplicationTokenService> logger)
    {
        _logger = logger;
        _clientAuthConfiguration = configurationProvider.Value;
        if (_clientAuthConfiguration is null)
        {
            throw new ArgumentNullException(nameof(_clientAuthConfiguration));
        }

        Task.Run(async () =>
        {
            var reUpdateTimeSpan = TimeSpan.FromMinutes(_clientAuthConfiguration.ReUpdateMinutes);
            var periodicTimer = new PeriodicTimer(reUpdateTimeSpan);

            while (await periodicTimer.WaitForNextTickAsync())
            {
                var credentials = await RequestApplicationTokenAsync();
                if (credentials.IsError)
                {
                    continue;
                }

                _appCredentials = credentials.Value;
            }
        });
    }

    private async Task<ErrorOr<AuthInfoResponse>> RequestApplicationTokenAsync()
    {
        var baseUrl = $"https://{_clientAuthConfiguration.ClientInfo.Domain}/";
        var httpClient = RestService.For<IAuthApiClient>(baseUrl);

        var response = await httpClient.GetAuthTokenAsync(
            new AuthInfoRequest
            {
                ClientId = _clientAuthConfiguration.ClientInfo.ClientId,
                ClientSecret = _clientAuthConfiguration.ClientInfo.ClientSecret,
                Audience = _clientAuthConfiguration.ClientInfo.Audience,
                GrantType = "client_credentials"
            });

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical(string.Join("\n", response.Error.Content));
            return Error.Failure("Application.TokenRequestFailed");
        }

        return response.Content;
    }

    public async ValueTask<AuthInfoResponse?> GetApplicationCredentialsAsync()
    {
        if (_appCredentials is not null)
        {
            return _appCredentials;
        }

        var credentials = await RequestApplicationTokenAsync();
        if (!credentials.IsError)
        {
            _appCredentials = credentials.Value;
        }

        return _appCredentials;
    }
}