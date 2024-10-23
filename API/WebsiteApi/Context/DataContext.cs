using Microsoft.EntityFrameworkCore;
using WebsiteApi.Controllers;
using WebsiteApi.Models;

namespace WebsiteApi.Context;

public class DataContext : DbContext 
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }

}