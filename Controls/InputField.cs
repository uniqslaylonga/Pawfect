using System.ComponentModel;

namespace MyApp.Controls;

/// <summary>Rounded text box with a leading icon (and a Show/Hide toggle for passwords).</summary>
internal sealed class InputField : CardPanel
{
    private readonly Label _icon;
    private readonly TextBox _box;
    private readonly Label _toggle;
    private bool _isPassword;
    private readonly Panel _host;
    private Panel? _chipHost;
    private IconBadge? _chip;
    private Label? _captionLabel;
    private string _caption = "";

    public InputField()
    {
        Size = new Size(316, 42);
        Radius = 8;
        CornerColor = Color.White;
        Padding = new Padding(10, 3, 10, 3);

        _icon = Ui.Glyph("", 10f, Theme.Muted, Color.White);
        _icon.Dock = DockStyle.Left;
        _icon.Width = 28;

        _box = new TextBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            ForeColor = Theme.Text,
            Font = Theme.Face(10f),
        };
        var host = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0, 9, 0, 0) };
        _host = host;
        host.Controls.Add(_box);

        _toggle = Ui.Text("Show", 8.5f, Theme.Primary, Color.White, FontStyle.Bold, ContentAlignment.MiddleRight);
        _toggle.Dock = DockStyle.Right;
        _toggle.Width = 44;
        _toggle.Cursor = Cursors.Hand;
        _toggle.Visible = false;
        _toggle.Click += (_, _) =>
        {
            _box.UseSystemPasswordChar = !_box.UseSystemPasswordChar;
            _toggle.Text = _box.UseSystemPasswordChar ? "Show" : "Hide";
        };

        Controls.Add(host);     // Fill
        Controls.Add(_icon);    // Left
        Controls.Add(_toggle);  // Right

        _box.Enter += (_, _) => BorderColor = Theme.Primary;
        _box.Leave += (_, _) => BorderColor = Theme.Border;
    }

    [Category("Pawfect"), Description("Icon character from the Segoe MDL2 Assets font.")]
    [DefaultValue("")]
    public string Glyph
    {
        get => _icon.Text;
        set
        {
            _icon.Text = value;
            if (_chip is not null) _chip.Glyph = value;
        }
    }

    /// <summary>
    /// Small label shown above the typed text. When set, the field switches to the card look: a round icon chip on the left,
    /// this caption in small grey type, and the text under it. Fields without a caption keep the plain look.
    /// </summary>
    [Category("Pawfect"), Description("Small label above the text. Setting it switches the field to the icon-chip look.")]
    [DefaultValue("")]
    public string Caption
    {
        get => _caption;
        set { _caption = value ?? ""; ApplyLook(); }
    }

    private void ApplyLook()
    {
        bool chipLook = _caption.Length > 0;
        if (chipLook && _chip is null)
        {
            _chip = new IconBadge(_icon.Text, Theme.PrimarySoft, Theme.Primary, Color.White, 34, circle: true, glyphPt: 10f);
            _chipHost = new Panel { Dock = DockStyle.Left, Width = 48, BackColor = Color.White };
            _chipHost.Controls.Add(_chip);
            void Center() => _chip.Location = new Point(Math.Max(0, (_chipHost.Width - _chip.Width) / 2),
                                                        Math.Max(0, (_chipHost.Height - _chip.Height) / 2));
            _chipHost.Resize += (_, _) => Center();
            _chip.SizeChanged += (_, _) => Center();
            Center();
            Controls.Add(_chipHost);

            _captionLabel = Ui.Text(_caption, 7.5f, Theme.Muted, Color.White);
            _captionLabel.Dock = DockStyle.Top;
            _captionLabel.Height = 14;
            _box.Dock = DockStyle.Top;
            _host.Padding = new Padding(0, 5, 0, 0);
            _host.Controls.Add(_captionLabel);   // added after the box, so it sits above it
        }

        if (_chipHost is not null) _chipHost.Visible = chipLook;
        _icon.Visible = !chipLook;
        if (_captionLabel is not null) _captionLabel.Text = _caption;
    }

    [Category("Pawfect"), Description("Grey hint shown while the box is empty.")]
    [DefaultValue("")]
    public string Placeholder
    {
        get => _box.PlaceholderText;
        set => _box.PlaceholderText = value;
    }

    [Category("Pawfect"), Description("Hide the typed characters and show a Show/Hide link.")]
    [DefaultValue(false)]
    public bool IsPassword
    {
        get => _isPassword;
        set
        {
            _isPassword = value;
            _box.UseSystemPasswordChar = value;
            _toggle.Text = "Show";
            _toggle.Visible = value;
        }
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TextBox Box => _box;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Value => _box.Text;
}
