using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Services
{

    internal class FileSystemService : FileSystem, IFileSystemService  
    {
        public const string SolutionPrefix = "sln";

        public IEnumerable<string> GetSolutions(string path)
        {
            var solutions = new List<string>();

            if (string.IsNullOrEmpty(path))
            {
                return solutions;
            }

            return Directory?.GetFiles(path, $"*.{SolutionPrefix}", SearchOption.AllDirectories);
        }

        public string GetSolution(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            var solutions = Directory
                .EnumerateFiles(path, $"*.{SolutionPrefix}", SearchOption.AllDirectories)
                .Take(2)
                .ToList();

            if (solutions.Count == 1)
            {
                return solutions.FirstOrDefault();
            }

            return string.Empty;
        }
    }
}
