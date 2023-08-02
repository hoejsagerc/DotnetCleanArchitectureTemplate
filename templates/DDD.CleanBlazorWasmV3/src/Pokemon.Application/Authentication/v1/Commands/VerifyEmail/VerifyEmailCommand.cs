using ErrorOr;
using MediatR;
using Pokemon.Application.Authentication.v1.Common;

namespace Pokemon.Application.Authentication.v1.Commands.VerifyEmail;

public record VerifyEmailCommand
(
    string UserId,
    string Code
) : IRequest<ErrorOr<AuthenticationResult>>;