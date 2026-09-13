using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RailwayTracker.API.Hubs;
using RailwayTracker.Application.Announcements.Commands.CreateAnnouncement;
using RailwayTracker.Application.Announcements.Queries.GetAnnouncements;

namespace RailwayTracker.API.Controllers;

[ApiController]
[Route("api/announcements")]
public class AnnouncementsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<StationHub> _hub;

    public AnnouncementsController(IMediator mediator, IHubContext<StationHub> hub)
    {
        _mediator = mediator;
        _hub = hub;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAnnouncementsQuery(), ct);
        return Ok(result.Value);
    }

    [HttpGet("station/{stationId}")]
    public async Task<IActionResult> GetByStation(int stationId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAnnouncementsQuery(stationId), ct);
        return Ok(result.Value);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateAnnouncementCommand cmd, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _mediator.Send(cmd, ct);
        if (!result.IsSuccess) return BadRequest(result.Error);

        await _hub.Clients.Group($"station-{cmd.StationId}")
            .SendAsync("NewAnnouncement", new
            {
                title = cmd.Title,
                body = cmd.Body,
                stationId = cmd.StationId,
                createdAt = DateTime.UtcNow
            }, ct);

        return CreatedAtAction(nameof(GetAll), new { }, result.Value);
    }
}