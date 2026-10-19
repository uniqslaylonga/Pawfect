using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class ScheduleDialog : ThemedDialog
{
    private readonly EmployeeRepository.EmployeeRow _emp;
    private readonly UserAccount? _actor;
    private readonly CheckBox[] _on = new CheckBox[7];
    private readonly DateTimePicker[] _from = new DateTimePicker[7];
    private readonly DateTimePicker[] _to = new DateTimePicker[7];
    private readonly DateTimePicker _absDate = new() { Format = DateTimePickerFormat.Short };
    private readonly TextBox _reason = new() { PlaceholderText = "Reason (optional)" };
    private readonly ListBox _list = new() { IntegralHeight = false };
    private readonly Label _count = new() { AutoSize = true };
    private List<ScheduleRepository.AbsenceRow> _absences = new();

    public ScheduleDialog(EmployeeRepository.EmployeeRow emp, UserAccount? actor)
        : base("Schedule & Absences", $"{emp.Name}: weekly shifts and recorded absences.", 440)
    {
        _emp = emp;
        _actor = actor;

        var tabs = new TabControl { Font = Theme.Face(9f) };
        var schedTab = new TabPage("Weekly schedule") { BackColor = Color.White };
        var absTab = new TabPage("Absences") { BackColor = Color.White };
        tabs.TabPages.Add(schedTab);
        tabs.TabPages.Add(absTab);

        // ---- weekly schedule
        List<ScheduleRepository.ShiftRow> shifts;
        try { shifts = ScheduleRepository.GetShifts(emp.Id); }
        catch (SqlException ex)
        {
            MessageBox.Show("Could not load the schedule: " + ex.Message);
            shifts = new();
        }
        bool fresh = shifts.Count == 0;   // no schedule yet: start with Monday to Friday, 9 to 5

        schedTab.Controls.Add(new Label { Text = "Start", Left = 130, Top = 10, AutoSize = true, ForeColor = Theme.Muted });
        schedTab.Controls.Add(new Label { Text = "End", Left = 258, Top = 10, AutoSize = true, ForeColor = Theme.Muted });

        for (int i = 0; i < 7; i++)
        {
            int day = i + 1;
            var shift = shifts.FirstOrDefault(s => s.Day == day);
            bool on = shift != null || (fresh && i < 5);
            int y = 36 + i * 34;

            var cb = new CheckBox { Text = ScheduleRepository.Days[i], Checked = on, Left = 12, Top = y + 3, Width = 110 };
            var from = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "h:mm tt",
                ShowUpDown = true,
                Left = 130,
                Top = y,
                Width = 118,
                Value = DateTime.Today + (shift?.Start ?? new TimeSpan(9, 0, 0)),
            };
            var to = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "h:mm tt",
                ShowUpDown = true,
                Left = 258,
                Top = y,
                Width = 118,
                Value = DateTime.Today + (shift?.End ?? new TimeSpan(17, 0, 0)),
            };
            from.Enabled = to.Enabled = on;
            cb.CheckedChanged += (_, _) => from.Enabled = to.Enabled = cb.Checked;

            _on[i] = cb;
            _from[i] = from;
            _to[i] = to;
            schedTab.Controls.Add(cb);
            schedTab.Controls.Add(from);
            schedTab.Controls.Add(to);
        }

        // ---- absences (these save immediately, not when you press Save)
        _absDate.SetBounds(12, 12, 110, 26);
        _reason.SetBounds(130, 12, 160, 26);
        var add = new Button { Text = "Add", Left = 298, Top = 11, Width = 76, Height = 28 };
        add.Click += (_, _) => AddAbsence();
        _list.SetBounds(12, 50, 362, 190);
        var remove = new Button { Text = "Remove selected", Left = 12, Top = 248, Width = 130, Height = 28 };
        remove.Click += (_, _) => RemoveAbsence();
        _count.Location = new Point(150, 254);
        foreach (Control c in new Control[] { _absDate, _reason, add, _list, remove, _count })
            absTab.Controls.Add(c);

        AddCustom(null, tabs, 322);
        AddButtons("Save Schedule", Save_Click);
        LoadAbsences();
    }

    private void LoadAbsences()
    {
        try { _absences = ScheduleRepository.GetAbsences(_emp.Id); }
        catch (SqlException ex)
        {
            MessageBox.Show("Could not load absences: " + ex.Message);
            return;
        }

        _list.Items.Clear();
        foreach (var a in _absences)
            _list.Items.Add(a.Date.ToString("MMM d, yyyy") +
                            (string.IsNullOrWhiteSpace(a.Reason) ? "" : "  -  " + a.Reason));

        var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        _count.Text = $"This month: {_absences.Count(a => a.Date >= monthStart)}    Total: {_absences.Count}";
    }

    private void AddAbsence()
    {
        try
        {
            ScheduleRepository.AddAbsence(_emp.Id, _absDate.Value.Date, _reason.Text, _actor?.Name ?? "System");
            AuditLog.Record(_actor, "Absence Recorded", $"{_emp.Name} absent on {_absDate.Value:MMM d, yyyy}");
            _reason.Clear();
            LoadAbsences();
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show(this, "That day is already recorded as an absence.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not save: " + ex.Message);
        }
    }

    private void RemoveAbsence()
    {
        if (_list.SelectedIndex < 0) return;
        var a = _absences[_list.SelectedIndex];
        try
        {
            ScheduleRepository.RemoveAbsence(a.Id);
            AuditLog.Record(_actor, "Absence Removed", $"{_emp.Name}, {a.Date:MMM d, yyyy}");
            LoadAbsences();
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not remove: " + ex.Message);
        }
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        var shifts = new List<ScheduleRepository.ShiftRow>();
        for (int i = 0; i < 7; i++)
        {
            if (!_on[i].Checked) continue;
            var start = _from[i].Value.TimeOfDay;
            var end = _to[i].Value.TimeOfDay;
            if (end <= start)
            {
                MessageBox.Show(this, $"{ScheduleRepository.Days[i]}: the end time must be after the start time.");
                return;
            }
            shifts.Add(new(i + 1, start, end));
        }

        try
        {
            ScheduleRepository.SaveShifts(_emp.Id, shifts);
            AuditLog.Record(_actor, "Schedule Updated", $"{_emp.Name}: {shifts.Count} working day(s) per week");
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not save: " + ex.Message);
        }
    }

    // Enter in the reason box adds the absence instead of saving the schedule.
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Enter && _reason.Focused) { AddAbsence(); return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}