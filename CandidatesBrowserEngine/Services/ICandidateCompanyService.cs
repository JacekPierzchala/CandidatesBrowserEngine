using CandidatesBrowserEngine.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public interface ICandidateCompanyService
    {
        public Task<CompanyViewModel> GetCandidateCompany(int id);
        public Task AddCandidateCompany(CompanyUpdateViewModel companyViewModel);
        public Task UpdateCandidateCompany(CompanyUpdateViewModel companyViewModel);

        public Task<bool> DeleteCandidateCompany(int id);

    }
}
