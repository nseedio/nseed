﻿using GettingThingsDone.Contracts.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GettingThingsDone.Contracts.Interface
{
    public interface IProjectService
    {
        Task<ServiceResult<ProjectDto>> GetProject(int id);

        Task<ServiceResult<List<ProjectDto>>> GetAll();

        Task<ServiceResult<ProjectDto>> CreateOrUpdate(ProjectDto projectDto);

        Task<ServiceResult<ProjectDto>> CreateFromLegacyExchangeFormat(string legacyExchangeValue);

        Task<ServiceResult<List<ActionDto>>> GetProjectActions(int id);

        Task<ServiceResult> Delete(int id);
    }
}
