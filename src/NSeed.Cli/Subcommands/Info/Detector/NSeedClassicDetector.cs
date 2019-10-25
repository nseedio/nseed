using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Services;
using NuGet.Packaging;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NSeed.Cli.Subcommands.Info.Detector
{
    internal class NSeedClassicDetector : NSeedDetector
    {
        public NSeedClassicDetector(IDependencyGraphService dependencyGraphService, IFileSystemService fileSystemService)
            : base(dependencyGraphService, fileSystemService) { }

        protected override IOperationResponse<Project> DetectDependency(Project project)
        {
            var document = XDocument.Load(Path.Combine(project.Directory, "packages.config"));
            var reader = new PackagesConfigReader(document);
            return reader
                .GetPackages()
                .Select(p => p.PackageIdentity.Id)
                .Any(txt => txt.Equals(NSeed, StringComparison.OrdinalIgnoreCase))
                ? OperationResponse<Project>.Success(project)
                : OperationResponse<Project>.Error(Resources.Info.Errors.NSeedProjectCouldNotBeFound);
        }
    }
}
