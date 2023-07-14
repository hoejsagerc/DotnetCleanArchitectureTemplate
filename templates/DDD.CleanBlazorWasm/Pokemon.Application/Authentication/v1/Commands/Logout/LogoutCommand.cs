using ErrorOr;
using MediatR;
using Pokemon.Domain.AuthenticationAggregates;

namespace Pokemon.Application.Authentication.v1.Commands.Logout;

public record LogoutCommand(string UserId)
    : IRequest<ErrorOr<RefreshToken>>;