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
    internal sealed class MountEverestBaseCampTrackNextSteps : ISeed<Action, ActionList>
    {
        private readonly IActionService actionService;
        private readonly IActionListService actionListService;
        private readonly IOutputSink output;
        private readonly ISomeSingletonService someSingletonService;

        private MountEverestBaseCampTrack.Yield MountEverestBaseCampTrack { get; set; }

        public MountEverestBaseCampTrackNextSteps(IActionService actionService, IActionListService actionListService, IOutputSink output, ISomeSingletonService someSingletonService)
        {
            output.WriteVerboseMessage($"Executing {nameof(MountEverestBaseCampTrackNextSteps)}.ctor");

            this.actionService = actionService;
            this.actionListService = actionListService;
            this.output = output;
            this.someSingletonService = someSingletonService;

            // TODO: How to silence the compiler?
            MountEverestBaseCampTrack = new MountEverestBaseCampTrack.Yield();
        }

        public async Task Seed()
        {
            output.WriteVerboseMessage(string.Empty);
            output.WriteVerboseMessage($"=== Service lifecycle info {nameof(MountEverestBaseCampTrackNextSteps)} ===");
            output.WriteVerboseMessage($"{nameof(IActionService)} unique value: {actionService.GetHashCode()}");
            output.WriteVerboseMessage($"{nameof(IOutputSink)} unique value: {output.GetHashCode()}");
            output.WriteVerboseMessage($"{nameof(ISomeSingletonService)} unique value: {someSingletonService.GetHashCode()}");
            output.WriteVerboseMessage($"=== Service lifecycle info {nameof(MountEverestBaseCampTrackNextSteps)} ===");
            output.WriteVerboseMessage(string.Empty);

            var project = await MountEverestBaseCampTrack.GetMountEverestBaseCampTrackProject();

            var actionNames = new[] { "Check visa requirements", "See which equipment can be borrowed in Kathmandu" };
            var actions = new List<ActionDto>();
            foreach (var actionName in actionNames)
            {
                actions.Add((await actionService.CreateOrUpdate(new ActionDto { Title = actionName, ProjectId = project.Id })).Value);
            }

            var doneList = (await actionListService.CreateOrUpdate(new ActionListDto { Name = "Next steps" })).Value;
            foreach (var action in actions)
            {
                await actionService.MoveToList(action.Id, doneList.Id);
            }
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return (await actionListService.GetAll()).Value.Any(list => list.Name == "Next steps");
        }
    }
}
