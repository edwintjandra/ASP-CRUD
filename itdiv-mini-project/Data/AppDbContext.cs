
using Microsoft.EntityFrameworkCore;
using itdiv_mini_project.Models;

namespace itdiv_mini_project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<itdiv_mini_project.Models.Category> Category { get; set; }

      //  public DbSet<Employee> Employees { get; set; }
    }
}
