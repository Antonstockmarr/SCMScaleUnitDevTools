using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ScaleUnitManagement.ScaleUnitFeatureManager.Utilities
{
    public class CommandExecutor
    {
        public static string Quotes = "\"\"\"";

        public static void RunCommand(string cmd, List<int> successCodes)
        {
            var process = BuildProcess("powershell", cmd);
            RunProcess(process);

            if (!successCodes.Contains(process.ExitCode))
                throw new Exception("Command: " + cmd);
        }

        public static void RunCommand(string cmd)
        {
            var process = BuildProcess("powershell", cmd);
            RunProcess(process);

            if (process.ExitCode != 0)
                throw new Exception("Command: " + cmd);
        }

        public static void RunCommand(string executable, string arguments, string outputFile)
        {
            var process = BuildProcess(executable, arguments);
            process = AddOutputFile(process, outputFile);
            RunProcess(process);

            if (process.ExitCode != 0)
                throw new Exception("Command: " + $"{executable} {arguments} > {outputFile}");
        }

        public static void RunCommand(string executable, string arguments)
        {
            var process = BuildProcess(executable, arguments);
            RunProcess(process);

            if (process.ExitCode != 0)
                throw new Exception("Command: " + $"{executable} {arguments}");
        }

        private static Process BuildProcess(string executable, string arguments)
        {
            return new Process
            {
                StartInfo = {
                    FileName = executable,
                    Arguments = arguments
                }
            };
        }

        private static Process AddOutputFile(Process process, string outputFile)
        {
            var outputStream = new StreamWriter(outputFile);
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    outputStream.WriteLine(e.Data);
                }
            });
            return process;
        }

        private static void RunProcess(Process process)
        {
            process.Start();
            if (process.StartInfo.RedirectStandardOutput)
            {
                process.BeginOutputReadLine();
            }
            process.WaitForExit();
        }
    }
}
