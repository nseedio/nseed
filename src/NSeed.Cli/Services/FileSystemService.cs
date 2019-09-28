using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
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

        public const string SolutionExtension = ".sln";
        public const string ProjectExtension = ".csproj";
        public const string ZipTemplatesFile = "templates.zip";
        public const string TemplatesDirectory = "templates";

        public string ZipTemplatesFilePath { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), ZipTemplatesFile);

        public string TemplatesDirectoryPath { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), TemplatesDirectory);

        public (bool IsSuccesful, string Message) TryGetSolutionPath(string input, out string path)
        {
            return TryGetFilePath(input, SolutionExtension, New.SearchSolutionPathErrors.Instance, out path);
        }

        public (bool IsSuccesful, string Message) TryGetNSeedProjectPath(string input, out string path)
        {
            return TryGetFilePath(input, ProjectExtension, Info.SearchNSeedProjectPathErrors.Instance, out path);
        }

        public (bool IsSuccesful, string Message) TryGetTemplate(Framework framework, out Template template)
        {
            template = new Template();

            using Stream stream = GetEmbeddedResource(ZipTemplatesFile);

            using (Stream fileStream = File.Create(ZipTemplatesFilePath))
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

        private (bool IsSuccesful, string Message) TryGetFilePath(string input, string extension, IFileSearchErrorMessage errorMessage, out string path)
        {
            var fileInfo = FileInfo.FromFileName(input);
            path = string.Empty;

            return fileInfo switch
            {
                var info when !IsDirectory(info.Directory) => (false, errorMessage.FilePathDirectoryDoesNotExist),
                var info when IsFile(info) => TryGetFileFromFilePath(info, extension, errorMessage, out path),
                var info when IsDirectory(info) => TryGetFileFromDirectoryPath(DirectoryInfo.FromDirectoryName(info.FullName), extension, errorMessage, out path),
                _ => TryGetFileFromFilePath(FileInfo.FromFileName($"{fileInfo.FullName}{extension}"), extension, errorMessage, out path),
            };

            static bool IsFile(IFileInfo fileInfo)
            {
                return !string.IsNullOrEmpty(fileInfo.Extension) &&
                    !IsDirectory(fileInfo);
            }

            static bool IsDirectory(IFileSystemInfo info)
            {
                return (int)info.Attributes != -1 && info.Attributes.HasFlag(FileAttributes.Directory);
            }

            (bool IsSuccesful, string Message) TryGetFileFromFilePath(IFileInfo fileInfo, string extension, IFileSearchErrorMessage errorMessage, out string solutionPath)
            {
                solutionPath = string.Empty;

                if (fileInfo.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase))
                {
                    var (isSuccesful, message, payload) = GetFilePath(fileInfo);
                    if (isSuccesful)
                    {
                        solutionPath = payload;
                        return succesResponse;
                    }

                    return ErrorResponse(message);
                }
                else
                {
                    return ErrorResponse(errorMessage.InvalidFile);
                }

                (bool isSuccesful, string message, string payload) GetFilePath(IFileInfo fileInfo)
                {
                    if (fileInfo.Exists)
                    {
                        return (true, string.Empty, fileInfo.FullName);
                    }

                    return FindFile(
                        fileInfo.Directory.FullName,
                        SearchOption.AllDirectories,
                        extension,
                        errorMessage,
                        fileInfo.Name);
                }
            }

            (bool IsSuccesful, string Message) TryGetFileFromDirectoryPath(IDirectoryInfo directoryInfo, string extension, IFileSearchErrorMessage errorMessage, out string solutionPath)
            {
                solutionPath = string.Empty;

                if (!directoryInfo.Exists)
                {
                    return ErrorResponse(errorMessage.FilePathDirectoryDoesNotExist);
                }

                var response = FindFile(directoryInfo.FullName, SearchOption.TopDirectoryOnly, extension, errorMessage);
                if (response.IsSuccesful)
                {
                    solutionPath = response.Payload;
                    return succesResponse;
                }

                response = FindFile(directoryInfo.FullName, SearchOption.AllDirectories, extension, errorMessage);
                if (response.IsSuccesful)
                {
                    solutionPath = response.Payload;
                    return succesResponse;
                }

                return ErrorResponse(response.Message);
            }

            (bool IsSuccesful, string Message, string Payload) FindFile(
                string path,
                SearchOption searchOption,
                string extension,
                IFileSearchErrorMessage errorMessage,
                string fileName = "")
            {
                var solutions = Directory
                    ?.EnumerateFiles(path, string.IsNullOrEmpty(fileName) ? $"*{extension}" : fileName, searchOption)
                    ?.Take(2)
                    ?.ToList() ?? new List<string>();

                if (!solutions.Any())
                {
                    return (false, errorMessage.WorkingDirectoryDoesNotContainAnyFile, string.Empty);
                }

                if (solutions.Any() && solutions.Count > 1)
                {
                    return (false, errorMessage.MultipleFilesFound, string.Empty);
                }

                return (true, string.Empty, solutions.FirstOrDefault());
            }
        }

        private Stream GetEmbeddedResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name, System.StringComparison.OrdinalIgnoreCase));
            return assembly.GetManifestResourceStream(resourceName);
        }

        private string GetTemplateDirectory(Framework framework)
        {
            return framework switch
            {
                Framework.NETCoreApp => NSeedCoreTemplateDirectory,

                Framework.NETFramework => NSeedClassicTemplateDirectory,

                Framework.None => string.Empty,

                _ => string.Empty,
            };
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

        private static (bool IsSuccesful, string Message) succesResponse = (true, string.Empty);

        private static (bool IsSuccesful, string Message) ErrorResponse(string message) => (false, message);
    }
}
