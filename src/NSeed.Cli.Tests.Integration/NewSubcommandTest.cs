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
            var currentAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var srcFolderPath = Path.GetFullPath(Path.Combine(currentAssemblyPath, "..", "..", "..", "..", ".."));
            var toolNupkgPath = Path.Combine(srcFolderPath, "NSeed.Cli", "bin", "Debug");
            // dotnet tool install --global --add-source C:\Repositories\Osobno\25.06.2019_NSeed\src\NSeed.Cli\bin\Debug NSeed.Cli
            var installToolResponse = runner.Run(srcFolderPath, new string[]
            {
                $"dotnet tool install --global --add-source {toolNupkgPath} NSeed.Cli"
            });

            var response = runner.Run(srcFolderPath, new string[] { "nseed" });
            if (response.IsSuccess)
            {
                runner.Run(srcFolderPath, new string[] { "dotnet tool uninstall --global NSeed.Cli" });
            }
        }
    }

    internal class TestRunner : DotNetRunner
    {
        public new RunStatus Run(string workingDirectory, string[] arguments)
        {
            return base.Run(workingDirectory, arguments);
        }
    }
}
