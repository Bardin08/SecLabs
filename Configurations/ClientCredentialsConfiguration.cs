namespace SecurityLabs.Configurations;

internal record ClientCredentialsConfiguration
{
    public static string SectionName
        => nameof(ClientCredentialsConfiguration).Replace("Configuration", "");

    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string Audience { get; init; }
    public required string Domain { get; init; }
}