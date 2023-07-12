namespace Pokemon.Contracts.v1.Authentication;

public record LoginRequest
(
    string Email,
    string Password
);