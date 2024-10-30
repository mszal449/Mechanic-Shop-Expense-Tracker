using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllJobs(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? jobId = null,
            [FromQuery] string? name = null,
            [FromQuery] string? description = null,
            [FromQuery] string? carModel = null,
            [FromQuery] Status? jobStatus = null,
            [FromQuery] string? supervisor = null,
            [FromQuery] string? price = null)
        {
            // Try parsing request value to Decimal object
            Decimal? newPrice = null;
            if (price is not null)
            {
                try
                {
                    newPrice = Decimal.Parse(price);
                } catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = e.Message });
                }
            }

            var jobs = await _jobService.GetAllJobs(
            pageNumber,
            pageSize,
            jobId,
            name,
            description,
            carModel,
            jobStatus,
            supervisor,
            newPrice);

            return Ok(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewJob([FromBody] CreateJobRequest createJobRequest)
        {
            try
            {
                // Try parsing request data to model
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
                JobStatus = Enum.Parse<Status>(this.jobStatus, true),
                Supervisor = this.supervisor,
                Price = Decimal.Parse(this.price),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
    
public record GetJobsResponse(List<Job> jobs, int length)
    {
        public static GetJobsResponse FromJobList(List<Job> jobs)
        {
            return new GetJobsResponse(jobs, jobs.Count);
        }
    }
}
