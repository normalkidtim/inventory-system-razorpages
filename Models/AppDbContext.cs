using Microsoft.EntityFrameworkCore;

namespace InventoryRazorApp.Models
{
    // The AppDbContext handles connections and transactions with the database.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Represents the "Items" table in the database.
        public DbSet<Item> Items { get; set; }
    }
}