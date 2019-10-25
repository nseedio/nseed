using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static NSeed.Cli.Assets.FrameworkType;

namespace NSeed.Cli.Abstractions
{
    internal class Framework : IFramework
    {
        public Framework()
        {
            Type = None;
            Version = string.Empty;
        }

        public Framework(FrameworkType type, string version)
        {
            Type = type;
            Version = version;
        }

        public Framework(TargetFrameworkInformation targetFrameworkInformation)
        {
            Type = GetType(targetFrameworkInformation);
            Version = GetVersion(targetFrameworkInformation, Type);
            Dependencies = targetFrameworkInformation?.Dependencies?.Select(d => d.Name)
                ?? Enumerable.Empty<string>();
        }

        public Framework(string name)
        {
            if (name.IsNotProvidedByUser())
            {
                Type = None;
                Version = string.Empty;
            }
            else
            {
                var framework = Regex.Split(
                    name,
                    $@"(?<={NETFramework.ToString()}|{NETCoreApp.ToString()})",
                    RegexOptions.IgnoreCase);

                Type = GetType(framework.First());
                Version = GetVersion(framework.Last(), Type);
            }
        }

        public FrameworkType Type { get; private set; }

        public string Version { get; private set; }

        public string Name => GetName();

        public bool IsDefined => Type == NETCoreApp || Type == NETFramework;

        public IEnumerable<string> Dependencies { get; private set; } = Enumerable.Empty<string>();

        private FrameworkType GetType(TargetFrameworkInformation targetFrameworkInformation)
        {
            var frameworkName = targetFrameworkInformation.FrameworkName.Framework.TrimStart('.');
            return GetType(frameworkName);
        }

        private FrameworkType GetType(string frameworkName)
        {
            return frameworkName switch
            {
                var name when string.IsNullOrEmpty(name) => None,
                var name when name.Equals(NETCoreApp.ToString(), StringComparison.OrdinalIgnoreCase) => NETCoreApp,
                var name when name.Equals(NETFramework.ToString(), StringComparison.OrdinalIgnoreCase) => NETFramework,
                _ => Undefined,
            };
        }

        private string GetVersion(TargetFrameworkInformation frameworkInfo, FrameworkType framework)
        {
            var major = frameworkInfo.FrameworkName.Version.Major;
            var minor = frameworkInfo.FrameworkName.Version.Minor;
            var build = frameworkInfo.FrameworkName.Version.Build;

            var version = $"{major}.{minor}";

            if (build > 0 && framework is FrameworkType.NETFramework)
            {
                version = $"{version}.{build}";
            }

            return GetVersion(version, framework);
        }

        private string GetVersion(string version, FrameworkType framework)
        {
            return (framework is NETCoreApp || framework is NETFramework)
                ? version
                : string.Empty;
        }

        private string GetName()
        {
            return Type switch
            {
                NETCoreApp => $"{Type.ToString().ToLower()}{Version}",
                NETFramework => $"v{Version}",
                _ => string.Empty,
            };
        }
    }
}
