using CandidatesBrowserEngine.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;


namespace CandidatesBrowserEngine.Models.ViewModels
{
    public class CandidateSearchViewModel
    {

        public IPagedList<CandidateViewModel> Candidates { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public MultiSelectList ProjectList { get; set; }
        public int[] ProjectIds { get; set; }

        public MultiSelectList CompaniesList { get; set; }
        public int[] CompanyIds { get; set; }

        public bool PageChanged { get; set; }

        public int PageNumber { get; set; }

        public int RecordsCount { get; set; }

        
    }
}
