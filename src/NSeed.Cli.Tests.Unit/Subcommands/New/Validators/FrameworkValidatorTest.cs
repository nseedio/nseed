using FluentAssertions;
using NSeed.Cli.Assets;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New.Validators
{
    public class FrameworkValidatorﾠWithﾠInvalidﾠFramework
    {
        private readonly FrameworkValidator validator = new FrameworkValidator();

        private readonly NewSubcommand subcommand = new NewSubcommand();

        [Theory]
        [InlineData("", Resources.New.Errors.FrameworkNotProvided)]
        [InlineData(null, Resources.New.Errors.FrameworkNotProvided)]
        [InlineData("net", Resources.New.Errors.InvalidFramework)]
        [InlineData("netcoreapp2.7", Resources.New.Errors.InvalidDotNetCoreVersion)]
        [InlineData("NETCoreApp2.7", Resources.New.Errors.InvalidDotNetCoreVersion)]
        [InlineData("coreapp2.7", Resources.New.Errors.InvalidFramework)]
        [InlineData("NETFramework5.3", Resources.New.Errors.InvalidDotNetClassicVersion)]
        [InlineData("netcoreapp_3.1", Resources.New.Errors.InvalidDotNetCoreVersion)]
        public void Returnﾠinvalidﾠvalidationﾠresponseﾠwithﾠerrorﾠmessageﾠ(string framework, string errorMessage)
        {
            subcommand.SetResolvedFramework(framework);
            var result = validator.Validate(subcommand);

            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeFalse();
            result.Message.Should().Be(errorMessage);
        }
    }

    public class FrameworkValidatorﾠWithﾠValidﾠFramework
    {
        private readonly FrameworkValidator validator = new FrameworkValidator();

        private readonly NewSubcommand subcommand = new NewSubcommand();

        [Theory]
        [InlineData("netcoreapp2.2")]
        [InlineData("NETCoreApp3.0")]
        [InlineData("NETFramework4.5.2")]
        [InlineData("netframework4.7.2")]
        public void ReturnﾠSuccessﾠvalidationﾠresponse(string framework)
        {
            subcommand.SetResolvedFramework(framework);
            var result = validator.Validate(subcommand);

            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeTrue();
            result.Message.Should().BeEmpty();
        }
    }
}
