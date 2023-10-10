using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SecurityLabs.Contracts.Api.Models;

namespace SecurityLabs.Contracts.Api.External;

public class Auth0ChangeUserPasswordRequest
{
    [JsonProperty("password")]
    [JsonPropertyName("password")]
    public required string Password { get; set; }

    [JsonProperty("connection")]
    [JsonPropertyName("connection")]
    public string Connection => "Username-Password-Authentication";

    internal static Auth0ChangeUserPasswordRequest FromChangeUserPasswordRequest(
        ChangeUserPasswordRequest req)
    {
        return new Auth0ChangeUserPasswordRequest
        {
            Password = req.Password
        };
    }
}