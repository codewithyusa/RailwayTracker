namespace RailwayTracker.Domain.Entities;

public class Schedule
{
    public int Id { get; set; }
    public int TrainId { get; set; }
    public int StationId { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public int Platform { get; set; }
    public bool IsActive { get; set; } = true;

    public Train Train { get; set; } = null!;
    public Station Station { get; set; } = null!;
}