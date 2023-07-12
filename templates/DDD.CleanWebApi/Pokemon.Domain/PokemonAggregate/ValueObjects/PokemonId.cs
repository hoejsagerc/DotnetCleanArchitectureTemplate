using ErrorOr;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.Common.Models.Identities;

namespace Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

public sealed class PokemonId : AggregateRootId<Guid>
{
    private PokemonId(Guid value) : base(value)
    {
    }

    public static PokemonId CreateUnique()
    {
        return new PokemonId(Guid.NewGuid());
    }

    public static PokemonId Create(Guid value)
    {
        return new PokemonId(value);
    }

    public static ErrorOr<PokemonId> Create(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            return Errors.Pokemon.InvalidPokemonId;
        }

        return new PokemonId(guid);
    }
}