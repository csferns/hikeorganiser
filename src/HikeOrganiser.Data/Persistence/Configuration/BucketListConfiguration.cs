namespace HikeOrganiser.Data.Persistence.Configuration;

public sealed class BucketListConfiguration : IEntityTypeConfiguration<BucketList>
{
    public void Configure(EntityTypeBuilder<BucketList> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.SuggestedBy)
            .WithMany(x => x.SuggestedBucketListItems)
            .HasForeignKey(x => x.SuggestedById)
            .OnDelete(DeleteBehavior.Cascade);
    }
}