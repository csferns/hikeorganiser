namespace HikeOrganiser.Core.Behaviours.BucketList.GetAll;

public sealed record Request : IRequest<PagedModel<BucketListModel>>
{
    public FilterModel Filter { get; init; } = new();
    public bool? Unplanned { get; init; }
}