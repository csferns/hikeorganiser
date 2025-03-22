namespace HikeOrganiser.Data.Entities;

public class UserReferralCode
{
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
    
    public int ReferralCodeId { get; set; }
    public virtual ReferralCode? ReferralCode { get; set; }
}