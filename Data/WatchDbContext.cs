using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WatchApp.Models;

namespace WatchApp.Data
{
    public class WatchDbContext : IdentityDbContext<IdentityUser>
    {
        public WatchDbContext(DbContextOptions<WatchDbContext> options): base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<Watch> Watches { get; set; }
       public DbSet<User> Users { get; set; }
    }
}
