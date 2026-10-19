namespace MyApp.Controls;

/// <summary>
/// One digit of the emailed code: a rounded, softly bordered box (instead of a plain Windows TextBox with a black
/// outline) that lights up blue while it has focus. The real TextBox lives inside as <see cref="Box"/>.
/// </summary>
internal sealed class OtpBox : CardPanel
{
    private static readonly Color IdleBorder = Color.FromArgb(205, 217, 234);

    private readonly TextBox _box;

    public OtpBox()
    {
        Size = new Size(47, 46);
        Radius = 10;
        FillColor = Color.White;
        CornerColor = Color.White;
        BorderColor = IdleBorder;

        _box = new TextBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            ForeColor = Theme.Text,
            Font = Theme.Face(16f, FontStyle.Bold),
            MaxLength = 1,
            TextAlign = HorizontalAlignment.Center,
        };
        Controls.Add(_box);

        _box.Enter += (_, _) => BorderColor = Theme.Primary;
        _box.Leave += (_, _) => BorderColor = IdleBorder;
        _box.EnabledChanged += (_, _) => ApplyEnabledLook();

        // Clicking the padding around the digit should still focus the box.
        Click += (_, _) => { if (_box.Enabled) _box.Focus(); };

        ApplyEnabledLook();
        LayoutBox();
    }

    /// <summary>The TextBox that receives the typing.</summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public TextBox Box => _box;

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);
        LayoutBox();
    }

    // When the form is scaled for the screen's DPI (125%, 150%, 200%...), WinForms resizes this box and THEN scales its
    // child TextBox by the same factor on top of the bounds LayoutBox just set. The TextBox ends up too wide and shifted
    // right: it paints white over the right border and rounded corners and pushes the digit off-centre. Re-centring it
    // once the scaling pass has finished layout puts it back inside the border.
    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);
        LayoutBox();
    }

    // Keeps the digit centred inside the rounded border, whatever the box size is.
    private void LayoutBox()
    {
        if (_box is null) return;
        int h = _box.PreferredHeight;
        _box.SetBounds(4, Math.Max(0, (Height - h) / 2), Math.Max(10, Width - 8), h);
    }

    private void ApplyEnabledLook()
    {
        var fill = _box.Enabled ? Color.White : Theme.Subtle;
        FillColor = fill;
        CornerColor = Color.White;
        _box.BackColor = fill;
        BorderColor = _box.Enabled ? (_box.Focused ? Theme.Primary : IdleBorder) : Theme.Border;
    }
}