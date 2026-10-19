using MyApp.Controls;

namespace MyApp;

/// <summary>Base class that gives every popup the same look as the app's pages.</summary>
internal abstract class ThemedDialog : Form
{
    private const int Pad = 24;
    private readonly int Width_;
    private int FieldW => Width_ - Pad * 2;
    private const int HeaderH = 76;
    private const int FooterH = 68;

    private readonly Panel _content = new() { Dock = DockStyle.Fill, BackColor = Color.White };
    private EventHandler? _onSave;
    private int _y = 20;

    protected ThemedDialog(string title, string subtitle, int width = 400)
    {
        Width_ = width;
        Text = title;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = MinimizeBox = ShowInTaskbar = false;
        BackColor = Color.White;
        Font = Theme.Face(9.5f);
        ClientSize = new Size(Width_, 300);

        var header = new EdgePanel
        {
            Dock = DockStyle.Top,
            Height = HeaderH,
            BackColor = Color.White,
            EdgeColor = Theme.Border,
            Side = Edge.Bottom,
            Padding = new Padding(0, 0, 0, 1),
        };
        var t = Ui.Text(title, 13f, Theme.Text, Color.White, FontStyle.Bold, ContentAlignment.MiddleLeft);
        t.SetBounds(Pad, 14, FieldW, 28);
        var s = Ui.Text(subtitle, 8.5f, Theme.Muted, Color.White, FontStyle.Regular, ContentAlignment.MiddleLeft);
        s.SetBounds(Pad, 42, FieldW, 20);
        header.Controls.Add(t);
        header.Controls.Add(s);

        Controls.Add(_content);   // Fill first, then Top and Bottom
        Controls.Add(header);
    }

    // ---------------------------------------------------------------- field helpers

    private void Place(string label, Control c)
    {
        var l = Ui.Text(label, 8.5f, Theme.TextSoft, Color.White, FontStyle.Regular, ContentAlignment.MiddleLeft);
        l.SetBounds(Pad, _y, FieldW, 18);
        c.SetBounds(Pad, _y + 20, FieldW, 42);
        _content.Controls.Add(l);
        _content.Controls.Add(c);
        _y += 70;
    }

    private static CardPanel Boxed(Control inner)
    {
        var host = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0, 7, 0, 0) };
        inner.Dock = DockStyle.Fill;
        host.Controls.Add(inner);

        var card = new CardPanel
        {
            Radius = 8,
            BorderColor = Theme.Border,
            FillColor = Color.White,
            CornerColor = Color.White,
            Padding = new Padding(10, 3, 10, 3),
        };
        card.Controls.Add(host);
        return card;
    }

    protected InputField AddInput(string label, string glyph, string placeholder = "",
                                  string? text = null, bool password = false)
    {
        var f = new InputField { Glyph = glyph, Placeholder = placeholder, IsPassword = password };
        f.Box.Text = text ?? "";
        Place(label, f);
        return f;
    }

    protected ComboBox AddCombo(string label, IEnumerable<string> items, string? selected = null)
    {
        var combo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Theme.Text,
            Font = Theme.Face(10f),
        };
        // Items (not DataSource), so the selection can't be reset when the dialog opens.
        var list = items.ToArray();
        combo.Items.AddRange(list);
        int i = selected == null ? 0 : Array.IndexOf(list, selected);
        combo.SelectedIndex = i < 0 ? 0 : i;
        Place(label, Boxed(combo));
        return combo;
    }

    protected DateTimePicker AddDate(string label, DateTime value)
    {
        var d = new DateTimePicker { Format = DateTimePickerFormat.Long, Font = Theme.Face(10f), Value = value };
        Place(label, Boxed(d));
        return d;
    }

    protected NumericUpDown AddNumber(string label, decimal value, decimal min, decimal max,
                                      int decimals = 0, decimal step = 1)
    {
        var n = new NumericUpDown
        {
            Minimum = min,
            Maximum = max,
            DecimalPlaces = decimals,
            Increment = step,
            Value = Math.Min(max, Math.Max(min, value)),
            BorderStyle = BorderStyle.None,
            Font = Theme.Face(10f),
            BackColor = Color.White,
            ForeColor = Theme.Text,
        };
        Place(label, Boxed(n));
        return n;
    }

    /// <summary>Places any control (a list, a tab control) in the dialog.</summary>
    protected void AddCustom(string? label, Control c, int height)
    {
        if (label != null)
        {
            var l = Ui.Text(label, 8.5f, Theme.TextSoft, Color.White, FontStyle.Regular, ContentAlignment.MiddleLeft);
            l.SetBounds(Pad, _y, FieldW, 18);
            _content.Controls.Add(l);
            _y += 20;
        }
        c.SetBounds(Pad, _y, FieldW, height);
        _content.Controls.Add(c);
        _y += height + 12;
    }

    protected CheckBox AddCheck(string text, bool value = false)
    {
        var c = new CheckBox
        {
            Text = text,
            Checked = value,
            AutoSize = true,
            BackColor = Color.White,
            ForeColor = Theme.TextSoft,
            Font = Theme.Face(9f),
            Location = new Point(Pad, _y),
        };
        _content.Controls.Add(c);
        _y += 34;
        return c;
    }

    /// <summary>Adds the Cancel and Save buttons and sizes the dialog. Call this last.</summary>
    protected void AddButtons(string saveText, EventHandler onSave)
    {
        _onSave = onSave;

        var footer = new EdgePanel
        {
            Dock = DockStyle.Bottom,
            Height = FooterH,
            BackColor = Color.White,
            EdgeColor = Theme.Border,
            Side = Edge.Top,
            Padding = new Padding(0, 1, 0, 0),
        };
        var save = new PrimaryButton { Text = saveText, Size = new Size(132, 38) };
        save.Location = new Point(Width_ - Pad - save.Width, 16);
        save.Click += onSave;

        var cancel = new OutlineButton { Text = "Cancel", Size = new Size(96, 38) };
        cancel.Location = new Point(save.Left - 10 - cancel.Width, 16);
        cancel.Click += (_, _) => DialogResult = DialogResult.Cancel;

        footer.Controls.Add(save);
        footer.Controls.Add(cancel);
        Controls.Add(footer);

        ClientSize = new Size(Width_, HeaderH + _y + 4 + FooterH);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Enter && _onSave != null) { _onSave(this, EventArgs.Empty); return true; }
        if (keyData == Keys.Escape) { DialogResult = DialogResult.Cancel; return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}