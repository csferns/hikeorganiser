namespace HikeOrganiser.Core.Behaviours.Events.Delete;

public sealed record Request : IRequest<Model>
{
    public int Id { get; init; }
}