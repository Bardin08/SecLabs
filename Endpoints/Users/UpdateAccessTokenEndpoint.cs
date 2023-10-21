using FastEndpoints;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Endpoints.Users;

internal class UpdateAccessTokenEndpoint
    : Endpoint<RefreshAccessTokenRequest, Response<AuthInfoWithRefreshTokenResponse>>
{
    private readonly IAccessTokenService _accessTokenService;

    public UpdateAccessTokenEndpoint(IAccessTokenService accessTokenService)
    {
        _accessTokenService = accessTokenService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/api/users/token/refresh");
    }

    public override async Task HandleAsync(RefreshAccessTokenRequest req, CancellationToken ct)
    {
        var userToken = await _accessTokenService.RefreshUserTokenAsync(req, ct);
        await SendAsync(userToken.ToResponse(), cancellation: ct);
    }
}