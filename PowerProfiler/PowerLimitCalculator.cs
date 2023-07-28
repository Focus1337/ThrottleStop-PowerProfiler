using Serilog;

namespace PowerProfiler;

internal enum PowerLimitType
{
    Long,
    Short
}

public class PowerLimitCalculator
{
    private static readonly ILogger Logger = Log.ForContext<PowerLimitCalculator>();

    /// <summary>
    /// Base value for Long Power PL1. This means Long Power is 0W. HEX: 0x00DF8000
    /// </summary>
    private readonly int _longPowerBase; // POWERLIMITEAX 0x00DF8000

    /// <summary>
    /// Base value for Short Power PL2. This means Short Power is 0W. HEX: 0x00DF8000
    /// </summary>
    private readonly int _shortPowerBase; // POWERLIMITEDX 0x00438000

    /// <summary>
    /// Prefix added to HEX
    /// </summary>
    private readonly string _hexPrefix;

    /// <summary>
    /// The value by which the decimal representation is changed.
    /// </summary>
    private readonly int _step;

    public PowerLimitCalculator(int longPowerBase, int shortPowerBase, string hexPrefix, int step)
    {
        _longPowerBase = longPowerBase;
        _shortPowerBase = shortPowerBase;
        _hexPrefix = hexPrefix;
        _step = step;
    }

    /// <summary>
    /// Calculates PLs and transforms them into HEX representation.
    /// </summary>
    /// <param name="longPowerLimit">Long Power Limit in Watts</param>
    /// <param name="shortPowerLimit">Short Power Limit in Watts</param>
    /// <returns>Tuple, where 1st value is Long Power in HEX, 2nd value is Short Power in HEX</returns>
    public (string?, string?) Calculate(int longPowerLimit, int shortPowerLimit)
    {
        if (longPowerLimit <= 0 || shortPowerLimit <= 0)
        {
            Logger.Error("Power limits cannot be 0 or less than 0.");
            return (null, null);
        }

        if (longPowerLimit > shortPowerLimit)
        {
            Logger.Error("Long Power cannot be greater than Short Power: {long} > {short}",
                longPowerLimit, shortPowerLimit);
            return (null, null);
        }

        var longHex = ConvertToHex(ConvertToDecimal(longPowerLimit, PowerLimitType.Long));
        var shortHex = ConvertToHex(ConvertToDecimal(shortPowerLimit, PowerLimitType.Short));

        Logger.Information("POWERLIMITEAX={long}\tPOWERLIMITEDX={short}", longHex, shortHex);

        return (longHex, shortHex);
    }

    /// <summary>
    /// Transforms PL in Watts to decimal representation
    /// </summary>
    /// <param name="powerLimitWatts">Power Limit in Watts</param>
    /// <param name="type">Type of Power Limit</param>
    /// <returns>Decimal representation</returns>
    private int ConvertToDecimal(int powerLimitWatts, PowerLimitType type) =>
        powerLimitWatts * _step + (type == PowerLimitType.Long ? _longPowerBase : _shortPowerBase);

    /// <summary>
    /// Transforms PL in decimal representation to HEX representation.
    /// </summary>
    /// <param name="powerLimitDecimal">Power Limit in decimal representation</param>
    /// <returns>Hex representation</returns>
    private string ConvertToHex(int powerLimitDecimal) =>
        _hexPrefix + powerLimitDecimal.ToString("X");
}