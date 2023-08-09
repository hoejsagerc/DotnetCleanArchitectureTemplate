using ErrorOr;
using MediatR;
using Pokemon.Application.Authentication.v1.Common;

namespace Pokemon.Application.Authentication.v1.Commands.UpdateUser;

public record UpdateUserEmailCommand
(
    string Id,
    string Email
) : IRequest<ErrorOr<AuthenticationResult>>;