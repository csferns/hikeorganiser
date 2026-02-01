namespace HikeOrganiser.Core.Behaviours.Events.Get;

public sealed record Request : IRequest<Model>
{
    public int Id { get; init; }
}