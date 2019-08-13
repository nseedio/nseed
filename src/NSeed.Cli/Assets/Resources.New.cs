namespace NSeed.Cli.Assets
{
    internal static partial class Resources
    {
        internal static class New
        {
            public const string CommandDescription = "Scaffold new NSeed project inside current folder";

            public const string FrameworkDescription = "Create for a specific framework.Values: netcoreapp2.0 to create a.NET Core Class Library or netstandard2.0 to create a.NET Standard Class Library.The default value is netstandard2.0";
            public const string SolutionDescription = "Solution name. Multiple solutions in define path";
            public const string ProjectNameDescription = "Name of the project. Default is Seeds";

            public const string DefaultProjectName = "Seeds";

            internal static class Errors
            {
                public const string SolutionPathIsNotProvided = "Solution path is not provided";
                public const string SolutionPathDirectoryNotExist = "Directory not existing on provided solution path";
                public const string MultipleSolutionsFound = "Multiple solutions exists on provided solution path";
                public const string SolutionNotFound = "Solution doesn't exist on provided solution path";
            }
        }
    }
}
