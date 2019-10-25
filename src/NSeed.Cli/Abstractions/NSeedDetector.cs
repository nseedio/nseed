using NSeed.Cli.Services;
using System;

namespace NSeed.Cli.Abstractions
{
    internal abstract class NSeedDetector : IDetector
    {
        protected const string NSeed = "nseed";

        protected IDependencyGraphService DependencyGraphService { get; }

        protected IFileSystemService FileSystemService { get; }

        protected NSeedDetector(
            IDependencyGraphService dependencyGraphService,
            IFileSystemService fileSystemService)
        {
            DependencyGraphService = dependencyGraphService;
            FileSystemService = fileSystemService;
        }

        public IOperationResponse<Project> Detect(Project project)
        {
            var response = DetectDependency(project);

            // response = IsExecutable(response);

            if (!response.IsSuccessful)
            {
                return response;
            }

            return response;
        }

        protected abstract IOperationResponse<Project> DetectDependency(Project project);

        private IOperationResponse<Project> IsExecutable(IOperationResponse<Project> operationResponse)
        {
            if (!operationResponse.IsSuccessful)
            {
                return operationResponse;
            }

            throw new NotImplementedException();
        }
    }
}
