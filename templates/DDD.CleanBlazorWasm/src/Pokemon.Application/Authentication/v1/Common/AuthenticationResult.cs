using Pokemon.Domain.AuthenticationAggregates;

namespace Pokemon.Application.Authentication.v1.Common;

public record AuthenticationResult
(
    User User,
    string Token,
    string RefreshToken
);