#nullable disable
namespace MyApp
{
    partial class AppointmentsPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            bodyGrid = new TableLayoutPanel();
            leftColumn = new TableLayoutPanel();
            statsRow = new TableLayoutPanel();
            totalCard = new MyApp.Controls.CardPanel();
            totalIcon = new MyApp.Controls.IconBadge();
            totalDelta = new Label();
            totalValue = new Label();
            totalLabel = new Label();
            pendingCard = new MyApp.Controls.CardPanel();
            pendingIcon = new MyApp.Controls.IconBadge();
            pendingDelta = new Label();
            pendingValue = new Label();
            pendingLabel = new Label();
            confirmedCard = new MyApp.Controls.CardPanel();
            confirmedIcon = new MyApp.Controls.IconBadge();
            confirmedDelta = new Label();
            confirmedValue = new Label();
            confirmedLabel = new Label();
            cancelledCard = new MyApp.Controls.CardPanel();
            cancelledIcon = new MyApp.Controls.IconBadge();
            cancelledDelta = new Label();
            cancelledValue = new Label();
            cancelledLabel = new Label();
            appointmentsCard = new MyApp.Controls.CardPanel();
            rowsHost = new Panel();
            appointmentsEmpty = new MyApp.Controls.EmptyState();
            tableFooter = new Panel();
            showingLabel = new Label();
            pager = new Panel();
            nextPageButton = new MyApp.Controls.IconButton();
            pageChip = new MyApp.Controls.PrimaryButton();
            prevPageButton = new MyApp.Controls.IconButton();
            columnsHead = new Panel();
            columnsGrid = new TableLayoutPanel();
            headerCheck = new MyApp.Controls.CheckMark();
            colDate = new Label();
            colCustomer = new Label();
            colPet = new Label();
            colService = new Label();
            colStatus = new Label();
            colActions = new Label();
            toolbar = new Panel();
            searchField = new MyApp.Controls.InputField();
            statusFilter = new MyApp.Controls.SelectField();
            serviceFilter = new MyApp.Controls.SelectField();
            dateRange = new MyApp.Controls.DateRangeField();
            appointmentsHeader = new Panel();
            newAppointmentButton = new MyApp.Controls.PrimaryButton();
            appointmentsTitle = new Label();
            appointmentsIcon = new Label();
            rightColumn = new TableLayoutPanel();
            calendarCard = new MyApp.Controls.CardPanel();
            monthCalendar = new MyApp.Controls.MonthCalendarView();
            legendFlow = new FlowLayoutPanel();
            legendConfirmedDot = new MyApp.Controls.Dot();
            legendConfirmedLabel = new Label();
            legendPendingDot = new MyApp.Controls.Dot();
            legendPendingLabel = new Label();
            legendRescheduledDot = new MyApp.Controls.Dot();
            legendRescheduledLabel = new Label();
            legendCancelledDot = new MyApp.Controls.Dot();
            legendCancelledLabel = new Label();
            calendarHeader = new Panel();
            calendarLink = new Label();
            calendarTitle = new Label();
            calendarIcon = new Label();
            quickCard = new MyApp.Controls.CardPanel();
            quickGrid = new TableLayoutPanel();
            newAppointmentTile = new MyApp.Controls.ActionTile();
            viewAllTile = new MyApp.Controls.ActionTile();
            manageCustomersTile = new MyApp.Controls.ActionTile();
            servicesCatalogTile = new MyApp.Controls.ActionTile();
            quickHeader = new Panel();
            quickTitle = new Label();
            quickIcon = new Label();
            upcomingCard = new MyApp.Controls.CardPanel();
            upcomingList = new Panel();
            upcomingEmpty = new MyApp.Controls.EmptyState();
            upcomingHeader = new Panel();
            upcomingLink = new Label();
            upcomingTitle = new Label();
            upcomingIcon = new Label();
            bodyGrid.SuspendLayout();
            leftColumn.SuspendLayout();
            statsRow.SuspendLayout();
            totalCard.SuspendLayout();
            pendingCard.SuspendLayout();
            confirmedCard.SuspendLayout();
            cancelledCard.SuspendLayout();
            appointmentsCard.SuspendLayout();
            rowsHost.SuspendLayout();
            tableFooter.SuspendLayout();
            pager.SuspendLayout();
            columnsHead.SuspendLayout();
            columnsGrid.SuspendLayout();
            toolbar.SuspendLayout();
            appointmentsHeader.SuspendLayout();
            rightColumn.SuspendLayout();
            calendarCard.SuspendLayout();
            legendFlow.SuspendLayout();
            calendarHeader.SuspendLayout();
            quickCard.SuspendLayout();
            quickGrid.SuspendLayout();
            quickHeader.SuspendLayout();
            upcomingCard.SuspendLayout();
            upcomingHeader.SuspendLayout();
            SuspendLayout();
            // 
            // bodyGrid
            // 
            bodyGrid.BackColor = Color.FromArgb(243, 247, 252);
            bodyGrid.ColumnCount = 2;
            bodyGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bodyGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            bodyGrid.Controls.Add(leftColumn, 0, 0);
            bodyGrid.Controls.Add(rightColumn, 1, 0);
            bodyGrid.Dock = DockStyle.Fill;
            bodyGrid.Location = new Point(20, 20);
            bodyGrid.Margin = new Padding(0);
            bodyGrid.Name = "bodyGrid";
            bodyGrid.RowCount = 1;
            bodyGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bodyGrid.Size = new Size(996, 692);
            bodyGrid.TabIndex = 0;
            // 
            // leftColumn
            // 
            leftColumn.BackColor = Color.FromArgb(243, 247, 252);
            leftColumn.ColumnCount = 1;
            leftColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            leftColumn.Controls.Add(statsRow, 0, 0);
            leftColumn.Controls.Add(appointmentsCard, 0, 1);
            leftColumn.Dock = DockStyle.Fill;
            leftColumn.Location = new Point(0, 0);
            leftColumn.Margin = new Padding(0, 0, 16, 0);
            leftColumn.Name = "leftColumn";
            leftColumn.RowCount = 2;
            leftColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 124F));
            leftColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            leftColumn.Size = new Size(660, 692);
            leftColumn.TabIndex = 0;
            // 
            // statsRow
            // 
            statsRow.BackColor = Color.FromArgb(243, 247, 252);
            statsRow.ColumnCount = 4;
            statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsRow.Controls.Add(totalCard, 0, 0);
            statsRow.Controls.Add(pendingCard, 1, 0);
            statsRow.Controls.Add(confirmedCard, 2, 0);
            statsRow.Controls.Add(cancelledCard, 3, 0);
            statsRow.Dock = DockStyle.Fill;
            statsRow.Location = new Point(0, 0);
            statsRow.Margin = new Padding(0, 0, 0, 16);
            statsRow.Name = "statsRow";
            statsRow.RowCount = 1;
            statsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statsRow.Size = new Size(660, 108);
            statsRow.TabIndex = 0;
            // 
            // totalCard
            // 
            totalCard.BorderColor = Color.FromArgb(227, 233, 242);
            totalCard.Controls.Add(totalIcon);
            totalCard.Controls.Add(totalDelta);
            totalCard.Controls.Add(totalValue);
            totalCard.Controls.Add(totalLabel);
            totalCard.CornerColor = Color.FromArgb(243, 247, 252);
            totalCard.Dock = DockStyle.Fill;
            totalCard.FillColor = Color.White;
            totalCard.Location = new Point(0, 0);
            totalCard.Margin = new Padding(0, 0, 12, 0);
            totalCard.Name = "totalCard";
            totalCard.Padding = new Padding(14, 12, 14, 8);
            totalCard.Radius = 10;
            totalCard.Size = new Size(153, 108);
            totalCard.TabIndex = 0;
            // 
            // totalIcon
            // 
            totalIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            totalIcon.BackColor = Color.White;
            totalIcon.FillColor = Color.FromArgb(234, 242, 253);
            totalIcon.ForeColor = Color.FromArgb(58, 123, 213);
            totalIcon.Glyph = "";
            totalIcon.GlyphSize = 9.5F;
            totalIcon.Location = new Point(111, 42);
            totalIcon.Name = "totalIcon";
            totalIcon.Size = new Size(28, 28);
            totalIcon.TabIndex = 0;
            totalIcon.TabStop = false;
            // 
            // totalDelta
            // 
            totalDelta.BackColor = Color.FromArgb(255, 255, 255);
            totalDelta.Dock = DockStyle.Top;
            totalDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalDelta.ForeColor = Color.FromArgb(122, 136, 156);
            totalDelta.Location = new Point(14, 60);
            totalDelta.Name = "totalDelta";
            totalDelta.Size = new Size(125, 16);
            totalDelta.TabIndex = 1;
            totalDelta.Text = "0 today";
            totalDelta.TextAlign = ContentAlignment.MiddleLeft;
            totalDelta.UseMnemonic = false;
            // 
            // totalValue
            // 
            totalValue.BackColor = Color.FromArgb(255, 255, 255);
            totalValue.Dock = DockStyle.Top;
            totalValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalValue.ForeColor = Color.FromArgb(31, 42, 60);
            totalValue.Location = new Point(14, 30);
            totalValue.Name = "totalValue";
            totalValue.Size = new Size(125, 30);
            totalValue.TabIndex = 2;
            totalValue.Text = "0";
            totalValue.TextAlign = ContentAlignment.MiddleLeft;
            totalValue.UseMnemonic = false;
            // 
            // totalLabel
            // 
            totalLabel.BackColor = Color.FromArgb(255, 255, 255);
            totalLabel.Dock = DockStyle.Top;
            totalLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalLabel.ForeColor = Color.FromArgb(68, 83, 106);
            totalLabel.Location = new Point(14, 12);
            totalLabel.Name = "totalLabel";
            totalLabel.Size = new Size(125, 18);
            totalLabel.TabIndex = 3;
            totalLabel.Text = "Total Appointments";
            totalLabel.TextAlign = ContentAlignment.MiddleLeft;
            totalLabel.UseMnemonic = false;
            // 
            // pendingCard
            // 
            pendingCard.BorderColor = Color.FromArgb(227, 233, 242);
            pendingCard.Controls.Add(pendingIcon);
            pendingCard.Controls.Add(pendingDelta);
            pendingCard.Controls.Add(pendingValue);
            pendingCard.Controls.Add(pendingLabel);
            pendingCard.CornerColor = Color.FromArgb(243, 247, 252);
            pendingCard.Dock = DockStyle.Fill;
            pendingCard.FillColor = Color.White;
            pendingCard.Location = new Point(165, 0);
            pendingCard.Margin = new Padding(0, 0, 12, 0);
            pendingCard.Name = "pendingCard";
            pendingCard.Padding = new Padding(14, 12, 14, 8);
            pendingCard.Radius = 10;
            pendingCard.Size = new Size(153, 108);
            pendingCard.TabIndex = 1;
            // 
            // pendingIcon
            // 
            pendingIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pendingIcon.BackColor = Color.White;
            pendingIcon.FillColor = Color.FromArgb(254, 243, 224);
            pendingIcon.ForeColor = Color.FromArgb(245, 158, 11);
            pendingIcon.Glyph = "";
            pendingIcon.GlyphSize = 9.5F;
            pendingIcon.Location = new Point(111, 42);
            pendingIcon.Name = "pendingIcon";
            pendingIcon.Size = new Size(28, 28);
            pendingIcon.TabIndex = 0;
            pendingIcon.TabStop = false;
            // 
            // pendingDelta
            // 
            pendingDelta.BackColor = Color.FromArgb(255, 255, 255);
            pendingDelta.Dock = DockStyle.Top;
            pendingDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pendingDelta.ForeColor = Color.FromArgb(122, 136, 156);
            pendingDelta.Location = new Point(14, 60);
            pendingDelta.Name = "pendingDelta";
            pendingDelta.Size = new Size(125, 16);
            pendingDelta.TabIndex = 1;
            pendingDelta.Text = "0 today";
            pendingDelta.TextAlign = ContentAlignment.MiddleLeft;
            pendingDelta.UseMnemonic = false;
            // 
            // pendingValue
            // 
            pendingValue.BackColor = Color.FromArgb(255, 255, 255);
            pendingValue.Dock = DockStyle.Top;
            pendingValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pendingValue.ForeColor = Color.FromArgb(31, 42, 60);
            pendingValue.Location = new Point(14, 30);
            pendingValue.Name = "pendingValue";
            pendingValue.Size = new Size(125, 30);
            pendingValue.TabIndex = 2;
            pendingValue.Text = "0";
            pendingValue.TextAlign = ContentAlignment.MiddleLeft;
            pendingValue.UseMnemonic = false;
            // 
            // pendingLabel
            // 
            pendingLabel.BackColor = Color.FromArgb(255, 255, 255);
            pendingLabel.Dock = DockStyle.Top;
            pendingLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pendingLabel.ForeColor = Color.FromArgb(68, 83, 106);
            pendingLabel.Location = new Point(14, 12);
            pendingLabel.Name = "pendingLabel";
            pendingLabel.Size = new Size(125, 18);
            pendingLabel.TabIndex = 3;
            pendingLabel.Text = "Pending";
            pendingLabel.TextAlign = ContentAlignment.MiddleLeft;
            pendingLabel.UseMnemonic = false;
            // 
            // confirmedCard
            // 
            confirmedCard.BorderColor = Color.FromArgb(227, 233, 242);
            confirmedCard.Controls.Add(confirmedIcon);
            confirmedCard.Controls.Add(confirmedDelta);
            confirmedCard.Controls.Add(confirmedValue);
            confirmedCard.Controls.Add(confirmedLabel);
            confirmedCard.CornerColor = Color.FromArgb(243, 247, 252);
            confirmedCard.Dock = DockStyle.Fill;
            confirmedCard.FillColor = Color.White;
            confirmedCard.Location = new Point(330, 0);
            confirmedCard.Margin = new Padding(0, 0, 12, 0);
            confirmedCard.Name = "confirmedCard";
            confirmedCard.Padding = new Padding(14, 12, 14, 8);
            confirmedCard.Radius = 10;
            confirmedCard.Size = new Size(153, 108);
            confirmedCard.TabIndex = 2;
            // 
            // confirmedIcon
            // 
            confirmedIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            confirmedIcon.BackColor = Color.White;
            confirmedIcon.FillColor = Color.FromArgb(229, 246, 236);
            confirmedIcon.ForeColor = Color.FromArgb(46, 158, 91);
            confirmedIcon.Glyph = "";
            confirmedIcon.GlyphSize = 9.5F;
            confirmedIcon.Location = new Point(111, 42);
            confirmedIcon.Name = "confirmedIcon";
            confirmedIcon.Size = new Size(28, 28);
            confirmedIcon.TabIndex = 0;
            confirmedIcon.TabStop = false;
            // 
            // confirmedDelta
            // 
            confirmedDelta.BackColor = Color.FromArgb(255, 255, 255);
            confirmedDelta.Dock = DockStyle.Top;
            confirmedDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            confirmedDelta.ForeColor = Color.FromArgb(122, 136, 156);
            confirmedDelta.Location = new Point(14, 60);
            confirmedDelta.Name = "confirmedDelta";
            confirmedDelta.Size = new Size(125, 16);
            confirmedDelta.TabIndex = 1;
            confirmedDelta.Text = "0 today";
            confirmedDelta.TextAlign = ContentAlignment.MiddleLeft;
            confirmedDelta.UseMnemonic = false;
            // 
            // confirmedValue
            // 
            confirmedValue.BackColor = Color.FromArgb(255, 255, 255);
            confirmedValue.Dock = DockStyle.Top;
            confirmedValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            confirmedValue.ForeColor = Color.FromArgb(31, 42, 60);
            confirmedValue.Location = new Point(14, 30);
            confirmedValue.Name = "confirmedValue";
            confirmedValue.Size = new Size(125, 30);
            confirmedValue.TabIndex = 2;
            confirmedValue.Text = "0";
            confirmedValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmedValue.UseMnemonic = false;
            // 
            // confirmedLabel
            // 
            confirmedLabel.BackColor = Color.FromArgb(255, 255, 255);
            confirmedLabel.Dock = DockStyle.Top;
            confirmedLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            confirmedLabel.ForeColor = Color.FromArgb(68, 83, 106);
            confirmedLabel.Location = new Point(14, 12);
            confirmedLabel.Name = "confirmedLabel";
            confirmedLabel.Size = new Size(125, 18);
            confirmedLabel.TabIndex = 3;
            confirmedLabel.Text = "Confirmed";
            confirmedLabel.TextAlign = ContentAlignment.MiddleLeft;
            confirmedLabel.UseMnemonic = false;
            // 
            // cancelledCard
            // 
            cancelledCard.BorderColor = Color.FromArgb(227, 233, 242);
            cancelledCard.Controls.Add(cancelledIcon);
            cancelledCard.Controls.Add(cancelledDelta);
            cancelledCard.Controls.Add(cancelledValue);
            cancelledCard.Controls.Add(cancelledLabel);
            cancelledCard.CornerColor = Color.FromArgb(243, 247, 252);
            cancelledCard.Dock = DockStyle.Fill;
            cancelledCard.FillColor = Color.White;
            cancelledCard.Location = new Point(495, 0);
            cancelledCard.Margin = new Padding(0);
            cancelledCard.Name = "cancelledCard";
            cancelledCard.Padding = new Padding(14, 12, 14, 8);
            cancelledCard.Radius = 10;
            cancelledCard.Size = new Size(165, 108);
            cancelledCard.TabIndex = 3;
            // 
            // cancelledIcon
            // 
            cancelledIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cancelledIcon.BackColor = Color.White;
            cancelledIcon.FillColor = Color.FromArgb(253, 232, 232);
            cancelledIcon.ForeColor = Color.FromArgb(239, 68, 68);
            cancelledIcon.Glyph = "";
            cancelledIcon.GlyphSize = 9.5F;
            cancelledIcon.Location = new Point(123, 42);
            cancelledIcon.Name = "cancelledIcon";
            cancelledIcon.Size = new Size(28, 28);
            cancelledIcon.TabIndex = 0;
            cancelledIcon.TabStop = false;
            // 
            // cancelledDelta
            // 
            cancelledDelta.BackColor = Color.FromArgb(255, 255, 255);
            cancelledDelta.Dock = DockStyle.Top;
            cancelledDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cancelledDelta.ForeColor = Color.FromArgb(122, 136, 156);
            cancelledDelta.Location = new Point(14, 60);
            cancelledDelta.Name = "cancelledDelta";
            cancelledDelta.Size = new Size(137, 16);
            cancelledDelta.TabIndex = 1;
            cancelledDelta.Text = "0 today";
            cancelledDelta.TextAlign = ContentAlignment.MiddleLeft;
            cancelledDelta.UseMnemonic = false;
            // 
            // cancelledValue
            // 
            cancelledValue.BackColor = Color.FromArgb(255, 255, 255);
            cancelledValue.Dock = DockStyle.Top;
            cancelledValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cancelledValue.ForeColor = Color.FromArgb(31, 42, 60);
            cancelledValue.Location = new Point(14, 30);
            cancelledValue.Name = "cancelledValue";
            cancelledValue.Size = new Size(137, 30);
            cancelledValue.TabIndex = 2;
            cancelledValue.Text = "0";
            cancelledValue.TextAlign = ContentAlignment.MiddleLeft;
            cancelledValue.UseMnemonic = false;
            // 
            // cancelledLabel
            // 
            cancelledLabel.BackColor = Color.FromArgb(255, 255, 255);
            cancelledLabel.Dock = DockStyle.Top;
            cancelledLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cancelledLabel.ForeColor = Color.FromArgb(68, 83, 106);
            cancelledLabel.Location = new Point(14, 12);
            cancelledLabel.Name = "cancelledLabel";
            cancelledLabel.Size = new Size(137, 18);
            cancelledLabel.TabIndex = 3;
            cancelledLabel.Text = "Cancelled";
            cancelledLabel.TextAlign = ContentAlignment.MiddleLeft;
            cancelledLabel.UseMnemonic = false;
            // 
            // appointmentsCard
            // 
            appointmentsCard.BorderColor = Color.FromArgb(227, 233, 242);
            appointmentsCard.Controls.Add(rowsHost);
            appointmentsCard.Controls.Add(tableFooter);
            appointmentsCard.Controls.Add(columnsHead);
            appointmentsCard.Controls.Add(toolbar);
            appointmentsCard.Controls.Add(appointmentsHeader);
            appointmentsCard.CornerColor = Color.FromArgb(243, 247, 252);
            appointmentsCard.Dock = DockStyle.Fill;
            appointmentsCard.FillColor = Color.White;
            appointmentsCard.Location = new Point(0, 124);
            appointmentsCard.Margin = new Padding(0);
            appointmentsCard.Name = "appointmentsCard";
            appointmentsCard.Padding = new Padding(16, 12, 16, 12);
            appointmentsCard.Radius = 10;
            appointmentsCard.Size = new Size(660, 568);
            appointmentsCard.TabIndex = 1;
            // 
            // rowsHost
            // 
            rowsHost.AutoScroll = true;
            rowsHost.BackColor = Color.White;
            rowsHost.Controls.Add(appointmentsEmpty);
            rowsHost.Dock = DockStyle.Fill;
            rowsHost.Location = new Point(16, 142);
            rowsHost.Name = "rowsHost";
            rowsHost.Size = new Size(628, 374);
            rowsHost.TabIndex = 0;
            // 
            // appointmentsEmpty
            // 
            appointmentsEmpty.BackColor = Color.White;
            appointmentsEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            appointmentsEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            appointmentsEmpty.Dock = DockStyle.Fill;
            appointmentsEmpty.Glyph = "";
            appointmentsEmpty.Hint = "Appointments will show up here once they are booked.";
            appointmentsEmpty.Location = new Point(0, 0);
            appointmentsEmpty.Name = "appointmentsEmpty";
            appointmentsEmpty.Size = new Size(628, 374);
            appointmentsEmpty.TabIndex = 0;
            appointmentsEmpty.TabStop = false;
            appointmentsEmpty.Title = "No appointments found";
            // 
            // tableFooter
            // 
            tableFooter.BackColor = Color.FromArgb(255, 255, 255);
            tableFooter.Controls.Add(showingLabel);
            tableFooter.Controls.Add(pager);
            tableFooter.Dock = DockStyle.Bottom;
            tableFooter.Location = new Point(16, 516);
            tableFooter.Name = "tableFooter";
            tableFooter.Size = new Size(628, 40);
            tableFooter.TabIndex = 1;
            // 
            // showingLabel
            // 
            showingLabel.BackColor = Color.FromArgb(255, 255, 255);
            showingLabel.Dock = DockStyle.Fill;
            showingLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            showingLabel.ForeColor = Color.FromArgb(122, 136, 156);
            showingLabel.Location = new Point(0, 0);
            showingLabel.Name = "showingLabel";
            showingLabel.Size = new Size(516, 40);
            showingLabel.TabIndex = 0;
            showingLabel.Text = "Showing 0 appointments";
            showingLabel.TextAlign = ContentAlignment.MiddleLeft;
            showingLabel.UseMnemonic = false;
            // 
            // pager
            // 
            pager.BackColor = Color.FromArgb(255, 255, 255);
            pager.Controls.Add(nextPageButton);
            pager.Controls.Add(pageChip);
            pager.Controls.Add(prevPageButton);
            pager.Dock = DockStyle.Right;
            pager.Location = new Point(516, 0);
            pager.Name = "pager";
            pager.Size = new Size(112, 40);
            pager.TabIndex = 1;
            // 
            // nextPageButton
            // 
            nextPageButton.BackColor = Color.White;
            nextPageButton.Enabled = false;
            nextPageButton.Glyph = "";
            nextPageButton.Location = new Point(78, 5);
            nextPageButton.Name = "nextPageButton";
            nextPageButton.Size = new Size(30, 30);
            nextPageButton.TabIndex = 0;
            nextPageButton.Click += nextPageButton_Click;
            // 
            // pageChip
            // 
            pageChip.BackColor = Color.White;
            pageChip.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pageChip.Location = new Point(42, 6);
            pageChip.Name = "pageChip";
            pageChip.Size = new Size(28, 28);
            pageChip.TabIndex = 1;
            pageChip.TabStop = false;
            pageChip.Text = "1";
            // 
            // prevPageButton
            // 
            prevPageButton.BackColor = Color.White;
            prevPageButton.Enabled = false;
            prevPageButton.Glyph = "";
            prevPageButton.Location = new Point(4, 5);
            prevPageButton.Name = "prevPageButton";
            prevPageButton.Size = new Size(30, 30);
            prevPageButton.TabIndex = 2;
            prevPageButton.Click += prevPageButton_Click;
            // 
            // columnsHead
            // 
            columnsHead.BackColor = Color.FromArgb(246, 248, 251);
            columnsHead.Controls.Add(columnsGrid);
            columnsHead.Dock = DockStyle.Top;
            columnsHead.Location = new Point(16, 108);
            columnsHead.Name = "columnsHead";
            columnsHead.Size = new Size(628, 34);
            columnsHead.TabIndex = 2;
            // 
            // columnsGrid
            // 
            columnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            columnsGrid.ColumnCount = 7;
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            columnsGrid.Controls.Add(headerCheck, 0, 0);
            columnsGrid.Controls.Add(colDate, 1, 0);
            columnsGrid.Controls.Add(colCustomer, 2, 0);
            columnsGrid.Controls.Add(colPet, 3, 0);
            columnsGrid.Controls.Add(colService, 4, 0);
            columnsGrid.Controls.Add(colStatus, 5, 0);
            columnsGrid.Controls.Add(colActions, 6, 0);
            columnsGrid.Dock = DockStyle.Fill;
            columnsGrid.Location = new Point(0, 0);
            columnsGrid.Margin = new Padding(0);
            columnsGrid.Name = "columnsGrid";
            columnsGrid.RowCount = 1;
            columnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            columnsGrid.Size = new Size(628, 34);
            columnsGrid.TabIndex = 0;
            // 
            // headerCheck
            // 
            headerCheck.Anchor = AnchorStyles.None;
            headerCheck.BackColor = Color.FromArgb(246, 248, 251);
            headerCheck.Location = new Point(11, 8);
            headerCheck.Name = "headerCheck";
            headerCheck.Size = new Size(18, 18);
            headerCheck.TabIndex = 0;
            headerCheck.TabStop = false;
            headerCheck.CheckedChanged += headerCheck_CheckedChanged;
            // 
            // colDate
            // 
            colDate.BackColor = Color.FromArgb(246, 248, 251);
            colDate.Dock = DockStyle.Fill;
            colDate.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colDate.ForeColor = Color.FromArgb(68, 83, 106);
            colDate.Location = new Point(40, 0);
            colDate.Margin = new Padding(0);
            colDate.Name = "colDate";
            colDate.Padding = new Padding(4, 0, 0, 0);
            colDate.Size = new Size(88, 34);
            colDate.TabIndex = 1;
            colDate.Text = "Date & Time";
            colDate.TextAlign = ContentAlignment.MiddleLeft;
            colDate.UseMnemonic = false;
            // 
            // colCustomer
            // 
            colCustomer.BackColor = Color.FromArgb(246, 248, 251);
            colCustomer.Dock = DockStyle.Fill;
            colCustomer.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colCustomer.ForeColor = Color.FromArgb(68, 83, 106);
            colCustomer.Location = new Point(128, 0);
            colCustomer.Margin = new Padding(0);
            colCustomer.Name = "colCustomer";
            colCustomer.Padding = new Padding(4, 0, 0, 0);
            colCustomer.Size = new Size(129, 34);
            colCustomer.TabIndex = 2;
            colCustomer.Text = "Customer";
            colCustomer.TextAlign = ContentAlignment.MiddleLeft;
            colCustomer.UseMnemonic = false;
            // 
            // colPet
            // 
            colPet.BackColor = Color.FromArgb(246, 248, 251);
            colPet.Dock = DockStyle.Fill;
            colPet.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colPet.ForeColor = Color.FromArgb(68, 83, 106);
            colPet.Location = new Point(257, 0);
            colPet.Margin = new Padding(0);
            colPet.Name = "colPet";
            colPet.Padding = new Padding(4, 0, 0, 0);
            colPet.Size = new Size(105, 34);
            colPet.TabIndex = 3;
            colPet.Text = "Pet";
            colPet.TextAlign = ContentAlignment.MiddleLeft;
            colPet.UseMnemonic = false;
            // 
            // colService
            // 
            colService.BackColor = Color.FromArgb(246, 248, 251);
            colService.Dock = DockStyle.Fill;
            colService.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colService.ForeColor = Color.FromArgb(68, 83, 106);
            colService.Location = new Point(362, 0);
            colService.Margin = new Padding(0);
            colService.Name = "colService";
            colService.Padding = new Padding(4, 0, 0, 0);
            colService.Size = new Size(82, 34);
            colService.TabIndex = 4;
            colService.Text = "Service";
            colService.TextAlign = ContentAlignment.MiddleLeft;
            colService.UseMnemonic = false;
            // 
            // colStatus
            // 
            colStatus.BackColor = Color.FromArgb(246, 248, 251);
            colStatus.Dock = DockStyle.Fill;
            colStatus.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colStatus.ForeColor = Color.FromArgb(68, 83, 106);
            colStatus.Location = new Point(444, 0);
            colStatus.Margin = new Padding(0);
            colStatus.Name = "colStatus";
            colStatus.Padding = new Padding(4, 0, 0, 0);
            colStatus.Size = new Size(99, 34);
            colStatus.TabIndex = 5;
            colStatus.Text = "Status";
            colStatus.TextAlign = ContentAlignment.MiddleLeft;
            colStatus.UseMnemonic = false;
            // 
            // colActions
            // 
            colActions.BackColor = Color.FromArgb(246, 248, 251);
            colActions.Dock = DockStyle.Fill;
            colActions.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colActions.ForeColor = Color.FromArgb(68, 83, 106);
            colActions.Location = new Point(543, 0);
            colActions.Margin = new Padding(0);
            colActions.Name = "colActions";
            colActions.Padding = new Padding(0, 0, 12, 0);
            colActions.Size = new Size(85, 34);
            colActions.TabIndex = 6;
            colActions.Text = "Actions";
            colActions.TextAlign = ContentAlignment.MiddleRight;
            colActions.UseMnemonic = false;
            // 
            // toolbar
            // 
            toolbar.BackColor = Color.FromArgb(255, 255, 255);
            toolbar.Controls.Add(searchField);
            toolbar.Controls.Add(statusFilter);
            toolbar.Controls.Add(serviceFilter);
            toolbar.Controls.Add(dateRange);
            toolbar.Dock = DockStyle.Top;
            toolbar.Location = new Point(16, 56);
            toolbar.Name = "toolbar";
            toolbar.Size = new Size(628, 52);
            toolbar.TabIndex = 3;
            // 
            // searchField
            // 
            searchField.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchField.BorderColor = Color.FromArgb(227, 233, 242);
            searchField.CornerColor = Color.White;
            searchField.FillColor = Color.White;
            searchField.Glyph = "";
            searchField.Location = new Point(0, 8);
            searchField.Margin = new Padding(0);
            searchField.Name = "searchField";
            searchField.Padding = new Padding(10, 3, 10, 3);
            searchField.Placeholder = "Search customer, pet, or service";
            searchField.Radius = 8;
            searchField.Size = new Size(196, 36);
            searchField.TabIndex = 0;
            // 
            // statusFilter
            // 
            statusFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            statusFilter.BackColor = Color.White;
            statusFilter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusFilter.Location = new Point(204, 8);
            statusFilter.Name = "statusFilter";
            statusFilter.Size = new Size(100, 36);
            statusFilter.TabIndex = 1;
            statusFilter.Text = "All Statuses";
            statusFilter.SelectedIndexChanged += statusFilter_SelectedIndexChanged;
            // 
            // serviceFilter
            // 
            serviceFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            serviceFilter.BackColor = Color.White;
            serviceFilter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            serviceFilter.Location = new Point(312, 8);
            serviceFilter.Name = "serviceFilter";
            serviceFilter.Size = new Size(100, 36);
            serviceFilter.TabIndex = 2;
            serviceFilter.Text = "All Services";
            serviceFilter.SelectedIndexChanged += serviceFilter_SelectedIndexChanged;
            // 
            // dateRange
            // 
            dateRange.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dateRange.BackColor = Color.White;
            dateRange.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateRange.Location = new Point(420, 8);
            dateRange.Name = "dateRange";
            dateRange.Size = new Size(208, 36);
            dateRange.TabIndex = 3;
            dateRange.Text = "This week";
            dateRange.PreviousClicked += dateRange_PreviousClicked;
            dateRange.NextClicked += dateRange_NextClicked;
            dateRange.TextClicked += dateRange_TextClicked;
            // 
            // appointmentsHeader
            // 
            appointmentsHeader.BackColor = Color.FromArgb(255, 255, 255);
            appointmentsHeader.Controls.Add(newAppointmentButton);
            appointmentsHeader.Controls.Add(appointmentsTitle);
            appointmentsHeader.Controls.Add(appointmentsIcon);
            appointmentsHeader.Dock = DockStyle.Top;
            appointmentsHeader.Location = new Point(16, 12);
            appointmentsHeader.Name = "appointmentsHeader";
            appointmentsHeader.Size = new Size(628, 44);
            appointmentsHeader.TabIndex = 4;
            // 
            // newAppointmentButton
            // 
            newAppointmentButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            newAppointmentButton.BackColor = Color.White;
            newAppointmentButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            newAppointmentButton.Location = new Point(478, 5);
            newAppointmentButton.Name = "newAppointmentButton";
            newAppointmentButton.Size = new Size(150, 34);
            newAppointmentButton.TabIndex = 0;
            newAppointmentButton.Text = "+   New Appointment";
            newAppointmentButton.Click += newAppointmentButton_Click;
            // 
            // appointmentsTitle
            // 
            appointmentsTitle.BackColor = Color.FromArgb(255, 255, 255);
            appointmentsTitle.Dock = DockStyle.Left;
            appointmentsTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            appointmentsTitle.ForeColor = Color.FromArgb(31, 42, 60);
            appointmentsTitle.Location = new Point(28, 0);
            appointmentsTitle.Name = "appointmentsTitle";
            appointmentsTitle.Size = new Size(130, 44);
            appointmentsTitle.TabIndex = 1;
            appointmentsTitle.Text = "Appointments";
            appointmentsTitle.TextAlign = ContentAlignment.MiddleLeft;
            appointmentsTitle.UseMnemonic = false;
            // 
            // appointmentsIcon
            // 
            appointmentsIcon.BackColor = Color.FromArgb(255, 255, 255);
            appointmentsIcon.Dock = DockStyle.Left;
            appointmentsIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            appointmentsIcon.ForeColor = Color.FromArgb(58, 123, 213);
            appointmentsIcon.Location = new Point(0, 0);
            appointmentsIcon.Name = "appointmentsIcon";
            appointmentsIcon.Size = new Size(28, 44);
            appointmentsIcon.TabIndex = 2;
            appointmentsIcon.Text = "";
            appointmentsIcon.TextAlign = ContentAlignment.MiddleCenter;
            appointmentsIcon.UseMnemonic = false;
            // 
            // rightColumn
            // 
            rightColumn.BackColor = Color.FromArgb(243, 247, 252);
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightColumn.Controls.Add(calendarCard, 0, 0);
            rightColumn.Controls.Add(quickCard, 0, 1);
            rightColumn.Controls.Add(upcomingCard, 0, 2);
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(676, 0);
            rightColumn.Margin = new Padding(0);
            rightColumn.Name = "rightColumn";
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 304F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 172F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightColumn.Size = new Size(320, 692);
            rightColumn.TabIndex = 1;
            // 
            // calendarCard
            // 
            calendarCard.BorderColor = Color.FromArgb(227, 233, 242);
            calendarCard.Controls.Add(monthCalendar);
            calendarCard.Controls.Add(legendFlow);
            calendarCard.Controls.Add(calendarHeader);
            calendarCard.CornerColor = Color.FromArgb(243, 247, 252);
            calendarCard.Dock = DockStyle.Fill;
            calendarCard.FillColor = Color.White;
            calendarCard.Location = new Point(0, 0);
            calendarCard.Margin = new Padding(0, 0, 0, 12);
            calendarCard.Name = "calendarCard";
            calendarCard.Padding = new Padding(16, 12, 16, 12);
            calendarCard.Radius = 10;
            calendarCard.Size = new Size(320, 292);
            calendarCard.TabIndex = 0;
            // 
            // monthCalendar
            // 
            monthCalendar.BackColor = Color.White;
            monthCalendar.Dock = DockStyle.Fill;
            monthCalendar.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            monthCalendar.Location = new Point(16, 44);
            monthCalendar.Name = "monthCalendar";
            monthCalendar.Size = new Size(288, 208);
            monthCalendar.TabIndex = 0;
            monthCalendar.TabStop = false;
            monthCalendar.DateClicked += monthCalendar_DateClicked;
            // 
            // legendFlow
            // 
            legendFlow.BackColor = Color.White;
            legendFlow.Controls.Add(legendConfirmedDot);
            legendFlow.Controls.Add(legendConfirmedLabel);
            legendFlow.Controls.Add(legendPendingDot);
            legendFlow.Controls.Add(legendPendingLabel);
            legendFlow.Controls.Add(legendRescheduledDot);
            legendFlow.Controls.Add(legendRescheduledLabel);
            legendFlow.Controls.Add(legendCancelledDot);
            legendFlow.Controls.Add(legendCancelledLabel);
            legendFlow.Dock = DockStyle.Bottom;
            legendFlow.Location = new Point(16, 252);
            legendFlow.Name = "legendFlow";
            legendFlow.Padding = new Padding(0, 6, 0, 0);
            legendFlow.Size = new Size(288, 28);
            legendFlow.TabIndex = 1;
            legendFlow.WrapContents = false;
            // 
            // legendConfirmedDot
            // 
            legendConfirmedDot.BackColor = Color.White;
            legendConfirmedDot.DotColor = Color.FromArgb(46, 158, 91);
            legendConfirmedDot.Location = new Point(0, 6);
            legendConfirmedDot.Margin = new Padding(0);
            legendConfirmedDot.Name = "legendConfirmedDot";
            legendConfirmedDot.Size = new Size(14, 20);
            legendConfirmedDot.TabIndex = 0;
            legendConfirmedDot.TabStop = false;
            // 
            // legendConfirmedLabel
            // 
            legendConfirmedLabel.BackColor = Color.FromArgb(255, 255, 255);
            legendConfirmedLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            legendConfirmedLabel.ForeColor = Color.FromArgb(122, 136, 156);
            legendConfirmedLabel.Location = new Point(14, 6);
            legendConfirmedLabel.Margin = new Padding(0, 0, 8, 0);
            legendConfirmedLabel.Name = "legendConfirmedLabel";
            legendConfirmedLabel.Size = new Size(51, 20);
            legendConfirmedLabel.TabIndex = 1;
            legendConfirmedLabel.Text = "Confirmed";
            legendConfirmedLabel.TextAlign = ContentAlignment.MiddleLeft;
            legendConfirmedLabel.UseMnemonic = false;
            // 
            // legendPendingDot
            // 
            legendPendingDot.BackColor = Color.White;
            legendPendingDot.DotColor = Color.FromArgb(245, 158, 11);
            legendPendingDot.Location = new Point(73, 6);
            legendPendingDot.Margin = new Padding(0);
            legendPendingDot.Name = "legendPendingDot";
            legendPendingDot.Size = new Size(14, 20);
            legendPendingDot.TabIndex = 2;
            legendPendingDot.TabStop = false;
            // 
            // legendPendingLabel
            // 
            legendPendingLabel.BackColor = Color.FromArgb(255, 255, 255);
            legendPendingLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            legendPendingLabel.ForeColor = Color.FromArgb(122, 136, 156);
            legendPendingLabel.Location = new Point(87, 6);
            legendPendingLabel.Margin = new Padding(0, 0, 8, 0);
            legendPendingLabel.Name = "legendPendingLabel";
            legendPendingLabel.Size = new Size(42, 23);
            legendPendingLabel.TabIndex = 3;
            legendPendingLabel.Text = "Pending";
            legendPendingLabel.TextAlign = ContentAlignment.MiddleLeft;
            legendPendingLabel.UseMnemonic = false;
            // 
            // legendRescheduledDot
            // 
            legendRescheduledDot.BackColor = Color.White;
            legendRescheduledDot.DotColor = Color.FromArgb(139, 92, 246);
            legendRescheduledDot.Location = new Point(137, 6);
            legendRescheduledDot.Margin = new Padding(0);
            legendRescheduledDot.Name = "legendRescheduledDot";
            legendRescheduledDot.Size = new Size(14, 20);
            legendRescheduledDot.TabIndex = 4;
            legendRescheduledDot.TabStop = false;
            // 
            // legendRescheduledLabel
            // 
            legendRescheduledLabel.BackColor = Color.FromArgb(255, 255, 255);
            legendRescheduledLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            legendRescheduledLabel.ForeColor = Color.FromArgb(122, 136, 156);
            legendRescheduledLabel.Location = new Point(151, 6);
            legendRescheduledLabel.Margin = new Padding(0, 0, 8, 0);
            legendRescheduledLabel.Name = "legendRescheduledLabel";
            legendRescheduledLabel.Size = new Size(62, 20);
            legendRescheduledLabel.TabIndex = 5;
            legendRescheduledLabel.Text = "Rescheduled";
            legendRescheduledLabel.TextAlign = ContentAlignment.MiddleLeft;
            legendRescheduledLabel.UseMnemonic = false;
            // 
            // legendCancelledDot
            // 
            legendCancelledDot.BackColor = Color.White;
            legendCancelledDot.DotColor = Color.FromArgb(239, 68, 68);
            legendCancelledDot.Location = new Point(221, 6);
            legendCancelledDot.Margin = new Padding(0);
            legendCancelledDot.Name = "legendCancelledDot";
            legendCancelledDot.Size = new Size(14, 20);
            legendCancelledDot.TabIndex = 6;
            legendCancelledDot.TabStop = false;
            // 
            // legendCancelledLabel
            // 
            legendCancelledLabel.BackColor = Color.FromArgb(255, 255, 255);
            legendCancelledLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            legendCancelledLabel.ForeColor = Color.FromArgb(122, 136, 156);
            legendCancelledLabel.Location = new Point(235, 6);
            legendCancelledLabel.Margin = new Padding(0, 0, 8, 0);
            legendCancelledLabel.Name = "legendCancelledLabel";
            legendCancelledLabel.Size = new Size(50, 20);
            legendCancelledLabel.TabIndex = 7;
            legendCancelledLabel.Text = "Cancelled";
            legendCancelledLabel.TextAlign = ContentAlignment.MiddleLeft;
            legendCancelledLabel.UseMnemonic = false;
            // 
            // calendarHeader
            // 
            calendarHeader.BackColor = Color.FromArgb(255, 255, 255);
            calendarHeader.Controls.Add(calendarLink);
            calendarHeader.Controls.Add(calendarTitle);
            calendarHeader.Controls.Add(calendarIcon);
            calendarHeader.Dock = DockStyle.Top;
            calendarHeader.Location = new Point(16, 12);
            calendarHeader.Name = "calendarHeader";
            calendarHeader.Size = new Size(288, 32);
            calendarHeader.TabIndex = 2;
            // 
            // calendarLink
            // 
            calendarLink.BackColor = Color.FromArgb(255, 255, 255);
            calendarLink.Cursor = Cursors.Hand;
            calendarLink.Dock = DockStyle.Right;
            calendarLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            calendarLink.ForeColor = Color.FromArgb(58, 123, 213);
            calendarLink.Location = new Point(208, 0);
            calendarLink.Name = "calendarLink";
            calendarLink.Size = new Size(80, 32);
            calendarLink.TabIndex = 0;
            calendarLink.Text = "View Full →";
            calendarLink.TextAlign = ContentAlignment.MiddleRight;
            calendarLink.UseMnemonic = false;
            calendarLink.Click += calendarLink_Click;
            // 
            // calendarTitle
            // 
            calendarTitle.BackColor = Color.FromArgb(255, 255, 255);
            calendarTitle.Dock = DockStyle.Left;
            calendarTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            calendarTitle.ForeColor = Color.FromArgb(31, 42, 60);
            calendarTitle.Location = new Point(28, 0);
            calendarTitle.Name = "calendarTitle";
            calendarTitle.Size = new Size(100, 32);
            calendarTitle.TabIndex = 1;
            calendarTitle.Text = "Calendar";
            calendarTitle.TextAlign = ContentAlignment.MiddleLeft;
            calendarTitle.UseMnemonic = false;
            // 
            // calendarIcon
            // 
            calendarIcon.BackColor = Color.FromArgb(255, 255, 255);
            calendarIcon.Dock = DockStyle.Left;
            calendarIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            calendarIcon.ForeColor = Color.FromArgb(58, 123, 213);
            calendarIcon.Location = new Point(0, 0);
            calendarIcon.Name = "calendarIcon";
            calendarIcon.Size = new Size(28, 32);
            calendarIcon.TabIndex = 2;
            calendarIcon.Text = "";
            calendarIcon.TextAlign = ContentAlignment.MiddleCenter;
            calendarIcon.UseMnemonic = false;
            // 
            // quickCard
            // 
            quickCard.BorderColor = Color.FromArgb(227, 233, 242);
            quickCard.Controls.Add(quickGrid);
            quickCard.Controls.Add(quickHeader);
            quickCard.CornerColor = Color.FromArgb(243, 247, 252);
            quickCard.Dock = DockStyle.Fill;
            quickCard.FillColor = Color.White;
            quickCard.Location = new Point(0, 304);
            quickCard.Margin = new Padding(0, 0, 0, 12);
            quickCard.Name = "quickCard";
            quickCard.Padding = new Padding(16, 12, 16, 12);
            quickCard.Radius = 10;
            quickCard.Size = new Size(320, 160);
            quickCard.TabIndex = 1;
            // 
            // quickGrid
            // 
            quickGrid.BackColor = Color.White;
            quickGrid.ColumnCount = 2;
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.Controls.Add(newAppointmentTile, 0, 0);
            quickGrid.Controls.Add(viewAllTile, 1, 0);
            quickGrid.Controls.Add(manageCustomersTile, 0, 1);
            quickGrid.Controls.Add(servicesCatalogTile, 1, 1);
            quickGrid.Dock = DockStyle.Fill;
            quickGrid.Location = new Point(16, 44);
            quickGrid.Margin = new Padding(0);
            quickGrid.Name = "quickGrid";
            quickGrid.RowCount = 2;
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            quickGrid.Size = new Size(288, 104);
            quickGrid.TabIndex = 0;
            // 
            // newAppointmentTile
            // 
            newAppointmentTile.BackColor = Color.White;
            newAppointmentTile.Dock = DockStyle.Fill;
            newAppointmentTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            newAppointmentTile.Glyph = "";
            newAppointmentTile.Location = new Point(3, 3);
            newAppointmentTile.Name = "newAppointmentTile";
            newAppointmentTile.Size = new Size(138, 46);
            newAppointmentTile.TabIndex = 0;
            newAppointmentTile.Text = "New Appointment";
            newAppointmentTile.Click += newAppointmentTile_Click;
            // 
            // viewAllTile
            // 
            viewAllTile.BackColor = Color.White;
            viewAllTile.Dock = DockStyle.Fill;
            viewAllTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            viewAllTile.Glyph = "";
            viewAllTile.Location = new Point(147, 3);
            viewAllTile.Name = "viewAllTile";
            viewAllTile.Size = new Size(138, 46);
            viewAllTile.TabIndex = 1;
            viewAllTile.Text = "View All Appointments";
            viewAllTile.Click += viewAllTile_Click;
            // 
            // manageCustomersTile
            // 
            manageCustomersTile.BackColor = Color.White;
            manageCustomersTile.Dock = DockStyle.Fill;
            manageCustomersTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            manageCustomersTile.Glyph = "";
            manageCustomersTile.Location = new Point(3, 55);
            manageCustomersTile.Name = "manageCustomersTile";
            manageCustomersTile.Size = new Size(138, 46);
            manageCustomersTile.TabIndex = 2;
            manageCustomersTile.Text = "Manage Customers";
            manageCustomersTile.Click += manageCustomersTile_Click;
            // 
            // servicesCatalogTile
            // 
            servicesCatalogTile.BackColor = Color.White;
            servicesCatalogTile.Dock = DockStyle.Fill;
            servicesCatalogTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            servicesCatalogTile.Glyph = "";
            servicesCatalogTile.Location = new Point(147, 55);
            servicesCatalogTile.Name = "servicesCatalogTile";
            servicesCatalogTile.Size = new Size(138, 46);
            servicesCatalogTile.TabIndex = 3;
            servicesCatalogTile.Text = "Services Catalog";
            servicesCatalogTile.Click += servicesCatalogTile_Click;
            // 
            // quickHeader
            // 
            quickHeader.BackColor = Color.FromArgb(255, 255, 255);
            quickHeader.Controls.Add(quickTitle);
            quickHeader.Controls.Add(quickIcon);
            quickHeader.Dock = DockStyle.Top;
            quickHeader.Location = new Point(16, 12);
            quickHeader.Name = "quickHeader";
            quickHeader.Size = new Size(288, 32);
            quickHeader.TabIndex = 1;
            // 
            // quickTitle
            // 
            quickTitle.BackColor = Color.FromArgb(255, 255, 255);
            quickTitle.Dock = DockStyle.Left;
            quickTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickTitle.ForeColor = Color.FromArgb(31, 42, 60);
            quickTitle.Location = new Point(28, 0);
            quickTitle.Name = "quickTitle";
            quickTitle.Size = new Size(160, 32);
            quickTitle.TabIndex = 0;
            quickTitle.Text = "Quick Actions";
            quickTitle.TextAlign = ContentAlignment.MiddleLeft;
            quickTitle.UseMnemonic = false;
            // 
            // quickIcon
            // 
            quickIcon.BackColor = Color.FromArgb(255, 255, 255);
            quickIcon.Dock = DockStyle.Left;
            quickIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            quickIcon.ForeColor = Color.FromArgb(58, 123, 213);
            quickIcon.Location = new Point(0, 0);
            quickIcon.Name = "quickIcon";
            quickIcon.Size = new Size(28, 32);
            quickIcon.TabIndex = 1;
            quickIcon.Text = "";
            quickIcon.TextAlign = ContentAlignment.MiddleCenter;
            quickIcon.UseMnemonic = false;
            // 
            // upcomingCard
            // 
            upcomingCard.BorderColor = Color.FromArgb(227, 233, 242);
            upcomingCard.Controls.Add(upcomingList);
            upcomingCard.Controls.Add(upcomingEmpty);
            upcomingCard.Controls.Add(upcomingHeader);
            upcomingCard.CornerColor = Color.FromArgb(243, 247, 252);
            upcomingCard.Dock = DockStyle.Fill;
            upcomingCard.FillColor = Color.White;
            upcomingCard.Location = new Point(0, 476);
            upcomingCard.Margin = new Padding(0);
            upcomingCard.Name = "upcomingCard";
            upcomingCard.Padding = new Padding(16, 12, 16, 12);
            upcomingCard.Radius = 10;
            upcomingCard.Size = new Size(320, 216);
            upcomingCard.TabIndex = 2;
            // 
            // upcomingList
            // 
            upcomingList.AutoScroll = true;
            upcomingList.BackColor = Color.White;
            upcomingList.Dock = DockStyle.Fill;
            upcomingList.Location = new Point(16, 44);
            upcomingList.Name = "upcomingList";
            upcomingList.Size = new Size(288, 160);
            upcomingList.TabIndex = 0;
            upcomingList.Visible = false;
            // 
            // upcomingEmpty
            // 
            upcomingEmpty.BackColor = Color.White;
            upcomingEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            upcomingEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            upcomingEmpty.Dock = DockStyle.Fill;
            upcomingEmpty.Glyph = "";
            upcomingEmpty.Hint = "Today's appointments will show up here.";
            upcomingEmpty.Location = new Point(16, 44);
            upcomingEmpty.Name = "upcomingEmpty";
            upcomingEmpty.Size = new Size(288, 160);
            upcomingEmpty.TabIndex = 1;
            upcomingEmpty.TabStop = false;
            upcomingEmpty.Title = "Nothing scheduled today";
            // 
            // upcomingHeader
            // 
            upcomingHeader.BackColor = Color.FromArgb(255, 255, 255);
            upcomingHeader.Controls.Add(upcomingLink);
            upcomingHeader.Controls.Add(upcomingTitle);
            upcomingHeader.Controls.Add(upcomingIcon);
            upcomingHeader.Dock = DockStyle.Top;
            upcomingHeader.Location = new Point(16, 12);
            upcomingHeader.Name = "upcomingHeader";
            upcomingHeader.Size = new Size(288, 32);
            upcomingHeader.TabIndex = 2;
            // 
            // upcomingLink
            // 
            upcomingLink.BackColor = Color.FromArgb(255, 255, 255);
            upcomingLink.Cursor = Cursors.Hand;
            upcomingLink.Dock = DockStyle.Right;
            upcomingLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            upcomingLink.ForeColor = Color.FromArgb(58, 123, 213);
            upcomingLink.Location = new Point(218, 0);
            upcomingLink.Name = "upcomingLink";
            upcomingLink.Size = new Size(70, 32);
            upcomingLink.TabIndex = 0;
            upcomingLink.Text = "View All →";
            upcomingLink.TextAlign = ContentAlignment.MiddleRight;
            upcomingLink.UseMnemonic = false;
            upcomingLink.Click += upcomingLink_Click;
            // 
            // upcomingTitle
            // 
            upcomingTitle.BackColor = Color.FromArgb(255, 255, 255);
            upcomingTitle.Dock = DockStyle.Left;
            upcomingTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            upcomingTitle.ForeColor = Color.FromArgb(31, 42, 60);
            upcomingTitle.Location = new Point(28, 0);
            upcomingTitle.Name = "upcomingTitle";
            upcomingTitle.Size = new Size(150, 32);
            upcomingTitle.TabIndex = 1;
            upcomingTitle.Text = "Upcoming Today";
            upcomingTitle.TextAlign = ContentAlignment.MiddleLeft;
            upcomingTitle.UseMnemonic = false;
            // 
            // upcomingIcon
            // 
            upcomingIcon.BackColor = Color.FromArgb(255, 255, 255);
            upcomingIcon.Dock = DockStyle.Left;
            upcomingIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            upcomingIcon.ForeColor = Color.FromArgb(58, 123, 213);
            upcomingIcon.Location = new Point(0, 0);
            upcomingIcon.Name = "upcomingIcon";
            upcomingIcon.Size = new Size(28, 32);
            upcomingIcon.TabIndex = 2;
            upcomingIcon.Text = "";
            upcomingIcon.TextAlign = ContentAlignment.MiddleCenter;
            upcomingIcon.UseMnemonic = false;
            // 
            // AppointmentsPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            Controls.Add(bodyGrid);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "AppointmentsPage";
            Padding = new Padding(20);
            Size = new Size(1036, 732);
            bodyGrid.ResumeLayout(false);
            leftColumn.ResumeLayout(false);
            statsRow.ResumeLayout(false);
            totalCard.ResumeLayout(false);
            pendingCard.ResumeLayout(false);
            confirmedCard.ResumeLayout(false);
            cancelledCard.ResumeLayout(false);
            appointmentsCard.ResumeLayout(false);
            rowsHost.ResumeLayout(false);
            tableFooter.ResumeLayout(false);
            pager.ResumeLayout(false);
            columnsHead.ResumeLayout(false);
            columnsGrid.ResumeLayout(false);
            toolbar.ResumeLayout(false);
            appointmentsHeader.ResumeLayout(false);
            rightColumn.ResumeLayout(false);
            calendarCard.ResumeLayout(false);
            legendFlow.ResumeLayout(false);
            calendarHeader.ResumeLayout(false);
            quickCard.ResumeLayout(false);
            quickGrid.ResumeLayout(false);
            quickHeader.ResumeLayout(false);
            upcomingCard.ResumeLayout(false);
            upcomingHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel bodyGrid;
        private System.Windows.Forms.TableLayoutPanel leftColumn;
        private System.Windows.Forms.TableLayoutPanel statsRow;
        private MyApp.Controls.CardPanel totalCard;
        private MyApp.Controls.IconBadge totalIcon;
        private System.Windows.Forms.Label totalDelta;
        private System.Windows.Forms.Label totalValue;
        private System.Windows.Forms.Label totalLabel;
        private MyApp.Controls.CardPanel pendingCard;
        private MyApp.Controls.IconBadge pendingIcon;
        private System.Windows.Forms.Label pendingDelta;
        private System.Windows.Forms.Label pendingValue;
        private System.Windows.Forms.Label pendingLabel;
        private MyApp.Controls.CardPanel confirmedCard;
        private MyApp.Controls.IconBadge confirmedIcon;
        private System.Windows.Forms.Label confirmedDelta;
        private System.Windows.Forms.Label confirmedValue;
        private System.Windows.Forms.Label confirmedLabel;
        private MyApp.Controls.CardPanel cancelledCard;
        private MyApp.Controls.IconBadge cancelledIcon;
        private System.Windows.Forms.Label cancelledDelta;
        private System.Windows.Forms.Label cancelledValue;
        private System.Windows.Forms.Label cancelledLabel;
        private MyApp.Controls.CardPanel appointmentsCard;
        private System.Windows.Forms.Panel rowsHost;
        private MyApp.Controls.EmptyState appointmentsEmpty;
        private System.Windows.Forms.Panel tableFooter;
        private System.Windows.Forms.Label showingLabel;
        private System.Windows.Forms.Panel pager;
        private MyApp.Controls.IconButton nextPageButton;
        private MyApp.Controls.PrimaryButton pageChip;
        private MyApp.Controls.IconButton prevPageButton;
        private System.Windows.Forms.Panel columnsHead;
        private System.Windows.Forms.TableLayoutPanel columnsGrid;
        private MyApp.Controls.CheckMark headerCheck;
        private System.Windows.Forms.Label colDate;
        private System.Windows.Forms.Label colCustomer;
        private System.Windows.Forms.Label colPet;
        private System.Windows.Forms.Label colService;
        private System.Windows.Forms.Label colStatus;
        private System.Windows.Forms.Label colActions;
        private System.Windows.Forms.Panel toolbar;
        private MyApp.Controls.InputField searchField;
        private MyApp.Controls.SelectField statusFilter;
        private MyApp.Controls.SelectField serviceFilter;
        private MyApp.Controls.DateRangeField dateRange;
        private System.Windows.Forms.Panel appointmentsHeader;
        private MyApp.Controls.PrimaryButton newAppointmentButton;
        private System.Windows.Forms.Label appointmentsTitle;
        private System.Windows.Forms.Label appointmentsIcon;
        private System.Windows.Forms.TableLayoutPanel rightColumn;
        private MyApp.Controls.CardPanel calendarCard;
        private MyApp.Controls.MonthCalendarView monthCalendar;
        private System.Windows.Forms.FlowLayoutPanel legendFlow;
        private MyApp.Controls.Dot legendConfirmedDot;
        private System.Windows.Forms.Label legendConfirmedLabel;
        private MyApp.Controls.Dot legendPendingDot;
        private System.Windows.Forms.Label legendPendingLabel;
        private MyApp.Controls.Dot legendRescheduledDot;
        private System.Windows.Forms.Label legendRescheduledLabel;
        private MyApp.Controls.Dot legendCancelledDot;
        private System.Windows.Forms.Label legendCancelledLabel;
        private System.Windows.Forms.Panel calendarHeader;
        private System.Windows.Forms.Label calendarLink;
        private System.Windows.Forms.Label calendarTitle;
        private System.Windows.Forms.Label calendarIcon;
        private MyApp.Controls.CardPanel quickCard;
        private System.Windows.Forms.TableLayoutPanel quickGrid;
        private MyApp.Controls.ActionTile newAppointmentTile;
        private MyApp.Controls.ActionTile viewAllTile;
        private MyApp.Controls.ActionTile manageCustomersTile;
        private MyApp.Controls.ActionTile servicesCatalogTile;
        private System.Windows.Forms.Panel quickHeader;
        private System.Windows.Forms.Label quickTitle;
        private System.Windows.Forms.Label quickIcon;
        private MyApp.Controls.CardPanel upcomingCard;
        private System.Windows.Forms.Panel upcomingList;
        private MyApp.Controls.EmptyState upcomingEmpty;
        private System.Windows.Forms.Panel upcomingHeader;
        private System.Windows.Forms.Label upcomingLink;
        private System.Windows.Forms.Label upcomingTitle;
        private System.Windows.Forms.Label upcomingIcon;
    }
}
