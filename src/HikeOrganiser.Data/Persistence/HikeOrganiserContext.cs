using System.Reflection;
using HikeOrganiser.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HikeOrganiser.Data.Persistence;

public class HikeOrganiserContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public HikeOrganiserContext()
    {
        
    }

    public HikeOrganiserContext(DbContextOptions<HikeOrganiserContext> options)
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseModel(ContextModel.Instance);
    }
}