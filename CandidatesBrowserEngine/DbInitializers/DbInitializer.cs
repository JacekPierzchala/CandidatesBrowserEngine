using CandidatesBrowserEngine.Data;
using CandidatesBrowserEngine.Models;
using CandidatesBrowserEngine.Models.Candidates;
using CandidatesBrowserEngine.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.DbInitializers
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize(string adminL, string adminP)
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (_dbContext.Roles.Any(x => x.Name == Helper.Admin))
            {
                return;
            }

            _dbContext.Companies.Add(new Company { CompanyName= "Avengers",Deleted=false });
            _dbContext.Companies.Add(new Company { CompanyName = "Fellowship of the Ring", Deleted = false });
            _dbContext.Companies.Add(new Company { CompanyName = "Hobbits", Deleted = false });
            _dbContext.Companies.Add(new Company { CompanyName = "Pirates of the Caribbean", Deleted = false });

            _dbContext.Projects.Add(new Project { ProjectName= "Avengers: End Game",Deleted=false });
            _dbContext.Projects.Add(new Project { ProjectName = "Iron Man 2", Deleted = false });
            _dbContext.Projects.Add(new Project { ProjectName = "Iron Man 3", Deleted = false });
            _dbContext.Projects.Add(new Project { ProjectName = "Lord of The Rings", Deleted = false });
            _dbContext.Projects.Add(new Project { ProjectName = "Return of The King", Deleted = false });
            _dbContext.Projects.Add(new Project { ProjectName = "The Hobbit: An Unexpected Journey", Deleted = false });
            _dbContext.Projects.Add(new Project { ProjectName = "The Hobbit: The Battle of the Five Armies", Deleted = false });
            _dbContext.Projects.Add(new Project { ProjectName = "The Hobbit: The Desolation of Smaug", Deleted = false });
            _dbContext.Projects.Add(new Project { ProjectName = "Thor: Ragnarok", Deleted = false });
            _dbContext.Projects.Add(new Project { ProjectName = "Two Towers", Deleted = false });

            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Helper.User)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = adminL,
                    Email = adminL,
                    EmailConfirmed = true,
                    Name = adminL,
                    DateCreated=DateTime.Now
                }, adminP)
                .GetAwaiter()
                .GetResult();

            var user = _dbContext.Users.FirstOrDefault(e => e.Email == adminL);
            _userManager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult();
        }
    }
}
