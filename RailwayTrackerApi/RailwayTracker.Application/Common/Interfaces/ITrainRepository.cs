using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Common.Interfaces;

public interface ITrainRepository
{
    Task<IEnumerable<Train>> GetAllAsync(CancellationToken ct = default);
    Task<Train?> GetByIdAsync(int id, CancellationToken ct = default);
    Task UpdatePositionAsync(int id, double lat, double lng, CancellationToken ct = default);
}