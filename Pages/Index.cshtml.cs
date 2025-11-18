// File: Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryRazorApp.Models;

namespace InventoryRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Item> ItemList { get;set; } = new List<Item>();
        public string? SuccessMessage { get; set; }

        public async Task OnGetAsync()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                SuccessMessage = TempData["SuccessMessage"] as string;
            }

            ItemList = await _context.Items.ToListAsync();
        }
    }
}