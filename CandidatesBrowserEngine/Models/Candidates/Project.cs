using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.Candidates
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string ProjectName { get; set; }
        public bool Deleted { get; set; }

        public IList<CandidateProject> CandidateProjects { get; set; }

    }
}
