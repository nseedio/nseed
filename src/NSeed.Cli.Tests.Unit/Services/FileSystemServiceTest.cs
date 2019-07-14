using FluentAssertions;
using NSeed.Cli.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Services
{
    public class FileSystemServiceTest
    {
        public class TryGetSolutionPathﾠ
        {
            [Theory]
            [InlineData(@"C:\Repositories\Osobno\Fit\Fit.Web.sln")]
            [InlineData(@"C:\Repositories\Osobno\Fit")]
            [InlineData(@"C:\Repositories\LeadApi")]
            [InlineData(@"C:\TestNSeed\NestedSolution")]
            [InlineData(@"C:\Repositories\Pims")]
            public void IsﾠSuccesfulﾠAndﾠReturnsﾠValidﾠPath(string path)
            {
                var service = new FileSystemService();
                var response = service.TryGetSolutionPath(path, out var sln);
                response.IsSuccesful.Should().Be(true);
                sln.Should().NotBeNullOrEmpty();
            }

            
            [Theory(Skip ="Not finished yet")]
            [InlineData("Fit.Web", "")]
            [InlineData(@"C:\Repositories\Fit.Web.sln", "")]
            [InlineData(@"C:\Repositories\Osobno\Fit\Fit.Web\Program.cs", "")]
            [InlineData(@"C:\TestNSeed\MultipleSln", "")]
            public void IsﾠNotﾠSuccesfulﾠAndﾠReturnsﾠErrorﾠResponse(string path, string errorMessage)
            {
                var service = new FileSystemService();
                var response = service.TryGetSolutionPath(path, out var sln);
                response.IsSuccesful.Should().Be(false);
                response.Message.Should().Be(errorMessage);
                sln.Should().BeNullOrEmpty();
            }

        }
    }
}
