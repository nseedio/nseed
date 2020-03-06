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
    internal sealed class RentANewApartment : ISeed<Project, Action, ActionList>
    {
        private readonly IProjectService projectService;
        private readonly IActionService actionService;
        private readonly IActionListService actionListService;

        public RentANewApartment(IProjectService projectService, IActionService actionService, IActionListService actionListService)
        {
            this.projectService = projectService;
            this.actionService = actionService;
            this.actionListService = actionListService;
        }

        public async Task Seed()
        {
            var project = (await projectService.CreateOrUpdate(new ProjectDto { Name = "Rent a new apartment" })).Value;

            var actionNames = new[] { "Find apartment" };
            var actions = new List<ActionDto>();
            foreach (var actionName in actionNames)
            {
                actions.Add((await actionService.CreateOrUpdate(new ActionDto { Title = actionName, ProjectId = project.Id })).Value);
            }

            var todoList = (await actionListService.CreateOrUpdate(new ActionListDto { Name = "ToDo" })).Value;
            foreach (var action in actions)
            {
                await actionService.MoveToList(action.Id, todoList.Id);
            }
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return (await projectService.GetAll()).Value.Any(project => project.Name == "Rent a new apartment");
        }

        internal class Yield : YieldOf<RentANewApartment>
        {
            // TODO: Use the Yield class to provide the yield of this seed to other seeds.
            //       To learn how to use the Yield class see TODO-URL.
        }
    }
}
