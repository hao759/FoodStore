using CuaHangDoAn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CuaHangDoAn.Data;

public class CuaHangDoAnContext : IdentityDbContext<AppUser>
{
    public CuaHangDoAnContext(DbContextOptions<CuaHangDoAnContext> options)
        : base(options)
    {
    }
    public DbSet<Product>? Products { get; set; }
    public DbSet<CategoryProduct> CategoryProducts { get; set; }
    public DbSet<Order>? Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<ProductsComments> ProductsComments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<OrderDetails>()
          .HasKey(m => new { m.OrderID, m.ProductID });
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    //public DbSet<CuaHangDoAn.Models.AppRole>? AppRole { get; set; }
}
