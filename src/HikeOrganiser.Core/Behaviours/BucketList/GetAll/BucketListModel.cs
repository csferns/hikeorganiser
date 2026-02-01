namespace HikeOrganiser.Core.Behaviours.BucketList.GetAll;

public sealed record BucketListModel
{
    public int Id { get; set; }
    
    public string FriendlyName { get; set; }
    public string Location { get; set; }
    
    public string UserDisplayName { get; set; }
    public int? PlannedEventId { get; set; }
}