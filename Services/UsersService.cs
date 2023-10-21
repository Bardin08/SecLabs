using ErrorOr;
using Microsoft.Extensions.Options;
using Refit;
using SecurityLabs.Configurations;
using SecurityLabs.Contracts.Api;
using SecurityLabs.Contracts.Api.External;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Interfaces;
using SecurityLabs.Services.Proxies;
using SecurityLabs.Services.Proxies.Interfaces;

namespace SecurityLabs.Services;

internal sealed class UsersService : BaseRestService, IUsersService
{
    private readonly IAccessTokenServiceProxy _accessTokenServiceProxy;
    private readonly AppCredentialsConfiguration _appCredentialsConfiguration;

    public UsersService(
        IAccessTokenServiceProxy accessTokenServiceProxy,
        IApplicationTokenService applicationTokenService,
        IOptions<AppCredentialsConfiguration> appCredentialsConfiguration) : base(applicationTokenService)
    {
        _accessTokenServiceProxy = accessTokenServiceProxy;

        ArgumentNullException.ThrowIfNull(appCredentialsConfiguration.Value);
        _appCredentialsConfiguration = appCredentialsConfiguration.Value;
    }

    public async Task<ErrorOr<UserInfoResponse>> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        return await DoApiRequestInternalAsync(
            request,
            Auth0CreateUserRequest.FromCreateUserRequest,
            restClient.CreateUserAsync,
            cancellationToken);
    }

    public async Task<ErrorOr<UserInfoResponse>> ChangePasswordAsync(
        ChangeUserPasswordRequest request, CancellationToken cancellationToken)
    {
        var baseUrl = $"https://{_appCredentialsConfiguration.ClientInfo.Domain}/";
        var restClient = RestService.For<IUsersApi>(baseUrl, RefitSettingsExtension.ProjectDefaultSettings);

        var userToken = await restClient.ChangePasswordAsync(
            request.UserId!,
            Auth0ChangeUserPasswordRequest.FromChangeUserPasswordRequest(request),
            await GetDefaultHeadersAsync(),
            cancellationToken);

        if (!userToken.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.ChangePassword", userToken.Error.Content!);
        }

        return userToken.Content;
    }
}