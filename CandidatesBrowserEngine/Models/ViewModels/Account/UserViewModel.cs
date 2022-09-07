using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels.Account
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? LastAccess { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }

    }
}
