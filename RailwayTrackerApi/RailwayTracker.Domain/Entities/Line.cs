namespace RailwayTracker.Domain.Entities;

public class Line
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string TerminusA { get; set; } = string.Empty;
    public string TerminusB { get; set; } = string.Empty;
    public ICollection<Station> Stations { get; set; } = new List<Station>();
}