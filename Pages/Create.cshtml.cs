using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryRazorApp.Models;

namespace InventoryRazorApp.Pages
{
    // This is the C# class that handles the logic for the Create.cshtml page.
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        // Dependency Injection: EF Core Context and Environment (for file paths) 
        public CreateModel(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // This property will hold the data entered in the form.
        [BindProperty]
        public Item Item { get; set; } = new Item();

        // Called when the user first navigates to the page (GET request).
        public void OnGet()
        {
        }

        // Called when the user submits the form (POST request).
        public async Task<IActionResult> OnPostAsync()
        {
            // Checks if the form data (Name, Price, Category) meets the validation rules in Item.cs
            if (!ModelState.IsValid)
            {
                return Page(); // If invalid, return to the page with error messages.
            }

            // --- Photo Handling Logic ---
            if (Item.PhotoFile != null)
            {
                // 1. Define the folder where images are saved (wwwroot/images)
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imagesPath = Path.Combine(wwwRootPath, "images");

                // 2. Generate a unique file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Item.PhotoFile.FileName);
                string filePath = Path.Combine(imagesPath, fileName);

                // 3. Save the file to the server's disk
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Item.PhotoFile.CopyToAsync(fileStream);
                }

                // 4. Store the relative URL path in the database
                Item.PhotoPath = "/images/" + fileName;
            }

            // --- Database Insertion ---
            _context.Items.Add(Item);
            await _context.SaveChangesAsync();

            // Success message and redirect
            TempData["SuccessMessage"] = $"Item '{Item.Name}' added successfully!";
            return RedirectToPage("./Index"); // Redirect to the main index page (which will list items later)
        }
    }
}