namespace HikeOrganiser.Core.Behaviours.BucketList.Get;

public sealed record Request : IRequest<Model>
{
    public int Id { get; init; }
}