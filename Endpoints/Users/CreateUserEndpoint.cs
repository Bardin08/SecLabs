using FastEndpoints;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Extensions;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Endpoints.Users;

internal sealed class CreateUserEndpoint
    : Endpoint<CreateUserRequest, Response<UserInfoResponse>>
{
    private readonly IUsersService _usersService;

    public CreateUserEndpoint(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/api/users/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var userInfoResult = await _usersService.CreateUserAsync(req, ct);
        await SendAsync(userInfoResult.ToResponse(), cancellation: ct);
    }
}