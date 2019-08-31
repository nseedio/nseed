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

        [Theory]
        [InlineData("", Resources.New.Errors.ProjectNameNotProvided)]
        [InlineData(null, Resources.New.Errors.ProjectNameNotProvided)]
        [InlineData(@"Name\Name", Resources.New.Errors.ProjectNameContainUnallowedCharacters)]
        [InlineData(@"Name/Name", Resources.New.Errors.ProjectNameContainUnallowedCharacters)]
        [InlineData(@"Name#Name", Resources.New.Errors.ProjectNameContainUnallowedCharacters)]
        [InlineData(@"Name///Name", Resources.New.Errors.ProjectNameContainUnallowedCharacters)]
        [InlineData(@"Name𝄞Name", Resources.New.Errors.ProjectNameContainUnallowedCharacters)]
        [InlineData(@"com1", Resources.New.Errors.InvalidProjectName)]
        [InlineData(@"PRN", Resources.New.Errors.InvalidProjectName)]
        [InlineData(@"..", Resources.New.Errors.InvalidProjectName)]
        [InlineData("Project1", Resources.New.Errors.ProjectNameExists)]
        [InlineData("Pr", Resources.New.Errors.ProjectNameToShort)]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", Resources.New.Errors.ProjectNameToLong)]
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
