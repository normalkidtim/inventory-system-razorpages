// File: Models/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace InventoryRazorApp.Models
{
    // UPDATED: Inherits from IdentityDbContext and uses the custom ApplicationUser model
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Represents the "Items" table for your inventory data.
        public DbSet<Item> Items { get; set; }
    }
}