#nullable disable
namespace MyApp
{
    partial class AuditTrailPage
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pageHost = new System.Windows.Forms.Panel();
            pageGrid = new System.Windows.Forms.TableLayoutPanel();
            leftColumn = new System.Windows.Forms.Panel();
            activityLogsCard = new MyApp.Controls.CardPanel();
            activityTableArea = new System.Windows.Forms.Panel();
            activityRows = new System.Windows.Forms.Panel();
            activityEmpty = new MyApp.Controls.EmptyState();
            activityColumnsHead = new System.Windows.Forms.Panel();
            activityColumnsGrid = new System.Windows.Forms.TableLayoutPanel();
            activityHeaderCheck = new MyApp.Controls.CheckMark();
            activityColDateTime = new System.Windows.Forms.Label();
            activityColUser = new System.Windows.Forms.Label();
            activityColAction = new System.Windows.Forms.Label();
            activityColModule = new System.Windows.Forms.Label();
            activityColDescription = new System.Windows.Forms.Label();
            activityColReference = new System.Windows.Forms.Label();
            activityColStatus = new System.Windows.Forms.Label();
            activityColActions = new System.Windows.Forms.Label();
            activityTableTitle = new System.Windows.Forms.Panel();
            activityTableTitleText = new System.Windows.Forms.Label();
            activityTableTitleIcon = new System.Windows.Forms.Label();
            activityFooter = new System.Windows.Forms.Panel();
            activityShowing = new System.Windows.Forms.Label();
            activityPager = new System.Windows.Forms.Panel();
            activityPrevPage = new MyApp.Controls.IconButton();
            activityPageChip = new MyApp.Controls.PrimaryButton();
            activityNextPage = new MyApp.Controls.IconButton();
            activityFiltersRow = new System.Windows.Forms.Panel();
            activityDateRange = new MyApp.Controls.DateRangeField();
            filterGapC = new System.Windows.Forms.Panel();
            actionTypeFilter = new MyApp.Controls.SelectField();
            filterGapB = new System.Windows.Forms.Panel();
            moduleFilter = new MyApp.Controls.SelectField();
            filterGapA = new System.Windows.Forms.Panel();
            userFilter = new MyApp.Controls.SelectField();
            activitySearchRow = new System.Windows.Forms.Panel();
            activitySearch = new MyApp.Controls.InputField();
            gapTwo = new System.Windows.Forms.Panel();
            activityStats = new System.Windows.Forms.TableLayoutPanel();
            actTotalTile = new MyApp.Controls.CardPanel();
            actTotalDelta = new System.Windows.Forms.Label();
            actTotalValue = new System.Windows.Forms.Label();
            actTotalTop = new System.Windows.Forms.Panel();
            actTotalLabel = new System.Windows.Forms.Label();
            actTotalIconHost = new System.Windows.Forms.Panel();
            actTotalIcon = new MyApp.Controls.IconBadge();
            actTodayTile = new MyApp.Controls.CardPanel();
            actTodayDelta = new System.Windows.Forms.Label();
            actTodayValue = new System.Windows.Forms.Label();
            actTodayTop = new System.Windows.Forms.Panel();
            actTodayLabel = new System.Windows.Forms.Label();
            actTodayIconHost = new System.Windows.Forms.Panel();
            actTodayIcon = new MyApp.Controls.IconBadge();
            actPendingTile = new MyApp.Controls.CardPanel();
            actPendingDelta = new System.Windows.Forms.Label();
            actPendingValue = new System.Windows.Forms.Label();
            actPendingTop = new System.Windows.Forms.Panel();
            actPendingLabel = new System.Windows.Forms.Label();
            actPendingIconHost = new System.Windows.Forms.Panel();
            actPendingIcon = new MyApp.Controls.IconBadge();
            actUsersTile = new MyApp.Controls.CardPanel();
            actUsersDelta = new System.Windows.Forms.Label();
            actUsersValue = new System.Windows.Forms.Label();
            actUsersTop = new System.Windows.Forms.Panel();
            actUsersLabel = new System.Windows.Forms.Label();
            actUsersIconHost = new System.Windows.Forms.Panel();
            actUsersIcon = new MyApp.Controls.IconBadge();
            gapOne = new System.Windows.Forms.Panel();
            overviewCard = new MyApp.Controls.CardPanel();
            overviewHead = new System.Windows.Forms.Panel();
            overviewText = new System.Windows.Forms.Panel();
            overviewSubtitle = new System.Windows.Forms.Label();
            overviewTitle = new System.Windows.Forms.Label();
            overviewIconHost = new System.Windows.Forms.Panel();
            overviewIcon = new MyApp.Controls.IconBadge();
            overviewDateHost = new System.Windows.Forms.Panel();
            overviewDateRange = new MyApp.Controls.DateRangeField();
            rightColumn = new System.Windows.Forms.TableLayoutPanel();
            quickFiltersCard = new MyApp.Controls.CardPanel();
            loginRow = new System.Windows.Forms.Panel();
            loginName = new System.Windows.Forms.Label();
            loginIconHost = new System.Windows.Forms.Panel();
            loginIcon = new MyApp.Controls.IconBadge();
            loginCountHost = new System.Windows.Forms.Panel();
            loginCount = new System.Windows.Forms.Label();
            loginChevron = new System.Windows.Forms.Label();
            criticalRow = new System.Windows.Forms.Panel();
            criticalName = new System.Windows.Forms.Label();
            criticalIconHost = new System.Windows.Forms.Panel();
            criticalIcon = new MyApp.Controls.IconBadge();
            criticalCountHost = new System.Windows.Forms.Panel();
            criticalCount = new System.Windows.Forms.Label();
            criticalChevron = new System.Windows.Forms.Label();
            weekRow = new System.Windows.Forms.Panel();
            weekName = new System.Windows.Forms.Label();
            weekIconHost = new System.Windows.Forms.Panel();
            weekIcon = new MyApp.Controls.IconBadge();
            weekCountHost = new System.Windows.Forms.Panel();
            weekCount = new System.Windows.Forms.Label();
            weekChevron = new System.Windows.Forms.Label();
            todayRow = new System.Windows.Forms.Panel();
            todayName = new System.Windows.Forms.Label();
            todayIconHost = new System.Windows.Forms.Panel();
            todayIcon = new MyApp.Controls.IconBadge();
            todayCountHost = new System.Windows.Forms.Panel();
            todayCount = new System.Windows.Forms.Label();
            todayChevron = new System.Windows.Forms.Label();
            quickFiltersHeader = new System.Windows.Forms.Panel();
            quickFiltersTitle = new System.Windows.Forms.Label();
            quickFiltersIcon = new System.Windows.Forms.Label();
            quickFiltersLink = new System.Windows.Forms.Label();
            recentActivityCard = new MyApp.Controls.CardPanel();
            recentActivityEmpty = new MyApp.Controls.EmptyState();
            recentActivityHeader = new System.Windows.Forms.Panel();
            recentActivityTitle = new System.Windows.Forms.Label();
            recentActivityIcon = new System.Windows.Forms.Label();
            recentActivityLink = new System.Windows.Forms.Label();
            auditSummaryCard = new MyApp.Controls.CardPanel();
            summaryGrid = new System.Windows.Forms.TableLayoutPanel();
            createdTile = new MyApp.Controls.CardPanel();
            createdIcon = new System.Windows.Forms.Label();
            createdLabel = new System.Windows.Forms.Label();
            createdValue = new System.Windows.Forms.Label();
            updatedTile = new MyApp.Controls.CardPanel();
            updatedIcon = new System.Windows.Forms.Label();
            updatedLabel = new System.Windows.Forms.Label();
            updatedValue = new System.Windows.Forms.Label();
            approvedTile = new MyApp.Controls.CardPanel();
            approvedIcon = new System.Windows.Forms.Label();
            approvedLabel = new System.Windows.Forms.Label();
            approvedValue = new System.Windows.Forms.Label();
            deletedTile = new MyApp.Controls.CardPanel();
            deletedIcon = new System.Windows.Forms.Label();
            deletedLabel = new System.Windows.Forms.Label();
            deletedValue = new System.Windows.Forms.Label();
            auditSummaryHeader = new System.Windows.Forms.Panel();
            auditSummaryTitle = new System.Windows.Forms.Label();
            auditSummaryIcon = new System.Windows.Forms.Label();
            exportCard = new MyApp.Controls.CardPanel();
            exportGrid = new System.Windows.Forms.TableLayoutPanel();
            exportPdfTile = new MyApp.Controls.CardPanel();
            exportPdfIconHost = new System.Windows.Forms.Panel();
            exportPdfIcon = new MyApp.Controls.IconBadge();
            exportPdfTextHost = new System.Windows.Forms.Panel();
            exportPdfSub = new System.Windows.Forms.Label();
            exportPdfLabel = new System.Windows.Forms.Label();
            exportExcelTile = new MyApp.Controls.CardPanel();
            exportExcelIconHost = new System.Windows.Forms.Panel();
            exportExcelIcon = new MyApp.Controls.IconBadge();
            exportExcelTextHost = new System.Windows.Forms.Panel();
            exportExcelSub = new System.Windows.Forms.Label();
            exportExcelLabel = new System.Windows.Forms.Label();
            exportHeader = new System.Windows.Forms.Panel();
            exportTitle = new System.Windows.Forms.Label();
            exportIcon = new System.Windows.Forms.Label();
            pageHost.SuspendLayout();
            pageGrid.SuspendLayout();
            leftColumn.SuspendLayout();
            activityLogsCard.SuspendLayout();
            activityTableArea.SuspendLayout();
            activityRows.SuspendLayout();
            activityColumnsHead.SuspendLayout();
            activityColumnsGrid.SuspendLayout();
            activityTableTitle.SuspendLayout();
            activityFooter.SuspendLayout();
            activityPager.SuspendLayout();
            activityFiltersRow.SuspendLayout();
            activitySearchRow.SuspendLayout();
            activityStats.SuspendLayout();
            actTotalTile.SuspendLayout();
            actTotalTop.SuspendLayout();
            actTotalIconHost.SuspendLayout();
            actTodayTile.SuspendLayout();
            actTodayTop.SuspendLayout();
            actTodayIconHost.SuspendLayout();
            actPendingTile.SuspendLayout();
            actPendingTop.SuspendLayout();
            actPendingIconHost.SuspendLayout();
            actUsersTile.SuspendLayout();
            actUsersTop.SuspendLayout();
            actUsersIconHost.SuspendLayout();
            overviewCard.SuspendLayout();
            overviewHead.SuspendLayout();
            overviewText.SuspendLayout();
            overviewIconHost.SuspendLayout();
            overviewDateHost.SuspendLayout();
            rightColumn.SuspendLayout();
            quickFiltersCard.SuspendLayout();
            loginRow.SuspendLayout();
            loginIconHost.SuspendLayout();
            loginCountHost.SuspendLayout();
            criticalRow.SuspendLayout();
            criticalIconHost.SuspendLayout();
            criticalCountHost.SuspendLayout();
            weekRow.SuspendLayout();
            weekIconHost.SuspendLayout();
            weekCountHost.SuspendLayout();
            todayRow.SuspendLayout();
            todayIconHost.SuspendLayout();
            todayCountHost.SuspendLayout();
            quickFiltersHeader.SuspendLayout();
            recentActivityCard.SuspendLayout();
            recentActivityHeader.SuspendLayout();
            auditSummaryCard.SuspendLayout();
            summaryGrid.SuspendLayout();
            createdTile.SuspendLayout();
            updatedTile.SuspendLayout();
            approvedTile.SuspendLayout();
            deletedTile.SuspendLayout();
            auditSummaryHeader.SuspendLayout();
            exportCard.SuspendLayout();
            exportGrid.SuspendLayout();
            exportPdfTile.SuspendLayout();
            exportPdfIconHost.SuspendLayout();
            exportPdfTextHost.SuspendLayout();
            exportExcelTile.SuspendLayout();
            exportExcelIconHost.SuspendLayout();
            exportExcelTextHost.SuspendLayout();
            exportHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pageHost
            // 
            pageHost.BackColor = Color.FromArgb(243, 247, 252);
            pageHost.Controls.Add(pageGrid);
            pageHost.Dock = DockStyle.Fill;
            pageHost.Location = new Point(0, 0);
            pageHost.Name = "pageHost";
            pageHost.Padding = new Padding(20, 20, 20, 20);
            pageHost.Size = new Size(1036, 732);
            pageHost.TabIndex = 0;
            // 
            // pageGrid
            // 
            pageGrid.BackColor = Color.FromArgb(243, 247, 252);
            pageGrid.ColumnCount = 3;
            pageGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pageGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            pageGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            pageGrid.Controls.Add(leftColumn, 0, 0);
            pageGrid.Controls.Add(rightColumn, 2, 0);
            pageGrid.Dock = DockStyle.Fill;
            pageGrid.Location = new Point(20, 20);
            pageGrid.Margin = new Padding(0, 0, 0, 0);
            pageGrid.Name = "pageGrid";
            pageGrid.RowCount = 1;
            pageGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pageGrid.Size = new Size(996, 692);
            pageGrid.TabIndex = 0;
            // 
            // leftColumn
            // 
            leftColumn.BackColor = Color.FromArgb(243, 247, 252);
            leftColumn.Controls.Add(activityLogsCard);
            leftColumn.Controls.Add(gapTwo);
            leftColumn.Controls.Add(activityStats);
            leftColumn.Controls.Add(gapOne);
            leftColumn.Controls.Add(overviewCard);
            leftColumn.Dock = DockStyle.Fill;
            leftColumn.Location = new Point(0, 0);
            leftColumn.Margin = new Padding(0, 0, 0, 0);
            leftColumn.Name = "leftColumn";
            leftColumn.Size = new Size(700, 692);
            leftColumn.TabIndex = 0;
            // 
            // activityLogsCard
            // 
            activityLogsCard.BorderColor = Color.FromArgb(227, 233, 242);
            activityLogsCard.Controls.Add(activityTableArea);
            activityLogsCard.Controls.Add(activityFooter);
            activityLogsCard.Controls.Add(activityFiltersRow);
            activityLogsCard.Controls.Add(activitySearchRow);
            activityLogsCard.CornerColor = Color.FromArgb(243, 247, 252);
            activityLogsCard.Dock = DockStyle.Fill;
            activityLogsCard.FillColor = Color.White;
            activityLogsCard.Location = new Point(0, 0);
            activityLogsCard.Name = "activityLogsCard";
            activityLogsCard.Padding = new Padding(16, 14, 16, 10);
            activityLogsCard.Radius = 10;
            activityLogsCard.Size = new Size(700, 340);
            activityLogsCard.TabIndex = 0;
            // 
            // activityTableArea
            // 
            activityTableArea.BackColor = Color.White;
            activityTableArea.Controls.Add(activityRows);
            activityTableArea.Controls.Add(activityColumnsHead);
            activityTableArea.Controls.Add(activityTableTitle);
            activityTableArea.Dock = DockStyle.Fill;
            activityTableArea.Location = new Point(0, 0);
            activityTableArea.Name = "activityTableArea";
            activityTableArea.Size = new Size(668, 240);
            activityTableArea.TabIndex = 0;
            // 
            // activityRows
            // 
            activityRows.AutoScroll = true;
            activityRows.BackColor = Color.White;
            activityRows.Controls.Add(activityEmpty);
            activityRows.Dock = DockStyle.Fill;
            activityRows.Location = new Point(0, 66);
            activityRows.Name = "activityRows";
            activityRows.Size = new Size(668, 200);
            activityRows.TabIndex = 0;
            // 
            // activityEmpty
            // 
            activityEmpty.BackColor = Color.White;
            activityEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            activityEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            activityEmpty.Dock = DockStyle.Fill;
            activityEmpty.Glyph = "\uE81C";
            activityEmpty.Hint = "System actions, approvals, updates and record changes will be logged here.";
            activityEmpty.Location = new Point(0, 0);
            activityEmpty.Name = "activityEmpty";
            activityEmpty.Size = new Size(668, 200);
            activityEmpty.TabIndex = 0;
            activityEmpty.TabStop = false;
            activityEmpty.Title = "No activity logs found";
            // 
            // activityColumnsHead
            // 
            activityColumnsHead.BackColor = Color.FromArgb(246, 248, 251);
            activityColumnsHead.Controls.Add(activityColumnsGrid);
            activityColumnsHead.Dock = DockStyle.Top;
            activityColumnsHead.Location = new Point(0, 0);
            activityColumnsHead.Name = "activityColumnsHead";
            activityColumnsHead.Size = new Size(668, 36);
            activityColumnsHead.TabIndex = 1;
            // 
            // activityColumnsGrid
            // 
            activityColumnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            activityColumnsGrid.ColumnCount = 9;
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            activityColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 64F));
            activityColumnsGrid.Controls.Add(activityHeaderCheck, 0, 0);
            activityColumnsGrid.Controls.Add(activityColDateTime, 1, 0);
            activityColumnsGrid.Controls.Add(activityColUser, 2, 0);
            activityColumnsGrid.Controls.Add(activityColAction, 3, 0);
            activityColumnsGrid.Controls.Add(activityColModule, 4, 0);
            activityColumnsGrid.Controls.Add(activityColDescription, 5, 0);
            activityColumnsGrid.Controls.Add(activityColReference, 6, 0);
            activityColumnsGrid.Controls.Add(activityColStatus, 7, 0);
            activityColumnsGrid.Controls.Add(activityColActions, 8, 0);
            activityColumnsGrid.Dock = DockStyle.Fill;
            activityColumnsGrid.Location = new Point(0, 0);
            activityColumnsGrid.Margin = new Padding(0, 0, 0, 0);
            activityColumnsGrid.Name = "activityColumnsGrid";
            activityColumnsGrid.RowCount = 1;
            activityColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            activityColumnsGrid.Size = new Size(668, 36);
            activityColumnsGrid.TabIndex = 0;
            // 
            // activityHeaderCheck
            // 
            activityHeaderCheck.Anchor = AnchorStyles.None;
            activityHeaderCheck.BackColor = Color.FromArgb(246, 248, 251);
            activityHeaderCheck.Location = new Point(13, 9);
            activityHeaderCheck.Name = "activityHeaderCheck";
            activityHeaderCheck.Size = new Size(18, 18);
            activityHeaderCheck.TabIndex = 0;
            activityHeaderCheck.TabStop = false;
            // 
            // activityColDateTime
            // 
            activityColDateTime.BackColor = Color.FromArgb(246, 248, 251);
            activityColDateTime.Dock = DockStyle.Fill;
            activityColDateTime.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityColDateTime.ForeColor = Color.FromArgb(122, 136, 156);
            activityColDateTime.Location = new Point(0, 0);
            activityColDateTime.Margin = new Padding(0, 0, 0, 0);
            activityColDateTime.Name = "activityColDateTime";
            activityColDateTime.Padding = new Padding(8, 0, 0, 0);
            activityColDateTime.Size = new Size(60, 36);
            activityColDateTime.TabIndex = 1;
            activityColDateTime.Text = "DATE & TIME";
            activityColDateTime.TextAlign = ContentAlignment.MiddleLeft;
            activityColDateTime.UseMnemonic = false;
            // 
            // activityColUser
            // 
            activityColUser.BackColor = Color.FromArgb(246, 248, 251);
            activityColUser.Dock = DockStyle.Fill;
            activityColUser.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityColUser.ForeColor = Color.FromArgb(122, 136, 156);
            activityColUser.Location = new Point(0, 0);
            activityColUser.Margin = new Padding(0, 0, 0, 0);
            activityColUser.Name = "activityColUser";
            activityColUser.Padding = new Padding(8, 0, 0, 0);
            activityColUser.Size = new Size(60, 36);
            activityColUser.TabIndex = 2;
            activityColUser.Text = "USER";
            activityColUser.TextAlign = ContentAlignment.MiddleLeft;
            activityColUser.UseMnemonic = false;
            // 
            // activityColAction
            // 
            activityColAction.BackColor = Color.FromArgb(246, 248, 251);
            activityColAction.Dock = DockStyle.Fill;
            activityColAction.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityColAction.ForeColor = Color.FromArgb(122, 136, 156);
            activityColAction.Location = new Point(0, 0);
            activityColAction.Margin = new Padding(0, 0, 0, 0);
            activityColAction.Name = "activityColAction";
            activityColAction.Padding = new Padding(8, 0, 0, 0);
            activityColAction.Size = new Size(60, 36);
            activityColAction.TabIndex = 3;
            activityColAction.Text = "ACTION";
            activityColAction.TextAlign = ContentAlignment.MiddleLeft;
            activityColAction.UseMnemonic = false;
            // 
            // activityColModule
            // 
            activityColModule.BackColor = Color.FromArgb(246, 248, 251);
            activityColModule.Dock = DockStyle.Fill;
            activityColModule.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityColModule.ForeColor = Color.FromArgb(122, 136, 156);
            activityColModule.Location = new Point(0, 0);
            activityColModule.Margin = new Padding(0, 0, 0, 0);
            activityColModule.Name = "activityColModule";
            activityColModule.Padding = new Padding(8, 0, 0, 0);
            activityColModule.Size = new Size(60, 36);
            activityColModule.TabIndex = 4;
            activityColModule.Text = "MODULE";
            activityColModule.TextAlign = ContentAlignment.MiddleLeft;
            activityColModule.UseMnemonic = false;
            // 
            // activityColDescription
            // 
            activityColDescription.BackColor = Color.FromArgb(246, 248, 251);
            activityColDescription.Dock = DockStyle.Fill;
            activityColDescription.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityColDescription.ForeColor = Color.FromArgb(122, 136, 156);
            activityColDescription.Location = new Point(0, 0);
            activityColDescription.Margin = new Padding(0, 0, 0, 0);
            activityColDescription.Name = "activityColDescription";
            activityColDescription.Padding = new Padding(8, 0, 0, 0);
            activityColDescription.Size = new Size(60, 36);
            activityColDescription.TabIndex = 5;
            activityColDescription.Text = "DESCRIPTION";
            activityColDescription.TextAlign = ContentAlignment.MiddleLeft;
            activityColDescription.UseMnemonic = false;
            // 
            // activityColReference
            // 
            activityColReference.BackColor = Color.FromArgb(246, 248, 251);
            activityColReference.Dock = DockStyle.Fill;
            activityColReference.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityColReference.ForeColor = Color.FromArgb(122, 136, 156);
            activityColReference.Location = new Point(0, 0);
            activityColReference.Margin = new Padding(0, 0, 0, 0);
            activityColReference.Name = "activityColReference";
            activityColReference.Padding = new Padding(8, 0, 0, 0);
            activityColReference.Size = new Size(60, 36);
            activityColReference.TabIndex = 6;
            activityColReference.Text = "REFERENCE";
            activityColReference.TextAlign = ContentAlignment.MiddleLeft;
            activityColReference.UseMnemonic = false;
            // 
            // activityColStatus
            // 
            activityColStatus.BackColor = Color.FromArgb(246, 248, 251);
            activityColStatus.Dock = DockStyle.Fill;
            activityColStatus.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityColStatus.ForeColor = Color.FromArgb(122, 136, 156);
            activityColStatus.Location = new Point(0, 0);
            activityColStatus.Margin = new Padding(0, 0, 0, 0);
            activityColStatus.Name = "activityColStatus";
            activityColStatus.Padding = new Padding(8, 0, 0, 0);
            activityColStatus.Size = new Size(60, 36);
            activityColStatus.TabIndex = 7;
            activityColStatus.Text = "STATUS";
            activityColStatus.TextAlign = ContentAlignment.MiddleLeft;
            activityColStatus.UseMnemonic = false;
            // 
            // activityColActions
            // 
            activityColActions.BackColor = Color.FromArgb(246, 248, 251);
            activityColActions.Dock = DockStyle.Fill;
            activityColActions.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityColActions.ForeColor = Color.FromArgb(122, 136, 156);
            activityColActions.Location = new Point(0, 0);
            activityColActions.Margin = new Padding(0, 0, 0, 0);
            activityColActions.Name = "activityColActions";
            activityColActions.Padding = new Padding(8, 0, 0, 0);
            activityColActions.Size = new Size(60, 36);
            activityColActions.TabIndex = 8;
            activityColActions.Text = "ACTIONS";
            activityColActions.TextAlign = ContentAlignment.MiddleLeft;
            activityColActions.UseMnemonic = false;
            // 
            // activityTableTitle
            // 
            activityTableTitle.BackColor = Color.White;
            activityTableTitle.Controls.Add(activityTableTitleText);
            activityTableTitle.Controls.Add(activityTableTitleIcon);
            activityTableTitle.Dock = DockStyle.Top;
            activityTableTitle.Location = new Point(0, 0);
            activityTableTitle.Name = "activityTableTitle";
            activityTableTitle.Size = new Size(668, 30);
            activityTableTitle.TabIndex = 2;
            // 
            // activityTableTitleText
            // 
            activityTableTitleText.BackColor = Color.White;
            activityTableTitleText.Dock = DockStyle.Fill;
            activityTableTitleText.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityTableTitleText.ForeColor = Color.FromArgb(31, 42, 60);
            activityTableTitleText.Location = new Point(0, 0);
            activityTableTitleText.Name = "activityTableTitleText";
            activityTableTitleText.Size = new Size(200, 30);
            activityTableTitleText.TabIndex = 0;
            activityTableTitleText.Text = "System Activity Logs";
            activityTableTitleText.TextAlign = ContentAlignment.MiddleLeft;
            activityTableTitleText.UseMnemonic = false;
            // 
            // activityTableTitleIcon
            // 
            activityTableTitleIcon.BackColor = Color.White;
            activityTableTitleIcon.Dock = DockStyle.Left;
            activityTableTitleIcon.Font = new Font("Segoe MDL2 Assets", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            activityTableTitleIcon.ForeColor = Color.FromArgb(58, 123, 213);
            activityTableTitleIcon.Location = new Point(0, 0);
            activityTableTitleIcon.Name = "activityTableTitleIcon";
            activityTableTitleIcon.Size = new Size(24, 30);
            activityTableTitleIcon.TabIndex = 1;
            activityTableTitleIcon.Text = "\uE81C";
            activityTableTitleIcon.TextAlign = ContentAlignment.MiddleCenter;
            activityTableTitleIcon.UseMnemonic = false;
            // 
            // activityFooter
            // 
            activityFooter.BackColor = Color.White;
            activityFooter.Controls.Add(activityShowing);
            activityFooter.Controls.Add(activityPager);
            activityFooter.Dock = DockStyle.Bottom;
            activityFooter.Location = new Point(0, 0);
            activityFooter.Name = "activityFooter";
            activityFooter.Size = new Size(668, 44);
            activityFooter.TabIndex = 1;
            // 
            // activityShowing
            // 
            activityShowing.BackColor = Color.White;
            activityShowing.Dock = DockStyle.Fill;
            activityShowing.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            activityShowing.ForeColor = Color.FromArgb(122, 136, 156);
            activityShowing.Location = new Point(0, 0);
            activityShowing.Name = "activityShowing";
            activityShowing.Size = new Size(500, 44);
            activityShowing.TabIndex = 0;
            activityShowing.Text = "Showing 0 activities";
            activityShowing.TextAlign = ContentAlignment.MiddleLeft;
            activityShowing.UseMnemonic = false;
            // 
            // activityPager
            // 
            activityPager.BackColor = Color.White;
            activityPager.Controls.Add(activityPrevPage);
            activityPager.Controls.Add(activityPageChip);
            activityPager.Controls.Add(activityNextPage);
            activityPager.Dock = DockStyle.Right;
            activityPager.Location = new Point(0, 0);
            activityPager.Name = "activityPager";
            activityPager.Size = new Size(116, 44);
            activityPager.TabIndex = 1;
            // 
            // activityPrevPage
            // 
            activityPrevPage.BackColor = Color.White;
            activityPrevPage.Enabled = false;
            activityPrevPage.Glyph = "\uE76B";
            activityPrevPage.Location = new Point(4, 7);
            activityPrevPage.Name = "activityPrevPage";
            activityPrevPage.Size = new Size(30, 30);
            activityPrevPage.TabIndex = 0;
            // 
            // activityPageChip
            // 
            activityPageChip.BackColor = Color.White;
            activityPageChip.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            activityPageChip.Location = new Point(42, 7);
            activityPageChip.Name = "activityPageChip";
            activityPageChip.Size = new Size(30, 30);
            activityPageChip.TabIndex = 1;
            activityPageChip.Text = "1";
            // 
            // activityNextPage
            // 
            activityNextPage.BackColor = Color.White;
            activityNextPage.Enabled = false;
            activityNextPage.Glyph = "\uE76C";
            activityNextPage.Location = new Point(80, 7);
            activityNextPage.Name = "activityNextPage";
            activityNextPage.Size = new Size(30, 30);
            activityNextPage.TabIndex = 2;
            // 
            // activityFiltersRow
            // 
            activityFiltersRow.BackColor = Color.White;
            activityFiltersRow.Controls.Add(activityDateRange);
            activityFiltersRow.Controls.Add(filterGapC);
            activityFiltersRow.Controls.Add(actionTypeFilter);
            activityFiltersRow.Controls.Add(filterGapB);
            activityFiltersRow.Controls.Add(moduleFilter);
            activityFiltersRow.Controls.Add(filterGapA);
            activityFiltersRow.Controls.Add(userFilter);
            activityFiltersRow.Dock = DockStyle.Top;
            activityFiltersRow.Location = new Point(0, 0);
            activityFiltersRow.Name = "activityFiltersRow";
            activityFiltersRow.Padding = new Padding(0, 0, 0, 8);
            activityFiltersRow.Size = new Size(668, 44);
            activityFiltersRow.TabIndex = 2;
            // 
            // activityDateRange
            // 
            activityDateRange.BackColor = Color.White;
            activityDateRange.Dock = DockStyle.Left;
            activityDateRange.Location = new Point(0, 0);
            activityDateRange.Name = "activityDateRange";
            activityDateRange.Size = new Size(214, 36);
            activityDateRange.TabIndex = 0;
            activityDateRange.Text = "All dates";
            // 
            // filterGapC
            // 
            filterGapC.BackColor = Color.White;
            filterGapC.Dock = DockStyle.Left;
            filterGapC.Location = new Point(0, 0);
            filterGapC.Name = "filterGapC";
            filterGapC.Size = new Size(8, 36);
            filterGapC.TabIndex = 1;
            // 
            // actionTypeFilter
            // 
            actionTypeFilter.BackColor = Color.White;
            actionTypeFilter.Dock = DockStyle.Left;
            actionTypeFilter.Location = new Point(0, 0);
            actionTypeFilter.Name = "actionTypeFilter";
            actionTypeFilter.Size = new Size(134, 36);
            actionTypeFilter.TabIndex = 2;
            actionTypeFilter.Text = "All Action Types";
            // 
            // filterGapB
            // 
            filterGapB.BackColor = Color.White;
            filterGapB.Dock = DockStyle.Left;
            filterGapB.Location = new Point(0, 0);
            filterGapB.Name = "filterGapB";
            filterGapB.Size = new Size(8, 36);
            filterGapB.TabIndex = 3;
            // 
            // moduleFilter
            // 
            moduleFilter.BackColor = Color.White;
            moduleFilter.Dock = DockStyle.Left;
            moduleFilter.Location = new Point(0, 0);
            moduleFilter.Name = "moduleFilter";
            moduleFilter.Size = new Size(116, 36);
            moduleFilter.TabIndex = 4;
            moduleFilter.Text = "All Modules";
            // 
            // filterGapA
            // 
            filterGapA.BackColor = Color.White;
            filterGapA.Dock = DockStyle.Left;
            filterGapA.Location = new Point(0, 0);
            filterGapA.Name = "filterGapA";
            filterGapA.Size = new Size(8, 36);
            filterGapA.TabIndex = 5;
            // 
            // userFilter
            // 
            userFilter.BackColor = Color.White;
            userFilter.Dock = DockStyle.Left;
            userFilter.Location = new Point(0, 0);
            userFilter.Name = "userFilter";
            userFilter.Size = new Size(110, 36);
            userFilter.TabIndex = 6;
            userFilter.Text = "All Users";
            // 
            // activitySearchRow
            // 
            activitySearchRow.BackColor = Color.White;
            activitySearchRow.Controls.Add(activitySearch);
            activitySearchRow.Dock = DockStyle.Top;
            activitySearchRow.Location = new Point(0, 0);
            activitySearchRow.Name = "activitySearchRow";
            activitySearchRow.Padding = new Padding(0, 0, 0, 8);
            activitySearchRow.Size = new Size(668, 44);
            activitySearchRow.TabIndex = 3;
            // 
            // activitySearch
            // 
            activitySearch.BorderColor = Color.FromArgb(227, 233, 242);
            activitySearch.CornerColor = Color.White;
            activitySearch.Dock = DockStyle.Fill;
            activitySearch.FillColor = Color.White;
            activitySearch.Glyph = "\uE721";
            activitySearch.Location = new Point(0, 0);
            activitySearch.Name = "activitySearch";
            activitySearch.Padding = new Padding(10, 3, 10, 3);
            activitySearch.Placeholder = "Search activity (user, action, description, reference...)";
            activitySearch.Radius = 8;
            activitySearch.Size = new Size(668, 36);
            activitySearch.TabIndex = 0;
            // 
            // gapTwo
            // 
            gapTwo.BackColor = Color.FromArgb(243, 247, 252);
            gapTwo.Dock = DockStyle.Top;
            gapTwo.Location = new Point(0, 0);
            gapTwo.Name = "gapTwo";
            gapTwo.Size = new Size(10, 16);
            gapTwo.TabIndex = 1;
            // 
            // activityStats
            // 
            activityStats.BackColor = Color.FromArgb(243, 247, 252);
            activityStats.ColumnCount = 7;
            activityStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            activityStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            activityStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            activityStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            activityStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            activityStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            activityStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            activityStats.Controls.Add(actTotalTile, 0, 0);
            activityStats.Controls.Add(actTodayTile, 2, 0);
            activityStats.Controls.Add(actPendingTile, 4, 0);
            activityStats.Controls.Add(actUsersTile, 6, 0);
            activityStats.Dock = DockStyle.Top;
            activityStats.Location = new Point(0, 0);
            activityStats.Margin = new Padding(0, 0, 0, 0);
            activityStats.Name = "activityStats";
            activityStats.RowCount = 1;
            activityStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            activityStats.Size = new Size(700, 100);
            activityStats.TabIndex = 2;
            // 
            // actTotalTile
            // 
            actTotalTile.BorderColor = Color.FromArgb(227, 233, 242);
            actTotalTile.Controls.Add(actTotalDelta);
            actTotalTile.Controls.Add(actTotalValue);
            actTotalTile.Controls.Add(actTotalTop);
            actTotalTile.CornerColor = Color.FromArgb(243, 247, 252);
            actTotalTile.Dock = DockStyle.Fill;
            actTotalTile.FillColor = Color.White;
            actTotalTile.Location = new Point(0, 0);
            actTotalTile.Margin = new Padding(0, 0, 0, 0);
            actTotalTile.Name = "actTotalTile";
            actTotalTile.Padding = new Padding(12, 10, 12, 8);
            actTotalTile.Radius = 10;
            actTotalTile.Size = new Size(160, 100);
            actTotalTile.TabIndex = 0;
            // 
            // actTotalDelta
            // 
            actTotalDelta.AutoEllipsis = true;
            actTotalDelta.BackColor = Color.White;
            actTotalDelta.Dock = DockStyle.Top;
            actTotalDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actTotalDelta.ForeColor = Color.FromArgb(122, 136, 156);
            actTotalDelta.Location = new Point(0, 0);
            actTotalDelta.Name = "actTotalDelta";
            actTotalDelta.Size = new Size(130, 16);
            actTotalDelta.TabIndex = 0;
            actTotalDelta.Text = "\u2014 vs. last month";
            actTotalDelta.TextAlign = ContentAlignment.MiddleLeft;
            actTotalDelta.UseMnemonic = false;
            // 
            // actTotalValue
            // 
            actTotalValue.BackColor = Color.White;
            actTotalValue.Dock = DockStyle.Top;
            actTotalValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            actTotalValue.ForeColor = Color.FromArgb(31, 42, 60);
            actTotalValue.Location = new Point(0, 0);
            actTotalValue.Name = "actTotalValue";
            actTotalValue.Size = new Size(130, 28);
            actTotalValue.TabIndex = 1;
            actTotalValue.Text = "0";
            actTotalValue.TextAlign = ContentAlignment.MiddleLeft;
            actTotalValue.UseMnemonic = false;
            // 
            // actTotalTop
            // 
            actTotalTop.BackColor = Color.White;
            actTotalTop.Controls.Add(actTotalLabel);
            actTotalTop.Controls.Add(actTotalIconHost);
            actTotalTop.Dock = DockStyle.Top;
            actTotalTop.Location = new Point(0, 0);
            actTotalTop.Name = "actTotalTop";
            actTotalTop.Size = new Size(130, 34);
            actTotalTop.TabIndex = 2;
            // 
            // actTotalLabel
            // 
            actTotalLabel.BackColor = Color.White;
            actTotalLabel.Dock = DockStyle.Fill;
            actTotalLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actTotalLabel.ForeColor = Color.FromArgb(68, 83, 106);
            actTotalLabel.Location = new Point(0, 0);
            actTotalLabel.Name = "actTotalLabel";
            actTotalLabel.Size = new Size(90, 34);
            actTotalLabel.TabIndex = 0;
            actTotalLabel.Text = "Total Activities";
            actTotalLabel.TextAlign = ContentAlignment.MiddleLeft;
            actTotalLabel.UseMnemonic = false;
            // 
            // actTotalIconHost
            // 
            actTotalIconHost.BackColor = Color.White;
            actTotalIconHost.Controls.Add(actTotalIcon);
            actTotalIconHost.Dock = DockStyle.Left;
            actTotalIconHost.Location = new Point(0, 0);
            actTotalIconHost.Name = "actTotalIconHost";
            actTotalIconHost.Size = new Size(38, 34);
            actTotalIconHost.TabIndex = 1;
            // 
            // actTotalIcon
            // 
            actTotalIcon.BackColor = Color.White;
            actTotalIcon.FillColor = Color.FromArgb(234, 242, 253);
            actTotalIcon.ForeColor = Color.FromArgb(58, 123, 213);
            actTotalIcon.Glyph = "\uE9F9";
            actTotalIcon.GlyphSize = 10F;
            actTotalIcon.Location = new Point(0, 2);
            actTotalIcon.Name = "actTotalIcon";
            actTotalIcon.Radius = 8;
            actTotalIcon.Size = new Size(30, 30);
            actTotalIcon.TabIndex = 0;
            actTotalIcon.TabStop = false;
            // 
            // actTodayTile
            // 
            actTodayTile.BorderColor = Color.FromArgb(227, 233, 242);
            actTodayTile.Controls.Add(actTodayDelta);
            actTodayTile.Controls.Add(actTodayValue);
            actTodayTile.Controls.Add(actTodayTop);
            actTodayTile.CornerColor = Color.FromArgb(243, 247, 252);
            actTodayTile.Dock = DockStyle.Fill;
            actTodayTile.FillColor = Color.White;
            actTodayTile.Location = new Point(0, 0);
            actTodayTile.Margin = new Padding(0, 0, 0, 0);
            actTodayTile.Name = "actTodayTile";
            actTodayTile.Padding = new Padding(12, 10, 12, 8);
            actTodayTile.Radius = 10;
            actTodayTile.Size = new Size(160, 100);
            actTodayTile.TabIndex = 1;
            // 
            // actTodayDelta
            // 
            actTodayDelta.AutoEllipsis = true;
            actTodayDelta.BackColor = Color.White;
            actTodayDelta.Dock = DockStyle.Top;
            actTodayDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actTodayDelta.ForeColor = Color.FromArgb(122, 136, 156);
            actTodayDelta.Location = new Point(0, 0);
            actTodayDelta.Name = "actTodayDelta";
            actTodayDelta.Size = new Size(130, 16);
            actTodayDelta.TabIndex = 0;
            actTodayDelta.Text = "\u2014 vs. same day last week";
            actTodayDelta.TextAlign = ContentAlignment.MiddleLeft;
            actTodayDelta.UseMnemonic = false;
            // 
            // actTodayValue
            // 
            actTodayValue.BackColor = Color.White;
            actTodayValue.Dock = DockStyle.Top;
            actTodayValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            actTodayValue.ForeColor = Color.FromArgb(31, 42, 60);
            actTodayValue.Location = new Point(0, 0);
            actTodayValue.Name = "actTodayValue";
            actTodayValue.Size = new Size(130, 28);
            actTodayValue.TabIndex = 1;
            actTodayValue.Text = "0";
            actTodayValue.TextAlign = ContentAlignment.MiddleLeft;
            actTodayValue.UseMnemonic = false;
            // 
            // actTodayTop
            // 
            actTodayTop.BackColor = Color.White;
            actTodayTop.Controls.Add(actTodayLabel);
            actTodayTop.Controls.Add(actTodayIconHost);
            actTodayTop.Dock = DockStyle.Top;
            actTodayTop.Location = new Point(0, 0);
            actTodayTop.Name = "actTodayTop";
            actTodayTop.Size = new Size(130, 34);
            actTodayTop.TabIndex = 2;
            // 
            // actTodayLabel
            // 
            actTodayLabel.BackColor = Color.White;
            actTodayLabel.Dock = DockStyle.Fill;
            actTodayLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actTodayLabel.ForeColor = Color.FromArgb(68, 83, 106);
            actTodayLabel.Location = new Point(0, 0);
            actTodayLabel.Name = "actTodayLabel";
            actTodayLabel.Size = new Size(90, 34);
            actTodayLabel.TabIndex = 0;
            actTodayLabel.Text = "Today";
            actTodayLabel.TextAlign = ContentAlignment.MiddleLeft;
            actTodayLabel.UseMnemonic = false;
            // 
            // actTodayIconHost
            // 
            actTodayIconHost.BackColor = Color.White;
            actTodayIconHost.Controls.Add(actTodayIcon);
            actTodayIconHost.Dock = DockStyle.Left;
            actTodayIconHost.Location = new Point(0, 0);
            actTodayIconHost.Name = "actTodayIconHost";
            actTodayIconHost.Size = new Size(38, 34);
            actTodayIconHost.TabIndex = 1;
            // 
            // actTodayIcon
            // 
            actTodayIcon.BackColor = Color.White;
            actTodayIcon.FillColor = Color.FromArgb(234, 242, 253);
            actTodayIcon.ForeColor = Color.FromArgb(58, 123, 213);
            actTodayIcon.Glyph = "\uE787";
            actTodayIcon.GlyphSize = 10F;
            actTodayIcon.Location = new Point(0, 2);
            actTodayIcon.Name = "actTodayIcon";
            actTodayIcon.Radius = 8;
            actTodayIcon.Size = new Size(30, 30);
            actTodayIcon.TabIndex = 0;
            actTodayIcon.TabStop = false;
            // 
            // actPendingTile
            // 
            actPendingTile.BorderColor = Color.FromArgb(227, 233, 242);
            actPendingTile.Controls.Add(actPendingDelta);
            actPendingTile.Controls.Add(actPendingValue);
            actPendingTile.Controls.Add(actPendingTop);
            actPendingTile.CornerColor = Color.FromArgb(243, 247, 252);
            actPendingTile.Dock = DockStyle.Fill;
            actPendingTile.FillColor = Color.White;
            actPendingTile.Location = new Point(0, 0);
            actPendingTile.Margin = new Padding(0, 0, 0, 0);
            actPendingTile.Name = "actPendingTile";
            actPendingTile.Padding = new Padding(12, 10, 12, 8);
            actPendingTile.Radius = 10;
            actPendingTile.Size = new Size(160, 100);
            actPendingTile.TabIndex = 2;
            // 
            // actPendingDelta
            // 
            actPendingDelta.AutoEllipsis = true;
            actPendingDelta.BackColor = Color.White;
            actPendingDelta.Dock = DockStyle.Top;
            actPendingDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actPendingDelta.ForeColor = Color.FromArgb(122, 136, 156);
            actPendingDelta.Location = new Point(0, 0);
            actPendingDelta.Name = "actPendingDelta";
            actPendingDelta.Size = new Size(130, 16);
            actPendingDelta.TabIndex = 0;
            actPendingDelta.Text = "\u2014 vs. last week";
            actPendingDelta.TextAlign = ContentAlignment.MiddleLeft;
            actPendingDelta.UseMnemonic = false;
            // 
            // actPendingValue
            // 
            actPendingValue.BackColor = Color.White;
            actPendingValue.Dock = DockStyle.Top;
            actPendingValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            actPendingValue.ForeColor = Color.FromArgb(31, 42, 60);
            actPendingValue.Location = new Point(0, 0);
            actPendingValue.Name = "actPendingValue";
            actPendingValue.Size = new Size(130, 28);
            actPendingValue.TabIndex = 1;
            actPendingValue.Text = "0";
            actPendingValue.TextAlign = ContentAlignment.MiddleLeft;
            actPendingValue.UseMnemonic = false;
            // 
            // actPendingTop
            // 
            actPendingTop.BackColor = Color.White;
            actPendingTop.Controls.Add(actPendingLabel);
            actPendingTop.Controls.Add(actPendingIconHost);
            actPendingTop.Dock = DockStyle.Top;
            actPendingTop.Location = new Point(0, 0);
            actPendingTop.Name = "actPendingTop";
            actPendingTop.Size = new Size(130, 34);
            actPendingTop.TabIndex = 2;
            // 
            // actPendingLabel
            // 
            actPendingLabel.BackColor = Color.White;
            actPendingLabel.Dock = DockStyle.Fill;
            actPendingLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actPendingLabel.ForeColor = Color.FromArgb(68, 83, 106);
            actPendingLabel.Location = new Point(0, 0);
            actPendingLabel.Name = "actPendingLabel";
            actPendingLabel.Size = new Size(90, 34);
            actPendingLabel.TabIndex = 0;
            actPendingLabel.Text = "Pending Actions";
            actPendingLabel.TextAlign = ContentAlignment.MiddleLeft;
            actPendingLabel.UseMnemonic = false;
            // 
            // actPendingIconHost
            // 
            actPendingIconHost.BackColor = Color.White;
            actPendingIconHost.Controls.Add(actPendingIcon);
            actPendingIconHost.Dock = DockStyle.Left;
            actPendingIconHost.Location = new Point(0, 0);
            actPendingIconHost.Name = "actPendingIconHost";
            actPendingIconHost.Size = new Size(38, 34);
            actPendingIconHost.TabIndex = 1;
            // 
            // actPendingIcon
            // 
            actPendingIcon.BackColor = Color.White;
            actPendingIcon.FillColor = Color.FromArgb(234, 242, 253);
            actPendingIcon.ForeColor = Color.FromArgb(58, 123, 213);
            actPendingIcon.Glyph = "\uE7BA";
            actPendingIcon.GlyphSize = 10F;
            actPendingIcon.Location = new Point(0, 2);
            actPendingIcon.Name = "actPendingIcon";
            actPendingIcon.Radius = 8;
            actPendingIcon.Size = new Size(30, 30);
            actPendingIcon.TabIndex = 0;
            actPendingIcon.TabStop = false;
            // 
            // actUsersTile
            // 
            actUsersTile.BorderColor = Color.FromArgb(227, 233, 242);
            actUsersTile.Controls.Add(actUsersDelta);
            actUsersTile.Controls.Add(actUsersValue);
            actUsersTile.Controls.Add(actUsersTop);
            actUsersTile.CornerColor = Color.FromArgb(243, 247, 252);
            actUsersTile.Dock = DockStyle.Fill;
            actUsersTile.FillColor = Color.White;
            actUsersTile.Location = new Point(0, 0);
            actUsersTile.Margin = new Padding(0, 0, 0, 0);
            actUsersTile.Name = "actUsersTile";
            actUsersTile.Padding = new Padding(12, 10, 12, 8);
            actUsersTile.Radius = 10;
            actUsersTile.Size = new Size(160, 100);
            actUsersTile.TabIndex = 3;
            // 
            // actUsersDelta
            // 
            actUsersDelta.AutoEllipsis = true;
            actUsersDelta.BackColor = Color.White;
            actUsersDelta.Dock = DockStyle.Top;
            actUsersDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actUsersDelta.ForeColor = Color.FromArgb(122, 136, 156);
            actUsersDelta.Location = new Point(0, 0);
            actUsersDelta.Name = "actUsersDelta";
            actUsersDelta.Size = new Size(130, 16);
            actUsersDelta.TabIndex = 0;
            actUsersDelta.Text = "\u2014 vs. last month";
            actUsersDelta.TextAlign = ContentAlignment.MiddleLeft;
            actUsersDelta.UseMnemonic = false;
            // 
            // actUsersValue
            // 
            actUsersValue.BackColor = Color.White;
            actUsersValue.Dock = DockStyle.Top;
            actUsersValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            actUsersValue.ForeColor = Color.FromArgb(31, 42, 60);
            actUsersValue.Location = new Point(0, 0);
            actUsersValue.Name = "actUsersValue";
            actUsersValue.Size = new Size(130, 28);
            actUsersValue.TabIndex = 1;
            actUsersValue.Text = "0";
            actUsersValue.TextAlign = ContentAlignment.MiddleLeft;
            actUsersValue.UseMnemonic = false;
            // 
            // actUsersTop
            // 
            actUsersTop.BackColor = Color.White;
            actUsersTop.Controls.Add(actUsersLabel);
            actUsersTop.Controls.Add(actUsersIconHost);
            actUsersTop.Dock = DockStyle.Top;
            actUsersTop.Location = new Point(0, 0);
            actUsersTop.Name = "actUsersTop";
            actUsersTop.Size = new Size(130, 34);
            actUsersTop.TabIndex = 2;
            // 
            // actUsersLabel
            // 
            actUsersLabel.BackColor = Color.White;
            actUsersLabel.Dock = DockStyle.Fill;
            actUsersLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            actUsersLabel.ForeColor = Color.FromArgb(68, 83, 106);
            actUsersLabel.Location = new Point(0, 0);
            actUsersLabel.Name = "actUsersLabel";
            actUsersLabel.Size = new Size(90, 34);
            actUsersLabel.TabIndex = 0;
            actUsersLabel.Text = "System Users";
            actUsersLabel.TextAlign = ContentAlignment.MiddleLeft;
            actUsersLabel.UseMnemonic = false;
            // 
            // actUsersIconHost
            // 
            actUsersIconHost.BackColor = Color.White;
            actUsersIconHost.Controls.Add(actUsersIcon);
            actUsersIconHost.Dock = DockStyle.Left;
            actUsersIconHost.Location = new Point(0, 0);
            actUsersIconHost.Name = "actUsersIconHost";
            actUsersIconHost.Size = new Size(38, 34);
            actUsersIconHost.TabIndex = 1;
            // 
            // actUsersIcon
            // 
            actUsersIcon.BackColor = Color.White;
            actUsersIcon.FillColor = Color.FromArgb(234, 242, 253);
            actUsersIcon.ForeColor = Color.FromArgb(58, 123, 213);
            actUsersIcon.Glyph = "\uE716";
            actUsersIcon.GlyphSize = 10F;
            actUsersIcon.Location = new Point(0, 2);
            actUsersIcon.Name = "actUsersIcon";
            actUsersIcon.Radius = 8;
            actUsersIcon.Size = new Size(30, 30);
            actUsersIcon.TabIndex = 0;
            actUsersIcon.TabStop = false;
            // 
            // gapOne
            // 
            gapOne.BackColor = Color.FromArgb(243, 247, 252);
            gapOne.Dock = DockStyle.Top;
            gapOne.Location = new Point(0, 0);
            gapOne.Name = "gapOne";
            gapOne.Size = new Size(10, 16);
            gapOne.TabIndex = 3;
            // 
            // overviewCard
            // 
            overviewCard.BorderColor = Color.FromArgb(227, 233, 242);
            overviewCard.Controls.Add(overviewHead);
            overviewCard.Controls.Add(overviewDateHost);
            overviewCard.CornerColor = Color.FromArgb(243, 247, 252);
            overviewCard.Dock = DockStyle.Top;
            overviewCard.FillColor = Color.White;
            overviewCard.Location = new Point(0, 0);
            overviewCard.Name = "overviewCard";
            overviewCard.Padding = new Padding(16, 12, 16, 8);
            overviewCard.Radius = 10;
            overviewCard.Size = new Size(700, 112);
            overviewCard.TabIndex = 4;
            // 
            // overviewHead
            // 
            overviewHead.BackColor = Color.White;
            overviewHead.Controls.Add(overviewText);
            overviewHead.Controls.Add(overviewIconHost);
            overviewHead.Dock = DockStyle.Fill;
            overviewHead.Location = new Point(0, 0);
            overviewHead.Name = "overviewHead";
            overviewHead.Size = new Size(668, 46);
            overviewHead.TabIndex = 0;
            // 
            // overviewText
            // 
            overviewText.BackColor = Color.White;
            overviewText.Controls.Add(overviewSubtitle);
            overviewText.Controls.Add(overviewTitle);
            overviewText.Dock = DockStyle.Fill;
            overviewText.Location = new Point(0, 0);
            overviewText.Name = "overviewText";
            overviewText.Size = new Size(500, 46);
            overviewText.TabIndex = 0;
            // 
            // overviewSubtitle
            // 
            overviewSubtitle.AutoEllipsis = true;
            overviewSubtitle.BackColor = Color.White;
            overviewSubtitle.Dock = DockStyle.Top;
            overviewSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            overviewSubtitle.ForeColor = Color.FromArgb(122, 136, 156);
            overviewSubtitle.Location = new Point(0, 0);
            overviewSubtitle.Name = "overviewSubtitle";
            overviewSubtitle.Size = new Size(500, 18);
            overviewSubtitle.TabIndex = 0;
            overviewSubtitle.Text = "Track system activities, today's actions, pending approvals, and active users.";
            overviewSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            overviewSubtitle.UseMnemonic = false;
            // 
            // overviewTitle
            // 
            overviewTitle.BackColor = Color.White;
            overviewTitle.Dock = DockStyle.Top;
            overviewTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            overviewTitle.ForeColor = Color.FromArgb(31, 42, 60);
            overviewTitle.Location = new Point(0, 0);
            overviewTitle.Name = "overviewTitle";
            overviewTitle.Size = new Size(500, 24);
            overviewTitle.TabIndex = 1;
            overviewTitle.Text = "Activity Overview";
            overviewTitle.TextAlign = ContentAlignment.MiddleLeft;
            overviewTitle.UseMnemonic = false;
            // 
            // overviewIconHost
            // 
            overviewIconHost.BackColor = Color.White;
            overviewIconHost.Controls.Add(overviewIcon);
            overviewIconHost.Dock = DockStyle.Left;
            overviewIconHost.Location = new Point(0, 0);
            overviewIconHost.Name = "overviewIconHost";
            overviewIconHost.Size = new Size(56, 46);
            overviewIconHost.TabIndex = 1;
            // 
            // overviewIcon
            // 
            overviewIcon.BackColor = Color.White;
            overviewIcon.FillColor = Color.FromArgb(234, 242, 253);
            overviewIcon.ForeColor = Color.FromArgb(58, 123, 213);
            overviewIcon.Glyph = "\uE823";
            overviewIcon.GlyphSize = 14F;
            overviewIcon.Location = new Point(0, 3);
            overviewIcon.Name = "overviewIcon";
            overviewIcon.Radius = 10;
            overviewIcon.Size = new Size(40, 40);
            overviewIcon.TabIndex = 0;
            overviewIcon.TabStop = false;
            // 
            // overviewDateHost
            // 
            overviewDateHost.BackColor = Color.White;
            overviewDateHost.Controls.Add(overviewDateRange);
            overviewDateHost.Dock = DockStyle.Bottom;
            overviewDateHost.Location = new Point(0, 0);
            overviewDateHost.Name = "overviewDateHost";
            overviewDateHost.Padding = new Padding(56, 8, 0, 2);
            overviewDateHost.Size = new Size(668, 42);
            overviewDateHost.TabIndex = 1;
            // 
            // overviewDateRange
            // 
            overviewDateRange.BackColor = Color.White;
            overviewDateRange.Dock = DockStyle.Left;
            overviewDateRange.Location = new Point(56, 8);
            overviewDateRange.Name = "overviewDateRange";
            overviewDateRange.Size = new Size(214, 32);
            overviewDateRange.TabIndex = 0;
            overviewDateRange.Text = "All dates";
            // 
            // rightColumn
            // 
            rightColumn.BackColor = Color.FromArgb(243, 247, 252);
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightColumn.Controls.Add(quickFiltersCard, 0, 0);
            rightColumn.Controls.Add(recentActivityCard, 0, 1);
            rightColumn.Controls.Add(auditSummaryCard, 0, 2);
            rightColumn.Controls.Add(exportCard, 0, 3);
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(0, 0);
            rightColumn.Margin = new Padding(0, 0, 0, 0);
            rightColumn.Name = "rightColumn";
            rightColumn.RowCount = 4;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 206F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 138F));
            rightColumn.Size = new Size(280, 692);
            rightColumn.TabIndex = 1;
            // 
            // quickFiltersCard
            // 
            quickFiltersCard.BorderColor = Color.FromArgb(227, 233, 242);
            quickFiltersCard.Controls.Add(loginRow);
            quickFiltersCard.Controls.Add(criticalRow);
            quickFiltersCard.Controls.Add(weekRow);
            quickFiltersCard.Controls.Add(todayRow);
            quickFiltersCard.Controls.Add(quickFiltersHeader);
            quickFiltersCard.CornerColor = Color.FromArgb(243, 247, 252);
            quickFiltersCard.Dock = DockStyle.Fill;
            quickFiltersCard.FillColor = Color.White;
            quickFiltersCard.Location = new Point(0, 0);
            quickFiltersCard.Margin = new Padding(0, 0, 0, 16);
            quickFiltersCard.Name = "quickFiltersCard";
            quickFiltersCard.Padding = new Padding(16, 12, 16, 12);
            quickFiltersCard.Radius = 10;
            quickFiltersCard.Size = new Size(280, 190);
            quickFiltersCard.TabIndex = 0;
            // 
            // loginRow
            // 
            loginRow.BackColor = Color.White;
            loginRow.Controls.Add(loginName);
            loginRow.Controls.Add(loginIconHost);
            loginRow.Controls.Add(loginCountHost);
            loginRow.Controls.Add(loginChevron);
            loginRow.Cursor = Cursors.Hand;
            loginRow.Dock = DockStyle.Top;
            loginRow.Location = new Point(0, 0);
            loginRow.Name = "loginRow";
            loginRow.Size = new Size(248, 34);
            loginRow.TabIndex = 0;
            // 
            // loginName
            // 
            loginName.BackColor = Color.White;
            loginName.Cursor = Cursors.Hand;
            loginName.Dock = DockStyle.Fill;
            loginName.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            loginName.ForeColor = Color.FromArgb(68, 83, 106);
            loginName.Location = new Point(0, 0);
            loginName.Name = "loginName";
            loginName.Size = new Size(120, 34);
            loginName.TabIndex = 0;
            loginName.Text = "Login Activity";
            loginName.TextAlign = ContentAlignment.MiddleLeft;
            loginName.UseMnemonic = false;
            // 
            // loginIconHost
            // 
            loginIconHost.BackColor = Color.White;
            loginIconHost.Controls.Add(loginIcon);
            loginIconHost.Cursor = Cursors.Hand;
            loginIconHost.Dock = DockStyle.Left;
            loginIconHost.Location = new Point(0, 0);
            loginIconHost.Name = "loginIconHost";
            loginIconHost.Size = new Size(36, 34);
            loginIconHost.TabIndex = 1;
            // 
            // loginIcon
            // 
            loginIcon.BackColor = Color.White;
            loginIcon.FillColor = Color.FromArgb(234, 242, 253);
            loginIcon.ForeColor = Color.FromArgb(58, 123, 213);
            loginIcon.Glyph = "\uE77B";
            loginIcon.GlyphSize = 9F;
            loginIcon.Location = new Point(0, 5);
            loginIcon.Name = "loginIcon";
            loginIcon.Radius = 6;
            loginIcon.Size = new Size(24, 24);
            loginIcon.TabIndex = 0;
            loginIcon.TabStop = false;
            // 
            // loginCountHost
            // 
            loginCountHost.BackColor = Color.White;
            loginCountHost.Controls.Add(loginCount);
            loginCountHost.Cursor = Cursors.Hand;
            loginCountHost.Dock = DockStyle.Right;
            loginCountHost.Location = new Point(0, 0);
            loginCountHost.Name = "loginCountHost";
            loginCountHost.Padding = new Padding(6, 7, 4, 7);
            loginCountHost.Size = new Size(44, 34);
            loginCountHost.TabIndex = 2;
            // 
            // loginCount
            // 
            loginCount.BackColor = Color.FromArgb(246, 248, 251);
            loginCount.Cursor = Cursors.Hand;
            loginCount.Dock = DockStyle.Fill;
            loginCount.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            loginCount.ForeColor = Color.FromArgb(68, 83, 106);
            loginCount.Location = new Point(0, 0);
            loginCount.Name = "loginCount";
            loginCount.Size = new Size(34, 20);
            loginCount.TabIndex = 0;
            loginCount.Text = "0";
            loginCount.TextAlign = ContentAlignment.MiddleCenter;
            loginCount.UseMnemonic = false;
            // 
            // loginChevron
            // 
            loginChevron.BackColor = Color.White;
            loginChevron.Dock = DockStyle.Right;
            loginChevron.Font = new Font("Segoe MDL2 Assets", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            loginChevron.ForeColor = Color.FromArgb(122, 136, 156);
            loginChevron.Location = new Point(0, 0);
            loginChevron.Name = "loginChevron";
            loginChevron.Size = new Size(16, 34);
            loginChevron.TabIndex = 3;
            loginChevron.Text = "\uE76C";
            loginChevron.TextAlign = ContentAlignment.MiddleCenter;
            loginChevron.UseMnemonic = false;
            // 
            // criticalRow
            // 
            criticalRow.BackColor = Color.White;
            criticalRow.Controls.Add(criticalName);
            criticalRow.Controls.Add(criticalIconHost);
            criticalRow.Controls.Add(criticalCountHost);
            criticalRow.Controls.Add(criticalChevron);
            criticalRow.Cursor = Cursors.Hand;
            criticalRow.Dock = DockStyle.Top;
            criticalRow.Location = new Point(0, 0);
            criticalRow.Name = "criticalRow";
            criticalRow.Size = new Size(248, 34);
            criticalRow.TabIndex = 1;
            // 
            // criticalName
            // 
            criticalName.BackColor = Color.White;
            criticalName.Cursor = Cursors.Hand;
            criticalName.Dock = DockStyle.Fill;
            criticalName.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            criticalName.ForeColor = Color.FromArgb(68, 83, 106);
            criticalName.Location = new Point(0, 0);
            criticalName.Name = "criticalName";
            criticalName.Size = new Size(120, 34);
            criticalName.TabIndex = 0;
            criticalName.Text = "Critical Actions";
            criticalName.TextAlign = ContentAlignment.MiddleLeft;
            criticalName.UseMnemonic = false;
            // 
            // criticalIconHost
            // 
            criticalIconHost.BackColor = Color.White;
            criticalIconHost.Controls.Add(criticalIcon);
            criticalIconHost.Cursor = Cursors.Hand;
            criticalIconHost.Dock = DockStyle.Left;
            criticalIconHost.Location = new Point(0, 0);
            criticalIconHost.Name = "criticalIconHost";
            criticalIconHost.Size = new Size(36, 34);
            criticalIconHost.TabIndex = 1;
            // 
            // criticalIcon
            // 
            criticalIcon.BackColor = Color.White;
            criticalIcon.FillColor = Color.FromArgb(234, 242, 253);
            criticalIcon.ForeColor = Color.FromArgb(58, 123, 213);
            criticalIcon.Glyph = "\uE7BA";
            criticalIcon.GlyphSize = 9F;
            criticalIcon.Location = new Point(0, 5);
            criticalIcon.Name = "criticalIcon";
            criticalIcon.Radius = 6;
            criticalIcon.Size = new Size(24, 24);
            criticalIcon.TabIndex = 0;
            criticalIcon.TabStop = false;
            // 
            // criticalCountHost
            // 
            criticalCountHost.BackColor = Color.White;
            criticalCountHost.Controls.Add(criticalCount);
            criticalCountHost.Cursor = Cursors.Hand;
            criticalCountHost.Dock = DockStyle.Right;
            criticalCountHost.Location = new Point(0, 0);
            criticalCountHost.Name = "criticalCountHost";
            criticalCountHost.Padding = new Padding(6, 7, 4, 7);
            criticalCountHost.Size = new Size(44, 34);
            criticalCountHost.TabIndex = 2;
            // 
            // criticalCount
            // 
            criticalCount.BackColor = Color.FromArgb(246, 248, 251);
            criticalCount.Cursor = Cursors.Hand;
            criticalCount.Dock = DockStyle.Fill;
            criticalCount.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            criticalCount.ForeColor = Color.FromArgb(68, 83, 106);
            criticalCount.Location = new Point(0, 0);
            criticalCount.Name = "criticalCount";
            criticalCount.Size = new Size(34, 20);
            criticalCount.TabIndex = 0;
            criticalCount.Text = "0";
            criticalCount.TextAlign = ContentAlignment.MiddleCenter;
            criticalCount.UseMnemonic = false;
            // 
            // criticalChevron
            // 
            criticalChevron.BackColor = Color.White;
            criticalChevron.Dock = DockStyle.Right;
            criticalChevron.Font = new Font("Segoe MDL2 Assets", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            criticalChevron.ForeColor = Color.FromArgb(122, 136, 156);
            criticalChevron.Location = new Point(0, 0);
            criticalChevron.Name = "criticalChevron";
            criticalChevron.Size = new Size(16, 34);
            criticalChevron.TabIndex = 3;
            criticalChevron.Text = "\uE76C";
            criticalChevron.TextAlign = ContentAlignment.MiddleCenter;
            criticalChevron.UseMnemonic = false;
            // 
            // weekRow
            // 
            weekRow.BackColor = Color.White;
            weekRow.Controls.Add(weekName);
            weekRow.Controls.Add(weekIconHost);
            weekRow.Controls.Add(weekCountHost);
            weekRow.Controls.Add(weekChevron);
            weekRow.Cursor = Cursors.Hand;
            weekRow.Dock = DockStyle.Top;
            weekRow.Location = new Point(0, 0);
            weekRow.Name = "weekRow";
            weekRow.Size = new Size(248, 34);
            weekRow.TabIndex = 2;
            // 
            // weekName
            // 
            weekName.BackColor = Color.White;
            weekName.Cursor = Cursors.Hand;
            weekName.Dock = DockStyle.Fill;
            weekName.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            weekName.ForeColor = Color.FromArgb(68, 83, 106);
            weekName.Location = new Point(0, 0);
            weekName.Name = "weekName";
            weekName.Size = new Size(120, 34);
            weekName.TabIndex = 0;
            weekName.Text = "This Week";
            weekName.TextAlign = ContentAlignment.MiddleLeft;
            weekName.UseMnemonic = false;
            // 
            // weekIconHost
            // 
            weekIconHost.BackColor = Color.White;
            weekIconHost.Controls.Add(weekIcon);
            weekIconHost.Cursor = Cursors.Hand;
            weekIconHost.Dock = DockStyle.Left;
            weekIconHost.Location = new Point(0, 0);
            weekIconHost.Name = "weekIconHost";
            weekIconHost.Size = new Size(36, 34);
            weekIconHost.TabIndex = 1;
            // 
            // weekIcon
            // 
            weekIcon.BackColor = Color.White;
            weekIcon.FillColor = Color.FromArgb(234, 242, 253);
            weekIcon.ForeColor = Color.FromArgb(58, 123, 213);
            weekIcon.Glyph = "\uE787";
            weekIcon.GlyphSize = 9F;
            weekIcon.Location = new Point(0, 5);
            weekIcon.Name = "weekIcon";
            weekIcon.Radius = 6;
            weekIcon.Size = new Size(24, 24);
            weekIcon.TabIndex = 0;
            weekIcon.TabStop = false;
            // 
            // weekCountHost
            // 
            weekCountHost.BackColor = Color.White;
            weekCountHost.Controls.Add(weekCount);
            weekCountHost.Cursor = Cursors.Hand;
            weekCountHost.Dock = DockStyle.Right;
            weekCountHost.Location = new Point(0, 0);
            weekCountHost.Name = "weekCountHost";
            weekCountHost.Padding = new Padding(6, 7, 4, 7);
            weekCountHost.Size = new Size(44, 34);
            weekCountHost.TabIndex = 2;
            // 
            // weekCount
            // 
            weekCount.BackColor = Color.FromArgb(246, 248, 251);
            weekCount.Cursor = Cursors.Hand;
            weekCount.Dock = DockStyle.Fill;
            weekCount.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            weekCount.ForeColor = Color.FromArgb(68, 83, 106);
            weekCount.Location = new Point(0, 0);
            weekCount.Name = "weekCount";
            weekCount.Size = new Size(34, 20);
            weekCount.TabIndex = 0;
            weekCount.Text = "0";
            weekCount.TextAlign = ContentAlignment.MiddleCenter;
            weekCount.UseMnemonic = false;
            // 
            // weekChevron
            // 
            weekChevron.BackColor = Color.White;
            weekChevron.Dock = DockStyle.Right;
            weekChevron.Font = new Font("Segoe MDL2 Assets", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            weekChevron.ForeColor = Color.FromArgb(122, 136, 156);
            weekChevron.Location = new Point(0, 0);
            weekChevron.Name = "weekChevron";
            weekChevron.Size = new Size(16, 34);
            weekChevron.TabIndex = 3;
            weekChevron.Text = "\uE76C";
            weekChevron.TextAlign = ContentAlignment.MiddleCenter;
            weekChevron.UseMnemonic = false;
            // 
            // todayRow
            // 
            todayRow.BackColor = Color.White;
            todayRow.Controls.Add(todayName);
            todayRow.Controls.Add(todayIconHost);
            todayRow.Controls.Add(todayCountHost);
            todayRow.Controls.Add(todayChevron);
            todayRow.Cursor = Cursors.Hand;
            todayRow.Dock = DockStyle.Top;
            todayRow.Location = new Point(0, 0);
            todayRow.Name = "todayRow";
            todayRow.Size = new Size(248, 34);
            todayRow.TabIndex = 3;
            // 
            // todayName
            // 
            todayName.BackColor = Color.White;
            todayName.Cursor = Cursors.Hand;
            todayName.Dock = DockStyle.Fill;
            todayName.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            todayName.ForeColor = Color.FromArgb(68, 83, 106);
            todayName.Location = new Point(0, 0);
            todayName.Name = "todayName";
            todayName.Size = new Size(120, 34);
            todayName.TabIndex = 0;
            todayName.Text = "Today";
            todayName.TextAlign = ContentAlignment.MiddleLeft;
            todayName.UseMnemonic = false;
            // 
            // todayIconHost
            // 
            todayIconHost.BackColor = Color.White;
            todayIconHost.Controls.Add(todayIcon);
            todayIconHost.Cursor = Cursors.Hand;
            todayIconHost.Dock = DockStyle.Left;
            todayIconHost.Location = new Point(0, 0);
            todayIconHost.Name = "todayIconHost";
            todayIconHost.Size = new Size(36, 34);
            todayIconHost.TabIndex = 1;
            // 
            // todayIcon
            // 
            todayIcon.BackColor = Color.White;
            todayIcon.FillColor = Color.FromArgb(234, 242, 253);
            todayIcon.ForeColor = Color.FromArgb(58, 123, 213);
            todayIcon.Glyph = "\uE787";
            todayIcon.GlyphSize = 9F;
            todayIcon.Location = new Point(0, 5);
            todayIcon.Name = "todayIcon";
            todayIcon.Radius = 6;
            todayIcon.Size = new Size(24, 24);
            todayIcon.TabIndex = 0;
            todayIcon.TabStop = false;
            // 
            // todayCountHost
            // 
            todayCountHost.BackColor = Color.White;
            todayCountHost.Controls.Add(todayCount);
            todayCountHost.Cursor = Cursors.Hand;
            todayCountHost.Dock = DockStyle.Right;
            todayCountHost.Location = new Point(0, 0);
            todayCountHost.Name = "todayCountHost";
            todayCountHost.Padding = new Padding(6, 7, 4, 7);
            todayCountHost.Size = new Size(44, 34);
            todayCountHost.TabIndex = 2;
            // 
            // todayCount
            // 
            todayCount.BackColor = Color.FromArgb(246, 248, 251);
            todayCount.Cursor = Cursors.Hand;
            todayCount.Dock = DockStyle.Fill;
            todayCount.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            todayCount.ForeColor = Color.FromArgb(68, 83, 106);
            todayCount.Location = new Point(0, 0);
            todayCount.Name = "todayCount";
            todayCount.Size = new Size(34, 20);
            todayCount.TabIndex = 0;
            todayCount.Text = "0";
            todayCount.TextAlign = ContentAlignment.MiddleCenter;
            todayCount.UseMnemonic = false;
            // 
            // todayChevron
            // 
            todayChevron.BackColor = Color.White;
            todayChevron.Dock = DockStyle.Right;
            todayChevron.Font = new Font("Segoe MDL2 Assets", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            todayChevron.ForeColor = Color.FromArgb(122, 136, 156);
            todayChevron.Location = new Point(0, 0);
            todayChevron.Name = "todayChevron";
            todayChevron.Size = new Size(16, 34);
            todayChevron.TabIndex = 3;
            todayChevron.Text = "\uE76C";
            todayChevron.TextAlign = ContentAlignment.MiddleCenter;
            todayChevron.UseMnemonic = false;
            // 
            // quickFiltersHeader
            // 
            quickFiltersHeader.BackColor = Color.White;
            quickFiltersHeader.Controls.Add(quickFiltersTitle);
            quickFiltersHeader.Controls.Add(quickFiltersIcon);
            quickFiltersHeader.Controls.Add(quickFiltersLink);
            quickFiltersHeader.Dock = DockStyle.Top;
            quickFiltersHeader.Location = new Point(0, 0);
            quickFiltersHeader.Name = "quickFiltersHeader";
            quickFiltersHeader.Size = new Size(248, 34);
            quickFiltersHeader.TabIndex = 4;
            // 
            // quickFiltersTitle
            // 
            quickFiltersTitle.BackColor = Color.White;
            quickFiltersTitle.Dock = DockStyle.Fill;
            quickFiltersTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickFiltersTitle.ForeColor = Color.FromArgb(31, 42, 60);
            quickFiltersTitle.Location = new Point(26, 0);
            quickFiltersTitle.Name = "quickFiltersTitle";
            quickFiltersTitle.Size = new Size(160, 34);
            quickFiltersTitle.TabIndex = 0;
            quickFiltersTitle.Text = "Quick Filters";
            quickFiltersTitle.TextAlign = ContentAlignment.MiddleLeft;
            quickFiltersTitle.UseMnemonic = false;
            // 
            // quickFiltersIcon
            // 
            quickFiltersIcon.BackColor = Color.White;
            quickFiltersIcon.Dock = DockStyle.Left;
            quickFiltersIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            quickFiltersIcon.ForeColor = Color.FromArgb(58, 123, 213);
            quickFiltersIcon.Location = new Point(0, 0);
            quickFiltersIcon.Name = "quickFiltersIcon";
            quickFiltersIcon.Size = new Size(26, 34);
            quickFiltersIcon.TabIndex = 1;
            quickFiltersIcon.Text = "\uE71C";
            quickFiltersIcon.TextAlign = ContentAlignment.MiddleCenter;
            quickFiltersIcon.UseMnemonic = false;
            // 
            // quickFiltersLink
            // 
            quickFiltersLink.BackColor = Color.White;
            quickFiltersLink.Cursor = Cursors.Hand;
            quickFiltersLink.Dock = DockStyle.Right;
            quickFiltersLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickFiltersLink.ForeColor = Color.FromArgb(58, 123, 213);
            quickFiltersLink.Location = new Point(0, 0);
            quickFiltersLink.Name = "quickFiltersLink";
            quickFiltersLink.Size = new Size(62, 34);
            quickFiltersLink.TabIndex = 2;
            quickFiltersLink.Text = "View All \u2192";
            quickFiltersLink.TextAlign = ContentAlignment.MiddleRight;
            quickFiltersLink.UseMnemonic = false;
            // 
            // recentActivityCard
            // 
            recentActivityCard.BorderColor = Color.FromArgb(227, 233, 242);
            recentActivityCard.Controls.Add(recentActivityEmpty);
            recentActivityCard.Controls.Add(recentActivityHeader);
            recentActivityCard.CornerColor = Color.FromArgb(243, 247, 252);
            recentActivityCard.Dock = DockStyle.Fill;
            recentActivityCard.FillColor = Color.White;
            recentActivityCard.Location = new Point(0, 0);
            recentActivityCard.Margin = new Padding(0, 0, 0, 16);
            recentActivityCard.Name = "recentActivityCard";
            recentActivityCard.Padding = new Padding(16, 12, 16, 12);
            recentActivityCard.Radius = 10;
            recentActivityCard.Size = new Size(280, 200);
            recentActivityCard.TabIndex = 1;
            // 
            // recentActivityEmpty
            // 
            recentActivityEmpty.BackColor = Color.White;
            recentActivityEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            recentActivityEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            recentActivityEmpty.Dock = DockStyle.Fill;
            recentActivityEmpty.Glyph = "\uE81C";
            recentActivityEmpty.Hint = "The latest system actions will appear here.";
            recentActivityEmpty.Location = new Point(0, 0);
            recentActivityEmpty.Name = "recentActivityEmpty";
            recentActivityEmpty.Size = new Size(248, 140);
            recentActivityEmpty.TabIndex = 0;
            recentActivityEmpty.TabStop = false;
            recentActivityEmpty.Title = "No recent activity";
            // 
            // recentActivityHeader
            // 
            recentActivityHeader.BackColor = Color.White;
            recentActivityHeader.Controls.Add(recentActivityTitle);
            recentActivityHeader.Controls.Add(recentActivityIcon);
            recentActivityHeader.Controls.Add(recentActivityLink);
            recentActivityHeader.Dock = DockStyle.Top;
            recentActivityHeader.Location = new Point(0, 0);
            recentActivityHeader.Name = "recentActivityHeader";
            recentActivityHeader.Size = new Size(248, 34);
            recentActivityHeader.TabIndex = 1;
            // 
            // recentActivityTitle
            // 
            recentActivityTitle.BackColor = Color.White;
            recentActivityTitle.Dock = DockStyle.Fill;
            recentActivityTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recentActivityTitle.ForeColor = Color.FromArgb(31, 42, 60);
            recentActivityTitle.Location = new Point(26, 0);
            recentActivityTitle.Name = "recentActivityTitle";
            recentActivityTitle.Size = new Size(160, 34);
            recentActivityTitle.TabIndex = 0;
            recentActivityTitle.Text = "Recent Activity";
            recentActivityTitle.TextAlign = ContentAlignment.MiddleLeft;
            recentActivityTitle.UseMnemonic = false;
            // 
            // recentActivityIcon
            // 
            recentActivityIcon.BackColor = Color.White;
            recentActivityIcon.Dock = DockStyle.Left;
            recentActivityIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            recentActivityIcon.ForeColor = Color.FromArgb(58, 123, 213);
            recentActivityIcon.Location = new Point(0, 0);
            recentActivityIcon.Name = "recentActivityIcon";
            recentActivityIcon.Size = new Size(26, 34);
            recentActivityIcon.TabIndex = 1;
            recentActivityIcon.Text = "\uE823";
            recentActivityIcon.TextAlign = ContentAlignment.MiddleCenter;
            recentActivityIcon.UseMnemonic = false;
            // 
            // recentActivityLink
            // 
            recentActivityLink.BackColor = Color.White;
            recentActivityLink.Cursor = Cursors.Hand;
            recentActivityLink.Dock = DockStyle.Right;
            recentActivityLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recentActivityLink.ForeColor = Color.FromArgb(58, 123, 213);
            recentActivityLink.Location = new Point(0, 0);
            recentActivityLink.Name = "recentActivityLink";
            recentActivityLink.Size = new Size(62, 34);
            recentActivityLink.TabIndex = 2;
            recentActivityLink.Text = "View All \u2192";
            recentActivityLink.TextAlign = ContentAlignment.MiddleRight;
            recentActivityLink.UseMnemonic = false;
            // 
            // auditSummaryCard
            // 
            auditSummaryCard.BorderColor = Color.FromArgb(227, 233, 242);
            auditSummaryCard.Controls.Add(summaryGrid);
            auditSummaryCard.Controls.Add(auditSummaryHeader);
            auditSummaryCard.CornerColor = Color.FromArgb(243, 247, 252);
            auditSummaryCard.Dock = DockStyle.Fill;
            auditSummaryCard.FillColor = Color.White;
            auditSummaryCard.Location = new Point(0, 0);
            auditSummaryCard.Margin = new Padding(0, 0, 0, 16);
            auditSummaryCard.Name = "auditSummaryCard";
            auditSummaryCard.Padding = new Padding(16, 12, 16, 12);
            auditSummaryCard.Radius = 10;
            auditSummaryCard.Size = new Size(280, 116);
            auditSummaryCard.TabIndex = 2;
            // 
            // summaryGrid
            // 
            summaryGrid.BackColor = Color.White;
            summaryGrid.ColumnCount = 7;
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryGrid.Controls.Add(createdTile, 0, 0);
            summaryGrid.Controls.Add(updatedTile, 2, 0);
            summaryGrid.Controls.Add(approvedTile, 4, 0);
            summaryGrid.Controls.Add(deletedTile, 6, 0);
            summaryGrid.Dock = DockStyle.Fill;
            summaryGrid.Location = new Point(0, 0);
            summaryGrid.Margin = new Padding(0, 0, 0, 0);
            summaryGrid.Name = "summaryGrid";
            summaryGrid.RowCount = 1;
            summaryGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            summaryGrid.Size = new Size(248, 62);
            summaryGrid.TabIndex = 0;
            // 
            // createdTile
            // 
            createdTile.BorderColor = Color.FromArgb(227, 233, 242);
            createdTile.Controls.Add(createdIcon);
            createdTile.Controls.Add(createdLabel);
            createdTile.Controls.Add(createdValue);
            createdTile.CornerColor = Color.White;
            createdTile.Dock = DockStyle.Fill;
            createdTile.FillColor = Color.White;
            createdTile.Location = new Point(0, 0);
            createdTile.Margin = new Padding(0, 0, 0, 0);
            createdTile.Name = "createdTile";
            createdTile.Padding = new Padding(2, 4, 2, 4);
            createdTile.Radius = 8;
            createdTile.Size = new Size(52, 62);
            createdTile.TabIndex = 0;
            // 
            // createdIcon
            // 
            createdIcon.BackColor = Color.White;
            createdIcon.Dock = DockStyle.Fill;
            createdIcon.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            createdIcon.ForeColor = Color.FromArgb(46, 158, 91);
            createdIcon.Location = new Point(0, 0);
            createdIcon.Name = "createdIcon";
            createdIcon.Size = new Size(50, 20);
            createdIcon.TabIndex = 0;
            createdIcon.Text = "\uE710";
            createdIcon.TextAlign = ContentAlignment.MiddleCenter;
            createdIcon.UseMnemonic = false;
            // 
            // createdLabel
            // 
            createdLabel.BackColor = Color.White;
            createdLabel.Dock = DockStyle.Bottom;
            createdLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            createdLabel.ForeColor = Color.FromArgb(122, 136, 156);
            createdLabel.Location = new Point(0, 0);
            createdLabel.Name = "createdLabel";
            createdLabel.Size = new Size(50, 14);
            createdLabel.TabIndex = 1;
            createdLabel.Text = "Created";
            createdLabel.TextAlign = ContentAlignment.MiddleCenter;
            createdLabel.UseMnemonic = false;
            // 
            // createdValue
            // 
            createdValue.BackColor = Color.White;
            createdValue.Dock = DockStyle.Bottom;
            createdValue.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            createdValue.ForeColor = Color.FromArgb(31, 42, 60);
            createdValue.Location = new Point(0, 0);
            createdValue.Name = "createdValue";
            createdValue.Size = new Size(50, 18);
            createdValue.TabIndex = 2;
            createdValue.Text = "0";
            createdValue.TextAlign = ContentAlignment.MiddleCenter;
            createdValue.UseMnemonic = false;
            // 
            // updatedTile
            // 
            updatedTile.BorderColor = Color.FromArgb(227, 233, 242);
            updatedTile.Controls.Add(updatedIcon);
            updatedTile.Controls.Add(updatedLabel);
            updatedTile.Controls.Add(updatedValue);
            updatedTile.CornerColor = Color.White;
            updatedTile.Dock = DockStyle.Fill;
            updatedTile.FillColor = Color.White;
            updatedTile.Location = new Point(0, 0);
            updatedTile.Margin = new Padding(0, 0, 0, 0);
            updatedTile.Name = "updatedTile";
            updatedTile.Padding = new Padding(2, 4, 2, 4);
            updatedTile.Radius = 8;
            updatedTile.Size = new Size(52, 62);
            updatedTile.TabIndex = 1;
            // 
            // updatedIcon
            // 
            updatedIcon.BackColor = Color.White;
            updatedIcon.Dock = DockStyle.Fill;
            updatedIcon.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            updatedIcon.ForeColor = Color.FromArgb(58, 123, 213);
            updatedIcon.Location = new Point(0, 0);
            updatedIcon.Name = "updatedIcon";
            updatedIcon.Size = new Size(50, 20);
            updatedIcon.TabIndex = 0;
            updatedIcon.Text = "\uE72C";
            updatedIcon.TextAlign = ContentAlignment.MiddleCenter;
            updatedIcon.UseMnemonic = false;
            // 
            // updatedLabel
            // 
            updatedLabel.BackColor = Color.White;
            updatedLabel.Dock = DockStyle.Bottom;
            updatedLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            updatedLabel.ForeColor = Color.FromArgb(122, 136, 156);
            updatedLabel.Location = new Point(0, 0);
            updatedLabel.Name = "updatedLabel";
            updatedLabel.Size = new Size(50, 14);
            updatedLabel.TabIndex = 1;
            updatedLabel.Text = "Updated";
            updatedLabel.TextAlign = ContentAlignment.MiddleCenter;
            updatedLabel.UseMnemonic = false;
            // 
            // updatedValue
            // 
            updatedValue.BackColor = Color.White;
            updatedValue.Dock = DockStyle.Bottom;
            updatedValue.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            updatedValue.ForeColor = Color.FromArgb(31, 42, 60);
            updatedValue.Location = new Point(0, 0);
            updatedValue.Name = "updatedValue";
            updatedValue.Size = new Size(50, 18);
            updatedValue.TabIndex = 2;
            updatedValue.Text = "0";
            updatedValue.TextAlign = ContentAlignment.MiddleCenter;
            updatedValue.UseMnemonic = false;
            // 
            // approvedTile
            // 
            approvedTile.BorderColor = Color.FromArgb(227, 233, 242);
            approvedTile.Controls.Add(approvedIcon);
            approvedTile.Controls.Add(approvedLabel);
            approvedTile.Controls.Add(approvedValue);
            approvedTile.CornerColor = Color.White;
            approvedTile.Dock = DockStyle.Fill;
            approvedTile.FillColor = Color.White;
            approvedTile.Location = new Point(0, 0);
            approvedTile.Margin = new Padding(0, 0, 0, 0);
            approvedTile.Name = "approvedTile";
            approvedTile.Padding = new Padding(2, 4, 2, 4);
            approvedTile.Radius = 8;
            approvedTile.Size = new Size(52, 62);
            approvedTile.TabIndex = 2;
            // 
            // approvedIcon
            // 
            approvedIcon.BackColor = Color.White;
            approvedIcon.Dock = DockStyle.Fill;
            approvedIcon.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            approvedIcon.ForeColor = Color.FromArgb(46, 158, 91);
            approvedIcon.Location = new Point(0, 0);
            approvedIcon.Name = "approvedIcon";
            approvedIcon.Size = new Size(50, 20);
            approvedIcon.TabIndex = 0;
            approvedIcon.Text = "\uE73E";
            approvedIcon.TextAlign = ContentAlignment.MiddleCenter;
            approvedIcon.UseMnemonic = false;
            // 
            // approvedLabel
            // 
            approvedLabel.BackColor = Color.White;
            approvedLabel.Dock = DockStyle.Bottom;
            approvedLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            approvedLabel.ForeColor = Color.FromArgb(122, 136, 156);
            approvedLabel.Location = new Point(0, 0);
            approvedLabel.Name = "approvedLabel";
            approvedLabel.Size = new Size(50, 14);
            approvedLabel.TabIndex = 1;
            approvedLabel.Text = "Approved";
            approvedLabel.TextAlign = ContentAlignment.MiddleCenter;
            approvedLabel.UseMnemonic = false;
            // 
            // approvedValue
            // 
            approvedValue.BackColor = Color.White;
            approvedValue.Dock = DockStyle.Bottom;
            approvedValue.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            approvedValue.ForeColor = Color.FromArgb(31, 42, 60);
            approvedValue.Location = new Point(0, 0);
            approvedValue.Name = "approvedValue";
            approvedValue.Size = new Size(50, 18);
            approvedValue.TabIndex = 2;
            approvedValue.Text = "0";
            approvedValue.TextAlign = ContentAlignment.MiddleCenter;
            approvedValue.UseMnemonic = false;
            // 
            // deletedTile
            // 
            deletedTile.BorderColor = Color.FromArgb(227, 233, 242);
            deletedTile.Controls.Add(deletedIcon);
            deletedTile.Controls.Add(deletedLabel);
            deletedTile.Controls.Add(deletedValue);
            deletedTile.CornerColor = Color.White;
            deletedTile.Dock = DockStyle.Fill;
            deletedTile.FillColor = Color.White;
            deletedTile.Location = new Point(0, 0);
            deletedTile.Margin = new Padding(0, 0, 0, 0);
            deletedTile.Name = "deletedTile";
            deletedTile.Padding = new Padding(2, 4, 2, 4);
            deletedTile.Radius = 8;
            deletedTile.Size = new Size(52, 62);
            deletedTile.TabIndex = 3;
            // 
            // deletedIcon
            // 
            deletedIcon.BackColor = Color.White;
            deletedIcon.Dock = DockStyle.Fill;
            deletedIcon.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            deletedIcon.ForeColor = Color.FromArgb(214, 69, 65);
            deletedIcon.Location = new Point(0, 0);
            deletedIcon.Name = "deletedIcon";
            deletedIcon.Size = new Size(50, 20);
            deletedIcon.TabIndex = 0;
            deletedIcon.Text = "\uE74D";
            deletedIcon.TextAlign = ContentAlignment.MiddleCenter;
            deletedIcon.UseMnemonic = false;
            // 
            // deletedLabel
            // 
            deletedLabel.BackColor = Color.White;
            deletedLabel.Dock = DockStyle.Bottom;
            deletedLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            deletedLabel.ForeColor = Color.FromArgb(122, 136, 156);
            deletedLabel.Location = new Point(0, 0);
            deletedLabel.Name = "deletedLabel";
            deletedLabel.Size = new Size(50, 14);
            deletedLabel.TabIndex = 1;
            deletedLabel.Text = "Deleted";
            deletedLabel.TextAlign = ContentAlignment.MiddleCenter;
            deletedLabel.UseMnemonic = false;
            // 
            // deletedValue
            // 
            deletedValue.BackColor = Color.White;
            deletedValue.Dock = DockStyle.Bottom;
            deletedValue.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            deletedValue.ForeColor = Color.FromArgb(31, 42, 60);
            deletedValue.Location = new Point(0, 0);
            deletedValue.Name = "deletedValue";
            deletedValue.Size = new Size(50, 18);
            deletedValue.TabIndex = 2;
            deletedValue.Text = "0";
            deletedValue.TextAlign = ContentAlignment.MiddleCenter;
            deletedValue.UseMnemonic = false;
            // 
            // auditSummaryHeader
            // 
            auditSummaryHeader.BackColor = Color.White;
            auditSummaryHeader.Controls.Add(auditSummaryTitle);
            auditSummaryHeader.Controls.Add(auditSummaryIcon);
            auditSummaryHeader.Dock = DockStyle.Top;
            auditSummaryHeader.Location = new Point(0, 0);
            auditSummaryHeader.Name = "auditSummaryHeader";
            auditSummaryHeader.Size = new Size(248, 34);
            auditSummaryHeader.TabIndex = 1;
            // 
            // auditSummaryTitle
            // 
            auditSummaryTitle.BackColor = Color.White;
            auditSummaryTitle.Dock = DockStyle.Fill;
            auditSummaryTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditSummaryTitle.ForeColor = Color.FromArgb(31, 42, 60);
            auditSummaryTitle.Location = new Point(26, 0);
            auditSummaryTitle.Name = "auditSummaryTitle";
            auditSummaryTitle.Size = new Size(160, 34);
            auditSummaryTitle.TabIndex = 0;
            auditSummaryTitle.Text = "Audit Summary";
            auditSummaryTitle.TextAlign = ContentAlignment.MiddleLeft;
            auditSummaryTitle.UseMnemonic = false;
            // 
            // auditSummaryIcon
            // 
            auditSummaryIcon.BackColor = Color.White;
            auditSummaryIcon.Dock = DockStyle.Left;
            auditSummaryIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditSummaryIcon.ForeColor = Color.FromArgb(58, 123, 213);
            auditSummaryIcon.Location = new Point(0, 0);
            auditSummaryIcon.Name = "auditSummaryIcon";
            auditSummaryIcon.Size = new Size(26, 34);
            auditSummaryIcon.TabIndex = 1;
            auditSummaryIcon.Text = "\uE9D9";
            auditSummaryIcon.TextAlign = ContentAlignment.MiddleCenter;
            auditSummaryIcon.UseMnemonic = false;
            // 
            // exportCard
            // 
            exportCard.BorderColor = Color.FromArgb(227, 233, 242);
            exportCard.Controls.Add(exportGrid);
            exportCard.Controls.Add(exportHeader);
            exportCard.CornerColor = Color.FromArgb(243, 247, 252);
            exportCard.Dock = DockStyle.Fill;
            exportCard.FillColor = Color.White;
            exportCard.Location = new Point(0, 0);
            exportCard.Margin = new Padding(0, 0, 0, 0);
            exportCard.Name = "exportCard";
            exportCard.Padding = new Padding(16, 12, 16, 12);
            exportCard.Radius = 10;
            exportCard.Size = new Size(280, 138);
            exportCard.TabIndex = 3;
            // 
            // exportGrid
            // 
            exportGrid.BackColor = Color.White;
            exportGrid.ColumnCount = 3;
            exportGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            exportGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            exportGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            exportGrid.Controls.Add(exportPdfTile, 0, 0);
            exportGrid.Controls.Add(exportExcelTile, 2, 0);
            exportGrid.Dock = DockStyle.Fill;
            exportGrid.Location = new Point(0, 0);
            exportGrid.Margin = new Padding(0, 0, 0, 0);
            exportGrid.Name = "exportGrid";
            exportGrid.RowCount = 1;
            exportGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            exportGrid.Size = new Size(248, 84);
            exportGrid.TabIndex = 0;
            // 
            // exportPdfTile
            // 
            exportPdfTile.BorderColor = Color.FromArgb(227, 233, 242);
            exportPdfTile.Controls.Add(exportPdfIconHost);
            exportPdfTile.Controls.Add(exportPdfTextHost);
            exportPdfTile.CornerColor = Color.White;
            exportPdfTile.Cursor = Cursors.Hand;
            exportPdfTile.Dock = DockStyle.Fill;
            exportPdfTile.FillColor = Color.White;
            exportPdfTile.Location = new Point(0, 0);
            exportPdfTile.Margin = new Padding(0, 0, 0, 0);
            exportPdfTile.Name = "exportPdfTile";
            exportPdfTile.Padding = new Padding(4, 6, 4, 4);
            exportPdfTile.Radius = 10;
            exportPdfTile.Size = new Size(110, 84);
            exportPdfTile.TabIndex = 0;
            // 
            // exportPdfIconHost
            // 
            exportPdfIconHost.BackColor = Color.White;
            exportPdfIconHost.Controls.Add(exportPdfIcon);
            exportPdfIconHost.Cursor = Cursors.Hand;
            exportPdfIconHost.Dock = DockStyle.Fill;
            exportPdfIconHost.Location = new Point(0, 0);
            exportPdfIconHost.Name = "exportPdfIconHost";
            exportPdfIconHost.Size = new Size(108, 44);
            exportPdfIconHost.TabIndex = 0;
            // 
            // exportPdfIcon
            // 
            exportPdfIcon.Anchor = AnchorStyles.None;
            exportPdfIcon.BackColor = Color.White;
            exportPdfIcon.FillColor = Color.FromArgb(234, 242, 253);
            exportPdfIcon.ForeColor = Color.FromArgb(58, 123, 213);
            exportPdfIcon.Glyph = "\uE8A5";
            exportPdfIcon.GlyphSize = 10F;
            exportPdfIcon.Location = new Point(40, 8);
            exportPdfIcon.Name = "exportPdfIcon";
            exportPdfIcon.Radius = 6;
            exportPdfIcon.Size = new Size(28, 28);
            exportPdfIcon.TabIndex = 0;
            exportPdfIcon.TabStop = false;
            // 
            // exportPdfTextHost
            // 
            exportPdfTextHost.BackColor = Color.White;
            exportPdfTextHost.Controls.Add(exportPdfSub);
            exportPdfTextHost.Controls.Add(exportPdfLabel);
            exportPdfTextHost.Cursor = Cursors.Hand;
            exportPdfTextHost.Dock = DockStyle.Bottom;
            exportPdfTextHost.Location = new Point(0, 0);
            exportPdfTextHost.Name = "exportPdfTextHost";
            exportPdfTextHost.Size = new Size(108, 32);
            exportPdfTextHost.TabIndex = 1;
            // 
            // exportPdfSub
            // 
            exportPdfSub.BackColor = Color.White;
            exportPdfSub.Cursor = Cursors.Hand;
            exportPdfSub.Dock = DockStyle.Top;
            exportPdfSub.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exportPdfSub.ForeColor = Color.FromArgb(122, 136, 156);
            exportPdfSub.Location = new Point(0, 0);
            exportPdfSub.Name = "exportPdfSub";
            exportPdfSub.Size = new Size(108, 14);
            exportPdfSub.TabIndex = 0;
            exportPdfSub.Text = "(Full Report)";
            exportPdfSub.TextAlign = ContentAlignment.MiddleCenter;
            exportPdfSub.UseMnemonic = false;
            // 
            // exportPdfLabel
            // 
            exportPdfLabel.BackColor = Color.White;
            exportPdfLabel.Cursor = Cursors.Hand;
            exportPdfLabel.Dock = DockStyle.Top;
            exportPdfLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportPdfLabel.ForeColor = Color.FromArgb(31, 42, 60);
            exportPdfLabel.Location = new Point(0, 0);
            exportPdfLabel.Name = "exportPdfLabel";
            exportPdfLabel.Size = new Size(108, 16);
            exportPdfLabel.TabIndex = 1;
            exportPdfLabel.Text = "Download PDF";
            exportPdfLabel.TextAlign = ContentAlignment.MiddleCenter;
            exportPdfLabel.UseMnemonic = false;
            // 
            // exportExcelTile
            // 
            exportExcelTile.BorderColor = Color.FromArgb(227, 233, 242);
            exportExcelTile.Controls.Add(exportExcelIconHost);
            exportExcelTile.Controls.Add(exportExcelTextHost);
            exportExcelTile.CornerColor = Color.White;
            exportExcelTile.Cursor = Cursors.Hand;
            exportExcelTile.Dock = DockStyle.Fill;
            exportExcelTile.FillColor = Color.White;
            exportExcelTile.Location = new Point(0, 0);
            exportExcelTile.Margin = new Padding(0, 0, 0, 0);
            exportExcelTile.Name = "exportExcelTile";
            exportExcelTile.Padding = new Padding(4, 6, 4, 4);
            exportExcelTile.Radius = 10;
            exportExcelTile.Size = new Size(110, 84);
            exportExcelTile.TabIndex = 1;
            // 
            // exportExcelIconHost
            // 
            exportExcelIconHost.BackColor = Color.White;
            exportExcelIconHost.Controls.Add(exportExcelIcon);
            exportExcelIconHost.Cursor = Cursors.Hand;
            exportExcelIconHost.Dock = DockStyle.Fill;
            exportExcelIconHost.Location = new Point(0, 0);
            exportExcelIconHost.Name = "exportExcelIconHost";
            exportExcelIconHost.Size = new Size(108, 44);
            exportExcelIconHost.TabIndex = 0;
            // 
            // exportExcelIcon
            // 
            exportExcelIcon.Anchor = AnchorStyles.None;
            exportExcelIcon.BackColor = Color.White;
            exportExcelIcon.FillColor = Color.FromArgb(234, 242, 253);
            exportExcelIcon.ForeColor = Color.FromArgb(58, 123, 213);
            exportExcelIcon.Glyph = "\uE80A";
            exportExcelIcon.GlyphSize = 10F;
            exportExcelIcon.Location = new Point(40, 8);
            exportExcelIcon.Name = "exportExcelIcon";
            exportExcelIcon.Radius = 6;
            exportExcelIcon.Size = new Size(28, 28);
            exportExcelIcon.TabIndex = 0;
            exportExcelIcon.TabStop = false;
            // 
            // exportExcelTextHost
            // 
            exportExcelTextHost.BackColor = Color.White;
            exportExcelTextHost.Controls.Add(exportExcelSub);
            exportExcelTextHost.Controls.Add(exportExcelLabel);
            exportExcelTextHost.Cursor = Cursors.Hand;
            exportExcelTextHost.Dock = DockStyle.Bottom;
            exportExcelTextHost.Location = new Point(0, 0);
            exportExcelTextHost.Name = "exportExcelTextHost";
            exportExcelTextHost.Size = new Size(108, 32);
            exportExcelTextHost.TabIndex = 1;
            // 
            // exportExcelSub
            // 
            exportExcelSub.BackColor = Color.White;
            exportExcelSub.Cursor = Cursors.Hand;
            exportExcelSub.Dock = DockStyle.Top;
            exportExcelSub.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exportExcelSub.ForeColor = Color.FromArgb(122, 136, 156);
            exportExcelSub.Location = new Point(0, 0);
            exportExcelSub.Name = "exportExcelSub";
            exportExcelSub.Size = new Size(108, 14);
            exportExcelSub.TabIndex = 0;
            exportExcelSub.Text = "(Transactions)";
            exportExcelSub.TextAlign = ContentAlignment.MiddleCenter;
            exportExcelSub.UseMnemonic = false;
            // 
            // exportExcelLabel
            // 
            exportExcelLabel.BackColor = Color.White;
            exportExcelLabel.Cursor = Cursors.Hand;
            exportExcelLabel.Dock = DockStyle.Top;
            exportExcelLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportExcelLabel.ForeColor = Color.FromArgb(31, 42, 60);
            exportExcelLabel.Location = new Point(0, 0);
            exportExcelLabel.Name = "exportExcelLabel";
            exportExcelLabel.Size = new Size(108, 16);
            exportExcelLabel.TabIndex = 1;
            exportExcelLabel.Text = "Download Excel";
            exportExcelLabel.TextAlign = ContentAlignment.MiddleCenter;
            exportExcelLabel.UseMnemonic = false;
            // 
            // exportHeader
            // 
            exportHeader.BackColor = Color.White;
            exportHeader.Controls.Add(exportTitle);
            exportHeader.Controls.Add(exportIcon);
            exportHeader.Dock = DockStyle.Top;
            exportHeader.Location = new Point(0, 0);
            exportHeader.Name = "exportHeader";
            exportHeader.Size = new Size(248, 34);
            exportHeader.TabIndex = 1;
            // 
            // exportTitle
            // 
            exportTitle.BackColor = Color.White;
            exportTitle.Dock = DockStyle.Fill;
            exportTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportTitle.ForeColor = Color.FromArgb(31, 42, 60);
            exportTitle.Location = new Point(26, 0);
            exportTitle.Name = "exportTitle";
            exportTitle.Size = new Size(160, 34);
            exportTitle.TabIndex = 0;
            exportTitle.Text = "Export Logs";
            exportTitle.TextAlign = ContentAlignment.MiddleLeft;
            exportTitle.UseMnemonic = false;
            // 
            // exportIcon
            // 
            exportIcon.BackColor = Color.White;
            exportIcon.Dock = DockStyle.Left;
            exportIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exportIcon.ForeColor = Color.FromArgb(58, 123, 213);
            exportIcon.Location = new Point(0, 0);
            exportIcon.Name = "exportIcon";
            exportIcon.Size = new Size(26, 34);
            exportIcon.TabIndex = 1;
            exportIcon.Text = "\uE898";
            exportIcon.TextAlign = ContentAlignment.MiddleCenter;
            exportIcon.UseMnemonic = false;
            // 
            // AuditTrailPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            Controls.Add(pageHost);
            Name = "AuditTrailPage";
            Size = new Size(1036, 732);
            pageHost.ResumeLayout(false);
            pageGrid.ResumeLayout(false);
            leftColumn.ResumeLayout(false);
            activityLogsCard.ResumeLayout(false);
            activityTableArea.ResumeLayout(false);
            activityRows.ResumeLayout(false);
            activityColumnsHead.ResumeLayout(false);
            activityColumnsGrid.ResumeLayout(false);
            activityTableTitle.ResumeLayout(false);
            activityFooter.ResumeLayout(false);
            activityPager.ResumeLayout(false);
            activityFiltersRow.ResumeLayout(false);
            activitySearchRow.ResumeLayout(false);
            activityStats.ResumeLayout(false);
            actTotalTile.ResumeLayout(false);
            actTotalTop.ResumeLayout(false);
            actTotalIconHost.ResumeLayout(false);
            actTodayTile.ResumeLayout(false);
            actTodayTop.ResumeLayout(false);
            actTodayIconHost.ResumeLayout(false);
            actPendingTile.ResumeLayout(false);
            actPendingTop.ResumeLayout(false);
            actPendingIconHost.ResumeLayout(false);
            actUsersTile.ResumeLayout(false);
            actUsersTop.ResumeLayout(false);
            actUsersIconHost.ResumeLayout(false);
            overviewCard.ResumeLayout(false);
            overviewHead.ResumeLayout(false);
            overviewText.ResumeLayout(false);
            overviewIconHost.ResumeLayout(false);
            overviewDateHost.ResumeLayout(false);
            rightColumn.ResumeLayout(false);
            quickFiltersCard.ResumeLayout(false);
            loginRow.ResumeLayout(false);
            loginIconHost.ResumeLayout(false);
            loginCountHost.ResumeLayout(false);
            criticalRow.ResumeLayout(false);
            criticalIconHost.ResumeLayout(false);
            criticalCountHost.ResumeLayout(false);
            weekRow.ResumeLayout(false);
            weekIconHost.ResumeLayout(false);
            weekCountHost.ResumeLayout(false);
            todayRow.ResumeLayout(false);
            todayIconHost.ResumeLayout(false);
            todayCountHost.ResumeLayout(false);
            quickFiltersHeader.ResumeLayout(false);
            recentActivityCard.ResumeLayout(false);
            recentActivityHeader.ResumeLayout(false);
            auditSummaryCard.ResumeLayout(false);
            summaryGrid.ResumeLayout(false);
            createdTile.ResumeLayout(false);
            updatedTile.ResumeLayout(false);
            approvedTile.ResumeLayout(false);
            deletedTile.ResumeLayout(false);
            auditSummaryHeader.ResumeLayout(false);
            exportCard.ResumeLayout(false);
            exportGrid.ResumeLayout(false);
            exportPdfTile.ResumeLayout(false);
            exportPdfIconHost.ResumeLayout(false);
            exportPdfTextHost.ResumeLayout(false);
            exportExcelTile.ResumeLayout(false);
            exportExcelIconHost.ResumeLayout(false);
            exportExcelTextHost.ResumeLayout(false);
            exportHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pageHost;
        private System.Windows.Forms.TableLayoutPanel pageGrid;
        private System.Windows.Forms.Panel leftColumn;
        private MyApp.Controls.CardPanel activityLogsCard;
        private System.Windows.Forms.Panel activityTableArea;
        private System.Windows.Forms.Panel activityRows;
        private MyApp.Controls.EmptyState activityEmpty;
        private System.Windows.Forms.Panel activityColumnsHead;
        private System.Windows.Forms.TableLayoutPanel activityColumnsGrid;
        private MyApp.Controls.CheckMark activityHeaderCheck;
        private System.Windows.Forms.Label activityColDateTime;
        private System.Windows.Forms.Label activityColUser;
        private System.Windows.Forms.Label activityColAction;
        private System.Windows.Forms.Label activityColModule;
        private System.Windows.Forms.Label activityColDescription;
        private System.Windows.Forms.Label activityColReference;
        private System.Windows.Forms.Label activityColStatus;
        private System.Windows.Forms.Label activityColActions;
        private System.Windows.Forms.Panel activityTableTitle;
        private System.Windows.Forms.Label activityTableTitleText;
        private System.Windows.Forms.Label activityTableTitleIcon;
        private System.Windows.Forms.Panel activityFooter;
        private System.Windows.Forms.Label activityShowing;
        private System.Windows.Forms.Panel activityPager;
        private MyApp.Controls.IconButton activityPrevPage;
        private MyApp.Controls.PrimaryButton activityPageChip;
        private MyApp.Controls.IconButton activityNextPage;
        private System.Windows.Forms.Panel activityFiltersRow;
        private MyApp.Controls.DateRangeField activityDateRange;
        private System.Windows.Forms.Panel filterGapC;
        private MyApp.Controls.SelectField actionTypeFilter;
        private System.Windows.Forms.Panel filterGapB;
        private MyApp.Controls.SelectField moduleFilter;
        private System.Windows.Forms.Panel filterGapA;
        private MyApp.Controls.SelectField userFilter;
        private System.Windows.Forms.Panel activitySearchRow;
        private MyApp.Controls.InputField activitySearch;
        private System.Windows.Forms.Panel gapTwo;
        private System.Windows.Forms.TableLayoutPanel activityStats;
        private MyApp.Controls.CardPanel actTotalTile;
        private System.Windows.Forms.Label actTotalDelta;
        private System.Windows.Forms.Label actTotalValue;
        private System.Windows.Forms.Panel actTotalTop;
        private System.Windows.Forms.Label actTotalLabel;
        private System.Windows.Forms.Panel actTotalIconHost;
        private MyApp.Controls.IconBadge actTotalIcon;
        private MyApp.Controls.CardPanel actTodayTile;
        private System.Windows.Forms.Label actTodayDelta;
        private System.Windows.Forms.Label actTodayValue;
        private System.Windows.Forms.Panel actTodayTop;
        private System.Windows.Forms.Label actTodayLabel;
        private System.Windows.Forms.Panel actTodayIconHost;
        private MyApp.Controls.IconBadge actTodayIcon;
        private MyApp.Controls.CardPanel actPendingTile;
        private System.Windows.Forms.Label actPendingDelta;
        private System.Windows.Forms.Label actPendingValue;
        private System.Windows.Forms.Panel actPendingTop;
        private System.Windows.Forms.Label actPendingLabel;
        private System.Windows.Forms.Panel actPendingIconHost;
        private MyApp.Controls.IconBadge actPendingIcon;
        private MyApp.Controls.CardPanel actUsersTile;
        private System.Windows.Forms.Label actUsersDelta;
        private System.Windows.Forms.Label actUsersValue;
        private System.Windows.Forms.Panel actUsersTop;
        private System.Windows.Forms.Label actUsersLabel;
        private System.Windows.Forms.Panel actUsersIconHost;
        private MyApp.Controls.IconBadge actUsersIcon;
        private System.Windows.Forms.Panel gapOne;
        private MyApp.Controls.CardPanel overviewCard;
        private System.Windows.Forms.Panel overviewHead;
        private System.Windows.Forms.Panel overviewText;
        private System.Windows.Forms.Label overviewSubtitle;
        private System.Windows.Forms.Label overviewTitle;
        private System.Windows.Forms.Panel overviewIconHost;
        private MyApp.Controls.IconBadge overviewIcon;
        private System.Windows.Forms.Panel overviewDateHost;
        private MyApp.Controls.DateRangeField overviewDateRange;
        private System.Windows.Forms.TableLayoutPanel rightColumn;
        private MyApp.Controls.CardPanel quickFiltersCard;
        private System.Windows.Forms.Panel loginRow;
        private System.Windows.Forms.Label loginName;
        private System.Windows.Forms.Panel loginIconHost;
        private MyApp.Controls.IconBadge loginIcon;
        private System.Windows.Forms.Panel loginCountHost;
        private System.Windows.Forms.Label loginCount;
        private System.Windows.Forms.Label loginChevron;
        private System.Windows.Forms.Panel criticalRow;
        private System.Windows.Forms.Label criticalName;
        private System.Windows.Forms.Panel criticalIconHost;
        private MyApp.Controls.IconBadge criticalIcon;
        private System.Windows.Forms.Panel criticalCountHost;
        private System.Windows.Forms.Label criticalCount;
        private System.Windows.Forms.Label criticalChevron;
        private System.Windows.Forms.Panel weekRow;
        private System.Windows.Forms.Label weekName;
        private System.Windows.Forms.Panel weekIconHost;
        private MyApp.Controls.IconBadge weekIcon;
        private System.Windows.Forms.Panel weekCountHost;
        private System.Windows.Forms.Label weekCount;
        private System.Windows.Forms.Label weekChevron;
        private System.Windows.Forms.Panel todayRow;
        private System.Windows.Forms.Label todayName;
        private System.Windows.Forms.Panel todayIconHost;
        private MyApp.Controls.IconBadge todayIcon;
        private System.Windows.Forms.Panel todayCountHost;
        private System.Windows.Forms.Label todayCount;
        private System.Windows.Forms.Label todayChevron;
        private System.Windows.Forms.Panel quickFiltersHeader;
        private System.Windows.Forms.Label quickFiltersTitle;
        private System.Windows.Forms.Label quickFiltersIcon;
        private System.Windows.Forms.Label quickFiltersLink;
        private MyApp.Controls.CardPanel recentActivityCard;
        private MyApp.Controls.EmptyState recentActivityEmpty;
        private System.Windows.Forms.Panel recentActivityHeader;
        private System.Windows.Forms.Label recentActivityTitle;
        private System.Windows.Forms.Label recentActivityIcon;
        private System.Windows.Forms.Label recentActivityLink;
        private MyApp.Controls.CardPanel auditSummaryCard;
        private System.Windows.Forms.TableLayoutPanel summaryGrid;
        private MyApp.Controls.CardPanel createdTile;
        private System.Windows.Forms.Label createdIcon;
        private System.Windows.Forms.Label createdLabel;
        private System.Windows.Forms.Label createdValue;
        private MyApp.Controls.CardPanel updatedTile;
        private System.Windows.Forms.Label updatedIcon;
        private System.Windows.Forms.Label updatedLabel;
        private System.Windows.Forms.Label updatedValue;
        private MyApp.Controls.CardPanel approvedTile;
        private System.Windows.Forms.Label approvedIcon;
        private System.Windows.Forms.Label approvedLabel;
        private System.Windows.Forms.Label approvedValue;
        private MyApp.Controls.CardPanel deletedTile;
        private System.Windows.Forms.Label deletedIcon;
        private System.Windows.Forms.Label deletedLabel;
        private System.Windows.Forms.Label deletedValue;
        private System.Windows.Forms.Panel auditSummaryHeader;
        private System.Windows.Forms.Label auditSummaryTitle;
        private System.Windows.Forms.Label auditSummaryIcon;
        private MyApp.Controls.CardPanel exportCard;
        private System.Windows.Forms.TableLayoutPanel exportGrid;
        private MyApp.Controls.CardPanel exportPdfTile;
        private System.Windows.Forms.Panel exportPdfIconHost;
        private MyApp.Controls.IconBadge exportPdfIcon;
        private System.Windows.Forms.Panel exportPdfTextHost;
        private System.Windows.Forms.Label exportPdfSub;
        private System.Windows.Forms.Label exportPdfLabel;
        private MyApp.Controls.CardPanel exportExcelTile;
        private System.Windows.Forms.Panel exportExcelIconHost;
        private MyApp.Controls.IconBadge exportExcelIcon;
        private System.Windows.Forms.Panel exportExcelTextHost;
        private System.Windows.Forms.Label exportExcelSub;
        private System.Windows.Forms.Label exportExcelLabel;
        private System.Windows.Forms.Panel exportHeader;
        private System.Windows.Forms.Label exportTitle;
        private System.Windows.Forms.Label exportIcon;
    }
}
