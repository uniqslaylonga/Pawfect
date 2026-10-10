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
            upcomingList.SuspendLayout();
            upcomingHeader.SuspendLayout();
            SuspendLayout();
            // 
            // bodyGrid
            // 
            bodyGrid.Controls.Add(leftColumn, 0, 0);
            bodyGrid.Controls.Add(rightColumn, 1, 0);
            bodyGrid.BackColor = Color.FromArgb(243, 247, 252);
            bodyGrid.ColumnCount = 2;
            bodyGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bodyGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            bodyGrid.Dock = DockStyle.Fill;
            bodyGrid.Location = new Point(20, 20);
            bodyGrid.Margin = new Padding(0);
            bodyGrid.RowCount = 1;
            bodyGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bodyGrid.Size = new Size(996, 692);
            bodyGrid.Name = "bodyGrid";
            // 
            // leftColumn
            // 
            leftColumn.Controls.Add(statsRow, 0, 0);
            leftColumn.Controls.Add(appointmentsCard, 0, 1);
            leftColumn.BackColor = Color.FromArgb(243, 247, 252);
            leftColumn.ColumnCount = 1;
            leftColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            leftColumn.Dock = DockStyle.Fill;
            leftColumn.Location = new Point(0, 0);
            leftColumn.Margin = new Padding(0, 0, 16, 0);
            leftColumn.RowCount = 2;
            leftColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 124F));
            leftColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            leftColumn.Size = new Size(660, 692);
            leftColumn.Name = "leftColumn";
            leftColumn.TabIndex = 0;
            // 
            // statsRow
            // 
            statsRow.Controls.Add(totalCard, 0, 0);
            statsRow.Controls.Add(pendingCard, 1, 0);
            statsRow.Controls.Add(confirmedCard, 2, 0);
            statsRow.Controls.Add(cancelledCard, 3, 0);
            statsRow.BackColor = Color.FromArgb(243, 247, 252);
            statsRow.ColumnCount = 4;
            statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsRow.Dock = DockStyle.Fill;
            statsRow.Location = new Point(0, 0);
            statsRow.Margin = new Padding(0, 0, 0, 16);
            statsRow.RowCount = 1;
            statsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statsRow.Size = new Size(660, 108);
            statsRow.Name = "statsRow";
            statsRow.TabIndex = 0;
            // 
            // totalCard
            // 
            totalCard.Controls.Add(totalIcon);
            totalCard.Controls.Add(totalDelta);
            totalCard.Controls.Add(totalValue);
            totalCard.Controls.Add(totalLabel);
            totalCard.BorderColor = Color.FromArgb(227, 233, 242);
            totalCard.CornerColor = Color.FromArgb(243, 247, 252);
            totalCard.Dock = DockStyle.Fill;
            totalCard.FillColor = Color.White;
            totalCard.Location = new Point(0, 0);
            totalCard.Margin = new Padding(0, 0, 12, 0);
            totalCard.Padding = new Padding(14, 12, 14, 8);
            totalCard.Radius = 10;
            totalCard.Size = new Size(156, 108);
            totalCard.Name = "totalCard";
            totalCard.TabIndex = 0;
            // 
            // totalIcon
            // 
            totalIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            totalIcon.BackColor = Color.White;
            totalIcon.FillColor = Color.FromArgb(234, 242, 253);
            totalIcon.ForeColor = Color.FromArgb(58, 123, 213);
            totalIcon.Glyph = "\uE787";
            totalIcon.GlyphSize = 9.5F;
            totalIcon.Location = new Point(114, 42);
            totalIcon.Name = "totalIcon";
            totalIcon.Radius = 8;
            totalIcon.Size = new Size(28, 28);
            totalIcon.TabStop = false;
            totalIcon.TabIndex = 0;
            // 
            // totalDelta
            // 
            totalDelta.BackColor = Color.FromArgb(255, 255, 255);
            totalDelta.Dock = DockStyle.Top;
            totalDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalDelta.ForeColor = Color.FromArgb(122, 136, 156);
            totalDelta.Location = new Point(14, 70);
            totalDelta.Size = new Size(128, 16);
            totalDelta.Text = "0 today";
            totalDelta.TextAlign = ContentAlignment.MiddleLeft;
            totalDelta.UseMnemonic = false;
            totalDelta.Name = "totalDelta";
            totalDelta.TabIndex = 1;
            // 
            // totalValue
            // 
            totalValue.BackColor = Color.FromArgb(255, 255, 255);
            totalValue.Dock = DockStyle.Top;
            totalValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalValue.ForeColor = Color.FromArgb(31, 42, 60);
            totalValue.Location = new Point(14, 40);
            totalValue.Size = new Size(128, 30);
            totalValue.Text = "0";
            totalValue.TextAlign = ContentAlignment.MiddleLeft;
            totalValue.UseMnemonic = false;
            totalValue.Name = "totalValue";
            totalValue.TabIndex = 2;
            // 
            // totalLabel
            // 
            totalLabel.BackColor = Color.FromArgb(255, 255, 255);
            totalLabel.Dock = DockStyle.Top;
            totalLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalLabel.ForeColor = Color.FromArgb(68, 83, 106);
            totalLabel.Location = new Point(14, 12);
            totalLabel.Size = new Size(128, 18);
            totalLabel.Text = "Total Appointments";
            totalLabel.TextAlign = ContentAlignment.MiddleLeft;
            totalLabel.UseMnemonic = false;
            totalLabel.Name = "totalLabel";
            totalLabel.TabIndex = 3;
            // 
            // pendingCard
            // 
            pendingCard.Controls.Add(pendingIcon);
            pendingCard.Controls.Add(pendingDelta);
            pendingCard.Controls.Add(pendingValue);
            pendingCard.Controls.Add(pendingLabel);
            pendingCard.BorderColor = Color.FromArgb(227, 233, 242);
            pendingCard.CornerColor = Color.FromArgb(243, 247, 252);
            pendingCard.Dock = DockStyle.Fill;
            pendingCard.FillColor = Color.White;
            pendingCard.Location = new Point(0, 0);
            pendingCard.Margin = new Padding(0, 0, 12, 0);
            pendingCard.Padding = new Padding(14, 12, 14, 8);
            pendingCard.Radius = 10;
            pendingCard.Size = new Size(156, 108);
            pendingCard.Name = "pendingCard";
            pendingCard.TabIndex = 1;
            // 
            // pendingIcon
            // 
            pendingIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pendingIcon.BackColor = Color.White;
            pendingIcon.FillColor = Color.FromArgb(254, 243, 224);
            pendingIcon.ForeColor = Color.FromArgb(245, 158, 11);
            pendingIcon.Glyph = "\uE823";
            pendingIcon.GlyphSize = 9.5F;
            pendingIcon.Location = new Point(114, 42);
            pendingIcon.Name = "pendingIcon";
            pendingIcon.Radius = 8;
            pendingIcon.Size = new Size(28, 28);
            pendingIcon.TabStop = false;
            pendingIcon.TabIndex = 0;
            // 
            // pendingDelta
            // 
            pendingDelta.BackColor = Color.FromArgb(255, 255, 255);
            pendingDelta.Dock = DockStyle.Top;
            pendingDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pendingDelta.ForeColor = Color.FromArgb(122, 136, 156);
            pendingDelta.Location = new Point(14, 70);
            pendingDelta.Size = new Size(128, 16);
            pendingDelta.Text = "0 today";
            pendingDelta.TextAlign = ContentAlignment.MiddleLeft;
            pendingDelta.UseMnemonic = false;
            pendingDelta.Name = "pendingDelta";
            pendingDelta.TabIndex = 1;
            // 
            // pendingValue
            // 
            pendingValue.BackColor = Color.FromArgb(255, 255, 255);
            pendingValue.Dock = DockStyle.Top;
            pendingValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pendingValue.ForeColor = Color.FromArgb(31, 42, 60);
            pendingValue.Location = new Point(14, 40);
            pendingValue.Size = new Size(128, 30);
            pendingValue.Text = "0";
            pendingValue.TextAlign = ContentAlignment.MiddleLeft;
            pendingValue.UseMnemonic = false;
            pendingValue.Name = "pendingValue";
            pendingValue.TabIndex = 2;
            // 
            // pendingLabel
            // 
            pendingLabel.BackColor = Color.FromArgb(255, 255, 255);
            pendingLabel.Dock = DockStyle.Top;
            pendingLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pendingLabel.ForeColor = Color.FromArgb(68, 83, 106);
            pendingLabel.Location = new Point(14, 12);
            pendingLabel.Size = new Size(128, 18);
            pendingLabel.Text = "Pending";
            pendingLabel.TextAlign = ContentAlignment.MiddleLeft;
            pendingLabel.UseMnemonic = false;
            pendingLabel.Name = "pendingLabel";
            pendingLabel.TabIndex = 3;
            // 
            // confirmedCard
            // 
            confirmedCard.Controls.Add(confirmedIcon);
            confirmedCard.Controls.Add(confirmedDelta);
            confirmedCard.Controls.Add(confirmedValue);
            confirmedCard.Controls.Add(confirmedLabel);
            confirmedCard.BorderColor = Color.FromArgb(227, 233, 242);
            confirmedCard.CornerColor = Color.FromArgb(243, 247, 252);
            confirmedCard.Dock = DockStyle.Fill;
            confirmedCard.FillColor = Color.White;
            confirmedCard.Location = new Point(0, 0);
            confirmedCard.Margin = new Padding(0, 0, 12, 0);
            confirmedCard.Padding = new Padding(14, 12, 14, 8);
            confirmedCard.Radius = 10;
            confirmedCard.Size = new Size(156, 108);
            confirmedCard.Name = "confirmedCard";
            confirmedCard.TabIndex = 2;
            // 
            // confirmedIcon
            // 
            confirmedIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            confirmedIcon.BackColor = Color.White;
            confirmedIcon.FillColor = Color.FromArgb(229, 246, 236);
            confirmedIcon.ForeColor = Color.FromArgb(46, 158, 91);
            confirmedIcon.Glyph = "\uE73E";
            confirmedIcon.GlyphSize = 9.5F;
            confirmedIcon.Location = new Point(114, 42);
            confirmedIcon.Name = "confirmedIcon";
            confirmedIcon.Radius = 8;
            confirmedIcon.Size = new Size(28, 28);
            confirmedIcon.TabStop = false;
            confirmedIcon.TabIndex = 0;
            // 
            // confirmedDelta
            // 
            confirmedDelta.BackColor = Color.FromArgb(255, 255, 255);
            confirmedDelta.Dock = DockStyle.Top;
            confirmedDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            confirmedDelta.ForeColor = Color.FromArgb(122, 136, 156);
            confirmedDelta.Location = new Point(14, 70);
            confirmedDelta.Size = new Size(128, 16);
            confirmedDelta.Text = "0 today";
            confirmedDelta.TextAlign = ContentAlignment.MiddleLeft;
            confirmedDelta.UseMnemonic = false;
            confirmedDelta.Name = "confirmedDelta";
            confirmedDelta.TabIndex = 1;
            // 
            // confirmedValue
            // 
            confirmedValue.BackColor = Color.FromArgb(255, 255, 255);
            confirmedValue.Dock = DockStyle.Top;
            confirmedValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            confirmedValue.ForeColor = Color.FromArgb(31, 42, 60);
            confirmedValue.Location = new Point(14, 40);
            confirmedValue.Size = new Size(128, 30);
            confirmedValue.Text = "0";
            confirmedValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmedValue.UseMnemonic = false;
            confirmedValue.Name = "confirmedValue";
            confirmedValue.TabIndex = 2;
            // 
            // confirmedLabel
            // 
            confirmedLabel.BackColor = Color.FromArgb(255, 255, 255);
            confirmedLabel.Dock = DockStyle.Top;
            confirmedLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            confirmedLabel.ForeColor = Color.FromArgb(68, 83, 106);
            confirmedLabel.Location = new Point(14, 12);
            confirmedLabel.Size = new Size(128, 18);
            confirmedLabel.Text = "Confirmed";
            confirmedLabel.TextAlign = ContentAlignment.MiddleLeft;
            confirmedLabel.UseMnemonic = false;
            confirmedLabel.Name = "confirmedLabel";
            confirmedLabel.TabIndex = 3;
            // 
            // cancelledCard
            // 
            cancelledCard.Controls.Add(cancelledIcon);
            cancelledCard.Controls.Add(cancelledDelta);
            cancelledCard.Controls.Add(cancelledValue);
            cancelledCard.Controls.Add(cancelledLabel);
            cancelledCard.BorderColor = Color.FromArgb(227, 233, 242);
            cancelledCard.CornerColor = Color.FromArgb(243, 247, 252);
            cancelledCard.Dock = DockStyle.Fill;
            cancelledCard.FillColor = Color.White;
            cancelledCard.Location = new Point(0, 0);
            cancelledCard.Margin = new Padding(0);
            cancelledCard.Padding = new Padding(14, 12, 14, 8);
            cancelledCard.Radius = 10;
            cancelledCard.Size = new Size(156, 108);
            cancelledCard.Name = "cancelledCard";
            cancelledCard.TabIndex = 3;
            // 
            // cancelledIcon
            // 
            cancelledIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cancelledIcon.BackColor = Color.White;
            cancelledIcon.FillColor = Color.FromArgb(253, 232, 232);
            cancelledIcon.ForeColor = Color.FromArgb(239, 68, 68);
            cancelledIcon.Glyph = "\uE711";
            cancelledIcon.GlyphSize = 9.5F;
            cancelledIcon.Location = new Point(114, 42);
            cancelledIcon.Name = "cancelledIcon";
            cancelledIcon.Radius = 8;
            cancelledIcon.Size = new Size(28, 28);
            cancelledIcon.TabStop = false;
            cancelledIcon.TabIndex = 0;
            // 
            // cancelledDelta
            // 
            cancelledDelta.BackColor = Color.FromArgb(255, 255, 255);
            cancelledDelta.Dock = DockStyle.Top;
            cancelledDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cancelledDelta.ForeColor = Color.FromArgb(122, 136, 156);
            cancelledDelta.Location = new Point(14, 70);
            cancelledDelta.Size = new Size(128, 16);
            cancelledDelta.Text = "0 today";
            cancelledDelta.TextAlign = ContentAlignment.MiddleLeft;
            cancelledDelta.UseMnemonic = false;
            cancelledDelta.Name = "cancelledDelta";
            cancelledDelta.TabIndex = 1;
            // 
            // cancelledValue
            // 
            cancelledValue.BackColor = Color.FromArgb(255, 255, 255);
            cancelledValue.Dock = DockStyle.Top;
            cancelledValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cancelledValue.ForeColor = Color.FromArgb(31, 42, 60);
            cancelledValue.Location = new Point(14, 40);
            cancelledValue.Size = new Size(128, 30);
            cancelledValue.Text = "0";
            cancelledValue.TextAlign = ContentAlignment.MiddleLeft;
            cancelledValue.UseMnemonic = false;
            cancelledValue.Name = "cancelledValue";
            cancelledValue.TabIndex = 2;
            // 
            // cancelledLabel
            // 
            cancelledLabel.BackColor = Color.FromArgb(255, 255, 255);
            cancelledLabel.Dock = DockStyle.Top;
            cancelledLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cancelledLabel.ForeColor = Color.FromArgb(68, 83, 106);
            cancelledLabel.Location = new Point(14, 12);
            cancelledLabel.Size = new Size(128, 18);
            cancelledLabel.Text = "Cancelled";
            cancelledLabel.TextAlign = ContentAlignment.MiddleLeft;
            cancelledLabel.UseMnemonic = false;
            cancelledLabel.Name = "cancelledLabel";
            cancelledLabel.TabIndex = 3;
            // 
            // appointmentsCard
            // 
            appointmentsCard.Controls.Add(rowsHost);
            appointmentsCard.Controls.Add(tableFooter);
            appointmentsCard.Controls.Add(columnsHead);
            appointmentsCard.Controls.Add(toolbar);
            appointmentsCard.Controls.Add(appointmentsHeader);
            appointmentsCard.BorderColor = Color.FromArgb(227, 233, 242);
            appointmentsCard.CornerColor = Color.FromArgb(243, 247, 252);
            appointmentsCard.Dock = DockStyle.Fill;
            appointmentsCard.FillColor = Color.White;
            appointmentsCard.Location = new Point(0, 0);
            appointmentsCard.Margin = new Padding(0);
            appointmentsCard.Padding = new Padding(16, 12, 16, 12);
            appointmentsCard.Radius = 10;
            appointmentsCard.Size = new Size(660, 568);
            appointmentsCard.Name = "appointmentsCard";
            appointmentsCard.TabIndex = 1;
            // 
            // rowsHost
            // 
            rowsHost.Controls.Add(appointmentsEmpty);
            rowsHost.AutoScroll = true;
            rowsHost.BackColor = Color.White;
            rowsHost.Dock = DockStyle.Fill;
            rowsHost.Location = new Point(16, 142);
            rowsHost.Size = new Size(628, 374);
            rowsHost.Name = "rowsHost";
            rowsHost.TabIndex = 0;
            // 
            // appointmentsEmpty
            // 
            appointmentsEmpty.BackColor = Color.White;
            appointmentsEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            appointmentsEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            appointmentsEmpty.Dock = DockStyle.Fill;
            appointmentsEmpty.Glyph = "\uE787";
            appointmentsEmpty.Hint = "Appointments will show up here once they are booked.";
            appointmentsEmpty.Location = new Point(0, 0);
            appointmentsEmpty.Size = new Size(628, 374);
            appointmentsEmpty.TabStop = false;
            appointmentsEmpty.Title = "No appointments found";
            appointmentsEmpty.Name = "appointmentsEmpty";
            appointmentsEmpty.TabIndex = 0;
            // 
            // tableFooter
            // 
            tableFooter.Controls.Add(showingLabel);
            tableFooter.Controls.Add(pager);
            tableFooter.BackColor = Color.FromArgb(255, 255, 255);
            tableFooter.Dock = DockStyle.Bottom;
            tableFooter.Location = new Point(0, 0);
            tableFooter.Size = new Size(628, 40);
            tableFooter.Name = "tableFooter";
            tableFooter.TabIndex = 1;
            // 
            // showingLabel
            // 
            showingLabel.BackColor = Color.FromArgb(255, 255, 255);
            showingLabel.Dock = DockStyle.Fill;
            showingLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            showingLabel.ForeColor = Color.FromArgb(122, 136, 156);
            showingLabel.Location = new Point(0, 0);
            showingLabel.Size = new Size(500, 40);
            showingLabel.Text = "Showing 0 appointments";
            showingLabel.TextAlign = ContentAlignment.MiddleLeft;
            showingLabel.UseMnemonic = false;
            showingLabel.Name = "showingLabel";
            showingLabel.TabIndex = 0;
            // 
            // pager
            // 
            pager.Controls.Add(nextPageButton);
            pager.Controls.Add(pageChip);
            pager.Controls.Add(prevPageButton);
            pager.BackColor = Color.FromArgb(255, 255, 255);
            pager.Dock = DockStyle.Right;
            pager.Location = new Point(0, 0);
            pager.Size = new Size(112, 40);
            pager.Name = "pager";
            pager.TabIndex = 1;
            // 
            // nextPageButton
            // 
            nextPageButton.BackColor = Color.White;
            nextPageButton.Enabled = false;
            nextPageButton.Glyph = "\uE76C";
            nextPageButton.Location = new Point(78, 5);
            nextPageButton.Size = new Size(30, 30);
            nextPageButton.Name = "nextPageButton";
            nextPageButton.TabIndex = 0;
            nextPageButton.Click += nextPageButton_Click;
            // 
            // pageChip
            // 
            pageChip.BackColor = Color.White;
            pageChip.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pageChip.Location = new Point(42, 6);
            pageChip.Size = new Size(28, 28);
            pageChip.TabStop = false;
            pageChip.Text = "1";
            pageChip.Name = "pageChip";
            pageChip.TabIndex = 1;
            // 
            // prevPageButton
            // 
            prevPageButton.BackColor = Color.White;
            prevPageButton.Enabled = false;
            prevPageButton.Glyph = "\uE76B";
            prevPageButton.Location = new Point(4, 5);
            prevPageButton.Size = new Size(30, 30);
            prevPageButton.Name = "prevPageButton";
            prevPageButton.TabIndex = 2;
            prevPageButton.Click += prevPageButton_Click;
            // 
            // columnsHead
            // 
            columnsHead.Controls.Add(columnsGrid);
            columnsHead.BackColor = Color.FromArgb(246, 248, 251);
            columnsHead.Dock = DockStyle.Top;
            columnsHead.Location = new Point(0, 0);
            columnsHead.Size = new Size(628, 34);
            columnsHead.Name = "columnsHead";
            columnsHead.TabIndex = 2;
            // 
            // columnsGrid
            // 
            columnsGrid.Controls.Add(headerCheck, 0, 0);
            columnsGrid.Controls.Add(colDate, 1, 0);
            columnsGrid.Controls.Add(colCustomer, 2, 0);
            columnsGrid.Controls.Add(colPet, 3, 0);
            columnsGrid.Controls.Add(colService, 4, 0);
            columnsGrid.Controls.Add(colStatus, 5, 0);
            columnsGrid.Controls.Add(colActions, 6, 0);
            columnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            columnsGrid.ColumnCount = 7;
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            columnsGrid.Dock = DockStyle.Fill;
            columnsGrid.Location = new Point(0, 0);
            columnsGrid.Margin = new Padding(0);
            columnsGrid.RowCount = 1;
            columnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            columnsGrid.Size = new Size(628, 34);
            columnsGrid.Name = "columnsGrid";
            columnsGrid.TabIndex = 0;
            // 
            // headerCheck
            // 
            headerCheck.Anchor = AnchorStyles.None;
            headerCheck.BackColor = Color.FromArgb(246, 248, 251);
            headerCheck.Location = new Point(11, 8);
            headerCheck.Size = new Size(18, 18);
            headerCheck.Name = "headerCheck";
            headerCheck.TabIndex = 0;
            headerCheck.CheckedChanged += headerCheck_CheckedChanged;
            // 
            // colDate
            // 
            colDate.BackColor = Color.FromArgb(246, 248, 251);
            colDate.Dock = DockStyle.Fill;
            colDate.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colDate.ForeColor = Color.FromArgb(68, 83, 106);
            colDate.Padding = new Padding(4, 0, 0, 0);
            colDate.Margin = new Padding(0);
            colDate.Size = new Size(100, 34);
            colDate.Text = "Date & Time";
            colDate.TextAlign = ContentAlignment.MiddleLeft;
            colDate.UseMnemonic = false;
            colDate.Name = "colDate";
            colDate.TabIndex = 1;
            // 
            // colCustomer
            // 
            colCustomer.BackColor = Color.FromArgb(246, 248, 251);
            colCustomer.Dock = DockStyle.Fill;
            colCustomer.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colCustomer.ForeColor = Color.FromArgb(68, 83, 106);
            colCustomer.Padding = new Padding(4, 0, 0, 0);
            colCustomer.Margin = new Padding(0);
            colCustomer.Size = new Size(100, 34);
            colCustomer.Text = "Customer";
            colCustomer.TextAlign = ContentAlignment.MiddleLeft;
            colCustomer.UseMnemonic = false;
            colCustomer.Name = "colCustomer";
            colCustomer.TabIndex = 2;
            // 
            // colPet
            // 
            colPet.BackColor = Color.FromArgb(246, 248, 251);
            colPet.Dock = DockStyle.Fill;
            colPet.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colPet.ForeColor = Color.FromArgb(68, 83, 106);
            colPet.Padding = new Padding(4, 0, 0, 0);
            colPet.Margin = new Padding(0);
            colPet.Size = new Size(100, 34);
            colPet.Text = "Pet";
            colPet.TextAlign = ContentAlignment.MiddleLeft;
            colPet.UseMnemonic = false;
            colPet.Name = "colPet";
            colPet.TabIndex = 3;
            // 
            // colService
            // 
            colService.BackColor = Color.FromArgb(246, 248, 251);
            colService.Dock = DockStyle.Fill;
            colService.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colService.ForeColor = Color.FromArgb(68, 83, 106);
            colService.Padding = new Padding(4, 0, 0, 0);
            colService.Margin = new Padding(0);
            colService.Size = new Size(100, 34);
            colService.Text = "Service";
            colService.TextAlign = ContentAlignment.MiddleLeft;
            colService.UseMnemonic = false;
            colService.Name = "colService";
            colService.TabIndex = 4;
            // 
            // colStatus
            // 
            colStatus.BackColor = Color.FromArgb(246, 248, 251);
            colStatus.Dock = DockStyle.Fill;
            colStatus.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colStatus.ForeColor = Color.FromArgb(68, 83, 106);
            colStatus.Padding = new Padding(4, 0, 0, 0);
            colStatus.Margin = new Padding(0);
            colStatus.Size = new Size(100, 34);
            colStatus.Text = "Status";
            colStatus.TextAlign = ContentAlignment.MiddleLeft;
            colStatus.UseMnemonic = false;
            colStatus.Name = "colStatus";
            colStatus.TabIndex = 5;
            // 
            // colActions
            // 
            colActions.BackColor = Color.FromArgb(246, 248, 251);
            colActions.Dock = DockStyle.Fill;
            colActions.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colActions.ForeColor = Color.FromArgb(68, 83, 106);
            colActions.Padding = new Padding(0, 0, 12, 0);
            colActions.Margin = new Padding(0);
            colActions.Size = new Size(100, 34);
            colActions.Text = "Actions";
            colActions.TextAlign = ContentAlignment.MiddleRight;
            colActions.UseMnemonic = false;
            colActions.Name = "colActions";
            colActions.TabIndex = 6;
            // 
            // toolbar
            // 
            toolbar.Controls.Add(searchField);
            toolbar.Controls.Add(statusFilter);
            toolbar.Controls.Add(serviceFilter);
            toolbar.Controls.Add(dateRange);
            toolbar.BackColor = Color.FromArgb(255, 255, 255);
            toolbar.Dock = DockStyle.Top;
            toolbar.Location = new Point(0, 0);
            toolbar.Size = new Size(628, 52);
            toolbar.Name = "toolbar";
            toolbar.TabIndex = 3;
            // 
            // searchField
            // 
            searchField.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchField.Glyph = "\uE721";
            searchField.Location = new Point(0, 8);
            searchField.Placeholder = "Search customer, pet, or service";
            searchField.Size = new Size(196, 36);
            searchField.Name = "searchField";
            searchField.TabIndex = 0;
            // 
            // statusFilter
            // 
            statusFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            statusFilter.BackColor = Color.White;
            statusFilter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusFilter.Location = new Point(204, 8);
            statusFilter.Size = new Size(100, 36);
            statusFilter.Text = "All Statuses";
            statusFilter.Name = "statusFilter";
            statusFilter.TabIndex = 1;
            statusFilter.SelectedIndexChanged += statusFilter_SelectedIndexChanged;
            // 
            // serviceFilter
            // 
            serviceFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            serviceFilter.BackColor = Color.White;
            serviceFilter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            serviceFilter.Location = new Point(312, 8);
            serviceFilter.Size = new Size(100, 36);
            serviceFilter.Text = "All Services";
            serviceFilter.Name = "serviceFilter";
            serviceFilter.TabIndex = 2;
            serviceFilter.SelectedIndexChanged += serviceFilter_SelectedIndexChanged;
            // 
            // dateRange
            // 
            dateRange.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dateRange.BackColor = Color.White;
            dateRange.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateRange.Location = new Point(420, 8);
            dateRange.Size = new Size(208, 36);
            dateRange.Text = "This week";
            dateRange.Name = "dateRange";
            dateRange.TabIndex = 3;
            dateRange.PreviousClicked += dateRange_PreviousClicked;
            dateRange.NextClicked += dateRange_NextClicked;
            dateRange.TextClicked += dateRange_TextClicked;
            // 
            // appointmentsHeader
            // 
            appointmentsHeader.Controls.Add(newAppointmentButton);
            appointmentsHeader.Controls.Add(appointmentsTitle);
            appointmentsHeader.Controls.Add(appointmentsIcon);
            appointmentsHeader.BackColor = Color.FromArgb(255, 255, 255);
            appointmentsHeader.Dock = DockStyle.Top;
            appointmentsHeader.Location = new Point(0, 0);
            appointmentsHeader.Size = new Size(628, 44);
            appointmentsHeader.Name = "appointmentsHeader";
            appointmentsHeader.TabIndex = 4;
            // 
            // newAppointmentButton
            // 
            newAppointmentButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            newAppointmentButton.BackColor = Color.White;
            newAppointmentButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            newAppointmentButton.Location = new Point(478, 5);
            newAppointmentButton.Size = new Size(150, 34);
            newAppointmentButton.Text = "+   New Appointment";
            newAppointmentButton.Name = "newAppointmentButton";
            newAppointmentButton.TabIndex = 0;
            newAppointmentButton.Click += newAppointmentButton_Click;
            // 
            // appointmentsTitle
            // 
            appointmentsTitle.BackColor = Color.FromArgb(255, 255, 255);
            appointmentsTitle.Dock = DockStyle.Left;
            appointmentsTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            appointmentsTitle.ForeColor = Color.FromArgb(31, 42, 60);
            appointmentsTitle.Location = new Point(28, 0);
            appointmentsTitle.Size = new Size(130, 44);
            appointmentsTitle.Text = "Appointments";
            appointmentsTitle.TextAlign = ContentAlignment.MiddleLeft;
            appointmentsTitle.UseMnemonic = false;
            appointmentsTitle.Name = "appointmentsTitle";
            appointmentsTitle.TabIndex = 1;
            // 
            // appointmentsIcon
            // 
            appointmentsIcon.BackColor = Color.FromArgb(255, 255, 255);
            appointmentsIcon.Dock = DockStyle.Left;
            appointmentsIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            appointmentsIcon.ForeColor = Color.FromArgb(58, 123, 213);
            appointmentsIcon.Location = new Point(0, 0);
            appointmentsIcon.Size = new Size(28, 44);
            appointmentsIcon.Text = "\uE787";
            appointmentsIcon.TextAlign = ContentAlignment.MiddleCenter;
            appointmentsIcon.UseMnemonic = false;
            appointmentsIcon.Name = "appointmentsIcon";
            appointmentsIcon.TabIndex = 2;
            // 
            // rightColumn
            // 
            rightColumn.Controls.Add(calendarCard, 0, 0);
            rightColumn.Controls.Add(quickCard, 0, 1);
            rightColumn.Controls.Add(upcomingCard, 0, 2);
            rightColumn.BackColor = Color.FromArgb(243, 247, 252);
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(676, 0);
            rightColumn.Margin = new Padding(0);
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 304F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 172F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightColumn.Size = new Size(320, 692);
            rightColumn.Name = "rightColumn";
            rightColumn.TabIndex = 1;
            // 
            // calendarCard
            // 
            calendarCard.Controls.Add(monthCalendar);
            calendarCard.Controls.Add(legendFlow);
            calendarCard.Controls.Add(calendarHeader);
            calendarCard.BorderColor = Color.FromArgb(227, 233, 242);
            calendarCard.CornerColor = Color.FromArgb(243, 247, 252);
            calendarCard.Dock = DockStyle.Fill;
            calendarCard.FillColor = Color.White;
            calendarCard.Location = new Point(0, 0);
            calendarCard.Margin = new Padding(0, 0, 0, 12);
            calendarCard.Padding = new Padding(16, 12, 16, 12);
            calendarCard.Radius = 10;
            calendarCard.Size = new Size(320, 292);
            calendarCard.Name = "calendarCard";
            calendarCard.TabIndex = 0;
            // 
            // monthCalendar
            // 
            monthCalendar.BackColor = Color.White;
            monthCalendar.Dock = DockStyle.Fill;
            monthCalendar.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            monthCalendar.Location = new Point(16, 44);
            monthCalendar.Size = new Size(288, 196);
            monthCalendar.TabStop = false;
            monthCalendar.Name = "monthCalendar";
            monthCalendar.TabIndex = 0;
            monthCalendar.DateClicked += monthCalendar_DateClicked;
            // 
            // legendFlow
            // 
            legendFlow.Controls.Add(legendConfirmedDot);
            legendFlow.Controls.Add(legendConfirmedLabel);
            legendFlow.Controls.Add(legendPendingDot);
            legendFlow.Controls.Add(legendPendingLabel);
            legendFlow.Controls.Add(legendRescheduledDot);
            legendFlow.Controls.Add(legendRescheduledLabel);
            legendFlow.Controls.Add(legendCancelledDot);
            legendFlow.Controls.Add(legendCancelledLabel);
            legendFlow.BackColor = Color.White;
            legendFlow.Dock = DockStyle.Bottom;
            legendFlow.Location = new Point(16, 240);
            legendFlow.Name = "legendFlow";
            legendFlow.Padding = new Padding(0, 6, 0, 0);
            legendFlow.Size = new Size(288, 28);
            legendFlow.WrapContents = false;
            legendFlow.TabIndex = 1;
            // 
            // legendConfirmedDot
            // 
            legendConfirmedDot.BackColor = Color.White;
            legendConfirmedDot.DotColor = Color.FromArgb(46, 158, 91);
            legendConfirmedDot.Location = new Point(0, 0);
            legendConfirmedDot.Margin = new Padding(0);
            legendConfirmedDot.Size = new Size(14, 20);
            legendConfirmedDot.TabStop = false;
            legendConfirmedDot.Name = "legendConfirmedDot";
            legendConfirmedDot.TabIndex = 0;
            // 
            // legendConfirmedLabel
            // 
            legendConfirmedLabel.BackColor = Color.FromArgb(255, 255, 255);
            legendConfirmedLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            legendConfirmedLabel.ForeColor = Color.FromArgb(122, 136, 156);
            legendConfirmedLabel.Margin = new Padding(0, 0, 8, 0);
            legendConfirmedLabel.Size = new Size(50, 20);
            legendConfirmedLabel.Text = "Confirmed";
            legendConfirmedLabel.TextAlign = ContentAlignment.MiddleLeft;
            legendConfirmedLabel.UseMnemonic = false;
            legendConfirmedLabel.Name = "legendConfirmedLabel";
            legendConfirmedLabel.TabIndex = 1;
            // 
            // legendPendingDot
            // 
            legendPendingDot.BackColor = Color.White;
            legendPendingDot.DotColor = Color.FromArgb(245, 158, 11);
            legendPendingDot.Location = new Point(0, 0);
            legendPendingDot.Margin = new Padding(0);
            legendPendingDot.Size = new Size(14, 20);
            legendPendingDot.TabStop = false;
            legendPendingDot.Name = "legendPendingDot";
            legendPendingDot.TabIndex = 2;
            // 
            // legendPendingLabel
            // 
            legendPendingLabel.BackColor = Color.FromArgb(255, 255, 255);
            legendPendingLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            legendPendingLabel.ForeColor = Color.FromArgb(122, 136, 156);
            legendPendingLabel.Margin = new Padding(0, 0, 8, 0);
            legendPendingLabel.Size = new Size(42, 20);
            legendPendingLabel.Text = "Pending";
            legendPendingLabel.TextAlign = ContentAlignment.MiddleLeft;
            legendPendingLabel.UseMnemonic = false;
            legendPendingLabel.Name = "legendPendingLabel";
            legendPendingLabel.TabIndex = 3;
            // 
            // legendRescheduledDot
            // 
            legendRescheduledDot.BackColor = Color.White;
            legendRescheduledDot.DotColor = Color.FromArgb(139, 92, 246);
            legendRescheduledDot.Location = new Point(0, 0);
            legendRescheduledDot.Margin = new Padding(0);
            legendRescheduledDot.Size = new Size(14, 20);
            legendRescheduledDot.TabStop = false;
            legendRescheduledDot.Name = "legendRescheduledDot";
            legendRescheduledDot.TabIndex = 4;
            // 
            // legendRescheduledLabel
            // 
            legendRescheduledLabel.BackColor = Color.FromArgb(255, 255, 255);
            legendRescheduledLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            legendRescheduledLabel.ForeColor = Color.FromArgb(122, 136, 156);
            legendRescheduledLabel.Margin = new Padding(0, 0, 8, 0);
            legendRescheduledLabel.Size = new Size(62, 20);
            legendRescheduledLabel.Text = "Rescheduled";
            legendRescheduledLabel.TextAlign = ContentAlignment.MiddleLeft;
            legendRescheduledLabel.UseMnemonic = false;
            legendRescheduledLabel.Name = "legendRescheduledLabel";
            legendRescheduledLabel.TabIndex = 5;
            // 
            // legendCancelledDot
            // 
            legendCancelledDot.BackColor = Color.White;
            legendCancelledDot.DotColor = Color.FromArgb(239, 68, 68);
            legendCancelledDot.Location = new Point(0, 0);
            legendCancelledDot.Margin = new Padding(0);
            legendCancelledDot.Size = new Size(14, 20);
            legendCancelledDot.TabStop = false;
            legendCancelledDot.Name = "legendCancelledDot";
            legendCancelledDot.TabIndex = 6;
            // 
            // legendCancelledLabel
            // 
            legendCancelledLabel.BackColor = Color.FromArgb(255, 255, 255);
            legendCancelledLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            legendCancelledLabel.ForeColor = Color.FromArgb(122, 136, 156);
            legendCancelledLabel.Margin = new Padding(0, 0, 8, 0);
            legendCancelledLabel.Size = new Size(50, 20);
            legendCancelledLabel.Text = "Cancelled";
            legendCancelledLabel.TextAlign = ContentAlignment.MiddleLeft;
            legendCancelledLabel.UseMnemonic = false;
            legendCancelledLabel.Name = "legendCancelledLabel";
            legendCancelledLabel.TabIndex = 7;
            // 
            // calendarHeader
            // 
            calendarHeader.Controls.Add(calendarLink);
            calendarHeader.Controls.Add(calendarTitle);
            calendarHeader.Controls.Add(calendarIcon);
            calendarHeader.BackColor = Color.FromArgb(255, 255, 255);
            calendarHeader.Dock = DockStyle.Top;
            calendarHeader.Location = new Point(0, 0);
            calendarHeader.Size = new Size(288, 32);
            calendarHeader.Name = "calendarHeader";
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
            calendarLink.Size = new Size(80, 32);
            calendarLink.Text = "View Full →";
            calendarLink.TextAlign = ContentAlignment.MiddleRight;
            calendarLink.UseMnemonic = false;
            calendarLink.Name = "calendarLink";
            calendarLink.TabIndex = 0;
            calendarLink.Click += calendarLink_Click;
            // 
            // calendarTitle
            // 
            calendarTitle.BackColor = Color.FromArgb(255, 255, 255);
            calendarTitle.Dock = DockStyle.Left;
            calendarTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            calendarTitle.ForeColor = Color.FromArgb(31, 42, 60);
            calendarTitle.Location = new Point(28, 0);
            calendarTitle.Size = new Size(100, 32);
            calendarTitle.Text = "Calendar";
            calendarTitle.TextAlign = ContentAlignment.MiddleLeft;
            calendarTitle.UseMnemonic = false;
            calendarTitle.Name = "calendarTitle";
            calendarTitle.TabIndex = 1;
            // 
            // calendarIcon
            // 
            calendarIcon.BackColor = Color.FromArgb(255, 255, 255);
            calendarIcon.Dock = DockStyle.Left;
            calendarIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            calendarIcon.ForeColor = Color.FromArgb(58, 123, 213);
            calendarIcon.Location = new Point(0, 0);
            calendarIcon.Size = new Size(28, 32);
            calendarIcon.Text = "\uE787";
            calendarIcon.TextAlign = ContentAlignment.MiddleCenter;
            calendarIcon.UseMnemonic = false;
            calendarIcon.Name = "calendarIcon";
            calendarIcon.TabIndex = 2;
            // 
            // quickCard
            // 
            quickCard.Controls.Add(quickGrid);
            quickCard.Controls.Add(quickHeader);
            quickCard.BorderColor = Color.FromArgb(227, 233, 242);
            quickCard.CornerColor = Color.FromArgb(243, 247, 252);
            quickCard.Dock = DockStyle.Fill;
            quickCard.FillColor = Color.White;
            quickCard.Location = new Point(0, 0);
            quickCard.Margin = new Padding(0, 0, 0, 12);
            quickCard.Padding = new Padding(16, 12, 16, 12);
            quickCard.Radius = 10;
            quickCard.Size = new Size(320, 160);
            quickCard.Name = "quickCard";
            quickCard.TabIndex = 1;
            // 
            // quickGrid
            // 
            quickGrid.Controls.Add(newAppointmentTile, 0, 0);
            quickGrid.Controls.Add(viewAllTile, 1, 0);
            quickGrid.Controls.Add(manageCustomersTile, 0, 1);
            quickGrid.Controls.Add(servicesCatalogTile, 1, 1);
            quickGrid.BackColor = Color.White;
            quickGrid.ColumnCount = 2;
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.Dock = DockStyle.Fill;
            quickGrid.Location = new Point(16, 44);
            quickGrid.Margin = new Padding(0);
            quickGrid.RowCount = 2;
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            quickGrid.Size = new Size(288, 104);
            quickGrid.Name = "quickGrid";
            quickGrid.TabIndex = 0;
            // 
            // newAppointmentTile
            // 
            newAppointmentTile.BackColor = Color.White;
            newAppointmentTile.Dock = DockStyle.Fill;
            newAppointmentTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            newAppointmentTile.Glyph = "\uE787";
            newAppointmentTile.Location = new Point(0, 0);
            newAppointmentTile.Margin = new Padding(3);
            newAppointmentTile.Size = new Size(138, 46);
            newAppointmentTile.Text = "New Appointment";
            newAppointmentTile.Name = "newAppointmentTile";
            newAppointmentTile.TabIndex = 0;
            newAppointmentTile.Click += newAppointmentTile_Click;
            // 
            // viewAllTile
            // 
            viewAllTile.BackColor = Color.White;
            viewAllTile.Dock = DockStyle.Fill;
            viewAllTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            viewAllTile.Glyph = "\uE8FD";
            viewAllTile.Location = new Point(0, 0);
            viewAllTile.Margin = new Padding(3);
            viewAllTile.Size = new Size(138, 46);
            viewAllTile.Text = "View All Appointments";
            viewAllTile.Name = "viewAllTile";
            viewAllTile.TabIndex = 1;
            viewAllTile.Click += viewAllTile_Click;
            // 
            // manageCustomersTile
            // 
            manageCustomersTile.BackColor = Color.White;
            manageCustomersTile.Dock = DockStyle.Fill;
            manageCustomersTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            manageCustomersTile.Glyph = "\uE716";
            manageCustomersTile.Location = new Point(0, 0);
            manageCustomersTile.Margin = new Padding(3);
            manageCustomersTile.Size = new Size(138, 46);
            manageCustomersTile.Text = "Manage Customers";
            manageCustomersTile.Name = "manageCustomersTile";
            manageCustomersTile.TabIndex = 2;
            manageCustomersTile.Click += manageCustomersTile_Click;
            // 
            // servicesCatalogTile
            // 
            servicesCatalogTile.BackColor = Color.White;
            servicesCatalogTile.Dock = DockStyle.Fill;
            servicesCatalogTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            servicesCatalogTile.Glyph = "\uE9D9";
            servicesCatalogTile.Location = new Point(0, 0);
            servicesCatalogTile.Margin = new Padding(3);
            servicesCatalogTile.Size = new Size(138, 46);
            servicesCatalogTile.Text = "Services Catalog";
            servicesCatalogTile.Name = "servicesCatalogTile";
            servicesCatalogTile.TabIndex = 3;
            servicesCatalogTile.Click += servicesCatalogTile_Click;
            // 
            // quickHeader
            // 
            quickHeader.Controls.Add(quickTitle);
            quickHeader.Controls.Add(quickIcon);
            quickHeader.BackColor = Color.FromArgb(255, 255, 255);
            quickHeader.Dock = DockStyle.Top;
            quickHeader.Location = new Point(0, 0);
            quickHeader.Size = new Size(288, 32);
            quickHeader.Name = "quickHeader";
            quickHeader.TabIndex = 1;
            // 
            // quickTitle
            // 
            quickTitle.BackColor = Color.FromArgb(255, 255, 255);
            quickTitle.Dock = DockStyle.Left;
            quickTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickTitle.ForeColor = Color.FromArgb(31, 42, 60);
            quickTitle.Location = new Point(28, 0);
            quickTitle.Size = new Size(160, 32);
            quickTitle.Text = "Quick Actions";
            quickTitle.TextAlign = ContentAlignment.MiddleLeft;
            quickTitle.UseMnemonic = false;
            quickTitle.Name = "quickTitle";
            quickTitle.TabIndex = 0;
            // 
            // quickIcon
            // 
            quickIcon.BackColor = Color.FromArgb(255, 255, 255);
            quickIcon.Dock = DockStyle.Left;
            quickIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            quickIcon.ForeColor = Color.FromArgb(58, 123, 213);
            quickIcon.Location = new Point(0, 0);
            quickIcon.Size = new Size(28, 32);
            quickIcon.Text = "\uE945";
            quickIcon.TextAlign = ContentAlignment.MiddleCenter;
            quickIcon.UseMnemonic = false;
            quickIcon.Name = "quickIcon";
            quickIcon.TabIndex = 1;
            // 
            // upcomingCard
            // 
            upcomingCard.Controls.Add(upcomingList);
            upcomingCard.Controls.Add(upcomingEmpty);
            upcomingCard.Controls.Add(upcomingHeader);
            upcomingCard.BorderColor = Color.FromArgb(227, 233, 242);
            upcomingCard.CornerColor = Color.FromArgb(243, 247, 252);
            upcomingCard.Dock = DockStyle.Fill;
            upcomingCard.FillColor = Color.White;
            upcomingCard.Location = new Point(0, 0);
            upcomingCard.Margin = new Padding(0);
            upcomingCard.Padding = new Padding(16, 12, 16, 12);
            upcomingCard.Radius = 10;
            upcomingCard.Size = new Size(320, 216);
            upcomingCard.Name = "upcomingCard";
            upcomingCard.TabIndex = 2;
            // 
            // upcomingList
            // 
            upcomingList.AutoScroll = true;
            upcomingList.BackColor = Color.White;
            upcomingList.Dock = DockStyle.Fill;
            upcomingList.Location = new Point(16, 44);
            upcomingList.Size = new Size(288, 160);
            upcomingList.Visible = false;
            upcomingList.Name = "upcomingList";
            upcomingList.TabIndex = 0;
            // 
            // upcomingEmpty
            // 
            upcomingEmpty.BackColor = Color.White;
            upcomingEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            upcomingEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            upcomingEmpty.Dock = DockStyle.Fill;
            upcomingEmpty.Glyph = "\uE823";
            upcomingEmpty.Hint = "Today's appointments will show up here.";
            upcomingEmpty.Location = new Point(16, 44);
            upcomingEmpty.Size = new Size(288, 160);
            upcomingEmpty.TabStop = false;
            upcomingEmpty.Title = "Nothing scheduled today";
            upcomingEmpty.Name = "upcomingEmpty";
            upcomingEmpty.TabIndex = 1;
            // 
            // upcomingHeader
            // 
            upcomingHeader.Controls.Add(upcomingLink);
            upcomingHeader.Controls.Add(upcomingTitle);
            upcomingHeader.Controls.Add(upcomingIcon);
            upcomingHeader.BackColor = Color.FromArgb(255, 255, 255);
            upcomingHeader.Dock = DockStyle.Top;
            upcomingHeader.Location = new Point(0, 0);
            upcomingHeader.Size = new Size(288, 32);
            upcomingHeader.Name = "upcomingHeader";
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
            upcomingLink.Size = new Size(70, 32);
            upcomingLink.Text = "View All →";
            upcomingLink.TextAlign = ContentAlignment.MiddleRight;
            upcomingLink.UseMnemonic = false;
            upcomingLink.Name = "upcomingLink";
            upcomingLink.TabIndex = 0;
            upcomingLink.Click += upcomingLink_Click;
            // 
            // upcomingTitle
            // 
            upcomingTitle.BackColor = Color.FromArgb(255, 255, 255);
            upcomingTitle.Dock = DockStyle.Left;
            upcomingTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            upcomingTitle.ForeColor = Color.FromArgb(31, 42, 60);
            upcomingTitle.Location = new Point(28, 0);
            upcomingTitle.Size = new Size(150, 32);
            upcomingTitle.Text = "Upcoming Today";
            upcomingTitle.TextAlign = ContentAlignment.MiddleLeft;
            upcomingTitle.UseMnemonic = false;
            upcomingTitle.Name = "upcomingTitle";
            upcomingTitle.TabIndex = 1;
            // 
            // upcomingIcon
            // 
            upcomingIcon.BackColor = Color.FromArgb(255, 255, 255);
            upcomingIcon.Dock = DockStyle.Left;
            upcomingIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            upcomingIcon.ForeColor = Color.FromArgb(58, 123, 213);
            upcomingIcon.Location = new Point(0, 0);
            upcomingIcon.Size = new Size(28, 32);
            upcomingIcon.Text = "\uE823";
            upcomingIcon.TextAlign = ContentAlignment.MiddleCenter;
            upcomingIcon.UseMnemonic = false;
            upcomingIcon.Name = "upcomingIcon";
            upcomingIcon.TabIndex = 2;
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
            upcomingList.ResumeLayout(false);
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
