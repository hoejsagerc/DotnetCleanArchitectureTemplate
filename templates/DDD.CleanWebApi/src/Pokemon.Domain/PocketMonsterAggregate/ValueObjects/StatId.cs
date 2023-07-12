using ErrorOr;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.Common.Models.Identities;

namespace Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

public sealed class StatId : EntityId<Guid>
{
    public StatId(Guid value) : base(value) 
    { 

    }

    public static StatId CreateUnique()
    {
        return new StatId(Guid.NewGuid());
    }

    public static StatId Create(Guid value)
    {
        return new StatId(value);
    }

    public static ErrorOr<StatId> Create(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            return Errors.PocketMonster.InvalidPokemonId;
        }

        return new StatId(guid);
    }
}