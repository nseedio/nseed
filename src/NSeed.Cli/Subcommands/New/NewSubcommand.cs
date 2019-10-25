using DiffLib;
using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Runners;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New.Models;
using NSeed.Cli.Subcommands.New.Runner;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Subcommands.New.ValueProviders;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.New
{
    [Command("new", Description = Resources.New.CommandDescription)]
    [NewValidator]
    internal class NewSubcommand : BaseCommand
    {
        [Option("-s|--solution", Description = Resources.New.SolutionDescription)]
        [SolutionDefaultValueProvider]
        public string Solution { get; private set; } = string.Empty;

        [Option("-f|--framework", Description = Resources.New.FrameworkDescription)]
        [FrameworkDefaultValueProvider]
        public string Framework { get; private set; } = string.Empty;

        [Option("-n|--name", Description = Resources.New.ProjectNameDescription)]
        [NameDefaultValueProvider(Resources.New.DefaultProjectName)]
        public string Name { get; private set; } = string.Empty;

        public string ResolvedSolution { get; private set; } = string.Empty;

        public string ResolvedSolutionDirectory => ResolvedSolution.Exists()
            ? new FileInfo(ResolvedSolution)?.DirectoryName ?? string.Empty
            : string.Empty;

        public IFramework ResolvedFramework { get; private set; } = new Framework();

        public string ResolvedName { get; private set; } = string.Empty;

        public bool IsValidResolvedSolution { get; private set; } = false;

        public string ResolvedSolutionErrorMessage { get; private set; } = string.Empty;

        public void SetResolvedSolution(string solution)
        {
            ResolvedSolution = solution;
        }

        public void SetResolvedName(string name)
        {
            ResolvedName = name;
        }

        public void SetResolvedFramework(string frameworkName)
        {
            ResolvedFramework = new Framework(frameworkName);
        }

        public void ResolveDefaultNameWithPrefix(IDependencyGraphService dependencyGraphService, string defaultName)
        {
            if (dependencyGraphService != null)
            {
                SetResolvedName(defaultName);
                var projectNames = dependencyGraphService
                    .GetSolutionProjectsNames(ResolvedSolution).ToList();
                var commonPrefix = GetCommonValue(projectNames);
                if (commonPrefix.Exists() && !commonPrefix.Equals(defaultName, StringComparison.OrdinalIgnoreCase))
                {
                    SetResolvedName($"{commonPrefix}.{defaultName}");
                }
            }
        }

        public void ResolveFramework(IDependencyGraphService dependencyGraphService)
        {
            if (dependencyGraphService != null)
            {
                var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(ResolvedSolution);
                if (dependencyGraph != null)
                {
                    var frameworkInfos = dependencyGraph.Projects
                        ?.SelectMany(p => p.TargetFrameworks, (packageSpec, targetFrameworkInfo) => { return targetFrameworkInfo; })
                        ?.ToList() ?? new List<TargetFrameworkInformation>();

                    if (AllFrameworksAreEqual(frameworkInfos))
                    {
                        ResolvedFramework = new Framework(frameworkInfos.FirstOrDefault());
                    }
                }
            }

            static bool AllFrameworksAreEqual(IEnumerable<TargetFrameworkInformation> frameworkInfos)
            {
                var frameworkNames = frameworkInfos.Select(frameworkInfo => frameworkInfo.ToString()).ToList();
                return frameworkNames.Any()
                    && frameworkNames.All(fName => fName.Equals(frameworkNames.First(), StringComparison.OrdinalIgnoreCase));
            }
        }

        public void ResolvedSolutionIsValid()
        {
            IsValidResolvedSolution = true;
        }

        public void SetResolvedSolutionErrorMessage(string errorMessage)
        {
            ResolvedSolutionErrorMessage = errorMessage;
        }

        public async Task OnExecute(
            CommandLineApplication app,
            IFileSystemService fileSystemService,
            IDotNetRunner<NewSubcommandRunnerArgs> dotNetRunner)
        {
            var getTemplateResponse = fileSystemService.TryGetTemplate(ResolvedFramework.Type, out Template template);

            if (getTemplateResponse.IsSuccesful)
            {
                var (isSuccessful, message) = dotNetRunner.Run(new NewSubcommandRunnerArgs
                {
                    SolutionDirectory = ResolvedSolutionDirectory,
                    Solution = ResolvedSolution,
                    Framework = ResolvedFramework.Name,
                    Name = ResolvedName,
                    Template = template
                });

                if (!isSuccessful)
                {
                    await app.Error.WriteLineAsync(message);
                }
            }
            else
            {
                await app.Error.WriteLineAsync(getTemplateResponse.Message);
            }

            fileSystemService.RemoveTempTemplates();
            await app.Out.WriteLineAsync(Resources.New.SuccessfulRun);
        }

        private string GetCommonValue(IList<string> values)
        {
            var byCharacters = new char[] { '.', '-', '_' };

            if (!values.Any())
            {
                return string.Empty;
            }

            if (values.Count == 1)
            {
                return values.First().TakeFirstOrEmpty(byCharacters);
            }

            var diffSection = Diff.CalculateSections(values[0].ToCharArray(), values[1].ToCharArray())
                ?.ToList() ?? new List<DiffSection>();

            if (!diffSection.Any())
            {
                return string.Empty;
            }

            var firstdiffSection = diffSection.FirstOrDefault();
            if (firstdiffSection.IsMatch)
            {
                var prefix = values[0].Substring(0, firstdiffSection.LengthInCollection1).Trim('.');
                if (!string.IsNullOrEmpty(prefix))
                {
                    return prefix.TakeFirstOrEmpty(byCharacters);
                }
            }

            return string.Empty;
        }
    }
}
