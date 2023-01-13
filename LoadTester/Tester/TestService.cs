using LoadTester.Repository;
using LoadTester.Repository.Entities;

using Microsoft.Extensions.Logging;

using RestSharp;

namespace LoadTester.Tester;
public class TestService : ITestService
{
    private readonly ILogger _logger;
    private readonly TestResultContext _context;
    public TestService(TestResultContext context, ILogger<TestService> logger)
    {
        this._context = context;
        this._logger = logger;
    }
    public async Task Execute(string mode, string url, int count)
    {
        ITester? tester = TestStrategyFactory.GetStrategy(mode);
        Method? method = TestStrategyFactory.GetMethod(mode);

        int timeOut = 1000 * (count / 10);

        try
        {
            if (tester != null && method != null)
            {
                this._logger.LogInformation($"Test started at:{DateTime.Now}");
                ITestResult testResult = await tester.Execute(url, count,
                    options =>
                    {
                        options.ThrowOnAnyError = true;
                        options.MaxTimeout = timeOut;
                    },
                    request =>
                    {
                        request.Method = method.Value;
                    },
                    response =>
                    {
                        if (!response.IsSuccessful)
                        {
                            this._logger.LogError($"Error:{response.ErrorMessage}");
                        }
                    });

                this._logger.LogInformation($"Test ended at:{DateTime.Now}");
                this._logger.LogInformation(testResult.ResultString);

                this._logger.LogInformation($"Saving..");


                TestResults dbEntry = new()
                {
                    TotalCalls = testResult.TotalCalls,
                    ResultString = testResult.ResultString,
                    Type = testResult.GetType().ToString()
                };

                switch (testResult)
                {
                    case ComprehensiveTestResult comprehensiveTestResult:
                        dbEntry.MaxMemoryFootprint = comprehensiveTestResult.MaxMemoryFootprint;
                        dbEntry.AverageMemoryFootprintPerCall = comprehensiveTestResult.AverageMemoryFootprintPerCall;
                        dbEntry.ElapsedMilliseconds = comprehensiveTestResult.ElapsedMilliseconds;
                        dbEntry.AverageMillisecondsPerCall = comprehensiveTestResult.AverageMillisecondsPerCall;
                        break;
                    case TimeTestResult timeTestResult:
                        dbEntry.ElapsedMilliseconds = timeTestResult.ElapsedMilliseconds;
                        dbEntry.AverageMillisecondsPerCall = timeTestResult.AverageMillisecondsPerCall;
                        break;
                    case MemoryTestResult memoryTestResult:
                        dbEntry.MaxMemoryFootprint = memoryTestResult.MaxMemoryFootprint;
                        dbEntry.AverageMemoryFootprintPerCall = memoryTestResult.AverageMemoryFootprintPerCall;
                        break;
                    default:
                        this._logger.LogError($"Cannot save result. Unknown kind");
                        break;
                }

                this._context.TestResults.Add(dbEntry);
                await this._context.SaveChangesAsync();

                this._logger.LogInformation("Saved");
            }
            else
            {
                this._logger.LogInformation($"Couldn't load tester and/or method. Check params: {url}, {mode}, {count}");
            }
        }
        catch (Exception e)
        {
            this._logger.LogError($"Unsuccessful test run due to: {e.Message}");
            Environment.Exit(1);
        }
    }
}

public interface ITestService
{
    Task Execute(string a, string b, int c);
}