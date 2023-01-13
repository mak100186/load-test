using LoadTester.Constants;

using RestSharp;

namespace LoadTester.Tester;

public static class TestStrategyFactory
{
    public static ITester? GetStrategy(string mode)
    {
        return mode switch
        {
            OperationMode.URL_GET_TIME => new TimeElapsedTester(),
            OperationMode.URL_POST_TIME => new TimeElapsedTester(),
            OperationMode.URL_PUT_TIME => new TimeElapsedTester(),
            OperationMode.URL_PATCH_TIME => new TimeElapsedTester(),
            OperationMode.URL_DELETE_TIME => new TimeElapsedTester(),
            OperationMode.URL_GET_MEMORY => new MemoryProfilingTester(),
            OperationMode.URL_POST_MEMORY => new MemoryProfilingTester(),
            OperationMode.URL_GET_COMP => new ComprehensiveTester(),
            OperationMode.URL_POST_COMP => new ComprehensiveTester(),
            _ => null
        };
    }

    public static Method? GetMethod(string mode)
    {
        return mode switch
        {
            OperationMode.URL_GET_TIME => Method.Get,
            OperationMode.URL_PUT_TIME => Method.Put,
            OperationMode.URL_POST_TIME => Method.Post,
            OperationMode.URL_DELETE_TIME => Method.Delete,
            OperationMode.URL_PATCH_TIME => Method.Patch,
            OperationMode.URL_GET_MEMORY => Method.Get,
            OperationMode.URL_POST_MEMORY => Method.Post,
            OperationMode.URL_GET_COMP => Method.Get,
            OperationMode.URL_POST_COMP => Method.Post,
            _ => null
        };
    }
}