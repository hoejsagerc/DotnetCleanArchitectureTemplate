using ErrorOr;

namespace Pokemon.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class Pokemon
    {
        public static Error InvalidPokemonId => Error.Validation(
            code: "Pokemon.Invalid",
            description: "Pokemon Id is invalid");

        public static Error NotFound => Error.NotFound(
            code: "Pokemon.NotFound",
            description: "Pokemon not found");
    }
}