using Microsoft.EntityFrameworkCore;
using WebsiteApi.Context;
using WebsiteApi.Models;

namespace WebsiteApi.Services
{
    public class JobService(DataContext dataContext)
    {
        private readonly DataContext _context = dataContext;

        public async Task<List<Job>> GetAllJobs(int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Jobs.AsQueryable();

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
