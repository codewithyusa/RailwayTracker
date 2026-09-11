using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Trains.Queries.GetAllTrains;

public class GetAllTrainsHandler : IRequestHandler<GetAllTrainsQuery, Result<IEnumerable<Train>>>
{
    private readonly ITrainRepository _repo;
    public GetAllTrainsHandler(ITrainRepository repo) => _repo = repo;

    public async Task<Result<IEnumerable<Train>>> Handle(GetAllTrainsQuery query, CancellationToken ct)
    {
        var trains = await _repo.GetAllAsync(ct);
        return Result<IEnumerable<Train>>.Success(trains);
    }
}