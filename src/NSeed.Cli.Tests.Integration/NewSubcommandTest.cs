using NSeed.Cli.Runners;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSeed.Cli.Tests.Integration
{
    public class NewSubcommandTest
    {
        [Fact]
        public void Test()
        {
            TestRunner runner = new TestRunner();
            var blahh = Assembly.GetExecutingAssembly();
            var response = runner.Bla(@"C:/Repositories/Osobno/25.06.2019_NSeed/src/NSeed.Cli/bin/Debug/netcoreapp2.2/", new string[] { "NSeed.Cli.dll", "--help" });
            if (response.IsSuccess)
            {
            }
        }
    }

    internal class TestRunner : Runners.DotNetRunner
    {
        public RunStatus Bla(string workingDirectory, string[] arguments)
        {
            return Run(workingDirectory, arguments);
        }
    }
}
