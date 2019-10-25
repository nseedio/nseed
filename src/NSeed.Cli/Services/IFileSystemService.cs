using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Subcommands.New.Models;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace NSeed.Cli.Services
{
    internal interface IFileSystemService
    {
        IFile File { get; }

        IPath Path { get; }

        IOperationResponse<string> GetSolutionPath(string solution);

        IOperationResponse<IEnumerable<string>> GetNSeedProjectPaths(string input);

        (bool IsSuccesful, string Message) TryGetTemplate(FrameworkType framework, out Template template);

        (bool IsSuccesful, string Message) RemoveTempTemplates();
    }
}
