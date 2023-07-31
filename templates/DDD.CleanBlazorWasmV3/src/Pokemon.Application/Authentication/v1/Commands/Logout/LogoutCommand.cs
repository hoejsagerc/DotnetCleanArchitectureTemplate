using ErrorOr;
using MediatR;
using Pokemon.Application.Authentication.v1.Common;

namespace Pokemon.Application.Authentication.v1.Commands.Logout;

public record LogoutCommand(string UserId)
    : IRequest<ErrorOr<string>>;