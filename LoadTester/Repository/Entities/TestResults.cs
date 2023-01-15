namespace LoadTester.Repository.Entities;
public class TestResults
{
    public long Id { get; init; } = default!;
    public string Type { get; set; }
    public int TotalCalls { get; set; }
    public long MaxMemoryFootprint { get; set; }
    public long AverageMemoryFootprintPerCall { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public long AverageMillisecondsPerCall { get; set; }
    public string ResultString { get; set; }
    public DateTime PerformedOn { get; set; }
}
