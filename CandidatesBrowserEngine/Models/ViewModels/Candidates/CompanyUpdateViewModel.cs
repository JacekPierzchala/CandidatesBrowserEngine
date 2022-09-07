using CandidatesBrowserEngine.Models.Candidates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels
{
    public class CompanyUpdateViewModel
    {
        public int CompanyId { get; set; }

        public int Id { get; set; }
        public int CandidateId { get; set; }

        public string DateStart { get; set; }

        public string DateEnd { get; set; }

    }
}
