using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RailwayTracker.Application.Common.Interfaces;
using RailwayTracker.Infrastructure.Persistence;
using RailwayTracker.Infrastructure.Persistence.Repositories;

namespace RailwayTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITrainRepository, TrainRepository>();
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

        return services;
    }
}