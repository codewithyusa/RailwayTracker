using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Common.Interfaces;

public interface IStationRepository
{
    Task<IEnumerable<Station>> GetAllAsync(CancellationToken ct = default);
    Task<Station?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Arrival>> GetArrivalsAsync(int stationId, CancellationToken ct = default);
}