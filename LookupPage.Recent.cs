using System.Drawing.Drawing2D;
using System.Globalization;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The two cards on the right of Customer &amp; Pet Lookup: Quick Filters (All / New customers, with the selected one highlighted
/// and "View All" clearing every filter) and Recent Lookups (the customers whose pets or "..." menu were opened, newest first).
/// </summary>
internal partial class LookupPage
{
    private const int RecentMax = 20;          // how many are remembered
    private const int RecentCollapsed = 5;     // how many show until "View All" is pressed

    // Kept while the app is open: newest first, one entry per customer.
    private readonly List<(int CustomerId, DateTime At)> _recent = new();
    private bool _recentAll;

    // ------------------------------------------------------------------ quick filters

    /// <summary>A Quick Filters row was clicked: switch to the customer list with that filter.</summary>
    private void ChooseQuickFilter(bool onlyNew)
    {
        _onlyNew = onlyNew;
        UpdateQuickFilterLook();
        if (_showPets) SelectTab(customerTab);      // the quick filters are about customers; this also applies the filter
        else ApplyFilters(resetPage: true);
    }

    /// <summary>"View All" on Quick Filters (or opening a recent lookup): back to the customer list with nothing filtered.</summary>
    private void ClearFilters(string search = "")
    {
        _suspendFilter = true;
        searchField.Box.Text = search;
        customerFilter.SelectedIndex = 0;
        petFilter.SelectedIndex = 0;
        sortFilter.SelectedIndex = 0;
        _suspendFilter = false;

        _onlyNew = false;
        UpdateQuickFilterLook();
        if (_showPets) SelectTab(customerTab);
        else ApplyFilters(resetPage: true);
    }

    /// <summary>Highlights the row that is currently applied (the same blue as the "All Customers" row starts with).</summary>
    private void UpdateQuickFilterLook()
    {
        StyleFilterRow(filterAllRow, filterAllName, filterAllCount, selected: !_onlyNew);
        StyleFilterRow(filterNewRow, filterNewName, filterNewCount, selected: _onlyNew);
    }

    private static void StyleFilterRow(Panel row, Label name, Label count, bool selected)
    {
        var back = selected ? Theme.PrimarySoft : Color.White;
        row.BackColor = back;
        name.BackColor = back;
        count.BackColor = back;
        name.Font = Theme.Face(9f, selected ? FontStyle.Bold : FontStyle.Regular);
        foreach (Control child in row.Controls)
            if (child is IconBadge) child.BackColor = back;
    }

    // ------------------------------------------------------------------ recent lookups

    /// <summary>A customer was "looked up" (View Pets or the "..." menu on their row, or on one of their pets).</summary>
    private void RecordLookup(int customerId)
    {
        _recent.RemoveAll(r => r.CustomerId == customerId);
        _recent.Insert(0, (customerId, DateTime.Now));
        if (_recent.Count > RecentMax) _recent.RemoveRange(RecentMax, _recent.Count - RecentMax);
        RefreshRecent();
    }

    private void ToggleRecentAll()
    {
        _recentAll = !_recentAll;
        RefreshRecent();
    }

    /// <summary>Rebuilds the Recent Lookups list (also called after the customers are reloaded, so renamed customers show their new name).</summary>
    private void RefreshRecent()
    {
        var byId = _customers.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.First());
        var items = _recent.Where(r => byId.ContainsKey(r.CustomerId)).ToList();
        int shown = _recentAll ? items.Count : Math.Min(items.Count, RecentCollapsed);

        recentList.SuspendLayout();
        foreach (var old in recentList.Controls.OfType<RecentLookupRowView>().ToList())
        {
            recentList.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the newest one on top.
        for (int i = shown - 1; i >= 0; i--)
        {
            var c = byId[items[i].CustomerId];
            string contact = !string.IsNullOrWhiteSpace(c.Phone) ? c.Phone : c.Email;
            string pets = c.PetCount == 1 ? "1 pet" : $"{c.PetCount} pets";
            var row = new RecentLookupRowView(c.Name, string.IsNullOrWhiteSpace(contact) ? pets : $"{contact}  ·  {pets}", Ago(items[i].At))
            {
                Dock = DockStyle.Top,
            };
            string name = c.Name;
            row.Click += (_, _) => ClearFilters(name);      // show just that customer in the main list
            recentList.Controls.Add(row);
        }
        recentList.ResumeLayout(true);

        recentList.Visible = items.Count > 0;
        recentEmpty.Visible = items.Count == 0;
        recentLink.Visible = items.Count > RecentCollapsed;
        recentLink.Text = _recentAll ? "Show less" : "View All →";
    }

    private static string Ago(DateTime at)
    {
        var d = DateTime.Now - at;
        if (d.TotalMinutes < 1) return "Just now";
        if (d.TotalMinutes < 60) return $"{(int)d.TotalMinutes} min ago";
        if (at.Date == DateTime.Today) return at.ToString("h:mm tt", CultureInfo.CurrentCulture);
        return at.ToString("MMM d", CultureInfo.CurrentCulture);
    }
}

/// <summary>One line in Recent Lookups: round initials, the customer's name, phone and pets, and when they were looked up.</summary>
internal sealed class RecentLookupRowView : Control
{
    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
        TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private readonly string _name;
    private readonly string _sub;
    private readonly string _when;
    private bool _hover;

    public RecentLookupRowView(string name, string sub, string when)
    {
        _name = name;
        _sub = sub;
        _when = when;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Height = 52;
        TabStop = false;
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    private static string Initials(string name)
    {
        var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0) return "?";
        if (words.Length == 1) return words[0][..1].ToUpperInvariant();
        return (words[0][..1] + words[^1][..1]).ToUpperInvariant();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var back = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(back, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, 8, Height - 1, Width - 8, Height - 1);

        var avatar = new Rectangle(8, (Height - 34) / 2, 34, 34);
        using (var fill = new SolidBrush(Theme.PrimarySoft))
            g.FillEllipse(fill, avatar);
        TextRenderer.DrawText(g, Initials(_name), Theme.Face(9f, FontStyle.Bold), avatar, Theme.Primary,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        int whenW = 64;
        int tx = avatar.Right + 10;
        TextRenderer.DrawText(g, _when, Theme.Face(8f), new Rectangle(Width - whenW - 8, 8, whenW, 20), Theme.Muted,
            TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        TextRenderer.DrawText(g, _name, Theme.Face(9.5f, FontStyle.Bold), new Rectangle(tx, 8, Math.Max(10, Width - tx - whenW - 14), 20), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, _sub, Theme.Face(8.5f), new Rectangle(tx, 28, Math.Max(10, Width - tx - 8), 18), Theme.Muted, LineFlags);
    }
}
