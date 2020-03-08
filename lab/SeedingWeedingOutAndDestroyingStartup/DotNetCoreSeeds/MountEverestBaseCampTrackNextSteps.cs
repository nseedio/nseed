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
    internal sealed class MountEverestBaseCampTrackNextSteps : ISeed<Action, ActionList>
    {
        private readonly IActionService actionService;
        private readonly IActionListService actionListService;

        private MountEverestBaseCampTrack.Yield MountEverestBaseCampTrack { get; set; }

        public MountEverestBaseCampTrackNextSteps(IActionService actionService, IActionListService actionListService)
        {
            this.actionService = actionService;
            this.actionListService = actionListService;

            // TODO: How to silence the compiler?
            MountEverestBaseCampTrack = new MountEverestBaseCampTrack.Yield();
        }

        public async Task Seed()
        {
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
