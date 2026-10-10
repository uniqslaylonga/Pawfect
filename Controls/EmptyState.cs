using System.ComponentModel;

namespace MyApp.Controls;

/// <summary>Centred icon + title + hint, shown wherever a list has no data yet.</summary>
internal sealed class EmptyState : Control
{
    private readonly Control _inner;
    private readonly IconBadge _badge;
    private readonly Label _title;
    private readonly Label _hint;

    public EmptyState()
    {
        TabStop = false;

        _inner = new Control { Size = new Size(280, 112), TabStop = false };
        _badge = new IconBadge { Size = new Size(44, 44), Circle = true, GlyphSize = 13f };
        _title = Ui.Text("", 9.5f, Theme.Text, Color.White, FontStyle.Bold, ContentAlignment.MiddleCenter);
        _hint = Ui.Text("", 8.5f, Theme.Muted, Color.White, FontStyle.Regular, ContentAlignment.TopCenter);
        _inner.Controls.AddRange(new Control[] { _badge, _title, _hint });
        Controls.Add(_inner);

        BackColor = Color.White; // also pushes the colour down to the children
        Relayout();
    }

    [Category("Pawfect"), Description("Icon character from the Segoe MDL2 Assets font.")]
    [DefaultValue("")]
    public string Glyph { get => _badge.Glyph; set => _badge.Glyph = value; }

    [Category("Pawfect"), Description("Bold heading under the icon.")]
    [DefaultValue("")]
    public string Title { get => _title.Text; set => _title.Text = value; }

    [Category("Pawfect"), Description("Smaller grey line under the heading.")]
    [DefaultValue("")]
    public string Hint { get => _hint.Text; set => _hint.Text = value; }

    [Category("Pawfect"), Description("Colour of the icon's circle.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BadgeFill { get => _badge.FillColor; set => _badge.FillColor = value; }

    [Category("Pawfect"), Description("Colour of the icon.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BadgeFore { get => _badge.ForeColor; set => _badge.ForeColor = value; }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (_inner is null) return;
        _inner.BackColor = BackColor;
        _badge.BackColor = BackColor;
        _title.BackColor = BackColor;
        _hint.BackColor = BackColor;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Relayout();
    }

    private void Relayout()
    {
        if (_inner is null) return;

        int w = Math.Min(280, Math.Max(40, Width));
        _inner.Width = w;
        _badge.Location = new Point((w - _badge.Width) / 2, 0);
        _title.SetBounds(0, 52, w, 20);
        _hint.SetBounds(0, 74, w, 36);
        _inner.Location = new Point((Width - w) / 2, Math.Max(0, (Height - _inner.Height) / 2));
    }
}
