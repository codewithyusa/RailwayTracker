using MediatR;
using RailwayTracker.Application.Common;
using RailwayTracker.Domain.Entities;

namespace RailwayTracker.Application.Stations.Queries.GetStationArrivals;

public record GetStationArrivalsQuery(int StationId) : IRequest<Result<IEnumerable<Arrival>>>;