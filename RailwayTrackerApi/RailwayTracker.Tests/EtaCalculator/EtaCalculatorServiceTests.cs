using RailwayTracker.Application.EtaCalculator;

namespace RailwayTracker.Tests.EtaCalculator;

public class EtaCalculatorServiceTests
{
    private readonly EtaCalculatorService _service = new();

    [Fact]
    public void CalculateDelayLevel_ZeroDelay_ReturnsOnTime()
    {
        var result = _service.CalculateDelayLevel(0);
        Assert.Equal(DelayLevel.OnTime, result);
    }

    [Theory]
    [InlineData(0,  DelayLevel.OnTime)]
    [InlineData(4,  DelayLevel.OnTime)]
    [InlineData(5,  DelayLevel.Minor)]
    [InlineData(14, DelayLevel.Minor)]
    [InlineData(15, DelayLevel.Major)]
    [InlineData(60, DelayLevel.Major)]
    [InlineData(-1, DelayLevel.Cancelled)]
    public void CalculateDelayLevel_VariousInputs_ReturnsExpectedLevel(
        int delayMinutes, DelayLevel expected)
    {
        var result = _service.CalculateDelayLevel(delayMinutes);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateEta_AddsDelayToScheduledTime()
    {
        var scheduled = new DateTime(2026, 1, 1, 10, 0, 0);
        var result = _service.CalculateEta(scheduled, 10);
        Assert.Equal(new DateTime(2026, 1, 1, 10, 10, 0), result);
    }

    [Fact]
    public void CalculateEta_NegativeDelay_ThrowsArgumentException()
    {
        var scheduled = new DateTime(2026, 1, 1, 10, 0, 0);
        Assert.Throws<ArgumentException>(() => _service.CalculateEta(scheduled, -1));
    }

    [Fact]
    public void CalculateMinutesUntilArrival_FutureEta_ReturnsPositive()
    {
        var now = new DateTime(2026, 1, 1, 10, 0, 0);
        var eta = new DateTime(2026, 1, 1, 10, 7, 0);
        var result = _service.CalculateMinutesUntilArrival(eta, now);
        Assert.Equal(7, result);
    }

    [Fact]
    public void CalculateMinutesUntilArrival_PastEta_ReturnsZero()
    {
        var now = new DateTime(2026, 1, 1, 10, 10, 0);
        var eta = new DateTime(2026, 1, 1, 10, 0, 0);
        var result = _service.CalculateMinutesUntilArrival(eta, now);
        Assert.Equal(0, result);
    }
}