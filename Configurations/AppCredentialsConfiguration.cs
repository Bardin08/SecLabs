namespace SecurityLabs.Configurations;

internal record AppCredentialsConfiguration
{
    public static string SectionName
        => nameof(AppCredentialsConfiguration).Replace("Configuration", "");

    public int ReUpdateMinutes { get; init; }
    public required ClientCredentialsConfiguration ClientInfo { get; set; }
}