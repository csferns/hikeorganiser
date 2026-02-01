namespace HikeOrganiser.Core.Behaviours.Events.GetAll;

public sealed record Request : IRequest<PagedModel<EventModel>>
{
    public FilterModel Filter { get; init; } = new();
}