namespace SecurityLabs.Configurations;

internal record JwtTokenConfiguration
{
    public static string SectionName
        => nameof(JwtTokenConfiguration).Replace("Configuration", "");

    public int RefreshBeforeEndInMinutes { get; init; }
    public IEnumerable<string> SkipRoutes { get; init; } = Enumerable.Empty<string>();
}