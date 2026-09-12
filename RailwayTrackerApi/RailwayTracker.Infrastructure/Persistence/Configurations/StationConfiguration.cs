using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence.Configurations;

public class StationConfiguration : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Code).IsRequired().HasMaxLength(10);
        builder.HasMany(s => s.Arrivals).WithOne(a => a.Station).HasForeignKey(a => a.StationId);
        builder.HasMany(s => s.Announcements).WithOne(a => a.Station).HasForeignKey(a => a.StationId);
    }
}