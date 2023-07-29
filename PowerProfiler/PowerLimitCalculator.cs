using System.Numerics;
using Serilog;

namespace PowerProfiler;

public class PowerLimitCalculator
{
    private readonly ILogger _logger;

    /// <summary>
    /// Base value for Long Power PL1 in HEX. This means Long Power is 0W.
    /// </summary>
    private readonly string _powerlimiteax;

    /// <summary>
    /// Base value for Short Power PL2 in HEX. This means Short Power is 0W.
    /// </summary>
    private readonly string _powerlimitedx;

    /// <summary>
    /// Prefix added to HEX
    /// </summary>
    private readonly string _hexPrefix;

    /// <summary>
    /// The value by which the decimal representation is changed. This value means the following: Step Value = 1W.
    /// </summary>
    private readonly int _step;

    public PowerLimitCalculator(string powerlimiteax, string powerlimitedx, string hexPrefix, int step, ILogger logger)
    {
        _powerlimiteax = powerlimiteax;
        _powerlimitedx = powerlimitedx;
        _hexPrefix = hexPrefix;
        _step = step;
        _logger = logger;
    }

    /// <summary>
    /// Calculates PLs
    /// </summary>
    /// <param name="longPowerLimit">Long Power Limit in Watts</param>
    /// <param name="shortPowerLimit">Short Power Limit in Watts</param>
    /// <returns>Tuple, where 1st value is Long Power in HEX, 2nd value is Short Power in HEX</returns>
    public (string?, string?) Calculate(int longPowerLimit, int shortPowerLimit)
    {
        if (longPowerLimit <= 0 || shortPowerLimit <= 0)
        {
            _logger.Error("Power limits cannot be 0 or less than 0.");
            return (null, null);
        }

        if (longPowerLimit > shortPowerLimit)
        {
            _logger.Error("Long Power cannot be greater than Short Power: {long} > {short}",
                longPowerLimit, shortPowerLimit);
            return (null, null);
        }

        _logger.Information("Calculating power limits.");

        var longHex = CalculateHex(_powerlimiteax, longPowerLimit);
        var shortHex = CalculateHex(_powerlimitedx, shortPowerLimit);

        _logger.Information("POWERLIMITEAX={long}\tPOWERLIMITEDX={short}", longHex, shortHex);

        return (longHex, shortHex);
    }

    /// <summary>
    /// Transforms PL in Watts to decimal representation
    /// </summary>
    /// <param name="powerLimit">Power Limit in Watts</param>
    /// <returns>Decimal representation</returns>
    private int ConvertWattsToDecimal(int powerLimit) =>
        powerLimit * _step;

    /// <summary>
    /// Calculates PL and returns in HEX
    /// </summary>
    /// <param name="powerLimitBase">Long or Short PL Base Value</param>
    /// <param name="powerLimit">Decimal Power Limit</param>
    /// <returns>PL in HEX</returns>
    private string CalculateHex(string powerLimitBase, int powerLimit) =>
        _hexPrefix + (BigInteger.Parse(powerLimitBase, System.Globalization.NumberStyles.HexNumber) +
                      ConvertWattsToDecimal(powerLimit)).ToString("X");
}