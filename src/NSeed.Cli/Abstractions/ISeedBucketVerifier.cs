namespace NSeed.Cli.Abstractions
{
    internal interface ISeedBucketVerifier
    {
        public IOperationResponse Verify(Project project);
    }
}
