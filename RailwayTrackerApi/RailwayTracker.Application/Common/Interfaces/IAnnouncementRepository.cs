using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Common.Interfaces;

public interface IAnnouncementRepository
{
    Task<IEnumerable<Announcement>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Announcement>> GetByStationAsync(int stationId, CancellationToken ct = default);
    Task AddAsync(Announcement announcement, CancellationToken ct = default);
}