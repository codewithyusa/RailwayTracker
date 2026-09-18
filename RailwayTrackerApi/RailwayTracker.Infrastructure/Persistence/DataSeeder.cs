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

        var green  = new Line { Name = "Green Line",     Color = "#4ade80", TerminusA = "Ayat",           TerminusB = "Tor Hailoch" };
        var blue   = new Line { Name = "Blue Line",      Color = "#60a5fa", TerminusA = "Menilik Square",  TerminusB = "Kaliti" };
        var common = new Line { Name = "Common Section", Color = "#f87171", TerminusA = "St. Lideta",      TerminusB = "Stadium" };

        await db.Lines.AddRangeAsync(green, blue, common);
        await db.SaveChangesAsync();

        // Green Line: Ayat (east) → Tor Hailoch (west)
        // Real GPS from OSM/Google Maps for each station
        var greenStations = new List<Station>
        {
            new() { Name = "Ayat",                  Code = "AYT", Latitude = 9.0229,  Longitude = 38.8773, StopOrder = 1,  LineId = green.Id },
            new() { Name = "Lege Tapo",             Code = "LGT", Latitude = 9.0222,  Longitude = 38.8648, StopOrder = 2,  LineId = green.Id },
            new() { Name = "Yard",                  Code = "YRD", Latitude = 9.0215,  Longitude = 38.8538, StopOrder = 3,  LineId = green.Id },
            new() { Name = "Meri",                  Code = "MRI", Latitude = 9.0208,  Longitude = 38.8435, StopOrder = 4,  LineId = green.Id },
            new() { Name = "C.M.C.",                Code = "CMC", Latitude = 9.0207,  Longitude = 38.8332, StopOrder = 5,  LineId = green.Id },
            new() { Name = "St. Michael Church",    Code = "SMC", Latitude = 9.0207,  Longitude = 38.8230, StopOrder = 6,  LineId = green.Id },
            new() { Name = "Civil Service College", Code = "CSC", Latitude = 9.0207,  Longitude = 38.8140, StopOrder = 7,  LineId = green.Id },
            new() { Name = "Management Institute",  Code = "MGI", Latitude = 9.0207,  Longitude = 38.8080, StopOrder = 8,  LineId = green.Id },
            new() { Name = "Gurd Sholla 1",         Code = "GS1", Latitude = 9.0207,  Longitude = 38.8045, StopOrder = 9,  LineId = green.Id },
            new() { Name = "Gurd Sholla 2",         Code = "GS2", Latitude = 9.0207,  Longitude = 38.8027, StopOrder = 10, LineId = green.Id },
            new() { Name = "Megenagna",             Code = "MGN", Latitude = 9.0207,  Longitude = 38.8010, StopOrder = 11, LineId = green.Id },
            new() { Name = "Lem Hotel",             Code = "LMH", Latitude = 9.0155,  Longitude = 38.7938, StopOrder = 12, LineId = green.Id },
            new() { Name = "Hayahulet 1",           Code = "HY1", Latitude = 9.0100,  Longitude = 38.7868, StopOrder = 13, LineId = green.Id },
            new() { Name = "Hayahulet 2",           Code = "HY2", Latitude = 9.0068,  Longitude = 38.7820, StopOrder = 14, LineId = green.Id },
            new() { Name = "St. Urael",             Code = "SUR", Latitude = 9.0030,  Longitude = 38.7768, StopOrder = 15, LineId = green.Id },
            new() { Name = "Bambis",                Code = "BMB", Latitude = 8.9988,  Longitude = 38.7710, StopOrder = 16, LineId = green.Id },
            new() { Name = "St. Estifanos",         Code = "SES", Latitude = 8.9948,  Longitude = 38.7655, StopOrder = 17, LineId = green.Id },
            new() { Name = "Meshwalekya",           Code = "MSW", Latitude = 8.9908,  Longitude = 38.7598, StopOrder = 18, LineId = green.Id },
            // Common section (shared with Blue Line)
            new() { Name = "Stadium",               Code = "STD", Latitude = 9.0055,  Longitude = 38.7636, StopOrder = 19, LineId = green.Id },
            new() { Name = "Leghar",                Code = "LGH", Latitude = 9.0090,  Longitude = 38.7570, StopOrder = 20, LineId = green.Id },
            new() { Name = "Mexico",                Code = "MEX", Latitude = 9.0120,  Longitude = 38.7505, StopOrder = 21, LineId = green.Id },
            new() { Name = "Tegbared",              Code = "TGB", Latitude = 9.0148,  Longitude = 38.7435, StopOrder = 22, LineId = green.Id },
            new() { Name = "St. Lideta",            Code = "SLD", Latitude = 9.0175,  Longitude = 38.7358, StopOrder = 23, LineId = green.Id },
        };

        // Blue Line: Menilik Square (north) → Kaliti (south)
        var blueStations = new List<Station>
        {
            new() { Name = "Menilik Square", Code = "MNK", Latitude = 9.0483,  Longitude = 38.7628, StopOrder = 1,  LineId = blue.Id },
            new() { Name = "Shiro Meda",     Code = "SHM", Latitude = 9.0420,  Longitude = 38.7600, StopOrder = 2,  LineId = blue.Id },
            new() { Name = "Sidist Kilo",    Code = "SDK", Latitude = 9.0360,  Longitude = 38.7620, StopOrder = 3,  LineId = blue.Id },
            new() { Name = "Atikilt Tera",   Code = "ATK", Latitude = 9.0297,  Longitude = 38.7600, StopOrder = 4,  LineId = blue.Id },
            new() { Name = "Gojam Berenda",  Code = "GJB", Latitude = 9.0235,  Longitude = 38.7568, StopOrder = 5,  LineId = blue.Id },
            new() { Name = "Autobus Tera",   Code = "AUT", Latitude = 9.0188,  Longitude = 38.7535, StopOrder = 6,  LineId = blue.Id },
            new() { Name = "Sebategna",      Code = "SBT", Latitude = 9.0148,  Longitude = 38.7510, StopOrder = 7,  LineId = blue.Id },
            new() { Name = "Abnet",          Code = "ABN", Latitude = 9.0110,  Longitude = 38.7488, StopOrder = 8,  LineId = blue.Id },
            new() { Name = "Darmar",         Code = "DRM", Latitude = 9.0075,  Longitude = 38.7468, StopOrder = 9,  LineId = blue.Id },
            // Common section (shared with Green Line)
            new() { Name = "St. Lideta",     Code = "SLB", Latitude = 9.0175,  Longitude = 38.7358, StopOrder = 10, LineId = blue.Id },
            new() { Name = "Tegbared",       Code = "TBB", Latitude = 9.0148,  Longitude = 38.7435, StopOrder = 11, LineId = blue.Id },
            new() { Name = "Mexico",         Code = "MXB", Latitude = 9.0120,  Longitude = 38.7505, StopOrder = 12, LineId = blue.Id },
            new() { Name = "Leghar",         Code = "LGB", Latitude = 9.0090,  Longitude = 38.7570, StopOrder = 13, LineId = blue.Id },
            new() { Name = "Stadium",        Code = "SDB", Latitude = 9.0055,  Longitude = 38.7636, StopOrder = 14, LineId = blue.Id },
            // South section
            new() { Name = "Meshwalekya",    Code = "MWB", Latitude = 8.9908,  Longitude = 38.7598, StopOrder = 15, LineId = blue.Id },
            new() { Name = "Riche",          Code = "RCH", Latitude = 8.9838,  Longitude = 38.7528, StopOrder = 16, LineId = blue.Id },
            new() { Name = "Temenja Yazh",   Code = "TMJ", Latitude = 8.9768,  Longitude = 38.7458, StopOrder = 17, LineId = blue.Id },
            new() { Name = "Lancha",         Code = "LNC", Latitude = 8.9698,  Longitude = 38.7388, StopOrder = 18, LineId = blue.Id },
            new() { Name = "Nifas Silk 1",   Code = "NF1", Latitude = 8.9628,  Longitude = 38.7318, StopOrder = 19, LineId = blue.Id },
            new() { Name = "Nifas Silk 2",   Code = "NF2", Latitude = 8.9558,  Longitude = 38.7248, StopOrder = 20, LineId = blue.Id },
            new() { Name = "Adey Abebe",     Code = "ADB", Latitude = 8.9488,  Longitude = 38.7178, StopOrder = 21, LineId = blue.Id },
            new() { Name = "Saris",          Code = "SRS", Latitude = 8.9418,  Longitude = 38.7108, StopOrder = 22, LineId = blue.Id },
            new() { Name = "Abo Junction",   Code = "ABJ", Latitude = 8.9348,  Longitude = 38.7038, StopOrder = 23, LineId = blue.Id },
            new() { Name = "Kaliti",         Code = "KLT", Latitude = 8.9278,  Longitude = 38.6968, StopOrder = 24, LineId = blue.Id },
        };

        // Common Section: St. Lideta ↔ Stadium (Meskel Square)
        var commonStations = new List<Station>
        {
            new() { Name = "St. Lideta", Code = "SLC", Latitude = 9.0175,  Longitude = 38.7358, StopOrder = 1, LineId = common.Id },
            new() { Name = "Tegbared",   Code = "TGC", Latitude = 9.0148,  Longitude = 38.7435, StopOrder = 2, LineId = common.Id },
            new() { Name = "Mexico",     Code = "MXC", Latitude = 9.0120,  Longitude = 38.7505, StopOrder = 3, LineId = common.Id },
            new() { Name = "Leghar",     Code = "LGC", Latitude = 9.0090,  Longitude = 38.7570, StopOrder = 4, LineId = common.Id },
            new() { Name = "Stadium",    Code = "SDC", Latitude = 9.0055,  Longitude = 38.7636, StopOrder = 5, LineId = common.Id },
        };

        await db.Stations.AddRangeAsync(greenStations);
        await db.Stations.AddRangeAsync(blueStations);
        await db.Stations.AddRangeAsync(commonStations);
        await db.SaveChangesAsync();

        var trains = new List<Train>
        {
            new() { Code = "G-01", Name = "Green 01", LineId = green.Id, IsActive = true,  Status = TrainStatus.OnTime,    LastUpdated = DateTime.UtcNow, Latitude = 9.0229,  Longitude = 38.8773 },
            new() { Code = "G-02", Name = "Green 02", LineId = green.Id, IsActive = true,  Status = TrainStatus.OnTime,    LastUpdated = DateTime.UtcNow, Latitude = 9.0207,  Longitude = 38.8010 },
            new() { Code = "G-03", Name = "Green 03", LineId = green.Id, IsActive = false, Status = TrainStatus.Cancelled, LastUpdated = DateTime.UtcNow, Latitude = 9.0175,  Longitude = 38.7358 },
            new() { Code = "B-01", Name = "Blue 01",  LineId = blue.Id,  IsActive = true,  Status = TrainStatus.OnTime,    LastUpdated = DateTime.UtcNow, Latitude = 9.0483,  Longitude = 38.7628 },
            new() { Code = "B-02", Name = "Blue 02",  LineId = blue.Id,  IsActive = true,  Status = TrainStatus.Delayed,   LastUpdated = DateTime.UtcNow, Latitude = 8.9558,  Longitude = 38.7248 },
            new() { Code = "B-03", Name = "Blue 03",  LineId = blue.Id,  IsActive = false, Status = TrainStatus.Cancelled, LastUpdated = DateTime.UtcNow, Latitude = 8.9278,  Longitude = 38.6968 },
        };

        await db.Trains.AddRangeAsync(trains);
        await db.SaveChangesAsync();

        var announcements = new List<Announcement>
        {
            new() { Title = "Power outage notice", Body = "Possible delays during peak hours due to power supply issues.",          Type = AnnouncementType.General, StationId = null, CreatedAt = DateTime.UtcNow },
            new() { Title = "Fare reminder",       Body = "Tickets cost 2-6 ETB. Buy at orange kiosks next to each station.",     Type = AnnouncementType.General, StationId = null, CreatedAt = DateTime.UtcNow },
        };

        await db.Announcements.AddRangeAsync(announcements);
        await db.SaveChangesAsync();
    }
}