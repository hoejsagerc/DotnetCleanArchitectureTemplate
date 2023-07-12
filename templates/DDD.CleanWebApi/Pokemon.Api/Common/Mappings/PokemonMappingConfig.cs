using Mapster;
using Pokemon.Application.Pokemon.v1.Commands.CreatePokemon;
using Pokemon.Application.Pokemon.v1.Queries.GetPokemonById;
using Pokemon.Contracts.v1.Pokemon;
using Pokemon.Domain.Common.PokemonAggregate.Entities;
using Pokemon.Domain.PokemonAggregate;

namespace Pokemon.Api.Common.Mappings;

public class PokemonMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreatePokemonRequest Request, string UserId), CreatePokemonCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<Move, MoveResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString());

        config.NewConfig<Stat, StatResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString());

        config.NewConfig<Ability, AbilityResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString());

        config.NewConfig<Evolutions, EvolutionResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString());

        config.NewConfig<PocketMonster, PokemonResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString());

        config.NewConfig<string, GetPokemonByIdQuery>()
            .MapWith(src => new GetPokemonByIdQuery(src));
    }
}