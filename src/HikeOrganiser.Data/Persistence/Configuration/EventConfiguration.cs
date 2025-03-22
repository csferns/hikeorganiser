namespace HikeOrganiser.Data.Persistence.Configuration;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Organiser)
            .WithMany(x => x.OrganisedEvents)
            .HasForeignKey(x => x.OrganiserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.BucketList)
            .WithOne(x => x.PlannedEvent)
            .HasForeignKey<Event>(x => x.BucketListId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}