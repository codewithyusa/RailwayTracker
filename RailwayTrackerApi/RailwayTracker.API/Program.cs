using RailwayTracker.Infrastructure;
using RailwayTracker.API.Hubs;
using RailwayTracker.API.Services;
using RailwayTracker.Application.Common.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(RailwayTracker.Application.Trains.Queries.GetAllTrains.GetAllTrainsHandler).Assembly));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITrainPositionBroadcaster, TrainPositionBroadcaster>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RailwayTracker.Infrastructure.Persistence.AppDbContext>();
    await RailwayTracker.Infrastructure.Persistence.DataSeeder.SeedAsync(db);
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<TrainHub>("/hubs/trains");
app.MapHub<StationHub>("/hubs/stations");
app.Run();

public partial class Program { }