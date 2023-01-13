using CommandLine;

namespace LoadTester.Constants;

public class Options
{
    [Option('m', "mode", Required = true, HelpText = "Set the mode of operation", Default = OperationMode.URL_GET_TIME)]
    public string Mode { get; set; }

    [Option('u', "url", Required = true, HelpText = "Set the url for the mode of operation")]
    public string Url { get; set; }

    [Option('c', "count", Required = true, HelpText = "Set the hit count for the operation", Default = 5)]
    public string Count { get; set; }
}