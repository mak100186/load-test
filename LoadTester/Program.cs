using LoadTester;
using LoadTester.Extensions;
using LoadTester.Repository;
using LoadTester.Tester;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Serilog;

//-m URL_GET_TIME -u http://localhost:5000/discovery/countries -c 50
//-m URL_POST_TIME -u http://localhost:5000/api/Publish?messagesToSend=1 -c 50
//-m URL_GET_MEMORY -u http://localhost:5000/discovery/countries -c 50
//-m URL_POST_MEMORY -u http://localhost:5000/api/Publish?messagesToSend=1 -c 50
//-m URL_GET_COMP -u http://localhost:5000/discovery/countries -c 50
//-m URL_POST_MEMORY -u http://localhost:5000/api/Publish?messagesToSend=1 -c 50

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.SetBasePath(Directory.GetCurrentDirectory());
        configHost.AddJsonFile("appsettings.json", optional: false);
        configHost.AddEnvironmentVariables(prefix: "PREFIX_");
        configHost.AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddDbContextPool<TestResultContext>(contextBuilder =>
                contextBuilder.BuildContext(hostContext.Configuration))
            .AddEntityFrameworkNpgsql()
            .AddHostedService<HostedService>();

        services.TryAddScoped<ITestService, TestService>();
    })
    .UseSerilog((ctx, conf) =>
    {
        conf.ReadFrom.Configuration(ctx.Configuration);
        conf.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}");
        conf.WriteTo.File(path: "\\bin\\Debug\\net6.0\\log-.txt", outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}", rollingInterval: RollingInterval.Day);
    })
    .Build();

await host.RunAsync();
