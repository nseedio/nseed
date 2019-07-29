using FluentAssertions;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New.Validators
{
    public class FrameworkValidatorﾠWithﾠInvalidﾠFramework
    {
        private FrameworkValidator Validator { get; set; }

        private readonly Subcommand subcommand = new Subcommand();

        [Theory]
        [InlineData("", "Project framework is empty")]
        [InlineData(null, "Project framework is empty")]
        [InlineData("netcoreapp2.7", "Core project framework version is not valid")]
        [InlineData("NETCoreApp2.7", "Core project framework version is not valid")]
        [InlineData("coreapp2.7", "Project framework is invalid")]
        [InlineData("NETFramework5.3", "Full dotnet project version is not valid")]
        public void Returnﾠinvalidﾠvalidationﾠresponseﾠwithﾠerrorﾠmessageﾠ(string framework, string errorMessage)
        {
            Validator = new FrameworkValidator();
            subcommand.SetResolvedFramework(framework);
            var result = Validator.Validate(subcommand);

            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeFalse();
            result.Message.Should().Be(errorMessage);
        }
    }

    public class FrameworkValidatorﾠWithﾠValidﾠFramework
    {
        private FrameworkValidator Validator { get; set; }

        private readonly Subcommand subcommand = new Subcommand();

        [Theory]
        [InlineData("netcoreapp2.2")]
        [InlineData("NETCoreApp3.0")]
        [InlineData("NETFramework4.5.2")]
        [InlineData("netframework4.7.2")]
        public void ReturnﾠSuccessﾠvalidationﾠresponse(string framework)
        {
            Validator = new FrameworkValidator();
            subcommand.SetResolvedFramework(framework);
            var result = Validator.Validate(subcommand);

            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeTrue();
            result.Message.Should().BeEmpty();
        }
    }
}
