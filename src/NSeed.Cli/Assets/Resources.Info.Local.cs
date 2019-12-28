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
                public static readonly string SeedBucketProjectCouldNotBeFound = "The seed bucket project could not be found.";
                public static readonly string SeedBucketProjectNameCouldNotBeDefined = "The seed bucket project name could not be defined from the provided *.csproj file.";
                public static readonly string SeedBucketProjectCouldNotBeBuilt = "The seed bucket project could not be built.";
            }

            internal class SearchNSeedProjectPathErrors : IFileSearchErrorMessage
            {
                private static readonly Lazy<SearchNSeedProjectPathErrors> Lazy =
                    new Lazy<SearchNSeedProjectPathErrors>(() => new SearchNSeedProjectPathErrors());

                public static SearchNSeedProjectPathErrors Instance => Lazy.Value;

                private SearchNSeedProjectPathErrors()
                {
                }

                public string WorkingDirectoryDoesNotContainAnyFile => "Could not find a project in the working directory. Ensure that a project exists in the working directory or any of its subdirectories, or pass the target project by using the --project option.";

                public string FilePathDirectoryDoesNotExist => $"The provided project directory does not exist. {DoYouMaybeHaveATypoInThe("directory path")}";

                public string MultipleFilesFound => "Multiple projects found. Specify a single project by using the --project option.";

                public string InvalidFile => "The provided file is invalid. Specify a valid .csproj file.";
            }
        }
    }
}
