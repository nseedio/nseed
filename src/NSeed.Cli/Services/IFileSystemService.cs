﻿using System.Collections.Generic;
using System.IO.Abstractions;

namespace NSeed.Cli.Services
{
    public interface IFileSystemService
    {
        IFile File { get; }
        IPath Path { get; }

        IEnumerable<string> GetSolutions(string path);
        string GetSolution(string path);

    }
}