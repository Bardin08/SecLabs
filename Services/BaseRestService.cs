using ErrorOr;
using Refit;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Services;

internal abstract class BaseRestService
{
    private readonly IApplicationTokenService _applicationTokenService;

    protected BaseRestService(IApplicationTokenService applicationTokenService)
    {
        _applicationTokenService = applicationTokenService;
    }

    protected async Task<ErrorOr<TResponse>> DoApiRequestInternalAsync<TModel, TRequest, TResponse>(
        TModel internalRequest,
        Func<TModel, TRequest> requestMapper,
        Func<TRequest, IDictionary<string, string>, CancellationToken, Task<ApiResponse<TResponse>>> apiDelegate,
        CancellationToken ct)
    {
        var request = requestMapper(internalRequest);

        var response = await apiDelegate(
            request, await GetDefaultHeadersAsync(), ct);

        if (!response.IsSuccessStatusCode)
        {
            return Error.Unexpected(apiDelegate.Method.Name, response.Error.Content!);
        }

        return response.Content;
    }

    protected async ValueTask<IDictionary<string, string>> GetDefaultHeadersAsync()
    {
        return new Dictionary<string, string>
        {
            {
                "Authorization",
                $"Bearer {(await _applicationTokenService.GetApplicationCredentialsAsync())!.AccessToken}"
            }
        };
    }
}