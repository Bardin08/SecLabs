using FastEndpoints;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Proxies.Interfaces;

namespace SecurityLabs.Endpoints.Users;

internal class GetUserTokenEndpoint : Endpoint<GetUserTokenRequest, Response<AuthInfoWithRefreshTokenResponse>>
{
    private readonly IAccessTokenServiceProxy _accessTokenService;

    public GetUserTokenEndpoint(
        IAccessTokenServiceProxy accessTokenService)
    {
        _accessTokenService = accessTokenService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/api/users/token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserTokenRequest req, CancellationToken ct)
    {
        var userToken = await _accessTokenService.GetUserTokenAsync(req, ct);
        await SendAsync(userToken.ToResponse(), cancellation: ct);
    }
}