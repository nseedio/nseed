using FluentAssertions;
using NSeed.Cli.Assets;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using System.Collections.Generic;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Subcommands.New.Validators
{
    public class FrameworkValidatorﾠWithﾠInvalidﾠFramework
    {
        private readonly FrameworkValidator validator = new FrameworkValidator();

        private readonly NewSubcommand subcommand = new NewSubcommand();

        public static IEnumerable<object?[]> InvalidFrameworksAndErrorMessages =>
            new[]
            {
                new object?[] { string.Empty, Resources.New.Errors.FrameworkNotProvided },
                new object?[] { null, Resources.New.Errors.FrameworkNotProvided },
                new object?[] { "net", Resources.New.Errors.InvalidFramework },
                new object?[] { "netcoreapp2.7", Resources.New.Errors.InvalidDotNetCoreVersion },
                new object?[] { "NETCoreApp2.7", Resources.New.Errors.InvalidDotNetCoreVersion },
                new object?[] { "coreapp2.7", Resources.New.Errors.InvalidFramework },
                new object?[] { "NETFramework5.3", Resources.New.Errors.InvalidDotNetClassicVersion },
                new object?[] { "netcoreapp_3.1", Resources.New.Errors.InvalidDotNetCoreVersion }
            };
        [Theory]
        // TODO: This looks like a bug in the xUnit analyzer. Strange.
        // For now, just disable it, but take a look at it.
        // It suddenly doesn't work on Igor's machine and we have to see why.
        // Take a look, fix the issue, and remove all disabling of xUnit1019 in all files.
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
        [MemberData(nameof(InvalidFrameworksAndErrorMessages))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
        public void Returnﾠinvalidﾠvalidationﾠresponseﾠwithﾠerrorﾠmessageﾠ(
            string framework,
            string errorMessage)
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
