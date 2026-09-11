using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Stations.Queries.GetAllStations;

public record GetAllStationsQuery : IRequest<Result<IEnumerable<Station>>>;