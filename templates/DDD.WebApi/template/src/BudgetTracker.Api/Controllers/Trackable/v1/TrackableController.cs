using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using BudgetTracker.Api.Controllers.Common;
using BudgetTracker.Application.Trackable.v1.Commands.CreateTrackable;
using BudgetTracker.Application.Trackable.v1.Commands.DeleteTrackable;
using BudgetTracker.Application.Trackable.v1.Commands.UpdateTrackable;
using BudgetTracker.Application.Trackable.v1.Queries.GetTrackableById;
using BudgetTracker.Application.Trackable.v1.Queries.GetTrackables;
using BudgetTracker.Contracts.v1.Common;
using BudgetTracker.Contracts.v1.Trackable;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Api.Controllers.Trackable.v1;

[Route("api/v{version:apiVersion}")]
[ApiVersion(1.0)]
public class MovieController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public MovieController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet("trackable/{id}", Name = "GetById")]
    [ProducesResponseType(typeof(TrackableResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(string id)
    {
        var query = _mapper.Map<GetTrackableByIdQuery>(id);
        var queryResult = await _mediator.Send(query);
        
        return queryResult.Match<IActionResult>(
            m => Ok(_mapper.Map<TrackableResponse>(m)),
            err => Problem(err));
    }
    
    [HttpGet("trackables", Name = "GetAll")]
    [ProducesResponseType(typeof(PagedListResponse<TrackableResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromQuery]string userId, string name, int page, int pageSize)
    {
        var query = _mapper.Map<GetTrackablesQuery>((userId, name, page, pageSize));
        var queryResult = await _mediator.Send(query);

        return queryResult.Match<IActionResult>(
            result =>
            {
                var listResult = result.Items.Select(aggregate =>
                    _mapper.Map<TrackableResponse>(aggregate)).ToList();

                var pagedResponseList = new PagedListResponse<TrackableResponse>(
                    listResult, result.Page, result.PageSize, result.TotalCount,
                    result.HasNextPage, result.HasPreviousPage);
                return Ok(pagedResponseList);
            },
            err => Problem(err));
    }
    
    
    [HttpPost("trackable", Name = "Create")]
    [ProducesResponseType(typeof(TrackableResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTrackableRequest request)
    {
        var command = _mapper.Map<CreateTrackableCommand>(request);
        var commandResult = await _mediator.Send(command);

        return commandResult.Match<IActionResult>(
            results =>
            {
                var response = _mapper.Map<TrackableResponse>(results);
                return CreatedAtRoute("GetById",
                    new { id = commandResult.Value.Id.Value },
                    response);
            },
            err => Problem(err));
    }
    
    
    [HttpPut("trackable/{id}", Name = "Update")]
    [ProducesResponseType(typeof(TrackableResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] CreateTrackableRequest request, string id)
    {
        var command = _mapper.Map<UpdateTrackableCommand>((request, id));
        var commandResult = await _mediator.Send(command);
                
        return commandResult.Match<IActionResult>(
            result => NoContent(),
            err => Problem(err));
    }
    
    
    [HttpDelete("trackable/{id}", Name = "Delete")]
    [ProducesResponseType(typeof(TrackableResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        var command = _mapper.Map<DeleteTrackableCommand>(id);
        var commandResult = await _mediator.Send(command);
                
        return commandResult.Match<IActionResult>(
            result => NoContent(),
            err => Problem(err));
    }
}
