using DiffLib;
using McMaster.Extensions.CommandLineUtils;
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
using System.Text.RegularExpressions;
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

        public string ResolvedFramework { get; private set; } = string.Empty;

        public (Framework Name, string Version) ResolvedFrameworkWithVersion { get; private set; }

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

        public void SetResolvedFramework(string framework)
        {
            ResolvedFramework = framework;
            SetResolvedFrameworkWithVersion(framework);
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
                        var frameworkInfo = frameworkInfos.FirstOrDefault();
                        var frameworkType = GetFrameworkType(frameworkInfo);
                        var frameworkVersion = GetFrameworkVersion(frameworkInfo, frameworkType);
                        switch (frameworkType)
                        {
                            case Assets.Framework.NETCoreApp:
                                ResolvedFramework = $"{frameworkType.ToString().ToLower()}{frameworkVersion}";
                                ResolvedFrameworkWithVersion = (frameworkType, frameworkVersion);
                                break;
                            case Assets.Framework.NETFramework:
                                ResolvedFramework = $"v{frameworkVersion}";
                                ResolvedFrameworkWithVersion = (frameworkType, frameworkVersion);
                                break;
                            case Assets.Framework.None:
                                ResolvedFramework = string.Empty;
                                ResolvedFrameworkWithVersion = (Assets.Framework.None, string.Empty);
                                break;
                        }
                    }
                }
            }

            static bool AllFrameworksAreEqual(IEnumerable<TargetFrameworkInformation> frameworkInfos)
            {
                var frameworkNames = frameworkInfos.Select(frameworkInfo => frameworkInfo.ToString()).ToList();
                return frameworkNames.Any()
                    && frameworkNames.All(fName => fName.Equals(frameworkNames.First(), StringComparison.OrdinalIgnoreCase));
            }

            static Framework GetFrameworkType(TargetFrameworkInformation frameworkInfo)
            {
                var frameworkName = frameworkInfo.FrameworkName.Framework.TrimStart('.');
                return frameworkName switch
                {
                    var name when name.Equals(Assets.Framework.NETCoreApp.ToString(), StringComparison.OrdinalIgnoreCase) => Assets.Framework.NETCoreApp,
                    var name when name.Equals(Assets.Framework.NETFramework.ToString(), StringComparison.OrdinalIgnoreCase) => Assets.Framework.NETFramework,
                    _ => Assets.Framework.None,
                };
            }

            static string GetFrameworkVersion(TargetFrameworkInformation frameworkInfo, Framework framework)
            {
                var major = frameworkInfo.FrameworkName.Version.Major;
                var minor = frameworkInfo.FrameworkName.Version.Minor;
                var build = frameworkInfo.FrameworkName.Version.Build;
                var version = $"{major}.{minor}";
                if (build > 0 && framework is Assets.Framework.NETFramework)
                {
                    version = $"{version}.{build}";
                }

                return version;
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

        public void SetResolvedFrameworkWithVersion(string frameworkWithVersion)
        {
            switch (frameworkWithVersion)
            {
                case var _ when string.IsNullOrEmpty(frameworkWithVersion):
                    ResolvedFrameworkWithVersion = (Assets.Framework.None, string.Empty);
                    break;
                case var _ when frameworkWithVersion.Contains(Assets.Framework.NETCoreApp.ToString(), StringComparison.OrdinalIgnoreCase):
                    var version = Regex.Split(frameworkWithVersion, Assets.Framework.NETCoreApp.ToString(), RegexOptions.IgnoreCase).Last().Trim();
                    ResolvedFrameworkWithVersion = (Assets.Framework.NETCoreApp, version);
                    break;
                case var _ when frameworkWithVersion.Contains(Assets.Framework.NETFramework.ToString(), StringComparison.OrdinalIgnoreCase):
                    version = Regex.Split(frameworkWithVersion, Assets.Framework.NETFramework.ToString(), RegexOptions.IgnoreCase).Last().Trim();
                    ResolvedFrameworkWithVersion = (Assets.Framework.NETFramework, version);
                    break;
                default:
                    ResolvedFrameworkWithVersion = (Assets.Framework.None, string.Empty);
                    break;
            }
        }

        public async Task OnExecute(
            CommandLineApplication app,
            IFileSystemService fileSystemService,
            IDotNetRunner<NewSubcommandRunnerArgs> dotNetRunner)
        {
            var getTemplateResponse = fileSystemService.TryGetTemplate(ResolvedFrameworkWithVersion.Name, out Template template);

            if (getTemplateResponse.IsSuccesful)
            {
                var (isSuccessful, message) = dotNetRunner.Run(new NewSubcommandRunnerArgs
                {
                    SolutionDirectory = ResolvedSolutionDirectory,
                    Solution = ResolvedSolution,
                    Framework = ResolvedFramework,
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
