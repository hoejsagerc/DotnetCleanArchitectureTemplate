using System.Text.Json.Serialization;

namespace Pokemon.Contracts.v1.Authentication;


/// <summary>
/// Response when either registering a user or performin login
/// </summary>
/// <param>UserName</param>
/// <param>Email</param>
/// <param>AccessToken</param>
public class AuthenticationResponse
{
    /// <summary>
    /// The users UserName
    /// </summary>
    /// <example>user@example.com</example>
    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    /// <summary>
    /// The users Email
    /// </summary>
    /// <example>user@example.com</example>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// The users Given name
    /// </summary>
    /// <example>John</example>
    [JsonPropertyName("givenName")]
    public string? GivenName { get; set; }

    /// <summary>
    /// The users Id
    /// </summary>
    /// <example>string</example>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Authorized Jwt Access Token
    /// </summary>
    /// <example>string</example>
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }
}