// File: Pages/Create.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryRazorApp.Models;

namespace InventoryRazorApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateModel(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Item Item { get; set; } = new Item();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Item.PhotoFile != null)
            {
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

            _context.Items.Add(Item);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Item '{Item.Name}' added successfully!";
            return RedirectToPage("./Index"); 
        }
    }
}