using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Stations.Queries.GetStationArrivals;

public class GetStationArrivalsHandler
    : IRequestHandler<GetStationArrivalsQuery, Result<IEnumerable<Arrival>>>
{
    private readonly IStationRepository _repo;
    public GetStationArrivalsHandler(IStationRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<Arrival>>> Handle(
        GetStationArrivalsQuery query, CancellationToken ct)
    {
        var station = await _repo.GetByIdAsync(query.StationId, ct);
        if (station is null)
            return Result<IEnumerable<Arrival>>.Failure(
                new Error("station_not_found", $"Station {query.StationId} not found."));

        var arrivals = await _repo.GetArrivalsAsync(query.StationId, ct);
        return Result<IEnumerable<Arrival>>.Success(arrivals);
    }
}