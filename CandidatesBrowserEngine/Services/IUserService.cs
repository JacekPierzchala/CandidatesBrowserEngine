using CandidatesBrowserEngine.Models;
using CandidatesBrowserEngine.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserViewModel>> GetAllUsers();
        public Task<UserViewModel> GetUserById(string id);

        public Task<bool> ChangeLockUser(string id);

        public Task<bool> UpdateUser(UserViewModel viewModel);
        public Task<bool> DeleteUser(string id);
    }
}
