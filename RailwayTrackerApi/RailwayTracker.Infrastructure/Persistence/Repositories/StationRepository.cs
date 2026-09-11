using Microsoft.EntityFrameworkCore;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence.Repositories;

public class StationRepository : IStationRepository
{
    private readonly AppDbContext _db;
    public StationRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Station>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Stations.ToListAsync(ct);

    public async Task<Station?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.Stations.Include(s => s.Arrivals).FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IEnumerable<Arrival>> GetArrivalsAsync(int stationId, CancellationToken ct = default) =>
        await _db.Arrivals
            .Include(a => a.Train)
            .Where(a => a.StationId == stationId)
            .OrderBy(a => a.ScheduledTime)
            .ToListAsync(ct);
}