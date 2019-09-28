using FluentAssertions;
using Moq;
using NSeed.Cli.Assets;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using NuGet.ProjectModel;
using System.Collections.Generic;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New.Validators
{
    public class SolutionValidatorﾠWithﾠInvalidﾠSolution
    {
        private readonly NewSubcommand subcommand = new NewSubcommand();
        private readonly Mock<IFileSystemService> mockFileSystemService = new Mock<IFileSystemService>();
        private readonly Mock<IDependencyGraphService> dependencyGraphService = new Mock<IDependencyGraphService>();

        public static IEnumerable<object?[]> InvalidSolutionsAndErrorMessages =>
            new[]
            {
                new object?[] { string.Empty, Resources.New.SearchSolutionPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile },
                new object?[] { null, Resources.New.SearchSolutionPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile },
            };
        [Theory]
        // TODO: This looks like a bug in the xUnit analyzer. Strange.
        // For now, just disable it, but take a look at it.
        // It suddenly doesn't work on Igor's machine and we have to see why.
        // Take a look, fix the issue, and remove all disabling of xUnit1019 in all files.
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
        [MemberData(nameof(InvalidSolutionsAndErrorMessages))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
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
