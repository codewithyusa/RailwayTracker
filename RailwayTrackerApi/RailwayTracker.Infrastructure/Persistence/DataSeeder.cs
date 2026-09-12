using Microsoft.EntityFrameworkCore;
using RailwayTracker.Domain.Entities;
using RailwayTracker.Domain.Enums;

namespace RailwayTracker.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Trains.AnyAsync()) return;

        var trains = new List<Train>
        {
            new() { Code = "T-01", Name = "Addis Express", Status = TrainStatus.OnTime, Latitude = 9.0192, Longitude = 38.7525 },
            new() { Code = "T-02", Name = "Dire Dawa Fast", Status = TrainStatus.OnTime, Latitude = 9.6000, Longitude = 41.8661 },
            new() { Code = "T-03", Name = "Adama Shuttle", Status = TrainStatus.Delayed, Latitude = 8.5400, Longitude = 39.2700 }
        };

        var stations = new List<Station>
        {
            new() { Name = "Addis Ababa Central", Code = "AAC", Latitude = 9.0192, Longitude = 38.7525 },
            new() { Name = "Dire Dawa", Code = "DDW", Latitude = 9.6000, Longitude = 41.8661 },
            new() { Name = "Adama", Code = "ADM", Latitude = 8.5400, Longitude = 39.2700 }
        };

        await db.Trains.AddRangeAsync(trains);
        await db.Stations.AddRangeAsync(stations);
        await db.SaveChangesAsync();
    }
}