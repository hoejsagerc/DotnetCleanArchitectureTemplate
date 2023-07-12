namespace Pokemon.Contracts.v1.Pokemon;

public record PokemonResponse
(
    string Id,
    string Name,
    string Type,
    int Height,
    int Weight,
    int Experience,
    int ExperienceToNextLevel,
    string Sprite,
    List<MoveResponse> Moves,
    List<StatResponse> Stats,
    List<AbilityResponse> Abilities,
    List<EvolutionResponse> Evolutions
);

public record MoveResponse
(
    string Id,
    string Name,
    string Description,
    float Damage,
    float CriticalHitFactor,
    float CriticalHitChance
);

public record StatResponse
(
    string Id,
    string Name,
    int BaseStat,
    int Effort
);

public record AbilityResponse
(
    string Id,
    string Name
);

public record EvolutionResponse
(
    string Id,
    string Name
);