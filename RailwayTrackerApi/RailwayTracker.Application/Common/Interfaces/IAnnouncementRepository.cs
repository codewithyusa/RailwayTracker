using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Common.Interfaces;

public interface IAnnouncementRepository
{
    Task<IEnumerable<Announcement>> GetByStationAsync(int stationId, CancellationToken ct = default);
    Task AddAsync(Announcement announcement, CancellationToken ct = default);
}