using ErrorOr;

namespace Pokemon.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error InvalidUserId => Error.Validation(
            code: "User.Invalid",
            description: "User Id is invalid.");

        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found.");
        
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "A user with this email already exists.");
    }
}