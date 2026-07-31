using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppLuncher.Models;

namespace AppLuncher.Services
{
    public sealed class LauncherExecutionService
    {
        public async Task ExecuteAsync(LauncherItem item, CancellationToken cancellationToken)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            foreach (LaunchAction action in item.Actions.OrderBy(value => value.Order))
            {
                cancellationToken.ThrowIfCancellationRequested();
                string programPath = Environment.ExpandEnvironmentVariables(action.ProgramPath ?? string.Empty);

                if (string.IsNullOrWhiteSpace(programPath) || !File.Exists(programPath))
                {
                    throw new FileNotFoundException(
                        string.Format("Program for step {0} was not found: {1}", action.Order, programPath),
                        programPath);
                }

                string workingDirectory = Environment.ExpandEnvironmentVariables(action.WorkingDirectory ?? string.Empty);
                if (string.IsNullOrWhiteSpace(workingDirectory))
                {
                    workingDirectory = Path.GetDirectoryName(programPath);
                }

                if (!Directory.Exists(workingDirectory))
                {
                    throw new DirectoryNotFoundException(
                        string.Format("Working directory for step {0} was not found: {1}", action.Order, workingDirectory));
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = programPath,
                    Arguments = action.Arguments ?? string.Empty,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = true
                };

                Process process;
                try
                {
                    process = Process.Start(startInfo);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(
                        string.Format("Step {0} could not start '{1}'.", action.Order, programPath),
                        exception);
                }

                if (action.WaitForExit && process != null)
                {
                    try
                    {
                        await Task.Run(
                            delegate { process.WaitForExit(); },
                            cancellationToken);
                    }
                    finally
                    {
                        process.Dispose();
                    }
                }
                else if (process != null)
                {
                    process.Dispose();
                }

                if (action.DelayAfterMs > 0)
                {
                    await Task.Delay(action.DelayAfterMs, cancellationToken);
                }
            }
        }
    }
}
