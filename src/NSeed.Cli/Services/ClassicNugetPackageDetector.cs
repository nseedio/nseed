using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NuGet.Packaging;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NSeed.Cli.Services
{
    internal class ClassicNugetPackageDetector : INugetPackageDetector
    {
        public IOperationResponse Detect(Project project, string nugetPackageName)
        {
            var packagesConfigPath = Path.Combine(project.Directory, "packages.config");

            if (!File.Exists(Path.Combine(packagesConfigPath)))
            {
                return OperationResponse.Error(Resources.Info.Errors.NSeedProjectCouldNotBeFound);
            }

            var document = XDocument.Load(packagesConfigPath);
            var reader = new PackagesConfigReader(document);
            return reader
                .GetPackages()
                .Select(p => p.PackageIdentity.Id)
                .Any(txt => txt.Equals(nugetPackageName, StringComparison.OrdinalIgnoreCase))
                ? OperationResponse.Success()
                : OperationResponse.Error(Resources.Info.Errors.NSeedProjectCouldNotBeFound);
        }
    }
}
