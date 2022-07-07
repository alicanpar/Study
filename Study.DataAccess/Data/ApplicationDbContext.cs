using Microsoft.EntityFrameworkCore;
using Study.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.DataAccess;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Company> Companies { get; set; }
    public DbSet<ProductCategory> ProductCategories  { get; set; }
    public DbSet<Product> Products  { get; set; }
}

