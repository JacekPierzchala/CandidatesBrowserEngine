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
    public class CandidateCompanyService : BaseDbService, ICandidateCompanyService
    {
        public CandidateCompanyService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AddCandidateCompany(CompanyUpdateViewModel companyViewModel)
        {
            var candidateCompany = new CandidateCompany
            {
                CompanyId = companyViewModel.CompanyId,
                CandidateId = companyViewModel.CandidateId,
                DateStart = DateTime.Parse(companyViewModel.DateStart),
                DateEnd = string.IsNullOrEmpty(companyViewModel.DateEnd) ? null : DateTime.Parse(companyViewModel.DateEnd)
            };
            _dbContext.CandidateCompanies.Add(candidateCompany);

             await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteCandidateCompany(int id)
        {
            var result = false;
            var candidateCompany=await _dbContext.CandidateCompanies.FirstOrDefaultAsync(e => e.Id == id);
            if (candidateCompany!=null)
            {
                _dbContext.CandidateCompanies.Remove(candidateCompany);
                await _dbContext.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task<CompanyViewModel> GetCandidateCompany(int id)
        {
            var candidateCompany = await _dbContext.CandidateCompanies.FirstOrDefaultAsync(c => c.Id == id);
            CompanyViewModel companyViewModel;
            if (candidateCompany!=null)
            {

                companyViewModel = new CompanyViewModel
                {
                    CandidateId = candidateCompany.CandidateId,
                    CompanyId = candidateCompany.CompanyId,
                    Id = candidateCompany.Id,
                    CompanyName = candidateCompany.Company.CompanyName,
                    DateStart = candidateCompany.DateStart,
                    DateEnd = candidateCompany.DateEnd,
                    DateStartString = candidateCompany.DateStart.ToString("yyyy/MM/dd"),
                    DateEndString = candidateCompany.DateEnd == null ? "current" : ((DateTime)candidateCompany.DateEnd).ToString("yyyy/MM/dd")
                };
            }
            else
            {
                companyViewModel = new CompanyViewModel { Id=0 };
            }

            return companyViewModel;


        }

        public async Task UpdateCandidateCompany(CompanyUpdateViewModel companyViewModel)
        {
            var candidateCompany = await _dbContext.CandidateCompanies.FirstOrDefaultAsync(c => c.Id == companyViewModel.Id);

            if (candidateCompany != null)
            {
                candidateCompany.CompanyId = companyViewModel.CompanyId;
                candidateCompany.DateStart = DateTime.Parse(companyViewModel.DateStart);
                candidateCompany.DateEnd = string.IsNullOrEmpty(companyViewModel.DateEnd) ? null : DateTime.Parse(companyViewModel.DateEnd);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
