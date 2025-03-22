using HikeOrganiser.Data.Enums;

namespace HikeOrganiser.Data.Entities;

public class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MeetingLocation { get; set; }
    public string? Location { get; set; }
    
    public DateType DateType { get; set; }
    public TimeOnly? MeetingTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public ulong? DiscordEventId { get; set; }

    public Guid OrganiserId { get; set; }
    public virtual User? Organiser { get; set; }
    
    public int? BucketListId { get; set; }
    public virtual BucketList? BucketList { get; set; }
    
    public virtual ICollection<AttendeeInformation> Attendees { get; set; } = new HashSet<AttendeeInformation>();
}