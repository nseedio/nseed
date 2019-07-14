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
    public class SubcommandTest
    {
        Subcommand Subcommand = new Subcommand { ResolvedSolutionIsValid = true };
        Mock<IDependencyGraphService> MockDependencyGraphService = new Mock<IDependencyGraphService>();

        public SubcommandTest()
        {
            Subcommand.SetResolvedSolution("TestSln");
        }
       
        public void WithEqualDotNetCoreProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });

            GenerateDependencyGraph(packageSpec);
        }

        public void WithDifferentDotNetCoreProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 1, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 2, 0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETCoreApp", new Version(2, 0, 0)) });

            GenerateDependencyGraph(packageSpec);
        }

        public void WithEqualFullDotNetProjects()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4, 6, 1)) });

            GenerateDependencyGraph(packageSpec);
        }

        public void WithEqualFullDotNetProjectsBuildAndMinorVersionNotSet()
        {
            var packageSpec = new PackageSpec();
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4,0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4,0)) });
            packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = new NuGetFramework(".NETFramework", new Version(4,0)) });

            GenerateDependencyGraph(packageSpec);
        }

        //Different dotnet core projects -- different versions
        //Different full framework projects -- different versions
        //Mixture full framework and core framework 

        private void GenerateDependencyGraph(PackageSpec packageSpec)
        {
            Mock<DependencyGraphSpec> mockDependencyGraphSpec = new Mock<DependencyGraphSpec>();
            var dependencyGraphSpec = mockDependencyGraphSpec.Object;
            dependencyGraphSpec.AddProject(packageSpec);
            MockDependencyGraphService
                .Setup(dgs => dgs.GenerateDependencyGraph(It.IsAny<string>()))
                .Returns(dependencyGraphSpec);
        }

        public class ResolveﾠFramework : SubcommandTest
        {
            [Fact]
            public void Setﾠv_4_6_1ﾠfullﾠdotﾠnetﾠframeworkﾠ()
            {
                WithEqualFullDotNetProjects();
                Subcommand.ResolveFramework(MockDependencyGraphService.Object);
                Subcommand.ResolvedFramework.Should().Be("v4.6.1");
            }

            [Fact]
            public void Setﾠv_4_0ﾠfullﾠdotﾠnetﾠframeworkﾠ()
            {
                WithEqualFullDotNetProjectsBuildAndMinorVersionNotSet();
                Subcommand.ResolveFramework(MockDependencyGraphService.Object);
                Subcommand.ResolvedFramework.Should().Be("v4.0");
            }

            [Fact]
            public void Setﾠ2_0ﾠdotﾠnetﾠcoreﾠframeworkﾠ()
            {
                WithEqualDotNetCoreProjects();
                Subcommand.ResolveFramework(MockDependencyGraphService.Object);
                Subcommand.ResolvedFramework.Should().Be("netcoreapp2.0");
            }

            [Fact]
            public void Setﾠemptyﾠframeworkﾠ()
            {
                WithDifferentDotNetCoreProjects();
                Subcommand.ResolveFramework(MockDependencyGraphService.Object);
                Subcommand.ResolvedFramework.Should().BeEmpty();
            }
        }
    }
}
