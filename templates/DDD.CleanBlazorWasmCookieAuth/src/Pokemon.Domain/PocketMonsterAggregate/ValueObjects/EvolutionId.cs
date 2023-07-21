using ErrorOr;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.Common.Models.Identities;

namespace Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

public sealed class EvolutionId : EntityId<Guid>
{
    public EvolutionId(Guid value) : base(value) 
    { 

    }

    public static EvolutionId CreateUnique()
    {
        return new EvolutionId(Guid.NewGuid());
    }

    public static EvolutionId Create(Guid value)
    {
        return new EvolutionId(value);
    }

    public static ErrorOr<EvolutionId> Create(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            return Errors.PocketMonster.InvalidPokemonId;
        }

        return new EvolutionId(guid);
    }
}