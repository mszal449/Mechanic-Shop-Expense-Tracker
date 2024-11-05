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
            var result = await _jobService.GetJobByIdAsync(newJobId);

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
            var createdJob = await _jobService.CreateJobAsync(newJob);

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

            // Save jobs
            _context.Jobs.AddRange(new List<Job> { newJob1, newJob2, newJob3 });
            await _context.SaveChangesAsync();

            // Assert
            var result1 = await _jobService.GetJobsAsync(carModel: "Mazda MX5");
            Assert.Equal(2, result1.Jobs.Count());
            Assert.Equal("Mazda MX5", result1.Jobs[0].CarModel);
            Assert.Equal(2, result1.TotalCount);

            var result2 = await _jobService.GetJobsAsync(supervisor: "John Doe");
            Assert.Single(result2.Jobs);
            Assert.Equal("Test Job 2", result2.Jobs.First().Name);
            
            var result3 = await _jobService.GetJobsAsync(description: "1");
            Assert.Single(result3.Jobs);
            Assert.Equal("Test Job 1", result3.Jobs.First().Name);
        }

        [Fact]
        public async Task UpdateJobAsync_ShouldReturnUpdatedJob()
        {
            // Arrange
            var newJob = new Job
            {
                Name = "Test Job",
                Description = "This is test job",
                JobStatus = (Status)1, // Assuming Status is an enum
                CarModel = "Mazda MX5",
                Supervisor = "Alice Johnson",
                CreatedAt = DateTime.UtcNow,
            };

            // Save job to database
            _context.Jobs.Add(newJob);
            await _context.SaveChangesAsync();
            var id = newJob.JobId;

            // Assert added job
            var initialJob = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == id);
            Assert.NotNull(initialJob);
            Assert.Equal(newJob.Name, initialJob.Name);
            Assert.Equal(newJob.Description, initialJob.Description);
            Assert.Equal(newJob.JobStatus, initialJob.JobStatus);
            Assert.Equal(newJob.CarModel, initialJob.CarModel);
            Assert.Equal(newJob.Supervisor, initialJob.Supervisor);

            // Prepare updated job data
            var updatedJobData = new Job
            {
                Name = "Updated Test Job",
                Description = "This is the updated test job",
                JobStatus = (Status)2, // Assuming Status enum has a value 2
                CarModel = "Toyota Corolla",
                Supervisor = "Bob Smith",
                Price = 25000.00m, // Assuming Price is a property to be updated
            };

            // Act
            var updatedJob = await _jobService.UpdateJobAsync(id, updatedJobData);

            // Assert
            Assert.NotNull(updatedJob);
            Assert.Equal(id, updatedJob.JobId);
            Assert.Equal("Updated Test Job", updatedJob.Name);
            Assert.Equal("This is the updated test job", updatedJob.Description);
            Assert.Equal((Status)2, updatedJob.JobStatus);
            Assert.Equal("Toyota Corolla", updatedJob.CarModel);
            Assert.Equal("Bob Smith", updatedJob.Supervisor);
            Assert.Equal(25000.00m, updatedJob.Price);
            Assert.NotNull(updatedJob.UpdatedAt);
            Assert.True(updatedJob.UpdatedAt > initialJob.CreatedAt, "UpdatedAt should be later than CreatedAt");

            // Verify changes in the database
            var jobInDb = await _context.Jobs.FindAsync(id);
            Assert.NotNull(jobInDb);
            Assert.Equal("Updated Test Job", jobInDb.Name);
            Assert.Equal("This is the updated test job", jobInDb.Description);
            Assert.Equal((Status)2, jobInDb.JobStatus);
            Assert.Equal("Toyota Corolla", jobInDb.CarModel);
            Assert.Equal("Bob Smith", jobInDb.Supervisor);
            Assert.Equal(25000.00m, jobInDb.Price);
            Assert.NotNull(jobInDb.UpdatedAt);
            Assert.True(jobInDb.UpdatedAt > jobInDb.CreatedAt, "UpdatedAt should be later than CreatedAt");
        }

        [Fact]
        public async Task DeleteJobAsync_ShouldReturnTrue_WhenJobExists()
        {
            // Arrange
            var job = new Job
            {
                Name = "Delete Test Job",
                Description = "Job to be deleted",
                JobStatus = (Status)1,
                CarModel = "Honda Civic",
                Supervisor = "Charlie Brown",
                CreatedAt = DateTime.UtcNow,
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            var jobId = job.JobId;

            // Act
            var result = await _jobService.DeleteJobAsync(jobId);

            // Assert
            Assert.True(result, "DeleteJobAsync should return true for existing job.");

            // Verify that the job is removed from the database
            var deletedJob = await _context.Jobs.FindAsync(jobId);
            Assert.Null(deletedJob);
        }

        [Fact]
        public async Task DeleteJobAsync_ShouldReturnFalse_WhenJobDoesNotExist()
        {
            // Arrange
            var nonExistentJobId = 999;

            // Act
            var result = await _jobService.DeleteJobAsync(nonExistentJobId);

            // Assert
            Assert.False(result, "DeleteJobAsync should return false for non-existent job.");
        }

        [Fact]
        public async Task BulkDeleteJobsAsync_ShouldReturnTrue_WhenJobsExist()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job
                {
                    Name = "Bulk Delete Job 1",
                    Description = "First job to delete",
                    JobStatus = (Status)1,
                    CarModel = "Ford Focus",
                    Supervisor = "Daisy Ridley",
                    CreatedAt = DateTime.UtcNow,
                },
                new Job
                {
                    Name = "Bulk Delete Job 2",
                    Description = "Second job to delete",
                    JobStatus = (Status)2,
                    CarModel = "Chevrolet Malibu",
                    Supervisor = "Evan Peters",
                    CreatedAt = DateTime.UtcNow,
                }
            };

            _context.Jobs.AddRange(jobs);
            await _context.SaveChangesAsync();

            var jobIdsToDelete = jobs.Select(j => j.JobId).ToList();

            // Act
            var result = await _jobService.BulkDeleteJobsAsync(jobIdsToDelete);

            // Assert
            Assert.True(result, "BulkDeleteJobsAsync should return true when jobs exist.");

            // Verify that the jobs are removed from the database
            foreach (var id in jobIdsToDelete)
            {
                var deletedJob = await _context.Jobs.FindAsync(id);
                Assert.Null(deletedJob);
            }
        }

        [Fact]
        public async Task BulkDeleteJobsAsync_ShouldReturnFalse_WhenNoJobsExist()
        {
            // Arrange
            var jobIdsToDelete = new List<int> { 1001, 1002, 1003 };

            // Act
            var result = await _jobService.BulkDeleteJobsAsync(jobIdsToDelete);

            // Assert
            Assert.False(result, "BulkDeleteJobsAsync should return false when no jobs exist.");
        }

        [Fact]
        public async Task BulkDeleteJobsAsync_ShouldDeleteExistingJobs_IgnoreNonExistent()
        {
            // Arrange
            var existingJobs = new List<Job>
            {
                new Job
                {
                    Name = "Bulk Partial Delete Job 1",
                    Description = "First job to delete partially",
                    JobStatus = (Status)1,
                    CarModel = "Nissan Sentra",
                    Supervisor = "Fiona Gallagher",
                    CreatedAt = DateTime.UtcNow,
                },
                new Job
                {
                    Name = "Bulk Partial Delete Job 2",
                    Description = "Second job to delete partially",
                    JobStatus = (Status)2,
                    CarModel = "Hyundai Elantra",
                    Supervisor = "George Martin",
                    CreatedAt = DateTime.UtcNow,
                }
            };

            _context.Jobs.AddRange(existingJobs);
            await _context.SaveChangesAsync();

            var existingJobIds = existingJobs.Select(j => j.JobId).ToList();
            var nonExistentJobId = 2001;
            var jobIdsToDelete = new List<int> { existingJobIds[0], nonExistentJobId };

            // Act
            var result = await _jobService.BulkDeleteJobsAsync(jobIdsToDelete);

            // Assert
            Assert.True(result, "BulkDeleteJobsAsync should return true when some jobs exist.");

            // Verify that the existing job is deleted
            var deletedJob = await _context.Jobs.FindAsync(existingJobIds[0]);
            Assert.Null(deletedJob);

            // Verify that the non-existent job was ignored and no exception thrown
            var remainingJob = await _context.Jobs.FindAsync(existingJobIds[1]);
            Assert.NotNull(remainingJob);
        }
    }   
}