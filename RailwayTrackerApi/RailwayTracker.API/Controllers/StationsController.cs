using MediatR;
using Microsoft.AspNetCore.Mvc;
using RailwayTracker.Application.Stations.Queries.GetAllStations;
using RailwayTracker.Application.Stations.Queries.GetStationArrivals;

namespace RailwayTracker.API.Controllers;

[ApiController]
[Route("api/stations")]
public class StationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public StationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllStationsQuery(), ct);
        return Ok(result.Value);
    }

    [HttpGet("{id}/arrivals")]
    public async Task<IActionResult> GetArrivals(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetStationArrivalsQuery(id), ct);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }
}