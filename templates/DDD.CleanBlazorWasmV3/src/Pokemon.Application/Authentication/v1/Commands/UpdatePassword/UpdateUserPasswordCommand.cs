using ErrorOr;
using MediatR;
using Pokemon.Application.Authentication.v1.Common;

namespace Pokemon.Application.Authentication.v1.Commands.UpdatePassword;

public record UpdateUserPasswordCommand
(
    string Id,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;