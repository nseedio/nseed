namespace NSeed.Cli.Assets
{
    internal static partial class Resources
    {
        internal static class New
        {
            public const string CommandDescription = "Create new Seed Bucket project.";

            public const string FrameworkDescription = "The target framework for the Seed Bucket project. For .NET Core project use netcoreappX.Y. For .NET Classic project use netframeworkX.Y. The default framework is derived based on frameworks already used in the target solution.";
            public const string SolutionDescription = "The target solution. For example: MySolution.sln, SubFolderWithSolution, C:\\Path\\To\\Solutions\\FirstSolution.sln. The default is the nearest single solution found in the working directory or its subdirectories.";
            public const string ProjectNameDescription = "The project name. The default name is " + DefaultProjectName + " if the project with that name does not already exist.";

            public const string DefaultProjectName = "Seeds";

            internal static class Errors
            {
                public const string WorkingDirectoryDoesNotContainAnySolution = "Couldn't find a solution in the working directory. Ensure that a solution exists in the working directory or any of its subdirectories, or pass the target solution by using --solution";
                public const string SolutionPathDirectoryDoesNotExist = "The provided solution directory does not exist. Do you maybe have a typo in the directory path?";
                public const string MultipleSolutionsFound = "Multiple solutions found. Specify a single solution by using --solution with solution name.";
            }
        }
    }
}
