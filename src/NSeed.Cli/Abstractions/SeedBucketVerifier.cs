using NSeed.Cli.Services;

namespace NSeed.Cli.Abstractions
{
    internal class SeedBucketVerifier : ISeedBucketVerifier
    {
        protected const string NSeed = "nseed";

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
            return response.IsSuccessful
                ? OperationResponse.Success()
                : OperationResponse.Error(response.Message);
        }

        // public IOperationResponse<Project> Detect(Project project)
        // {
        //     var response = DetectDependency(project);
        //     // response = IsExecutable(response);
        //     if (!response.IsSuccessful)
        //     {
        //         return response;
        //     }
        //     return response;
        // }
        // protected abstract IOperationResponse<Project> DetectDependency(Project project);
        // private IOperationResponse<Project> IsExecutable(IOperationResponse<Project> operationResponse)
        // {
        //     if (!operationResponse.IsSuccessful)
        //     {
        //         return operationResponse;
        //     }
        //     throw new NotImplementedException();
        // }
    }
}
