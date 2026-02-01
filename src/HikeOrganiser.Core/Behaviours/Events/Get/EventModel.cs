namespace HikeOrganiser.Core.Behaviours.Events.Get;

public sealed record EventModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MeetingLocation { get; set; }
    public string? Location { get; set; }
    
    public TimeOnly? MeetingTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public List<string> Attendees { get; set; } = [];
}