using ErrorOr;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Services.Interfaces;

internal interface IUsersService
{
    Task<ErrorOr<UserInfoResponse>> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken);

    Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetUserTokenAsync(
        GetUserTokenRequest request, CancellationToken cancellationToken);
}