using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebsiteApi.Context;
using WebsiteApi.Models;

namespace WebsiteApi.Services
{
    public class JobService(DataContext dataContext)
    {
        private readonly DataContext _context = dataContext;

        public async Task<JobResult> GetAllRecords(
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

            //Apply filters
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

        public async Task<Job?> GetById(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task<Job> Add(Job job)
        {
            var result = await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Job> UpdateJob(int id, Job job)
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

        public async Task<bool> DeleteJob(int id)
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

        public async Task<bool> BulkDeleteJobs(List<int> jobIds)
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


        public class JobResult
        {
            public List<Job> Jobs { get; set; }
            public int TotalCount { get; set; }
        }
    }
}
