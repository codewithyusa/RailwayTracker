using MediatR;
using RailwayTracker.Application.Common;

namespace RailwayTracker.Application.Trains.Commands.UpdateTrainPosition;

public record UpdateTrainPositionCommand(int TrainId, double Latitude, double Longitude)
    : IRequest<Result<bool>>;