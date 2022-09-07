using CandidatesBrowserEngine.Models.Candidates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public interface ICompanyService
    {
        public Task<List<Company>> GetAllCompaniesAsync();
        public Task<Company> GetCompanyByIdAsync(int id);
    }
}