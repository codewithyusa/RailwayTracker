using NSubstitute;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Application.Trains.Commands.UpdateTrainPosition;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Tests.Handlers;

public class UpdateTrainPositionHandlerTests
{
    [Fact]
    public async Task Handle_TrainNotFound_ReturnsFailure()
    {
        var repo = Substitute.For<ITrainRepository>();
        repo.GetByIdAsync(99, Arg.Any<CancellationToken>())
            .Returns((Train?)null);

        var handler = new UpdateTrainPositionHandler(repo);
        var result = await handler.Handle(
            new UpdateTrainPositionCommand(99, 9.0, 38.0), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("train_not_found", result.Error!.Code);

        await repo.DidNotReceive()
            .UpdatePositionAsync(Arg.Any<int>(), Arg.Any<double>(),
                Arg.Any<double>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_TrainExists_UpdatesPositionOnce()
    {
        var repo = Substitute.For<ITrainRepository>();
        var train = new Train { Id = 1, Code = "T-01", Name = "Express" };

        repo.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(train);

        var handler = new UpdateTrainPositionHandler(repo);
        var result = await handler.Handle(
            new UpdateTrainPositionCommand(1, 9.0, 38.0), CancellationToken.None);

        Assert.True(result.IsSuccess);

        await repo.Received(1)
            .UpdatePositionAsync(1, 9.0, 38.0, Arg.Any<CancellationToken>());
    }
}