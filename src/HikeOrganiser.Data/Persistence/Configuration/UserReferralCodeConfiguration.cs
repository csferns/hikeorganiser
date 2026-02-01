namespace HikeOrganiser.Data.Persistence.Configuration;

public sealed class UserReferralCodeConfiguration : IEntityTypeConfiguration<UserReferralCode>
{
    public void Configure(EntityTypeBuilder<UserReferralCode> builder)
    {
        builder.HasKey(x => new { x.UserId, x.ReferralCodeId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.ReferralCodes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ReferralCode)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.ReferralCodeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}