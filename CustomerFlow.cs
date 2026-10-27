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

        GetQueueTicket(c.Id, petId, form.ChosenService, user.Email, form.PreferredDays, walkIn: false);   // signed in: a requested appointment
    }

    /// <summary>Kiosk (Start on the landing screen): no login. Step 1 is the contact information + emailed code.</summary>
    public static void RunKiosk()
    {
        using var form = new AddPetForm(kiosk: true);
        if (form.ShowDialog() != DialogResult.OK) return;      // closed or backed out of step 1: back to the landing screen
        if (form.ChosenPetId is not int petId) return;

        GetQueueTicket(form.CustomerId, petId, form.ChosenService, form.ContactEmail, form.PreferredDays, walkIn: true);   // at the kiosk, no booking: a walk-in
    }

    /// <summary>After the service step: the visit is queued right away (the customer only picked preferred days; staff set the real date and time) and the ticket is shown.</summary>
    private static void GetQueueTicket(int customerId, int petId, string? serviceName, string loginEmail, string preferredDays, bool walkIn)
    {
        try
        {
            // Last check before the ticket: an expired or missing vaccine record never gets in, whichever step let the pet this far.
            if (PetProfiles.GetPictures(petId).Card is not { Length: > 0 } || PetVaccinations.AnyLapsed(PetVaccinations.GetForPet(petId)))
            {
                Msg.Show(null, "This pet's vaccine record is missing or has expired, so it can't be admitted.\n\nPlease scan a vaccine card with current dates.",
                    "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var service = ServicePricing.Find(serviceName, petId);   // the service priced for this pet's weight
            if (service is null)
            {
                Msg.Show(null, "That service isn't available right now. Please pick another one.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 14-day immunity: a pet whose vaccine was given less than 14 days ago is booked for the day it becomes protected (staff set the
            // real time). It stays in Triage Pending and can't be admitted before that day, so it is not a walk-in for today.
            DateTime? holdUntil = PetVaccinations.ImmunityClearDate(PetVaccinations.GetForPet(petId));
            if (holdUntil is { } held && held.Date <= DateTime.Today) holdUntil = null;
            DateTime start = holdUntil is { } h ? h.Date.AddHours(9) : DateTime.Now;
            string notes = holdUntil is { } n
                ? $"Held for the 14-day immunity period: can't be admitted before {n:MMM d, yyyy}. Re-check the physical booklet when the pet arrives."
                : "";

            var ticket = EmployeeData.AddOwnAppointment(customerId, petId, service.Id, start, loginEmail, notes: notes, preferredDays: preferredDays,
                walkIn: walkIn && holdUntil is null, holdUntil: holdUntil);   // saved as Pending (Triage Pending), so staff see it too
            using var ticketForm = new QueueTicketForm(ticket);
            ticketForm.ShowDialog();
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            Msg.Show(null, "Couldn't create your queue ticket.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}