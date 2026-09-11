using Microsoft.EntityFrameworkCore;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Train> Trains => Set<Train>();
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Arrival> Arrivals => Set<Arrival>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Announcement> Announcements => Set<Announcement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Train>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Code).IsRequired().HasMaxLength(20);
            e.Property(t => t.Name).IsRequired().HasMaxLength(100);
            e.HasMany(t => t.Arrivals).WithOne(a => a.Train).HasForeignKey(a => a.TrainId);
            e.HasMany(t => t.Schedules).WithOne(s => s.Train).HasForeignKey(s => s.TrainId);
        });

        modelBuilder.Entity<Station>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Name).IsRequired().HasMaxLength(100);
            e.Property(s => s.Code).IsRequired().HasMaxLength(10);
            e.HasMany(s => s.Arrivals).WithOne(a => a.Station).HasForeignKey(a => a.StationId);
            e.HasMany(s => s.Announcements).WithOne(a => a.Station).HasForeignKey(a => a.StationId);
        });

        modelBuilder.Entity<Arrival>(e =>
        {
            e.HasKey(a => a.Id);
        });

        modelBuilder.Entity<Schedule>(e =>
        {
            e.HasKey(s => s.Id);
        });

        modelBuilder.Entity<Announcement>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Title).IsRequired().HasMaxLength(200);
            e.Property(a => a.Body).IsRequired().HasMaxLength(1000);
        });
    }
}