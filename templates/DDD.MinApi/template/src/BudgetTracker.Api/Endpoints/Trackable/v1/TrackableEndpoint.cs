using Asp.Versioning;
using BudgetTracker.Api.Endpoints.Common;
using BudgetTracker.Application.Trackable.v1.Commands.CreateTrackable;
using BudgetTracker.Application.Trackable.v1.Commands.DeleteTrackable;
using BudgetTracker.Application.Trackable.v1.Commands.UpdateTrackable;
using BudgetTracker.Application.Trackable.v1.Queries.GetTrackableById;
using BudgetTracker.Application.Trackable.v1.Queries.GetTrackables;
using BudgetTracker.Contracts.v1.Common;
using BudgetTracker.Contracts.v1.Trackable;
using MapsterMapper;
using MediatR;

namespace BudgetTracker.Api.Endpoints.Trackable.v1;

public static class TrackableEndpoints
{
    private static readonly Problem Problem = new Problem();
    public static void MapTrackableEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
                .HasDeprecatedApiVersion(new ApiVersion(0, 1))
                .HasApiVersion(new ApiVersion(1, 0))
                .ReportApiVersions()
                .Build();
        
        var group = app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(versionSet);


        group.MapGet("/trackable/{id}", async (string id, ISender mediator, IMapper mapper) =>
            {
                var query = mapper.Map<GetTrackableByIdQuery>(id);
                var queryResult = await mediator.Send(query);

                return queryResult.Match<IResult>(
                    result => Results.Ok(mapper.Map<TrackableResponse>(result)), 
                    Problem.Response);
            })
            .WithName("GetById")
            .Produces<TrackableResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Endpoint for querying a trackable by id.",
                Description = "Endpoint for querying a trackable by id."
            });


        group.MapGet("/trackables", async (string? userId, string? name,
                ISender mediator, IMapper mapper, int page = 1, int pageSize = 10) =>
            {
                var query = mapper.Map<GetTrackablesQuery>((userId, name, page, pageSize));
                var queryResult = await mediator.Send(query);

                return queryResult.Match<IResult>(
                    result =>
                    {
                        var listResult = result.Items.Select(aggregate =>
                            mapper.Map<TrackableResponse>(aggregate)).ToList();

                        var pagedResponseList = new PagedListResponse<TrackableResponse>(
                            listResult, result.Page, result.PageSize, result.TotalCount,
                            result.HasNextPage, result.HasPreviousPage);
                        return Results.Ok(pagedResponseList);
                    },
                    Problem.Response);
            })
            .WithName("GetAll")
            .Produces<PagedListResponse<TrackableResponse>>(StatusCodes.Status200OK)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Endpoint for querying all trackables.",
                Description = "Endpoint for querying all trackables."
            });


        group.MapPost("trackable", async (CreateTrackableRequest request,
                ISender mediator, IMapper mapper) =>
            {
                var command = mapper.Map<CreateTrackableCommand>(request);
                var commandResult = await mediator.Send(command);

                return commandResult.Match<IResult>(
                    results =>
                    {
                        var response = mapper.Map<TrackableResponse>(results);
                        return Results.CreatedAtRoute("GetById",
                            new { id = commandResult.Value.Id.Value },
                            response);
                    },
                    Problem.Response);
            })
            .WithName("Create")
            .Produces<TrackableResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Endpoint for creating a trackable.",
                Description = "Endpoint for creating a trackable."
            });


        group.MapPut("trackable/{id}", async (CreateTrackableRequest request, string id, 
            ISender mediator, IMapper mapper) =>
            {
                var command = mapper.Map<UpdateTrackableCommand>((request, id));
                var commandResult = await mediator.Send(command);
                
                return commandResult.Match<IResult>(
                    result => Results.NoContent(),
                    Problem.Response);
            })
            .WithName("Update")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Endpoint for updating a trackable.",
                Description = "Endpoint for updating a trackable."
            });
        
        
        group.MapDelete("trackable/{id}", async (string id, ISender mediator, IMapper mapper) =>
            {
                var command = mapper.Map<DeleteTrackableCommand>(id);
                var commandResult = await mediator.Send(command);
                
                return commandResult.Match<IResult>(
                    result => Results.NoContent(),
                    Problem.Response);
            })
            .WithName("Delete")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Endpoint for deleting a trackable.",
                Description = "Endpoint for deleting a trackable."
            });
    }
    
}