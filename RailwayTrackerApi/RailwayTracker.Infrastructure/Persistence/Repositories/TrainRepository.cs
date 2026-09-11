using Microsoft.EntityFrameworkCore;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Infrastructure.Persistence.Repositories;

public class TrainRepository : ITrainRepository
{
    private readonly AppDbContext _db;
    public TrainRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Train>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Trains.Include(t => t.Arrivals).ToListAsync(ct);

    public async Task<Train?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.Trains.Include(t => t.Arrivals).FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task UpdatePositionAsync(int id, double lat, double lng, CancellationToken ct = default)
    {
        var train = await _db.Trains.FindAsync([id], ct);
        if (train is null) return;
        train.Latitude = lat;
        train.Longitude = lng;
        train.LastUpdated = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
    }
}