using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.Candidates
{
    public class CandidateProject
    {
        [Key]
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
