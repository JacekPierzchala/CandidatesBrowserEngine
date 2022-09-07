using CandidatesBrowserEngine.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? LastAccess { get; set; }

        [NotMapped]
        public string RoleId { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
