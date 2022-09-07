using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Models.ViewModels.Candidates;
using CandidatesBrowserEngine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Controllers.Api
{
    [Authorize]
    [Route("api/CandidateDetails")]
    [ApiController]
    public class ProjectEditDetailsApiController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICandidateProjectService _candidateProjectService;

        public ProjectEditDetailsApiController(IProjectService projectService, ICandidateProjectService candidateProjectService)
        {
            _projectService = projectService;
            _candidateProjectService = candidateProjectService;
        }

        [HttpGet]
        [Route("GetProjectEditDetailsById/{id}/{candidateId}")]
        public async Task<IActionResult> GetProjectEditDetailsById(int id, int candidateId)
        {
            var commonReponse = new CommonResponse<ProjectEditViewModel>();

            try
            {
                var projects = await _projectService.GetAllProjectsAsync();
                ProjectViewModel projectViewModel;
                if (id != 0)
                {
                    projectViewModel = await _candidateProjectService.GetCandidateProject(id);
                }
                else
                {
                    projectViewModel = new ProjectViewModel
                    {
                        CandidateId = candidateId
                    };
                }
                var companyEditViewModel = new ProjectEditViewModel
                {
                    ProjectViewModel = projectViewModel,
                    ProjectList = projects.Select(co => new SelectListItem
                    {
                        Text = co.ProjectName,
                        Value = co.Id.ToString()
                    })
                };

                commonReponse.dataenum = companyEditViewModel;
                commonReponse.status = 1;

            }
            catch (Exception ex)
            {
                commonReponse.message = ex.Message;
                commonReponse.status = 0;
            }

            return Ok(commonReponse);
        }


        [HttpPost]
        [Route("SaveProjectEditDetails")]
        public async Task<IActionResult> SaveProjectEditDetails(ProjectUpdateViewModel projectUpdateViewModel)

        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (projectUpdateViewModel.Id != 0)
                {
                    await _candidateProjectService.UpdateCandidateProject(projectUpdateViewModel);
                    commonResponse.status = 1;
                    commonResponse.message = "saving success";
                }
                else
                {
                    await _candidateProjectService.AddCandidateProject(projectUpdateViewModel);
                    commonResponse.status = 2;
                    commonResponse.message = "saving success";
                }
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = 0;
                throw;
            }

            return Ok(commonResponse);
        }


        [HttpPost]
        [Route("DeleteProject/{id}")]
        public async Task<IActionResult> DeleteProject(int id)

        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var result = await _candidateProjectService.DeleteCandidateProject(id);
                if (result)
                {
                    commonResponse.status = 1;
                    commonResponse.message = "saving success";
                }
                else
                {
                    commonResponse.status = 0;
                    commonResponse.message = "record not found";
                }


            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = 0;
                throw;
            }

            return Ok(commonResponse);
        }

    }
}
