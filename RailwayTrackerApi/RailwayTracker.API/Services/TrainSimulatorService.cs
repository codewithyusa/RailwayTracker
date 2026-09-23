using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RailwayTracker.API.Hubs;
using RailwayTracker.Infrastructure.Persistence;
using RailwayTracker.Domain.Enums;

namespace RailwayTracker.API.Services;

public class TrainSimulatorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<TrainHub> _hub;
    private readonly ILogger<TrainSimulatorService> _logger;

    // Each train tracks which stop index it's heading to and direction
    private readonly Dictionary<int, (int stopIndex, int direction)> _trainState = new();

    public TrainSimulatorService(
        IServiceScopeFactory scopeFactory,
        IHubContext<TrainHub> hub,
        ILogger<TrainSimulatorService> logger)
    {
        _scopeFactory = scopeFactory;
        _hub = hub;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("Train simulator started.");

        // Wait for app to fully start
        await Task.Delay(3000, ct);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                await TickAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Simulator tick error.");
            }

            await Task.Delay(3000, ct); // move every 3 seconds
        }
    }

    private async Task TickAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var trains = await db.Trains
            .Where(t => t.IsActive && t.Status != TrainStatus.Cancelled)
            .Include(t => t.Line)
            .ThenInclude(l => l.Stations!.OrderBy(s => s.StopOrder))
            .ToListAsync(ct);

        foreach (var train in trains)
        {
            var stops = train.Line?.Stations?.ToList();
            if (stops == null || stops.Count < 2) continue;

            // Init state for new trains
            if (!_trainState.ContainsKey(train.Id))
            {
                // Find closest stop to current position
                int closest = 0;
                double minDist = double.MaxValue;
                for (int i = 0; i < stops.Count; i++)
                {
                    double d = Dist(train.Latitude, train.Longitude,
                                    stops[i].Latitude, stops[i].Longitude);
                    if (d < minDist) { minDist = d; closest = i; }
                }
                _trainState[train.Id] = (closest, 1);
            }

            var (stopIndex, direction) = _trainState[train.Id];

            // Move toward next stop
            int nextIndex = stopIndex + direction;
            if (nextIndex >= stops.Count) { direction = -1; nextIndex = stopIndex - 1; }
            if (nextIndex < 0)            { direction =  1; nextIndex = stopIndex + 1; }

            var target = stops[nextIndex];

            // Interpolate position (move 30% closer each tick)
            double newLat = train.Latitude  + (target.Latitude  - train.Latitude)  * 0.3;
            double newLng = train.Longitude + (target.Longitude - train.Longitude) * 0.3;

            // Snap to stop if close enough
            if (Dist(newLat, newLng, target.Latitude, target.Longitude) < 0.001)
            {
                newLat = target.Latitude;
                newLng = target.Longitude;
                _trainState[train.Id] = (nextIndex, direction);
            }

            train.Latitude     = newLat;
            train.Longitude    = newLng;
            train.LastUpdated  = DateTime.UtcNow;
        }

        await db.SaveChangesAsync(ct);

        // Broadcast all active positions via SignalR
        foreach (var train in trains)
        {
            await _hub.Clients.All.SendAsync(
                "ReceiveTrainPosition",
                train.Id, train.Latitude, train.Longitude, ct);
        }
    }

    private static double Dist(double lat1, double lng1, double lat2, double lng2)
    {
        double dlat = lat1 - lat2;
        double dlng = lng1 - lng2;
        return Math.Sqrt(dlat * dlat + dlng * dlng);
    }
}