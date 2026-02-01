using HikeOrganiser.Data.Enums;

namespace HikeOrganiser.Data.Entities;

public class AttendeeInformation
{
    public bool Driving { get; set; }
    public ResponseStatus Status { get; set; }
    
    public int EventId { get; set; }
    public virtual Event? Event { get; set; }
    
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
}