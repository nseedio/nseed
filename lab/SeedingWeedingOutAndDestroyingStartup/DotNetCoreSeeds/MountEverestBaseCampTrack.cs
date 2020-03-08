using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using NSeed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action = GettingThingsDone.Contracts.Model.Action;

namespace DotNetCoreSeeds
{
    internal sealed class MountEverestBaseCampTrack : ISeed<Project, Action, ActionList>
    {
        private readonly IProjectService projectService;
        private readonly IActionService actionService;
        private readonly IActionListService actionListService;

        public MountEverestBaseCampTrack(IProjectService projectService, IActionService actionService, IActionListService actionListService)
        {
            this.projectService = projectService;
            this.actionService = actionService;
            this.actionListService = actionListService;
        }

        public async Task Seed()
        {
            var project = (await projectService.CreateOrUpdate(new ProjectDto { Name = Yield.MountEverestBaseCampTrackProjectName })).Value;

            var actionNames = new[] { "Buy shoes", "Buy socks", "Borrow sleeping bag" };
            var actions = new List<ActionDto>();
            foreach (var actionName in actionNames)
            {
                actions.Add((await actionService.CreateOrUpdate(new ActionDto { Title = actionName, ProjectId = project.Id })).Value);
            }

            var doneList = (await actionListService.CreateOrUpdate(new ActionListDto { Name = "Done" })).Value;
            foreach (var action in actions)
            {
                await actionService.MoveToList(action.Id, doneList.Id);
            }
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return (await projectService.GetAll()).Value.Any(project => project.Name == Yield.MountEverestBaseCampTrackProjectName);
        }

        internal class Yield : YieldOf<MountEverestBaseCampTrack>
        {
            public const string MountEverestBaseCampTrackProjectName = "Mount Everest Base Camp track";

            public async Task<ProjectDto> GetMountEverestBaseCampTrackProject()
            {
                return (await Seed.projectService.GetAll()).Value.First(project => project.Name == MountEverestBaseCampTrackProjectName);
            }
        }
    }
}
