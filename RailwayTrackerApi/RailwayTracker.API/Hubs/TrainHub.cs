using Microsoft.AspNetCore.SignalR;

namespace RailwayTracker.API.Hubs;

public class TrainHub : Hub
{
    public async Task JoinTrainGroup(string trainId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"train-{trainId}");

    public async Task LeaveTrainGroup(string trainId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"train-{trainId}");
}