using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence.Configurations;

public class ArrivalConfiguration : IEntityTypeConfiguration<Arrival>
{
    public void Configure(EntityTypeBuilder<Arrival> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.ScheduledTime).IsRequired();
        builder.Property(a => a.DelayMinutes).HasDefaultValue(0);
    }
}