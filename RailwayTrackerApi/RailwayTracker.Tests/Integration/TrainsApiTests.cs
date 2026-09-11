using System.Net;

namespace RailwayTracker.Tests.Integration;

public class TrainsApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TrainsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTrains_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/trains");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}