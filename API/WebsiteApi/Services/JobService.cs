using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebsiteApi.Context;
using WebsiteApi.Models;

namespace WebsiteApi.Services
{
    public class JobService(DataContext dataContext)
    {
        private readonly DataContext _context = dataContext;

        public async Task<List<Job>> GetAllJobs(
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

            if (pageNumber > 0 && pageSize > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<Job> AddJob(Job job)
        {
            var result = await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

    }
}
