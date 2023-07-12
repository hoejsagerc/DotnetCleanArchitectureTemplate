using ErrorOr;
using MediatR;
using Pokemon.Domain.PokemonAggregate;

namespace Pokemon.Application.Pokemon.v1.Commands.CreatePokemon;

public record CreatePokemonCommand
(
    string Name,
    string Type,
    int Height,
    int Weight,
    int Level,
    int Experience,
    int ExperienceToNextLevel,
    string Sprite,
    string UserId,
    List<CreateMoveCommand> Moves,
    List<CreateStatCommand> Stats,
    List<CreateAbilityCommand> Abilities,
    List<CreateEvolutionCommand> Evolutions
) : IRequest<ErrorOr<PocketMonster>>;

public record CreateMoveCommand
(
    string Name,
    string Description,
    float Damage,
    float CriticalHitFactor,
    float CriticalHitChance
);

public record CreateStatCommand
(
    string Name,
    int BaseStat,
    int Effort
);

public record CreateAbilityCommand
(
    string Name
);

public record CreateEvolutionCommand
(
    string Name
);
