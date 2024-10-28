using Microsoft.EntityFrameworkCore;
using WebsiteApi.Context;
using WebsiteApi.Models;

namespace WebsiteApi.Services
{
    public class JobService(DataContext dataContext)
    {
        private readonly DataContext _context = dataContext;

        public async Task<List<Job>> GetAllJobs()
        {
            return await _context.Jobs.ToListAsync();
        }
    }
}
