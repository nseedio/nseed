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
        public (bool IsSuccesful, string Message) RunNewSubcommand(NewSubcommandArgs args)
        {
            return RunNewSubcommand(
                args,
                AddTemplate,
                CreateProject,
                RemoveTemplate,
                AddProjectToSolution);
        }

        public (bool IsSuccesful, string Message) AddTemplate(NewSubcommandArgs args)
        {
            var arguments = new[] { "new --install", args.Template.Path };
            return Response(Run(args.SolutionDirectory, arguments));
        }

        public (bool IsSuccesful, string Message) CreateProject(NewSubcommandArgs args)
        {
            var newProjectPath = Path.Combine(args.SolutionDirectory, args.Name);
            string[] arguments = new[] { $"new", args.Template.Name, "-n ", args.Name, "-o ", newProjectPath, "-f ", args.Framework };
            return Response(Run(args.SolutionDirectory, arguments));
        }

        public (bool IsSuccesful, string Message) RemoveTemplate(NewSubcommandArgs args)
        {
            string[] arguments = new[] { "new --uninstall", args.Template.Path };
            return Response(Run(args.SolutionDirectory, arguments));
        }

        public (bool IsSuccesful, string Message) AddProjectToSolution(NewSubcommandArgs args)
        {
            var newProjectCsprojFilePath = Path.Combine(args.SolutionDirectory, args.Name, $"{args.Name}.csproj");
            string[] arguments = new[] { $"sln", args.Solution, "add", newProjectCsprojFilePath };
            return Response(Run(args.SolutionDirectory, arguments));
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

        private (bool IsSuccesful, string Message) RunNewSubcommand(NewSubcommandArgs arguments, params Func<NewSubcommandArgs, (bool IsSuccesful, string Message)>[] commands)
        {
            foreach (var command in commands)
            {
                var (isSuccesful, message) = command(arguments);
                if (!isSuccesful)
                {
                    return ErrorResponse(message);
                }
            }

            return succesResponse;
        }
    }
}
