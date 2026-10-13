namespace MyApp;

/// <summary>A small captioned picture (pet photo or vaccination card). Click it to open the full-size viewer.</summary>
internal sealed class PetPictureBox : Panel
{
    private readonly PictureBox _box = new() { SizeMode = PictureBoxSizeMode.Zoom, BackColor = Theme.Subtle, Cursor = Cursors.Hand };
    private readonly Label _empty = new()
    {
        Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Theme.Muted,
        BackColor = Theme.Subtle, Font = Theme.Face(8.5f), UseMnemonic = false,
    };
    private Image? _image;
    private readonly string _title;

    public PetPictureBox(string title, string emptyText, Size size)
    {
        _title = title;
        _empty.Text = emptyText;
        Size = size;
        Padding = new Padding(1);
        BackColor = Theme.Border;
        _box.Dock = DockStyle.Fill;
        Controls.Add(_box);
        Controls.Add(_empty);
        _empty.BringToFront();
        _box.Click += (_, _) => OpenLarge();
    }

    /// <summary>Shows these picture bytes (JPEG/PNG), or the "no picture" text when null or unreadable.</summary>
    public void SetBytes(byte[]? bytes)
    {
        var old = _image;
        _image = AvatarImages.FromBytes(bytes);
        _box.Image = _image;
        old?.Dispose();
        _empty.Visible = _image is null;
        _box.Cursor = _image is null ? Cursors.Default : Cursors.Hand;
    }

    private void OpenLarge()
    {
        if (_image is null) return;
        using var viewer = new ImageViewerForm(_title, _image);
        viewer.ShowDialog(FindForm());
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _image?.Dispose();
        base.Dispose(disposing);
    }
}
