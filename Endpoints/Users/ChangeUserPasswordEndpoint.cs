using FastEndpoints;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Endpoints.Users;

internal class ChangeUserPasswordEndpoint
    : Endpoint<ChangeUserPasswordRequest, Response>
{
    private readonly IUsersService _usersService;

    public ChangeUserPasswordEndpoint(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("/api/users/{userId}/password/change");
    }

    public override async Task HandleAsync(ChangeUserPasswordRequest req, CancellationToken ct)
    {
        var response = await _usersService.ChangePasswordAsync(req, ct);
        await SendAsync(response.ToResponseWithoutBody(), cancellation: ct);
    }
}