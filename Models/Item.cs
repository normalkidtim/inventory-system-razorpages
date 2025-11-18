using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; 

namespace InventoryRazorApp.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the item name.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the item price.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter the item category.")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        public string? PhotoPath { get; set; }
        
        [NotMapped]
        public IFormFile? PhotoFile { get; set; }
    }
}