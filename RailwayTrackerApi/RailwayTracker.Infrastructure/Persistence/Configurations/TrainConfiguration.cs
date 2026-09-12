using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence.Configurations;

public class TrainConfiguration : IEntityTypeConfiguration<Train>
{
    public void Configure(EntityTypeBuilder<Train> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Code).IsRequired().HasMaxLength(20);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Status).HasConversion<string>();
        builder.HasMany(t => t.Arrivals).WithOne(a => a.Train).HasForeignKey(a => a.TrainId);
        builder.HasMany(t => t.Schedules).WithOne(s => s.Train).HasForeignKey(s => s.TrainId);
    }
}