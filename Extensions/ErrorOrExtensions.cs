using ErrorOr;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Extensions;

public static class ErrorOrExtensions
{
    public static Response<TValue> ToResponse<TValue>(this ErrorOr<TValue> errorOr)
    {
        return errorOr.IsError
            ? Response<TValue>.Fail(errorOr.Value,
                errorOr.Errors.ToDictionary(p => p.Code, e => (object)e.Description))
            : Response<TValue>.Success(errorOr.Value);
    }
}