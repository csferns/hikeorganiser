namespace HikeOrganiser.Core.Behaviours.BucketList.Get;

public sealed record Model
{
    public required BucketListModel? BucketList { get; init; }
}