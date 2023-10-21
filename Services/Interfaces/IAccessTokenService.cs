using ErrorOr;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Services.Interfaces;

internal interface IAccessTokenService
{
    Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> GetUserTokenAsync(
        GetUserTokenRequest request, CancellationToken cancellationToken);

    Task<ErrorOr<AuthInfoWithRefreshTokenResponse>> RefreshUserTokenAsync(
        RefreshAccessTokenRequest request, CancellationToken cancellationToken);
}