using NSeed.Cli.Abstractions;
using System;

namespace NSeed.Cli.Assets
{
    internal static partial class Resources
    {
        internal static class New
        {
            public const string CommandDescription = "Create a new seed bucket project.";

            public const string FrameworkDescription = "The target framework. For .NET Core project use netcoreappX.Y. For .NET Classic project use netframeworkX.Y. For example: netcoreapp2.0. The default framework is determined based on frameworks already used in the target solution.";
            public const string SolutionDescription = "The target solution. For example: MySolution.sln, SubFolderWithSolution, C:\\Path\\To\\Several\\Solutions\\SomeSolution.sln. The default solution is the nearest single solution found in the working directory or its subdirectories.";
            public const string ProjectNameDescription = "The project name. The default name is " + DefaultProjectName + " (if a project with that name does not already exist).";

            public static string SuccessfulRun(string projectName) => $"The seed bucket project '{projectName}' was created successfully.";

            public static string NSeedNuGetPackageHasToBeAddedManuallyToTheProject(string projectName) =>
                $"The created project does not reference the NSeed NuGet package.{Environment.NewLine}" +
                $"You need to add the NuGet package to the project manually, by using any of the usual methods for adding a NuGet package to a project.{Environment.NewLine}" +
                $"For example:{Environment.NewLine}" +
                $"  - by using the 'Manage NuGet Packages...' option in Visual Studio{Environment.NewLine}" +
                $"  - by using the Package Manager: Install-Package NSeed -ProjectName {projectName}{Environment.NewLine}" +
                $"  - by using the Paket CLI:       paket add NSeed"; // TODO: Check Paket docs and add the project name once the airplane lands :-) Also, check if Paket works with .NET Classic.

               // We cannot propose .NET CLI here because adding packages does not work with .NET Classic.
               // Otherwise we would call it in the command and automatically add the package ;-)

            public const string DefaultProjectName = "Seeds";

            public const int MinProjectNameCharacters = 3;
            public const int MaxProjectNameCharacters = 50;

            internal static class Errors
            {
                public static readonly string InvalidSolution = "The provided solution file is invalid or corrupted. Use the --solution option to set a valid solution file.";

                public static readonly string FrameworkNotProvided = "The framework is not provided or could not be determined based on frameworks already used in the target solution. Use the --framework option to set a valid framework.";
                public static readonly string InvalidFramework = $"The provided framework is invalid. {DoYouMaybeHaveATypoInThe("framework name")}";
                public static readonly string InvalidDotNetCoreVersion = $"The provided version of .NET Core framework is not supported. The supported versions are: {string.Join(", ", DotNetCoreVersions)}.";
                public static readonly string InvalidDotNetClassicVersion = $"The provided version of .NET Classic framework is not supported. The supported versions are: {string.Join(", ", DotNetClassicVersions)}.";

                public static readonly string ProjectNameNotProvided = $"The project name is not provided or could not be assigned automatically. {UseNameOptionToSetProjectName()}";
                public static readonly string ProjectNameContainsUnallowedCharacters = $"The provided project name contains unallowed characters. {UseNameOptionToSetProjectName("a valid")}";
                public static readonly string InvalidProjectName = $"The provided project name is invalid (contains reserved words like PRN or COM1). {UseNameOptionToSetProjectName("a valid")}";
                public static readonly string ProjectNameExists = $"The project name already exists in the provided solution. {UseNameOptionToSetProjectName("a new")}";
                public static readonly string ProjectNameTooLong = "The provided project name is too long. The maximum length of the project name is " + MaxProjectNameCharacters + " characters.";
                public static readonly string ProjectNameTooShort = "The provided project name is too short. The minimum length of the project name is " + MinProjectNameCharacters + " characters.";
                public static readonly string ProjectNotAddedToSolution = "The provided project is not added to the solution.";

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

                public string WorkingDirectoryDoesNotContainAnyFile => "Could not find a solution in the working directory. Ensure that a solution exists in the working directory or any of its subdirectories, or pass the target solution by using the --solution option.";

                public string FilePathDirectoryDoesNotExist => $"The provided solution directory does not exist. {DoYouMaybeHaveATypoInThe("directory path")}";

                public string MultipleFilesFound => "Multiple solutions found. Specify a single solution by using the --solution option.";

                public string InvalidFile => "The provided file is invalid. Specify a valid .sln file.";
            }
        }
    }
}
