using Microsoft.EntityFrameworkCore;
using RescueSphere.Api.Domain.Entities;

namespace RescueSphere.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<SupportCategory> SupportCategories => Set<SupportCategory>();
    public DbSet<HelpRequest> HelpRequests => Set<HelpRequest>();
    public DbSet<VolunteerAssignment> VolunteerAssignments => Set<VolunteerAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft Delete Global Filter
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<SupportCategory>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<HelpRequest>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<VolunteerAssignment>().HasQueryFilter(x => !x.IsDeleted);
    }
}
