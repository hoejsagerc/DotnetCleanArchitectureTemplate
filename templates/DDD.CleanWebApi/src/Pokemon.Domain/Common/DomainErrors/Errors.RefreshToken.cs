using ErrorOr;

namespace Pokemon.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class RefreshToken
    {
        public static Error InvalidRefreshTokenId => Error.Validation(
            code: "RefreshTokenId.Invalid",
            description: "RefreshToken Id is invalid.");

        public static Error InvalidRefreshToken => Error.Validation(
            code: "RefreshToken.Invalid",
            description: "RefreshToken is invalid");

        public static Error NotFound => Error.NotFound(
            code: "RefreshToken.NotFound",
            description: "RefreshToken not found.");
    }
}