using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Models.ViewModels.Candidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public interface ICandidateProjectService
    {
        public Task<ProjectViewModel> GetCandidateProject(int id);
        public Task AddCandidateProject(ProjectUpdateViewModel projectUpdateViewmodel);
        public Task UpdateCandidateProject(ProjectUpdateViewModel projectUpdateViewmodel);

        public Task<bool> DeleteCandidateProject(int id);
    }
}
