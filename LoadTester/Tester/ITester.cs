using System.Diagnostics;

using RestSharp;

namespace LoadTester.Tester;

public interface ITester
{
    Task<ITestResult> Execute(string url, int hitCount, Action<RestClient>? clientAction = null, Action<RestClientOptions>? optionsAction = null, Action<RestRequest>? requestAction = null, Action<RestResponse>? responseAction = null);
}

public class TimeElapsedTester : ITester
{
    public async Task<ITestResult> Execute(string url, int hitCount, Action<RestClient>? clientAction = null, Action<RestClientOptions>? optionsAction = null, Action<RestRequest>? requestAction = null, Action<RestResponse>? responseAction = null)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        TimeTestResult timeTestResult = new()
        {
            TotalCalls = hitCount
        };
        
        CancellationToken cancellationToken = new();

        RestClientOptions options = new(url);
        optionsAction?.Invoke(options);

        using RestClient client = new(options);
        clientAction?.Invoke(client);

        try
        {
            await Parallel.ForEachAsync(Enumerable.Range(1, hitCount), cancellationToken, async (counter, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                RestRequest request = new();
                requestAction?.Invoke(request);

                RestResponse response = await client.ExecuteAsync(request, cancellationToken);

                responseAction?.Invoke(response);

                ct.ThrowIfCancellationRequested();
            });
        }
        finally
        {
            client.Dispose();
        }

        stopwatch.Stop();
        timeTestResult.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        timeTestResult.AverageMillisecondsPerCall = timeTestResult.ElapsedMilliseconds / hitCount;
        timeTestResult.ResultString = $"For {timeTestResult.TotalCalls} took {timeTestResult.ElapsedMilliseconds} ms at an average of {timeTestResult.AverageMillisecondsPerCall} ms per call";

        return timeTestResult;
    }
}

public class MemoryProfilingTester : ITester
{
    public async Task<ITestResult> Execute(string url, int hitCount, Action<RestClient>? clientAction = null, Action<RestClientOptions>? optionsAction = null, Action<RestRequest>? requestAction = null, Action<RestResponse>? responseAction = null)
    {
        GC.TryStartNoGCRegion(100000000); //100Mb
        long memoryBeforeCalls = GC.GetTotalMemory(false);

        MemoryTestResult memoryTestResult = new()
        {
            TotalCalls = hitCount
        };
        
        CancellationToken cancellationToken = new();

        RestClientOptions options = new(url);
        optionsAction?.Invoke(options);

        using RestClient client = new(options);
        clientAction?.Invoke(client);

        try
        {
            await Parallel.ForEachAsync(Enumerable.Range(1, hitCount), cancellationToken, async (counter, ct) =>
            {
                ct.ThrowIfCancellationRequested();
                RestRequest request = new();
                requestAction?.Invoke(request);

                RestResponse response = await client.ExecuteAsync(request, cancellationToken);

                responseAction?.Invoke(response);
                ct.ThrowIfCancellationRequested();
            });
        }
        finally
        {
            client.Dispose();
        }

        long memoryUsedByCalls = GC.GetTotalMemory(false) - memoryBeforeCalls;
        GC.EndNoGCRegion();

        memoryTestResult.MaxMemoryFootprint = memoryUsedByCalls;
        memoryTestResult.AverageMemoryFootprintPerCall = memoryTestResult.MaxMemoryFootprint / hitCount;
        memoryTestResult.ResultString = $"For {memoryTestResult.TotalCalls} took {memoryTestResult.MaxMemoryFootprint / 1024} Kb at an average of {memoryTestResult.AverageMemoryFootprintPerCall / 1024} Kb per call";

        return memoryTestResult;
    }
}

public class ComprehensiveTester : ITester
{
    public async Task<ITestResult> Execute(string url, int hitCount, Action<RestClient>? clientAction = null, Action<RestClientOptions>? optionsAction = null, Action<RestRequest>? requestAction = null, Action<RestResponse>? responseAction = null)
    {
        GC.TryStartNoGCRegion(100000000); //100Mb
        long memoryBeforeCalls = GC.GetTotalMemory(false);
        Stopwatch stopwatch = new();
        stopwatch.Start();

        ComprehensiveTestResult comprehensiveTestResult = new()
        {
            TotalCalls = hitCount
        };
        
        CancellationToken cancellationToken = new();

        RestClientOptions options = new(url);
        optionsAction?.Invoke(options);

        RestClient client = new(options);
        
        clientAction?.Invoke(client);

        try
        {
            await Parallel.ForEachAsync(Enumerable.Range(1, hitCount), cancellationToken, async (counter, ct) =>
            {
                ct.ThrowIfCancellationRequested();
                RestRequest request = new();
                requestAction?.Invoke(request);

                RestResponse response = await client.ExecuteAsync(request, cancellationToken);

                responseAction?.Invoke(response);
                ct.ThrowIfCancellationRequested();
            });
        }
        finally
        {
            client.Dispose();
        }

        long memoryUsedByCalls = GC.GetTotalMemory(false) - memoryBeforeCalls;
        GC.EndNoGCRegion();
        stopwatch.Stop();
        
        comprehensiveTestResult.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        comprehensiveTestResult.AverageMillisecondsPerCall = comprehensiveTestResult.ElapsedMilliseconds / hitCount;
        comprehensiveTestResult.MaxMemoryFootprint = memoryUsedByCalls;
        comprehensiveTestResult.AverageMemoryFootprintPerCall = comprehensiveTestResult.MaxMemoryFootprint / hitCount;
        comprehensiveTestResult.ResultString = $"For {comprehensiveTestResult.TotalCalls} calls\n" +
                                               $"took {comprehensiveTestResult.MaxMemoryFootprint / 1024} Kb at an average of {comprehensiveTestResult.AverageMemoryFootprintPerCall / 1024} Kb per call and\n" +
                                               $"took {comprehensiveTestResult.ElapsedMilliseconds} ms at an average of {comprehensiveTestResult.AverageMillisecondsPerCall} ms per call";

        return comprehensiveTestResult;
    }
}