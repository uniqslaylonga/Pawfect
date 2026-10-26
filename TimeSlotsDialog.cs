using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>Service Catalog Management > Set Time Slots: opening time, closing time and how long each bookable slot is.</summary>
internal sealed class TimeSlotsDialog : DialogBase
{
    private readonly PawSelect opensBox = new();
    private readonly PawSelect closesBox = new();
    private readonly PawSelect stepBox = new();
    private readonly Label summary = new()
    {
        AutoSize = false,
        Height = 36,
        TextAlign = ContentAlignment.MiddleLeft,
        ForeColor = Theme.Muted,
        Font = Theme.Face(8.5f),
    };

    private readonly List<TimeSpan> _times = new();

    /// <summary>Short description of the saved hours, so AdminForm can write it to the audit log.</summary>
    public string? SavedSummary { get; private set; }

    public TimeSlotsDialog()
        : base("Set Time Slots", "Save", "\uE787", "Choose when appointments can start")
    {
        ServiceHours.EnsureLoaded();

        // Every half hour from 5:00 AM to 11:00 PM.
        for (var t = TimeSpan.FromHours(5); t <= TimeSpan.FromHours(23); t += TimeSpan.FromMinutes(30))
            _times.Add(t);

        var labels = _times.Select(t => (object)Format(t)).ToArray();
        opensBox.Items.AddRange(labels);
        closesBox.Items.AddRange(labels);
        stepBox.Items.AddRange(ServiceHours.StepChoices.Select(m => (object)StepText(m)).ToArray());

        opensBox.SelectedIndex = NearestIndex(ServiceHours.Opens);
        closesBox.SelectedIndex = NearestIndex(ServiceHours.Closes);
        stepBox.SelectedIndex = Math.Max(0, Array.IndexOf(ServiceHours.StepChoices, ServiceHours.StepMinutes));

        opensBox.SelectedIndexChanged += (_, _) => UpdateSummary();
        closesBox.SelectedIndexChanged += (_, _) => UpdateSummary();
        stepBox.SelectedIndexChanged += (_, _) => UpdateSummary();

        AddRow("Opens at", opensBox);
        AddRow("Last start at", closesBox);
        AddRow("Slot length", stepBox);
        AddRow("", summary);
        UpdateSummary();
        FitHeight();
    }

    private static string Format(TimeSpan t) => (DateTime.Today + t).ToString("h:mm tt");

    private static string StepText(int minutes) => minutes == 60 ? "1 hour" : minutes + " minutes";

    private int NearestIndex(TimeSpan value)
    {
        int best = 0;
        for (int i = 1; i < _times.Count; i++)
            if (Math.Abs((_times[i] - value).Ticks) < Math.Abs((_times[best] - value).Ticks)) best = i;
        return best;
    }

    private bool TryRead(out TimeSpan opens, out TimeSpan closes, out int step)
    {
        opens = closes = TimeSpan.Zero;
        step = 0;
        if (opensBox.SelectedIndex < 0 || closesBox.SelectedIndex < 0 || stepBox.SelectedIndex < 0) return false;
        opens = _times[opensBox.SelectedIndex];
        closes = _times[closesBox.SelectedIndex];
        step = ServiceHours.StepChoices[stepBox.SelectedIndex];
        return true;
    }

    private void UpdateSummary()
    {
        ShowError("");
        if (!TryRead(out var opens, out var closes, out int step)) return;
        if (closes <= opens)
        {
            summary.Text = "";
            return;
        }
        int count = (int)((closes - opens).TotalMinutes / step) + 1;
        summary.Text = $"{count} start times a day, from {Format(opens)} to {Format(closes)}.";
    }

    protected override void OnSaveClicked()
    {
        if (!TryRead(out var opens, out var closes, out int step))
        {
            ShowError("Choose the opening time, last start time and slot length.");
            return;
        }
        if (closes <= opens)
        {
            ShowError("The last start time must be later than the opening time.");
            return;
        }

        try
        {
            ServiceHours.Save(opens, closes, step);
            SavedSummary = $"{Format(opens)} - {Format(closes)}, {StepText(step)} slots";
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }
}
