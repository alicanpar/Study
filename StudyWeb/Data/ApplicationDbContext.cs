using Microsoft.EntityFrameworkCore;
using StudyWeb.Models;

namespace StudyWeb.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ProductCategory> productCategories { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Company> companies { get; set; }
    }
}
