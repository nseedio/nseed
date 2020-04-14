using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using NSeed;
using NSeed.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action = GettingThingsDone.Contracts.Model.Action;

namespace DotNetCoreSeeds
{
    [AlwaysRequired]
    public sealed class SomeAlwaysRequiredProject : ISeed<Project>
    {
        private readonly IProjectService projectService;
        private readonly IOutputSink output;

        public SomeAlwaysRequiredProject(IProjectService projectService, IOutputSink output)
        {
            output.WriteVerboseMessage($"Executing {nameof(MountEverestBaseCampTrack)}.ctor");

            this.projectService = projectService;
            this.output = output;
        }

        public async Task Seed()
        {
            await projectService.CreateOrUpdate(new ProjectDto { Name = Yield.SomeAlwaysRequiredProjectName });
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return (await projectService.GetAll()).Value.Any(project => project.Name == Yield.SomeAlwaysRequiredProjectName);
        }

        public class Yield : YieldOf<SomeAlwaysRequiredProject>
        {
            public const string SomeAlwaysRequiredProjectName = "Always required project";

            public async Task<ProjectDto> GetAlwaysRequiredProject()
            {
                return (await Seed.projectService.GetAll()).Value.First(project => project.Name == SomeAlwaysRequiredProjectName);
            }
        }
    }
}
