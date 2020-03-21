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
    public sealed class MountEverestBaseCampTrack : ISeed<Project, Action, ActionList>
    {
        private readonly IProjectService projectService;
        private readonly IActionService actionService;
        private readonly IActionListService actionListService;
        private readonly IOutputSink output;
        private readonly ISomeSingletonService someSingletonService;

        public MountEverestBaseCampTrack(IProjectService projectService, IActionService actionService, IActionListService actionListService, IOutputSink output, ISomeSingletonService someSingletonService)
        {
            output.WriteVerboseMessage($"Executing {nameof(MountEverestBaseCampTrack)}.ctor");

            this.projectService = projectService;
            this.actionService = actionService;
            this.actionListService = actionListService;
            this.output = output;
            this.someSingletonService = someSingletonService;
        }

        public async Task Seed()
        {
            output.WriteVerboseMessage(string.Empty);
            output.WriteVerboseMessage($"=== Service lifecycle info {nameof(MountEverestBaseCampTrack)} ===");
            output.WriteVerboseMessage($"{nameof(IActionService)} unique value: {actionService.GetHashCode()}");
            output.WriteVerboseMessage($"{nameof(IOutputSink)} unique value: {output.GetHashCode()}");
            output.WriteVerboseMessage($"{nameof(ISomeSingletonService)} unique value: {someSingletonService.GetHashCode()}");
            output.WriteVerboseMessage($"=== Service lifecycle info {nameof(MountEverestBaseCampTrack)} ===");
            output.WriteVerboseMessage(string.Empty);

            // We attach the has code just to see in unit tests if a single or a new object is created every time.
            // This is our lab diagnostics and not the practice that we want to have in real seeds!
            var project = (await projectService.CreateOrUpdate(new ProjectDto { Name = Yield.MountEverestBaseCampTrackProjectName + GetHashCode() })).Value;

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
            return (await projectService.GetAll()).Value.Any(project => project.Name.StartsWith(Yield.MountEverestBaseCampTrackProjectName));
        }

        internal class Yield : YieldOf<MountEverestBaseCampTrack>
        {
            public const string MountEverestBaseCampTrackProjectName = "Mount Everest Base Camp track";

            public async Task<ProjectDto> GetMountEverestBaseCampTrackProject()
            {
                Seed.output.WriteVerboseMessage(string.Empty);
                Seed.output.WriteVerboseMessage($"=== Service lifecycle info {nameof(MountEverestBaseCampTrack)}.Yield ===");
                Seed.output.WriteVerboseMessage($"{nameof(IActionService)} unique value: {Seed.actionService.GetHashCode()}");
                Seed.output.WriteVerboseMessage($"{nameof(IOutputSink)} unique value: {Seed.output.GetHashCode()}");
                Seed.output.WriteVerboseMessage($"{nameof(ISomeSingletonService)} unique value: {Seed.someSingletonService.GetHashCode()}");
                Seed.output.WriteVerboseMessage($"=== Service lifecycle info {nameof(MountEverestBaseCampTrack)}.Yield ===");
                Seed.output.WriteVerboseMessage(string.Empty);

                return (await Seed.projectService.GetAll()).Value.First(project => project.Name.StartsWith(MountEverestBaseCampTrackProjectName));
            }
        }
    }
}
