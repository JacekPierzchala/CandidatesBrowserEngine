using CandidatesBrowserEngine.Models.Candidates;
using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Controllers.Api
{
    [Authorize]
    [Route("api/CandidateDetails")]
    [ApiController]
    public class CompanyEditDetailsApiController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ICandidateCompanyService _candidateCompanyService;

        public CompanyEditDetailsApiController(ICompanyService companyService, IWebHostEnvironment hostEnvironment, ICandidateCompanyService candidateCompanyService)
        {
            _companyService = companyService;
            _hostEnvironment = hostEnvironment;
            _candidateCompanyService = candidateCompanyService;
        }

        [HttpGet]
        [Route("GetCompanyEditDetailsById/{id}/{candidateId}")]
        public async Task<IActionResult> GetCompanyEditDetailsById(int id, int candidateId)
        {
            var commonReponse = new CommonResponse<CompanyEditViewModel>();

            try
            {
                var companies = await _companyService.GetAllCompaniesAsync();
                CompanyViewModel companyViewModel;
                if (id!=0)
                {
                     companyViewModel = await _candidateCompanyService.GetCandidateCompany(id);
                }
                else
                {
                    companyViewModel = new CompanyViewModel
                    {
                        CandidateId = candidateId,
                        DateStart = DateTime.Now

                    };
                }
                var companyEditViewModel = new CompanyEditViewModel
                {
                    CompanyViewModel = companyViewModel,
                    CompaniesList = companies.Select(co => new SelectListItem
                    {
                        Text = co.CompanyName,
                        Value = co.Id.ToString()
                    })
                };

                commonReponse.dataenum = companyEditViewModel;
                commonReponse.status = 1;

            }
            catch (Exception ex)
            {
                commonReponse.message = ex.Message;
                commonReponse.status = 0;
            }

            return Ok(commonReponse);
        }


        [HttpPost]
        [Route("SaveCompanyEditDetails")]
        public async Task<IActionResult> SaveCompanyEditDetails(CompanyUpdateViewModel companyViewModel)

        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (companyViewModel.Id!=0)
                {
                    await _candidateCompanyService.UpdateCandidateCompany(companyViewModel);
                    commonResponse.status = 1;
                    commonResponse.message = "saving success";
                }
                else
                {
                    await _candidateCompanyService.AddCandidateCompany(companyViewModel);
                    commonResponse.status = 2;
                    commonResponse.message = "saving success";
                }
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = 0;
                throw;
            }

            return Ok(commonResponse);
        }


        [HttpPost]
        [Route("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)

        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                    var result=await _candidateCompanyService.DeleteCandidateCompany(id);
                    if(result)
                    {
                        commonResponse.status = 1;
                        commonResponse.message = "saving success";
                    }
                    else
                    {
                        commonResponse.status = 0;
                        commonResponse.message = "record not found";
                    }

                
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = 0;
                throw;
            }

            return Ok(commonResponse);
        }
    }
}
