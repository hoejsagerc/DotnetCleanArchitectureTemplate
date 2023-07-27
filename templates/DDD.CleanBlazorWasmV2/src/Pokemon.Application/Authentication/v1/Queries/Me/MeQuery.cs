using ErrorOr;
using MediatR;
using Pokemon.Application.Authentication.v1.Common;

namespace Pokemon.Application.Authentication.v1.Queries.Me;

public record MeQuery
(
    string Email,
    string GivenName,
    string Id
) : IRequest<ErrorOr<AuthenticationResult>>;