using NSeed.Cli.Abstractions;
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

        public static bool IsFile(IFileSystemInfo info)
        {
            return !string.IsNullOrEmpty(info.Extension) &&
                !IsDirectory(info);
        }

        public static bool IsDirectory(IFileSystemInfo info)
        {
            return (int)info.Attributes != -1 && info.Attributes.HasFlag(FileAttributes.Directory);
        }

        public bool IsDirectory(string path)
        {
            var fileInfo = FileInfo.FromFileName(path);

            return (int)fileInfo.Attributes != -1 && fileInfo.Attributes.HasFlag(FileAttributes.Directory);
        }

        public const string SolutionExtension = ".sln";
        public const string ProjectExtension = ".csproj";
        public const string ZipTemplatesFile = "templates.zip";
        public const string TemplatesDirectory = "templates";

        public string ZipTemplatesFilePath { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), ZipTemplatesFile);

        public string TemplatesDirectoryPath { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), TemplatesDirectory);

        public IOperationResponse<string> GetSolutionPath(string input)
        {
            var response = GetFilePaths(input, SolutionExtension, New.SearchSolutionPathErrors.Instance);

            if (!response.IsSuccessful)
            {
                return OperationResponse<string>.Error(response.Message);
            }

            return OperationResponse<string>.Success(response.Payload.FirstOrDefault());
        }

        public IOperationResponse<IEnumerable<string>> GetNSeedProjectPaths(string input)
        {
            return GetFilePaths(input, ProjectExtension, Info.SearchNSeedProjectPathErrors.Instance, allowMultiple: true);
        }

        public (bool IsSuccesful, string Message) TryGetTemplate(FrameworkType framework, out Template template)
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

        private IOperationResponse<IEnumerable<string>> GetFilePaths(
            string input,
            string extension,
            IFileSearchErrorMessage errorMessage,
            bool allowMultiple = false)
        {
            var fileInfo = FileInfo.FromFileName(input);

            return fileInfo switch
            {
                var info when !IsDirectory(info.Directory) => OperationResponse<IEnumerable<string>>.Error(errorMessage.FilePathDirectoryDoesNotExist),
                var info when IsFile(info) => GetFilesFromFilePath(info, extension, errorMessage, allowMultiple),
                var info when IsDirectory(info) => GetFilesFromDirectoryPath(DirectoryInfo.FromDirectoryName(info.FullName), extension, errorMessage, allowMultiple),
                _ => GetFilesFromFilePath(FileInfo.FromFileName($"{fileInfo.FullName}{extension}"), extension, errorMessage, allowMultiple),
            };

            IOperationResponse<IEnumerable<string>> GetFilesFromFilePath(
                IFileInfo fileInfo,
                string extension,
                IFileSearchErrorMessage errorMessage,
                bool allowMultiple)
            {
                if (fileInfo.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return GetFilePath(fileInfo);
                }
                else
                {
                    return OperationResponse<IEnumerable<string>>.Error(errorMessage.InvalidFile);
                }

                IOperationResponse<IEnumerable<string>> GetFilePath(IFileInfo fileInfo)
                {
                    if (fileInfo.Exists)
                    {
                        return OperationResponse<IEnumerable<string>>.Success(new string[] { fileInfo.FullName });
                    }

                    if (allowMultiple)
                    {
                        return FindFiles(
                           fileInfo.Directory.FullName,
                           extension,
                           errorMessage,
                           fileInfo.Name);
                    }
                    else
                    {
                        return FindFile(
                            fileInfo.Directory.FullName,
                            SearchOption.AllDirectories,
                            extension,
                            errorMessage,
                            fileInfo.Name);
                    }
                }
            }

            IOperationResponse<IEnumerable<string>> GetFilesFromDirectoryPath(
                IDirectoryInfo directoryInfo,
                string extension,
                IFileSearchErrorMessage errorMessage,
                bool allowMultiple)
            {
                if (!directoryInfo.Exists)
                {
                    return OperationResponse<IEnumerable<string>>.Error(errorMessage.FilePathDirectoryDoesNotExist);
                }

                if (allowMultiple)
                {
                    return FindFiles(directoryInfo.FullName, extension, errorMessage);
                }
                else
                {
                    var response = FindFile(directoryInfo.FullName, SearchOption.TopDirectoryOnly, extension, errorMessage);
                    if (response.IsSuccessful)
                    {
                        return response;
                    }

                    return FindFile(directoryInfo.FullName, SearchOption.AllDirectories, extension, errorMessage);
                }
            }

            IOperationResponse<IEnumerable<string>> FindFiles(
               string path,
               string extension,
               IFileSearchErrorMessage errorMessage,
               string fileName = "")
            {
                var searchItem = fileName.IsNotProvidedByUser() ? $"*{extension}" : fileName;

                var files = Directory
                    ?.GetFiles(path, searchItem, SearchOption.AllDirectories)
                    ?.ToList() ?? new List<string>();

                return files switch
                {
                    var _ when !files.Any() => OperationResponse<IEnumerable<string>>.Error(errorMessage.WorkingDirectoryDoesNotContainAnyFile),
                    _ => OperationResponse<string>.Success(files),
                };
            }

            IOperationResponse<IEnumerable<string>> FindFile(
                string path,
                SearchOption searchOption,
                string extension,
                IFileSearchErrorMessage errorMessage,
                string fileName = "")
            {
                var searchItem = fileName.IsNotProvidedByUser() ? $"*{extension}" : fileName;

                var files = Directory
                    ?.EnumerateFiles(path, searchItem, searchOption)
                    ?.Take(2)
                    ?.ToList() ?? new List<string>();

                return files switch
                {
                    var _ when !files.Any() => OperationResponse<IEnumerable<string>>.Error(errorMessage.WorkingDirectoryDoesNotContainAnyFile),
                    var _ when files.Any() && files.Count > 1 => OperationResponse<IEnumerable<string>>.Error(errorMessage.MultipleFilesFound),
                    _ => OperationResponse<string>.Success(files),
                };
            }
        }

        private Stream GetEmbeddedResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name, System.StringComparison.OrdinalIgnoreCase));
            return assembly.GetManifestResourceStream(resourceName);
        }

        private string GetTemplateDirectory(FrameworkType framework)
        {
            return framework switch
            {
                FrameworkType.NETCoreApp => NSeedCoreTemplateDirectory,

                FrameworkType.NETFramework => NSeedClassicTemplateDirectory,

                FrameworkType.None => string.Empty,

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
    }
}
