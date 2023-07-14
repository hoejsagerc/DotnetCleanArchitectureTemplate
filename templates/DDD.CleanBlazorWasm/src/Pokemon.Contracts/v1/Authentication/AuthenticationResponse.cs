namespace Pokemon.Contracts.v1.Authentication;

public record AuthenticationResponse
(
    Guid Id,
    string Username,
    string Firstname,
    string Lastname,
    string Email,
    string Token,
    string RefreshToken
);