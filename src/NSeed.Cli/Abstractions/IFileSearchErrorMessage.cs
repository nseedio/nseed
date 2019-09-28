namespace NSeed.Cli.Abstractions
{
    internal interface IFileSearchErrorMessage
    {
        public string WorkingDirectoryDoesNotContainAnyFile { get; }

        public string FilePathDirectoryDoesNotExist { get; }

        public string MultipleFilesFound { get; }

        public string InvalidFile { get; }
    }
}
