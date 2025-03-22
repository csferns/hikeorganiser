namespace HikeOrganiser.Data.Entities;

public class ReferralCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;

    public virtual ICollection<UserReferralCode> Users { get; set; } = new HashSet<UserReferralCode>();
}