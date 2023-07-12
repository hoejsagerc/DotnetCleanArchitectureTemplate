using ErrorOr;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.Common.Models.Identities;

namespace Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

public sealed class AbilityId : EntityId<Guid>
{
    public AbilityId(Guid value) : base(value) 
    { 

    }

    public static AbilityId CreateUnique()
    {
        return new AbilityId(Guid.NewGuid());
    }

    public static AbilityId Create(Guid value)
    {
        return new AbilityId(value);
    }

    public static ErrorOr<AbilityId> Create(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            return Errors.Pokemon.InvalidPokemonId;
        }

        return new AbilityId(guid);
    }
}