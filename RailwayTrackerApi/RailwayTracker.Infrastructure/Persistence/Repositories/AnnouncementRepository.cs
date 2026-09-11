using Microsoft.EntityFrameworkCore;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly AppDbContext _db;
    public AnnouncementRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Announcement>> GetByStationAsync(int stationId, CancellationToken ct = default) =>
        await _db.Announcements
            .Where(a => a.StationId == stationId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(Announcement announcement, CancellationToken ct = default)
    {
        _db.Announcements.Add(announcement);
        await _db.SaveChangesAsync(ct);
    }
}