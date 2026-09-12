using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Announcements.Queries.GetAnnouncements;

public record GetAnnouncementsQuery(int? StationId = null) : IRequest<Result<IEnumerable<Announcement>>>;