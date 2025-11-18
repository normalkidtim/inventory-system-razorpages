// File: Pages/Edit.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.EntityFrameworkCore;
using InventoryRazorApp.Models;
using System.IO; 

namespace InventoryRazorApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditModel(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Item Item { get; set; } = default!; 

        // Handles loading the item for display
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            
            if (item == null)
            {
                return NotFound();
            }
            Item = item;
            return Page();
        }

        // Handles saving the changes
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // This is where the misleading error originates if any field (like Price) fails validation
                return Page();
            }

            // --- PHOTO HANDLING LOGIC ---
            if (Item.PhotoFile != null)
            {
                // 1. Delete old photo if it exists
                if (!string.IsNullOrEmpty(Item.PhotoPath))
                {
                    string oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, Item.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // 2. Save new photo (same logic as Create)
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imagesPath = Path.Combine(wwwRootPath, "images");
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Item.PhotoFile.FileName);
                string filePath = Path.Combine(imagesPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Item.PhotoFile.CopyToAsync(fileStream);
                }

                Item.PhotoPath = "/images/" + fileName;
            }
            // --- END PHOTO HANDLING ---
            
            // This tells EF Core the entity has been modified.
            _context.Attach(Item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Items.Any(e => e.Id == Item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = $"Item '{Item.Name}' updated successfully!";
            return RedirectToPage("./Dashboard");
        }
    }
}