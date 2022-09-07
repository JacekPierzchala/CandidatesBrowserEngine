using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels
{
    public class CompanyEditViewModel
    {
        public CompanyViewModel CompanyViewModel { get; set; }
        public IEnumerable<SelectListItem> CompaniesList { get; set; } 
    }
}
