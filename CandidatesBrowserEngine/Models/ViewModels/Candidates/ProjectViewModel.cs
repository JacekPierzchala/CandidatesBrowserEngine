using CandidatesBrowserEngine.Models.Candidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels
{
    public class ProjectViewModel
    {
        public CandidateDetailsViewModel CandidateDetailsViewModel { get; set; }
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public int CandidateId { get; set; }
    }
}
