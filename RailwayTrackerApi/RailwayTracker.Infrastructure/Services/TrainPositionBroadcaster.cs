using Microsoft.AspNetCore.SignalR;
using RailwayTracker.API.Hubs;
using RailwayTracker.Application.Common.Interfaces;

namespace RailwayTracker.Infrastructure.Services;

public class TrainPositionBroadcaster : ITrainPositionBroadcaster
{
    private readonly IHubContext<TrainHub> _hub;
    public TrainPositionBroadcaster(IHubContext<TrainHub> hub) => _hub = hub;

    public async Task BroadcastAsync(int trainId, double latitude, double longitude, CancellationToken ct)
    {
        await _hub.Clients.Group($"train-{trainId}")
            .SendAsync("PositionUpdated", new { trainId, latitude, longitude }, ct);
    }
}