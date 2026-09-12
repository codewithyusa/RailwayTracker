using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Trains.Queries.GetTrainById;

public class GetTrainByIdHandler : IRequestHandler<GetTrainByIdQuery, Result<Train>>
{
    private readonly ITrainRepository _repo;
    public GetTrainByIdHandler(ITrainRepository repo) => _repo = repo;

    public async Task<Result<Train>> Handle(GetTrainByIdQuery query, CancellationToken ct)
    {
        var train = await _repo.GetByIdAsync(query.Id, ct);
        if (train is null)
            return Result<Train>.Failure(new Error("train_not_found", $"Train {query.Id} not found."));
        return Result<Train>.Success(train);
    }
}