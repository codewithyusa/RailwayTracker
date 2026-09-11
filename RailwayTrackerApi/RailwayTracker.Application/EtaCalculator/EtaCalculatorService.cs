namespace RailwayTracker.Application.EtaCalculator;

public class EtaCalculatorService
{
    public const int MinorDelayThreshold = 5;
    public const int MajorDelayThreshold = 15;

    public DelayLevel CalculateDelayLevel(int delayMinutes)
    {
        if (delayMinutes < 0) return DelayLevel.Cancelled;
        if (delayMinutes < MinorDelayThreshold) return DelayLevel.OnTime;
        if (delayMinutes < MajorDelayThreshold) return DelayLevel.Minor;
        return DelayLevel.Major;
    }

    public DateTime CalculateEta(DateTime scheduledTime, int delayMinutes)
    {
        if (delayMinutes < 0) throw new ArgumentException("Delay cannot be negative.");
        return scheduledTime.AddMinutes(delayMinutes);
    }

    public int CalculateMinutesUntilArrival(DateTime eta, DateTime now)
    {
        var diff = (eta - now).TotalMinutes;
        return (int)Math.Max(0, Math.Ceiling(diff));
    }
}