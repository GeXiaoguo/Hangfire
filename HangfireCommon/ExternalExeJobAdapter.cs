using System;
using System.Diagnostics;

namespace HangfireClient
{
    [LogEverything]
    public class ExternalExeJobAdapter
    {
        [LogEverything]
        public static void StartExecutable(string exePath, string arguments, int expectedExitCode)
        {
            var procInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                FileName = exePath,
                WorkingDirectory = Environment.CurrentDirectory,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (var proc = Process.Start(procInfo))
            {
                proc.WaitForExit();

                if (proc.ExitCode != expectedExitCode)
                    throw new ApplicationException($"{exePath} failed with ExiteCode:{proc.ExitCode}");
            }
        }
    }
}
