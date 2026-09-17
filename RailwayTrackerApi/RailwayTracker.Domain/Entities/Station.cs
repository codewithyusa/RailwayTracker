namespace RailwayTracker.Domain.Entities;

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int LineId { get; set; }
    public Line? Line { get; set; }
    public int StopOrder { get; set; }
    public ICollection<Arrival> Arrivals { get; set; } = new List<Arrival>();
}