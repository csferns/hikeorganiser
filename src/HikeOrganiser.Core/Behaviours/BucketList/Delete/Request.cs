namespace HikeOrganiser.Core.Behaviours.BucketList.Delete;

public sealed record Request : IRequest<Model>
{
    public int Id { get; init; }
}