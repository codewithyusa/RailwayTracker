using System.Net;
using System.Net.Http.Json;

namespace RailwayTracker.Tests.Integration;

public class StationsApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public StationsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetStations_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/stations");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetStationArrivals_InvalidId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/stations/999/arrivals");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateAnnouncement_InvalidPayload_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/announcements", new
        {
            title = "",
            body = "",
            stationId = 0
        });
        Assert.True(
            response.StatusCode is HttpStatusCode.BadRequest
            or HttpStatusCode.UnprocessableEntity);
    }
}