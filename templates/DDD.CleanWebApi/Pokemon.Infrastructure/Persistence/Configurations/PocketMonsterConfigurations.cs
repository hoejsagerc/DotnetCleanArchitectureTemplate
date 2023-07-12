using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;
using Pokemon.Domain.PokemonAggregate;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Infrastructure.Persistence.Configurations;

public class PocketMonsterConfigurations : IEntityTypeConfiguration<PocketMonster>
{
    public void Configure(EntityTypeBuilder<PocketMonster> builder)
    {
        ConfigurePokemonsTable(builder);
        ConfigureMovesTable(builder);
        ConfigureAbilitiesTable(builder);
        ConfigureEvolutionsTable(builder);
        ConfigureStatsTable(builder);
    }

    private static void ConfigureMovesTable(EntityTypeBuilder<PocketMonster> builder)
    {
        builder.OwnsMany(m => m.Moves, mb =>
        {
            mb.ToTable("PokemonMoves");

            mb.WithOwner().HasForeignKey("PokemonId");

            mb.HasKey("Id", "PokemonId");

            mb.Property(t => t.Id)
                .HasColumnName("MoveId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => MoveId.Create(value));
        });

        builder.Metadata.FindNavigation(nameof(PocketMonster.Moves))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureAbilitiesTable(EntityTypeBuilder<PocketMonster> builder)
    {
        builder.OwnsMany(a => a.Abilities, ab =>
        {
            ab.ToTable("PokemonAbilities");

            ab.WithOwner().HasForeignKey("PokemonId");

            ab.HasKey("Id", "PokemonId");

            ab.Property(t => t.Id)
                .HasColumnName("AbilityId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => AbilityId.Create(value));
        });

        builder.Metadata.FindNavigation(nameof(PocketMonster.Abilities))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureEvolutionsTable(EntityTypeBuilder<PocketMonster> builder)
    {
        builder.OwnsMany(e => e.Evolutions, eb =>
        {
            eb.ToTable("PokemonEvolutions");

            eb.WithOwner().HasForeignKey("PokemonId");

            eb.HasKey("Id", "PokemonId");

            eb.Property(t => t.Id)
                .HasColumnName("EvolutionId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => EvolutionId.Create(value));
        });

        builder.Metadata.FindNavigation(nameof(PocketMonster.Evolutions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureStatsTable(EntityTypeBuilder<PocketMonster> builder)
    {
        builder.OwnsMany(s => s.Stats, sb =>
        {
            sb.ToTable("PokemonStats");

            sb.WithOwner().HasForeignKey("PokemonId");

            sb.HasKey("Id", "PokemonId");

            sb.Property(t => t.Id)
                .HasColumnName("StatId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => StatId.Create(value));
        });

        builder.Metadata.FindNavigation(nameof(PocketMonster.Stats))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigurePokemonsTable(EntityTypeBuilder<PocketMonster> builder)
    {
        builder.ToTable("Pokemons");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => PokemonId.Create(value));

        builder.Property(p => p.UserId!)
            .HasColumnName("UserId")
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));
    }
}
