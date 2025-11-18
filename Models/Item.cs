using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryRazorApp.Models
{
    // This class defines the structure of your inventory item.
    public class Item
    {
        // This is the Primary Key for the database table (unique ID).
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the item name.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the item price.")]
        [Column(TypeName = "decimal(18, 2)")] // Stores price with 2 decimal places
        [Range(0.01, 10000.00, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter the item category.")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        // This will store the relative path to the saved image file.
        public string? PhotoPath { get; set; }
        
        // This is not stored in the database but used to hold the uploaded file temporarily.
        [NotMapped]
        public IFormFile? PhotoFile { get; set; }
    }
}