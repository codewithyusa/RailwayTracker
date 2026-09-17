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

        // Lines
        var green  = new Line { Name = "Green Line",      Color = "#4ade80", TerminusA = "Ayat",          TerminusB = "Tor Hailoch" };
        var blue   = new Line { Name = "Blue Line",       Color = "#60a5fa", TerminusA = "Menilik Square", TerminusB = "Kaliti" };
        var common = new Line { Name = "Common Section",  Color = "#f87171", TerminusA = "St. Lideta",     TerminusB = "Stadium" };

        await db.Lines.AddRangeAsync(green, blue, common);
        await db.SaveChangesAsync();

        // Green Line stations
        var greenStations = new List<Station>
        {
            new() { Name = "Ayat",                 Code = "AYT", Latitude = 9.0227, Longitude = 38.8776, StopOrder = 1,  LineId = green.Id },
            new() { Name = "Lege Tapo",            Code = "LGT", Latitude = 9.0195, Longitude = 38.8680, StopOrder = 2,  LineId = green.Id },
            new() { Name = "Yard",                 Code = "YRD", Latitude = 9.0170, Longitude = 38.8600, StopOrder = 3,  LineId = green.Id },
            new() { Name = "Meri",                 Code = "MRI", Latitude = 9.0140, Longitude = 38.8520, StopOrder = 4,  LineId = green.Id },
            new() { Name = "C.M.C.",               Code = "CMC", Latitude = 9.0100, Longitude = 38.8450, StopOrder = 5,  LineId = green.Id },
            new() { Name = "St. Michael Church",   Code = "SMC", Latitude = 9.0060, Longitude = 38.8370, StopOrder = 6,  LineId = green.Id },
            new() { Name = "Civil Service College",Code = "CSC", Latitude = 9.0020, Longitude = 38.8290, StopOrder = 7,  LineId = green.Id },
            new() { Name = "Management Institute", Code = "MGI", Latitude = 8.9980, Longitude = 38.8210, StopOrder = 8,  LineId = green.Id },
            new() { Name = "Gurd Sholla 1",        Code = "GS1", Latitude = 8.9940, Longitude = 38.8130, StopOrder = 9,  LineId = green.Id },
            new() { Name = "Gurd Sholla 2",        Code = "GS2", Latitude = 8.9900, Longitude = 38.8050, StopOrder = 10, LineId = green.Id },
            new() { Name = "Megenagna",            Code = "MGN", Latitude = 9.0050, Longitude = 38.7980, StopOrder = 11, LineId = green.Id },
            new() { Name = "Lem Hotel",            Code = "LMH", Latitude = 9.0020, Longitude = 38.7900, StopOrder = 12, LineId = green.Id },
            new() { Name = "Hayahulet 1",          Code = "HY1", Latitude = 8.9990, Longitude = 38.7820, StopOrder = 13, LineId = green.Id },
            new() { Name = "Hayahulet 2",          Code = "HY2", Latitude = 8.9960, Longitude = 38.7740, StopOrder = 14, LineId = green.Id },
            new() { Name = "St. Urael",            Code = "SUR", Latitude = 8.9930, Longitude = 38.7660, StopOrder = 15, LineId = green.Id },
            new() { Name = "Bambis",               Code = "BMB", Latitude = 8.9900, Longitude = 38.7580, StopOrder = 16, LineId = green.Id },
            new() { Name = "St. Estifanos",        Code = "SES", Latitude = 8.9870, Longitude = 38.7500, StopOrder = 17, LineId = green.Id },
            new() { Name = "Meshwalekya",          Code = "MSW", Latitude = 8.9840, Longitude = 38.7420, StopOrder = 18, LineId = green.Id },
            new() { Name = "Stadium",              Code = "STD", Latitude = 9.0050, Longitude = 38.7630, StopOrder = 19, LineId = green.Id },
            new() { Name = "Leghar",               Code = "LGH", Latitude = 9.0080, Longitude = 38.7550, StopOrder = 20, LineId = green.Id },
            new() { Name = "Mexico",               Code = "MEX", Latitude = 9.0110, Longitude = 38.7470, StopOrder = 21, LineId = green.Id },
            new() { Name = "Tegbared",             Code = "TGB", Latitude = 9.0140, Longitude = 38.7390, StopOrder = 22, LineId = green.Id },
            new() { Name = "St. Lideta",           Code = "SLD", Latitude = 9.0170, Longitude = 38.7310, StopOrder = 23, LineId = green.Id },
        };

        // Blue Line stations
        var blueStations = new List<Station>
        {
            new() { Name = "Menilik Square",   Code = "MNK", Latitude = 9.0380, Longitude = 38.7490, StopOrder = 1,  LineId = blue.Id },
            new() { Name = "Shiro Meda",       Code = "SHM", Latitude = 9.0340, Longitude = 38.7430, StopOrder = 2,  LineId = blue.Id },
            new() { Name = "Sidist Kilo",      Code = "SDK", Latitude = 9.0300, Longitude = 38.7370, StopOrder = 3,  LineId = blue.Id },
            new() { Name = "Atikilt Tera",     Code = "ATK", Latitude = 9.0260, Longitude = 38.7310, StopOrder = 4,  LineId = blue.Id },
            new() { Name = "Gojam Berenda",    Code = "GJB", Latitude = 9.0220, Longitude = 38.7250, StopOrder = 5,  LineId = blue.Id },
            new() { Name = "Autobus Tera",     Code = "AUT", Latitude = 9.0180, Longitude = 38.7190, StopOrder = 6,  LineId = blue.Id },
            new() { Name = "Sebategna",        Code = "SBT", Latitude = 9.0140, Longitude = 38.7130, StopOrder = 7,  LineId = blue.Id },
            new() { Name = "Abnet",            Code = "ABN", Latitude = 9.0100, Longitude = 38.7070, StopOrder = 8,  LineId = blue.Id },
            new() { Name = "Darmar",           Code = "DRM", Latitude = 9.0060, Longitude = 38.7010, StopOrder = 9,  LineId = blue.Id },
            new() { Name = "St. Lideta",       Code = "SLB", Latitude = 9.0170, Longitude = 38.7310, StopOrder = 10, LineId = blue.Id },
            new() { Name = "Tegbared",         Code = "TBB", Latitude = 9.0140, Longitude = 38.7390, StopOrder = 11, LineId = blue.Id },
            new() { Name = "Mexico",           Code = "MXB", Latitude = 9.0110, Longitude = 38.7470, StopOrder = 12, LineId = blue.Id },
            new() { Name = "Leghar",           Code = "LGB", Latitude = 9.0080, Longitude = 38.7550, StopOrder = 13, LineId = blue.Id },
            new() { Name = "Stadium",          Code = "SDB", Latitude = 9.0050, Longitude = 38.7630, StopOrder = 14, LineId = blue.Id },
            new() { Name = "Meshwalekya",      Code = "MWB", Latitude = 8.9840, Longitude = 38.7420, StopOrder = 15, LineId = blue.Id },
            new() { Name = "Riche",            Code = "RCH", Latitude = 8.9800, Longitude = 38.7340, StopOrder = 16, LineId = blue.Id },
            new() { Name = "Temenja Yazh",     Code = "TMJ", Latitude = 8.9760, Longitude = 38.7260, StopOrder = 17, LineId = blue.Id },
            new() { Name = "Lancha",           Code = "LNC", Latitude = 8.9720, Longitude = 38.7180, StopOrder = 18, LineId = blue.Id },
            new() { Name = "Nifas Silk 1",     Code = "NF1", Latitude = 8.9680, Longitude = 38.7100, StopOrder = 19, LineId = blue.Id },
            new() { Name = "Nifas Silk 2",     Code = "NF2", Latitude = 8.9640, Longitude = 38.7020, StopOrder = 20, LineId = blue.Id },
            new() { Name = "Adey Abebe",       Code = "ADB", Latitude = 8.9600, Longitude = 38.6940, StopOrder = 21, LineId = blue.Id },
            new() { Name = "Saris",            Code = "SRS", Latitude = 8.9560, Longitude = 38.6860, StopOrder = 22, LineId = blue.Id },
            new() { Name = "Abo Junction",     Code = "ABJ", Latitude = 8.9520, Longitude = 38.6780, StopOrder = 23, LineId = blue.Id },
            new() { Name = "Kaliti",           Code = "KLT", Latitude = 8.9480, Longitude = 38.6700, StopOrder = 24, LineId = blue.Id },
        };

        // Common Section stations
        var commonStations = new List<Station>
        {
            new() { Name = "St. Lideta", Code = "SLC", Latitude = 9.0170, Longitude = 38.7310, StopOrder = 1, LineId = common.Id },
            new() { Name = "Tegbared",   Code = "TGC", Latitude = 9.0140, Longitude = 38.7390, StopOrder = 2, LineId = common.Id },
            new() { Name = "Mexico",     Code = "MXC", Latitude = 9.0110, Longitude = 38.7470, StopOrder = 3, LineId = common.Id },
            new() { Name = "Leghar",     Code = "LGC", Latitude = 9.0080, Longitude = 38.7550, StopOrder = 4, LineId = common.Id },
            new() { Name = "Stadium",    Code = "SDC", Latitude = 9.0050, Longitude = 38.7630, StopOrder = 5, LineId = common.Id },
        };

        await db.Stations.AddRangeAsync(greenStations);
        await db.Stations.AddRangeAsync(blueStations);
        await db.Stations.AddRangeAsync(commonStations);
        await db.SaveChangesAsync();

        // Trains
        var trains = new List<Train>
        {
            new() { Code = "G-01", Name = "Green 01", LineId = green.Id, IsActive = true,  Status = TrainStatus.OnTime, LastUpdated = DateTime.UtcNow, Latitude = 9.0227, Longitude = 38.8776 },
            new() { Code = "G-02", Name = "Green 02", LineId = green.Id, IsActive = true,  Status = TrainStatus.OnTime, LastUpdated = DateTime.UtcNow, Latitude = 9.0050, Longitude = 38.7980 },
            new() { Code = "G-03", Name = "Green 03", LineId = green.Id, IsActive = false, Status = TrainStatus.OutOfService, LastUpdated = DateTime.UtcNow, Latitude = 9.0170, Longitude = 38.7310 },
            new() { Code = "B-01", Name = "Blue 01",  LineId = blue.Id,  IsActive = true,  Status = TrainStatus.OnTime, LastUpdated = DateTime.UtcNow, Latitude = 9.0380, Longitude = 38.7490 },
            new() { Code = "B-02", Name = "Blue 02",  LineId = blue.Id,  IsActive = true,  Status = TrainStatus.Delayed, LastUpdated = DateTime.UtcNow, Latitude = 8.9900, Longitude = 38.7050 },
            new() { Code = "B-03", Name = "Blue 03",  LineId = blue.Id,  IsActive = false, Status = TrainStatus.OutOfService, LastUpdated = DateTime.UtcNow, Latitude = 8.9480, Longitude = 38.6700 },
        };

        await db.Trains.AddRangeAsync(trains);
        await db.SaveChangesAsync();

        // Announcements
        var announcements = new List<Announcement>
        {
            new() { Title = "Power outage notice", Body = "Possible delays during peak hours due to power supply issues.", Type = AnnouncementType.Warning, StationId = null, CreatedAt = DateTime.UtcNow },
            new() { Title = "Fare reminder", Body = "Tickets cost 2–6 ETB. Buy at orange kiosks next to each station.", Type = AnnouncementType.Info, StationId = null, CreatedAt = DateTime.UtcNow },
        };

        await db.Announcements.AddRangeAsync(announcements);
        await db.SaveChangesAsync();
    }
}