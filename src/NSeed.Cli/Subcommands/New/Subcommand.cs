using DiffLib;
using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Subcommands.New.ValueProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NSeed.Cli.Resources.Resources;

namespace NSeed.Cli.Subcommands.New
{
    [Command("new", Description = Resources.Resources.New.CommandDescription)]
    [NewValidator]
    internal class Subcommand
    {
        [Option("-s|--solution", Description = Resources.Resources.New.SolutionDescription)]
        [SolutionDefaultValueProvider]
        public string Solution { get; private set; }

        [Option("-f|--framework", Description = Resources.Resources.New.FrameworkDescription)]
        [FrameworkDefaultValueProvider]
        public string Framework { get; private set; }

        [Option("-n|--name", Description = Resources.Resources.New.ProjectNameDescription)]
        [NameDefaultValueProvider(Resources.Resources.New.DefaultProjectName)]
        public string Name { get; private set; }

        public string ResolvedSoluiton { get; private set; } = string.Empty;

        public string ResolvedFramework { get; private set; } = string.Empty;

        public string ResolvedName { get; private set; } = string.Empty;

        public bool ResolvedSolutionIsValid { get; set; } = false;

        public void SetResolvedSolution(string solution)
        {
            ResolvedSoluiton = solution;
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
                var projectNames = dependencyGraphService.GetSolutionProjectsNames(ResolvedSoluiton).ToList();
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
                var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(ResolvedSoluiton);
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
                            if (framework.FrameworkName.Framework.Equals(CoreDotNetFramework))
                            {
                                ResolvedFramework = $"{framework.FrameworkName.Framework.ToLower().TrimStart('.')}{framework.FrameworkName.Version.Major}.{framework.FrameworkName.Version.Minor}";
                            }
                            else if (framework.FrameworkName.Framework.Equals(FullDotNetFramework))
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

        public (Resources.Framework Name, string Version) GetFrameworkWithVersion()
        {
            var frameworkWithVersion = GetFrameworkWithVersion(Resources.Framework.NETCoreApp);

            if (frameworkWithVersion.IsSuccessful)
            {
                return (Resources.Framework.NETCoreApp, frameworkWithVersion.Version);
            }

            frameworkWithVersion = GetFrameworkWithVersion(Resources.Framework.NETFramework);
            if (frameworkWithVersion.IsSuccessful)
            {
                return (Resources.Framework.NETFramework, frameworkWithVersion.Version);
            }

            return (Resources.Framework.None, string.Empty);
        }

        private (string Name, string Version, bool IsSuccessful) GetFrameworkWithVersion(Resources.Framework framework)
        {
            if (ResolvedFramework.Contains(framework.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                var parts = ResolvedFramework.Split(framework.ToString().ToLower()).ToList();
                if (!parts.IsNullOrEmpty() && parts.Count == 2)
                {
                    return (parts.First(), parts.Last(), true);
                }
            }

            return (string.Empty, string.Empty, false);
        }

        private Task OnExecute(CommandLineApplication app)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Resolved solution: " + ResolvedSoluiton);
            Console.WriteLine("Resolved Name:" + ResolvedName);
            Console.WriteLine("Resolved Framework:" + ResolvedFramework);

            return Task.CompletedTask;
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
