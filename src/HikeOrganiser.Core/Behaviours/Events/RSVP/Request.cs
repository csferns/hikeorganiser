using HikeOrganiser.Data.Enums;

namespace HikeOrganiser.Core.Behaviours.Events.RSVP;

public sealed record Request : IRequest
{
    public int EventId { get; init; }
    public ResponseStatus Status { get; init; }
}