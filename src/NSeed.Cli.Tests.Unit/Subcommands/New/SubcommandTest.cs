using FluentAssertions;
using Moq;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using System;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New
{
    public abstract class BaseSubcommand
    {
        readonly Subcommand Subcommand = new Subcommand { ResolvedSolutionIsValid = true };
        readonly Mock<IDependencyGraphService> MockDependencyGraphService = new Mock<IDependencyGraphService>();
        readonly DependencyGraphSpec DependencyGraphSpec = new DependencyGraphSpec();
        private const string SlnName = "TestSln";

        private BaseSubcommand()
        {
            Subcommand.SetResolvedSolution(SlnName);
        }

        protected BaseSubcommand GenerateDependencyGraph
        {
            get
            {
                MockDependencyGraphService
                .Setup(dgs => dgs.GenerateDependencyGraph(It.IsAny<string>()))
                .Returns(DependencyGraphSpec);
                return this;
            }
        }

        protected BaseSubcommand WithEqualDotNetCoreProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });
            DependencyGraphSpec.AddProject(packageSpec);
            return this;
        }

        protected BaseSubcommand WithDifferentDotNetCoreProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 1, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 2, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });

            DependencyGraphSpec.AddProject(packageSpec);
            return this;
        }

        protected BaseSubcommand WithEqualFullDotNetProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });

            DependencyGraphSpec.AddProject(packageSpec);
            return this;
        }

        protected BaseSubcommand WithEqualFullDotNetProjectsBuildAndMinorVersionNotSet()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 0)) });

            DependencyGraphSpec.AddProject(packageSpec);
            return this;
        }

        protected void ResolveFramework()
        {
            Subcommand.ResolveFramework(MockDependencyGraphService.Object);
        }


        public class ResolveﾠFramework : BaseSubcommand
        {
            [Fact]
            public void Resolvedﾠ2_0ﾠdotﾠnetﾠcoreﾠframeworkﾠ()
            {
                GenerateDependencyGraph.WithEqualDotNetCoreProjects();
                ResolveFramework();
                Subcommand.ResolvedFramework.Should().Be("netcoreapp2.0");
            }

            [Fact]
            public void Resolvedﾠv_4_6_1ﾠfullﾠdotﾠnetﾠframeworkﾠ()
            {
                GenerateDependencyGraph.WithEqualFullDotNetProjects();
                ResolveFramework();
                Subcommand.ResolvedFramework.Should().Be("v4.6.1");
            }

            [Fact]
            public void Resolvedﾠv_4_0ﾠfullﾠdotﾠnetﾠframeworkﾠ()
            {
                GenerateDependencyGraph.WithEqualFullDotNetProjectsBuildAndMinorVersionNotSet();
                ResolveFramework();
                Subcommand.ResolvedFramework.Should().Be("v4.0");
            }

            [Fact]
            public void Resolvedﾠemptyﾠframeworkﾠ()
            {
                GenerateDependencyGraph.WithDifferentDotNetCoreProjects();
                ResolveFramework();
                Subcommand.ResolvedFramework.Should().BeEmpty();
            }
        }
    }
}
