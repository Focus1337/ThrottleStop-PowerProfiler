using IniParser;
using IniParser.Exceptions;
using Serilog;

namespace PowerProfiler;

public class AppManager
{
    private const string AppSettingsFile = "PowerProfiler.ini";
    private const string TsSettingsFile = "ThrottleStop.ini";
    private const string LongPowerKey = "POWERLIMITEAX";
    private const string ShortPowerKey = "POWERLIMITEDX";
    private readonly ILogger _logger;
    private readonly FileIniDataParser _parser;

    private static readonly string CurrentDirectory =
        Path.GetDirectoryName(AppContext.BaseDirectory)!;

    public readonly PowerLimitCalculator PowerLimitCalculator = null!;
    public readonly TsConfigManager TsConfigManager = null!;
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

        PowerLimitCalculator =
            new PowerLimitCalculator(Settings.LongPowerBase, Settings.ShortPowerBase,
                Settings.HexPrefix, Settings.Step, _logger.ForContext<PowerLimitCalculator>());

        TsConfigManager =
            new TsConfigManager(Path.Combine(CurrentDirectory, TsSettingsFile), LongPowerKey, ShortPowerKey,
                _parser, _logger.ForContext<TsConfigManager>());

        ProcessManager = new ProcessManager(Settings.ProcessName,
            Path.Combine(CurrentDirectory, $"{Settings.ProcessName}.exe"), _logger.ForContext<ProcessManager>());
    }

    private void SetupAppSettings()
    {
        try
        {
            var data = _parser.ReadFile(Path.Combine(CurrentDirectory, AppSettingsFile));
            Settings = new AppSettings
            {
                // General
                CalculatePowerLimits = bool.Parse(data[AppSettings.GeneralSection]["CalculatePowerLimits"]),
                SetPowerLimits = bool.Parse(data[AppSettings.GeneralSection]["SetPowerLimits"]),
                RestartThrottleStop = bool.Parse(data[AppSettings.GeneralSection]["RestartThrottleStop"]),
                // Calculator
                LongPowerBase = data[AppSettings.CalculatorSection]["LongPowerBase"],
                ShortPowerBase = data[AppSettings.CalculatorSection]["ShortPowerBase"],
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