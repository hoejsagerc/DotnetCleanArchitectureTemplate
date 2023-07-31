using ErrorOr;

namespace Pokemon.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class UserRoleClaim
    {
        public static Error InvalidUserRoleClaimId => Error.Validation(
            code: "UserRoleClaimId.Invalid",
            description: "UserRoleClaim Id is invalid.");

        public static Error InvalidUserRoleClaim => Error.Validation(
            code: "UserRoleClaim.Invalid",
            description: "UserRoleClaim is invalid");

        public static Error NotFound => Error.NotFound(
            code: "UserRoleClaim.NotFound",
            description: "UserRoleClaim not found.");
    }
}