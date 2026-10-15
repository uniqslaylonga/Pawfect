using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>
/// What a customer sees. There is no customer dashboard: the Add Pet dialog IS the customer experience.
///  - "I'm New Here"      -> opens on step 1 (Contact + emailed code).
///  - Existing, logged in -> opens on step 2 (Select a Pet).
/// When the service step is finished the visit is queued and the ticket is shown, then the app returns to the landing screen.
/// </summary>
internal static class CustomerFlow
{
    /// <summary>New customer: no account yet, start at step 1.</summary>
    public static void RunNewCustomer()
    {
        using var form = AddPetForm.ForNewCustomer();
        Finish(form);
    }

    /// <summary>Existing customer who just logged in: skip step 1, start at step 2.</summary>
    public static void RunExistingCustomer(UserAccount user)
    {
        (int Id, string Name)? customer;
        try { customer = EmployeeData.GetCustomerForLogin(user.Email); }
        catch (SqlException ex)
        {
            Msg.Show(null, "Couldn't load your account.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (customer is not { } c)
        {
            Msg.Show(null, "This login isn't linked to a customer record yet. Please ask the front desk to link it.",
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new AddPetForm(c.Id, c.Name, user.Email);
        Finish(form);
    }

    private static void Finish(AddPetForm form)
    {
        if (form.ShowDialog() != DialogResult.OK) return;   // closed / backed out: nothing more to do
        if (form.ChosenPetId is int petId)
            GetQueueTicket(form.CustomerId, form.ContactEmail, petId, form.ChosenService);   // service chosen: straight to the queue ticket
    }

    /// <summary>After the service step: the visit is queued right away (no date or time to pick) and the ticket is shown.</summary>
    private static void GetQueueTicket(int customerId, string email, int petId, string? serviceName)
    {
        try
        {
            var service = EmployeeData.GetServices().FirstOrDefault(x => string.Equals(x.Name, serviceName, StringComparison.OrdinalIgnoreCase));
            if (service is null)
            {
                Msg.Show(null, "That service isn't available right now. Please ask the front desk for help.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ticket = EmployeeData.AddOwnAppointment(customerId, petId, service.Id, DateTime.Now, email);   // saved as Pending, so staff see it too
            using var ticketForm = new QueueTicketForm(ticket);
            ticketForm.ShowDialog();
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            Msg.Show(null, "Couldn't create your queue ticket.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
