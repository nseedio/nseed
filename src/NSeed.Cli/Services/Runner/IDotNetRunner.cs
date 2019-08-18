using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Models;
using System;
using System.Collections.Generic;

namespace NSeed.Cli.Services
{
    internal interface IDotNetRunner
    {
        (bool IsSuccesful, string Message) RunNewSubcommand(NewSubcommandArgs args);

        (bool IsSuccesful, string Message) AddTemplate(NewSubcommandArgs args);

        (bool IsSuccesful, string Message) CreateProject(NewSubcommandArgs args);

        (bool IsSuccesful, string Message) RemoveTemplate(NewSubcommandArgs args);

        (bool IsSuccesful, string Message) AddProjectToSolution(NewSubcommandArgs args);

        RunStatus Run(string workingDirectory, string[] arguments);
    }
}
