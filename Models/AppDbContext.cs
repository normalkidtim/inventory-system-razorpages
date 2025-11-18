using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace InventoryRazorApp.Models
{
    // Inherits from IdentityDbContext to include all user/role tables
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Represents the "Items" table for your inventory data.
        public DbSet<Item> Items { get; set; }
    }
}