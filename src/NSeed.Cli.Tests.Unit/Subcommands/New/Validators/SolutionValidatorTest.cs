using FluentAssertions;
using Moq;
using NSeed.Cli.Assets;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using NuGet.ProjectModel;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New.Validators
{
    public class SolutionValidatorﾠWithﾠInvalidﾠSolution
    {
        private readonly NewSubcommand subcommand = new NewSubcommand();
        private readonly Mock<IFileSystemService> mockFileSystemService = new Mock<IFileSystemService>();
        private readonly Mock<IDependencyGraphService> dependencyGraphService = new Mock<IDependencyGraphService>();

        [Theory]
        [InlineData("", Resources.New.Errors.SolutionPathIsNotProvided)]
        [InlineData(null, Resources.New.Errors.SolutionPathIsNotProvided)]
        public void Returnﾠinvalidﾠvalidationﾠresponseﾠwithﾠerrorﾠmessageﾠ(string solution, string errorMessage)
        {
            var resultSlnPath = string.Empty;
            mockFileSystemService.Setup(t => t.TryGetSolutionPath(It.IsAny<string>(), out resultSlnPath)).Returns((true, string.Empty));
            dependencyGraphService.Setup(t => t.GenerateDependencyGraph(It.IsAny<string>())).Returns(new DependencyGraphSpec());

            var validator = new SolutionValidator(mockFileSystemService.Object, dependencyGraphService.Object);
            subcommand.SetResolvedSolution(solution);
            var result = validator.Validate(subcommand);

            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeFalse();
            result.Message.Should().Be(errorMessage);
        }
    }
}
