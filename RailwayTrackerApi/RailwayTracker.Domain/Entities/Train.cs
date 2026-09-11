using RailwayTracker.Domain.Enums;

namespace RailwayTracker.Domain.Entities;

public class Train
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public TrainStatus Status { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public ICollection<Arrival> Arrivals { get; set; } = new List<Arrival>();
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}