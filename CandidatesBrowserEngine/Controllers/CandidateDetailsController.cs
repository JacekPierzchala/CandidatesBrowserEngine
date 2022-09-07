using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Controllers
{
    [Authorize]
    public class CandidateDetailsController : Controller
    {
        private readonly ICandidateService _candidateService;

        public static CandidateDetailsViewModel ViewModel { get; set; }
        public static CompanyEditViewModel CompanyUpdateViewModel { get; set; }

        public static string CurrentPicture { get; set; }


        public CandidateDetailsController(ICandidateService candidateService)
        {
            _candidateService = candidateService;

        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            //return RedirectToAction("Login", "Account");
            if (id==0 && ViewModel.Id==0) 
            { 
                return RedirectToAction("Index", "CandidatesMainView", new { pageChanged = true });
            }

            var candidate = await _candidateService.GetCandidate(id!=0?id:ViewModel.Id);

            if (candidate == null)
            {
                return NotFound();
            }
            ViewModel = candidate;
            return View(candidate);
        }

      
    
    }
}
