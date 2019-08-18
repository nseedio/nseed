using FluentAssertions;
using Moq;
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
        [InlineData("", "Project name is empty")]
        [InlineData(null, "Project name is empty")]
        [InlineData(@"Name\Name", "Project name contain unallowed characters")]
        [InlineData(@"Name/Name", "Project name contain unallowed characters")]
        [InlineData(@"Name#Name", "Project name contain unallowed characters")]
        [InlineData(@"Name///Name", "Project name contain unallowed characters")]
        [InlineData(@"Name𝄞Name", "Project name contain unallowed characters")]
        [InlineData(@"com1", "Project name is invalid")]
        [InlineData(@"PRN", "Project name is invalid")]
        [InlineData(@"..", "Project name is invalid")]
        [InlineData("Project1", "Project name already exist")]
        [InlineData("Pr", "Project name is to short Min. 3 characters")]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "Project name is to long Max. 50 characters")]
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
