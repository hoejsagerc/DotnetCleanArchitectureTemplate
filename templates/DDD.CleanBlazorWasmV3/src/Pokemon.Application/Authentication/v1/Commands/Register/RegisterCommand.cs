using ErrorOr;
using MediatR;
using Pokemon.Application.Authentication.v1.Common;

namespace Pokemon.Application.Authentication.v1.Commands.Register;

public record RegisterCommand
(
    string Username,
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;