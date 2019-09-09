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

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.
            if (!System.IO.Directory.Exists(destDirName))
            {
                System.IO.Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public string ZipTemplatesFilePath { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), ZipTemplatesFile);

        public string TemplatesDirectoryPath { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "templates");

        public (bool IsSuccesful, string Message) TryGetSolutionPath(string input, out string path)
        {
            var fileInfo = FileInfo.FromFileName(input);
            path = string.Empty;

            switch (fileInfo)
            {
                case var info when !IsDirectory(info.Directory):
                    return (false, New.Errors.SolutionPathDirectoryDoesNotExist);
                case var info when IsFile(info):
                    return TryGetSolutionFromFilePath(info, out path);
                case var info when IsDirectory(info):
                    return TryGetSolutionFromDirectoryPath(DirectoryInfo.FromDirectoryName(info.FullName), out path);
                default:
                    return TryGetSolutionFromFilePath(FileInfo.FromFileName($"{fileInfo.FullName}{SolutionExtension}"), out path);
            }

            bool IsFile(IFileInfo fileInfo)
            {
                return !string.IsNullOrEmpty(fileInfo.Extension) &&
                    !IsDirectory(fileInfo);
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

        private (bool IsSuccesful, string Message) TryGetSolutionFromFilePath(IFileInfo fileInfo, out string solutionPath)
        {
            solutionPath = string.Empty;

            if (IsSlnFile(fileInfo))
            {
                var response = GetSolutionFilePath();
                if (response.IsSuccesful)
                {
                    solutionPath = response.Payload;
                    return succesResponse;
                }

                return ErrorResponse(response.Message);
            }
            else
            {
                return ErrorResponse(New.Errors.InvalidFile);
            }

            (bool IsSuccesful, string Message, string Payload) GetSolutionFilePath()
            {
                if (fileInfo.Exists)
                {
                    return (true, string.Empty, fileInfo.FullName);
                }

                return FindSolution(
                    fileInfo.Directory.FullName,
                    SearchOption.AllDirectories,
                    fileInfo.Name);
            }

            bool IsSlnFile(IFileInfo fileInfo)
            {
                return
                    string.Equals(fileInfo.Extension, SolutionExtension, StringComparison.OrdinalIgnoreCase)
                    && !IsDirectory(fileInfo);
            }
        }

        private (bool IsSuccesful, string Message) TryGetSolutionFromDirectoryPath(IDirectoryInfo directoryInfo, out string solutionPath)
        {
            solutionPath = string.Empty;

            if (!directoryInfo.Exists)
            {
                return ErrorResponse(New.Errors.SolutionPathDirectoryDoesNotExist);
            }

            var response = FindSolution(directoryInfo.FullName, SearchOption.TopDirectoryOnly);
            if (response.IsSuccesful)
            {
                solutionPath = response.Payload;
                return succesResponse;
            }

            response = FindSolution(directoryInfo.FullName, SearchOption.AllDirectories);
            if (response.IsSuccesful)
            {
                solutionPath = response.Payload;
                return succesResponse;
            }

            return ErrorResponse(response.Message);
        }

        private (bool IsSuccesful, string Message, string Payload) FindSolution(string path, SearchOption searchOption, string fileName = "")
        {
            var solutions = Directory
                ?.EnumerateFiles(path, string.IsNullOrEmpty(fileName) ? $"*{SolutionExtension}" : fileName, searchOption)
                ?.Take(2)
                ?.ToList() ?? new List<string>();

            if (!solutions.Any())
            {
                return (false, New.Errors.WorkingDirectoryDoesNotContainAnySolution, string.Empty);
            }

            if (solutions.Any() && solutions.Count > 1)
            {
                return (false, New.Errors.MultipleSolutionsFound, string.Empty);
            }

            return (true, string.Empty, solutions.FirstOrDefault());
        }

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

        private bool IsDirectory(IFileInfo fileInfo)
        {
            return (int)fileInfo.Attributes != -1 && fileInfo.Attributes.HasFlag(FileAttributes.Directory);
        }

        private bool IsDirectory(IDirectoryInfo directoryInfo)
        {
            return (int)directoryInfo.Attributes != -1 && directoryInfo.Attributes.HasFlag(FileAttributes.Directory);
        }

        private (bool IsSuccesful, string Message) succesResponse = (true, string.Empty);

        private (bool IsSuccesful, string Message) ErrorResponse(string message) => (false, message);
    }
}
