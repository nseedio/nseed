using NSeed.Cli.Abstractions;
using System;

namespace NSeed.Cli.Assets
{
    internal static partial class Resources
    {
        internal static partial class Info
        {
            internal static class Errors
            {
                public static readonly string NSeedProjectCouldNotBeFound = "The NSeed project could not be found.";
                public static readonly string SeedBucketProjectNameCouldNotBeDefined = "The SeedBucket project name could not be defined from provided *.csproj file";
                public static readonly string SeedBucketProjectCouldNotBeBuild = "The SeedBucket project could not be build.";
            }

            internal class SearchNSeedProjectPathErrors : IFileSearchErrorMessage
            {
                private static readonly Lazy<SearchNSeedProjectPathErrors> Lazy =
                    new Lazy<SearchNSeedProjectPathErrors>(() => new SearchNSeedProjectPathErrors());

                public static SearchNSeedProjectPathErrors Instance => Lazy.Value;

                private SearchNSeedProjectPathErrors()
                {
                }

                public string WorkingDirectoryDoesNotContainAnyFile => "Could not find a project in the working directory. Ensure that a project exists in the working directory or any of its subdirectories, or pass the target project by using --project.";

                public string FilePathDirectoryDoesNotExist => $"The provided project directory does not exist. {DoYouMaybeHaveATypoInThe("directory path")}";

                public string MultipleFilesFound => "Multiple projects found. Specify a single project by using --project with project name.";

                public string InvalidFile => "The provided file is invalid. Specify a valid .csproj file.";
            }
        }
    }
}
