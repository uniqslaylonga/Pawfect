using System.Drawing.Drawing2D;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Customer side, step 2: a third tab "Transaction History" next to My Pets and Add New Pet. The history itself goes to the
/// customer's email (so there is nothing to scroll through on a shared kiosk screen): the tab says where it will be sent,
/// one button sends it, and afterwards it says "See your transaction history at a******@gmail.com".
/// It is built in code so the Visual Studio designer file stays untouched.
/// </summary>
internal sealed partial class AddPetForm
{
    private SegmentButton? _tabHistory;
    private Panel? _historyView;
    private Label? _historyText;
    private Label? _historyStatus;
    private PawButton? _historySendButton;
    private bool _historyBusy;

    /// <summary>Adds the third tab and its panel the first time step 2 is shown (the first two tabs are re-spaced to make room).</summary>
    private void EnsureHistoryTab()
    {
        if (_tabHistory is not null) return;

        // The two designer tabs (already scaled to this screen) are narrowed so three equal tabs span the same width as before.
        int gap = tabAddPet.Left - tabMyPets.Right;
        int total = tabAddPet.Right - tabMyPets.Left;
        int w = (total - gap * 2) / 3;
        tabMyPets.SetBounds(tabMyPets.Left, tabMyPets.Top, w, tabMyPets.Height);
        tabAddPet.SetBounds(tabMyPets.Left + w + gap, tabAddPet.Top, w, tabAddPet.Height);

        // Everything below is written in the 900 x 540 design; the content scaler grows it when it joins the panel.
        _tabHistory = new SegmentButton
        {
            Name = "tabHistory",
            Text = "Transaction History",
            Glyph = "\uE81C",   // history clock
            BackColor = Color.FromArgb(243, 247, 252),
            Location = new Point(596, 0),
            Size = new Size(264, 44),
        };
        _tabHistory.Click += (_, _) => ShowHistoryTab();

        _historyView = new Panel { Location = new Point(40, 60), Size = new Size(820, 262), BackColor = Theme.Bg, Visible = false };

        var card = new Panel { Location = new Point(0, 0), Size = new Size(820, 200), BackColor = Theme.Bg };
        card.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            float k = Math.Max(0.1f, card.Width / 820f);
            e.Graphics.ScaleTransform(k, k);   // drawn in the original 820 x 200 design, then enlarged
            using var path = RoundedRect(new Rectangle(0, 0, 819, 199), 14);
            using var fill = new SolidBrush(Color.White);
            using var pen = new Pen(Theme.Border);
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(pen, path);
            using var badge = new SolidBrush(Theme.PrimarySoft);
            e.Graphics.FillEllipse(badge, new Rectangle(32, 32, 52, 52));
        };

        var icon = new Label
        {
            Location = new Point(32, 32), Size = new Size(52, 52), BackColor = Color.Transparent, ForeColor = Theme.Primary,
            Font = Theme.GlyphFont(16f), Text = "\uE715", TextAlign = ContentAlignment.MiddleCenter,   // mail
        };
        var title = new Label
        {
            Location = new Point(100, 32), Size = new Size(690, 28), BackColor = Color.Transparent, ForeColor = Theme.Text,
            Font = Theme.Face(13f, FontStyle.Bold), Text = "Your transaction history", TextAlign = ContentAlignment.MiddleLeft,
        };
        _historyText = new Label
        {
            Location = new Point(100, 62), Size = new Size(690, 44), BackColor = Color.Transparent, ForeColor = Theme.TextSoft,
            Font = Theme.Face(10f), TextAlign = ContentAlignment.TopLeft,
        };
        _historySendButton = new PawButton
        {
            Location = new Point(100, 118), Size = new Size(210, 42), Kind = PawButtonKind.Primary, BackColor = Color.White,
            Font = Theme.Face(10f, FontStyle.Bold), Text = "Email my history  \u2192", Radius = 12,
        };
        _historySendButton.Click += async (_, _) => await SendHistoryAsync();
        _historyStatus = new Label
        {
            Location = new Point(100, 164), Size = new Size(690, 28), BackColor = Color.Transparent, ForeColor = Theme.Muted,
            Font = Theme.Face(9.5f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft,
        };

        card.Controls.AddRange(new Control[] { icon, title, _historyText, _historySendButton, _historyStatus });
        _historyView.Controls.Add(card);

        selectPanel.Controls.Add(_historyView);
        selectPanel.Controls.Add(_tabHistory);
    }

    /// <summary>The address the history goes to: the email on the customer's record, else the one they signed in / were verified with.</summary>
    private string HistoryEmail()
        => TransactionHistoryMailer.ResolveEmail(_customerId, _kiosk ? ContactEmail : _loginEmail);

    private void ShowHistoryTab()
    {
        EnsureHistoryTab();
        tabMyPets.Active = false;
        tabAddPet.Active = false;
        _tabHistory!.Active = true;
        myPetsView.Visible = false;
        addPetView.Visible = false;
        _historyView!.Visible = true;
        continueButton.Visible = false;

        string email = HistoryEmail();
        if (email.Length == 0)
        {
            _historyText!.Text = "There's no email address on your account yet, so we can't send your history. Please ask the front desk to add one.";
            _historySendButton!.Enabled = false;
        }
        else
        {
            _historyText!.Text = $"We'll email every transaction on your account (what you paid for, the date and the total) to {TransactionHistoryMailer.Mask(email)}.";
            _historySendButton!.Enabled = !_historyBusy;
        }
        _historyStatus!.Text = "";
    }

    /// <summary>Called by ShowTab so the other two tabs hide this one.</summary>
    private void HideHistoryTab()
    {
        if (_tabHistory is null) return;
        _tabHistory.Active = false;
        _historyView!.Visible = false;
    }

    private async Task SendHistoryAsync()
    {
        if (_historyBusy) return;
        _historyBusy = true;
        _historySendButton!.Enabled = false;
        _historyStatus!.ForeColor = Theme.Muted;
        _historyStatus.Text = "Sending...";
        try
        {
            string name = string.IsNullOrWhiteSpace(_ownerName) ? _typedName : _ownerName;
            var (masked, count) = await TransactionHistoryMailer.SendAsync(_customerId, name, _kiosk ? ContactEmail : _loginEmail);
            if (count == 0)
            {
                _historyStatus.Text = "You don't have any transactions yet.";
            }
            else
            {
                _historyStatus.ForeColor = Theme.Green;
                _historyStatus.Text = $"\u2713  See your transaction history at {masked}";
            }
        }
        catch (OtpException ex)
        {
            _historyStatus.ForeColor = Theme.Danger;
            _historyStatus.Text = ex.Message.Split('\n')[0].Trim();
        }
        catch (Exception ex)
        {
            _historyStatus.ForeColor = Theme.Danger;
            _historyStatus.Text = "Couldn't send your history. " + ex.Message.Split('\n')[0].Trim();
        }
        finally
        {
            _historyBusy = false;
            if (!IsDisposed) _historySendButton.Enabled = HistoryEmail().Length > 0;
        }
    }
}
