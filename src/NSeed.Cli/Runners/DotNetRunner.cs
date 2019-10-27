using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Runners
{
    internal abstract class DotNetRunner
    {
        protected (bool IsSuccessful, string Message) Run<T>(T arguments, params Func<T, (bool IsSuccesful, string Message)>[] commands)
        {
            foreach (var command in commands)
            {
                var (isSuccesful, message) = command(arguments);
                if (!isSuccesful)
                {
                    return ErrorResponse(message);
                }
            }

            return SuccessResponse;
        }

        /// <summary>
        /// This run method by default starts dotnet command.
        /// </summary>
        /// <param name="workingDirectory">Working directory where dotnet command will be executed.</param>
        /// <param name="arguments">dotnet command arguments.</param>
        /// <returns>Status.</returns>
        protected RunStatus RunDotNet(string workingDirectory, string[] arguments)
        {
            var psi = new ProcessStartInfo(DotNetExe.FullPathOrDefault(), string.Join(" ", arguments))
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            return Run(psi);
        }

        protected RunStatus Run(string command, string workingDirectory, string[] arguments)
        {
            var psi = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            return Run(psi);
        }

        protected void RunDotNetSeedBucket(string workingDirectory, string[] arguments)
        {
            var psi = new ProcessStartInfo(DotNetExe.FullPathOrDefault(), string.Join(" ", arguments))
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
            };

            RunSeedBucket(psi);
        }

        protected void RunSeedBucket(string command, string workingDirectory, string[] arguments)
        {
            var psi = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
            };

            RunSeedBucket(psi);
        }

        protected (bool IsSuccessful, string Message) Response(RunStatus status)
        {
            if (status.IsSuccess)
            {
                return SuccessResponse;
            }

            return ErrorResponse(status.Errors);
        }

        private static RunStatus Run(ProcessStartInfo processStartInfo)
        {
            using var process = new Process
            {
                StartInfo = processStartInfo
            };

            process.Start();

            var output = new StringBuilder();

            var errors = new StringBuilder();

            var outputTask = ConsumeStreamReaderAsync(process.StandardOutput, output);

            var errorTask = ConsumeStreamReaderAsync(process.StandardError, errors);

            Task.WaitAll(outputTask, errorTask);

            return new RunStatus(output.ToString(), errors.ToString(), process.ExitCode);
        }

        private static void RunSeedBucket(ProcessStartInfo processStartInfo)
        {
            using var process = new Process
            {
                StartInfo = processStartInfo
            };
            process.Start();
            process.WaitForExit();
        }

        private static async Task ConsumeStreamReaderAsync(StreamReader reader, StringBuilder lines)
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.AppendLine(line);
            }
        }

        private static (bool IsSuccessful, string Message) SuccessResponse => (true, string.Empty);

        private static (bool IsSuccessful, string Message) ErrorResponse(string message) => (false, message);
    }
}
