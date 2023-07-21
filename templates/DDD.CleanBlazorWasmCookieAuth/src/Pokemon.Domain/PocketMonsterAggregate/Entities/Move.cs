using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;

namespace Pokemon.Domain.Common.PokemonAggregate.Entities;

public sealed class Move : Entity<MoveId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public float Damage { get; private set; }
    public float CriticalHitFactor { get; private set; }
    public float CriticalHitChance { get; private set; }


    private Move(
        string name,
        string description,
        float damage,
        float criticalHitFactor,
        float criticalHitChance,
        MoveId? id = null) : base(id ?? MoveId.CreateUnique())
    {
        Name = name;
        Description = description;
        Damage = damage;
        CriticalHitFactor = criticalHitFactor;
        CriticalHitChance = criticalHitChance;
    }

    public static Move Create(
        string name,
        string description,
        float damage,
        float criticalHitFactor,
        float criticalHitChance)
    {
        return new Move(
            name,
            description,
            damage,
            criticalHitFactor,
            criticalHitChance);
    }
}