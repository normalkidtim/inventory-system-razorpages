using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using InventoryRazorApp.Models; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryRazorApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        // FIX: Initialized to string.Empty to remove CS8618 warning
        public string Username { get; set; } = string.Empty; 

        [TempData]
        // FIX: Initialized to string.Empty to remove CS8618 warning
        public string StatusMessage { get; set; } = string.Empty;

        [BindProperty]
        // FIX: Initialized to a new InputModel to remove CS8618 warning
        public InputModel Input { get; set; } = new InputModel(); 

        public class InputModel
        {
            // FIX: Made PhoneNumber nullable (string?) as it can be null/empty in Identity
            [Phone]
            [Display(Name = "Phone number")]
            public string? PhoneNumber { get; set; } 

            // Custom fields from ApplicationUser.cs
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; } = string.Empty; // Initialized

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; } = string.Empty; // Initialized

            [Required]
            [StringLength(20)]
            [Display(Name = "Contact Number")]
            public string ContactNumber { get; set; } = string.Empty; // Initialized
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            // The result of GetUserNameAsync and GetPhoneNumberAsync is string?, which is fine.
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            // FIX: Use the null-forgiving operator (!) or null check,
            // but GetUserNameAsync usually returns the UserName property, which is non-null.
            Username = userName ?? user.Email ?? string.Empty;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber, // string? to string? (OK)
                // Load custom fields. They are guaranteed to be non-null by ApplicationUser model.
                FirstName = user.FirstName,
                LastName = user.LastName,
                ContactNumber = user.ContactNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Update custom fields
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.ContactNumber = Input.ContactNumber;

            var updateCustomResult = await _userManager.UpdateAsync(user);
            if (!updateCustomResult.Succeeded)
            {
                StatusMessage = "Error: Failed to update profile details.";
                return RedirectToPage();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error: Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}