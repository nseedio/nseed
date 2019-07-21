using FluentAssertions;
using Moq;
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
        private SolutionValidator SolutionValidator { get; set; }

        private readonly Subcommand Subcommand = new Subcommand();
        private Mock<IFileSystemService> MockFileSystemService = new Mock<IFileSystemService>();
        private Mock<IDependencyGraphService> DependencyGraphService = new Mock<IDependencyGraphService>();

        [Theory]
        [InlineData("", Resources.Resources.Error.SolutionPathIsNotProvided)]
        [InlineData(null, Resources.Resources.Error.SolutionPathIsNotProvided)]
        public void Returnﾠinvalidﾠvalidationﾠresponseﾠwithﾠerrorﾠmessageﾠ(string solution, string errorMessage)
        {
            var resultSlnPath = string.Empty;
            MockFileSystemService.Setup(t => t.TryGetSolutionPath(It.IsAny<string>(), out resultSlnPath)).Returns((true, string.Empty));
            DependencyGraphService.Setup(t => t.GenerateDependencyGraph(It.IsAny<string>())).Returns(new DependencyGraphSpec());

            SolutionValidator = new SolutionValidator(MockFileSystemService.Object, DependencyGraphService.Object);
            Subcommand.SetResolvedSolution(solution);
            var result = SolutionValidator.Validate(Subcommand);

            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeFalse();
            result.Message.Should().Be(errorMessage);
        }
    }
}
