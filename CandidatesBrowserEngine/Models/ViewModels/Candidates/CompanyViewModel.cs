using CandidatesBrowserEngine.Models.Candidates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels
{
    public class CompanyViewModel 
    {
        public CandidateDetailsViewModel CandidateDetailsViewModel { get; set; }

        public Guid TempKey { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CandidateId { get; set; }

        public string CompanyName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateEnd { get; set; }
        public string DateRange { get => $"{DateStartString} - {DateEndString}"; }
        public string DateEndString { get; set; }
        public string DateStartString { get; set; }
    }
}
