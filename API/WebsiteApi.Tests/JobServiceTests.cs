using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using WebsiteApi.Models;
using WebsiteApi.Services;
using WebsiteApi.Context;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace WebsiteApi.Tests
{
    public class JobServiceTests
    {
        private readonly DataContext _context;
        private readonly JobService _jobService;

        public JobServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);
            _jobService = new JobService(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnJobWithExpenses()
        {
            var newJob = new Job
            {
                Name = "Test Job",
                JobStatus = 0, // Assuming 0 represents a specific status
                CarModel = "Test Car Model",
                Supervisor = "Alice Johnson",
                CreatedAt = DateTime.UtcNow,
                Expenses = new List<Expense>
                {
                    new Expense
                    {
                        Description = "Job Expense",
                        Amount = 150.00m,
                        Date = DateTime.UtcNow
                    }
                }
            };

            _context.Jobs.Add(newJob);
            _context.SaveChanges();

            // Act
            var newJobId = newJob.JobId;
            var result = await _jobService.GetByIdAsync(newJobId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Job", result.Name);
            Assert.Single(result.Expenses);
            var firstExpense = result.Expenses.First();
            Assert.Equal("Job Expense", firstExpense.Description);
        }


        [Fact]
        public async Task CreateJobAsync_ShouldCreateJobWithExpenses()
        {
            // New Job
            var newJob = new Job
            {
                Name = "Front left door replacement",
                JobStatus = 0,
                CarModel = "Ford Fiesta 2003",
                Supervisor = "Alice Johnson",
                CreatedAt = DateTime.UtcNow,
                Expenses = new List<Expense>
                {
                    new Expense
                    {
                        Description = "5 working hours",
                        Amount = 500.00m,
                        Date = DateTime.UtcNow
                    },
                    new Expense
                    {
                        Description = "New door panel",
                        Amount = 300.00m,
                        Date = DateTime.UtcNow
                    }
                }
            };

            // Create
            var createdJob = await _jobService.CreateAsync(newJob);

            // Test result data
            Assert.NotNull(createdJob);
            Assert.True(createdJob.JobId > 0, "New Job id should be greater than 0");
            Assert.Equal(createdJob.Name, newJob.Name);
            Assert.Equal(createdJob.Expenses.Count(), newJob.Expenses.Count());

            foreach (var expense in createdJob.Expenses)
            {
                Assert.True(expense.ExpenseId > 0, "Expense ID should be greater than zero.");
                Assert.Equal(createdJob.JobId, expense.JobId);
                Assert.False(string.IsNullOrWhiteSpace(expense.Description), "Expense description should not be empty.");
                Assert.True(expense.Amount > 0, "Expense amount should be greater than zero.");
            }

            // Check data in context
            var JobInDb = await _context
                .Jobs.Include(j => j.Expenses)
                .FirstOrDefaultAsync(j => j.JobId == createdJob.JobId);

            Assert.NotNull(JobInDb);
            Assert.Equal(newJob.Name, JobInDb.Name);
            Assert.Equal(newJob.Expenses.Count(), JobInDb.Expenses.Count());
        }


        [Fact]
        public async Task GetAllJobsAsync_ShouldReturnFilteredJobs()
        {
            // Add Test Data
            var newJob1 = new Job
            {
                Name = "Test Job 1",
                Description= "This is test job 1",
                JobStatus = (Status)1,
                CarModel = "Mazda MX5",
                Supervisor = "Alice Johnson",
                CreatedAt = DateTime.UtcNow,
            };
            var newJob2 = new Job
            {
                Name = "Test Job 2",
                Description = "This is test job 2",
                JobStatus = (Status)2,
                CarModel = "Porsche 911 GT3",
                Supervisor = "John Doe",
                CreatedAt = DateTime.UtcNow,
            };
            var newJob3 = new Job
            {
                Name = "Test Job 3",
                Description = "This is test job 3",
                JobStatus = (Status)3,
                CarModel = "Mazda MX5",
                Supervisor = "Adam Kowalski",
                CreatedAt = DateTime.UtcNow,
            };

            _context.Jobs.AddRange(new List<Job> { newJob1, newJob2, newJob3 });
            await _context.SaveChangesAsync();

            // Assert
            var result1 = await _jobService.GetAllJobsAsync(carModel: "Mazda MX5");
            Assert.Equal(2, result1.Jobs.Count());
            Assert.Equal("Mazda MX5", result1.Jobs[0].CarModel);
            Assert.Equal(2, result1.TotalCount);

            var result2 = await _jobService.GetAllJobsAsync(supervisor: "John Doe");
            Assert.Single(result2.Jobs);
            Assert.Equal("Test Job 2", result2.Jobs.First().Name);
            
            var result3 = await _jobService.GetAllJobsAsync(description: "1");
            Assert.Single(result3.Jobs);
            Assert.Equal("Test Job 1", result3.Jobs.First().Name);


        }
    }   
}