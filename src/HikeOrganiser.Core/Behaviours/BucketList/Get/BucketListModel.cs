namespace HikeOrganiser.Core.Behaviours.BucketList.Get;

public sealed record BucketListModel
{
    public int Id { get; set; }
    
    public string FriendlyName { get; set; }
    public string Location { get; set; }
    
    public Guid SuggestedById { get; set; }
    public int? PlannedEventId { get; set; }
}