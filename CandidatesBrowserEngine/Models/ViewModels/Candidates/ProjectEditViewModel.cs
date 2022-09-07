using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels.Candidates
{
    public class ProjectEditViewModel
    {
        public ProjectViewModel ProjectViewModel { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}
