using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RailwayTracker.Infrastructure.Persistence;

namespace RailwayTracker.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove every descriptor that references AppDbContext or DbContextOptions
            var toRemove = services
                .Where(d =>
                    d.ServiceType.FullName != null &&
                    (d.ServiceType == typeof(AppDbContext) ||
                     d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                     d.ImplementationType?.FullName?.Contains("Npgsql") == true ||
                     d.ImplementationFactory?.Method.DeclaringType?.FullName?.Contains("Npgsql") == true ||
                     d.ServiceType.FullName.Contains("Npgsql") ||
                     d.ServiceType.FullName.Contains("DbContextOptions")))
                .ToList();

            foreach (var d in toRemove)
                services.Remove(d);

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid()));
        });
    }
}