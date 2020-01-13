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

        internal Thens ShouldShowSuccessMessage(string projectName, bool shouldShowMissingNugetPackageWarning = false)
        {
            RunStatus.Output.Should().Contain(SuccessfulRun(projectName));
            if (shouldShowMissingNugetPackageWarning)
            {
                RunStatus.Output.Should().Contain(NSeedNuGetPackageHasToBeAddedManuallyToTheProject(projectName));
            }

            return this;
        }

        internal Thens ShouldNotBeSuccessful(
            string errorMessage = "",
            string output = "Specify --help for a list of available options and commands.",
            string argOutput = "")
        {
            RunStatus.IsSuccess.Should().BeFalse();

            if (errorMessage.Exists())
            {
                RunStatus.Errors.Should().Contain(errorMessage);
            }

            RunStatus.Output.Should().Contain($"{output}");

            if (argOutput.Exists())
            {
                RunStatus.Output.Should().Contain(argOutput);
            }

            return this;
        }

        internal Thens ShouldShowHelpMessageForNSeedCommand()
        {
            RunStatus.Output.Should().StartWith("Data seeding tool for .NET.");
            RunStatus.Output.Should().Contain("Usage:");
            RunStatus.Output.Should().Contain("Run 'nseed [command] --help' for more information about a command.");
            return this;
        }

        internal Thens ShouldShowHelpMessageForInfoSubcommand()
        {
            RunStatus.Output.Should().StartWith("Display seed bucket information.");
            RunStatus.Output.Should().Contain("Options:");
            RunStatus.Output.Should().Contain($"-p|--project    {ProjectDescription}");
            RunStatus.Output.Should().Contain("-?|-h|--help    Show command line help.");
            return this;
        }

        internal Thens ShouldShowHelpMessageForNewSubcommand()
        {
            RunStatus.Output.Should().StartWith("Create a new seed bucket project.");
            RunStatus.Output.Should().Contain("Options:");
            RunStatus.Output.Should().Contain($"-s|--solution   {SolutionDescription}");
            RunStatus.Output.Should().Contain($"-f|--framework  {FrameworkDescription}");
            RunStatus.Output.Should().Contain($"-n|--name       {ProjectNameDescription}");
            RunStatus.Output.Should().Contain("-?|-h|--help    Show command line help.");
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
