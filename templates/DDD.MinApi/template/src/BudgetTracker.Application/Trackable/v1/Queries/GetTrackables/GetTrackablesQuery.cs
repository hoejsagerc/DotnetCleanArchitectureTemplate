using BudgetTracker.Contracts.v1.Common;
using BudgetTracker.Domain.Common.Models;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.Enums;
using ErrorOr;
using MediatR;

namespace BudgetTracker.Application.Trackable.v1.Queries.GetTrackables;

public record GetTrackablesQuery(int Page, int PageSize, string? UserId, string? Name) : 
    IRequest<ErrorOr<PagedList<TrackableAggregate>>>;