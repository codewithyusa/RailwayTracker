namespace RailwayTracker.Domain.Entities;

public class Arrival
{
    public int Id { get; set; }
    public int TrainId { get; set; }
    public int StationId { get; set; }
    public int Platform { get; set; }
    public DateTime ScheduledTime { get; set; }
    public DateTime? ActualTime { get; set; }
    public int DelayMinutes { get; set; }

    public Train Train { get; set; } = null!;
    public Station Station { get; set; } = null!;
}