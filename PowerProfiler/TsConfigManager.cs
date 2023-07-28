using IniParser;
using IniParser.Model;
using Serilog;

namespace PowerProfiler;

/// <summary>
/// Responsible for working with the ThrottleStop configuration file
/// </summary>
public class TsConfigManager
{
    private readonly string _filePath;
    private readonly string _longPowerKey;
    private readonly string _shortPowerKey;
    private readonly ILogger _logger;
    private readonly FileIniDataParser _parser;

    public TsConfigManager(string filePath, string longPowerKey, string shortPowerKey, FileIniDataParser parser,
        ILogger logger)
    {
        _filePath = filePath;
        _longPowerKey = longPowerKey;
        _shortPowerKey = shortPowerKey;
        _parser = parser;
        _logger = logger;
    }

    /// <summary>
    /// Replaces the Power Limit values in the ThrottleStop config file
    /// </summary>
    /// <param name="longPower">Long Power HEX representation</param>
    /// <param name="shortPower">Short Power HEX representation</param>
    public void ReplacePowerLimits(string longPower, string shortPower)
    {
        try
        {
            _logger.Information("Applying Power Limits");
            ReplaceKeyData(_longPowerKey, longPower);
            ReplaceKeyData(_shortPowerKey, shortPower);
        }
        catch (Exception ex)
        {
            _logger.Error("Error when working with ThrottleStop's config file. {message}", ex.Message);
        }
    }

    private void ReplaceKeyData(string key, string value)
    {
        var data = _parser.ReadFile(_filePath);
        var section = data.Sections.GetSectionData("ThrottleStop");

        if (section is null)
        {
            _logger.Error("Section [ThrottleStop] not found, check if ThrottleStop.ini is correct.");
            return;
        }

        if (!section.Keys.ContainsKey(key))
        {
            _logger.Error("Key {key} not found, check if ThrottleStop.ini is correct.", key);
            _logger.Information("{key} not applied", key);
            return;
        }

        section.Keys.SetKeyData(new KeyData(key) { KeyName = key, Value = value });
        _parser.WriteFile(_filePath, data);
        _logger.Information("{key} applied", key);
    }
}