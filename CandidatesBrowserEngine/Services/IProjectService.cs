using CandidatesBrowserEngine.Models.Candidates;
using CandidatesBrowserEngine.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public interface IProjectService
    {
        public Task<List<Project>> GetAllProjectsAsync();
        public Task<Project> GetProjectByIdAsync(int id);
    }
}
