using ErrorOr;
namespace Pokemon.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Auth.InvalidCredential",
            description: "Invalid Credentials");
    }
}