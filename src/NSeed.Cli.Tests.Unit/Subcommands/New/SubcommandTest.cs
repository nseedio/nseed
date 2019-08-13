using FluentAssertions;
using Moq;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New
{
    public abstract class BaseSubcommand
    {
        private readonly NewSubcommand subcommand = new NewSubcommand { ResolvedSolutionIsValid = true };
        private readonly Mock<IDependencyGraphService> mockDependencyGraphService = new Mock<IDependencyGraphService>();
        private readonly DependencyGraphSpec dependencyGraphSpec = new DependencyGraphSpec();
        private readonly List<string> projectNames = new List<string>();
        private const string SlnName = "TestSln";
        private const string DefaultProjectName = Resources.Resources.New.DefaultProjectName;

        private BaseSubcommand()
        {
            subcommand.SetResolvedSolution(SlnName);
        }

        protected BaseSubcommand GenerateDependencyGraph
        {
            get
            {
                mockDependencyGraphService
                .Setup(dgs => dgs.GenerateDependencyGraph(It.IsAny<string>()))
                .Returns(dependencyGraphSpec);
                return this;
            }
        }

        protected void GenerateSoluitonProjects(IEnumerable<string> projectNames)
        {
            mockDependencyGraphService
            .Setup(dgs => dgs.GetSolutionProjectsNames(It.IsAny<string>()))
            .Returns(projectNames);
        }

        protected BaseSubcommand WithEqualDotNetCoreProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });
            dependencyGraphSpec.AddProject(packageSpec);
            return this;
        }

        protected BaseSubcommand WithDifferentDotNetCoreProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 1, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 2, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });

            dependencyGraphSpec.AddProject(packageSpec);
            return this;
        }

        protected BaseSubcommand WithEqualFullDotNetProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });

            dependencyGraphSpec.AddProject(packageSpec);
            return this;
        }

        protected BaseSubcommand WithEqualFullDotNetProjectsBuildAndMinorVersionNotSet()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 0)) });

            dependencyGraphSpec.AddProject(packageSpec);
            return this;
        }

        public static IEnumerable<object[]> EqualPrefixes => new List<object[]>
        {
            new object[] { new List<string> { "NSeed.Web", "NSeed.Test", "NSeed.Data", "NSeed.Core" } },
            new object[] { new List<string> { "NSeed.Web", "NSeed.Web.Test", "NSeed.Data", "NSeed.Core", "NSeed.Auth" } },
            new object[] { new List<string> { "NSeed.Web" } },
        };

        public static IEnumerable<object[]> NotEqualPrefixes => new List<object[]>
        {
            new object[] { new List<string> { "Miro.NSeed.Web", "Slavko.NSeed.Web.Test", "Sanjin.NSeed.Data", "Milivoj.NSeed.Core", "Slavica.NSeed.Auth" } },
            new object[] { new List<string> { "Mirko", "Slavko", "Ivan", "Mirela", "Tihomir" }, },
            new object[] { new List<string> { "NSewed.Web", "lxSevved.Web.Test", "NpSeed.Data", "NuuzSeed.Core", "NjkSeed.Auth" } },
            new object[] { new List<string>() },
        };

        protected void ResolveFramework()
        {
            subcommand.ResolveFramework(mockDependencyGraphService.Object);
        }

        protected void ResolveDefaultNameWithPrefix()
        {
            subcommand.ResolveDefaultNameWithPrefix(mockDependencyGraphService.Object, DefaultProjectName);
        }

        public class ResolveﾠFramework : BaseSubcommand
        {
            [Fact]
            public void Resolvedﾠ2_0ﾠdotﾠnetﾠcoreﾠframeworkﾠ()
            {
                GenerateDependencyGraph.WithEqualDotNetCoreProjects();
                ResolveFramework();
                subcommand.ResolvedFramework.Should().Be("netcoreapp2.0");
            }

            [Fact]
            public void Resolvedﾠv_4_6_1ﾠfullﾠdotﾠnetﾠframeworkﾠ()
            {
                GenerateDependencyGraph.WithEqualFullDotNetProjects();
                ResolveFramework();
                subcommand.ResolvedFramework.Should().Be("v4.6.1");
            }

            [Fact]
            public void Resolvedﾠv_4_0ﾠfullﾠdotﾠnetﾠframeworkﾠ()
            {
                GenerateDependencyGraph.WithEqualFullDotNetProjectsBuildAndMinorVersionNotSet();
                ResolveFramework();
                subcommand.ResolvedFramework.Should().Be("v4.0");
            }

            [Fact]
            public void Resolvedﾠemptyﾠframeworkﾠ()
            {
                GenerateDependencyGraph.WithDifferentDotNetCoreProjects();
                ResolveFramework();
                subcommand.ResolvedFramework.Should().BeEmpty();
            }
        }

        public class ResolveﾠDefaultﾠNameﾠWithﾠPrefix : BaseSubcommand
        {
            [Theory]
            [MemberData(nameof(EqualPrefixes))]
            public void ResolvedﾠWithﾠCommonﾠPrefix(IEnumerable<string> projectNames)
            {
                GenerateSoluitonProjects(projectNames);
                ResolveDefaultNameWithPrefix();
                subcommand.ResolvedName.Should().Be($"NSeed.{DefaultProjectName}");
            }

            [Theory]
            [MemberData(nameof(NotEqualPrefixes))]
            public void ResolvedﾠWithﾠNotﾠCommonﾠPrefix(IEnumerable<string> projectNames)
            {
                GenerateSoluitonProjects(projectNames);
                ResolveDefaultNameWithPrefix();
                subcommand.ResolvedName.Should().Be(DefaultProjectName);
            }
        }
    }
}
