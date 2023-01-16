namespace LoadTester.Tester;

public interface ITestResult
{
    int TotalCalls { get; set; }

    string ResultString { get; set; }
}
public class TimeTestResult : ITestResult
{
    public int TotalCalls { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public long AverageMillisecondsPerCall { get; set; }
    public string ResultString { get; set; }
}

public class MemoryTestResult : ITestResult
{
    public int TotalCalls { get; set; }
    public long MaxMemoryFootprint { get; set; }
    public long AverageMemoryFootprintPerCall { get; set; }

    public string ResultString { get; set; }
}

public class ComprehensiveTestResult : ITestResult
{
    public int TotalCalls { get; set; }
    public long MaxMemoryFootprint { get; set; }
    public long AverageMemoryFootprintPerCall { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public long AverageMillisecondsPerCall { get; set; }

    public string ResultString { get; set; }
}