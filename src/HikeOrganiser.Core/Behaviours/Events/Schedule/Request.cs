using HikeOrganiser.Data.Enums;

namespace HikeOrganiser.Core.Behaviours.Events.Schedule;

public sealed record Request : IRequest<Model>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? MeetingLocation { get; set; }
    public string Location { get; set; } = string.Empty;

    public DateType DateType { get; set; }
    public TimeOnly? MeetingTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int? BucketListId { get; set; }
}