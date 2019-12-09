using NSeed.Cli.Services;

namespace NSeed.Cli.Abstractions
{
    internal class SeedBucketVerifier : ISeedBucketVerifier
    {
        protected string NSeed { get; } = nameof(NSeed).ToLower();

        protected IDependencyGraphService DependencyGraphService { get; }

        protected IFileSystemService FileSystemService { get; }

        protected INugetPackageDetector NugetPackageDetector { get; }

        public SeedBucketVerifier(
            IDependencyGraphService dependencyGraphService,
            IFileSystemService fileSystemService,
            INugetPackageDetector nugetPackageDetector
            )
        {
            DependencyGraphService = dependencyGraphService;
            FileSystemService = fileSystemService;
            NugetPackageDetector = nugetPackageDetector;
        }

        public IOperationResponse Verify(Project project)
        {
            var response = NugetPackageDetector.Detect(project, NSeed);

            // TODO:am
            // Is project .exe project is it console app
            // Does project contains SeedBucket class or have class that derive from SeedBucket

            return response.IsSuccessful
                ? OperationResponse.Success()
                : OperationResponse.Error(response.Message);
        }
    }
}
