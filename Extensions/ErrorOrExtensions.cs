using ErrorOr;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Extensions;

public static class ErrorOrExtensions
{
    internal static Response ToResponseWithoutBody<TValue>(this ErrorOr<TValue> errorOr)
    {
        return errorOr.IsError
            ? Response.Fail(errorOr.Errors.ToDictionary(p => p.Code, e => (object)e.Description))
            : Response.Success();
    }

    internal static Response<TValue> ToResponse<TValue>(this ErrorOr<TValue> errorOr)
    {
        return errorOr.IsError
            ? Response<TValue>.Fail(errorOr.Value, 
                errorOr.Errors.ToDictionary(p => p.Code, e => (object)e.Description))
            : Response<TValue>.Success(errorOr.Value);
    }
}