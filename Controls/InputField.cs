using System.ComponentModel;

namespace MyApp.Controls;

/// <summary>Rounded text box with a leading icon (and a Show/Hide toggle for passwords).</summary>
internal sealed class InputField : CardPanel
{
    private readonly Label _icon;
    private readonly TextBox _box;
    private readonly Label _toggle;
    private bool _isPassword;

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
        set => _icon.Text = value;
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
