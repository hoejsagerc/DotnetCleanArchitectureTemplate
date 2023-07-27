using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace Pokemon.Contracts.v1.Authentication;

/// <summary>
/// Contract for authentication response
/// </summary>
public class AuthenticationResponse
{
    /// <summary>
    /// The Users id
    /// </summary>
    /// <example>2dc52914-035b-4553-b41e-e12cd3b3cf15</example>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }


    /// <summary>
    /// The Users username
    /// </summary>
    /// <example>MyUsername</example>
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;


    /// <summary>
    /// The Users firstname
    /// </summary>
    /// <example>John</example>
    [JsonPropertyName("firstname")]
    public string Firstname {get; set; } = string.Empty;


    /// <summary>
    /// The Users lastname
    /// </summary>
    /// <example>Doe</example>
    [JsonPropertyName("lastname")]
    public string Lastname { get; set; } = string.Empty;


    /// <summary>
    /// The Users email
    /// </summary>
    /// <example>j.doe@mail.com</example>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;


    /// <summary>
    /// Jwt token provided to the user
    /// </summary>
    /// <example>Standard JWT token</example>
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;


    /// <summary>
    /// Refresh token provided to the user for refresihing JWT token
    /// </summary>
    /// <example>Base64 encoded string</example>
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}
