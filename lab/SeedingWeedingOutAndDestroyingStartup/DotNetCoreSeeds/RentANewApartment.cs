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
    public sealed class RentANewApartment : ISeed<Project, Action, ActionList>
    {
        private readonly IProjectService projectService;
        private readonly IActionService actionService;
        private readonly IActionListService actionListService;
        private readonly IOutputSink output;
        private readonly ISomeSingletonService someSingletonService;

        public RentANewApartment(IProjectService projectService, IActionService actionService, IActionListService actionListService, IOutputSink output, ISomeSingletonService someSingletonService)
        {
            this.projectService = projectService;
            this.actionService = actionService;
            this.actionListService = actionListService;
            this.output = output;
            this.someSingletonService = someSingletonService;
        }

        public async Task Seed()
        {
            output.WriteMessage("=== Service lifecycle info ===");
            output.WriteMessage($"{nameof(IActionService)} unique value: {actionService.GetHashCode()}");
            output.WriteMessage($"{nameof(IOutputSink)} unique value: {output.GetHashCode()}");
            output.WriteMessage($"{nameof(ISomeSingletonService)} unique value: {someSingletonService.GetHashCode()}");
            output.WriteMessage("=== Service lifecycle info ===");

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

        public class Yield : YieldOf<RentANewApartment>
        {
            public async Task<ProjectDto> GetRentANewApartmentProject()
            {
                return (await Seed.projectService.GetAll()).Value.First(project => project.Name == "Rent a new apartment");
            }
        }
    }
}
