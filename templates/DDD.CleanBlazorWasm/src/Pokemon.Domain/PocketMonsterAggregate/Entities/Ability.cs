using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

namespace Pokemon.Domain.Common.PokemonAggregate.Entities;

public sealed class Ability : Entity<AbilityId>
{
    public string Name { get; private set; }

    private Ability(string name, AbilityId? id = null) : base(id ?? AbilityId.CreateUnique())
    {
        Name = name;
    }

    public static Ability Create(string name)
    {
        return new Ability(name);
    }
}