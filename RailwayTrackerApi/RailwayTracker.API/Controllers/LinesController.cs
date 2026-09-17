using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayTracker.Infrastructure.Persistence;

namespace RailwayTracker.API.Controllers;

[ApiController]
[Route("api/lines")]
public class LinesController : ControllerBase
{
    private readonly AppDbContext _db;
    public LinesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var lines = await _db.Lines
            .Include(l => l.Stations.OrderBy(s => s.StopOrder))
            .ToListAsync(ct);
        return Ok(lines);
    }
}