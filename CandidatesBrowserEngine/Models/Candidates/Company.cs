using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.Candidates
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public bool Deleted { get; set; }

        public IList<CandidateCompany> CandidateCompanies { get; set; }
    }
}
