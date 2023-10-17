using SecurityLabs.Contracts.Api;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Services.Interfaces;

/// <summary>
/// Marker interface for DI
/// </summary>
internal interface IApplicationTokenService
{
    ValueTask<AuthInfoResponse?> GetApplicationCredentialsAsync();
}