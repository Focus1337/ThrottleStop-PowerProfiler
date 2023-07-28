using PowerProfiler;

var limits = args.Take(2).Select(x =>
{
    if (int.TryParse(x, out var res) == false)
        throw new InvalidOperationException("Invalid number format.");
    return res;
}).ToList();

var appManager = new ApplicationManager();
// appManager.Profiler.SetPowerLimits(limits[0], limits[1]);
appManager.ProcessManager.RestartThrottleStop();