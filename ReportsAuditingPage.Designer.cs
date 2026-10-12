#nullable disable
namespace MyApp
{
    partial class ReportsAuditingPage
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
            auditTableCard = new MyApp.Controls.CardPanel();
            auditTableArea = new System.Windows.Forms.Panel();
            auditRows = new System.Windows.Forms.Panel();
            auditEmpty = new MyApp.Controls.EmptyState();
            auditColumnsHead = new System.Windows.Forms.Panel();
            auditColumnsGrid = new System.Windows.Forms.TableLayoutPanel();
            auditHeaderCheck = new MyApp.Controls.CheckMark();
            auditColDateTime = new System.Windows.Forms.Label();
            auditColType = new System.Windows.Forms.Label();
            auditColProduct = new System.Windows.Forms.Label();
            auditColQuantity = new System.Windows.Forms.Label();
            auditColAmount = new System.Windows.Forms.Label();
            auditColReference = new System.Windows.Forms.Label();
            auditColStatus = new System.Windows.Forms.Label();
            auditColActions = new System.Windows.Forms.Label();
            auditFooter = new System.Windows.Forms.Panel();
            auditShowing = new System.Windows.Forms.Label();
            auditPager = new System.Windows.Forms.Panel();
            auditPrevPage = new MyApp.Controls.IconButton();
            auditPageChip = new MyApp.Controls.PrimaryButton();
            auditNextPage = new MyApp.Controls.IconButton();
            reportFiltersRow = new System.Windows.Forms.Panel();
            auditDateRange = new MyApp.Controls.DateRangeField();
            filterGapB = new System.Windows.Forms.Panel();
            staffFilter = new MyApp.Controls.SelectField();
            filterGapA = new System.Windows.Forms.Panel();
            categoryFilter = new MyApp.Controls.SelectField();
            reportTabsRow = new System.Windows.Forms.Panel();
            reportTabs = new System.Windows.Forms.FlowLayoutPanel();
            salesLogsTab = new MyApp.Controls.TabPill();
            movementTab = new MyApp.Controls.TabPill();
            summaryTab = new MyApp.Controls.TabPill();
            gapTwo = new System.Windows.Forms.Panel();
            auditStats = new System.Windows.Forms.TableLayoutPanel();
            auditSalesTile = new MyApp.Controls.CardPanel();
            auditSalesDelta = new System.Windows.Forms.Label();
            auditSalesValue = new System.Windows.Forms.Label();
            auditSalesTop = new System.Windows.Forms.Panel();
            auditSalesLabel = new System.Windows.Forms.Label();
            auditSalesIconHost = new System.Windows.Forms.Panel();
            auditSalesIcon = new MyApp.Controls.IconBadge();
            auditMovementTile = new MyApp.Controls.CardPanel();
            auditMovementDelta = new System.Windows.Forms.Label();
            auditMovementValue = new System.Windows.Forms.Label();
            auditMovementTop = new System.Windows.Forms.Panel();
            auditMovementLabel = new System.Windows.Forms.Label();
            auditMovementIconHost = new System.Windows.Forms.Panel();
            auditMovementIcon = new MyApp.Controls.IconBadge();
            auditDiscrepanciesTile = new MyApp.Controls.CardPanel();
            auditDiscrepanciesDelta = new System.Windows.Forms.Label();
            auditDiscrepanciesValue = new System.Windows.Forms.Label();
            auditDiscrepanciesTop = new System.Windows.Forms.Panel();
            auditDiscrepanciesLabel = new System.Windows.Forms.Label();
            auditDiscrepanciesIconHost = new System.Windows.Forms.Panel();
            auditDiscrepanciesIcon = new MyApp.Controls.IconBadge();
            auditPeriodTile = new MyApp.Controls.CardPanel();
            auditPeriodValue = new System.Windows.Forms.Label();
            auditPeriodTop = new System.Windows.Forms.Panel();
            auditPeriodLabel = new System.Windows.Forms.Label();
            auditPeriodIconHost = new System.Windows.Forms.Panel();
            auditPeriodIcon = new MyApp.Controls.IconBadge();
            gapOne = new System.Windows.Forms.Panel();
            reportsHeaderCard = new MyApp.Controls.CardPanel();
            reportsHeaderText = new System.Windows.Forms.Panel();
            reportsSubtitle = new System.Windows.Forms.Label();
            reportsTitle = new System.Windows.Forms.Label();
            reportsIconHost = new System.Windows.Forms.Panel();
            reportsIcon = new MyApp.Controls.IconBadge();
            downloadReportHost = new System.Windows.Forms.Panel();
            downloadReportButton = new MyApp.Controls.PrimaryButton();
            rightColumn = new System.Windows.Forms.TableLayoutPanel();
            quickReportsCard = new MyApp.Controls.CardPanel();
            printableReportRow = new System.Windows.Forms.Panel();
            printableReportTextHost = new System.Windows.Forms.Panel();
            printableReportDesc = new System.Windows.Forms.Label();
            printableReportTitle = new System.Windows.Forms.Label();
            printableReportChevron = new System.Windows.Forms.Label();
            printableReportIconHost = new System.Windows.Forms.Panel();
            printableReportIcon = new MyApp.Controls.IconBadge();
            financialReportRow = new System.Windows.Forms.Panel();
            financialReportTextHost = new System.Windows.Forms.Panel();
            financialReportDesc = new System.Windows.Forms.Label();
            financialReportTitle = new System.Windows.Forms.Label();
            financialReportChevron = new System.Windows.Forms.Label();
            financialReportIconHost = new System.Windows.Forms.Panel();
            financialReportIcon = new MyApp.Controls.IconBadge();
            movementReportRow = new System.Windows.Forms.Panel();
            movementReportTextHost = new System.Windows.Forms.Panel();
            movementReportDesc = new System.Windows.Forms.Label();
            movementReportTitle = new System.Windows.Forms.Label();
            movementReportChevron = new System.Windows.Forms.Label();
            movementReportIconHost = new System.Windows.Forms.Panel();
            movementReportIcon = new MyApp.Controls.IconBadge();
            salesReportRow = new System.Windows.Forms.Panel();
            salesReportTextHost = new System.Windows.Forms.Panel();
            salesReportDesc = new System.Windows.Forms.Label();
            salesReportTitle = new System.Windows.Forms.Label();
            salesReportChevron = new System.Windows.Forms.Label();
            salesReportIconHost = new System.Windows.Forms.Panel();
            salesReportIcon = new MyApp.Controls.IconBadge();
            quickReportsHeader = new System.Windows.Forms.Panel();
            quickReportsTitle = new System.Windows.Forms.Label();
            quickReportsIcon = new System.Windows.Forms.Label();
            quickReportsLink = new System.Windows.Forms.Label();
            recentActivityCard = new MyApp.Controls.CardPanel();
            recentActivityEmpty = new MyApp.Controls.EmptyState();
            recentActivityHeader = new System.Windows.Forms.Panel();
            recentActivityTitle = new System.Windows.Forms.Label();
            recentActivityIcon = new System.Windows.Forms.Label();
            recentActivityLink = new System.Windows.Forms.Label();
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
            auditTableCard.SuspendLayout();
            auditTableArea.SuspendLayout();
            auditRows.SuspendLayout();
            auditColumnsHead.SuspendLayout();
            auditColumnsGrid.SuspendLayout();
            auditFooter.SuspendLayout();
            auditPager.SuspendLayout();
            reportFiltersRow.SuspendLayout();
            reportTabsRow.SuspendLayout();
            reportTabs.SuspendLayout();
            auditStats.SuspendLayout();
            auditSalesTile.SuspendLayout();
            auditSalesTop.SuspendLayout();
            auditSalesIconHost.SuspendLayout();
            auditMovementTile.SuspendLayout();
            auditMovementTop.SuspendLayout();
            auditMovementIconHost.SuspendLayout();
            auditDiscrepanciesTile.SuspendLayout();
            auditDiscrepanciesTop.SuspendLayout();
            auditDiscrepanciesIconHost.SuspendLayout();
            auditPeriodTile.SuspendLayout();
            auditPeriodTop.SuspendLayout();
            auditPeriodIconHost.SuspendLayout();
            reportsHeaderCard.SuspendLayout();
            reportsHeaderText.SuspendLayout();
            reportsIconHost.SuspendLayout();
            downloadReportHost.SuspendLayout();
            rightColumn.SuspendLayout();
            quickReportsCard.SuspendLayout();
            printableReportRow.SuspendLayout();
            printableReportTextHost.SuspendLayout();
            printableReportIconHost.SuspendLayout();
            financialReportRow.SuspendLayout();
            financialReportTextHost.SuspendLayout();
            financialReportIconHost.SuspendLayout();
            movementReportRow.SuspendLayout();
            movementReportTextHost.SuspendLayout();
            movementReportIconHost.SuspendLayout();
            salesReportRow.SuspendLayout();
            salesReportTextHost.SuspendLayout();
            salesReportIconHost.SuspendLayout();
            quickReportsHeader.SuspendLayout();
            recentActivityCard.SuspendLayout();
            recentActivityHeader.SuspendLayout();
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
            leftColumn.Controls.Add(auditTableCard);
            leftColumn.Controls.Add(reportFiltersRow);
            leftColumn.Controls.Add(reportTabsRow);
            leftColumn.Controls.Add(gapTwo);
            leftColumn.Controls.Add(auditStats);
            leftColumn.Controls.Add(gapOne);
            leftColumn.Controls.Add(reportsHeaderCard);
            leftColumn.Dock = DockStyle.Fill;
            leftColumn.Location = new Point(0, 0);
            leftColumn.Margin = new Padding(0, 0, 0, 0);
            leftColumn.Name = "leftColumn";
            leftColumn.Size = new Size(700, 692);
            leftColumn.TabIndex = 0;
            // 
            // auditTableCard
            // 
            auditTableCard.BorderColor = Color.FromArgb(227, 233, 242);
            auditTableCard.Controls.Add(auditTableArea);
            auditTableCard.Controls.Add(auditFooter);
            auditTableCard.CornerColor = Color.FromArgb(243, 247, 252);
            auditTableCard.Dock = DockStyle.Fill;
            auditTableCard.FillColor = Color.White;
            auditTableCard.Location = new Point(0, 0);
            auditTableCard.Name = "auditTableCard";
            auditTableCard.Padding = new Padding(16, 14, 16, 10);
            auditTableCard.Radius = 10;
            auditTableCard.Size = new Size(700, 300);
            auditTableCard.TabIndex = 0;
            // 
            // auditTableArea
            // 
            auditTableArea.BackColor = Color.White;
            auditTableArea.Controls.Add(auditRows);
            auditTableArea.Controls.Add(auditColumnsHead);
            auditTableArea.Dock = DockStyle.Fill;
            auditTableArea.Location = new Point(0, 0);
            auditTableArea.Name = "auditTableArea";
            auditTableArea.Size = new Size(668, 256);
            auditTableArea.TabIndex = 0;
            // 
            // auditRows
            // 
            auditRows.AutoScroll = true;
            auditRows.BackColor = Color.White;
            auditRows.Controls.Add(auditEmpty);
            auditRows.Dock = DockStyle.Fill;
            auditRows.Location = new Point(0, 36);
            auditRows.Name = "auditRows";
            auditRows.Size = new Size(668, 220);
            auditRows.TabIndex = 0;
            // 
            // auditEmpty
            // 
            auditEmpty.BackColor = Color.White;
            auditEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            auditEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            auditEmpty.Dock = DockStyle.Fill;
            auditEmpty.Glyph = "\uE9F9";
            auditEmpty.Hint = "Sales, returns and stock adjustments will be listed here once they are recorded.";
            auditEmpty.Location = new Point(0, 0);
            auditEmpty.Name = "auditEmpty";
            auditEmpty.Size = new Size(668, 220);
            auditEmpty.TabIndex = 0;
            auditEmpty.TabStop = false;
            auditEmpty.Title = "No audit logs found";
            // 
            // auditColumnsHead
            // 
            auditColumnsHead.BackColor = Color.FromArgb(246, 248, 251);
            auditColumnsHead.Controls.Add(auditColumnsGrid);
            auditColumnsHead.Dock = DockStyle.Top;
            auditColumnsHead.Location = new Point(0, 0);
            auditColumnsHead.Name = "auditColumnsHead";
            auditColumnsHead.Size = new Size(668, 36);
            auditColumnsHead.TabIndex = 1;
            // 
            // auditColumnsGrid
            // 
            auditColumnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            auditColumnsGrid.ColumnCount = 9;
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            auditColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 64F));
            auditColumnsGrid.Controls.Add(auditHeaderCheck, 0, 0);
            auditColumnsGrid.Controls.Add(auditColDateTime, 1, 0);
            auditColumnsGrid.Controls.Add(auditColType, 2, 0);
            auditColumnsGrid.Controls.Add(auditColProduct, 3, 0);
            auditColumnsGrid.Controls.Add(auditColQuantity, 4, 0);
            auditColumnsGrid.Controls.Add(auditColAmount, 5, 0);
            auditColumnsGrid.Controls.Add(auditColReference, 6, 0);
            auditColumnsGrid.Controls.Add(auditColStatus, 7, 0);
            auditColumnsGrid.Controls.Add(auditColActions, 8, 0);
            auditColumnsGrid.Dock = DockStyle.Fill;
            auditColumnsGrid.Location = new Point(0, 0);
            auditColumnsGrid.Margin = new Padding(0, 0, 0, 0);
            auditColumnsGrid.Name = "auditColumnsGrid";
            auditColumnsGrid.RowCount = 1;
            auditColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            auditColumnsGrid.Size = new Size(668, 36);
            auditColumnsGrid.TabIndex = 0;
            // 
            // auditHeaderCheck
            // 
            auditHeaderCheck.Anchor = AnchorStyles.None;
            auditHeaderCheck.BackColor = Color.FromArgb(246, 248, 251);
            auditHeaderCheck.Location = new Point(13, 9);
            auditHeaderCheck.Name = "auditHeaderCheck";
            auditHeaderCheck.Size = new Size(18, 18);
            auditHeaderCheck.TabIndex = 0;
            auditHeaderCheck.TabStop = false;
            // 
            // auditColDateTime
            // 
            auditColDateTime.BackColor = Color.FromArgb(246, 248, 251);
            auditColDateTime.Dock = DockStyle.Fill;
            auditColDateTime.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditColDateTime.ForeColor = Color.FromArgb(122, 136, 156);
            auditColDateTime.Location = new Point(0, 0);
            auditColDateTime.Margin = new Padding(0, 0, 0, 0);
            auditColDateTime.Name = "auditColDateTime";
            auditColDateTime.Padding = new Padding(8, 0, 0, 0);
            auditColDateTime.Size = new Size(60, 36);
            auditColDateTime.TabIndex = 1;
            auditColDateTime.Text = "DATE & TIME";
            auditColDateTime.TextAlign = ContentAlignment.MiddleLeft;
            auditColDateTime.UseMnemonic = false;
            // 
            // auditColType
            // 
            auditColType.BackColor = Color.FromArgb(246, 248, 251);
            auditColType.Dock = DockStyle.Fill;
            auditColType.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditColType.ForeColor = Color.FromArgb(122, 136, 156);
            auditColType.Location = new Point(0, 0);
            auditColType.Margin = new Padding(0, 0, 0, 0);
            auditColType.Name = "auditColType";
            auditColType.Padding = new Padding(8, 0, 0, 0);
            auditColType.Size = new Size(60, 36);
            auditColType.TabIndex = 2;
            auditColType.Text = "TRANSACTION TYPE";
            auditColType.TextAlign = ContentAlignment.MiddleLeft;
            auditColType.UseMnemonic = false;
            // 
            // auditColProduct
            // 
            auditColProduct.BackColor = Color.FromArgb(246, 248, 251);
            auditColProduct.Dock = DockStyle.Fill;
            auditColProduct.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditColProduct.ForeColor = Color.FromArgb(122, 136, 156);
            auditColProduct.Location = new Point(0, 0);
            auditColProduct.Margin = new Padding(0, 0, 0, 0);
            auditColProduct.Name = "auditColProduct";
            auditColProduct.Padding = new Padding(8, 0, 0, 0);
            auditColProduct.Size = new Size(60, 36);
            auditColProduct.TabIndex = 3;
            auditColProduct.Text = "PRODUCT / SERVICE";
            auditColProduct.TextAlign = ContentAlignment.MiddleLeft;
            auditColProduct.UseMnemonic = false;
            // 
            // auditColQuantity
            // 
            auditColQuantity.BackColor = Color.FromArgb(246, 248, 251);
            auditColQuantity.Dock = DockStyle.Fill;
            auditColQuantity.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditColQuantity.ForeColor = Color.FromArgb(122, 136, 156);
            auditColQuantity.Location = new Point(0, 0);
            auditColQuantity.Margin = new Padding(0, 0, 0, 0);
            auditColQuantity.Name = "auditColQuantity";
            auditColQuantity.Padding = new Padding(8, 0, 0, 0);
            auditColQuantity.Size = new Size(60, 36);
            auditColQuantity.TabIndex = 4;
            auditColQuantity.Text = "QUANTITY";
            auditColQuantity.TextAlign = ContentAlignment.MiddleLeft;
            auditColQuantity.UseMnemonic = false;
            // 
            // auditColAmount
            // 
            auditColAmount.BackColor = Color.FromArgb(246, 248, 251);
            auditColAmount.Dock = DockStyle.Fill;
            auditColAmount.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditColAmount.ForeColor = Color.FromArgb(122, 136, 156);
            auditColAmount.Location = new Point(0, 0);
            auditColAmount.Margin = new Padding(0, 0, 0, 0);
            auditColAmount.Name = "auditColAmount";
            auditColAmount.Padding = new Padding(8, 0, 0, 0);
            auditColAmount.Size = new Size(60, 36);
            auditColAmount.TabIndex = 5;
            auditColAmount.Text = "AMOUNT";
            auditColAmount.TextAlign = ContentAlignment.MiddleLeft;
            auditColAmount.UseMnemonic = false;
            // 
            // auditColReference
            // 
            auditColReference.BackColor = Color.FromArgb(246, 248, 251);
            auditColReference.Dock = DockStyle.Fill;
            auditColReference.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditColReference.ForeColor = Color.FromArgb(122, 136, 156);
            auditColReference.Location = new Point(0, 0);
            auditColReference.Margin = new Padding(0, 0, 0, 0);
            auditColReference.Name = "auditColReference";
            auditColReference.Padding = new Padding(8, 0, 0, 0);
            auditColReference.Size = new Size(60, 36);
            auditColReference.TabIndex = 6;
            auditColReference.Text = "REFERENCE NO.";
            auditColReference.TextAlign = ContentAlignment.MiddleLeft;
            auditColReference.UseMnemonic = false;
            // 
            // auditColStatus
            // 
            auditColStatus.BackColor = Color.FromArgb(246, 248, 251);
            auditColStatus.Dock = DockStyle.Fill;
            auditColStatus.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditColStatus.ForeColor = Color.FromArgb(122, 136, 156);
            auditColStatus.Location = new Point(0, 0);
            auditColStatus.Margin = new Padding(0, 0, 0, 0);
            auditColStatus.Name = "auditColStatus";
            auditColStatus.Padding = new Padding(8, 0, 0, 0);
            auditColStatus.Size = new Size(60, 36);
            auditColStatus.TabIndex = 7;
            auditColStatus.Text = "STATUS";
            auditColStatus.TextAlign = ContentAlignment.MiddleLeft;
            auditColStatus.UseMnemonic = false;
            // 
            // auditColActions
            // 
            auditColActions.BackColor = Color.FromArgb(246, 248, 251);
            auditColActions.Dock = DockStyle.Fill;
            auditColActions.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditColActions.ForeColor = Color.FromArgb(122, 136, 156);
            auditColActions.Location = new Point(0, 0);
            auditColActions.Margin = new Padding(0, 0, 0, 0);
            auditColActions.Name = "auditColActions";
            auditColActions.Padding = new Padding(8, 0, 0, 0);
            auditColActions.Size = new Size(60, 36);
            auditColActions.TabIndex = 8;
            auditColActions.Text = "ACTIONS";
            auditColActions.TextAlign = ContentAlignment.MiddleLeft;
            auditColActions.UseMnemonic = false;
            // 
            // auditFooter
            // 
            auditFooter.BackColor = Color.White;
            auditFooter.Controls.Add(auditShowing);
            auditFooter.Controls.Add(auditPager);
            auditFooter.Dock = DockStyle.Bottom;
            auditFooter.Location = new Point(0, 0);
            auditFooter.Name = "auditFooter";
            auditFooter.Size = new Size(668, 44);
            auditFooter.TabIndex = 1;
            // 
            // auditShowing
            // 
            auditShowing.BackColor = Color.White;
            auditShowing.Dock = DockStyle.Fill;
            auditShowing.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditShowing.ForeColor = Color.FromArgb(122, 136, 156);
            auditShowing.Location = new Point(0, 0);
            auditShowing.Name = "auditShowing";
            auditShowing.Size = new Size(500, 44);
            auditShowing.TabIndex = 0;
            auditShowing.Text = "Showing 0 audit logs";
            auditShowing.TextAlign = ContentAlignment.MiddleLeft;
            auditShowing.UseMnemonic = false;
            // 
            // auditPager
            // 
            auditPager.BackColor = Color.White;
            auditPager.Controls.Add(auditPrevPage);
            auditPager.Controls.Add(auditPageChip);
            auditPager.Controls.Add(auditNextPage);
            auditPager.Dock = DockStyle.Right;
            auditPager.Location = new Point(0, 0);
            auditPager.Name = "auditPager";
            auditPager.Size = new Size(116, 44);
            auditPager.TabIndex = 1;
            // 
            // auditPrevPage
            // 
            auditPrevPage.BackColor = Color.White;
            auditPrevPage.Enabled = false;
            auditPrevPage.Glyph = "\uE76B";
            auditPrevPage.Location = new Point(4, 7);
            auditPrevPage.Name = "auditPrevPage";
            auditPrevPage.Size = new Size(30, 30);
            auditPrevPage.TabIndex = 0;
            // 
            // auditPageChip
            // 
            auditPageChip.BackColor = Color.White;
            auditPageChip.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditPageChip.Location = new Point(42, 7);
            auditPageChip.Name = "auditPageChip";
            auditPageChip.Size = new Size(30, 30);
            auditPageChip.TabIndex = 1;
            auditPageChip.Text = "1";
            // 
            // auditNextPage
            // 
            auditNextPage.BackColor = Color.White;
            auditNextPage.Enabled = false;
            auditNextPage.Glyph = "\uE76C";
            auditNextPage.Location = new Point(80, 7);
            auditNextPage.Name = "auditNextPage";
            auditNextPage.Size = new Size(30, 30);
            auditNextPage.TabIndex = 2;
            // 
            // reportFiltersRow
            // 
            reportFiltersRow.BackColor = Color.FromArgb(243, 247, 252);
            reportFiltersRow.Controls.Add(auditDateRange);
            reportFiltersRow.Controls.Add(filterGapB);
            reportFiltersRow.Controls.Add(staffFilter);
            reportFiltersRow.Controls.Add(filterGapA);
            reportFiltersRow.Controls.Add(categoryFilter);
            reportFiltersRow.Dock = DockStyle.Top;
            reportFiltersRow.Location = new Point(0, 0);
            reportFiltersRow.Name = "reportFiltersRow";
            reportFiltersRow.Padding = new Padding(0, 0, 0, 10);
            reportFiltersRow.Size = new Size(700, 46);
            reportFiltersRow.TabIndex = 1;
            // 
            // auditDateRange
            // 
            auditDateRange.BackColor = Color.FromArgb(243, 247, 252);
            auditDateRange.Dock = DockStyle.Left;
            auditDateRange.Location = new Point(0, 0);
            auditDateRange.Name = "auditDateRange";
            auditDateRange.Size = new Size(214, 36);
            auditDateRange.TabIndex = 0;
            auditDateRange.Text = "All dates";
            // 
            // filterGapB
            // 
            filterGapB.BackColor = Color.FromArgb(243, 247, 252);
            filterGapB.Dock = DockStyle.Left;
            filterGapB.Location = new Point(0, 0);
            filterGapB.Name = "filterGapB";
            filterGapB.Size = new Size(8, 36);
            filterGapB.TabIndex = 1;
            // 
            // staffFilter
            // 
            staffFilter.BackColor = Color.FromArgb(243, 247, 252);
            staffFilter.Dock = DockStyle.Left;
            staffFilter.Location = new Point(0, 0);
            staffFilter.Name = "staffFilter";
            staffFilter.Size = new Size(110, 36);
            staffFilter.TabIndex = 2;
            staffFilter.Text = "All Staff";
            // 
            // filterGapA
            // 
            filterGapA.BackColor = Color.FromArgb(243, 247, 252);
            filterGapA.Dock = DockStyle.Left;
            filterGapA.Location = new Point(0, 0);
            filterGapA.Name = "filterGapA";
            filterGapA.Size = new Size(8, 36);
            filterGapA.TabIndex = 3;
            // 
            // categoryFilter
            // 
            categoryFilter.BackColor = Color.FromArgb(243, 247, 252);
            categoryFilter.Dock = DockStyle.Left;
            categoryFilter.Location = new Point(0, 0);
            categoryFilter.Name = "categoryFilter";
            categoryFilter.Size = new Size(134, 36);
            categoryFilter.TabIndex = 4;
            categoryFilter.Text = "All Categories";
            // 
            // reportTabsRow
            // 
            reportTabsRow.BackColor = Color.FromArgb(243, 247, 252);
            reportTabsRow.Controls.Add(reportTabs);
            reportTabsRow.Dock = DockStyle.Top;
            reportTabsRow.Location = new Point(0, 0);
            reportTabsRow.Name = "reportTabsRow";
            reportTabsRow.Padding = new Padding(0, 4, 0, 4);
            reportTabsRow.Size = new Size(700, 40);
            reportTabsRow.TabIndex = 2;
            // 
            // reportTabs
            // 
            reportTabs.BackColor = Color.FromArgb(243, 247, 252);
            reportTabs.Controls.Add(salesLogsTab);
            reportTabs.Controls.Add(movementTab);
            reportTabs.Controls.Add(summaryTab);
            reportTabs.Dock = DockStyle.Fill;
            reportTabs.Location = new Point(0, 0);
            reportTabs.Name = "reportTabs";
            reportTabs.Size = new Size(700, 32);
            reportTabs.TabIndex = 0;
            reportTabs.WrapContents = false;
            // 
            // salesLogsTab
            // 
            salesLogsTab.Active = true;
            salesLogsTab.BackColor = Color.FromArgb(243, 247, 252);
            salesLogsTab.Font = new Font("Segoe UI", 8.5F);
            salesLogsTab.Location = new Point(0, 0);
            salesLogsTab.Margin = new Padding(0, 3, 8, 3);
            salesLogsTab.Name = "salesLogsTab";
            salesLogsTab.Size = new Size(112, 26);
            salesLogsTab.TabIndex = 0;
            salesLogsTab.Text = "Sales Audit Logs";
            // 
            // movementTab
            // 
            movementTab.BackColor = Color.FromArgb(243, 247, 252);
            movementTab.Font = new Font("Segoe UI", 8.5F);
            movementTab.Location = new Point(0, 0);
            movementTab.Margin = new Padding(0, 3, 8, 3);
            movementTab.Name = "movementTab";
            movementTab.Size = new Size(164, 26);
            movementTab.TabIndex = 1;
            movementTab.Text = "Inventory Movement Report";
            // 
            // summaryTab
            // 
            summaryTab.BackColor = Color.FromArgb(243, 247, 252);
            summaryTab.Font = new Font("Segoe UI", 8.5F);
            summaryTab.Location = new Point(0, 0);
            summaryTab.Margin = new Padding(0, 3, 8, 3);
            summaryTab.Name = "summaryTab";
            summaryTab.Size = new Size(98, 26);
            summaryTab.TabIndex = 2;
            summaryTab.Text = "Audit Summary";
            // 
            // gapTwo
            // 
            gapTwo.BackColor = Color.FromArgb(243, 247, 252);
            gapTwo.Dock = DockStyle.Top;
            gapTwo.Location = new Point(0, 0);
            gapTwo.Name = "gapTwo";
            gapTwo.Size = new Size(10, 16);
            gapTwo.TabIndex = 3;
            // 
            // auditStats
            // 
            auditStats.BackColor = Color.FromArgb(243, 247, 252);
            auditStats.ColumnCount = 7;
            auditStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            auditStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            auditStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            auditStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            auditStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            auditStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            auditStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            auditStats.Controls.Add(auditSalesTile, 0, 0);
            auditStats.Controls.Add(auditMovementTile, 2, 0);
            auditStats.Controls.Add(auditDiscrepanciesTile, 4, 0);
            auditStats.Controls.Add(auditPeriodTile, 6, 0);
            auditStats.Dock = DockStyle.Top;
            auditStats.Location = new Point(0, 0);
            auditStats.Margin = new Padding(0, 0, 0, 0);
            auditStats.Name = "auditStats";
            auditStats.RowCount = 1;
            auditStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            auditStats.Size = new Size(700, 100);
            auditStats.TabIndex = 4;
            // 
            // auditSalesTile
            // 
            auditSalesTile.BorderColor = Color.FromArgb(227, 233, 242);
            auditSalesTile.Controls.Add(auditSalesDelta);
            auditSalesTile.Controls.Add(auditSalesValue);
            auditSalesTile.Controls.Add(auditSalesTop);
            auditSalesTile.CornerColor = Color.FromArgb(243, 247, 252);
            auditSalesTile.Dock = DockStyle.Fill;
            auditSalesTile.FillColor = Color.White;
            auditSalesTile.Location = new Point(0, 0);
            auditSalesTile.Margin = new Padding(0, 0, 0, 0);
            auditSalesTile.Name = "auditSalesTile";
            auditSalesTile.Padding = new Padding(12, 10, 12, 8);
            auditSalesTile.Radius = 10;
            auditSalesTile.Size = new Size(160, 100);
            auditSalesTile.TabIndex = 0;
            // 
            // auditSalesDelta
            // 
            auditSalesDelta.BackColor = Color.White;
            auditSalesDelta.Dock = DockStyle.Top;
            auditSalesDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditSalesDelta.ForeColor = Color.FromArgb(122, 136, 156);
            auditSalesDelta.Location = new Point(0, 0);
            auditSalesDelta.Name = "auditSalesDelta";
            auditSalesDelta.Size = new Size(130, 16);
            auditSalesDelta.TabIndex = 0;
            auditSalesDelta.Text = "\u2014 vs. last month";
            auditSalesDelta.TextAlign = ContentAlignment.MiddleLeft;
            auditSalesDelta.UseMnemonic = false;
            // 
            // auditSalesValue
            // 
            auditSalesValue.BackColor = Color.White;
            auditSalesValue.Dock = DockStyle.Top;
            auditSalesValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditSalesValue.ForeColor = Color.FromArgb(31, 42, 60);
            auditSalesValue.Location = new Point(0, 0);
            auditSalesValue.Name = "auditSalesValue";
            auditSalesValue.Size = new Size(130, 28);
            auditSalesValue.TabIndex = 1;
            auditSalesValue.Text = "\u20B10.00";
            auditSalesValue.TextAlign = ContentAlignment.MiddleLeft;
            auditSalesValue.UseMnemonic = false;
            // 
            // auditSalesTop
            // 
            auditSalesTop.BackColor = Color.White;
            auditSalesTop.Controls.Add(auditSalesLabel);
            auditSalesTop.Controls.Add(auditSalesIconHost);
            auditSalesTop.Dock = DockStyle.Top;
            auditSalesTop.Location = new Point(0, 0);
            auditSalesTop.Name = "auditSalesTop";
            auditSalesTop.Size = new Size(130, 34);
            auditSalesTop.TabIndex = 2;
            // 
            // auditSalesLabel
            // 
            auditSalesLabel.BackColor = Color.White;
            auditSalesLabel.Dock = DockStyle.Fill;
            auditSalesLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditSalesLabel.ForeColor = Color.FromArgb(68, 83, 106);
            auditSalesLabel.Location = new Point(0, 0);
            auditSalesLabel.Name = "auditSalesLabel";
            auditSalesLabel.Size = new Size(90, 34);
            auditSalesLabel.TabIndex = 0;
            auditSalesLabel.Text = "Total Sales (Audited)";
            auditSalesLabel.TextAlign = ContentAlignment.MiddleLeft;
            auditSalesLabel.UseMnemonic = false;
            // 
            // auditSalesIconHost
            // 
            auditSalesIconHost.BackColor = Color.White;
            auditSalesIconHost.Controls.Add(auditSalesIcon);
            auditSalesIconHost.Dock = DockStyle.Left;
            auditSalesIconHost.Location = new Point(0, 0);
            auditSalesIconHost.Name = "auditSalesIconHost";
            auditSalesIconHost.Size = new Size(38, 34);
            auditSalesIconHost.TabIndex = 1;
            // 
            // auditSalesIcon
            // 
            auditSalesIcon.BackColor = Color.White;
            auditSalesIcon.FillColor = Color.FromArgb(234, 242, 253);
            auditSalesIcon.ForeColor = Color.FromArgb(58, 123, 213);
            auditSalesIcon.Glyph = "\uE8C7";
            auditSalesIcon.GlyphSize = 10F;
            auditSalesIcon.Location = new Point(0, 2);
            auditSalesIcon.Name = "auditSalesIcon";
            auditSalesIcon.Radius = 8;
            auditSalesIcon.Size = new Size(30, 30);
            auditSalesIcon.TabIndex = 0;
            auditSalesIcon.TabStop = false;
            // 
            // auditMovementTile
            // 
            auditMovementTile.BorderColor = Color.FromArgb(227, 233, 242);
            auditMovementTile.Controls.Add(auditMovementDelta);
            auditMovementTile.Controls.Add(auditMovementValue);
            auditMovementTile.Controls.Add(auditMovementTop);
            auditMovementTile.CornerColor = Color.FromArgb(243, 247, 252);
            auditMovementTile.Dock = DockStyle.Fill;
            auditMovementTile.FillColor = Color.White;
            auditMovementTile.Location = new Point(0, 0);
            auditMovementTile.Margin = new Padding(0, 0, 0, 0);
            auditMovementTile.Name = "auditMovementTile";
            auditMovementTile.Padding = new Padding(12, 10, 12, 8);
            auditMovementTile.Radius = 10;
            auditMovementTile.Size = new Size(160, 100);
            auditMovementTile.TabIndex = 1;
            // 
            // auditMovementDelta
            // 
            auditMovementDelta.BackColor = Color.White;
            auditMovementDelta.Dock = DockStyle.Top;
            auditMovementDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditMovementDelta.ForeColor = Color.FromArgb(122, 136, 156);
            auditMovementDelta.Location = new Point(0, 0);
            auditMovementDelta.Name = "auditMovementDelta";
            auditMovementDelta.Size = new Size(130, 16);
            auditMovementDelta.TabIndex = 0;
            auditMovementDelta.Text = "\u2014 vs. last month";
            auditMovementDelta.TextAlign = ContentAlignment.MiddleLeft;
            auditMovementDelta.UseMnemonic = false;
            // 
            // auditMovementValue
            // 
            auditMovementValue.BackColor = Color.White;
            auditMovementValue.Dock = DockStyle.Top;
            auditMovementValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditMovementValue.ForeColor = Color.FromArgb(31, 42, 60);
            auditMovementValue.Location = new Point(0, 0);
            auditMovementValue.Name = "auditMovementValue";
            auditMovementValue.Size = new Size(130, 28);
            auditMovementValue.TabIndex = 1;
            auditMovementValue.Text = "0 items";
            auditMovementValue.TextAlign = ContentAlignment.MiddleLeft;
            auditMovementValue.UseMnemonic = false;
            // 
            // auditMovementTop
            // 
            auditMovementTop.BackColor = Color.White;
            auditMovementTop.Controls.Add(auditMovementLabel);
            auditMovementTop.Controls.Add(auditMovementIconHost);
            auditMovementTop.Dock = DockStyle.Top;
            auditMovementTop.Location = new Point(0, 0);
            auditMovementTop.Name = "auditMovementTop";
            auditMovementTop.Size = new Size(130, 34);
            auditMovementTop.TabIndex = 2;
            // 
            // auditMovementLabel
            // 
            auditMovementLabel.BackColor = Color.White;
            auditMovementLabel.Dock = DockStyle.Fill;
            auditMovementLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditMovementLabel.ForeColor = Color.FromArgb(68, 83, 106);
            auditMovementLabel.Location = new Point(0, 0);
            auditMovementLabel.Name = "auditMovementLabel";
            auditMovementLabel.Size = new Size(90, 34);
            auditMovementLabel.TabIndex = 0;
            auditMovementLabel.Text = "Total Inventory Movement";
            auditMovementLabel.TextAlign = ContentAlignment.MiddleLeft;
            auditMovementLabel.UseMnemonic = false;
            // 
            // auditMovementIconHost
            // 
            auditMovementIconHost.BackColor = Color.White;
            auditMovementIconHost.Controls.Add(auditMovementIcon);
            auditMovementIconHost.Dock = DockStyle.Left;
            auditMovementIconHost.Location = new Point(0, 0);
            auditMovementIconHost.Name = "auditMovementIconHost";
            auditMovementIconHost.Size = new Size(38, 34);
            auditMovementIconHost.TabIndex = 1;
            // 
            // auditMovementIcon
            // 
            auditMovementIcon.BackColor = Color.White;
            auditMovementIcon.FillColor = Color.FromArgb(234, 242, 253);
            auditMovementIcon.ForeColor = Color.FromArgb(58, 123, 213);
            auditMovementIcon.Glyph = "\uE7B8";
            auditMovementIcon.GlyphSize = 10F;
            auditMovementIcon.Location = new Point(0, 2);
            auditMovementIcon.Name = "auditMovementIcon";
            auditMovementIcon.Radius = 8;
            auditMovementIcon.Size = new Size(30, 30);
            auditMovementIcon.TabIndex = 0;
            auditMovementIcon.TabStop = false;
            // 
            // auditDiscrepanciesTile
            // 
            auditDiscrepanciesTile.BorderColor = Color.FromArgb(227, 233, 242);
            auditDiscrepanciesTile.Controls.Add(auditDiscrepanciesDelta);
            auditDiscrepanciesTile.Controls.Add(auditDiscrepanciesValue);
            auditDiscrepanciesTile.Controls.Add(auditDiscrepanciesTop);
            auditDiscrepanciesTile.CornerColor = Color.FromArgb(243, 247, 252);
            auditDiscrepanciesTile.Dock = DockStyle.Fill;
            auditDiscrepanciesTile.FillColor = Color.White;
            auditDiscrepanciesTile.Location = new Point(0, 0);
            auditDiscrepanciesTile.Margin = new Padding(0, 0, 0, 0);
            auditDiscrepanciesTile.Name = "auditDiscrepanciesTile";
            auditDiscrepanciesTile.Padding = new Padding(12, 10, 12, 8);
            auditDiscrepanciesTile.Radius = 10;
            auditDiscrepanciesTile.Size = new Size(160, 100);
            auditDiscrepanciesTile.TabIndex = 2;
            // 
            // auditDiscrepanciesDelta
            // 
            auditDiscrepanciesDelta.BackColor = Color.White;
            auditDiscrepanciesDelta.Dock = DockStyle.Top;
            auditDiscrepanciesDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditDiscrepanciesDelta.ForeColor = Color.FromArgb(122, 136, 156);
            auditDiscrepanciesDelta.Location = new Point(0, 0);
            auditDiscrepanciesDelta.Name = "auditDiscrepanciesDelta";
            auditDiscrepanciesDelta.Size = new Size(130, 16);
            auditDiscrepanciesDelta.TabIndex = 0;
            auditDiscrepanciesDelta.Text = "\u2014 vs. last month";
            auditDiscrepanciesDelta.TextAlign = ContentAlignment.MiddleLeft;
            auditDiscrepanciesDelta.UseMnemonic = false;
            // 
            // auditDiscrepanciesValue
            // 
            auditDiscrepanciesValue.BackColor = Color.White;
            auditDiscrepanciesValue.Dock = DockStyle.Top;
            auditDiscrepanciesValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditDiscrepanciesValue.ForeColor = Color.FromArgb(31, 42, 60);
            auditDiscrepanciesValue.Location = new Point(0, 0);
            auditDiscrepanciesValue.Name = "auditDiscrepanciesValue";
            auditDiscrepanciesValue.Size = new Size(130, 28);
            auditDiscrepanciesValue.TabIndex = 1;
            auditDiscrepanciesValue.Text = "0";
            auditDiscrepanciesValue.TextAlign = ContentAlignment.MiddleLeft;
            auditDiscrepanciesValue.UseMnemonic = false;
            // 
            // auditDiscrepanciesTop
            // 
            auditDiscrepanciesTop.BackColor = Color.White;
            auditDiscrepanciesTop.Controls.Add(auditDiscrepanciesLabel);
            auditDiscrepanciesTop.Controls.Add(auditDiscrepanciesIconHost);
            auditDiscrepanciesTop.Dock = DockStyle.Top;
            auditDiscrepanciesTop.Location = new Point(0, 0);
            auditDiscrepanciesTop.Name = "auditDiscrepanciesTop";
            auditDiscrepanciesTop.Size = new Size(130, 34);
            auditDiscrepanciesTop.TabIndex = 2;
            // 
            // auditDiscrepanciesLabel
            // 
            auditDiscrepanciesLabel.BackColor = Color.White;
            auditDiscrepanciesLabel.Dock = DockStyle.Fill;
            auditDiscrepanciesLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditDiscrepanciesLabel.ForeColor = Color.FromArgb(68, 83, 106);
            auditDiscrepanciesLabel.Location = new Point(0, 0);
            auditDiscrepanciesLabel.Name = "auditDiscrepanciesLabel";
            auditDiscrepanciesLabel.Size = new Size(90, 34);
            auditDiscrepanciesLabel.TabIndex = 0;
            auditDiscrepanciesLabel.Text = "Discrepancies Found";
            auditDiscrepanciesLabel.TextAlign = ContentAlignment.MiddleLeft;
            auditDiscrepanciesLabel.UseMnemonic = false;
            // 
            // auditDiscrepanciesIconHost
            // 
            auditDiscrepanciesIconHost.BackColor = Color.White;
            auditDiscrepanciesIconHost.Controls.Add(auditDiscrepanciesIcon);
            auditDiscrepanciesIconHost.Dock = DockStyle.Left;
            auditDiscrepanciesIconHost.Location = new Point(0, 0);
            auditDiscrepanciesIconHost.Name = "auditDiscrepanciesIconHost";
            auditDiscrepanciesIconHost.Size = new Size(38, 34);
            auditDiscrepanciesIconHost.TabIndex = 1;
            // 
            // auditDiscrepanciesIcon
            // 
            auditDiscrepanciesIcon.BackColor = Color.White;
            auditDiscrepanciesIcon.FillColor = Color.FromArgb(234, 242, 253);
            auditDiscrepanciesIcon.ForeColor = Color.FromArgb(58, 123, 213);
            auditDiscrepanciesIcon.Glyph = "\uE7BA";
            auditDiscrepanciesIcon.GlyphSize = 10F;
            auditDiscrepanciesIcon.Location = new Point(0, 2);
            auditDiscrepanciesIcon.Name = "auditDiscrepanciesIcon";
            auditDiscrepanciesIcon.Radius = 8;
            auditDiscrepanciesIcon.Size = new Size(30, 30);
            auditDiscrepanciesIcon.TabIndex = 0;
            auditDiscrepanciesIcon.TabStop = false;
            // 
            // auditPeriodTile
            // 
            auditPeriodTile.BorderColor = Color.FromArgb(227, 233, 242);
            auditPeriodTile.Controls.Add(auditPeriodValue);
            auditPeriodTile.Controls.Add(auditPeriodTop);
            auditPeriodTile.CornerColor = Color.FromArgb(243, 247, 252);
            auditPeriodTile.Dock = DockStyle.Fill;
            auditPeriodTile.FillColor = Color.White;
            auditPeriodTile.Location = new Point(0, 0);
            auditPeriodTile.Margin = new Padding(0, 0, 0, 0);
            auditPeriodTile.Name = "auditPeriodTile";
            auditPeriodTile.Padding = new Padding(12, 10, 12, 8);
            auditPeriodTile.Radius = 10;
            auditPeriodTile.Size = new Size(160, 100);
            auditPeriodTile.TabIndex = 3;
            // 
            // auditPeriodValue
            // 
            auditPeriodValue.BackColor = Color.White;
            auditPeriodValue.Dock = DockStyle.Top;
            auditPeriodValue.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            auditPeriodValue.ForeColor = Color.FromArgb(31, 42, 60);
            auditPeriodValue.Location = new Point(0, 0);
            auditPeriodValue.Name = "auditPeriodValue";
            auditPeriodValue.Size = new Size(130, 28);
            auditPeriodValue.TabIndex = 0;
            auditPeriodValue.Text = "All dates";
            auditPeriodValue.TextAlign = ContentAlignment.MiddleLeft;
            auditPeriodValue.UseMnemonic = false;
            // 
            // auditPeriodTop
            // 
            auditPeriodTop.BackColor = Color.White;
            auditPeriodTop.Controls.Add(auditPeriodLabel);
            auditPeriodTop.Controls.Add(auditPeriodIconHost);
            auditPeriodTop.Dock = DockStyle.Top;
            auditPeriodTop.Location = new Point(0, 0);
            auditPeriodTop.Name = "auditPeriodTop";
            auditPeriodTop.Size = new Size(130, 34);
            auditPeriodTop.TabIndex = 1;
            // 
            // auditPeriodLabel
            // 
            auditPeriodLabel.BackColor = Color.White;
            auditPeriodLabel.Dock = DockStyle.Fill;
            auditPeriodLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            auditPeriodLabel.ForeColor = Color.FromArgb(68, 83, 106);
            auditPeriodLabel.Location = new Point(0, 0);
            auditPeriodLabel.Name = "auditPeriodLabel";
            auditPeriodLabel.Size = new Size(90, 34);
            auditPeriodLabel.TabIndex = 0;
            auditPeriodLabel.Text = "Audit Period";
            auditPeriodLabel.TextAlign = ContentAlignment.MiddleLeft;
            auditPeriodLabel.UseMnemonic = false;
            // 
            // auditPeriodIconHost
            // 
            auditPeriodIconHost.BackColor = Color.White;
            auditPeriodIconHost.Controls.Add(auditPeriodIcon);
            auditPeriodIconHost.Dock = DockStyle.Left;
            auditPeriodIconHost.Location = new Point(0, 0);
            auditPeriodIconHost.Name = "auditPeriodIconHost";
            auditPeriodIconHost.Size = new Size(38, 34);
            auditPeriodIconHost.TabIndex = 1;
            // 
            // auditPeriodIcon
            // 
            auditPeriodIcon.BackColor = Color.White;
            auditPeriodIcon.FillColor = Color.FromArgb(234, 242, 253);
            auditPeriodIcon.ForeColor = Color.FromArgb(58, 123, 213);
            auditPeriodIcon.Glyph = "\uE787";
            auditPeriodIcon.GlyphSize = 10F;
            auditPeriodIcon.Location = new Point(0, 2);
            auditPeriodIcon.Name = "auditPeriodIcon";
            auditPeriodIcon.Radius = 8;
            auditPeriodIcon.Size = new Size(30, 30);
            auditPeriodIcon.TabIndex = 0;
            auditPeriodIcon.TabStop = false;
            // 
            // gapOne
            // 
            gapOne.BackColor = Color.FromArgb(243, 247, 252);
            gapOne.Dock = DockStyle.Top;
            gapOne.Location = new Point(0, 0);
            gapOne.Name = "gapOne";
            gapOne.Size = new Size(10, 16);
            gapOne.TabIndex = 5;
            // 
            // reportsHeaderCard
            // 
            reportsHeaderCard.BorderColor = Color.FromArgb(227, 233, 242);
            reportsHeaderCard.Controls.Add(reportsHeaderText);
            reportsHeaderCard.Controls.Add(reportsIconHost);
            reportsHeaderCard.Controls.Add(downloadReportHost);
            reportsHeaderCard.CornerColor = Color.FromArgb(243, 247, 252);
            reportsHeaderCard.Dock = DockStyle.Top;
            reportsHeaderCard.FillColor = Color.White;
            reportsHeaderCard.Location = new Point(0, 0);
            reportsHeaderCard.Name = "reportsHeaderCard";
            reportsHeaderCard.Padding = new Padding(16, 12, 16, 12);
            reportsHeaderCard.Radius = 10;
            reportsHeaderCard.Size = new Size(700, 84);
            reportsHeaderCard.TabIndex = 6;
            // 
            // reportsHeaderText
            // 
            reportsHeaderText.BackColor = Color.White;
            reportsHeaderText.Controls.Add(reportsSubtitle);
            reportsHeaderText.Controls.Add(reportsTitle);
            reportsHeaderText.Dock = DockStyle.Fill;
            reportsHeaderText.Location = new Point(0, 0);
            reportsHeaderText.Name = "reportsHeaderText";
            reportsHeaderText.Padding = new Padding(0, 6, 0, 0);
            reportsHeaderText.Size = new Size(420, 60);
            reportsHeaderText.TabIndex = 0;
            // 
            // reportsSubtitle
            // 
            reportsSubtitle.AutoEllipsis = true;
            reportsSubtitle.BackColor = Color.White;
            reportsSubtitle.Dock = DockStyle.Top;
            reportsSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            reportsSubtitle.ForeColor = Color.FromArgb(122, 136, 156);
            reportsSubtitle.Location = new Point(0, 0);
            reportsSubtitle.Name = "reportsSubtitle";
            reportsSubtitle.Size = new Size(420, 18);
            reportsSubtitle.TabIndex = 0;
            reportsSubtitle.Text = "Sales audit logs, inventory movement report, and export options for audit summaries.";
            reportsSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            reportsSubtitle.UseMnemonic = false;
            // 
            // reportsTitle
            // 
            reportsTitle.BackColor = Color.White;
            reportsTitle.Dock = DockStyle.Top;
            reportsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            reportsTitle.ForeColor = Color.FromArgb(31, 42, 60);
            reportsTitle.Location = new Point(0, 0);
            reportsTitle.Name = "reportsTitle";
            reportsTitle.Size = new Size(420, 24);
            reportsTitle.TabIndex = 1;
            reportsTitle.Text = "Reports & Financial Auditing";
            reportsTitle.TextAlign = ContentAlignment.MiddleLeft;
            reportsTitle.UseMnemonic = false;
            // 
            // reportsIconHost
            // 
            reportsIconHost.BackColor = Color.White;
            reportsIconHost.Controls.Add(reportsIcon);
            reportsIconHost.Dock = DockStyle.Left;
            reportsIconHost.Location = new Point(0, 0);
            reportsIconHost.Name = "reportsIconHost";
            reportsIconHost.Size = new Size(56, 60);
            reportsIconHost.TabIndex = 1;
            // 
            // reportsIcon
            // 
            reportsIcon.Anchor = AnchorStyles.Left;
            reportsIcon.BackColor = Color.White;
            reportsIcon.FillColor = Color.FromArgb(234, 242, 253);
            reportsIcon.ForeColor = Color.FromArgb(58, 123, 213);
            reportsIcon.Glyph = "\uE9F9";
            reportsIcon.GlyphSize = 14F;
            reportsIcon.Location = new Point(0, 10);
            reportsIcon.Name = "reportsIcon";
            reportsIcon.Radius = 10;
            reportsIcon.Size = new Size(40, 40);
            reportsIcon.TabIndex = 0;
            reportsIcon.TabStop = false;
            // 
            // downloadReportHost
            // 
            downloadReportHost.BackColor = Color.White;
            downloadReportHost.Controls.Add(downloadReportButton);
            downloadReportHost.Dock = DockStyle.Right;
            downloadReportHost.Location = new Point(0, 0);
            downloadReportHost.Name = "downloadReportHost";
            downloadReportHost.Size = new Size(164, 60);
            downloadReportHost.TabIndex = 2;
            // 
            // downloadReportButton
            // 
            downloadReportButton.Anchor = AnchorStyles.Right;
            downloadReportButton.BackColor = Color.White;
            downloadReportButton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            downloadReportButton.Location = new Point(0, 12);
            downloadReportButton.Name = "downloadReportButton";
            downloadReportButton.Size = new Size(148, 36);
            downloadReportButton.TabIndex = 0;
            downloadReportButton.Text = "Download Report";
            // 
            // rightColumn
            // 
            rightColumn.BackColor = Color.FromArgb(243, 247, 252);
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightColumn.Controls.Add(quickReportsCard, 0, 0);
            rightColumn.Controls.Add(recentActivityCard, 0, 1);
            rightColumn.Controls.Add(exportCard, 0, 2);
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(0, 0);
            rightColumn.Margin = new Padding(0, 0, 0, 0);
            rightColumn.Name = "rightColumn";
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 282F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 142F));
            rightColumn.Size = new Size(280, 692);
            rightColumn.TabIndex = 1;
            // 
            // quickReportsCard
            // 
            quickReportsCard.BorderColor = Color.FromArgb(227, 233, 242);
            quickReportsCard.Controls.Add(printableReportRow);
            quickReportsCard.Controls.Add(financialReportRow);
            quickReportsCard.Controls.Add(movementReportRow);
            quickReportsCard.Controls.Add(salesReportRow);
            quickReportsCard.Controls.Add(quickReportsHeader);
            quickReportsCard.CornerColor = Color.FromArgb(243, 247, 252);
            quickReportsCard.Dock = DockStyle.Fill;
            quickReportsCard.FillColor = Color.White;
            quickReportsCard.Location = new Point(0, 0);
            quickReportsCard.Margin = new Padding(0, 0, 0, 16);
            quickReportsCard.Name = "quickReportsCard";
            quickReportsCard.Padding = new Padding(16, 12, 16, 12);
            quickReportsCard.Radius = 10;
            quickReportsCard.Size = new Size(280, 266);
            quickReportsCard.TabIndex = 0;
            // 
            // printableReportRow
            // 
            printableReportRow.BackColor = Color.White;
            printableReportRow.Controls.Add(printableReportTextHost);
            printableReportRow.Controls.Add(printableReportChevron);
            printableReportRow.Controls.Add(printableReportIconHost);
            printableReportRow.Cursor = Cursors.Hand;
            printableReportRow.Dock = DockStyle.Top;
            printableReportRow.Location = new Point(0, 0);
            printableReportRow.Name = "printableReportRow";
            printableReportRow.Size = new Size(248, 52);
            printableReportRow.TabIndex = 0;
            // 
            // printableReportTextHost
            // 
            printableReportTextHost.BackColor = Color.White;
            printableReportTextHost.Controls.Add(printableReportDesc);
            printableReportTextHost.Controls.Add(printableReportTitle);
            printableReportTextHost.Cursor = Cursors.Hand;
            printableReportTextHost.Dock = DockStyle.Fill;
            printableReportTextHost.Location = new Point(0, 0);
            printableReportTextHost.Name = "printableReportTextHost";
            printableReportTextHost.Padding = new Padding(0, 8, 0, 0);
            printableReportTextHost.Size = new Size(190, 52);
            printableReportTextHost.TabIndex = 0;
            // 
            // printableReportDesc
            // 
            printableReportDesc.AutoEllipsis = true;
            printableReportDesc.BackColor = Color.White;
            printableReportDesc.Dock = DockStyle.Top;
            printableReportDesc.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            printableReportDesc.ForeColor = Color.FromArgb(122, 136, 156);
            printableReportDesc.Location = new Point(0, 0);
            printableReportDesc.Name = "printableReportDesc";
            printableReportDesc.Size = new Size(190, 16);
            printableReportDesc.TabIndex = 0;
            printableReportDesc.Text = "Get audit-ready PDF";
            printableReportDesc.TextAlign = ContentAlignment.MiddleLeft;
            printableReportDesc.UseMnemonic = false;
            // 
            // printableReportTitle
            // 
            printableReportTitle.BackColor = Color.White;
            printableReportTitle.Dock = DockStyle.Top;
            printableReportTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            printableReportTitle.ForeColor = Color.FromArgb(31, 42, 60);
            printableReportTitle.Location = new Point(0, 0);
            printableReportTitle.Name = "printableReportTitle";
            printableReportTitle.Size = new Size(190, 18);
            printableReportTitle.TabIndex = 1;
            printableReportTitle.Text = "Download Printable Report";
            printableReportTitle.TextAlign = ContentAlignment.MiddleLeft;
            printableReportTitle.UseMnemonic = false;
            // 
            // printableReportChevron
            // 
            printableReportChevron.BackColor = Color.White;
            printableReportChevron.Dock = DockStyle.Right;
            printableReportChevron.Font = new Font("Segoe MDL2 Assets", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            printableReportChevron.ForeColor = Color.FromArgb(122, 136, 156);
            printableReportChevron.Location = new Point(0, 0);
            printableReportChevron.Name = "printableReportChevron";
            printableReportChevron.Size = new Size(20, 52);
            printableReportChevron.TabIndex = 1;
            printableReportChevron.Text = "\uE76C";
            printableReportChevron.TextAlign = ContentAlignment.MiddleCenter;
            printableReportChevron.UseMnemonic = false;
            // 
            // printableReportIconHost
            // 
            printableReportIconHost.BackColor = Color.White;
            printableReportIconHost.Controls.Add(printableReportIcon);
            printableReportIconHost.Cursor = Cursors.Hand;
            printableReportIconHost.Dock = DockStyle.Left;
            printableReportIconHost.Location = new Point(0, 0);
            printableReportIconHost.Name = "printableReportIconHost";
            printableReportIconHost.Size = new Size(38, 52);
            printableReportIconHost.TabIndex = 2;
            // 
            // printableReportIcon
            // 
            printableReportIcon.BackColor = Color.White;
            printableReportIcon.FillColor = Color.FromArgb(234, 242, 253);
            printableReportIcon.ForeColor = Color.FromArgb(58, 123, 213);
            printableReportIcon.Glyph = "\uE749";
            printableReportIcon.GlyphSize = 10F;
            printableReportIcon.Location = new Point(0, 12);
            printableReportIcon.Name = "printableReportIcon";
            printableReportIcon.Radius = 6;
            printableReportIcon.Size = new Size(28, 28);
            printableReportIcon.TabIndex = 0;
            printableReportIcon.TabStop = false;
            // 
            // financialReportRow
            // 
            financialReportRow.BackColor = Color.White;
            financialReportRow.Controls.Add(financialReportTextHost);
            financialReportRow.Controls.Add(financialReportChevron);
            financialReportRow.Controls.Add(financialReportIconHost);
            financialReportRow.Cursor = Cursors.Hand;
            financialReportRow.Dock = DockStyle.Top;
            financialReportRow.Location = new Point(0, 0);
            financialReportRow.Name = "financialReportRow";
            financialReportRow.Size = new Size(248, 52);
            financialReportRow.TabIndex = 1;
            // 
            // financialReportTextHost
            // 
            financialReportTextHost.BackColor = Color.White;
            financialReportTextHost.Controls.Add(financialReportDesc);
            financialReportTextHost.Controls.Add(financialReportTitle);
            financialReportTextHost.Cursor = Cursors.Hand;
            financialReportTextHost.Dock = DockStyle.Fill;
            financialReportTextHost.Location = new Point(0, 0);
            financialReportTextHost.Name = "financialReportTextHost";
            financialReportTextHost.Padding = new Padding(0, 8, 0, 0);
            financialReportTextHost.Size = new Size(190, 52);
            financialReportTextHost.TabIndex = 0;
            // 
            // financialReportDesc
            // 
            financialReportDesc.AutoEllipsis = true;
            financialReportDesc.BackColor = Color.White;
            financialReportDesc.Dock = DockStyle.Top;
            financialReportDesc.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            financialReportDesc.ForeColor = Color.FromArgb(122, 136, 156);
            financialReportDesc.Location = new Point(0, 0);
            financialReportDesc.Name = "financialReportDesc";
            financialReportDesc.Size = new Size(190, 16);
            financialReportDesc.TabIndex = 0;
            financialReportDesc.Text = "Revenue, expenses, and profitability";
            financialReportDesc.TextAlign = ContentAlignment.MiddleLeft;
            financialReportDesc.UseMnemonic = false;
            // 
            // financialReportTitle
            // 
            financialReportTitle.BackColor = Color.White;
            financialReportTitle.Dock = DockStyle.Top;
            financialReportTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            financialReportTitle.ForeColor = Color.FromArgb(31, 42, 60);
            financialReportTitle.Location = new Point(0, 0);
            financialReportTitle.Name = "financialReportTitle";
            financialReportTitle.Size = new Size(190, 18);
            financialReportTitle.TabIndex = 1;
            financialReportTitle.Text = "Financial Summary";
            financialReportTitle.TextAlign = ContentAlignment.MiddleLeft;
            financialReportTitle.UseMnemonic = false;
            // 
            // financialReportChevron
            // 
            financialReportChevron.BackColor = Color.White;
            financialReportChevron.Dock = DockStyle.Right;
            financialReportChevron.Font = new Font("Segoe MDL2 Assets", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            financialReportChevron.ForeColor = Color.FromArgb(122, 136, 156);
            financialReportChevron.Location = new Point(0, 0);
            financialReportChevron.Name = "financialReportChevron";
            financialReportChevron.Size = new Size(20, 52);
            financialReportChevron.TabIndex = 1;
            financialReportChevron.Text = "\uE76C";
            financialReportChevron.TextAlign = ContentAlignment.MiddleCenter;
            financialReportChevron.UseMnemonic = false;
            // 
            // financialReportIconHost
            // 
            financialReportIconHost.BackColor = Color.White;
            financialReportIconHost.Controls.Add(financialReportIcon);
            financialReportIconHost.Cursor = Cursors.Hand;
            financialReportIconHost.Dock = DockStyle.Left;
            financialReportIconHost.Location = new Point(0, 0);
            financialReportIconHost.Name = "financialReportIconHost";
            financialReportIconHost.Size = new Size(38, 52);
            financialReportIconHost.TabIndex = 2;
            // 
            // financialReportIcon
            // 
            financialReportIcon.BackColor = Color.White;
            financialReportIcon.FillColor = Color.FromArgb(234, 242, 253);
            financialReportIcon.ForeColor = Color.FromArgb(58, 123, 213);
            financialReportIcon.Glyph = "\uE9D9";
            financialReportIcon.GlyphSize = 10F;
            financialReportIcon.Location = new Point(0, 12);
            financialReportIcon.Name = "financialReportIcon";
            financialReportIcon.Radius = 6;
            financialReportIcon.Size = new Size(28, 28);
            financialReportIcon.TabIndex = 0;
            financialReportIcon.TabStop = false;
            // 
            // movementReportRow
            // 
            movementReportRow.BackColor = Color.White;
            movementReportRow.Controls.Add(movementReportTextHost);
            movementReportRow.Controls.Add(movementReportChevron);
            movementReportRow.Controls.Add(movementReportIconHost);
            movementReportRow.Cursor = Cursors.Hand;
            movementReportRow.Dock = DockStyle.Top;
            movementReportRow.Location = new Point(0, 0);
            movementReportRow.Name = "movementReportRow";
            movementReportRow.Size = new Size(248, 52);
            movementReportRow.TabIndex = 2;
            // 
            // movementReportTextHost
            // 
            movementReportTextHost.BackColor = Color.White;
            movementReportTextHost.Controls.Add(movementReportDesc);
            movementReportTextHost.Controls.Add(movementReportTitle);
            movementReportTextHost.Cursor = Cursors.Hand;
            movementReportTextHost.Dock = DockStyle.Fill;
            movementReportTextHost.Location = new Point(0, 0);
            movementReportTextHost.Name = "movementReportTextHost";
            movementReportTextHost.Padding = new Padding(0, 8, 0, 0);
            movementReportTextHost.Size = new Size(190, 52);
            movementReportTextHost.TabIndex = 0;
            // 
            // movementReportDesc
            // 
            movementReportDesc.AutoEllipsis = true;
            movementReportDesc.BackColor = Color.White;
            movementReportDesc.Dock = DockStyle.Top;
            movementReportDesc.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            movementReportDesc.ForeColor = Color.FromArgb(122, 136, 156);
            movementReportDesc.Location = new Point(0, 0);
            movementReportDesc.Name = "movementReportDesc";
            movementReportDesc.Size = new Size(190, 16);
            movementReportDesc.TabIndex = 0;
            movementReportDesc.Text = "Track stock in/out and adjustments";
            movementReportDesc.TextAlign = ContentAlignment.MiddleLeft;
            movementReportDesc.UseMnemonic = false;
            // 
            // movementReportTitle
            // 
            movementReportTitle.BackColor = Color.White;
            movementReportTitle.Dock = DockStyle.Top;
            movementReportTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            movementReportTitle.ForeColor = Color.FromArgb(31, 42, 60);
            movementReportTitle.Location = new Point(0, 0);
            movementReportTitle.Name = "movementReportTitle";
            movementReportTitle.Size = new Size(190, 18);
            movementReportTitle.TabIndex = 1;
            movementReportTitle.Text = "Inventory Movement Report";
            movementReportTitle.TextAlign = ContentAlignment.MiddleLeft;
            movementReportTitle.UseMnemonic = false;
            // 
            // movementReportChevron
            // 
            movementReportChevron.BackColor = Color.White;
            movementReportChevron.Dock = DockStyle.Right;
            movementReportChevron.Font = new Font("Segoe MDL2 Assets", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            movementReportChevron.ForeColor = Color.FromArgb(122, 136, 156);
            movementReportChevron.Location = new Point(0, 0);
            movementReportChevron.Name = "movementReportChevron";
            movementReportChevron.Size = new Size(20, 52);
            movementReportChevron.TabIndex = 1;
            movementReportChevron.Text = "\uE76C";
            movementReportChevron.TextAlign = ContentAlignment.MiddleCenter;
            movementReportChevron.UseMnemonic = false;
            // 
            // movementReportIconHost
            // 
            movementReportIconHost.BackColor = Color.White;
            movementReportIconHost.Controls.Add(movementReportIcon);
            movementReportIconHost.Cursor = Cursors.Hand;
            movementReportIconHost.Dock = DockStyle.Left;
            movementReportIconHost.Location = new Point(0, 0);
            movementReportIconHost.Name = "movementReportIconHost";
            movementReportIconHost.Size = new Size(38, 52);
            movementReportIconHost.TabIndex = 2;
            // 
            // movementReportIcon
            // 
            movementReportIcon.BackColor = Color.White;
            movementReportIcon.FillColor = Color.FromArgb(234, 242, 253);
            movementReportIcon.ForeColor = Color.FromArgb(58, 123, 213);
            movementReportIcon.Glyph = "\uE7B8";
            movementReportIcon.GlyphSize = 10F;
            movementReportIcon.Location = new Point(0, 12);
            movementReportIcon.Name = "movementReportIcon";
            movementReportIcon.Radius = 6;
            movementReportIcon.Size = new Size(28, 28);
            movementReportIcon.TabIndex = 0;
            movementReportIcon.TabStop = false;
            // 
            // salesReportRow
            // 
            salesReportRow.BackColor = Color.White;
            salesReportRow.Controls.Add(salesReportTextHost);
            salesReportRow.Controls.Add(salesReportChevron);
            salesReportRow.Controls.Add(salesReportIconHost);
            salesReportRow.Cursor = Cursors.Hand;
            salesReportRow.Dock = DockStyle.Top;
            salesReportRow.Location = new Point(0, 0);
            salesReportRow.Name = "salesReportRow";
            salesReportRow.Size = new Size(248, 52);
            salesReportRow.TabIndex = 3;
            // 
            // salesReportTextHost
            // 
            salesReportTextHost.BackColor = Color.White;
            salesReportTextHost.Controls.Add(salesReportDesc);
            salesReportTextHost.Controls.Add(salesReportTitle);
            salesReportTextHost.Cursor = Cursors.Hand;
            salesReportTextHost.Dock = DockStyle.Fill;
            salesReportTextHost.Location = new Point(0, 0);
            salesReportTextHost.Name = "salesReportTextHost";
            salesReportTextHost.Padding = new Padding(0, 8, 0, 0);
            salesReportTextHost.Size = new Size(190, 52);
            salesReportTextHost.TabIndex = 0;
            // 
            // salesReportDesc
            // 
            salesReportDesc.AutoEllipsis = true;
            salesReportDesc.BackColor = Color.White;
            salesReportDesc.Dock = DockStyle.Top;
            salesReportDesc.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            salesReportDesc.ForeColor = Color.FromArgb(122, 136, 156);
            salesReportDesc.Location = new Point(0, 0);
            salesReportDesc.Name = "salesReportDesc";
            salesReportDesc.Size = new Size(190, 16);
            salesReportDesc.TabIndex = 0;
            salesReportDesc.Text = "Detailed sales transactions and reconciliations";
            salesReportDesc.TextAlign = ContentAlignment.MiddleLeft;
            salesReportDesc.UseMnemonic = false;
            // 
            // salesReportTitle
            // 
            salesReportTitle.BackColor = Color.White;
            salesReportTitle.Dock = DockStyle.Top;
            salesReportTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            salesReportTitle.ForeColor = Color.FromArgb(31, 42, 60);
            salesReportTitle.Location = new Point(0, 0);
            salesReportTitle.Name = "salesReportTitle";
            salesReportTitle.Size = new Size(190, 18);
            salesReportTitle.TabIndex = 1;
            salesReportTitle.Text = "Sales Audit Log";
            salesReportTitle.TextAlign = ContentAlignment.MiddleLeft;
            salesReportTitle.UseMnemonic = false;
            // 
            // salesReportChevron
            // 
            salesReportChevron.BackColor = Color.White;
            salesReportChevron.Dock = DockStyle.Right;
            salesReportChevron.Font = new Font("Segoe MDL2 Assets", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            salesReportChevron.ForeColor = Color.FromArgb(122, 136, 156);
            salesReportChevron.Location = new Point(0, 0);
            salesReportChevron.Name = "salesReportChevron";
            salesReportChevron.Size = new Size(20, 52);
            salesReportChevron.TabIndex = 1;
            salesReportChevron.Text = "\uE76C";
            salesReportChevron.TextAlign = ContentAlignment.MiddleCenter;
            salesReportChevron.UseMnemonic = false;
            // 
            // salesReportIconHost
            // 
            salesReportIconHost.BackColor = Color.White;
            salesReportIconHost.Controls.Add(salesReportIcon);
            salesReportIconHost.Cursor = Cursors.Hand;
            salesReportIconHost.Dock = DockStyle.Left;
            salesReportIconHost.Location = new Point(0, 0);
            salesReportIconHost.Name = "salesReportIconHost";
            salesReportIconHost.Size = new Size(38, 52);
            salesReportIconHost.TabIndex = 2;
            // 
            // salesReportIcon
            // 
            salesReportIcon.BackColor = Color.White;
            salesReportIcon.FillColor = Color.FromArgb(234, 242, 253);
            salesReportIcon.ForeColor = Color.FromArgb(58, 123, 213);
            salesReportIcon.Glyph = "\uE8A5";
            salesReportIcon.GlyphSize = 10F;
            salesReportIcon.Location = new Point(0, 12);
            salesReportIcon.Name = "salesReportIcon";
            salesReportIcon.Radius = 6;
            salesReportIcon.Size = new Size(28, 28);
            salesReportIcon.TabIndex = 0;
            salesReportIcon.TabStop = false;
            // 
            // quickReportsHeader
            // 
            quickReportsHeader.BackColor = Color.White;
            quickReportsHeader.Controls.Add(quickReportsTitle);
            quickReportsHeader.Controls.Add(quickReportsIcon);
            quickReportsHeader.Controls.Add(quickReportsLink);
            quickReportsHeader.Dock = DockStyle.Top;
            quickReportsHeader.Location = new Point(0, 0);
            quickReportsHeader.Name = "quickReportsHeader";
            quickReportsHeader.Size = new Size(248, 34);
            quickReportsHeader.TabIndex = 4;
            // 
            // quickReportsTitle
            // 
            quickReportsTitle.BackColor = Color.White;
            quickReportsTitle.Dock = DockStyle.Fill;
            quickReportsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickReportsTitle.ForeColor = Color.FromArgb(31, 42, 60);
            quickReportsTitle.Location = new Point(26, 0);
            quickReportsTitle.Name = "quickReportsTitle";
            quickReportsTitle.Size = new Size(160, 34);
            quickReportsTitle.TabIndex = 0;
            quickReportsTitle.Text = "Quick Reports";
            quickReportsTitle.TextAlign = ContentAlignment.MiddleLeft;
            quickReportsTitle.UseMnemonic = false;
            // 
            // quickReportsIcon
            // 
            quickReportsIcon.BackColor = Color.White;
            quickReportsIcon.Dock = DockStyle.Left;
            quickReportsIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            quickReportsIcon.ForeColor = Color.FromArgb(58, 123, 213);
            quickReportsIcon.Location = new Point(0, 0);
            quickReportsIcon.Name = "quickReportsIcon";
            quickReportsIcon.Size = new Size(26, 34);
            quickReportsIcon.TabIndex = 1;
            quickReportsIcon.Text = "\uE945";
            quickReportsIcon.TextAlign = ContentAlignment.MiddleCenter;
            quickReportsIcon.UseMnemonic = false;
            // 
            // quickReportsLink
            // 
            quickReportsLink.BackColor = Color.White;
            quickReportsLink.Cursor = Cursors.Hand;
            quickReportsLink.Dock = DockStyle.Right;
            quickReportsLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickReportsLink.ForeColor = Color.FromArgb(58, 123, 213);
            quickReportsLink.Location = new Point(0, 0);
            quickReportsLink.Name = "quickReportsLink";
            quickReportsLink.Size = new Size(62, 34);
            quickReportsLink.TabIndex = 2;
            quickReportsLink.Text = "View All \u2192";
            quickReportsLink.TextAlign = ContentAlignment.MiddleRight;
            quickReportsLink.UseMnemonic = false;
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
            recentActivityEmpty.Hint = "Logged sales, returns and adjustments will appear here.";
            recentActivityEmpty.Location = new Point(0, 0);
            recentActivityEmpty.Name = "recentActivityEmpty";
            recentActivityEmpty.Size = new Size(248, 140);
            recentActivityEmpty.TabIndex = 0;
            recentActivityEmpty.TabStop = false;
            recentActivityEmpty.Title = "No audit activity yet";
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
            recentActivityTitle.Text = "Recent Audit Activity";
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
            recentActivityIcon.Text = "\uE81C";
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
            exportCard.Size = new Size(280, 142);
            exportCard.TabIndex = 2;
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
            exportTitle.Text = "Export Options";
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
            // ReportsAuditingPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            Controls.Add(pageHost);
            Name = "ReportsAuditingPage";
            Size = new Size(1036, 732);
            pageHost.ResumeLayout(false);
            pageGrid.ResumeLayout(false);
            leftColumn.ResumeLayout(false);
            auditTableCard.ResumeLayout(false);
            auditTableArea.ResumeLayout(false);
            auditRows.ResumeLayout(false);
            auditColumnsHead.ResumeLayout(false);
            auditColumnsGrid.ResumeLayout(false);
            auditFooter.ResumeLayout(false);
            auditPager.ResumeLayout(false);
            reportFiltersRow.ResumeLayout(false);
            reportTabsRow.ResumeLayout(false);
            reportTabs.ResumeLayout(false);
            auditStats.ResumeLayout(false);
            auditSalesTile.ResumeLayout(false);
            auditSalesTop.ResumeLayout(false);
            auditSalesIconHost.ResumeLayout(false);
            auditMovementTile.ResumeLayout(false);
            auditMovementTop.ResumeLayout(false);
            auditMovementIconHost.ResumeLayout(false);
            auditDiscrepanciesTile.ResumeLayout(false);
            auditDiscrepanciesTop.ResumeLayout(false);
            auditDiscrepanciesIconHost.ResumeLayout(false);
            auditPeriodTile.ResumeLayout(false);
            auditPeriodTop.ResumeLayout(false);
            auditPeriodIconHost.ResumeLayout(false);
            reportsHeaderCard.ResumeLayout(false);
            reportsHeaderText.ResumeLayout(false);
            reportsIconHost.ResumeLayout(false);
            downloadReportHost.ResumeLayout(false);
            rightColumn.ResumeLayout(false);
            quickReportsCard.ResumeLayout(false);
            printableReportRow.ResumeLayout(false);
            printableReportTextHost.ResumeLayout(false);
            printableReportIconHost.ResumeLayout(false);
            financialReportRow.ResumeLayout(false);
            financialReportTextHost.ResumeLayout(false);
            financialReportIconHost.ResumeLayout(false);
            movementReportRow.ResumeLayout(false);
            movementReportTextHost.ResumeLayout(false);
            movementReportIconHost.ResumeLayout(false);
            salesReportRow.ResumeLayout(false);
            salesReportTextHost.ResumeLayout(false);
            salesReportIconHost.ResumeLayout(false);
            quickReportsHeader.ResumeLayout(false);
            recentActivityCard.ResumeLayout(false);
            recentActivityHeader.ResumeLayout(false);
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
        private MyApp.Controls.CardPanel auditTableCard;
        private System.Windows.Forms.Panel auditTableArea;
        private System.Windows.Forms.Panel auditRows;
        private MyApp.Controls.EmptyState auditEmpty;
        private System.Windows.Forms.Panel auditColumnsHead;
        private System.Windows.Forms.TableLayoutPanel auditColumnsGrid;
        private MyApp.Controls.CheckMark auditHeaderCheck;
        private System.Windows.Forms.Label auditColDateTime;
        private System.Windows.Forms.Label auditColType;
        private System.Windows.Forms.Label auditColProduct;
        private System.Windows.Forms.Label auditColQuantity;
        private System.Windows.Forms.Label auditColAmount;
        private System.Windows.Forms.Label auditColReference;
        private System.Windows.Forms.Label auditColStatus;
        private System.Windows.Forms.Label auditColActions;
        private System.Windows.Forms.Panel auditFooter;
        private System.Windows.Forms.Label auditShowing;
        private System.Windows.Forms.Panel auditPager;
        private MyApp.Controls.IconButton auditPrevPage;
        private MyApp.Controls.PrimaryButton auditPageChip;
        private MyApp.Controls.IconButton auditNextPage;
        private System.Windows.Forms.Panel reportFiltersRow;
        private MyApp.Controls.DateRangeField auditDateRange;
        private System.Windows.Forms.Panel filterGapB;
        private MyApp.Controls.SelectField staffFilter;
        private System.Windows.Forms.Panel filterGapA;
        private MyApp.Controls.SelectField categoryFilter;
        private System.Windows.Forms.Panel reportTabsRow;
        private System.Windows.Forms.FlowLayoutPanel reportTabs;
        private MyApp.Controls.TabPill salesLogsTab;
        private MyApp.Controls.TabPill movementTab;
        private MyApp.Controls.TabPill summaryTab;
        private System.Windows.Forms.Panel gapTwo;
        private System.Windows.Forms.TableLayoutPanel auditStats;
        private MyApp.Controls.CardPanel auditSalesTile;
        private System.Windows.Forms.Label auditSalesDelta;
        private System.Windows.Forms.Label auditSalesValue;
        private System.Windows.Forms.Panel auditSalesTop;
        private System.Windows.Forms.Label auditSalesLabel;
        private System.Windows.Forms.Panel auditSalesIconHost;
        private MyApp.Controls.IconBadge auditSalesIcon;
        private MyApp.Controls.CardPanel auditMovementTile;
        private System.Windows.Forms.Label auditMovementDelta;
        private System.Windows.Forms.Label auditMovementValue;
        private System.Windows.Forms.Panel auditMovementTop;
        private System.Windows.Forms.Label auditMovementLabel;
        private System.Windows.Forms.Panel auditMovementIconHost;
        private MyApp.Controls.IconBadge auditMovementIcon;
        private MyApp.Controls.CardPanel auditDiscrepanciesTile;
        private System.Windows.Forms.Label auditDiscrepanciesDelta;
        private System.Windows.Forms.Label auditDiscrepanciesValue;
        private System.Windows.Forms.Panel auditDiscrepanciesTop;
        private System.Windows.Forms.Label auditDiscrepanciesLabel;
        private System.Windows.Forms.Panel auditDiscrepanciesIconHost;
        private MyApp.Controls.IconBadge auditDiscrepanciesIcon;
        private MyApp.Controls.CardPanel auditPeriodTile;
        private System.Windows.Forms.Label auditPeriodValue;
        private System.Windows.Forms.Panel auditPeriodTop;
        private System.Windows.Forms.Label auditPeriodLabel;
        private System.Windows.Forms.Panel auditPeriodIconHost;
        private MyApp.Controls.IconBadge auditPeriodIcon;
        private System.Windows.Forms.Panel gapOne;
        private MyApp.Controls.CardPanel reportsHeaderCard;
        private System.Windows.Forms.Panel reportsHeaderText;
        private System.Windows.Forms.Label reportsSubtitle;
        private System.Windows.Forms.Label reportsTitle;
        private System.Windows.Forms.Panel reportsIconHost;
        private MyApp.Controls.IconBadge reportsIcon;
        private System.Windows.Forms.Panel downloadReportHost;
        private MyApp.Controls.PrimaryButton downloadReportButton;
        private System.Windows.Forms.TableLayoutPanel rightColumn;
        private MyApp.Controls.CardPanel quickReportsCard;
        private System.Windows.Forms.Panel printableReportRow;
        private System.Windows.Forms.Panel printableReportTextHost;
        private System.Windows.Forms.Label printableReportDesc;
        private System.Windows.Forms.Label printableReportTitle;
        private System.Windows.Forms.Label printableReportChevron;
        private System.Windows.Forms.Panel printableReportIconHost;
        private MyApp.Controls.IconBadge printableReportIcon;
        private System.Windows.Forms.Panel financialReportRow;
        private System.Windows.Forms.Panel financialReportTextHost;
        private System.Windows.Forms.Label financialReportDesc;
        private System.Windows.Forms.Label financialReportTitle;
        private System.Windows.Forms.Label financialReportChevron;
        private System.Windows.Forms.Panel financialReportIconHost;
        private MyApp.Controls.IconBadge financialReportIcon;
        private System.Windows.Forms.Panel movementReportRow;
        private System.Windows.Forms.Panel movementReportTextHost;
        private System.Windows.Forms.Label movementReportDesc;
        private System.Windows.Forms.Label movementReportTitle;
        private System.Windows.Forms.Label movementReportChevron;
        private System.Windows.Forms.Panel movementReportIconHost;
        private MyApp.Controls.IconBadge movementReportIcon;
        private System.Windows.Forms.Panel salesReportRow;
        private System.Windows.Forms.Panel salesReportTextHost;
        private System.Windows.Forms.Label salesReportDesc;
        private System.Windows.Forms.Label salesReportTitle;
        private System.Windows.Forms.Label salesReportChevron;
        private System.Windows.Forms.Panel salesReportIconHost;
        private MyApp.Controls.IconBadge salesReportIcon;
        private System.Windows.Forms.Panel quickReportsHeader;
        private System.Windows.Forms.Label quickReportsTitle;
        private System.Windows.Forms.Label quickReportsIcon;
        private System.Windows.Forms.Label quickReportsLink;
        private MyApp.Controls.CardPanel recentActivityCard;
        private MyApp.Controls.EmptyState recentActivityEmpty;
        private System.Windows.Forms.Panel recentActivityHeader;
        private System.Windows.Forms.Label recentActivityTitle;
        private System.Windows.Forms.Label recentActivityIcon;
        private System.Windows.Forms.Label recentActivityLink;
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
