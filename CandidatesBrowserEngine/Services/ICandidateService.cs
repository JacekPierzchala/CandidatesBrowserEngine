
using CandidatesBrowserEngine.Models.Candidates;
using CandidatesBrowserEngine.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public interface ICandidateService
    {

        public Task<IEnumerable<CandidateViewModel>> GetAllCandidatesFiltered(CandidateSearchViewModel viewModel);
        public Task<CandidateDetailsViewModel> GetCandidate(int id);

        public Task UpdateCandidate(CandidateDetailsViewModel viewModel);

        public Task<int> AddCandidate(CandidateDetailsViewModel viewModel);

        public Task DeleteCandidate(int id);

    }
}
