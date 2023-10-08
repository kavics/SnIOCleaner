using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SenseNet.IO.Implementations;
using SnIOCleaner;

var host = CreateHostBuilder(args).Build();

var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Canceling...");
    cancellation.Cancel();
    e.Cancel = true;
};

await new Cleaner(Arguments.Parse(args), host.Services.GetRequiredService<ILogger<FsWriter>>()).RunAsync(cancellation.Token);

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });