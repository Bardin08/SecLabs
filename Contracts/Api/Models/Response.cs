using FastEndpoints;

namespace SecurityLabs.Contracts.Api.Models;

public class Response<TBody>
{
    public bool IsSuccessful  { get; set; }
    public TBody? Body { get; set; }
    public Dictionary<string, object>? Errors { get; set; }

    public static Response<TBody> Success(TBody body)
    {
        return new Response<TBody>()
        {
            IsSuccessful = true,
            Errors = null,
            Body = body
        };
    }

    public static Response<TBody> Fail(
        TBody body, Dictionary<string, object> errors)
    {
        return new Response<TBody>()
        {
            IsSuccessful = false,
            Body = body,
            Errors = errors
        };
    }

}