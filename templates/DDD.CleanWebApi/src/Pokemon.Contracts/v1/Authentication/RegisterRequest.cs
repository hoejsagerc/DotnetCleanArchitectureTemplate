namespace Pokemon.Contracts.v1.Authentication;

public record RegisterRequest
(
    string Username,
    string Firstname,
    string Lastname,
    string Email,
    string Password
);