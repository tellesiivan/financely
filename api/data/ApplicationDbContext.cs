using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.data;

public class ApplicationDbContext: DbContext
{
    
    // primary constructor example: public class ApplicationDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    public ApplicationDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
    {
    }
    // tables
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
}