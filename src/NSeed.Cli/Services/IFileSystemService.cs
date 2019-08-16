using NSeed.Cli.Assets;
using NSeed.Cli.Subcommands.New.Models;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace NSeed.Cli.Services
{
    /// <summary>
    /// Service for working with file system.
    /// </summary>
    public interface IFileSystemService
    {
        IFile File { get; }

        IPath Path { get; }

        (bool IsSuccesful, string Message) TryGetSolutionPath(string solution, out string path);

        (bool IsSuccesful, string Message) TryGetTemplate(Framework framework, out Template template);

        (bool IsSuccesful, string Message) RemoveTempTemplates();
    }
}
