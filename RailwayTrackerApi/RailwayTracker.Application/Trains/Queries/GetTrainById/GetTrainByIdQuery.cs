using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Trains.Queries.GetTrainById;

public record GetTrainByIdQuery(int Id) : IRequest<Result<Train>>;