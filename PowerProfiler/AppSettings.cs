namespace PowerProfiler;

public class AppSettings
{
    public static string GeneralSection => "General";
    public bool SetPowerLimits { get; init; }
    public bool RestartThrottleStop { get; init; }

    public static string CalculatorSection => "Calculator";
    public int LongPowerBase { get; init; }
    public int ShortPowerBase { get; init; }
    public int Step { get; init; }
    public string HexPrefix { get; init; } = null!;

    public static string ProcessSection => "Process";
    public string ProcessName { get; init; } = null!;
}