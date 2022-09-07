using CandidatesBrowserEngine.Data;
using CandidatesBrowserEngine.Models.Candidates;
using CandidatesBrowserEngine.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public class ProjectService: BaseDbService, IProjectService
    {

        public ProjectService(ApplicationDbContext dbContext) : base(dbContext){}
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            var projects = await _dbContext.Projects.Where(p => !p.Deleted)
                          .ToListAsync();

            return projects;
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(c => c.Id == id);
            return project;
        }

     
    }
}
