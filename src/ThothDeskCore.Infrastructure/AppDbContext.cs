using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Domain;

namespace ThothDeskCore.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Course> Courses => Set<Course>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Code).IsRequired().HasMaxLength(32);
            entity.Property(x=>x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Semester).IsRequired().HasMaxLength(20);
            entity.HasIndex(x => x.Code).IsUnique();
        });
    }
}

