using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.Candidates
{
    public class CandidateCompany
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateEnd { get; set; }


    }
}
