using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Subcommands.New.Models;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using static NSeed.Cli.Assets.Resources;

namespace NSeed.Cli.Services
{
    internal class FileSystemService : FileSystem, IFileSystemService
    {
        public const string SolutionPrefix = "sln";
        public const string Templates = "templates.zip";

        public (bool IsSuccesful, string Message) TryGetSolutionPath(string solution, out string path)
        {
            var fileInfo = FileInfo.FromFileName(solution);

            if (fileInfo.Extension == $".{SolutionPrefix}" && fileInfo.Exists)
            {
                path = fileInfo.FullName;
                return succesResponse;
            }
            else
            {
                var attr = fileInfo.Attributes;
                if ((int)attr != -1 && fileInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    return TryGetSolution(fileInfo.FullName, out path);
                }
                else
                {
                    return TryGetSolution($"{fileInfo.FullName}.{SolutionPrefix}", out path);
                }
            }
        }

        public (bool IsSuccesful, string Message) TryGetTemplate(Framework framework, out Template template)
        {
            template = new Template();
            using Stream stream = GetEmbeddedResource(Templates);
            using (var fileStream = File.Create(Path.Combine(Path.GetTempPath(), Templates)))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(Path.GetTempPath(), Templates), Path.GetTempPath());

            // Change properties for short name only in json with GUID but maybe also some other properties

            template.Path = Path.Combine(Path.GetTempPath(), "templates", GetTemplateFolder(framework));
            template.Name = "nseedcoreclasslib";

            return succesResponse;
        }

        public (bool IsSuccesful, string Message) RemoveTempTemplates()
        {
            File.Delete(Path.Combine(Path.GetTempPath(), Templates));
            Directory.Delete(Path.Combine(Path.GetTempPath(), "templates"), true);
            return succesResponse;
        }

        private (bool IsSuccesful, string Message) TryGetSolution(string path, out string solution)
        {
            solution = string.Empty;

            if (string.IsNullOrEmpty(path))
            {
                return ErrorResponse(New.Errors.SolutionPathIsNotProvided);
            }

            var directoryInfo = DirectoryInfo.FromDirectoryName(path);
            if (!directoryInfo.Exists)
            {
                return ErrorResponse(New.Errors.SolutionPathDirectoryNotExist);
            }

            var response = GetSolution(path, SearchOption.TopDirectoryOnly);

            if (response.foundMultiple)
            {
                return ErrorResponse(New.Errors.MultipleSolutionsFound);
            }

            if (response.notFound)
            {
                response = GetSolution(path, SearchOption.AllDirectories);

                if (response.foundMultiple)
                {
                    return ErrorResponse(New.Errors.MultipleSolutionsFound);
                }

                if (response.notFound)
                {
                    return ErrorResponse(New.Errors.SolutionNotFound);
                }
            }

            solution = response.solution;
            return succesResponse;
        }

        private (string solution, bool foundMultiple, bool notFound) GetSolution(string path, SearchOption searchOption)
        {
            var solutions = Directory
                ?.EnumerateFiles(path, $"*.{SolutionPrefix}", searchOption)
                ?.Take(2)
                ?.ToList() ?? new List<string>();

            if (solutions.IsNullOrEmpty())
            {
                return slnNotFound;
            }

            if (solutions.Any() && solutions.Count > 1)
            {
                return foundMultipleSln;
            }

            return Sln(solutions.FirstOrDefault());
        }

        private (string solution, bool foundMultiple, bool notFound) slnNotFound = (string.Empty, false, true);
        private (string solution, bool foundMultiple, bool notFound) foundMultipleSln = (string.Empty, true, false);

        private (string solution, bool foundMultiple, bool notFound) Sln(string sln) => (sln, false, false);

        private (bool IsSuccesful, string Message) succesResponse = (true, string.Empty);

        private (bool IsSuccesful, string Message) ErrorResponse(string message) => (false, message);

        private Stream GetEmbeddedResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name, System.StringComparison.OrdinalIgnoreCase));
            return assembly.GetManifestResourceStream(resourceName);
        }

        private string GetTemplateFolder(Framework framework)
        {
            switch (framework)
            {
                case Framework.NETCoreApp:
                    return "nseed_core_template";

                case Framework.NETFramework:
                    return "nseed_classic_template";
            }

            return string.Empty;
        }
    }
}
