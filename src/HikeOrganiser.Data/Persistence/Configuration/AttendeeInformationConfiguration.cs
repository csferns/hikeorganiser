namespace HikeOrganiser.Data.Persistence.Configuration;

public sealed class AttendeeInformationConfiguration : IEntityTypeConfiguration<AttendeeInformation>
{
    public void Configure(EntityTypeBuilder<AttendeeInformation> builder)
    {
        builder.HasKey(x => new { x.EventId, x.UserId });

        builder.HasOne(x => x.Event)
            .WithMany(x => x.Attendees)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}