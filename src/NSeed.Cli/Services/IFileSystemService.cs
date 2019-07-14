using System.Collections.Generic;
using System.IO.Abstractions;

namespace NSeed.Cli.Services
{
    public interface IFileSystemService
    {
        IFile File { get; }
        IPath Path { get; }

        (bool IsSuccesful, string Message) TryGetSolutionPath(string solution, out string path);
    }
}
