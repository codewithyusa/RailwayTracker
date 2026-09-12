using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Announcements.Queries.GetAnnouncements;

public class GetAnnouncementsHandler : IRequestHandler<GetAnnouncementsQuery, Result<IEnumerable<Announcement>>>
{
    private readonly IAnnouncementRepository _repo;
    public GetAnnouncementsHandler(IAnnouncementRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<Announcement>>> Handle(GetAnnouncementsQuery query, CancellationToken ct)
    {
        var announcements = query.StationId.HasValue
            ? await _repo.GetByStationAsync(query.StationId.Value, ct)
            : await _repo.GetAllAsync(ct);
        return Result<IEnumerable<Announcement>>.Success(announcements);
    }
}