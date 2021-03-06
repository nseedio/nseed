namespace NSeed.Cli.Runners
{
    internal class RunStatus
    {
        public string Output { get; }

        public string Errors { get; }

        public int ExitCode { get; }

        public bool IsSuccess => ExitCode == 0 && string.IsNullOrEmpty(Errors);

        public RunStatus()
           : this(string.Empty, string.Empty, default) { }

        public RunStatus(string output, string errors, int exitCode)
        {
            Output = output;
            Errors = errors;
            ExitCode = exitCode;
        }

        public RunStatus(string output, string errors)
        {
            Output = output;
            Errors = errors;
            ExitCode = -1;
        }
    }
}
