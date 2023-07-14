using ErrorOr;
using MediatR;
using Pokemon.Application.Authentication.v1.Common;

namespace Pokemon.Application.Authentication.v1.Commands.Refresh;

public record RefreshCommand
(
    string UserId,
    string RefreshToken
) : IRequest<ErrorOr<AuthenticationResult>>;
