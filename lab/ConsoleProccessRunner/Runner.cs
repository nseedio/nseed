using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProccessRunner
{
    public class Runner
    {
        public void Run(string command, string workingDirectory, string[] arguments)
        {
            var psi = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                //WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                //CreateNoWindow = true,
                //RedirectStandardOutput = true,
                //RedirectStandardError = true
            };
            Run(psi);
        }

        private RunStatus Run(ProcessStartInfo processStartInfo)
        {
            var process = new Process();

            try
            {
                process.StartInfo = processStartInfo;
                

                //process.OutputDataReceived += (sender, data) =>
                //{
                //    Console.WriteLine(data.Data);
                //};

                //process.ErrorDataReceived += (sender, data) =>
                //{

                //    Console.WriteLine(data.Data);
                //};

                process.Start();

                //process.BeginErrorReadLine();
                //process.BeginOutputReadLine();

                //process.WaitForExit();

                var output = new StringBuilder();

                var errors = new StringBuilder();

                var outputTask = ConsumeStreamReaderAsync(process.StandardOutput, output);

                var errorTask = ConsumeStreamReaderAsync(process.StandardError, errors);

                Task.WaitAll(outputTask, errorTask);
                return new RunStatus(output.ToString(), errors.ToString(), process.ExitCode);
            }
            finally
            {
                process.Dispose();
            }
        }

        private static async Task ConsumeStreamReaderAsync(StreamReader reader, StringBuilder lines)
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.AppendLine(line);
            }
        }

    }
}
