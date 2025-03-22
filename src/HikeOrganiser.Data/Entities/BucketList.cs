namespace HikeOrganiser.Data.Entities;

public class BucketList
{
    public int Id { get; set; }
    
    public string FriendlyName { get; set; }
    public string Location { get; set; }
    
    public Guid SuggestedById { get; set; }
    public virtual User? SuggestedBy { get; set; }
    
    public int? PlannedEventId { get; set; }
    public virtual Event? PlannedEvent { get; set; }
}