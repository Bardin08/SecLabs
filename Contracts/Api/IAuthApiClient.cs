using Refit;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Contracts.Api;

internal interface IAuthApiClient
{
    [Post("/oauth/token")]
    Task<ApiResponse<AuthInfoResponse>> GetAuthTokenAsync(
        [Body(BodySerializationMethod.UrlEncoded)] AuthInfoRequest request);
}