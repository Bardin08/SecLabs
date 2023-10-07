using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SecurityLabs.Contracts.Api.Models;

public record CreateUserRequest
{
    [JsonProperty("email")]
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonProperty("phone_number")]
    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; init; }

    [JsonProperty("user_metadata")]
    [JsonPropertyName("user_metadata")]
    public UserMetadata? UserMetadata { get; init; }

    [JsonProperty("blocked")]
    [JsonPropertyName("blocked")]
    public bool Blocked { get; init; }

    [JsonProperty("email_verified")]
    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; init; }

    [JsonProperty("phone_verified")]
    [JsonPropertyName("phone_verified")]
    public bool PhoneVerified { get; init; }

    [JsonProperty("app_metadata")]
    [JsonPropertyName("app_metadata")]
    public AppMetadata? AppMetadata { get; init; }

    [JsonProperty("given_name")]
    [JsonPropertyName("given_name")]
    public string? GivenName { get; init; }

    [JsonProperty("family_name")]
    [JsonPropertyName("family_name")]
    public string? FamilyName { get; init; }

    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonProperty("nickname")]
    [JsonPropertyName("nickname")]
    public string? Nickname { get; init; }

    [JsonProperty("picture")]
    [JsonPropertyName("picture")]
    public string? Picture { get; init; }

    [JsonProperty("user_id")]
    [JsonPropertyName("user_id")]
    public string? UserId { get; init; }

    [JsonProperty("connection")]
    [JsonPropertyName("connection")]
    public string? Connection { get; init; }

    [JsonProperty("password")]
    [JsonPropertyName("password")]
    public string? Password { get; init; }

    [JsonProperty("verify_email")]
    [JsonPropertyName("verify_email")]
    public bool VerifyEmail { get; init; }

    [JsonProperty("username")]
    [JsonPropertyName("username")]
    public string? Username { get; init; }
}

public record AppMetadata
{
}

public record UserMetadata
{
}