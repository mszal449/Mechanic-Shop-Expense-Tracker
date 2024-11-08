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

        // GET: api/jobs
        [HttpGet]
        public async Task<IActionResult> GetAllJobs(
            int pageNumber = 1,
            int pageSize = 10,
            int? jobId = null,
            string? name = null,
            string? description = null,
            string? carModel = null,
            Status? jobStatus = null,
            string? supervisor = null,
            decimal? price = null)
        {


            var jobs = await _jobService.GetJobsAsync(
            pageNumber,
            pageSize,
            jobId,
            name,
            description,
            carModel,
            jobStatus,
            supervisor,
            price);

            return Ok(jobs);
        }

        // GET: api/jobs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        // POST: api/jobs
        [HttpPost]
        public async Task<ActionResult<Job>> CreateJob(Job job)
        {
            var createdJob = await _jobService.CreateJobAsync(job);
            return CreatedAtAction(nameof(GetJob), new { id = createdJob.JobId }, createdJob);
        }

        // PUT: api/jobs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, Job job)
        {
            if (id != job.JobId)
            {
                return BadRequest();
            }

            var updatedJob = await _jobService.UpdateJobAsync(id, job);

            if (updatedJob == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/jobs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var deleted = await _jobService.DeleteJobAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("bulk-delete")]
        public async Task<IActionResult> BulkDeleteJobs(List<int> jobIds)
        {
            var deleted = await _jobService.BulkDeleteJobsAsync(jobIds);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/jobs/statistics
        [HttpGet]
        [Route("statistics")]
        public async Task<IActionResult> Statistics()
        {
            var result = await _jobService.GetStatisticsAsync();
            return Ok(result);
        }
    }
}
