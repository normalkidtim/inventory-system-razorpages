// File: Pages/Dashboard.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryRazorApp.Models;

namespace InventoryRazorApp.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;

        public DashboardModel(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, List<Item>> GroupedItems { get; set; } = new Dictionary<string, List<Item>>();

        public async Task OnGetAsync()
        {
            var allItems = await _context.Items.ToListAsync();

            GroupedItems = allItems
                .GroupBy(item => item.Category)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );
        }
    }
}