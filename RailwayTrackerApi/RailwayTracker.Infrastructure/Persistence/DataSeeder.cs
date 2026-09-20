using Microsoft.EntityFrameworkCore;
using RailwayTracker.Domain.Entities;
using RailwayTracker.Domain.Enums;

namespace RailwayTracker.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();
        if (await db.Lines.AnyAsync()) return;

        var green  = new Line { Name = "Green Line",     Color = "#4ade80", TerminusA = "Ayat",             TerminusB = "Torhailoch" };
        var blue   = new Line { Name = "Blue Line",      Color = "#60a5fa", TerminusA = "Menelik II Square", TerminusB = "Kality" };
        var common = new Line { Name = "Common Section", Color = "#f87171", TerminusA = "Tegbared",          TerminusB = "Stadium" };

        await db.Lines.AddRangeAsync(green, blue, common);
        await db.SaveChangesAsync();

        // Green Line: Torhailoch (west) → Ayat (east)
        var greenStations = new List<Station>
        {
            new() { Name = "Torhailoch",             Code = "TOR", Latitude = 8.9956, Longitude = 38.7142, StopOrder = 1,  LineId = green.Id },
            new() { Name = "CocaCola",               Code = "COC", Latitude = 8.9991, Longitude = 38.7242, StopOrder = 2,  LineId = green.Id },
            new() { Name = "St. Lideta",             Code = "SLD", Latitude = 9.0035, Longitude = 38.7358, StopOrder = 3,  LineId = green.Id },
            new() { Name = "Tegbared",               Code = "TGB", Latitude = 9.0068, Longitude = 38.7435, StopOrder = 4,  LineId = green.Id },
            new() { Name = "Mexico",                 Code = "MEX", Latitude = 9.0098, Longitude = 38.7505, StopOrder = 5,  LineId = green.Id },
            new() { Name = "Leghar",                 Code = "LGH", Latitude = 9.0122, Longitude = 38.7570, StopOrder = 6,  LineId = green.Id },
            new() { Name = "Stadium",                Code = "STD", Latitude = 9.0145, Longitude = 38.7636, StopOrder = 7,  LineId = green.Id },
            new() { Name = "St. Estraos",            Code = "SES", Latitude = 9.0155, Longitude = 38.7690, StopOrder = 8,  LineId = green.Id },
            new() { Name = "Bambis",                 Code = "BMB", Latitude = 9.0163, Longitude = 38.7745, StopOrder = 9,  LineId = green.Id },
            new() { Name = "St. Urael",              Code = "SUR", Latitude = 9.0171, Longitude = 38.7800, StopOrder = 10, LineId = green.Id },
            new() { Name = "Hayahulet 2",            Code = "HY2", Latitude = 9.0178, Longitude = 38.7855, StopOrder = 11, LineId = green.Id },
            new() { Name = "Hayahulet 1",            Code = "HY1", Latitude = 9.0185, Longitude = 38.7910, StopOrder = 12, LineId = green.Id },
            new() { Name = "Lem Hotel",              Code = "LMH", Latitude = 9.0192, Longitude = 38.7965, StopOrder = 13, LineId = green.Id },
            new() { Name = "Megenagna",              Code = "MGN", Latitude = 9.0200, Longitude = 38.8010, StopOrder = 14, LineId = green.Id },
            new() { Name = "Gurd Shola 2",           Code = "GS2", Latitude = 9.0200, Longitude = 38.8055, StopOrder = 15, LineId = green.Id },
            new() { Name = "Gurd Shola 1",           Code = "GS1", Latitude = 9.0200, Longitude = 38.8100, StopOrder = 16, LineId = green.Id },
            new() { Name = "Management Institute",   Code = "MGI", Latitude = 9.0200, Longitude = 38.8145, StopOrder = 17, LineId = green.Id },
            new() { Name = "Civil Service College",  Code = "CSC", Latitude = 9.0200, Longitude = 38.8190, StopOrder = 18, LineId = green.Id },
            new() { Name = "St. Michael",            Code = "SMC", Latitude = 9.0200, Longitude = 38.8235, StopOrder = 19, LineId = green.Id },
            new() { Name = "CMC",                    Code = "CMC", Latitude = 9.0200, Longitude = 38.8332, StopOrder = 20, LineId = green.Id },
            new() { Name = "Meri",                   Code = "MRI", Latitude = 9.0208, Longitude = 38.8435, StopOrder = 21, LineId = green.Id },
            new() { Name = "Ayat",                   Code = "AYT", Latitude = 9.0229, Longitude = 38.8773, StopOrder = 22, LineId = green.Id },
        };

        // Blue Line: Menelik II Square (north) → Kality (south)
        var blueStations = new List<Station>
        {
            new() { Name = "Menelik II Square", Code = "MNK", Latitude = 9.0483, Longitude = 38.7468, StopOrder = 1,  LineId = blue.Id },
            new() { Name = "Atikilt Tera",      Code = "ATK", Latitude = 9.0421, Longitude = 38.7471, StopOrder = 2,  LineId = blue.Id },
            new() { Name = "Gojam Berenda",     Code = "GJB", Latitude = 9.0360, Longitude = 38.7474, StopOrder = 3,  LineId = blue.Id },
            new() { Name = "Autobus Tera",      Code = "AUT", Latitude = 9.0295, Longitude = 38.7478, StopOrder = 4,  LineId = blue.Id },
            new() { Name = "Sebategna",         Code = "SBT", Latitude = 9.0230, Longitude = 38.7435, StopOrder = 5,  LineId = blue.Id },
            new() { Name = "Abnet",             Code = "ABN", Latitude = 9.0165, Longitude = 38.7400, StopOrder = 6,  LineId = blue.Id },
            new() { Name = "Darmar",            Code = "DRM", Latitude = 9.0100, Longitude = 38.7370, StopOrder = 7,  LineId = blue.Id },
            // Common section with Green Line
            new() { Name = "Tegbared",          Code = "TBB", Latitude = 9.0068, Longitude = 38.7435, StopOrder = 8,  LineId = blue.Id },
            new() { Name = "Mexico",            Code = "MXB", Latitude = 9.0098, Longitude = 38.7505, StopOrder = 9,  LineId = blue.Id },
            new() { Name = "Leghar",            Code = "LGB", Latitude = 9.0122, Longitude = 38.7570, StopOrder = 10, LineId = blue.Id },
            new() { Name = "Stadium",           Code = "SDB", Latitude = 9.0145, Longitude = 38.7636, StopOrder = 11, LineId = blue.Id },
            // South section — straight south
            new() { Name = "Meshwlekya",        Code = "MSH", Latitude = 9.0080, Longitude = 38.7636, StopOrder = 12, LineId = blue.Id },
            new() { Name = "Riche",             Code = "RCH", Latitude = 9.0010, Longitude = 38.7636, StopOrder = 13, LineId = blue.Id },
            new() { Name = "Temenja Yazh",      Code = "TMJ", Latitude = 8.9940, Longitude = 38.7636, StopOrder = 14, LineId = blue.Id },
            new() { Name = "Lancha",            Code = "LNC", Latitude = 8.9870, Longitude = 38.7636, StopOrder = 15, LineId = blue.Id },
            new() { Name = "Nefas Silk 2",      Code = "NF2", Latitude = 8.9800, Longitude = 38.7636, StopOrder = 16, LineId = blue.Id },
            new() { Name = "Nefas Silk 1",      Code = "NF1", Latitude = 8.9730, Longitude = 38.7636, StopOrder = 17, LineId = blue.Id },
            new() { Name = "Adey Ababa",        Code = "ADB", Latitude = 8.9660, Longitude = 38.7636, StopOrder = 18, LineId = blue.Id },
            new() { Name = "Saris",             Code = "SRS", Latitude = 8.9590, Longitude = 38.7636, StopOrder = 19, LineId = blue.Id },
            new() { Name = "Abo Junction",      Code = "ABJ", Latitude = 8.9510, Longitude = 38.7636, StopOrder = 20, LineId = blue.Id },
            new() { Name = "Kality",            Code = "KLT", Latitude = 8.9430, Longitude = 38.7636, StopOrder = 21, LineId = blue.Id },
        };

        // Common Section: Tegbared ↔ Stadium
        var commonStations = new List<Station>
        {
            new() { Name = "Tegbared", Code = "TGC", Latitude = 9.0068, Longitude = 38.7435, StopOrder = 1, LineId = common.Id },
            new() { Name = "Mexico",   Code = "MXC", Latitude = 9.0098, Longitude = 38.7505, StopOrder = 2, LineId = common.Id },
            new() { Name = "Leghar",   Code = "LGC", Latitude = 9.0122, Longitude = 38.7570, StopOrder = 3, LineId = common.Id },
            new() { Name = "Stadium",  Code = "SDC", Latitude = 9.0145, Longitude = 38.7636, StopOrder = 4, LineId = common.Id },
        };

        await db.Stations.AddRangeAsync(greenStations);
        await db.Stations.AddRangeAsync(blueStations);
        await db.Stations.AddRangeAsync(commonStations);
        await db.SaveChangesAsync();

        var trains = new List<Train>
        {
            new() { Code = "G-01", Name = "Green 01", LineId = green.Id, IsActive = true,  Status = TrainStatus.OnTime,    LastUpdated = DateTime.UtcNow, Latitude = 9.0229, Longitude = 38.8773 },
            new() { Code = "G-02", Name = "Green 02", LineId = green.Id, IsActive = true,  Status = TrainStatus.OnTime,    LastUpdated = DateTime.UtcNow, Latitude = 9.0200, Longitude = 38.8010 },
            new() { Code = "G-03", Name = "Green 03", LineId = green.Id, IsActive = false, Status = TrainStatus.Cancelled, LastUpdated = DateTime.UtcNow, Latitude = 8.9956, Longitude = 38.7142 },
            new() { Code = "B-01", Name = "Blue 01",  LineId = blue.Id,  IsActive = true,  Status = TrainStatus.OnTime,    LastUpdated = DateTime.UtcNow, Latitude = 9.0483, Longitude = 38.7468 },
            new() { Code = "B-02", Name = "Blue 02",  LineId = blue.Id,  IsActive = true,  Status = TrainStatus.Delayed,   LastUpdated = DateTime.UtcNow, Latitude = 8.9800, Longitude = 38.7636 },
            new() { Code = "B-03", Name = "Blue 03",  LineId = blue.Id,  IsActive = false, Status = TrainStatus.Cancelled, LastUpdated = DateTime.UtcNow, Latitude = 8.9430, Longitude = 38.7636 },
        };

        await db.Trains.AddRangeAsync(trains);
        await db.SaveChangesAsync();

        var announcements = new List<Announcement>
        {
            new() { Title = "Power outage notice", Body = "Possible delays during peak hours due to power supply issues.", Type = AnnouncementType.General, StationId = null, CreatedAt = DateTime.UtcNow },
            new() { Title = "Fare reminder",       Body = "Tickets cost 2-6 ETB. Buy at orange kiosks next to each station.", Type = AnnouncementType.General, StationId = null, CreatedAt = DateTime.UtcNow },
        };

        await db.Announcements.AddRangeAsync(announcements);
        await db.SaveChangesAsync();
    }
}