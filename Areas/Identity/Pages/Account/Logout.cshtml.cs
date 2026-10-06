using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pawfect.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<IdentityUser> signInManager) : PageModel
{
    // The sidebar "Log Out" button navigates here with a GET.
    public Task<IActionResult> OnGetAsync() => SignOutAndRedirectAsync();

    // The old _LoginPartial posts here.
    public Task<IActionResult> OnPostAsync(string? returnUrl = null) => SignOutAndRedirectAsync();

    private async Task<IActionResult> SignOutAndRedirectAsync()
    {
        await signInManager.SignOutAsync();
        TempData["InfoMessage"] = "You have been logged out.";
        return RedirectToPage("/Account/Login", new { area = "Identity" });
    }
}
