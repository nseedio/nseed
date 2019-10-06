using NSeed.Cli.Abstractions;
using System;

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

            public const string SuccessfulRun = "Seed Bucket project created successfully.";

            public const string DefaultProjectName = "Seeds";

            public const int MinProjectNameCharacters = 3;
            public const int MaxProjectNameCharacters = 50;

            internal static class Errors
            {
                public static readonly string InvalidSolution = "The provided solution (*.sln) file is invalid or corrupted. Use --solution to set a valid solution file";

                public static readonly string FrameworkNotProvided = "The framework is not provided or could not be derived based on frameworks already used in the target solution. Use --framework option to set a valid framework, netcoreappX.Y for .NET Core project or netframeworkX.Y for .NET Classic project.";
                public static readonly string InvalidFramework = $"The provided framework is invalid. {DoYouMaybeHaveATypoInThe("framework name")}";
                public static readonly string InvalidDotNetCoreVersion = $"The provided version of .NET Core framework is not supported. The supported versions are: {string.Join(", ", DotNetCoreVersions)}.";
                public static readonly string InvalidDotNetClassicVersion = $"The provided version of .NET Classic framework is not supported. The supported versions are: {string.Join(", ", DotNetClassicVersions)}.";

                public static readonly string ProjectNameNotProvided = $"The project name is not provided or could not be assigned automatically. {UseNameOptionToSetProjectName()}";
                public static readonly string ProjectNameContainsUnallowedCharacters = $"The provided project name contains unallowed characters. {UseNameOptionToSetProjectName("a valid")}";
                public static readonly string InvalidProjectName = $"The provided project name is invalid (contains reserved words like PRN or COM1). {UseNameOptionToSetProjectName("a valid")}";
                public static readonly string ProjectNameExists = $"The project name already exists in the provided solution. {UseNameOptionToSetProjectName("a new")}";
                public static readonly string ProjectNameTooLong = "The provided project name is too long. The maximum length of the project name is " + MaxProjectNameCharacters + " characters.";
                public static readonly string ProjectNameTooShort = "The provided project name is too short. The minimum length of project name is " + MinProjectNameCharacters + " characters.";

                private static string UseNameOptionToSetProjectName(string projectNameQualifier = "the") =>
                    $"Use --name option to set {projectNameQualifier} project name.";
            }

            internal class SearchSolutionPathErrors : IFileSearchErrorMessage
            {
                private static readonly Lazy<SearchSolutionPathErrors> Lazy =
                    new Lazy<SearchSolutionPathErrors>(() => new SearchSolutionPathErrors());

                public static SearchSolutionPathErrors Instance => Lazy.Value;

                private SearchSolutionPathErrors()
                {
                }

                public string WorkingDirectoryDoesNotContainAnyFile => "Could not find a solution in the working directory. Ensure that a solution exists in the working directory or any of its subdirectories, or pass the target solution by using --solution.";

                public string FilePathDirectoryDoesNotExist => $"The provided solution directory does not exist. {DoYouMaybeHaveATypoInThe("directory path")}";

                public string MultipleFilesFound => "Multiple solutions found. Specify a single solution by using --solution with solution name.";

                public string InvalidFile => "The provided file is invalid. Specify a valid .sln file.";
            }
        }
    }
}
