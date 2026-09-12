using MediatR;
using Microsoft.AspNetCore.Mvc;
using RailwayTracker.Application.Trains.Queries.GetAllTrains;
using RailwayTracker.Application.Trains.Queries.GetTrainById;
using RailwayTracker.Application.Trains.Commands.UpdateTrainPosition;

namespace RailwayTracker.API.Controllers;

[ApiController]
[Route("api/trains")]
public class TrainsController : ControllerBase
{
    private readonly IMediator _mediator;
    public TrainsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTrainsQuery(), ct);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTrainByIdQuery(id), ct);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPut("{id}/position")]
    public async Task<IActionResult> UpdatePosition(int id, [FromBody] UpdatePositionRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateTrainPositionCommand(id, request.Latitude, request.Longitude), ct);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok();
    }
}

public record UpdatePositionRequest(double Latitude, double Longitude);