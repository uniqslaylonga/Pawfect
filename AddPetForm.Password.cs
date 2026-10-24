using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The password part of step 1 (Contact). A new customer types a password on the contact page itself; once the emailed code is
/// right, the account is created with it and they go straight to Select a Pet (there is no separate password page).
/// </summary>
internal sealed partial class AddPetForm
{
    private const int MinPasswordLength = 8;

    private bool _welcomeNewAccount;   // the next Select a Pet screen greets a customer whose account was just created

    /// <summary>Only customers without a sign-in yet are asked for a password (a returning one already has it).</summary>
    private bool NeedsPassword()
    {
        try { return !EmployeeData.CustomerHasLogin(_customerId); }
        catch (SqlException) { return false; }   // can't tell: don't block the visit
    }

    private bool CustomerNeedsPassword() => _customerId == 0 || NeedsPassword();

    /// <summary>What is wrong with the typed password, or null when it is fine.</summary>
    private static string? PasswordProblem(string password, string confirm)
    {
        if (password.Length < MinPasswordLength) return $"Password must be at least {MinPasswordLength} characters.";
        if (password.Length > 100) return "That password is too long (100 characters at most).";
        if (password != confirm) return "The passwords don't match.";
        return null;
    }

    /// <summary>Creates the sign-in for this customer. Returns an error message, or null when it worked.</summary>
    private async Task<string?> CreateAccountAsync()
    {
        // Hashing the password and saving the account takes a moment: show that something is happening instead of
        // freezing, and do the work off the UI thread so the screen keeps painting.
        int customerId = _customerId;
        string email = ContactEmail;
        string password = _pendingPassword ?? "";
        string? error = null;
        SetBusy(true);
        verifyButton.Enabled = false;
        verifyButton.Text = "Creating your account…";
        verifyButton.Update();
        try
        {
            bool taken = await Task.Run(() => UserRepository.GetByEmail(email) is not null);
            if (taken) error = "That email is already used by another account.";
            else await Task.Run(() => EmployeeData.CreateCustomerLogin(customerId, email, password));
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            error = "That email is already used by another account.";
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            error = "The database doesn't allow the Customer role yet. Run Sql/customer_account.sql once.";
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            error = "Couldn't create your account. " + ex.Message;
        }

        if (IsDisposed) return null;   // closed while the account was being created
        SetBusy(false);
        verifyButton.Text = "Verify";
        verifyButton.Enabled = true;
        if (error is null) _pendingPassword = null;
        return error;
    }
}
