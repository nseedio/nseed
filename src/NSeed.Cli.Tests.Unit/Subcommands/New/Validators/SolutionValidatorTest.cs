using FluentAssertions;
using Moq;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New.Validators
{
    public class SolutionValidatorﾠWithﾠInvalidﾠSolution
    {
        //When solution is null or empty provided by user 
        //When solution is not valid you can't create dependency graph 
        SolutionValidator SolutionValidator { get; set; }
        readonly Subcommand Subcommand = new Subcommand();
        Mock<IFileSystemService> MockFileSystemService = new Mock<IFileSystemService>();
        Mock<IDependencyGraphService> DependencyGraphService = new Mock<IDependencyGraphService>();



        [Theory]
        [InlineData("", "Solution is empty")]
        [InlineData(null, "Solution is empty")]
        [InlineData("Test", "Provided solution path is invalid")]
        public void Returnﾠinvalidﾠvalidationﾠresponseﾠwithﾠerrorﾠmessageﾠ(string solution, string errorMessage)
        {
            var resultSlnPath = string.Empty;
            MockFileSystemService.Setup(t => t.TryGetSolutionPath(It.IsAny<string>(), out resultSlnPath)).Returns((true,string.Empty));
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
