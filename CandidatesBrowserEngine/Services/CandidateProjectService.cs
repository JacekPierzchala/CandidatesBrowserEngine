using CandidatesBrowserEngine.Data;
using CandidatesBrowserEngine.Models.Candidates;
using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Models.ViewModels.Candidates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public class CandidateProjectService: BaseDbService, ICandidateProjectService
    {
        public CandidateProjectService(ApplicationDbContext dbContext):base(dbContext){}

        public async Task AddCandidateProject(ProjectUpdateViewModel projectUpdateViewmodel)
        {
            var candidateProject = new CandidateProject
            {
                ProjectId=projectUpdateViewmodel.ProjectId,
                CandidateId = projectUpdateViewmodel.CandidateId
            };
            _dbContext.CandidateProjects.Add(candidateProject);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteCandidateProject(int id)
        {
            var result = false;
            var candidateProject = await _dbContext.CandidateProjects.FirstOrDefaultAsync(e => e.Id == id);
            if (candidateProject != null)
            {
                _dbContext.CandidateProjects.Remove(candidateProject);
                await _dbContext.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task<ProjectViewModel> GetCandidateProject(int id)
        {
            var candidateProject = await _dbContext.CandidateProjects.FirstOrDefaultAsync(c => c.Id == id);
            ProjectViewModel projectViewModel;
            if (candidateProject != null)
            {
                projectViewModel = new ProjectViewModel
                {
                    CandidateId = candidateProject.CandidateId,
                    ProjectId=candidateProject.ProjectId,
                    Id = candidateProject.Id

                };
            }
            else
            {
                projectViewModel = new ProjectViewModel { Id = 0 };
            }

            return projectViewModel;

        }

        public async Task UpdateCandidateProject(ProjectUpdateViewModel projectUpdateViewmodel)
        {
            var candidateProject = await _dbContext.CandidateProjects.FirstOrDefaultAsync(c => c.Id == projectUpdateViewmodel.Id);

            if (candidateProject != null)
            {
                candidateProject.ProjectId = projectUpdateViewmodel.ProjectId;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
