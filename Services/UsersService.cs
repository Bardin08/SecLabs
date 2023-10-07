using ErrorOr;
using Refit;
using SecurityLabs.Contracts.Api;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Services;

internal sealed class UsersService : IUsersService
{
    private readonly IApplicationTokenService _applicationTokenService;

    public UsersService(IApplicationTokenService applicationTokenService)
    {
        _applicationTokenService = applicationTokenService;
    }

    public async Task<ErrorOr<UserInfoResponse>> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken)
    {
        var restClient = RestService.For<IUsersApi>("");

        var userInfo = await restClient.CreateUserAsync(
            request,
            new Dictionary<string, string>
            {
                { "Authorization", (await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken }
            },
            cancellationToken);

        if (!userInfo.IsSuccessStatusCode)
        {
            return Error.Unexpected("Users.CreateUser", userInfo.Error.Content!);
        }

        return userInfo.Content;
    }
}