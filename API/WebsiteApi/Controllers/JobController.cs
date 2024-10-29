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
        public async Task<IActionResult> GetAllJobs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var data = await _jobService.GetAllJobs(pageNumber, pageSize);
            return Ok(data);
        }
    }


}
