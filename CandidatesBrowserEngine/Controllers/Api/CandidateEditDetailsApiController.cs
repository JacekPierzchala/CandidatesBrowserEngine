using CandidatesBrowserEngine.Models.ViewModels;
using CandidatesBrowserEngine.Services;
using CandidatesBrowserEngine.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CandidatesBrowserEngine.Controllers.Api
{
    [Authorize]
    [Route("api/CandidateDetails")]
    [ApiController]
    public class CandidateEditDetailsApiController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICandidateService _candidateService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public static string CurrentPicture { get; set; }
        public static string OriginalPicture { get; set; }
        public static IFormFile UploadedPicture { get; set; }
        public CandidateEditDetailsApiController(IHttpContextAccessor contextAccessor, ICandidateService candidateService, IWebHostEnvironment hostEnvironment)
        {
            _contextAccessor = contextAccessor;
            _candidateService = candidateService;
            _hostEnvironment = hostEnvironment;
        }


        [HttpGet]
        [Route("GetCandidateEditDetailsById/{id}")]
        public async Task<IActionResult> GetCandidateEditDetailsById(int id)
        {
            var commonReponse = new CommonResponse<CandidateDetailsViewModel>();
            try
            {

                commonReponse.dataenum = await _candidateService.GetCandidate(id);
                OriginalPicture = commonReponse.dataenum.ProfilePicture;
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
        [Route("UploadPicture")]
        public async Task<IActionResult> UploadPicture(IFormFile file)
        {
            UploadedPicture = file;
            if(CurrentPicture!=null)
            {
                try
                {
                    System.IO.File.Delete(Path.Combine(Path.GetTempPath(), CurrentPicture));
                    
                }
                catch (Exception)
                {

                    throw;
                }
           
            }
            string uniqueFileName = await uploadedFileAsync(UploadedPicture);
            CurrentPicture = uniqueFileName;
            return Ok();
        }


        
        [HttpPost]
        [Route("SaveCandidateEditDetails")]
        public async Task<IActionResult> SaveCandidateEditDetails(CandidateDetailsViewModel viewModel)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            if (CurrentPicture != OriginalPicture && CurrentPicture != null)
            {
                System.IO.File.Delete(Path.Combine(_hostEnvironment.WebRootPath, "images", OriginalPicture));
                viewModel.ProfilePicture = CurrentPicture;
                System.IO.File.Move(Path.Combine(Path.GetTempPath(), CurrentPicture), Path.Combine(_hostEnvironment.WebRootPath, "images", CurrentPicture));

            }
            else
            {
                if(CurrentPicture!=null)
                {
                    System.IO.File.Delete(Path.Combine(Path.GetTempPath(), CurrentPicture));
                }
                viewModel.ProfilePicture = OriginalPicture;
            }
            try
            {
                if (viewModel.Id != 0)
                {
                    commonResponse.status = 1;
                    await _candidateService.UpdateCandidate(viewModel);
                }
                else
                {
                    commonResponse.status = 2;
                }
                commonResponse.message = "saving success";



            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = 0;
            }
            return Ok(commonResponse);
        }


        [HttpPost]
        [Route("DeleteCandidate/{id}")]
        [Authorize(Roles = Helper.Admin)]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var commonReponse = new CommonResponse<CandidateDetailsViewModel>();
            try
            {
                await _candidateService.DeleteCandidate(id);
                commonReponse.status = 1;
            }
            catch (Exception ex)
            {
                commonReponse.message = ex.Message;
                commonReponse.status = 0;
            }
            return Ok(commonReponse);
        }

        private async Task<string> uploadedFileAsync(IFormFile profileImage)
        {
            string uniqueFileName = null;

            if (profileImage != null)
            {
                string uploadsFolder = Path.GetTempPath();
                uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    try
                    {
                        await profileImage.CopyToAsync(fileStream);
                    }
                    catch (Exception)
                    {
                        throw;

                    }

                }

            }
            return uniqueFileName;
        }



    }


}
