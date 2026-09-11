using NSubstitute;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Application.Stations.Queries.GetStationArrivals;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Tests.Handlers;

public class GetStationArrivalsHandlerTests
{
    [Fact]
    public async Task Handle_StationNotFound_ReturnsFailure()
    {
        var repo = Substitute.For<IStationRepository>();
        repo.GetByIdAsync(99, Arg.Any<CancellationToken>())
            .Returns((Station?)null);

        var handler = new GetStationArrivalsHandler(repo);
        var result = await handler.Handle(
            new GetStationArrivalsQuery(99), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("station_not_found", result.Error!.Code);
    }

    [Fact]
    public async Task Handle_StationExists_ReturnsArrivals()
    {
        var repo = Substitute.For<IStationRepository>();
        var station = new Station { Id = 1, Name = "Central", Code = "CTR" };
        var arrivals = new List<Arrival>
        {
            new() { Id = 1, StationId = 1, TrainId = 1, DelayMinutes = 0 },
            new() { Id = 2, StationId = 1, TrainId = 2, DelayMinutes = 5 }
        };

        repo.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(station);
        repo.GetArrivalsAsync(1, Arg.Any<CancellationToken>()).Returns(arrivals);

        var handler = new GetStationArrivalsHandler(repo);
        var result = await handler.Handle(
            new GetStationArrivalsQuery(1), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Count());
    }
}