using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InventoryRazorApp.Models
{
    // Inherits IdentityUser to add custom properties
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [PersonalData]
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [PersonalData]
        [Required(ErrorMessage = "Contact Number is required.")]
        [StringLength(20)]
        public string ContactNumber { get; set; } = string.Empty;
    }
}