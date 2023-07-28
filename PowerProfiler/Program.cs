using PowerProfiler;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] <{SourceContext}> {Message:lj}{NewLine}{Exception}",
        theme: AnsiConsoleTheme.Code)
    .CreateLogger();

try
{
    Console.WriteLine("ThrottleStop PowerProfiler by Focus");
    Log.Information("PowerProfiler is started.");
    if (args.Length <= 1)
    {
        Log.Error(
            "You should provide 2 variables: 1st is for Long Power, 2nd is for Short Power." +
            "\nExample: PowerProfiler.exe 40 50");
        return;
    }

    if (!int.TryParse(args[0], out var longPower) || !int.TryParse(args[1], out var shortPower))
    {
        Log.Error("Invalid number format.");
        return;
    }

    var appManager = new AppManager();
    if (appManager.Settings is null)
        return;

    if (appManager.Settings.SetPowerLimits)
    {
        Log.Information("Setting power limits.");
        appManager.SetPowerLimits(longPower, shortPower);
    }

    if (appManager.Settings.RestartThrottleStop)
    {
        Log.Information("Restarting ThrottleStop.");
        appManager.ProcessManager.RestartThrottleStop();
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "PowerProfiler terminated unexpectedly");
}
finally
{
    Log.Information("PowerProfiler is closed.");
    Log.CloseAndFlush();
}