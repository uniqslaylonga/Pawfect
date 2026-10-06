using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pawfect.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string ReturnUrl { get; set; } = "/";

    public class InputModel
    {
        [Required(ErrorMessage = "Enter your email.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a password.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The password must be at least {2} characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public IActionResult OnGet(string? returnUrl = null)
    {
        ReturnUrl = SafeReturnUrl(returnUrl);
        if (User.Identity?.IsAuthenticated == true)
            return LocalRedirect(ReturnUrl);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = SafeReturnUrl(returnUrl);

        if (!ModelState.IsValid)
            return Page();

        var email = Input.Email.Trim();
        var user = new IdentityUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(ReturnUrl);
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return Page();
    }

    private string SafeReturnUrl(string? returnUrl) =>
        !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? returnUrl : "/";
}
