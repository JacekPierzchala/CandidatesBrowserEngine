using CandidatesBrowserEngine.Models.Candidates;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels
{
    public class CandidateDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
        public string ProfilePicture { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string Description { get; set; }

        [Required]
        public string Email { get; set; }


        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public IList<CompanyViewModel> Companies { get; set; } = new List<CompanyViewModel>();
        public IList<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();
    }
}
