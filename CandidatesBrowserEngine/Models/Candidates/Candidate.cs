using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.Candidates
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }



        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Email  { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public bool Deleted { get; set; }


        [Column(TypeName = "varchar(MAX)")]
        public string ProfilePicture { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }

        public IList<CandidateProject> CandidateProjects { get; set; }
        public IList<CandidateCompany> CandidateCompanies { get; set; }


    }
}
