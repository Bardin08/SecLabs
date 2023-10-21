namespace SecurityLabs.Contracts;

internal record UserCacheModel
{
    public string RefreshToken { get; set; } = null!;
}