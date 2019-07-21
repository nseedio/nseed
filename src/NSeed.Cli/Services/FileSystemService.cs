using NSeed.Cli.Extensions;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using static NSeed.Cli.Resources.Resources;

namespace NSeed.Cli.Services
{
    internal class FileSystemService : FileSystem, IFileSystemService
    {
        public const string SolutionPrefix = "sln";

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

        private (bool IsSuccesful, string Message) TryGetSolution(string path, out string solution)
        {
            solution = string.Empty;

            if (string.IsNullOrEmpty(path))
            {
                return ErrorResponse(Error.SolutionPathIsNotProvided);
            }

            var directoryInfo = DirectoryInfo.FromDirectoryName(path);
            if (!directoryInfo.Exists)
            {
                return ErrorResponse(Error.SolutionPathDirectoryNotExist);
            }

            var response = GetSolution(path, SearchOption.TopDirectoryOnly);

            if (response.foundMultiple)
            {
                return ErrorResponse(Error.MultipleSolutionsFound);
            }

            if (response.notFound)
            {
                response = GetSolution(path, SearchOption.AllDirectories);

                if (response.foundMultiple)
                {
                    return ErrorResponse(Error.MultipleSolutionsFound);
                }

                if (response.notFound)
                {
                    return ErrorResponse(Error.SolutionNotFound);
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
    }
}
