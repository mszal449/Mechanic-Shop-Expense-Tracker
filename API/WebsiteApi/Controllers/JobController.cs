using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebsiteApi.Models;
using WebsiteApi.Services;

namespace WebsiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController(JobService jobService) : ControllerBase
    {
        private readonly JobService _jobService = jobService;

        [HttpGet]
        public async Task<IActionResult> GetAllJobs([FromQuery] int? pageSize, [FromQuery] int pageNumber = 1)
        {
            int size = pageSize ?? 10;
            var data = await _jobService.GetAllJobs(pageNumber, size);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewJob([FromBody] CreateJobRequest createJobRequest)
        {
            try
            {
                // to Job model
                var job = createJobRequest.ToModel();
                var result = await _jobService.AddJob(job);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
    }


    public record CreateJobRequest(
        string name,
        string? description,
        string carModel,
        string jobStatus,
        string price,
        string? supervisor
    )
    {
        public Job ToModel()
        {
            return new Job
            {
                Name = this.name,
                Description = this.description,
                CarModel = this.carModel,
                JobStatus = Enum.Parse<Status>(this.jobStatus, true), // Parse the string to Status enum
                Supervisor = this.supervisor,
                Price = Decimal.Parse(this.price),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
