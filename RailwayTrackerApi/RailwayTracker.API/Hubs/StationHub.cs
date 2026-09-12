using Microsoft.AspNetCore.SignalR;

namespace RailwayTracker.API.Hubs;

public class StationHub : Hub
{
    public async Task JoinStationGroup(string stationId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"station-{stationId}");

    public async Task LeaveStationGroup(string stationId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"station-{stationId}");
}