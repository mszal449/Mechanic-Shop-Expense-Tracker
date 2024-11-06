using Microsoft.EntityFrameworkCore;
using WebsiteApi.Context;
using WebsiteApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebsiteApi.Services
{
    /// <summary>
    /// Service for managing jobs.
    /// </summary>
    public class JobService
    {
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobService"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public JobService(DataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Retrieves a paginated list of jobs with optional filters.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="jobId">Optional filter by job ID.</param>
        /// <param name="name">Optional filter by job name.</param>
        /// <param name="description">Optional filter by job description.</param>
        /// <param name="carModel">Optional filter by car model.</param>
        /// <param name="jobStatus">Optional filter by job status.</param>
        /// <param name="supervisor">Optional filter by supervisor.</param>
        /// <param name="price">Optional filter by price.</param>
        /// <returns>A <see cref="JobResult"/> containing the list of jobs and the total count.</returns>
        public async Task<JobResult> GetJobsAsync(
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
            var query = _context.Jobs.AsQueryable();

            // Apply filters
            if (jobId.HasValue)
                query = query.Where(j => j.JobId == jobId.Value);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(j => j.Name.Contains(name));

            if (!string.IsNullOrEmpty(description))
                query = query.Where(j => j.Description.Contains(description));

            if (!string.IsNullOrEmpty(carModel))
                query = query.Where(j => j.CarModel.Contains(carModel));

            if (jobStatus.HasValue)
                query = query.Where(j => j.JobStatus == jobStatus.Value);

            if (!string.IsNullOrEmpty(supervisor))
                query = query.Where(j => j.Supervisor.Contains(supervisor));

            if (price.HasValue)
                query = query.Where(j => j.Price == price.Value);

            var totalCount = await query.CountAsync();

            // Pagination
            if (pageNumber > 0 && pageSize > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var jobs = await query.ToListAsync();
            return new JobResult
            {
                Jobs = jobs,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Retrieves a job by its ID.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        /// <returns>The job if found; otherwise, null.</returns>
        public async Task<Job?> GetJobByIdAsync(int id)
        {
            return await _context.Jobs
                .Include(j => j.Expenses)
                .FirstOrDefaultAsync(j => j.JobId == id);
        }

        /// <summary>
        /// Creates a new job.
        /// </summary>
        /// <param name="job">The job to create.</param>
        /// <returns>The created job.</returns>
        public async Task<Job> CreateJobAsync(Job job)
        {
            var result = await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Updates an existing job.
        /// </summary>
        /// <param name="id">The ID of the job to update.</param>
        /// <param name="job">The job with updated information.</param>
        /// <returns>The updated job if the update was successful; otherwise, null.</returns>
        public async Task<Job?> UpdateJobAsync(int id, Job job)
        {
            var existingJob = await _context.Jobs.FindAsync(id);
            if (existingJob == null)
            {
                return null;
            }

            existingJob.Name = job.Name;
            existingJob.Description = job.Description;
            existingJob.CarModel = job.CarModel;
            existingJob.JobStatus = job.JobStatus;
            existingJob.Supervisor = job.Supervisor;
            existingJob.Price = job.Price;
            existingJob.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingJob;
        }

        /// <summary>
        /// Deletes a job by its ID.
        /// </summary>
        /// <param name="id">The ID of the job to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return false;
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes multiple jobs by their IDs.
        /// </summary>
        /// <param name="jobIds">The list of job IDs to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> BulkDeleteJobsAsync(List<int> jobIds)
        {
            var jobs = await _context.Jobs.Where(j => jobIds.Contains(j.JobId)).ToListAsync();

            if (jobs.Count == 0)
            {
                return false;
            }

            _context.Jobs.RemoveRange(jobs);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Represents the result of a job query, including the list of jobs and the total count.
        /// </summary>
        public class JobResult
        {
            /// <summary>
            /// Gets or sets the list of jobs.
            /// </summary>
            public List<Job> Jobs { get; set; }

            /// <summary>
            /// Gets or sets the total count of jobs.
            /// </summary>
            public int TotalCount { get; set; }
        }
    }
}