using NSeed.Cli.Extensions;
using System.IO;

namespace NSeed.Cli.Abstractions
{
    internal class Project
    {
        public Project() { }

        public Project(string path)
        {
            Path = path;
        }

        public const string Extension = "csproj";

        public string Name { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Directory => Path.Exists()
            ? new FileInfo(Path)?.DirectoryName ?? string.Empty
            : string.Empty;

        public IFramework Framework { get; set; } = new Framework();
    }
}
