using Microsoft.EntityFrameworkCore;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Line> Lines => Set<Line>();
    public DbSet<Train> Trains => Set<Train>();
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Arrival> Arrivals => Set<Arrival>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Announcement> Announcements => Set<Announcement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Line>(e =>
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.Name).IsRequired().HasMaxLength(100);
            e.Property(l => l.Color).IsRequired().HasMaxLength(20);
            e.HasMany(l => l.Stations).WithOne(s => s.Line).HasForeignKey(s => s.LineId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(l => l.Trains).WithOne(t => t.Line).HasForeignKey(t => t.LineId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Train>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Code).IsRequired().HasMaxLength(20);
            e.Property(t => t.Name).IsRequired().HasMaxLength(100);
            e.Property(t => t.Status).HasConversion<string>();
            e.Property(t => t.LastUpdated).IsRequired();
            e.HasMany(t => t.Arrivals).WithOne(a => a.Train).HasForeignKey(a => a.TrainId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(t => t.Schedules).WithOne(s => s.Train).HasForeignKey(s => s.TrainId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Station>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Name).IsRequired().HasMaxLength(100);
            e.Property(s => s.Code).IsRequired().HasMaxLength(10);
            e.HasMany(s => s.Arrivals).WithOne(a => a.Station).HasForeignKey(a => a.StationId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(s => s.Announcements).WithOne(a => a.Station).HasForeignKey(a => a.StationId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Arrival>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.ScheduledTime).IsRequired();
            e.Property(a => a.DelayMinutes).HasDefaultValue(0);
        });

        modelBuilder.Entity<Schedule>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.DepartureTime).IsRequired();
            e.Property(s => s.ArrivalTime).IsRequired();
            e.Property(s => s.IsActive).HasDefaultValue(true);
            e.HasOne(s => s.Station).WithMany().HasForeignKey(s => s.StationId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Announcement>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Title).IsRequired().HasMaxLength(200);
            e.Property(a => a.Body).IsRequired().HasMaxLength(1000);
            e.Property(a => a.Type).HasConversion<string>();
            e.Property(a => a.CreatedAt).IsRequired();
        });
    }
}