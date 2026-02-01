namespace HikeOrganiser.Core.Behaviours.Events.Get;

public sealed record Model
{
    public required EventModel Event { get; init; }
}