namespace NSeed.Cli.Runners
{
#pragma warning disable SA1402
#pragma warning disable SA1618

    internal interface IDotNetRunner<TArgs>
    {
        (bool IsSuccessful, string Message) Run(TArgs args);
    }

#pragma warning restore SA1618
#pragma warning restore SA1402

    internal interface IDotNetRunner
    {
        (bool IsSuccessful, string Message) Run();
    }
}
