using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Stations.Queries.GetAllStations;

public class GetAllStationsHandler : IRequestHandler<GetAllStationsQuery, Result<IEnumerable<Station>>>
{
    private readonly IStationRepository _repo;
    public GetAllStationsHandler(IStationRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<Station>>> Handle(GetAllStationsQuery query, CancellationToken ct)
    {
        var stations = await _repo.GetAllAsync(ct);
        return Result<IEnumerable<Station>>.Success(stations);
    }
}