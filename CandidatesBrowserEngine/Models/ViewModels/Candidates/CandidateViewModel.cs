using CandidatesBrowserEngine.Models.Candidates;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels
{
    public class CandidateViewModel : Candidate
    {

        public IList<CompanyViewModel> Companies { get; set; } = new List<CompanyViewModel>();
        public IList<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();
        public string FullName { get => $"{FirstName} {LastName}"; }


        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
    }
}
