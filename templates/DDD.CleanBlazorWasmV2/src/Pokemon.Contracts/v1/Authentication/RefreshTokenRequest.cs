using System.Text.Json.Serialization;

namespace Pokemon.Contracts.v1.Authentication;


/// <summary>
/// Contract for refreshing jwt token
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Refresh token provided to the user for refreshing jwt token
    /// </summary>
    /// <example>kdKvRyC6Dv6uWpnL6UKcPlfVFeATPt+CKZK7HEdHXLs/S7had8DGYlqAG6+8tvbwjKctct7vHJuW9989rE01zg==</example>
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}