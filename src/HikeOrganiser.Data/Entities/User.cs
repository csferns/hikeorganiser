using Microsoft.AspNetCore.Identity;

namespace HikeOrganiser.Data.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public string DisplayName => $"{FirstName} {LastName}";
    
    public ulong? DiscordUserId { get; set; }
    
    public Guid? ReferredByUserId { get; set; }
    public virtual User? ReferredByUser { get; set; }
    
    public virtual ICollection<Event> OrganisedEvents { get; set; } = new HashSet<Event>();
    public virtual ICollection<AttendeeInformation> Events { get; set; } = new HashSet<AttendeeInformation>();
    public virtual ICollection<UserReferralCode> ReferralCodes { get; set; } = new HashSet<UserReferralCode>();
    public virtual ICollection<User> ReferredUsers { get; set; } = new HashSet<User>();
    public virtual ICollection<BucketList> SuggestedBucketListItems { get; set; } = new HashSet<BucketList>();
}