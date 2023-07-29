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

    (string?, string?) result = (null, null);
    if (appManager.Settings.CalculatePowerLimits)
        result = appManager.PowerLimitCalculator.Calculate(longPower, shortPower);

    if (appManager.Settings.CalculatePowerLimits && appManager.Settings.SetPowerLimits && result.Item1 is not null &&
        result.Item2 is not null)
        appManager.TsConfigManager.SetPowerLimits(result.Item1, result.Item2);

    if (appManager.Settings.RestartThrottleStop)
        appManager.ProcessManager.RestartThrottleStop();
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