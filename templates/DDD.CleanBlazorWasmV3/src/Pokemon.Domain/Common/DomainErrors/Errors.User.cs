using ErrorOr;

namespace Pokemon.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User was not found");

        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "A user with this email already exists");

        public static Error FailedCreating => Error.Unexpected(
            code: "User.FailedCreating",
            description: "Unexpected error while creating new user in database");

        public static Error PasswordValidationFailed => Error.Validation(
            code: "User.PasswordValidationFailed",
            description: "Password validation failed");

        public static Error InvalidCredentials => Error.Validation(
            code: "User.InvalidCredential",
            description: "Invalid Credential");
    }
}