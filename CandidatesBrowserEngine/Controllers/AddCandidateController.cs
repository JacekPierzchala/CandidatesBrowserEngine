using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Controllers
{
    [Authorize]
    public class AddCandidateController : Controller
    {
        private readonly ICandidateService _candidateService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProjectService _projectService;
        private readonly ICompanyService _companyService;
        public static string DefaultPicture { get; set; } = "avatar.png";

        public static CandidateDetailsViewModel ViewModel { get; set; }
        public AddCandidateController(ICandidateService candidateService, IWebHostEnvironment hostEnvironment, IProjectService projectService, ICompanyService companyService)
        {
            _candidateService = candidateService;
            _hostEnvironment = hostEnvironment;
            _projectService = projectService;
            _companyService = companyService;
        }



        [HttpGet]
        public IActionResult AddNew()
        {
            ViewModel = new CandidateDetailsViewModel
            {
                DateOfBirth = DateTime.Now,
                ProfilePicture = DefaultPicture
            };

            return View(ViewModel);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddNew(CandidateDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                string uniqueFileName = uploadedFile(viewModel);
                viewModel.ProfilePicture = uniqueFileName?? DefaultPicture;
                viewModel.Companies = ViewModel.Companies;
                int newId=await _candidateService.AddCandidate(viewModel);

                return RedirectToAction("Index", "CandidateDetails", new { Id = newId });
            }

            viewModel.Companies = ViewModel.Companies;
            viewModel.ProfilePicture = DefaultPicture;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Company(string tempKey)
        {
            var companyViewModel = ViewModel.Companies.FirstOrDefault(c => c.TempKey.ToString() == tempKey);
            if(companyViewModel==null)
            {
                companyViewModel = new CompanyViewModel
                {
                    DateStart = DateTime.Now,
                    TempKey = Guid.NewGuid()

                };
            }
          

            var companies = await _companyService.GetAllCompaniesAsync();

            var companyUpdateViewModel = new CompanyEditViewModel
            {
                CompanyViewModel = companyViewModel,
                CompaniesList = companies.Select(co => new SelectListItem
                {
                    Text = co.CompanyName,
                    Value = co.Id.ToString()
                })
            };

            return PartialView("_CompanyPartial", companyUpdateViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Company(CompanyEditViewModel companyUpdateViewModel, string TempKey , bool delete)
        {
            if (!delete)
            {
                var companyViewModel = companyUpdateViewModel.CompanyViewModel;
                var company = await _companyService.GetCompanyByIdAsync(companyViewModel.CompanyId);
                companyViewModel.CompanyName = company.CompanyName;
                companyViewModel.DateStartString = companyViewModel.DateStart.ToString("yyyy/MM/dd");
                companyViewModel.DateEndString = companyViewModel.DateEnd == null ? "current" : ((DateTime)companyViewModel.DateEnd).ToString("yyyy/MM/dd");

                if (ViewModel.Companies.Any(x => x.TempKey == companyViewModel.TempKey))
                {
                    ViewModel.Companies.Remove(ViewModel.Companies.FirstOrDefault(x => x.TempKey == companyViewModel.TempKey));
                    ViewModel.Companies.Add(companyViewModel);
                }
                else
                {
                    ViewModel.Companies.Add(companyViewModel);
                }
            }
            else
            {
                ViewModel.Companies.Remove(ViewModel.Companies.FirstOrDefault(x => x.TempKey.ToString() == TempKey));
            }

            ViewModel.Companies = ViewModel.Companies.OrderByDescending(x => x.DateStart).ToList();

            return View(nameof(AddNew), ViewModel);

        }

        private string uploadedFile(CandidateDetailsViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
