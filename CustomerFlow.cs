using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>
/// What a customer sees. There is no dashboard: a signed-in customer lands on step 2 of the Add Pet flow (Select a Pet),
/// the kiosk starts on step 1 (contact information + email code); both go on through the steps to the queue ticket.
/// </summary>
internal static class CustomerFlow
{
    public static void Run(UserAccount user)
    {
        (int Id, string Name)? customer;
        try { customer = EmployeeData.GetCustomerForLogin(user.Email); }
        catch (SqlException ex)
        {
            Msg.Show(null, "Couldn't load your data.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (customer is not { } c)
        {
            Msg.Show(null, $"Welcome, {user.Name}.\n\nThis login isn't linked to a customer record yet. Please ask the front desk to link it.",
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new AddPetForm(c.Id, c.Name, user.Email);   // signed in, so it opens on step 2 (Select a Pet); the email code was only for step 1
        if (form.ShowDialog() != DialogResult.OK) return;            // closed or backed out: back to the landing screen
        if (form.ChosenPetId is not int petId) return;

        GetQueueTicket(c.Id, petId, form.ChosenService, user.Email, form.PreferredDays);
    }

    /// <summary>Kiosk (Start on the landing screen): no login. Step 1 is the contact information + emailed code.</summary>
    public static void RunKiosk()
    {
        using var form = new AddPetForm(kiosk: true);
        if (form.ShowDialog() != DialogResult.OK) return;      // closed or backed out of step 1: back to the landing screen
        if (form.ChosenPetId is not int petId) return;

        GetQueueTicket(form.CustomerId, petId, form.ChosenService, form.ContactEmail, form.PreferredDays);
    }

    /// <summary>After the service step: the visit is queued right away (the customer only picked preferred days; staff set the real date and time) and the ticket is shown.</summary>
    private static void GetQueueTicket(int customerId, int petId, string? serviceName, string loginEmail, string preferredDays)
    {
        try
        {
            var service = ServicePricing.Find(serviceName, petId);   // the service priced for this pet's weight
            if (service is null)
            {
                Msg.Show(null, "That service isn't available right now. Please pick another one.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ticket = EmployeeData.AddOwnAppointment(customerId, petId, service.Id, DateTime.Now, loginEmail, preferredDays: preferredDays);   // saved as Pending, so staff see it too
            using var ticketForm = new QueueTicketForm(ticket);
            ticketForm.ShowDialog();
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            Msg.Show(null, "Couldn't create your queue ticket.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}