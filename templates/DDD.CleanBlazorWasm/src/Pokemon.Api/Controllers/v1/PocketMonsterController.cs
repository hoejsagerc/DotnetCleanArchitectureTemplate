using System.Security.Claims;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Application.Pokemon.v1.Commands.CreatePokemon;
using Pokemon.Application.Pokemon.v1.Queries.GetPokemonById;
using Pokemon.Contracts.v1.Pokemon;

namespace Pokemon.Api.Controllers.v1;


[Route("api/v{version:apiVersion}/pokemon")]
[ApiVersion("1.0")]
[Produces(contentType: "application/json")]
public class PocketMonsterController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public PocketMonsterController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PocketMonsterResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePokemon(CreatePocketMonsterRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var command = _mapper.Map<CreatePocketMonsterCommand>((request, userId));

        var createPokemonResult = await _mediator.Send(command);

        return createPokemonResult.Match(
            createPokemonResult => Created("", _mapper.Map<PocketMonsterResponse>(createPokemonResult)),
            errors => Problem(errors));
    }

    [HttpGet]
    [Route("{pokemonId}")]
    [ProducesResponseType(typeof(PocketMonsterResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPokemonById(string pokemonId)
    {
        var query = _mapper.Map<GetPokemonByIdQuery>(pokemonId);

        var getPokemonResult = await _mediator.Send(query);

        return getPokemonResult.Match(
            pokemon => Ok(_mapper.Map<PocketMonsterResponse>(pokemon)),
            errors => Problem(errors));
    }
}
