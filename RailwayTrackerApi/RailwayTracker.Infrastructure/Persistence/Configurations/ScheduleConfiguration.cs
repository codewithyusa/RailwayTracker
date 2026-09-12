using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.DepartureTime).IsRequired();
        builder.Property(s => s.ArrivalTime).IsRequired();
        builder.Property(s => s.IsActive).HasDefaultValue(true);
    }
}