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

        /// <summary>
        /// Gets .sln file path.
        /// </summary>
        public string ResolvedSolution { get; private set; } = string.Empty;

        public string ResolvedSolutionDirectory { get; private set; } = string.Empty;

        public string ResolvedFramework { get; private set; } = string.Empty;

        public string ResolvedName { get; private set; } = string.Empty;

        public bool ResolvedSolutionIsValid { get; set; } = false;

        public void SetResolvedSolution(string solution)
        {
            ResolvedSolution = solution ?? string.Empty;
            if (!string.IsNullOrEmpty(solution))
            {
                ResolvedSolutionDirectory = new FileInfo(solution ?? string.Empty)?.DirectoryName ?? string.Empty;
            }
        }

        public void SetResolvedName(string name)
        {
            ResolvedName = name;
        }

        public void SetResolvedFramework(string framework)
        {
            ResolvedFramework = framework;
        }

        public void ResolveDefaultNameWithPrefix(IDependencyGraphService dependencyGraphService, string defaultName)
        {
            if (ResolvedSolutionIsValid && dependencyGraphService != null)
            {
                SetResolvedName(defaultName);
                var projectNames = dependencyGraphService.GetSolutionProjectsNames(ResolvedSolution).ToList();
                var commonPrefix = GetCommonValue(projectNames);
                if (commonPrefix.Exists())
                {
                    SetResolvedName($"{commonPrefix}.{defaultName}");
                }
            }
        }

        public void ResolveFramework(IDependencyGraphService dependencyGraphService)
        {
            if (ResolvedSolutionIsValid && dependencyGraphService != null)
            {
                var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(ResolvedSolution);
                if (dependencyGraph != null)
                {
                    var frameworks = dependencyGraph.Projects
                        .SelectMany(p => p.TargetFrameworks, (t, v) => { return v; })
                        .ToList();

                    if (frameworks.Any())
                    {
                        var frameworkNames = frameworks.Select(f => $"{f.FrameworkName.Framework}{f.FrameworkName.Version.Major}{f.FrameworkName.Version.Minor}{f.FrameworkName.Version.Build}").ToList();

                        if (frameworkNames.Any() && frameworkNames.All(fn => fn == frameworkNames.First()))
                        {
                            var framework = frameworks.FirstOrDefault();
                            if (framework.FrameworkName.Framework.Equals(Resources.DotNetCoreFramework))
                            {
                                ResolvedFramework = $"{framework.FrameworkName.Framework.ToLower().TrimStart('.')}{framework.FrameworkName.Version.Major}.{framework.FrameworkName.Version.Minor}";
                            }
                            else if (framework.FrameworkName.Framework.Equals(Resources.DotNetClassicFramework))
                            {
                                ResolvedFramework = $"v{framework.FrameworkName.Version.Major}.{framework.FrameworkName.Version.Minor}";
                                if (framework.FrameworkName.Version.Build != 0)
                                {
                                    ResolvedFramework = $"{ResolvedFramework}.{framework.FrameworkName.Version.Build}";
                                }
                            }
                        }
                    }
                }
            }
        }

        public (Framework Name, string Version) GetFrameworkWithVersion()
        {
            var frameworkWithVersion = GetFrameworkWithVersion(Assets.Framework.NETCoreApp);

            if (frameworkWithVersion.IsSuccessful)
            {
                return (Assets.Framework.NETCoreApp, frameworkWithVersion.Version);
            }

            frameworkWithVersion = GetFrameworkWithVersion(Assets.Framework.NETFramework);
            if (frameworkWithVersion.IsSuccessful)
            {
                return (Assets.Framework.NETFramework, frameworkWithVersion.Version);
            }

            return (Assets.Framework.None, string.Empty);
        }

        public Task OnExecute(
            CommandLineApplication app,
            IFileSystemService fileSystemService,
            IDotNetRunner<NewSubcommandRunnerArgs> dotNetRunner)
        {
            var getTemplateResponse = fileSystemService.TryGetTemplate(Assets.Framework.NETCoreApp, out Template template);

            if (getTemplateResponse.IsSuccesful)
            {
                var response = dotNetRunner.Run(new NewSubcommandRunnerArgs
                {
                    SolutionDirectory = ResolvedSolutionDirectory,
                    Solution = ResolvedSolution,
                    Framework = ResolvedFramework,
                    Name = ResolvedName,
                    Template = template
                });

                if (!response.IsSuccesful)
                {
                    app.Error.WriteLine(response.Message);
                }
            }
            else
            {
                app.Error.WriteLine(getTemplateResponse.Message);
            }

            fileSystemService.RemoveTempTemplates();
            return Task.CompletedTask;
        }

        private (string Name, string Version, bool IsSuccessful) GetFrameworkWithVersion(Framework framework)
        {
            if (ResolvedFramework.Contains(framework.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                var parts = ResolvedFramework.ToLower().Split(framework.ToString().ToLower()).ToList();
                if (!parts.IsNullOrEmpty() && parts.Count == 2)
                {
                    return (parts.First(), parts.Last(), true);
                }
            }

            return (string.Empty, string.Empty, false);
        }

        private string GetCommonValue(IList<string> values)
        {
            var byCharacters = new char[] { '.', '-', '_' };
            if (values.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (values.Count == 1)
            {
                return values.First().SplitAndTakeFirst(byCharacters);
            }

            var diffSection = Diff.CalculateSections(values[0].ToCharArray(), values[1].ToCharArray()).ToList();

            if (diffSection.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var firstdiffSection = diffSection.FirstOrDefault();
            if (firstdiffSection.IsMatch)
            {
                var prefix = values[0].Substring(0, firstdiffSection.LengthInCollection1).Trim('.');
                if (!string.IsNullOrEmpty(prefix))
                {
                    return prefix.SplitAndTakeFirst(byCharacters);
                }
            }

            return string.Empty;
        }
    }
}
