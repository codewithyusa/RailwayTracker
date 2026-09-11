using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Trains.Queries.GetAllTrains;

public record GetAllTrainsQuery : IRequest<Result<IEnumerable<Train>>>;