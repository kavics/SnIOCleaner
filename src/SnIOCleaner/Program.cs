using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SenseNet.IO.Implementations;
using SenseNet.Tools.CommandLineArguments;
using SnIOCleaner;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    }).Build();

var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Canceling...");
    cancellation.Cancel();
    e.Cancel = true;
};


var arguments = new Arguments();
try
{
    var result = ArgumentParser.Parse(args, arguments);
    if (result.IsHelp)
        Console.WriteLine(result.GetHelpText());
    else
        await new Cleaner(arguments, host.Services.GetRequiredService<ILogger<FsWriter>>()).RunAsync(cancellation.Token);
}
catch (ParsingException e)
{
    Console.WriteLine(e.FormattedMessage);
    Console.WriteLine(e.Result.GetHelpText());
}
