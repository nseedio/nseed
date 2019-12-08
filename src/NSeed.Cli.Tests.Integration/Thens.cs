using FluentAssertions;
using NSeed.Cli.Extensions;
using NSeed.Cli.Runners;
using NSeed.Cli.Services;
using static NSeed.Cli.Assets.Resources.Info;
using static NSeed.Cli.Assets.Resources.New;

namespace NSeed.Cli.Tests.Integration
{
    public class Thens
    {
        private RunStatus RunStatus { get; }

        internal Thens(RunStatus runStatus)
        {
            RunStatus = runStatus;
        }

        internal static Thens Then(RunStatus runStatus)
        {
            return new Thens(runStatus);
        }

        internal Thens ShouldBeSuccessful()
        {
            RunStatus.IsSuccess.Should().BeTrue();
            return this;
        }

        internal Thens ShouldShowSuccessMessage()
        {
            RunStatus.Output.Should().BeEquivalentTo($"{SuccessfulRun}\r\n");
            return this;
        }

        internal Thens ShouldNotBeSuccessful(
            string errorMessage = "",
            string output = "Specify --help for a list of available options and commands.\r\n")
        {
            RunStatus.IsSuccess.Should().BeFalse();

            if (errorMessage.Exists())
            {
                RunStatus.Errors.Should().BeEquivalentTo($"{errorMessage}\r\n");
            }

            RunStatus.Output.Should().Be(output);

            return this;
        }

        internal Thens ShouldShowHelpMessageForNSeedCommand()
        {
            RunStatus.Output.Should().StartWith("Data seeding command line tool.");
            RunStatus.Output.Should().Contain("Usage:");
            RunStatus.Output.Should().EndWith("Run 'NSeed [command] --help' for more information about a command.\r\n\r\n");
            return this;
        }

        internal Thens ShouldShowHelpMessageForInfoSubcommand()
        {
            RunStatus.Output.Should().StartWith("Display seed bucket information.");
            RunStatus.Output.Should().Contain("Options:");
            RunStatus.Output.Should().Contain($"-p|--project    {ProjectDescription}");
            RunStatus.Output.Should().EndWith("-?|-h|--help    Show command line help.\r\n\r\n");
            return this;
        }

        internal Thens ShouldShowHelpMessageForNewSubcommand()
        {
            RunStatus.Output.Should().StartWith("Create new Seed Bucket project.");
            RunStatus.Output.Should().Contain("Options:");
            RunStatus.Output.Should().Contain($"-s|--solution   {SolutionDescription}");
            RunStatus.Output.Should().Contain($"-f|--framework  {FrameworkDescription}");
            RunStatus.Output.Should().Contain($"-n|--name       {ProjectNameDescription}");
            RunStatus.Output.Should().EndWith("-?|-h|--help    Show command line help.\r\n\r\n");
            return this;
        }

        internal Thens ShouldContainSeedBucketProject(string projectName, string slnPath)
        {
            var dependencyGraphService = new DependencyGraphService(new DependencyGraphRunner());

            var projectNames = dependencyGraphService.GetSolutionProjectsNames(slnPath);

            projectNames.Should().Contain(projectName);

            return this;
        }
    }
}
