using MediatR;
using Microsoft.AspNetCore.Mvc;
using RailwayTracker.Application.Announcements.Commands.CreateAnnouncement;

namespace RailwayTracker.API.Controllers;

[ApiController]
[Route("api/announcements")]
public class AnnouncementsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AnnouncementsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateAnnouncementCommand cmd, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _mediator.Send(cmd, ct);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }
}