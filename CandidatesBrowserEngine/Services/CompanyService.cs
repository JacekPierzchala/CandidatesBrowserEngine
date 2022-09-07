using CandidatesBrowserEngine.Data;
using CandidatesBrowserEngine.Models.Candidates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public class CompanyService : BaseDbService, ICompanyService
    {
        public CompanyService(ApplicationDbContext dbContext) : base(dbContext)
        { }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            var companies = await _dbContext.Companies.Where(p => !p.Deleted)
                          .ToListAsync();

            return companies;
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);

            return company;
        }
    }
}
