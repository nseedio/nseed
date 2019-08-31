using FluentAssertions;
using Moq;
using NSeed.Cli.Assets;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New.Validators
{
    public abstract class BaseNameValidatorTest
    {
        internal NameValidator Validator { get; }

        internal NewSubcommand Subcommand { get; } = new NewSubcommand();

        internal BaseNameValidatorTest()
        {
            Validator = new NameValidator(MockDependencyGraphService.Object);
        }

        internal ValidationResult Validate()
        {
            return Validator.Validate(Subcommand);
        }

        protected static List<string> SlnProjects { get; } = new List<string> { "Project1", "Project2", "Project3", "Project4" };

        protected Mock<IDependencyGraphService> MockDependencyGraphService { get; } = new Mock<IDependencyGraphService>();

        protected void GenerateSolutionProjects(IEnumerable<string> projectNames)
        {
            MockDependencyGraphService
                .Setup(dgs => dgs.GetSolutionProjectsNames(It.IsAny<string>()))
                .Returns(projectNames);
        }
    }

    public class NameValidatorﾠWithﾠInvalidﾠName : BaseNameValidatorTest
    {
        public NameValidatorﾠWithﾠInvalidﾠName()
        {
            GenerateSolutionProjects(SlnProjects);
        }

        public static IEnumerable<object?[]> InvalidProjectNamesAndErrorMessages =>
            new[]
            {
                new object?[] { string.Empty, Resources.New.Errors.ProjectNameNotProvided },
                new object?[] { null, Resources.New.Errors.ProjectNameNotProvided },
                new object?[] { @"Name\Name", Resources.New.Errors.ProjectNameContainsUnallowedCharacters },
                new object?[] { @"Name/Name", Resources.New.Errors.ProjectNameContainsUnallowedCharacters },
                new object?[] { @"Name#Name", Resources.New.Errors.ProjectNameContainsUnallowedCharacters },
                new object?[] { @"Name///Name", Resources.New.Errors.ProjectNameContainsUnallowedCharacters },
                new object?[] { @"Name𝄞Name", Resources.New.Errors.ProjectNameContainsUnallowedCharacters },
                new object?[] { @"com1", Resources.New.Errors.InvalidProjectName },
                new object?[] { @"PRN", Resources.New.Errors.InvalidProjectName },
                new object?[] { @"..", Resources.New.Errors.InvalidProjectName },
                new object?[] { "Project1", Resources.New.Errors.ProjectNameExists },
                new object?[] { "Pr", Resources.New.Errors.ProjectNameToShort },
                new object?[] { "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", Resources.New.Errors.ProjectNameToLong }
            };
        [Theory]
        // TODO: This looks like a bug in the xUnit analyzer. Strange.
        // For now, just disable it, but take a look at it.
        // It suddenly doesn't work on Igor's machine and we have to see why.
        // Take a look, fix the issue, and remove all disabling of xUnit1019 in all files..
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
        [MemberData(nameof(InvalidProjectNamesAndErrorMessages))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
        public void Returnﾠinvalidﾠvalidationﾠresponseﾠwithﾠerrorﾠmessage(string projectName, string errorMessage)
        {
            Subcommand.SetResolvedName(projectName);

            Validate().Should().NotBeNull().And.BeOfType<ValidationResult>();
            Validate().IsValid.Should().BeFalse();
            Validate().Message.Should().Be(errorMessage);
        }
    }

    public class NameValidatorﾠWithﾠValidﾠName : BaseNameValidatorTest
    {
        public NameValidatorﾠWithﾠValidﾠName()
        {
            GenerateSolutionProjects(SlnProjects);
        }

        [Theory]
        [InlineData("MyProject.Seeds")]
        [InlineData("Seeds.Seeds")]
        [InlineData("Seeds")]
        public void ReturnﾠSuccessﾠvalidationﾠresponse(string projectName)
        {
            Subcommand.SetResolvedName(projectName);

            Validate().Should().NotBeNull().And.BeOfType<ValidationResult>();
            Validate().IsValid.Should().BeTrue();
            Validate().Message.Should().BeEmpty();
        }
    }
}
