using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RailwayTracker.Application.Announcements.Commands.CreateAnnouncement;
using RailwayTracker.Application.Announcements.Queries.GetAnnouncements;

namespace RailwayTracker.API.Controllers;

[ApiController]
[Route("api/announcements")]
public class AnnouncementsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AnnouncementsController(IMediator mediator) => _mediator = mediator;

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
        return CreatedAtAction(nameof(GetAll), new { }, result.Value);
    }
}