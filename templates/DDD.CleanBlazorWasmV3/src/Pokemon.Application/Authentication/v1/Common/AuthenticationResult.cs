using Pokemon.Domain.AuthenticationAggregate;

namespace Pokemon.Application.Authentication.v1.Common;

public record AuthenticationResult
(
    ApplicationUser User,
    string AccessToken
);