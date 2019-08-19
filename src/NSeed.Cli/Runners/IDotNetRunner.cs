namespace NSeed.Cli.Runners
{
    // This interfaces is actually the "same" interface.
    // That's why we want to have them in the same file.
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1618 // Generic type parameters should be documented

    /// <summary>
    /// Interface for describing CLI DotNet command.
    /// </summary>
    /// <typeparam name="TArgs">Argument type that is describing required arguments for CLI command.</typeparam>
    internal interface IDotNetRunner<TArgs>
    {
        (bool IsSuccesful, string Message) Run(TArgs args);
    }

#pragma warning restore SA1618 // Generic type parameters should be documented
#pragma warning restore SA1402 // File may only contain a single type

    internal interface IDotNetRunner
    {
        (bool IsSuccesful, string Message) Run();
    }
}
