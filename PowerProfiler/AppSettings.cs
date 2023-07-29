namespace PowerProfiler;

public class AppSettings
{
    public static string GeneralSection => "General";
    public bool CalculatePowerLimits { get; init; }
    public bool SetPowerLimits { get; init; }
    public bool RestartThrottleStop { get; init; }

    public static string CalculatorSection => "Calculator";
    public string LongPowerBase { get; init; } = null!;
    public string ShortPowerBase { get; init; } = null!;
    public int Step { get; init; }
    public string HexPrefix { get; init; } = null!;

    public static string ProcessSection => "Process";
    public string ProcessName { get; init; } = null!;
}