using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.data;

public class ApplicationDbContext: IdentityDbContext<AppUser>
{
    
    // primary constructor example: public class ApplicationDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    public ApplicationDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
    {
    }
    // tables
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Portfolio>(
            portfolio => portfolio.HasKey(p => new { p.AppUserId, p.StockId }));
        
        builder.Entity<Portfolio>()
            .HasOne(portfolio => portfolio.AppUser)
            .WithMany(user => user.Portfolios)
            .HasForeignKey(p => p.AppUserId);
        
        builder.Entity<Portfolio>()
            .HasOne(portfolio => portfolio.Stock)
            .WithMany(user => user.Portfolios)
            .HasForeignKey(p => p.StockId);
        
        
        List<IdentityRole> roles = new()
        {
            new IdentityRole {Name = "Admin", NormalizedName = "ADMIN"},
            new IdentityRole {Name = "User", NormalizedName = "USER"}
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}