namespace SecurityLabs.Contracts.Api.Models;

internal record Response
{
    public bool IsSuccessful { get; set; }
    public Dictionary<string, object>? Errors { get; set; }

    public static Response Success()
    {
        return new Response
        {
            IsSuccessful = true,
            Errors = null,
        };
    }

    public static Response Fail(
        Dictionary<string, object> errors)
    {
        return new Response
        {
            IsSuccessful = false,
            Errors = errors
        };
    }
}

internal record Response<TBody> : Response
{
    public TBody? Body { get; set; }

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