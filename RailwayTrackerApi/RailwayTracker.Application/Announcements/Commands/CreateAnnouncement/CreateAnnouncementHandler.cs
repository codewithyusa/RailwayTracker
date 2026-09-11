using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Announcements.Commands.CreateAnnouncement;

public class CreateAnnouncementHandler : IRequestHandler<CreateAnnouncementCommand, Result<int>>
{
    private readonly IAnnouncementRepository _repo;
    public CreateAnnouncementHandler(IAnnouncementRepository repo) => _repo = repo;

    public async Task<Result<int>> Handle(CreateAnnouncementCommand cmd, CancellationToken ct)
    {
        var announcement = new Announcement
        {
            Title = cmd.Title,
            Body = cmd.Body,
            StationId = cmd.StationId,
            CreatedAt = DateTime.UtcNow
        };
        await _repo.AddAsync(announcement, ct);
        return Result<int>.Success(announcement.Id);
    }
}