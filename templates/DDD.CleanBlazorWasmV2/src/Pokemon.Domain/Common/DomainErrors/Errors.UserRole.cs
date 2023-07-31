using ErrorOr;

namespace Pokemon.Domain.Common.DomainErrors;


public static partial class Errors
{
    public static class UserRole
    {
        public static Error InvalidUserRoleId => Error.Validation(
            code: "UserRoleId.Invalid",
            description: "UserRole Id is invalid.");

        public static Error InvalidUserRole => Error.Validation(
            code: "UserRole.Invalid",
            description: "UserRole is invalid");

        public static Error NotFound => Error.NotFound(
            code: "UserRole.NotFound",
            description: "UserRole not found.");
    }
}