namespace HikeOrganiser.Data.Persistence.Configuration;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ReferredByUser)
            .WithMany(x => x.ReferredUsers)
            .HasForeignKey(x => x.ReferredByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}