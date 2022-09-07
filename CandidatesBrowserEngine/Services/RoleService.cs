using CandidatesBrowserEngine.Data;
using CandidatesBrowserEngine.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public class RoleService : BaseDbService, IRoleService
    {
        public RoleService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<IdentityRole>> GetAllRoles()
        {
            var roles = await _dbContext.Roles.ToListAsync();
            return roles;
        }
    }
}
