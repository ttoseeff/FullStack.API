using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Models
{
    public class FullStackDbContext : DbContext
    {
        public FullStackDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Employee> Employees { get; set; }
    }
}
