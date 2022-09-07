using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Utilities
{
    public static class Helper
    {
        public const string Admin = "Admin";
        public const string Recruiter = "Recruiter";
        public const string Candidate = "Candidate";
        public const string User = "User";

        public static int success_code = 1;
        public static int failure_code = 0;

        public const string Success = "Success";
        public const string Error = "Error";

        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{ Value=Helper.Admin,Text=Helper.Admin }

                };
            }
            else
            {
                return new List<SelectListItem>
                {

                    new SelectListItem{ Value=Helper.Recruiter,Text=Helper.User }//,
                    //new SelectListItem{ Value=Helper.Candidate,Text=Helper.Candidate }
                };
            }

        }
    }
}
