using Microsoft.EntityFrameworkCore;
using WebsiteApi.Controllers;
using WebsiteApi.Models;

namespace WebsiteApi.Context;

public class DataContext : DbContext 
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Job> Jobs { get; set; }  
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the one-to-many relationship between Job and Expense
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Job)
            .WithMany(j => j.Expenses)
            .HasForeignKey(e => e.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}