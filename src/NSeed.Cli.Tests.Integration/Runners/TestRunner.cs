using NSeed.Cli.Runners;
using System;
using System.IO;

namespace NSeed.Cli.Tests.Integration.Runners
{
    internal class TestRunner : DotNetRunner
    {
        public new RunStatus RunDotNet(string workingDirectory, string[] arguments)
        {
            return base.RunDotNet(workingDirectory, arguments);
        }

        public RunStatus RunNSeed(string[] arguments)
        {
            return Run("nseed", string.Empty, arguments);
        }
    }
}
