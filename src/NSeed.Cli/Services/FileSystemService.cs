using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Subcommands.New.Models;
using System;
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
        public const string SolutionExtension = ".sln";
        public const string ZipTemplatesFile = "templates.zip";

        public string ZipTemplatesFilePath { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), ZipTemplatesFile);

        public string TemplatesDirectoryPath { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "templates");

        public (bool IsSuccesful, string Message) TryGetSolutionPath(string solution, out string path)
        {
            var fileInfo = FileInfo.FromFileName(solution);

            if (string.Equals(fileInfo.Extension, SolutionExtension, StringComparison.OrdinalIgnoreCase) &&
                !IsDirectory(fileInfo) &&
                fileInfo.Exists)
            {
                path = fileInfo.FullName;
                return succesResponse;
            }
            else
            {
                if (IsDirectory(fileInfo))
                {
                    return TryGetSolution(fileInfo.FullName, out path);
                }
                else
                {
                    return TryGetSolution($"{fileInfo.FullName}.{SolutionExtension}", out path);
                }
            }

            static bool IsDirectory(IFileInfo fileInfo)
            {
                return (int)fileInfo.Attributes != -1 && fileInfo.Attributes.HasFlag(FileAttributes.Directory);
            }
        }

        public (bool IsSuccesful, string Message) TryGetTemplate(Framework framework, out Template template)
        {
            template = new Template();

            using Stream stream = GetEmbeddedResource(ZipTemplatesFile);
            using (var fileStream = File.Create(ZipTemplatesFilePath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(ZipTemplatesFilePath, Path.GetTempPath());

            template.Path = Path.Combine(TemplatesDirectoryPath, GetTemplateDirectory(framework));
            template.Name = GetTemplateName(template.Path);

            return succesResponse;
        }

        public (bool IsSuccesful, string Message) RemoveTempTemplates()
        {
            if (new FileInfo(ZipTemplatesFilePath).Exists)
            {
                File.Delete(ZipTemplatesFilePath);
            }

            if (new DirectoryInfo(TemplatesDirectoryPath).Exists)
            {
                Directory.Delete(TemplatesDirectoryPath, true);
            }

            return succesResponse;
        }

        private (bool IsSuccesful, string Message) TryGetSolution(string path, out string solution)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(path));

            solution = string.Empty;

            var directoryInfo = DirectoryInfo.FromDirectoryName(path);
            if (!directoryInfo.Exists)
            {
                return ErrorResponse(New.Errors.SolutionPathDirectoryDoesNotExist);
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
                    return ErrorResponse(New.Errors.WorkingDirectoryDoesNotContainAnySolution);
                }
            }

            solution = response.solution;
            return succesResponse;
        }

        private (string solution, bool foundMultiple, bool notFound) GetSolution(string path, SearchOption searchOption)
        {
            var solutions = Directory
                ?.EnumerateFiles(path, $"*.{SolutionExtension}", searchOption)
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

        private string GetTemplateDirectory(Framework framework)
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

        private string GetTemplateName(string path)
        {
            var name = Guid.NewGuid().ToString();
            ReplaceAllPlaceholders(path, name);
            return name;
        }

        private void ReplaceAllPlaceholders(string path, string name)
        {
            var templateConfigFilePath = Path.Combine(path, ".template.config", "template.json");
            string content = File.ReadAllText(templateConfigFilePath);
            content = content.Replace("guid", name);
            File.WriteAllText(templateConfigFilePath, content);
        }
    }
}
