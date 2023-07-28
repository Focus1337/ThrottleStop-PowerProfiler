using IniParser;
using IniParser.Exceptions;
using Serilog;

namespace PowerProfiler;

public class AppManager
{
    private const string AppSettingsFile = "PowerProfiler.ini";
    private const string TsSettingsFile = "ThrottleStop.ini";
    private const string LongPowerLine = "POWERLIMITEAX";
    private const string ShortPowerLine = "POWERLIMITEDX";
    private readonly ILogger _logger;
    private readonly FileIniDataParser _parser;

    private static readonly string CurrentDirectory =
        Path.GetDirectoryName(AppContext.BaseDirectory)!;

    private readonly PowerLimitCalculator _powerLimitCalculator = null!;
    private readonly TsConfigManager _tsConfigManager = null!;
    public readonly ProcessManager ProcessManager = null!;
    public AppSettings? Settings;

    public AppManager()
    {
        _logger = Log.ForContext<AppManager>();

        _parser = new FileIniDataParser();
        _parser.Parser.Configuration.AssigmentSpacer = "";

        SetupAppSettings();
        if (Settings is null)
        {
            _logger.Error("Failed to set values from the configuration file.");
            return;
        }

        _powerLimitCalculator = new PowerLimitCalculator(Settings.LongPowerBase, Settings.ShortPowerBase,
            Settings.HexPrefix, Settings.Step);

        _tsConfigManager =
            new TsConfigManager(Path.Combine(CurrentDirectory, TsSettingsFile), LongPowerLine, ShortPowerLine,
                _parser, _logger.ForContext<TsConfigManager>());

        ProcessManager = new ProcessManager(Settings.ProcessName,
            Path.Combine(CurrentDirectory, $"{Settings.ProcessName}.exe"), _logger.ForContext<ProcessManager>());
    }

    /// <summary>
    /// Sets Power Limits in the ThrottleStop's configuration file
    /// </summary>
    /// <param name="longPowerLimit">Long Power Limit in Watts</param>
    /// <param name="shortPowerLimit">Short Power Limit in Watts</param>
    public void SetPowerLimits(int longPowerLimit, int shortPowerLimit)
    {
        var result = _powerLimitCalculator.Calculate(longPowerLimit, shortPowerLimit);

        if (result.Item1 is null || result.Item2 is null)
            return;

        _tsConfigManager.ReplacePowerLimits(result.Item1, result.Item2);
    }

    private void SetupAppSettings()
    {
        try
        {
            var data = _parser.ReadFile(Path.Combine(CurrentDirectory, AppSettingsFile));
            Settings = new AppSettings
            {
                // General
                SetPowerLimits = bool.Parse(data[AppSettings.GeneralSection]["SetPowerLimits"]),
                RestartThrottleStop = bool.Parse(data[AppSettings.GeneralSection]["RestartThrottleStop"]),
                // Calculator
                LongPowerBase = int.Parse(data[AppSettings.CalculatorSection]["LongPowerBase"]),
                ShortPowerBase = int.Parse(data[AppSettings.CalculatorSection]["ShortPowerBase"]),
                Step = int.Parse(data[AppSettings.CalculatorSection]["Step"]),
                HexPrefix = data[AppSettings.CalculatorSection]["HexPrefix"],
                // Process
                ProcessName = data[AppSettings.ProcessSection]["ProcessName"]
            };
        }
        catch (Exception ex)
        {
            if (ex.InnerException is FileNotFoundException)
                _logger.Error("There is no PowerProfiler.ini configuration file in the directory.");

            if (ex.InnerException is ParsingException)
                _logger.Error("Error when reading config file PowerProfiler.ini.");
        }
    }
}