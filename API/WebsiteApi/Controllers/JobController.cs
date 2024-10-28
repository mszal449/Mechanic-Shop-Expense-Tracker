using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebsiteApi.Services;

namespace WebsiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController(JobService jobService) : ControllerBase
    {
        private readonly JobService _jobService = jobService;


        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var data = await _jobService.GetAllJobs();
            return Ok(data);
        }
    }


}
