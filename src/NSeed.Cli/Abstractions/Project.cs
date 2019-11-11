using NSeed.Cli.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NSeed.Cli.Abstractions
{
    internal class Project
    {
        public static Project Empty => new Project();

        public Project(string path, IFramework framework)
        {
            Path = path;
            Framework = framework;
        }

        private Project() { }

        public const string Extension = "csproj";

        public string Name
        {
            get
            {
                 try
                 {
                    var doc = XDocument.Load(Path, LoadOptions.PreserveWhitespace);

                    return doc.Root
                        ?.Elements()
                        ?.SelectMany(el => el.Elements())
                        ?.FirstOrDefault(el => el.Name.LocalName == "AssemblyName")
                        ?.Value ?? string.Empty;
                 }
                 catch (System.Xml.XmlException ex)
                 {
                    var g = ex;
                    ErrorMessage = Assets.Resources.Info.Errors.SeedBucketProjectNameCouldNotBeDefined;
                    return string.Empty;
                 }
            }
        }

        public string Path { get; set; } = string.Empty;

        public string Directory => Path.Exists()
            ? new FileInfo(Path)?.DirectoryName ?? string.Empty
            : string.Empty;

        public IFramework Framework { get; set; } = new Framework();

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
