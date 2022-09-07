using CandidatesBrowserEngine.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public interface IRoleService
    {
        public Task<IList<IdentityRole>> GetAllRoles();
    }
}
