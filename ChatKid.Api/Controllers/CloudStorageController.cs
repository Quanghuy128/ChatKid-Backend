using ChatKid.GoogleServices.GoogleCloudStorage;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatKid.Api.Controllers
{
    [Route("api/cloud-storage")]
    [ApiController]
    public class CloudStorageController : ControllerBase
    {
        private readonly ICloudStorageService _cloudStorageService;

        public CloudStorageController(ICloudStorageService cloudStorageService)
        {
            _cloudStorageService = cloudStorageService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]

        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var response = await _cloudStorageService.UploadFile(file);
            return Ok(response);
        }

    }
}
