using LoadTester.Tester;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoadTester;
public class HostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _mode;
    private readonly string _url;
    private readonly int _count;

    public HostedService(
        ILogger<HostedService> logger,
        IHostApplicationLifetime appLifetime,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration)
    {
        this._serviceScopeFactory = scopeFactory;
        this._logger = logger;
        this._config = configuration;
        try
        {
            this._mode = this._config["arguments:mode"];
            this._url = this._config["arguments:url"];
            this._count = int.Parse(this._config["arguments:count"]);
        }
        catch (Exception e)
        {
            this._logger.LogInformation("Parse error occurred that caused the parsing to stop");
            throw;
        }
        appLifetime.ApplicationStarted.Register(this.OnStarted);
        appLifetime.ApplicationStopping.Register(this.OnStopping);
        appLifetime.ApplicationStopped.Register(this.OnStopped);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = this._serviceScopeFactory.CreateScope();
        ITestService service = scope.ServiceProvider.GetRequiredService<ITestService>();
        await service.Execute(this._mode, this._url, this._count);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStarted()
    {
    }

    private void OnStopping()
    {
    }

    private void OnStopped()
    {
    }
}
