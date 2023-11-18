using BudgetTracker.Application.Trackable.v1.Commands.DeleteTrackable;
using BudgetTracker.Application.Trackable.v1.Commands.UpdateTrackable;
using BudgetTracker.Application.Trackable.v1.Queries.GetTrackableById;
using BudgetTracker.Application.Trackable.v1.Queries.GetTrackables;
using BudgetTracker.Contracts.v1.Trackable;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.Enums;
using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.Api.Common.Mappings;

public class TrackableMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, GetTrackableByIdQuery>()
            .MapWith(src => new GetTrackableByIdQuery(src));

        config.NewConfig<(string UserId, string Name, int Page, int PageSize), GetTrackablesQuery>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Page, src => src.Page)
            .Map(dest => dest.PageSize, src => src.PageSize);

        config.NewConfig<(CreateTrackableRequest Request, string Id), UpdateTrackableCommand>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<string, DeleteTrackableCommand>()
            .MapWith(src => new DeleteTrackableCommand(src));

        config.NewConfig<TrackableAggregate, TrackableResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest, src => src);
    }
}