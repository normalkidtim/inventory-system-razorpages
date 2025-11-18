// File: Pages/Edit.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryRazorApp.Models;

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
        public Item Item { get; set; } = new Item();

        [BindProperty]
        public string? OldPhotoPath { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) { return NotFound(); }
            Item = await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (Item == null) { return NotFound(); }
            
            OldPhotoPath = Item.PhotoPath;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Item.PhotoPath = OldPhotoPath; 
                return Page();
            }

            if (Item.PhotoFile != null)
            {
                // New File Upload
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imagesPath = Path.Combine(wwwRootPath, "images");
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Item.PhotoFile.FileName);
                string filePath = Path.Combine(imagesPath, fileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Item.PhotoFile.CopyToAsync(fileStream);
                }

                Item.PhotoPath = "/images/" + fileName;

                // Delete the old file
                if (!string.IsNullOrEmpty(OldPhotoPath))
                {
                    string oldFilePath = Path.Combine(wwwRootPath, OldPhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
            else
            {
                Item.PhotoPath = OldPhotoPath;
            }

            try
            {
                _context.Items.Update(Item);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Items.Any(e => e.Id == Item.Id)) { return NotFound(); }
                throw;
            }

            TempData["SuccessMessage"] = $"Item '{Item.Name}' updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}