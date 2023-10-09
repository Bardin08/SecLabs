using ErrorOr;
using Microsoft.Extensions.Options;
using Refit;
using SecurityLabs.Configurations;
using SecurityLabs.Contracts.Api;
using SecurityLabs.Contracts.Api.External;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Services;

internal sealed class UsersService : IUsersService
{
    private readonly IApplicationTokenService _applicationTokenService;
    private readonly AppCredentialsConfiguration _appCredentialsConfiguration;

    public UsersService(
        IApplicationTokenService applicationTokenService,
        IOptions<AppCredentialsConfiguration> appCredentialsConfiguration)
    {
        _applicationTokenService = applicationTokenService;
        _appCredentialsConfiguration = appCredentialsConfiguration.Value;
    }

    public async Task<ErrorOr<UserInfoResponse>> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        var auth0Request = Auth0CreateUserRequest.FromCreateUserRequest(
            request,
            _appCredentialsConfiguration.ClientInfo.ClientId,
            _appCredentialsConfiguration.ClientInfo.ClientSecret,
            _appCredentialsConfiguration.ClientInfo.Audience
        );

        var accessToken = (await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken;
        var userInfo = await restClient.CreateUserAsync(
            request,
            new Dictionary<string, string>
            {
                {
                    "Authorization",
                    $"Bearer {accessToken}"
                }
            },
            cancellationToken);

        if (!userInfo.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.CreateUser", userInfo.Error.Content!);
        }
        return userInfo.Content;
    }

    public async Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetUserTokenAsync(
        GetUserTokenRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        request.ClientId = _appCredentialsConfiguration.ClientInfo.ClientId;
        request.ClientSecret = _appCredentialsConfiguration.ClientInfo.ClientSecret;
        request.Audience = _appCredentialsConfiguration.ClientInfo.Audience;
        
        var userToken = await restClient.GetUserTokenAsync(
            request,
            new Dictionary<string, string>
            {
                { "Authorization", (await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken }
            },
            cancellationToken);

        if (!userToken.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.GetUserToken", userToken.Error.Content!);
        }

        return userToken.Content;
    }
}