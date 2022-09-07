using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Models.ViewModels.Account
{
    public class EditUserViewModel
    {
        public UserViewModel UserViewModel { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
