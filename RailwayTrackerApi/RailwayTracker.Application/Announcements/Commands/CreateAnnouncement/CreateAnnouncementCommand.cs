using System.ComponentModel.DataAnnotations;
using MediatR;
using RailwayTracker.Application.Common;

namespace RailwayTracker.Application.Announcements.Commands.CreateAnnouncement;

public record CreateAnnouncementCommand(
    [Required][MinLength(1)] string Title,
    [Required][MinLength(1)] string Body,
    [Range(1, int.MaxValue)] int StationId
) : IRequest<Result<int>>;