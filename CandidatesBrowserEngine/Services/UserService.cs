using CandidatesBrowserEngine.Data;
using CandidatesBrowserEngine.Models;
using CandidatesBrowserEngine.Models.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public class UserService : BaseDbService, IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        public async Task<bool> ChangeLockUser(string id)
        {
            bool success = false;
            try
            {
                var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (userFromDb != null)
                {
                    if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
                    {
                        //user is locked
                        userFromDb.LockoutEnd = DateTime.Now;

                    }
                    else
                    {
                        userFromDb.LockoutEnd = DateTime.Now.AddMinutes(15);

                    }
                }

                await _dbContext.SaveChangesAsync();
                success = true;
            }
            catch (Exception)
            {

                throw;
            }
         
            return success;
        }

        public async Task<bool> DeleteUser(string id)
        {
            bool success = false;
            try
            {
                var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                _dbContext.Users.Remove(userFromDb);
                await _dbContext.SaveChangesAsync();
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            var userRole = await _dbContext.UserRoles.ToListAsync();
            var roles = await _dbContext.Roles.ToListAsync();

            users.ForEach(u => 
            {

                var role = userRole.FirstOrDefault(ur => ur.UserId == u.Id);
                if (role == null)
                {
                    u.Role = "None";
                }
                else
                {
                    u.Role = roles.FirstOrDefault(r => r.Id == role.RoleId).Name;
                    u.RoleId = role.RoleId;
                }

            });

            return users.Select(x=> new UserViewModel 
            { 
                Id=x.Id,
                Name=x.Name,
                LockoutEnd=x.LockoutEnd,
                Role=x.Role,
                RoleId=x.RoleId,
                DateCreated=x.DateCreated,
                Email=x.Email,
                LastAccess=x.LastAccess            
            }).ToList();
        }

        public async Task<UserViewModel> GetUserById(string id)
        {
            var user = new UserViewModel { Id=string.Empty};

            var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            var roles = await _dbContext.Roles.ToListAsync();
            var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == id);
            if (userFromDb!=null)
            {
                user.Id = userFromDb.Id;
                user.Name = userFromDb.Name;
                user.LockoutEnd = userFromDb.LockoutEnd;
                user.Role = roles.FirstOrDefault(r => r.Id == userRole.RoleId).Name; ;
                user.RoleId = userRole.RoleId;
                user.DateCreated = userFromDb.DateCreated;
                user.Email = userFromDb.Email;
                user.LastAccess = userFromDb.LastAccess;

            }

            return user;

        }

        public async Task<bool> UpdateUser(UserViewModel viewModel)
        {
            bool success = false;
            try
            {
                var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == viewModel.Id);

                var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(r => r.UserId == viewModel.Id);

                if (userRole != null)
                {
                    var previousRoleName = await _dbContext.Roles.Where(r => r.Id == userRole.RoleId).Select(e => e.Name).FirstOrDefaultAsync();
                    await _userManager.RemoveFromRoleAsync(userFromDb, previousRoleName);

                }
                await _userManager.AddToRoleAsync(userFromDb, _dbContext.Roles.FirstOrDefault(r => r.Id == viewModel.RoleId).Name);

                userFromDb.Name = viewModel.Name;

               
                await _dbContext.SaveChangesAsync();
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }
    }

}
