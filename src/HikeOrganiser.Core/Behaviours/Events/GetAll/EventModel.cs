using HikeOrganiser.Data.Enums;

namespace HikeOrganiser.Core.Behaviours.Events.GetAll;

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
    
    public AttendeeInformationModel CurrentUser { get; set; } = new AttendeeInformationModel();
    public List<AttendeeInformationModel> Attendees { get; init; } = [];
}

public sealed record AttendeeInformationModel
{
    public string Name { get; set; } = string.Empty;
    public ResponseStatus Status { get; set; }
}