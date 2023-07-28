using System.Diagnostics;

namespace PowerProfiler;

internal enum PowerLimitType
{
    Long,
    Short
}

public class ApplicationManager
{
    public readonly Profiler Profiler;
    public readonly ConfigFileManager ConfigFileManager;
    public readonly ProcessManager ProcessManager;

    public ApplicationManager()
    {
        Profiler = new Profiler();
        ConfigFileManager = new ConfigFileManager();
        ProcessManager = new ProcessManager();
    }
}

public class Profiler
{
    /// <summary>
    /// Base value for Long Power PL1. This means Long Power is 0W. HEX: 0x00DF8000
    /// </summary>
    private const long LongPowerBase = 14647296; // POWERLIMITEAX 0x00DF8000

    /// <summary>
    /// Base value for Short Power PL2. This means Short Power is 0W. HEX: 0x00DF8000
    /// </summary>
    private const long ShortPowerBase = 4423680; // POWERLIMITEDX 0x00438000

    /// <summary>
    /// Prefix added to HEX
    /// </summary>
    private const string HexPrefix = "0x00";

    private string _longPowerHex;
    private string _shortPowerHex;

    /// <summary>
    /// The value by which the decimal representation is changed.
    /// </summary>
    private const int Step = 8;

    /// <summary>
    /// Sets Power Limits in the configuration file
    /// </summary>
    /// <param name="longPowerLimit">Long Power Limit in Watts</param>
    /// <param name="shortPowerLimit">Short Power Limit in Watts</param>
    public void SetPowerLimits(int longPowerLimit, int shortPowerLimit)
    {
        if (longPowerLimit <= 0 || shortPowerLimit <= 0)
            throw new InvalidOperationException("Power limits cannot be 0 or less than 0");

        if (longPowerLimit > shortPowerLimit)
            throw new InvalidOperationException("Long Power cannot be higher than Short Power");

        var longDecimal = ConvertToDecimal(longPowerLimit, PowerLimitType.Long);
        var shortDecimal = ConvertToDecimal(shortPowerLimit, PowerLimitType.Short);

        _longPowerHex = ConvertToHex(longDecimal);
        _shortPowerHex = ConvertToHex(shortDecimal);
        Console.WriteLine(_longPowerHex);
        Console.WriteLine(_shortPowerHex);
    }

    /// <summary>
    /// Transforms Power Limit in Watts to decimal representation
    /// </summary>
    /// <param name="powerLimitWatts">Power Limit in Watts</param>
    /// <param name="type">Type of Power Limit</param>
    /// <returns>Decimal representation</returns>
    private static long ConvertToDecimal(int powerLimitWatts, PowerLimitType type) =>
        powerLimitWatts * Step + (type == PowerLimitType.Long ? LongPowerBase : ShortPowerBase);

    /// <summary>
    /// Transforms Power Limit in decimal representation to HEX.
    /// </summary>
    /// <param name="powerLimitDecimal">Power Limit in decimal representation</param>
    /// <returns></returns>
    private static string ConvertToHex(long powerLimitDecimal) =>
        HexPrefix + powerLimitDecimal.ToString("X");
}

public class ConfigFileManager
{
    private void Open()
    {
    }

    private void FindLine()
    {
    }
}

public class ProcessManager
{
    private const string ProcessName = "ThrottleStop";

    public void RestartThrottleStop()
    {
        Console.WriteLine("Closing process");
        CloseProcess(ProcessName);
        Thread.Sleep(5000);
        Console.WriteLine("Starting process");
        StartProcess("C:\\Users\\Focus\\Desktop\\Apps\\ThrottleStop\\ThrottleStop.exe");
    }

    static void CloseProcess(string processName)
    {
        var processes = Process.GetProcessesByName(processName);

        Console.WriteLine("Found Processes:");
        foreach (var process in processes)
        {
            Console.WriteLine(process.ProcessName);
        }

        if (processes.Length == 0)
            throw new InvalidOperationException("ThrottleStop process is not running");

        foreach (var process in processes)
        {
            process.Kill();
            process.WaitForExit();
        }
    }

    static void StartProcess(string processPath)
    {
        var process = new Process();
        process.StartInfo.FileName = processPath;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.Verb = "runas";

        try
        {
            process.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception when trying to start a process: " + ex.Message);
        }
    }
}