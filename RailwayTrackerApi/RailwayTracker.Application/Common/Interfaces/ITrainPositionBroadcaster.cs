namespace RailwayTracker.Application.Common.Interfaces;

public interface ITrainPositionBroadcaster
{
    Task BroadcastAsync(int trainId, double latitude, double longitude, CancellationToken ct);
}