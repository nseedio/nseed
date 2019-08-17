using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Subcommands.New.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Services
{
    // TODO:am Replace DotNetRunner u common project so that evryone can use that

    internal class DotNetRunner : IDotNetRunner
    {
        public (bool IsSuccesful, string Message) AddTemplate(string solutionDirectory, Template template)
        {
            var arguments = new[] { "new --install", template.Path };
            return Response(Run(solutionDirectory, arguments));
        }

        public (bool IsSuccesful, string Message) CreateProject(string solutionDirectory, string name, string framework, Template template)
        {
            var newProjectPath = Path.Combine(solutionDirectory, name);
            string[] arguments = new[] { $"new", template.Name, "-n ", name, "-o ", newProjectPath, "-f ", framework };
            return Response(Run(solutionDirectory, arguments));
        }

        public (bool IsSuccesful, string Message) RemoveTemplate(string solutionDirectory, Template template)
        {
            string[] arguments = new[] { "new --uninstall", template.Path };
            return Response(Run(solutionDirectory, arguments));
        }

        public (bool IsSuccesful, string Message) AddProjectToSolution(string solutionDirectory, string solution, string name, Template template)
        {
            var newProjectCsprojFilePath = Path.Combine(solutionDirectory, name, $"{name}.csproj");
            string[] arguments = new[] { $"sln", solution, "add", newProjectCsprojFilePath };
            return Response(Run(solutionDirectory, arguments));
        }

        public RunStatus Run(string workingDirectory, string[] arguments)
        {
            var psi = new ProcessStartInfo(DotNetExe.FullPathOrDefault(), string.Join(" ", arguments))
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var p = new Process();
            try
            {
                p.StartInfo = psi;
                p.Start();
                var output = new StringBuilder();

                var errors = new StringBuilder();

                var outputTask = ConsumeStreamReaderAsync(p.StandardOutput, output);

                var errorTask = ConsumeStreamReaderAsync(p.StandardError, errors);

                var processExited = p.WaitForExit(20000);

                if (processExited == false)
                {
                    p.Kill();
                    return new RunStatus(output.ToString(), errors.ToString(), exitCode: -1);
                }

                Task.WaitAll(outputTask, errorTask);
                return new RunStatus(output.ToString(), errors.ToString(), p.ExitCode);
            }
            finally
            {
                p.Dispose();
            }
        }

        private static async Task ConsumeStreamReaderAsync(StreamReader reader, StringBuilder lines)
        {
            await Task.Yield();
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.AppendLine(line);
            }
        }

        private (bool IsSuccesful, string Message) Response(RunStatus status)
        {
            if (status.IsSuccess)
            {
                return succesResponse;
            }

            return ErrorResponse(status.Errors);
        }

        private (bool IsSuccesful, string Message) succesResponse = (true, string.Empty);

        private (bool IsSuccesful, string Message) ErrorResponse(string message) => (false, message);
    }
}
