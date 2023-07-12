namespace Pokemon.Contracts.v1.Authentication;

/// <summary>
/// Contract for refreshing a users jwt token
/// </summary>
/// <param name="RefreshToken">¨
/// <summary>The refresh token issued to the user</summary>
/// <example>kdKvRyC6Dv6uWpnL6UKcPlfVFeATPt+CKZK7HEdHXLs/S7had8DGYlqAG6+8tvbwjKctct7vHJuW9989rE01zg==</example>
/// </param>
public record RefreshTokenRequest
(
    string RefreshToken
);