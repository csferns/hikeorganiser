namespace HikeOrganiser.Data.Persistence.Configuration;

public sealed class ReferralCodeConfiguration : IEntityTypeConfiguration<ReferralCode>
{
    public void Configure(EntityTypeBuilder<ReferralCode> builder)
    {
        builder.HasKey(x => x.Id);
    }
}