using System.Diagnostics;
using Serilog;

namespace PowerProfiler;

/// <summary>
/// Responsible for working with the process
/// </summary>
public class ProcessManager
{
    private readonly ILogger _logger;
    private readonly string _processName;
    private readonly string _filePath;

    public ProcessManager(string processName, string filePath, ILogger logger)
    {
        _processName = processName;
        _filePath = filePath;
        _logger = logger;
    }

    /// <summary>
    /// Restarts the ThrottleStop process, if it was running, and restarts it.
    /// </summary>
    public void RestartThrottleStop()
    {
        _logger.Information("Closing process");
        CloseProcess(_processName);
        _logger.Information("Starting process");
        StartProcess(_filePath);
    }

    /// <summary>
    /// Closes running process
    /// </summary>
    /// <param name="processName">Process name</param>
    private void CloseProcess(string processName)
    {
        var processes = Process.GetProcessesByName(processName);

        if (processes.Length == 0)
        {
            _logger.Information("ThrottleStop process is not running, its closing skipped.");
            return;
        }

        foreach (var process in processes)
        {
            process.Kill();
            process.WaitForExit();
        }
    }

    /// <summary>
    /// Starts process as an administration
    /// </summary>
    /// <param name="processPath"></param>
    private void StartProcess(string processPath)
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
            _logger.Error("Error when trying to start a process. {message}", ex.Message);
        }
    }
}