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
    public class CandidateService : BaseDbService, ICandidateService
    {
     

        public CandidateService(ApplicationDbContext dbContext):base(dbContext)
        {}

        public async Task<IEnumerable<CandidateViewModel>> GetAllCandidatesFiltered(CandidateSearchViewModel viewModel)
        {
            var candidates = await _dbContext.Candidates
                                    .Where(x => !x.Deleted &&
                                     (string.IsNullOrEmpty(viewModel.FirstName) || x.FirstName.ToUpper().Contains(viewModel.FirstName.ToUpper())) &&
                                     (string.IsNullOrEmpty(viewModel.LastName) || x.LastName.ToUpper().Contains(viewModel.LastName.ToUpper())))
                                    .Include(c => c.CandidateProjects).ThenInclude(p=>p.Project)
                                    .Include(c => c.CandidateCompanies).ThenInclude(cc => cc.Company)
                                    .Where(c => (viewModel.ProjectIds == null || c.CandidateProjects.Any(p => viewModel.ProjectIds.Any(pp => pp == p.ProjectId))) 
                                                &&
                                                (viewModel.CompanyIds == null || c.CandidateCompanies.Any(p => viewModel.CompanyIds.Any(pp => pp == p.CompanyId)))
                                    ).ToListAsync();

            return candidates.Select(x => 
                                    new CandidateViewModel
                                     {
                                         Id = x.Id,
                                         FirstName = x.FirstName,
                                         LastName = x.LastName,
                                         ProfilePicture=x.ProfilePicture,
                                         Projects = x.CandidateProjects.Take(3).Select(p =>
                                         new ProjectViewModel
                                         {
                                             Id = p.Id,
                                             ProjectId = p.Id,
                                             ProjectName = p.Project.ProjectName
                                         }).ToList(),
                                        Companies = x.CandidateCompanies.OrderByDescending(c => c.DateStart).Take(3).Select(cc =>
                                          new CompanyViewModel
                                          {
                                              Id = cc.CompanyId,
                                              CompanyName = cc.Company.CompanyName,
                                              DateStartString = cc.DateStart.ToString("yyyy/MM/dd"),
                                              DateEndString = cc.DateEnd == null ? "current" : ((DateTime)cc.DateEnd).ToString("yyyy/MM/dd")
                                          }).ToList()

                                     }).ToList();


        }

    
        public async Task<CandidateDetailsViewModel> GetCandidate(int id)
        {
            var candidateDb =  await _dbContext.Candidates.Where(c => c.Id == id)
                .Include(c => c.CandidateProjects).ThenInclude(p=>p.Project)
                .Include(c => c.CandidateCompanies).ThenInclude(cc => cc.Company)
                .FirstOrDefaultAsync();

            var candidateVM = new CandidateDetailsViewModel
            {
                Id = candidateDb.Id,
                FirstName = candidateDb.FirstName,
                LastName = candidateDb.LastName,
                ProfilePicture = candidateDb.ProfilePicture,
                Description = candidateDb.Description,
                DateOfBirth=candidateDb.DateOfBirth,
                Email=candidateDb.Email,
                Projects = candidateDb.CandidateProjects.OrderBy(p => p.Project.ProjectName).Select(p =>
                                  new ProjectViewModel
                                  {
                                      Id=p.Id,
                                      ProjectId = p.Project.Id,
                                      ProjectName = p.Project.ProjectName

                                  }).ToList(),
                Companies = candidateDb.CandidateCompanies.OrderByDescending(c => c.DateStart).Select(cc =>
                    new CompanyViewModel
                    {
                        Id = cc.Id,
   
                        CandidateId=cc.CandidateId,
                        CompanyName = cc.Company.CompanyName,
                        CompanyId = cc.CompanyId,
                        DateStart =cc.DateStart,
                        DateEnd=cc.DateEnd,
                        DateStartString = cc.DateStart.ToString("yyyy/MM/dd"),
                        DateEndString = cc.DateEnd == null ? "current" : ((DateTime)cc.DateEnd).ToString("yyyy/MM/dd")
                    }).ToList()


            };

            return candidateVM;
        }

        public async Task UpdateCandidate(CandidateDetailsViewModel viewModel)
        {
            var candidateDb = await _dbContext.Candidates.FirstOrDefaultAsync(c => c.Id == viewModel.Id);

            if (candidateDb!=null)
            {
                candidateDb.FirstName = viewModel.FirstName;
                candidateDb.LastName = viewModel.LastName;
                candidateDb.DateOfBirth = viewModel.DateOfBirth;
                candidateDb.Description = viewModel.Description;
                candidateDb.Email = viewModel.Email;
                candidateDb.ProfilePicture = viewModel.ProfilePicture;

              

                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<int> AddCandidate(CandidateDetailsViewModel viewModel)
        {
            var candidate = new Candidate
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                Deleted = false,
                Description = viewModel.Description,
                Email = viewModel.Email,
                ProfilePicture = viewModel.ProfilePicture
            };

            _dbContext.Candidates.Add(candidate);

            await _dbContext.SaveChangesAsync();


            return candidate.Id;

        }

        public async Task DeleteCandidate(int id)
        {
            var candidate=await _dbContext.Candidates.FirstOrDefaultAsync(c => c.Id == id);
            if(candidate!=null)
            {
                candidate.Deleted = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
