﻿namespace NSeed.Cli.Services
{
    public interface IDotNetRunner
    {
        RunStatus Run(string workingDirectory, string[] arguments);
    }
}
