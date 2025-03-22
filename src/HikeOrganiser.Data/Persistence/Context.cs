using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HikeOrganiser.Data.Persistence;

public class Context : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public Context()
    {
        
    }

    public Context(DbContextOptions<Context> options)
        : base(options)
    {
        
    }
    
    public virtual DbSet<AttendeeInformation>  AttendeeInformation => Set<AttendeeInformation>();
    public virtual DbSet<BucketList> BucketLists => Set<BucketList>();
    public virtual DbSet<Event> Events => Set<Event>();
    public virtual DbSet<UserReferralCode> UserReferralCodes => Set<UserReferralCode>();
    public virtual DbSet<ReferralCode> ReferralCodes => Set<ReferralCode>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<string>()
            .HaveMaxLength(250);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}