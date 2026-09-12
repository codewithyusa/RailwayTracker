using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Application.Common.Interfaces;

namespace RailwayTracker.Application.Trains.Commands.UpdateTrainPosition;

public class UpdateTrainPositionHandler
    : IRequestHandler<UpdateTrainPositionCommand, Result<bool>>
{
    private readonly ITrainRepository _repo;
    private readonly ITrainPositionBroadcaster _broadcaster;

    public UpdateTrainPositionHandler(ITrainRepository repo, ITrainPositionBroadcaster broadcaster)
    {
        _repo = repo;
        _broadcaster = broadcaster;
    }

    public async Task<Result<bool>> Handle(
        UpdateTrainPositionCommand cmd, CancellationToken ct)
    {
        var train = await _repo.GetByIdAsync(cmd.TrainId, ct);
        if (train is null)
            return Result<bool>.Failure(new Error("train_not_found", $"Train {cmd.TrainId} not found."));

        await _repo.UpdatePositionAsync(cmd.TrainId, cmd.Latitude, cmd.Longitude, ct);
        await _broadcaster.BroadcastAsync(cmd.TrainId, cmd.Latitude, cmd.Longitude, ct);
        return Result<bool>.Success(true);
    }
}