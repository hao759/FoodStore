using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Security.Principal;

namespace CuaHangDoAn.Models
{
    //public class Db : DbContext
    //{
    //    public Db(DbContextOptions opt) : base(opt)
    //    {

    //    }
    //    public DbSet<Product>? Products { get; set; }
    //    public DbSet<CategoryProduct> CategoryProducts { get; set; }
    //}
    public class Db : IdentityDbContext<AppUser>
    {
        public Db(DbContextOptions opt) : base(opt)
        {

        }
        public DbSet<Product>? Products { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
    }
}
