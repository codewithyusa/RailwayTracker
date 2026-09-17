using RailwayTracker.Domain.Enums;

namespace RailwayTracker.Domain.Entities;

public class Announcement
{
    public int Id { get; set; }
    public int? StationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public AnnouncementType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Station? Station { get; set; }
}