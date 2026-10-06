using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pawfect.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string ReturnUrl { get; set; } = "/";

    [TempData]
    public string? InfoMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Enter your email or username.")]
        [Display(Name = "Email or username")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = SafeReturnUrl(returnUrl);

        // Already logged in? Go straight where they were headed.
        if (User.Identity?.IsAuthenticated == true)
            return LocalRedirect(User.IsInRole("Admin") ? AdminAwareUrl(ReturnUrl, true) : ReturnUrl);

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = SafeReturnUrl(returnUrl);

        if (!ModelState.IsValid)
            return Page();

        var login = Input.Login.Trim();

        // Employees are created with a username, self-registered users with their email - accept both.
        var user = await userManager.FindByNameAsync(login)
                   ?? await userManager.FindByEmailAsync(login);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login or password.");
            return Page();
        }

        var result = await signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Admins land on the admin dashboard instead of the customer home page.
            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
            return LocalRedirect(AdminAwareUrl(ReturnUrl, isAdmin));
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "This account is deactivated or locked. Please contact an administrator.");
            return Page();
        }

        ModelState.AddModelError(string.Empty, "Invalid login or password.");
        return Page();
    }

    private static string AdminAwareUrl(string url, bool isAdmin) =>
        isAdmin && (url == "/" || url.Equals("/pets", StringComparison.OrdinalIgnoreCase)) ? "/admin" : url;

    private string SafeReturnUrl(string? returnUrl) =>
        !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? returnUrl : "/";
}
