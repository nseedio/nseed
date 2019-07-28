using System;
using System.Collections.Generic;

namespace NSeed.Cli.Resources
{
    internal static class Resources
    {
        public const string CoreDotNetFramework = ".NETCoreApp";
        public const string FullDotNetFramework = ".NETFramework";

        public static List<string> DotNetCoreVersions => new List<string>
        {
            "1.0", "1.1", "2.0", "2.1", "2.2", "3.0"
        };

        public static List<string> FullDotNetVersions => new List<string>
        {
            "1.0", "1.1", "2.0", "3.0", "3.5", "4", "4.5", "4.5.1", "4.5.2", "4.6", "4.6.1", "4.6.2", "4.7", "4.7.1", "4.7.2", "4.8"
        };

        public static string InitDirectory { get; } = Environment.CurrentDirectory;

        public static class Error
        {
            public const string SolutionPathIsNotProvided = "Solution path is not provided";
            public const string SolutionPathDirectoryNotExist = "Directory not existing on provided solution path";
            public const string MultipleSolutionsFound = "Multiple solutions exists on provided solution path";
            public const string SolutionNotFound = "Solution doesn't exist on provided solution path";
        }

        public static class New
        {
            public const string DefaultProjectName = "Seeds";

            public const string CommandDescription = "Scaffold new NSeed project inside current folder";
            public const string FrameworkDescription = "Create for a specific framework.Values: netcoreapp2.0 to create a.NET Core Class Library or netstandard2.0 to create a.NET Standard Class Library.The default value is netstandard2.0";
            public const string SolutionDescription = "Solution name. Multiple solutions in define path";
            public const string ProjectNameDescription = "Name of the project. Default is Seeds";
        }
    }
}
