using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using System.Collections.Generic;
using CandidatesBrowserEngine.Models.Candidates;

namespace CandidatesBrowserEngine.Controllers
{
    [Authorize]
    public class CandidatesMainViewController : Controller
    {
        private readonly ICandidateService _candidateService;

        private readonly IProjectService _projectService;
        private readonly ICompanyService _companyService;

        public static int PageSize { get; set; } = 3;
        public  static bool PageChanged { get; set; }
        public static int PageNumber { get; set; }


        public static CandidateSearchViewModel ViewModel { get; set; }

        public CandidatesMainViewController(ICandidateService candidateService, IWebHostEnvironment webHostEnvironment, IProjectService projectService,
            ICompanyService companyService)
        {
            _candidateService = candidateService;
            _projectService = projectService;
            _companyService = companyService;
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(int? page)
        {
            var candidates = await _candidateService.GetAllCandidatesFiltered(new CandidateSearchViewModel());
            var projects = await _projectService.GetAllProjectsAsync();
            var companies = await _companyService.GetAllCompaniesAsync();
            ViewModel.PageNumber = page ?? 1;

 

            var candidateSearchVM = new CandidateSearchViewModel
            {
                Candidates = candidates.OrderBy(c => c.FullName).ToPagedList(ViewModel.PageNumber, PageSize),
                ProjectList = new MultiSelectList(projects.OrderBy(p => p.ProjectName).ToList(), "Id", "ProjectName"),
                CompaniesList = new MultiSelectList(companies.OrderBy(c => c.CompanyName).ToList(), "Id", "CompanyName"),
                RecordsCount = candidates.Count()

            };
            storeSearchSettings(candidateSearchVM);
            return View(candidateSearchVM);
        }
    
        [HttpGet]
        public async Task<IActionResult> Index( int? page, bool? pageChanged, CandidateSearchViewModel viewModel = null)
        {
            
            ViewModel ??= new();
            if (pageChanged != null) { ViewModel.PageChanged = (bool)pageChanged; }
            if (ViewModel.PageChanged)
            {
                overwriteSearchSettings(viewModel);
            }

            var candidates = await _candidateService.GetAllCandidatesFiltered(viewModel);
            var projects = await _projectService.GetAllProjectsAsync();
            var companies = await _companyService.GetAllCompaniesAsync();

            if (page == null && ViewModel.PageNumber == 0)
            {
                ViewModel.PageNumber = 1;
            }
            else if (viewModel.PageNumber > 0) { ViewModel.PageNumber = viewModel.PageNumber; }
            else { ViewModel.PageNumber = page ?? ViewModel.PageNumber; }

            viewModel.Candidates = candidates.OrderBy(c => c.FullName).ToPagedList(ViewModel.PageNumber, PageSize);
            viewModel.ProjectList = new MultiSelectList(projects.OrderBy(p => p.ProjectName).ToList(), "Id", "ProjectName");
            viewModel.CompaniesList = new MultiSelectList(companies.OrderBy(c => c.CompanyName).ToList(), "Id", "CompanyName");
            viewModel.RecordsCount = candidates.Count();

            storeSearchSettings(viewModel);

            return View(viewModel);
        }

      
        private  void overwriteSearchSettings(CandidateSearchViewModel viewModel)
        {
            viewModel.CompanyIds = ViewModel.CompanyIds;
            viewModel.ProjectIds = ViewModel.ProjectIds;
            viewModel.FirstName = ViewModel.FirstName;
            viewModel.LastName = ViewModel.LastName;
        }

        private  void storeSearchSettings(CandidateSearchViewModel viewModel)
        {
            
            ViewModel.CompanyIds = viewModel.CompanyIds;
            ViewModel.ProjectIds = viewModel.ProjectIds;
            ViewModel.FirstName = viewModel.FirstName;
            ViewModel.LastName = viewModel.LastName;
        }


            
    }
}
