using HikeOrganiser.Data.Enums;

namespace HikeOrganiser.Core.Behaviours.Events.Get;

public sealed record AttendeeInformationModel
{
    public bool Driving { get; set; }
    public ResponseStatus Status { get; set; }
    
    public int EventId { get; set; }
    public Guid UserId { get; set; }
}