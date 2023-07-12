using ErrorOr;
using MediatR;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;
using Pokemon.Domain.PokemonAggregate;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Application.Pokemon.v1.Queries.GetPokemonById;

public class GetPokemobByIdQueryHandler : IRequestHandler<GetPokemonByIdQuery, ErrorOr<PocketMonster>>
{
    private readonly IPocketMonsterRepository _pocketMonsterRepository;

    public GetPokemobByIdQueryHandler(IPocketMonsterRepository pocketMonsterRepository)
    {
        _pocketMonsterRepository = pocketMonsterRepository;
    }

    public async Task<ErrorOr<PocketMonster>> Handle(GetPokemonByIdQuery query, CancellationToken cancellationToken)
    {
        var pokemon = await _pocketMonsterRepository.GetByIdAsync(PokemonId.Create(Guid.Parse(query.PokemonId)));

        if (pokemon is null)
        {
            return Errors.Pokemon.NotFound;
        }

        return pokemon;
    }
}
