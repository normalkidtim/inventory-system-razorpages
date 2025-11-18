// File: Pages/Delete.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryRazorApp.Models;

namespace InventoryRazorApp.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DeleteModel(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Item Item { get; set; } = new Item();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) { return NotFound(); }
            Item = await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (Item == null) { return NotFound(); }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) { return NotFound(); }

            Item = await _context.Items.FindAsync(id);

            if (Item != null)
            {
                // 1. Delete the image file from the server's disk
                if (!string.IsNullOrEmpty(Item.PhotoPath))
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string filePath = Path.Combine(wwwRootPath, Item.PhotoPath.TrimStart('/'));
                    
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                
                // 2. Delete the database record
                _context.Items.Remove(Item);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Item '{Item.Name}' deleted successfully!";
            }

            return RedirectToPage("./Index");
        }
    }
}