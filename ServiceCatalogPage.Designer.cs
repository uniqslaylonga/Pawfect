#nullable disable
namespace MyApp
{
    partial class ServiceCatalogPage
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
            serviceTableCard = new MyApp.Controls.CardPanel();
            serviceTableArea = new System.Windows.Forms.Panel();
            serviceRows = new System.Windows.Forms.Panel();
            serviceEmpty = new MyApp.Controls.EmptyState();
            serviceColumnsHead = new System.Windows.Forms.Panel();
            serviceColumnsGrid = new System.Windows.Forms.TableLayoutPanel();
            serviceHeaderCheck = new MyApp.Controls.CheckMark();
            serviceColService = new System.Windows.Forms.Label();
            serviceColCategory = new System.Windows.Forms.Label();
            serviceColSize = new System.Windows.Forms.Label();
            serviceColDuration = new System.Windows.Forms.Label();
            serviceColPrice = new System.Windows.Forms.Label();
            serviceColStatus = new System.Windows.Forms.Label();
            serviceColActions = new System.Windows.Forms.Label();
            serviceFooter = new System.Windows.Forms.Panel();
            serviceShowing = new System.Windows.Forms.Label();
            servicePager = new System.Windows.Forms.Panel();
            servicePrevPage = new MyApp.Controls.IconButton();
            servicePageChip = new MyApp.Controls.PrimaryButton();
            serviceNextPage = new MyApp.Controls.IconButton();
            serviceToolbar = new System.Windows.Forms.Panel();
            serviceSearch = new MyApp.Controls.InputField();
            categoryFilter = new MyApp.Controls.SelectField();
            toolbarGapC = new System.Windows.Forms.Panel();
            sizeFilter = new MyApp.Controls.SelectField();
            toolbarGapS = new System.Windows.Forms.Panel();
            statusFilter = new MyApp.Controls.SelectField();
            toolbarGapT = new System.Windows.Forms.Panel();
            viewToggleHost = new System.Windows.Forms.Panel();
            listViewToggle = new MyApp.Controls.IconBadge();
            gridViewToggle = new MyApp.Controls.IconBadge();
            gapTwo = new System.Windows.Forms.Panel();
            serviceStats = new System.Windows.Forms.TableLayoutPanel();
            svcTotalTile = new MyApp.Controls.CardPanel();
            svcTotalDelta = new System.Windows.Forms.Label();
            svcTotalValue = new System.Windows.Forms.Label();
            svcTotalLabel = new System.Windows.Forms.Label();
            svcTotalIcon = new MyApp.Controls.IconBadge();
            svcPriceTile = new MyApp.Controls.CardPanel();
            svcPriceDelta = new System.Windows.Forms.Label();
            svcPriceValue = new System.Windows.Forms.Label();
            svcPriceLabel = new System.Windows.Forms.Label();
            svcPriceIcon = new MyApp.Controls.IconBadge();
            svcDurationTile = new MyApp.Controls.CardPanel();
            svcDurationDelta = new System.Windows.Forms.Label();
            svcDurationValue = new System.Windows.Forms.Label();
            svcDurationLabel = new System.Windows.Forms.Label();
            svcDurationIcon = new MyApp.Controls.IconBadge();
            svcBookingsTile = new MyApp.Controls.CardPanel();
            svcBookingsDelta = new System.Windows.Forms.Label();
            svcBookingsValue = new System.Windows.Forms.Label();
            svcBookingsLabel = new System.Windows.Forms.Label();
            svcBookingsIcon = new MyApp.Controls.IconBadge();
            gapOne = new System.Windows.Forms.Panel();
            catalogHeaderCard = new MyApp.Controls.CardPanel();
            catalogHeaderText = new System.Windows.Forms.Panel();
            catalogSubtitle = new System.Windows.Forms.Label();
            catalogTitle = new System.Windows.Forms.Label();
            catalogIconHost = new System.Windows.Forms.Panel();
            catalogIcon = new MyApp.Controls.IconBadge();
            addServiceHost = new System.Windows.Forms.Panel();
            addServiceButton = new MyApp.Controls.PrimaryButton();
            rightColumn = new System.Windows.Forms.TableLayoutPanel();
            categoriesCard = new MyApp.Controls.CardPanel();
            catOthersRow = new System.Windows.Forms.Panel();
            catOthersName = new System.Windows.Forms.Label();
            catOthersDot = new MyApp.Controls.Dot();
            catOthersCount = new System.Windows.Forms.Label();
            catAddOnsRow = new System.Windows.Forms.Panel();
            catAddOnsName = new System.Windows.Forms.Label();
            catAddOnsDot = new MyApp.Controls.Dot();
            catAddOnsCount = new System.Windows.Forms.Label();
            catHealthRow = new System.Windows.Forms.Panel();
            catHealthName = new System.Windows.Forms.Label();
            catHealthDot = new MyApp.Controls.Dot();
            catHealthCount = new System.Windows.Forms.Label();
            catGroomingRow = new System.Windows.Forms.Panel();
            catGroomingName = new System.Windows.Forms.Label();
            catGroomingDot = new MyApp.Controls.Dot();
            catGroomingCount = new System.Windows.Forms.Label();
            categoriesHeader = new System.Windows.Forms.Panel();
            categoriesTitle = new System.Windows.Forms.Label();
            categoriesIcon = new System.Windows.Forms.Label();
            popularCard = new MyApp.Controls.CardPanel();
            popularEmpty = new MyApp.Controls.EmptyState();
            popularHeader = new System.Windows.Forms.Panel();
            popularTitle = new System.Windows.Forms.Label();
            popularIcon = new System.Windows.Forms.Label();
            quickCard = new MyApp.Controls.CardPanel();
            timeSlotsAction = new System.Windows.Forms.Panel();
            timeSlotsTextHost = new System.Windows.Forms.Panel();
            timeSlotsDesc = new System.Windows.Forms.Label();
            timeSlotsTitle = new System.Windows.Forms.Label();
            timeSlotsIconHost = new System.Windows.Forms.Panel();
            timeSlotsIcon = new MyApp.Controls.IconBadge();
            pricingAction = new System.Windows.Forms.Panel();
            pricingTextHost = new System.Windows.Forms.Panel();
            pricingDesc = new System.Windows.Forms.Label();
            pricingTitle = new System.Windows.Forms.Label();
            pricingIconHost = new System.Windows.Forms.Panel();
            pricingIcon = new MyApp.Controls.IconBadge();
            addServiceAction = new System.Windows.Forms.Panel();
            addServiceTextHost = new System.Windows.Forms.Panel();
            addServiceDesc = new System.Windows.Forms.Label();
            addServiceTitle = new System.Windows.Forms.Label();
            addServiceIconHost = new System.Windows.Forms.Panel();
            addServiceIcon = new MyApp.Controls.IconBadge();
            quickHeader = new System.Windows.Forms.Panel();
            quickTitle = new System.Windows.Forms.Label();
            quickIcon = new System.Windows.Forms.Label();
            pageHost.SuspendLayout();
            pageGrid.SuspendLayout();
            leftColumn.SuspendLayout();
            serviceTableCard.SuspendLayout();
            serviceTableArea.SuspendLayout();
            serviceRows.SuspendLayout();
            serviceColumnsHead.SuspendLayout();
            serviceColumnsGrid.SuspendLayout();
            serviceFooter.SuspendLayout();
            servicePager.SuspendLayout();
            serviceToolbar.SuspendLayout();
            viewToggleHost.SuspendLayout();
            serviceStats.SuspendLayout();
            svcTotalTile.SuspendLayout();
            svcPriceTile.SuspendLayout();
            svcDurationTile.SuspendLayout();
            svcBookingsTile.SuspendLayout();
            catalogHeaderCard.SuspendLayout();
            catalogHeaderText.SuspendLayout();
            catalogIconHost.SuspendLayout();
            addServiceHost.SuspendLayout();
            rightColumn.SuspendLayout();
            categoriesCard.SuspendLayout();
            catOthersRow.SuspendLayout();
            catAddOnsRow.SuspendLayout();
            catHealthRow.SuspendLayout();
            catGroomingRow.SuspendLayout();
            categoriesHeader.SuspendLayout();
            popularCard.SuspendLayout();
            popularHeader.SuspendLayout();
            quickCard.SuspendLayout();
            timeSlotsAction.SuspendLayout();
            timeSlotsTextHost.SuspendLayout();
            timeSlotsIconHost.SuspendLayout();
            pricingAction.SuspendLayout();
            pricingTextHost.SuspendLayout();
            pricingIconHost.SuspendLayout();
            addServiceAction.SuspendLayout();
            addServiceTextHost.SuspendLayout();
            addServiceIconHost.SuspendLayout();
            quickHeader.SuspendLayout();
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
            leftColumn.Controls.Add(serviceTableCard);
            leftColumn.Controls.Add(gapTwo);
            leftColumn.Controls.Add(serviceStats);
            leftColumn.Controls.Add(gapOne);
            leftColumn.Controls.Add(catalogHeaderCard);
            leftColumn.Dock = DockStyle.Fill;
            leftColumn.Location = new Point(0, 0);
            leftColumn.Margin = new Padding(0, 0, 0, 0);
            leftColumn.Name = "leftColumn";
            leftColumn.Size = new Size(700, 692);
            leftColumn.TabIndex = 0;
            // 
            // serviceTableCard
            // 
            serviceTableCard.BorderColor = Color.FromArgb(227, 233, 242);
            serviceTableCard.Controls.Add(serviceTableArea);
            serviceTableCard.Controls.Add(serviceFooter);
            serviceTableCard.Controls.Add(serviceToolbar);
            serviceTableCard.CornerColor = Color.FromArgb(243, 247, 252);
            serviceTableCard.Dock = DockStyle.Fill;
            serviceTableCard.FillColor = Color.White;
            serviceTableCard.Location = new Point(0, 0);
            serviceTableCard.Name = "serviceTableCard";
            serviceTableCard.Padding = new Padding(16, 14, 16, 10);
            serviceTableCard.Radius = 10;
            serviceTableCard.Size = new Size(700, 420);
            serviceTableCard.TabIndex = 0;
            // 
            // serviceTableArea
            // 
            serviceTableArea.BackColor = Color.White;
            serviceTableArea.Controls.Add(serviceRows);
            serviceTableArea.Controls.Add(serviceColumnsHead);
            serviceTableArea.Dock = DockStyle.Fill;
            serviceTableArea.Location = new Point(0, 0);
            serviceTableArea.Name = "serviceTableArea";
            serviceTableArea.Size = new Size(636, 286);
            serviceTableArea.TabIndex = 0;
            // 
            // serviceRows
            // 
            serviceRows.AutoScroll = true;
            serviceRows.BackColor = Color.White;
            serviceRows.Controls.Add(serviceEmpty);
            serviceRows.Dock = DockStyle.Fill;
            serviceRows.Location = new Point(0, 36);
            serviceRows.Name = "serviceRows";
            serviceRows.Size = new Size(636, 250);
            serviceRows.TabIndex = 0;
            // 
            // serviceEmpty
            // 
            serviceEmpty.BackColor = Color.White;
            serviceEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            serviceEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            serviceEmpty.Dock = DockStyle.Fill;
            serviceEmpty.Glyph = "\uE734";
            serviceEmpty.Hint = "Services will appear here once they are added.";
            serviceEmpty.Location = new Point(0, 0);
            serviceEmpty.Name = "serviceEmpty";
            serviceEmpty.Size = new Size(636, 250);
            serviceEmpty.TabIndex = 0;
            serviceEmpty.TabStop = false;
            serviceEmpty.Title = "No services found";
            // 
            // serviceColumnsHead
            // 
            serviceColumnsHead.BackColor = Color.FromArgb(246, 248, 251);
            serviceColumnsHead.Controls.Add(serviceColumnsGrid);
            serviceColumnsHead.Dock = DockStyle.Top;
            serviceColumnsHead.Location = new Point(0, 0);
            serviceColumnsHead.Name = "serviceColumnsHead";
            serviceColumnsHead.Size = new Size(636, 36);
            serviceColumnsHead.TabIndex = 1;
            // 
            // serviceColumnsGrid
            // 
            serviceColumnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            serviceColumnsGrid.ColumnCount = 8;
            serviceColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            serviceColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            serviceColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            serviceColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            serviceColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
            serviceColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            serviceColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            serviceColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            serviceColumnsGrid.Controls.Add(serviceHeaderCheck, 0, 0);
            serviceColumnsGrid.Controls.Add(serviceColService, 1, 0);
            serviceColumnsGrid.Controls.Add(serviceColCategory, 2, 0);
            serviceColumnsGrid.Controls.Add(serviceColSize, 3, 0);
            serviceColumnsGrid.Controls.Add(serviceColDuration, 4, 0);
            serviceColumnsGrid.Controls.Add(serviceColPrice, 5, 0);
            serviceColumnsGrid.Controls.Add(serviceColStatus, 6, 0);
            serviceColumnsGrid.Controls.Add(serviceColActions, 7, 0);
            serviceColumnsGrid.Dock = DockStyle.Fill;
            serviceColumnsGrid.Location = new Point(0, 0);
            serviceColumnsGrid.Margin = new Padding(0, 0, 0, 0);
            serviceColumnsGrid.Name = "serviceColumnsGrid";
            serviceColumnsGrid.RowCount = 1;
            serviceColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            serviceColumnsGrid.Size = new Size(636, 36);
            serviceColumnsGrid.TabIndex = 0;
            // 
            // serviceHeaderCheck
            // 
            serviceHeaderCheck.Anchor = AnchorStyles.None;
            serviceHeaderCheck.BackColor = Color.FromArgb(246, 248, 251);
            serviceHeaderCheck.Location = new Point(13, 9);
            serviceHeaderCheck.Name = "serviceHeaderCheck";
            serviceHeaderCheck.Size = new Size(18, 18);
            serviceHeaderCheck.TabIndex = 0;
            serviceHeaderCheck.TabStop = false;
            // 
            // serviceColService
            // 
            serviceColService.BackColor = Color.FromArgb(246, 248, 251);
            serviceColService.Dock = DockStyle.Fill;
            serviceColService.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            serviceColService.ForeColor = Color.FromArgb(122, 136, 156);
            serviceColService.Location = new Point(0, 0);
            serviceColService.Margin = new Padding(0, 0, 0, 0);
            serviceColService.Name = "serviceColService";
            serviceColService.Padding = new Padding(8, 0, 0, 0);
            serviceColService.Size = new Size(80, 36);
            serviceColService.TabIndex = 1;
            serviceColService.Text = "SERVICE";
            serviceColService.TextAlign = ContentAlignment.MiddleLeft;
            serviceColService.UseMnemonic = false;
            // 
            // serviceColCategory
            // 
            serviceColCategory.BackColor = Color.FromArgb(246, 248, 251);
            serviceColCategory.Dock = DockStyle.Fill;
            serviceColCategory.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            serviceColCategory.ForeColor = Color.FromArgb(122, 136, 156);
            serviceColCategory.Location = new Point(0, 0);
            serviceColCategory.Margin = new Padding(0, 0, 0, 0);
            serviceColCategory.Name = "serviceColCategory";
            serviceColCategory.Padding = new Padding(8, 0, 0, 0);
            serviceColCategory.Size = new Size(80, 36);
            serviceColCategory.TabIndex = 2;
            serviceColCategory.Text = "CATEGORY";
            serviceColCategory.TextAlign = ContentAlignment.MiddleLeft;
            serviceColCategory.UseMnemonic = false;
            // 
            // serviceColSize
            // 
            serviceColSize.BackColor = Color.FromArgb(246, 248, 251);
            serviceColSize.Dock = DockStyle.Fill;
            serviceColSize.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            serviceColSize.ForeColor = Color.FromArgb(122, 136, 156);
            serviceColSize.Location = new Point(0, 0);
            serviceColSize.Margin = new Padding(0, 0, 0, 0);
            serviceColSize.Name = "serviceColSize";
            serviceColSize.Padding = new Padding(8, 0, 0, 0);
            serviceColSize.Size = new Size(80, 36);
            serviceColSize.TabIndex = 3;
            serviceColSize.Text = "SIZE / TYPE";
            serviceColSize.TextAlign = ContentAlignment.MiddleLeft;
            serviceColSize.UseMnemonic = false;
            // 
            // serviceColDuration
            // 
            serviceColDuration.BackColor = Color.FromArgb(246, 248, 251);
            serviceColDuration.Dock = DockStyle.Fill;
            serviceColDuration.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            serviceColDuration.ForeColor = Color.FromArgb(122, 136, 156);
            serviceColDuration.Location = new Point(0, 0);
            serviceColDuration.Margin = new Padding(0, 0, 0, 0);
            serviceColDuration.Name = "serviceColDuration";
            serviceColDuration.Padding = new Padding(8, 0, 0, 0);
            serviceColDuration.Size = new Size(80, 36);
            serviceColDuration.TabIndex = 4;
            serviceColDuration.Text = "DURATION";
            serviceColDuration.TextAlign = ContentAlignment.MiddleLeft;
            serviceColDuration.UseMnemonic = false;
            // 
            // serviceColPrice
            // 
            serviceColPrice.BackColor = Color.FromArgb(246, 248, 251);
            serviceColPrice.Dock = DockStyle.Fill;
            serviceColPrice.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            serviceColPrice.ForeColor = Color.FromArgb(122, 136, 156);
            serviceColPrice.Location = new Point(0, 0);
            serviceColPrice.Margin = new Padding(0, 0, 0, 0);
            serviceColPrice.Name = "serviceColPrice";
            serviceColPrice.Padding = new Padding(8, 0, 0, 0);
            serviceColPrice.Size = new Size(80, 36);
            serviceColPrice.TabIndex = 5;
            serviceColPrice.Text = "PRICE";
            serviceColPrice.TextAlign = ContentAlignment.MiddleLeft;
            serviceColPrice.UseMnemonic = false;
            // 
            // serviceColStatus
            // 
            serviceColStatus.BackColor = Color.FromArgb(246, 248, 251);
            serviceColStatus.Dock = DockStyle.Fill;
            serviceColStatus.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            serviceColStatus.ForeColor = Color.FromArgb(122, 136, 156);
            serviceColStatus.Location = new Point(0, 0);
            serviceColStatus.Margin = new Padding(0, 0, 0, 0);
            serviceColStatus.Name = "serviceColStatus";
            serviceColStatus.Padding = new Padding(8, 0, 0, 0);
            serviceColStatus.Size = new Size(80, 36);
            serviceColStatus.TabIndex = 6;
            serviceColStatus.Text = "STATUS";
            serviceColStatus.TextAlign = ContentAlignment.MiddleLeft;
            serviceColStatus.UseMnemonic = false;
            // 
            // serviceColActions
            // 
            serviceColActions.BackColor = Color.FromArgb(246, 248, 251);
            serviceColActions.Dock = DockStyle.Fill;
            serviceColActions.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            serviceColActions.ForeColor = Color.FromArgb(122, 136, 156);
            serviceColActions.Location = new Point(0, 0);
            serviceColActions.Margin = new Padding(0, 0, 0, 0);
            serviceColActions.Name = "serviceColActions";
            serviceColActions.Padding = new Padding(8, 0, 0, 0);
            serviceColActions.Size = new Size(80, 36);
            serviceColActions.TabIndex = 7;
            serviceColActions.Text = "ACTIONS";
            serviceColActions.TextAlign = ContentAlignment.MiddleLeft;
            serviceColActions.UseMnemonic = false;
            // 
            // serviceFooter
            // 
            serviceFooter.BackColor = Color.White;
            serviceFooter.Controls.Add(serviceShowing);
            serviceFooter.Controls.Add(servicePager);
            serviceFooter.Dock = DockStyle.Bottom;
            serviceFooter.Location = new Point(0, 0);
            serviceFooter.Name = "serviceFooter";
            serviceFooter.Size = new Size(668, 44);
            serviceFooter.TabIndex = 1;
            // 
            // serviceShowing
            // 
            serviceShowing.BackColor = Color.White;
            serviceShowing.Dock = DockStyle.Fill;
            serviceShowing.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            serviceShowing.ForeColor = Color.FromArgb(122, 136, 156);
            serviceShowing.Location = new Point(0, 0);
            serviceShowing.Name = "serviceShowing";
            serviceShowing.Size = new Size(500, 44);
            serviceShowing.TabIndex = 0;
            serviceShowing.Text = "Showing 0 services";
            serviceShowing.TextAlign = ContentAlignment.MiddleLeft;
            serviceShowing.UseMnemonic = false;
            // 
            // servicePager
            // 
            servicePager.BackColor = Color.White;
            servicePager.Controls.Add(servicePrevPage);
            servicePager.Controls.Add(servicePageChip);
            servicePager.Controls.Add(serviceNextPage);
            servicePager.Dock = DockStyle.Right;
            servicePager.Location = new Point(0, 0);
            servicePager.Name = "servicePager";
            servicePager.Size = new Size(116, 44);
            servicePager.TabIndex = 1;
            // 
            // servicePrevPage
            // 
            servicePrevPage.BackColor = Color.White;
            servicePrevPage.Enabled = false;
            servicePrevPage.Glyph = "\uE76B";
            servicePrevPage.Location = new Point(4, 7);
            servicePrevPage.Name = "servicePrevPage";
            servicePrevPage.Size = new Size(30, 30);
            servicePrevPage.TabIndex = 0;
            // 
            // servicePageChip
            // 
            servicePageChip.BackColor = Color.White;
            servicePageChip.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            servicePageChip.Location = new Point(42, 7);
            servicePageChip.Name = "servicePageChip";
            servicePageChip.Size = new Size(30, 30);
            servicePageChip.TabIndex = 1;
            servicePageChip.Text = "1";
            // 
            // serviceNextPage
            // 
            serviceNextPage.BackColor = Color.White;
            serviceNextPage.Enabled = false;
            serviceNextPage.Glyph = "\uE76C";
            serviceNextPage.Location = new Point(80, 7);
            serviceNextPage.Name = "serviceNextPage";
            serviceNextPage.Size = new Size(30, 30);
            serviceNextPage.TabIndex = 2;
            // 
            // serviceToolbar
            // 
            serviceToolbar.BackColor = Color.White;
            serviceToolbar.Controls.Add(serviceSearch);
            serviceToolbar.Controls.Add(categoryFilter);
            serviceToolbar.Controls.Add(toolbarGapC);
            serviceToolbar.Controls.Add(sizeFilter);
            serviceToolbar.Controls.Add(toolbarGapS);
            serviceToolbar.Controls.Add(statusFilter);
            serviceToolbar.Controls.Add(toolbarGapT);
            serviceToolbar.Controls.Add(viewToggleHost);
            serviceToolbar.Dock = DockStyle.Top;
            serviceToolbar.Location = new Point(0, 0);
            serviceToolbar.Name = "serviceToolbar";
            serviceToolbar.Padding = new Padding(0, 0, 0, 14);
            serviceToolbar.Size = new Size(668, 52);
            serviceToolbar.TabIndex = 2;
            // 
            // serviceSearch
            // 
            serviceSearch.BorderColor = Color.FromArgb(227, 233, 242);
            serviceSearch.CornerColor = Color.White;
            serviceSearch.Dock = DockStyle.Fill;
            serviceSearch.FillColor = Color.White;
            serviceSearch.Glyph = "\uE721";
            serviceSearch.Location = new Point(0, 0);
            serviceSearch.Name = "serviceSearch";
            serviceSearch.Padding = new Padding(10, 3, 10, 3);
            serviceSearch.Placeholder = "Search services...";
            serviceSearch.Radius = 8;
            serviceSearch.Size = new Size(200, 38);
            serviceSearch.TabIndex = 0;
            // 
            // categoryFilter
            // 
            categoryFilter.BackColor = Color.White;
            categoryFilter.Dock = DockStyle.Right;
            categoryFilter.Location = new Point(0, 0);
            categoryFilter.Name = "categoryFilter";
            categoryFilter.Size = new Size(148, 38);
            categoryFilter.TabIndex = 1;
            categoryFilter.Text = "All Categories";
            // 
            // toolbarGapC
            // 
            toolbarGapC.BackColor = Color.White;
            toolbarGapC.Dock = DockStyle.Right;
            toolbarGapC.Location = new Point(0, 0);
            toolbarGapC.Name = "toolbarGapC";
            toolbarGapC.Size = new Size(8, 38);
            toolbarGapC.TabIndex = 2;
            // 
            // sizeFilter
            // 
            sizeFilter.BackColor = Color.White;
            sizeFilter.Dock = DockStyle.Right;
            sizeFilter.Location = new Point(0, 0);
            sizeFilter.Name = "sizeFilter";
            sizeFilter.Size = new Size(118, 38);
            sizeFilter.TabIndex = 3;
            sizeFilter.Text = "All Sizes";
            // 
            // toolbarGapS
            // 
            toolbarGapS.BackColor = Color.White;
            toolbarGapS.Dock = DockStyle.Right;
            toolbarGapS.Location = new Point(0, 0);
            toolbarGapS.Name = "toolbarGapS";
            toolbarGapS.Size = new Size(8, 38);
            toolbarGapS.TabIndex = 4;
            // 
            // statusFilter
            // 
            statusFilter.BackColor = Color.White;
            statusFilter.Dock = DockStyle.Right;
            statusFilter.Location = new Point(0, 0);
            statusFilter.Name = "statusFilter";
            statusFilter.Size = new Size(118, 38);
            statusFilter.TabIndex = 5;
            statusFilter.Text = "All Status";
            // 
            // toolbarGapT
            // 
            toolbarGapT.BackColor = Color.White;
            toolbarGapT.Dock = DockStyle.Right;
            toolbarGapT.Location = new Point(0, 0);
            toolbarGapT.Name = "toolbarGapT";
            toolbarGapT.Size = new Size(8, 38);
            toolbarGapT.TabIndex = 6;
            // 
            // viewToggleHost
            // 
            viewToggleHost.BackColor = Color.White;
            viewToggleHost.Controls.Add(listViewToggle);
            viewToggleHost.Controls.Add(gridViewToggle);
            viewToggleHost.Dock = DockStyle.Right;
            viewToggleHost.Location = new Point(0, 0);
            viewToggleHost.Name = "viewToggleHost";
            viewToggleHost.Size = new Size(72, 38);
            viewToggleHost.TabIndex = 7;
            // 
            // listViewToggle
            // 
            listViewToggle.BackColor = Color.White;
            listViewToggle.Cursor = Cursors.Hand;
            listViewToggle.FillColor = Color.FromArgb(58, 123, 213);
            listViewToggle.ForeColor = Color.White;
            listViewToggle.Glyph = "\uE8FD";
            listViewToggle.GlyphSize = 10F;
            listViewToggle.Location = new Point(38, 4);
            listViewToggle.Name = "listViewToggle";
            listViewToggle.Radius = 8;
            listViewToggle.Size = new Size(30, 30);
            listViewToggle.TabIndex = 0;
            listViewToggle.TabStop = false;
            // 
            // gridViewToggle
            // 
            gridViewToggle.BackColor = Color.White;
            gridViewToggle.Cursor = Cursors.Hand;
            gridViewToggle.FillColor = Color.FromArgb(246, 248, 251);
            gridViewToggle.ForeColor = Color.FromArgb(122, 136, 156);
            gridViewToggle.Glyph = "\uE80A";
            gridViewToggle.GlyphSize = 10F;
            gridViewToggle.Location = new Point(4, 4);
            gridViewToggle.Name = "gridViewToggle";
            gridViewToggle.Radius = 8;
            gridViewToggle.Size = new Size(30, 30);
            gridViewToggle.TabIndex = 1;
            gridViewToggle.TabStop = false;
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
            // serviceStats
            // 
            serviceStats.BackColor = Color.FromArgb(243, 247, 252);
            serviceStats.ColumnCount = 7;
            serviceStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            serviceStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            serviceStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            serviceStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            serviceStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            serviceStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            serviceStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            serviceStats.Controls.Add(svcTotalTile, 0, 0);
            serviceStats.Controls.Add(svcPriceTile, 2, 0);
            serviceStats.Controls.Add(svcDurationTile, 4, 0);
            serviceStats.Controls.Add(svcBookingsTile, 6, 0);
            serviceStats.Dock = DockStyle.Top;
            serviceStats.Location = new Point(0, 0);
            serviceStats.Margin = new Padding(0, 0, 0, 0);
            serviceStats.Name = "serviceStats";
            serviceStats.RowCount = 1;
            serviceStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            serviceStats.Size = new Size(700, 96);
            serviceStats.TabIndex = 2;
            // 
            // svcTotalTile
            // 
            svcTotalTile.BorderColor = Color.FromArgb(227, 233, 242);
            svcTotalTile.Controls.Add(svcTotalDelta);
            svcTotalTile.Controls.Add(svcTotalValue);
            svcTotalTile.Controls.Add(svcTotalLabel);
            svcTotalTile.Controls.Add(svcTotalIcon);
            svcTotalTile.CornerColor = Color.FromArgb(243, 247, 252);
            svcTotalTile.Dock = DockStyle.Fill;
            svcTotalTile.FillColor = Color.White;
            svcTotalTile.Location = new Point(0, 0);
            svcTotalTile.Margin = new Padding(0, 0, 0, 0);
            svcTotalTile.Name = "svcTotalTile";
            svcTotalTile.Padding = new Padding(14, 10, 14, 8);
            svcTotalTile.Radius = 10;
            svcTotalTile.Size = new Size(160, 96);
            svcTotalTile.TabIndex = 0;
            // 
            // svcTotalDelta
            // 
            svcTotalDelta.BackColor = Color.White;
            svcTotalDelta.Dock = DockStyle.Top;
            svcTotalDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            svcTotalDelta.ForeColor = Color.FromArgb(122, 136, 156);
            svcTotalDelta.Location = new Point(0, 0);
            svcTotalDelta.Name = "svcTotalDelta";
            svcTotalDelta.Size = new Size(130, 16);
            svcTotalDelta.TabIndex = 0;
            svcTotalDelta.Text = "\u2014 vs. last month";
            svcTotalDelta.TextAlign = ContentAlignment.MiddleLeft;
            svcTotalDelta.UseMnemonic = false;
            // 
            // svcTotalValue
            // 
            svcTotalValue.BackColor = Color.White;
            svcTotalValue.Dock = DockStyle.Top;
            svcTotalValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            svcTotalValue.ForeColor = Color.FromArgb(31, 42, 60);
            svcTotalValue.Location = new Point(0, 0);
            svcTotalValue.Name = "svcTotalValue";
            svcTotalValue.Size = new Size(130, 30);
            svcTotalValue.TabIndex = 1;
            svcTotalValue.Text = "0";
            svcTotalValue.TextAlign = ContentAlignment.MiddleLeft;
            svcTotalValue.UseMnemonic = false;
            // 
            // svcTotalLabel
            // 
            svcTotalLabel.BackColor = Color.White;
            svcTotalLabel.Dock = DockStyle.Top;
            svcTotalLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            svcTotalLabel.ForeColor = Color.FromArgb(68, 83, 106);
            svcTotalLabel.Location = new Point(0, 0);
            svcTotalLabel.Name = "svcTotalLabel";
            svcTotalLabel.Padding = new Padding(0, 0, 36, 0);
            svcTotalLabel.Size = new Size(130, 30);
            svcTotalLabel.TabIndex = 2;
            svcTotalLabel.Text = "Total Services";
            svcTotalLabel.TextAlign = ContentAlignment.TopLeft;
            svcTotalLabel.UseMnemonic = false;
            // 
            // svcTotalIcon
            // 
            svcTotalIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            svcTotalIcon.BackColor = Color.White;
            svcTotalIcon.FillColor = Color.FromArgb(234, 242, 253);
            svcTotalIcon.ForeColor = Color.FromArgb(58, 123, 213);
            svcTotalIcon.Glyph = "\uE8C6";
            svcTotalIcon.GlyphSize = 10F;
            svcTotalIcon.Location = new Point(116, 10);
            svcTotalIcon.Name = "svcTotalIcon";
            svcTotalIcon.Radius = 8;
            svcTotalIcon.Size = new Size(30, 30);
            svcTotalIcon.TabIndex = 3;
            svcTotalIcon.TabStop = false;
            // 
            // svcPriceTile
            // 
            svcPriceTile.BorderColor = Color.FromArgb(227, 233, 242);
            svcPriceTile.Controls.Add(svcPriceDelta);
            svcPriceTile.Controls.Add(svcPriceValue);
            svcPriceTile.Controls.Add(svcPriceLabel);
            svcPriceTile.Controls.Add(svcPriceIcon);
            svcPriceTile.CornerColor = Color.FromArgb(243, 247, 252);
            svcPriceTile.Dock = DockStyle.Fill;
            svcPriceTile.FillColor = Color.White;
            svcPriceTile.Location = new Point(0, 0);
            svcPriceTile.Margin = new Padding(0, 0, 0, 0);
            svcPriceTile.Name = "svcPriceTile";
            svcPriceTile.Padding = new Padding(14, 10, 14, 8);
            svcPriceTile.Radius = 10;
            svcPriceTile.Size = new Size(160, 96);
            svcPriceTile.TabIndex = 1;
            // 
            // svcPriceDelta
            // 
            svcPriceDelta.BackColor = Color.White;
            svcPriceDelta.Dock = DockStyle.Top;
            svcPriceDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            svcPriceDelta.ForeColor = Color.FromArgb(122, 136, 156);
            svcPriceDelta.Location = new Point(0, 0);
            svcPriceDelta.Name = "svcPriceDelta";
            svcPriceDelta.Size = new Size(130, 16);
            svcPriceDelta.TabIndex = 0;
            svcPriceDelta.Text = "\u2014 vs. last month";
            svcPriceDelta.TextAlign = ContentAlignment.MiddleLeft;
            svcPriceDelta.UseMnemonic = false;
            // 
            // svcPriceValue
            // 
            svcPriceValue.BackColor = Color.White;
            svcPriceValue.Dock = DockStyle.Top;
            svcPriceValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            svcPriceValue.ForeColor = Color.FromArgb(31, 42, 60);
            svcPriceValue.Location = new Point(0, 0);
            svcPriceValue.Name = "svcPriceValue";
            svcPriceValue.Size = new Size(130, 30);
            svcPriceValue.TabIndex = 1;
            svcPriceValue.Text = "\u20B10";
            svcPriceValue.TextAlign = ContentAlignment.MiddleLeft;
            svcPriceValue.UseMnemonic = false;
            // 
            // svcPriceLabel
            // 
            svcPriceLabel.BackColor = Color.White;
            svcPriceLabel.Dock = DockStyle.Top;
            svcPriceLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            svcPriceLabel.ForeColor = Color.FromArgb(68, 83, 106);
            svcPriceLabel.Location = new Point(0, 0);
            svcPriceLabel.Name = "svcPriceLabel";
            svcPriceLabel.Padding = new Padding(0, 0, 36, 0);
            svcPriceLabel.Size = new Size(130, 30);
            svcPriceLabel.TabIndex = 2;
            svcPriceLabel.Text = "Avg. Price";
            svcPriceLabel.TextAlign = ContentAlignment.TopLeft;
            svcPriceLabel.UseMnemonic = false;
            // 
            // svcPriceIcon
            // 
            svcPriceIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            svcPriceIcon.BackColor = Color.White;
            svcPriceIcon.FillColor = Color.FromArgb(234, 242, 253);
            svcPriceIcon.ForeColor = Color.FromArgb(58, 123, 213);
            svcPriceIcon.Glyph = "\uE8C7";
            svcPriceIcon.GlyphSize = 10F;
            svcPriceIcon.Location = new Point(116, 10);
            svcPriceIcon.Name = "svcPriceIcon";
            svcPriceIcon.Radius = 8;
            svcPriceIcon.Size = new Size(30, 30);
            svcPriceIcon.TabIndex = 3;
            svcPriceIcon.TabStop = false;
            // 
            // svcDurationTile
            // 
            svcDurationTile.BorderColor = Color.FromArgb(227, 233, 242);
            svcDurationTile.Controls.Add(svcDurationDelta);
            svcDurationTile.Controls.Add(svcDurationValue);
            svcDurationTile.Controls.Add(svcDurationLabel);
            svcDurationTile.Controls.Add(svcDurationIcon);
            svcDurationTile.CornerColor = Color.FromArgb(243, 247, 252);
            svcDurationTile.Dock = DockStyle.Fill;
            svcDurationTile.FillColor = Color.White;
            svcDurationTile.Location = new Point(0, 0);
            svcDurationTile.Margin = new Padding(0, 0, 0, 0);
            svcDurationTile.Name = "svcDurationTile";
            svcDurationTile.Padding = new Padding(14, 10, 14, 8);
            svcDurationTile.Radius = 10;
            svcDurationTile.Size = new Size(160, 96);
            svcDurationTile.TabIndex = 2;
            // 
            // svcDurationDelta
            // 
            svcDurationDelta.BackColor = Color.White;
            svcDurationDelta.Dock = DockStyle.Top;
            svcDurationDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            svcDurationDelta.ForeColor = Color.FromArgb(122, 136, 156);
            svcDurationDelta.Location = new Point(0, 0);
            svcDurationDelta.Name = "svcDurationDelta";
            svcDurationDelta.Size = new Size(130, 16);
            svcDurationDelta.TabIndex = 0;
            svcDurationDelta.Text = "\u2014 vs. last month";
            svcDurationDelta.TextAlign = ContentAlignment.MiddleLeft;
            svcDurationDelta.UseMnemonic = false;
            // 
            // svcDurationValue
            // 
            svcDurationValue.BackColor = Color.White;
            svcDurationValue.Dock = DockStyle.Top;
            svcDurationValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            svcDurationValue.ForeColor = Color.FromArgb(31, 42, 60);
            svcDurationValue.Location = new Point(0, 0);
            svcDurationValue.Name = "svcDurationValue";
            svcDurationValue.Size = new Size(130, 30);
            svcDurationValue.TabIndex = 1;
            svcDurationValue.Text = "0 mins";
            svcDurationValue.TextAlign = ContentAlignment.MiddleLeft;
            svcDurationValue.UseMnemonic = false;
            // 
            // svcDurationLabel
            // 
            svcDurationLabel.BackColor = Color.White;
            svcDurationLabel.Dock = DockStyle.Top;
            svcDurationLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            svcDurationLabel.ForeColor = Color.FromArgb(68, 83, 106);
            svcDurationLabel.Location = new Point(0, 0);
            svcDurationLabel.Name = "svcDurationLabel";
            svcDurationLabel.Padding = new Padding(0, 0, 36, 0);
            svcDurationLabel.Size = new Size(130, 30);
            svcDurationLabel.TabIndex = 2;
            svcDurationLabel.Text = "Avg. Duration";
            svcDurationLabel.TextAlign = ContentAlignment.TopLeft;
            svcDurationLabel.UseMnemonic = false;
            // 
            // svcDurationIcon
            // 
            svcDurationIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            svcDurationIcon.BackColor = Color.White;
            svcDurationIcon.FillColor = Color.FromArgb(234, 242, 253);
            svcDurationIcon.ForeColor = Color.FromArgb(58, 123, 213);
            svcDurationIcon.Glyph = "\uE823";
            svcDurationIcon.GlyphSize = 10F;
            svcDurationIcon.Location = new Point(116, 10);
            svcDurationIcon.Name = "svcDurationIcon";
            svcDurationIcon.Radius = 8;
            svcDurationIcon.Size = new Size(30, 30);
            svcDurationIcon.TabIndex = 3;
            svcDurationIcon.TabStop = false;
            // 
            // svcBookingsTile
            // 
            svcBookingsTile.BorderColor = Color.FromArgb(227, 233, 242);
            svcBookingsTile.Controls.Add(svcBookingsDelta);
            svcBookingsTile.Controls.Add(svcBookingsValue);
            svcBookingsTile.Controls.Add(svcBookingsLabel);
            svcBookingsTile.Controls.Add(svcBookingsIcon);
            svcBookingsTile.CornerColor = Color.FromArgb(243, 247, 252);
            svcBookingsTile.Dock = DockStyle.Fill;
            svcBookingsTile.FillColor = Color.White;
            svcBookingsTile.Location = new Point(0, 0);
            svcBookingsTile.Margin = new Padding(0, 0, 0, 0);
            svcBookingsTile.Name = "svcBookingsTile";
            svcBookingsTile.Padding = new Padding(14, 10, 14, 8);
            svcBookingsTile.Radius = 10;
            svcBookingsTile.Size = new Size(160, 96);
            svcBookingsTile.TabIndex = 3;
            // 
            // svcBookingsDelta
            // 
            svcBookingsDelta.BackColor = Color.White;
            svcBookingsDelta.Dock = DockStyle.Top;
            svcBookingsDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            svcBookingsDelta.ForeColor = Color.FromArgb(122, 136, 156);
            svcBookingsDelta.Location = new Point(0, 0);
            svcBookingsDelta.Name = "svcBookingsDelta";
            svcBookingsDelta.Size = new Size(130, 16);
            svcBookingsDelta.TabIndex = 0;
            svcBookingsDelta.Text = "\u2014 vs. last month";
            svcBookingsDelta.TextAlign = ContentAlignment.MiddleLeft;
            svcBookingsDelta.UseMnemonic = false;
            // 
            // svcBookingsValue
            // 
            svcBookingsValue.BackColor = Color.White;
            svcBookingsValue.Dock = DockStyle.Top;
            svcBookingsValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            svcBookingsValue.ForeColor = Color.FromArgb(31, 42, 60);
            svcBookingsValue.Location = new Point(0, 0);
            svcBookingsValue.Name = "svcBookingsValue";
            svcBookingsValue.Size = new Size(130, 30);
            svcBookingsValue.TabIndex = 1;
            svcBookingsValue.Text = "0";
            svcBookingsValue.TextAlign = ContentAlignment.MiddleLeft;
            svcBookingsValue.UseMnemonic = false;
            // 
            // svcBookingsLabel
            // 
            svcBookingsLabel.BackColor = Color.White;
            svcBookingsLabel.Dock = DockStyle.Top;
            svcBookingsLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            svcBookingsLabel.ForeColor = Color.FromArgb(68, 83, 106);
            svcBookingsLabel.Location = new Point(0, 0);
            svcBookingsLabel.Name = "svcBookingsLabel";
            svcBookingsLabel.Padding = new Padding(0, 0, 36, 0);
            svcBookingsLabel.Size = new Size(130, 30);
            svcBookingsLabel.TabIndex = 2;
            svcBookingsLabel.Text = "Total Bookings (Service)";
            svcBookingsLabel.TextAlign = ContentAlignment.TopLeft;
            svcBookingsLabel.UseMnemonic = false;
            // 
            // svcBookingsIcon
            // 
            svcBookingsIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            svcBookingsIcon.BackColor = Color.White;
            svcBookingsIcon.FillColor = Color.FromArgb(234, 242, 253);
            svcBookingsIcon.ForeColor = Color.FromArgb(58, 123, 213);
            svcBookingsIcon.Glyph = "\uE787";
            svcBookingsIcon.GlyphSize = 10F;
            svcBookingsIcon.Location = new Point(116, 10);
            svcBookingsIcon.Name = "svcBookingsIcon";
            svcBookingsIcon.Radius = 8;
            svcBookingsIcon.Size = new Size(30, 30);
            svcBookingsIcon.TabIndex = 3;
            svcBookingsIcon.TabStop = false;
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
            // catalogHeaderCard
            // 
            catalogHeaderCard.BorderColor = Color.FromArgb(227, 233, 242);
            catalogHeaderCard.Controls.Add(catalogHeaderText);
            catalogHeaderCard.Controls.Add(catalogIconHost);
            catalogHeaderCard.Controls.Add(addServiceHost);
            catalogHeaderCard.CornerColor = Color.FromArgb(243, 247, 252);
            catalogHeaderCard.Dock = DockStyle.Top;
            catalogHeaderCard.FillColor = Color.White;
            catalogHeaderCard.Location = new Point(0, 0);
            catalogHeaderCard.Name = "catalogHeaderCard";
            catalogHeaderCard.Padding = new Padding(16, 12, 16, 12);
            catalogHeaderCard.Radius = 10;
            catalogHeaderCard.Size = new Size(700, 84);
            catalogHeaderCard.TabIndex = 4;
            // 
            // catalogHeaderText
            // 
            catalogHeaderText.BackColor = Color.White;
            catalogHeaderText.Controls.Add(catalogSubtitle);
            catalogHeaderText.Controls.Add(catalogTitle);
            catalogHeaderText.Dock = DockStyle.Fill;
            catalogHeaderText.Location = new Point(0, 0);
            catalogHeaderText.Name = "catalogHeaderText";
            catalogHeaderText.Padding = new Padding(0, 6, 0, 0);
            catalogHeaderText.Size = new Size(440, 60);
            catalogHeaderText.TabIndex = 0;
            // 
            // catalogSubtitle
            // 
            catalogSubtitle.AutoEllipsis = true;
            catalogSubtitle.BackColor = Color.White;
            catalogSubtitle.Dock = DockStyle.Top;
            catalogSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            catalogSubtitle.ForeColor = Color.FromArgb(122, 136, 156);
            catalogSubtitle.Location = new Point(0, 0);
            catalogSubtitle.Name = "catalogSubtitle";
            catalogSubtitle.Size = new Size(440, 18);
            catalogSubtitle.TabIndex = 0;
            catalogSubtitle.Text = "View, add, edit, and manage grooming services, pricing, and time slots.";
            catalogSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            catalogSubtitle.UseMnemonic = false;
            // 
            // catalogTitle
            // 
            catalogTitle.BackColor = Color.White;
            catalogTitle.Dock = DockStyle.Top;
            catalogTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            catalogTitle.ForeColor = Color.FromArgb(31, 42, 60);
            catalogTitle.Location = new Point(0, 0);
            catalogTitle.Name = "catalogTitle";
            catalogTitle.Size = new Size(440, 24);
            catalogTitle.TabIndex = 1;
            catalogTitle.Text = "Service Catalog";
            catalogTitle.TextAlign = ContentAlignment.MiddleLeft;
            catalogTitle.UseMnemonic = false;
            // 
            // catalogIconHost
            // 
            catalogIconHost.BackColor = Color.White;
            catalogIconHost.Controls.Add(catalogIcon);
            catalogIconHost.Dock = DockStyle.Left;
            catalogIconHost.Location = new Point(0, 0);
            catalogIconHost.Name = "catalogIconHost";
            catalogIconHost.Size = new Size(56, 60);
            catalogIconHost.TabIndex = 1;
            // 
            // catalogIcon
            // 
            catalogIcon.Anchor = AnchorStyles.Left;
            catalogIcon.BackColor = Color.White;
            catalogIcon.FillColor = Color.FromArgb(234, 242, 253);
            catalogIcon.ForeColor = Color.FromArgb(58, 123, 213);
            catalogIcon.Glyph = "\uE734";
            catalogIcon.GlyphSize = 14F;
            catalogIcon.Location = new Point(0, 10);
            catalogIcon.Name = "catalogIcon";
            catalogIcon.Radius = 10;
            catalogIcon.Size = new Size(40, 40);
            catalogIcon.TabIndex = 0;
            catalogIcon.TabStop = false;
            // 
            // addServiceHost
            // 
            addServiceHost.BackColor = Color.White;
            addServiceHost.Controls.Add(addServiceButton);
            addServiceHost.Dock = DockStyle.Right;
            addServiceHost.Location = new Point(0, 0);
            addServiceHost.Name = "addServiceHost";
            addServiceHost.Size = new Size(148, 60);
            addServiceHost.TabIndex = 2;
            // 
            // addServiceButton
            // 
            addServiceButton.Anchor = AnchorStyles.Right;
            addServiceButton.BackColor = Color.White;
            addServiceButton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addServiceButton.Location = new Point(16, 12);
            addServiceButton.Name = "addServiceButton";
            addServiceButton.Size = new Size(132, 36);
            addServiceButton.TabIndex = 0;
            addServiceButton.Text = "+ Add Service";
            // 
            // rightColumn
            // 
            rightColumn.BackColor = Color.FromArgb(243, 247, 252);
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightColumn.Controls.Add(categoriesCard, 0, 0);
            rightColumn.Controls.Add(popularCard, 0, 1);
            rightColumn.Controls.Add(quickCard, 0, 2);
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(0, 0);
            rightColumn.Margin = new Padding(0, 0, 0, 0);
            rightColumn.Name = "rightColumn";
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));
            rightColumn.Size = new Size(280, 692);
            rightColumn.TabIndex = 1;
            // 
            // categoriesCard
            // 
            categoriesCard.BorderColor = Color.FromArgb(227, 233, 242);
            categoriesCard.Controls.Add(catOthersRow);
            categoriesCard.Controls.Add(catAddOnsRow);
            categoriesCard.Controls.Add(catHealthRow);
            categoriesCard.Controls.Add(catGroomingRow);
            categoriesCard.Controls.Add(categoriesHeader);
            categoriesCard.CornerColor = Color.FromArgb(243, 247, 252);
            categoriesCard.Dock = DockStyle.Fill;
            categoriesCard.FillColor = Color.White;
            categoriesCard.Location = new Point(0, 0);
            categoriesCard.Margin = new Padding(0, 0, 0, 16);
            categoriesCard.Name = "categoriesCard";
            categoriesCard.Padding = new Padding(16, 12, 16, 12);
            categoriesCard.Radius = 10;
            categoriesCard.Size = new Size(280, 164);
            categoriesCard.TabIndex = 0;
            // 
            // catOthersRow
            // 
            catOthersRow.BackColor = Color.White;
            catOthersRow.Controls.Add(catOthersName);
            catOthersRow.Controls.Add(catOthersDot);
            catOthersRow.Controls.Add(catOthersCount);
            catOthersRow.Dock = DockStyle.Top;
            catOthersRow.Location = new Point(0, 0);
            catOthersRow.Name = "catOthersRow";
            catOthersRow.Size = new Size(248, 26);
            catOthersRow.TabIndex = 0;
            // 
            // catOthersName
            // 
            catOthersName.BackColor = Color.White;
            catOthersName.Dock = DockStyle.Fill;
            catOthersName.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            catOthersName.ForeColor = Color.FromArgb(68, 83, 106);
            catOthersName.Location = new Point(0, 0);
            catOthersName.Name = "catOthersName";
            catOthersName.Size = new Size(150, 26);
            catOthersName.TabIndex = 0;
            catOthersName.Text = "Others";
            catOthersName.TextAlign = ContentAlignment.MiddleLeft;
            catOthersName.UseMnemonic = false;
            // 
            // catOthersDot
            // 
            catOthersDot.BackColor = Color.White;
            catOthersDot.Dock = DockStyle.Left;
            catOthersDot.DotColor = Color.FromArgb(242, 201, 76);
            catOthersDot.Location = new Point(0, 0);
            catOthersDot.Name = "catOthersDot";
            catOthersDot.Size = new Size(20, 26);
            catOthersDot.TabIndex = 1;
            catOthersDot.TabStop = false;
            // 
            // catOthersCount
            // 
            catOthersCount.BackColor = Color.White;
            catOthersCount.Dock = DockStyle.Right;
            catOthersCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            catOthersCount.ForeColor = Color.FromArgb(68, 83, 106);
            catOthersCount.Location = new Point(0, 0);
            catOthersCount.Name = "catOthersCount";
            catOthersCount.Size = new Size(32, 26);
            catOthersCount.TabIndex = 2;
            catOthersCount.Text = "0";
            catOthersCount.TextAlign = ContentAlignment.MiddleRight;
            catOthersCount.UseMnemonic = false;
            // 
            // catAddOnsRow
            // 
            catAddOnsRow.BackColor = Color.White;
            catAddOnsRow.Controls.Add(catAddOnsName);
            catAddOnsRow.Controls.Add(catAddOnsDot);
            catAddOnsRow.Controls.Add(catAddOnsCount);
            catAddOnsRow.Dock = DockStyle.Top;
            catAddOnsRow.Location = new Point(0, 0);
            catAddOnsRow.Name = "catAddOnsRow";
            catAddOnsRow.Size = new Size(248, 26);
            catAddOnsRow.TabIndex = 1;
            // 
            // catAddOnsName
            // 
            catAddOnsName.BackColor = Color.White;
            catAddOnsName.Dock = DockStyle.Fill;
            catAddOnsName.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            catAddOnsName.ForeColor = Color.FromArgb(68, 83, 106);
            catAddOnsName.Location = new Point(0, 0);
            catAddOnsName.Name = "catAddOnsName";
            catAddOnsName.Size = new Size(150, 26);
            catAddOnsName.TabIndex = 0;
            catAddOnsName.Text = "Add-ons";
            catAddOnsName.TextAlign = ContentAlignment.MiddleLeft;
            catAddOnsName.UseMnemonic = false;
            // 
            // catAddOnsDot
            // 
            catAddOnsDot.BackColor = Color.White;
            catAddOnsDot.Dock = DockStyle.Left;
            catAddOnsDot.DotColor = Color.FromArgb(242, 153, 74);
            catAddOnsDot.Location = new Point(0, 0);
            catAddOnsDot.Name = "catAddOnsDot";
            catAddOnsDot.Size = new Size(20, 26);
            catAddOnsDot.TabIndex = 1;
            catAddOnsDot.TabStop = false;
            // 
            // catAddOnsCount
            // 
            catAddOnsCount.BackColor = Color.White;
            catAddOnsCount.Dock = DockStyle.Right;
            catAddOnsCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            catAddOnsCount.ForeColor = Color.FromArgb(68, 83, 106);
            catAddOnsCount.Location = new Point(0, 0);
            catAddOnsCount.Name = "catAddOnsCount";
            catAddOnsCount.Size = new Size(32, 26);
            catAddOnsCount.TabIndex = 2;
            catAddOnsCount.Text = "0";
            catAddOnsCount.TextAlign = ContentAlignment.MiddleRight;
            catAddOnsCount.UseMnemonic = false;
            // 
            // catHealthRow
            // 
            catHealthRow.BackColor = Color.White;
            catHealthRow.Controls.Add(catHealthName);
            catHealthRow.Controls.Add(catHealthDot);
            catHealthRow.Controls.Add(catHealthCount);
            catHealthRow.Dock = DockStyle.Top;
            catHealthRow.Location = new Point(0, 0);
            catHealthRow.Name = "catHealthRow";
            catHealthRow.Size = new Size(248, 26);
            catHealthRow.TabIndex = 2;
            // 
            // catHealthName
            // 
            catHealthName.BackColor = Color.White;
            catHealthName.Dock = DockStyle.Fill;
            catHealthName.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            catHealthName.ForeColor = Color.FromArgb(68, 83, 106);
            catHealthName.Location = new Point(0, 0);
            catHealthName.Name = "catHealthName";
            catHealthName.Size = new Size(150, 26);
            catHealthName.TabIndex = 0;
            catHealthName.Text = "Health & Wellness";
            catHealthName.TextAlign = ContentAlignment.MiddleLeft;
            catHealthName.UseMnemonic = false;
            // 
            // catHealthDot
            // 
            catHealthDot.BackColor = Color.White;
            catHealthDot.Dock = DockStyle.Left;
            catHealthDot.DotColor = Color.FromArgb(160, 86, 224);
            catHealthDot.Location = new Point(0, 0);
            catHealthDot.Name = "catHealthDot";
            catHealthDot.Size = new Size(20, 26);
            catHealthDot.TabIndex = 1;
            catHealthDot.TabStop = false;
            // 
            // catHealthCount
            // 
            catHealthCount.BackColor = Color.White;
            catHealthCount.Dock = DockStyle.Right;
            catHealthCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            catHealthCount.ForeColor = Color.FromArgb(68, 83, 106);
            catHealthCount.Location = new Point(0, 0);
            catHealthCount.Name = "catHealthCount";
            catHealthCount.Size = new Size(32, 26);
            catHealthCount.TabIndex = 2;
            catHealthCount.Text = "0";
            catHealthCount.TextAlign = ContentAlignment.MiddleRight;
            catHealthCount.UseMnemonic = false;
            // 
            // catGroomingRow
            // 
            catGroomingRow.BackColor = Color.White;
            catGroomingRow.Controls.Add(catGroomingName);
            catGroomingRow.Controls.Add(catGroomingDot);
            catGroomingRow.Controls.Add(catGroomingCount);
            catGroomingRow.Dock = DockStyle.Top;
            catGroomingRow.Location = new Point(0, 0);
            catGroomingRow.Name = "catGroomingRow";
            catGroomingRow.Size = new Size(248, 26);
            catGroomingRow.TabIndex = 3;
            // 
            // catGroomingName
            // 
            catGroomingName.BackColor = Color.White;
            catGroomingName.Dock = DockStyle.Fill;
            catGroomingName.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            catGroomingName.ForeColor = Color.FromArgb(68, 83, 106);
            catGroomingName.Location = new Point(0, 0);
            catGroomingName.Name = "catGroomingName";
            catGroomingName.Size = new Size(150, 26);
            catGroomingName.TabIndex = 0;
            catGroomingName.Text = "Grooming";
            catGroomingName.TextAlign = ContentAlignment.MiddleLeft;
            catGroomingName.UseMnemonic = false;
            // 
            // catGroomingDot
            // 
            catGroomingDot.BackColor = Color.White;
            catGroomingDot.Dock = DockStyle.Left;
            catGroomingDot.DotColor = Color.FromArgb(58, 123, 213);
            catGroomingDot.Location = new Point(0, 0);
            catGroomingDot.Name = "catGroomingDot";
            catGroomingDot.Size = new Size(20, 26);
            catGroomingDot.TabIndex = 1;
            catGroomingDot.TabStop = false;
            // 
            // catGroomingCount
            // 
            catGroomingCount.BackColor = Color.White;
            catGroomingCount.Dock = DockStyle.Right;
            catGroomingCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            catGroomingCount.ForeColor = Color.FromArgb(68, 83, 106);
            catGroomingCount.Location = new Point(0, 0);
            catGroomingCount.Name = "catGroomingCount";
            catGroomingCount.Size = new Size(32, 26);
            catGroomingCount.TabIndex = 2;
            catGroomingCount.Text = "0";
            catGroomingCount.TextAlign = ContentAlignment.MiddleRight;
            catGroomingCount.UseMnemonic = false;
            // 
            // categoriesHeader
            // 
            categoriesHeader.BackColor = Color.White;
            categoriesHeader.Controls.Add(categoriesTitle);
            categoriesHeader.Controls.Add(categoriesIcon);
            categoriesHeader.Dock = DockStyle.Top;
            categoriesHeader.Location = new Point(0, 0);
            categoriesHeader.Name = "categoriesHeader";
            categoriesHeader.Size = new Size(248, 30);
            categoriesHeader.TabIndex = 4;
            // 
            // categoriesTitle
            // 
            categoriesTitle.BackColor = Color.White;
            categoriesTitle.Dock = DockStyle.Fill;
            categoriesTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            categoriesTitle.ForeColor = Color.FromArgb(31, 42, 60);
            categoriesTitle.Location = new Point(0, 0);
            categoriesTitle.Name = "categoriesTitle";
            categoriesTitle.Size = new Size(224, 30);
            categoriesTitle.TabIndex = 0;
            categoriesTitle.Text = "SERVICE CATEGORIES";
            categoriesTitle.TextAlign = ContentAlignment.MiddleLeft;
            categoriesTitle.UseMnemonic = false;
            // 
            // categoriesIcon
            // 
            categoriesIcon.BackColor = Color.White;
            categoriesIcon.Dock = DockStyle.Left;
            categoriesIcon.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            categoriesIcon.ForeColor = Color.FromArgb(58, 123, 213);
            categoriesIcon.Location = new Point(0, 0);
            categoriesIcon.Name = "categoriesIcon";
            categoriesIcon.Size = new Size(24, 30);
            categoriesIcon.TabIndex = 1;
            categoriesIcon.Text = "\uE734";
            categoriesIcon.TextAlign = ContentAlignment.MiddleCenter;
            categoriesIcon.UseMnemonic = false;
            // 
            // popularCard
            // 
            popularCard.BorderColor = Color.FromArgb(227, 233, 242);
            popularCard.Controls.Add(popularEmpty);
            popularCard.Controls.Add(popularHeader);
            popularCard.CornerColor = Color.FromArgb(243, 247, 252);
            popularCard.Dock = DockStyle.Fill;
            popularCard.FillColor = Color.White;
            popularCard.Location = new Point(0, 0);
            popularCard.Margin = new Padding(0, 0, 0, 16);
            popularCard.Name = "popularCard";
            popularCard.Padding = new Padding(16, 12, 16, 12);
            popularCard.Radius = 10;
            popularCard.Size = new Size(280, 250);
            popularCard.TabIndex = 1;
            // 
            // popularEmpty
            // 
            popularEmpty.BackColor = Color.White;
            popularEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            popularEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            popularEmpty.Dock = DockStyle.Fill;
            popularEmpty.Glyph = "\uE734";
            popularEmpty.Hint = "Your most booked services will appear here.";
            popularEmpty.Location = new Point(0, 0);
            popularEmpty.Name = "popularEmpty";
            popularEmpty.Size = new Size(248, 200);
            popularEmpty.TabIndex = 0;
            popularEmpty.TabStop = false;
            popularEmpty.Title = "No bookings yet";
            // 
            // popularHeader
            // 
            popularHeader.BackColor = Color.White;
            popularHeader.Controls.Add(popularTitle);
            popularHeader.Controls.Add(popularIcon);
            popularHeader.Dock = DockStyle.Top;
            popularHeader.Location = new Point(0, 0);
            popularHeader.Name = "popularHeader";
            popularHeader.Size = new Size(248, 30);
            popularHeader.TabIndex = 1;
            // 
            // popularTitle
            // 
            popularTitle.BackColor = Color.White;
            popularTitle.Dock = DockStyle.Fill;
            popularTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            popularTitle.ForeColor = Color.FromArgb(31, 42, 60);
            popularTitle.Location = new Point(0, 0);
            popularTitle.Name = "popularTitle";
            popularTitle.Size = new Size(224, 30);
            popularTitle.TabIndex = 0;
            popularTitle.Text = "POPULAR SERVICES";
            popularTitle.TextAlign = ContentAlignment.MiddleLeft;
            popularTitle.UseMnemonic = false;
            // 
            // popularIcon
            // 
            popularIcon.BackColor = Color.White;
            popularIcon.Dock = DockStyle.Left;
            popularIcon.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            popularIcon.ForeColor = Color.FromArgb(58, 123, 213);
            popularIcon.Location = new Point(0, 0);
            popularIcon.Name = "popularIcon";
            popularIcon.Size = new Size(24, 30);
            popularIcon.TabIndex = 1;
            popularIcon.Text = "\uE734";
            popularIcon.TextAlign = ContentAlignment.MiddleCenter;
            popularIcon.UseMnemonic = false;
            // 
            // quickCard
            // 
            quickCard.BorderColor = Color.FromArgb(227, 233, 242);
            quickCard.Controls.Add(timeSlotsAction);
            quickCard.Controls.Add(pricingAction);
            quickCard.Controls.Add(addServiceAction);
            quickCard.Controls.Add(quickHeader);
            quickCard.CornerColor = Color.FromArgb(243, 247, 252);
            quickCard.Dock = DockStyle.Fill;
            quickCard.FillColor = Color.White;
            quickCard.Location = new Point(0, 0);
            quickCard.Margin = new Padding(0, 0, 0, 0);
            quickCard.Name = "quickCard";
            quickCard.Padding = new Padding(16, 12, 16, 12);
            quickCard.Radius = 10;
            quickCard.Size = new Size(280, 230);
            quickCard.TabIndex = 2;
            // 
            // timeSlotsAction
            // 
            timeSlotsAction.BackColor = Color.White;
            timeSlotsAction.Controls.Add(timeSlotsTextHost);
            timeSlotsAction.Controls.Add(timeSlotsIconHost);
            timeSlotsAction.Cursor = Cursors.Hand;
            timeSlotsAction.Dock = DockStyle.Top;
            timeSlotsAction.Location = new Point(0, 0);
            timeSlotsAction.Name = "timeSlotsAction";
            timeSlotsAction.Size = new Size(248, 54);
            timeSlotsAction.TabIndex = 0;
            // 
            // timeSlotsTextHost
            // 
            timeSlotsTextHost.BackColor = Color.White;
            timeSlotsTextHost.Controls.Add(timeSlotsDesc);
            timeSlotsTextHost.Controls.Add(timeSlotsTitle);
            timeSlotsTextHost.Cursor = Cursors.Hand;
            timeSlotsTextHost.Dock = DockStyle.Fill;
            timeSlotsTextHost.Location = new Point(0, 0);
            timeSlotsTextHost.Name = "timeSlotsTextHost";
            timeSlotsTextHost.Padding = new Padding(0, 8, 0, 0);
            timeSlotsTextHost.Size = new Size(200, 54);
            timeSlotsTextHost.TabIndex = 0;
            // 
            // timeSlotsDesc
            // 
            timeSlotsDesc.BackColor = Color.White;
            timeSlotsDesc.Dock = DockStyle.Top;
            timeSlotsDesc.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            timeSlotsDesc.ForeColor = Color.FromArgb(122, 136, 156);
            timeSlotsDesc.Location = new Point(0, 0);
            timeSlotsDesc.Name = "timeSlotsDesc";
            timeSlotsDesc.Size = new Size(200, 16);
            timeSlotsDesc.TabIndex = 0;
            timeSlotsDesc.Text = "Configure duration & availability";
            timeSlotsDesc.TextAlign = ContentAlignment.MiddleLeft;
            timeSlotsDesc.UseMnemonic = false;
            // 
            // timeSlotsTitle
            // 
            timeSlotsTitle.BackColor = Color.White;
            timeSlotsTitle.Dock = DockStyle.Top;
            timeSlotsTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            timeSlotsTitle.ForeColor = Color.FromArgb(31, 42, 60);
            timeSlotsTitle.Location = new Point(0, 0);
            timeSlotsTitle.Name = "timeSlotsTitle";
            timeSlotsTitle.Size = new Size(200, 18);
            timeSlotsTitle.TabIndex = 1;
            timeSlotsTitle.Text = "Set Time Slots";
            timeSlotsTitle.TextAlign = ContentAlignment.MiddleLeft;
            timeSlotsTitle.UseMnemonic = false;
            // 
            // timeSlotsIconHost
            // 
            timeSlotsIconHost.BackColor = Color.White;
            timeSlotsIconHost.Controls.Add(timeSlotsIcon);
            timeSlotsIconHost.Cursor = Cursors.Hand;
            timeSlotsIconHost.Dock = DockStyle.Left;
            timeSlotsIconHost.Location = new Point(0, 0);
            timeSlotsIconHost.Name = "timeSlotsIconHost";
            timeSlotsIconHost.Size = new Size(44, 54);
            timeSlotsIconHost.TabIndex = 1;
            // 
            // timeSlotsIcon
            // 
            timeSlotsIcon.BackColor = Color.White;
            timeSlotsIcon.Circle = true;
            timeSlotsIcon.FillColor = Color.FromArgb(234, 242, 253);
            timeSlotsIcon.ForeColor = Color.FromArgb(58, 123, 213);
            timeSlotsIcon.Glyph = "\uE787";
            timeSlotsIcon.GlyphSize = 10F;
            timeSlotsIcon.Location = new Point(0, 10);
            timeSlotsIcon.Name = "timeSlotsIcon";
            timeSlotsIcon.Radius = 8;
            timeSlotsIcon.Size = new Size(32, 32);
            timeSlotsIcon.TabIndex = 0;
            timeSlotsIcon.TabStop = false;
            // 
            // pricingAction
            // 
            pricingAction.BackColor = Color.White;
            pricingAction.Controls.Add(pricingTextHost);
            pricingAction.Controls.Add(pricingIconHost);
            pricingAction.Cursor = Cursors.Hand;
            pricingAction.Dock = DockStyle.Top;
            pricingAction.Location = new Point(0, 0);
            pricingAction.Name = "pricingAction";
            pricingAction.Size = new Size(248, 54);
            pricingAction.TabIndex = 1;
            // 
            // pricingTextHost
            // 
            pricingTextHost.BackColor = Color.White;
            pricingTextHost.Controls.Add(pricingDesc);
            pricingTextHost.Controls.Add(pricingTitle);
            pricingTextHost.Cursor = Cursors.Hand;
            pricingTextHost.Dock = DockStyle.Fill;
            pricingTextHost.Location = new Point(0, 0);
            pricingTextHost.Name = "pricingTextHost";
            pricingTextHost.Padding = new Padding(0, 8, 0, 0);
            pricingTextHost.Size = new Size(200, 54);
            pricingTextHost.TabIndex = 0;
            // 
            // pricingDesc
            // 
            pricingDesc.BackColor = Color.White;
            pricingDesc.Dock = DockStyle.Top;
            pricingDesc.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pricingDesc.ForeColor = Color.FromArgb(122, 136, 156);
            pricingDesc.Location = new Point(0, 0);
            pricingDesc.Name = "pricingDesc";
            pricingDesc.Size = new Size(200, 16);
            pricingDesc.TabIndex = 0;
            pricingDesc.Text = "Update service rates";
            pricingDesc.TextAlign = ContentAlignment.MiddleLeft;
            pricingDesc.UseMnemonic = false;
            // 
            // pricingTitle
            // 
            pricingTitle.BackColor = Color.White;
            pricingTitle.Dock = DockStyle.Top;
            pricingTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pricingTitle.ForeColor = Color.FromArgb(31, 42, 60);
            pricingTitle.Location = new Point(0, 0);
            pricingTitle.Name = "pricingTitle";
            pricingTitle.Size = new Size(200, 18);
            pricingTitle.TabIndex = 1;
            pricingTitle.Text = "Manage Pricing";
            pricingTitle.TextAlign = ContentAlignment.MiddleLeft;
            pricingTitle.UseMnemonic = false;
            // 
            // pricingIconHost
            // 
            pricingIconHost.BackColor = Color.White;
            pricingIconHost.Controls.Add(pricingIcon);
            pricingIconHost.Cursor = Cursors.Hand;
            pricingIconHost.Dock = DockStyle.Left;
            pricingIconHost.Location = new Point(0, 0);
            pricingIconHost.Name = "pricingIconHost";
            pricingIconHost.Size = new Size(44, 54);
            pricingIconHost.TabIndex = 1;
            // 
            // pricingIcon
            // 
            pricingIcon.BackColor = Color.White;
            pricingIcon.Circle = true;
            pricingIcon.FillColor = Color.FromArgb(234, 242, 253);
            pricingIcon.ForeColor = Color.FromArgb(58, 123, 213);
            pricingIcon.Glyph = "\uE8C7";
            pricingIcon.GlyphSize = 10F;
            pricingIcon.Location = new Point(0, 10);
            pricingIcon.Name = "pricingIcon";
            pricingIcon.Radius = 8;
            pricingIcon.Size = new Size(32, 32);
            pricingIcon.TabIndex = 0;
            pricingIcon.TabStop = false;
            // 
            // addServiceAction
            // 
            addServiceAction.BackColor = Color.White;
            addServiceAction.Controls.Add(addServiceTextHost);
            addServiceAction.Controls.Add(addServiceIconHost);
            addServiceAction.Cursor = Cursors.Hand;
            addServiceAction.Dock = DockStyle.Top;
            addServiceAction.Location = new Point(0, 0);
            addServiceAction.Name = "addServiceAction";
            addServiceAction.Size = new Size(248, 54);
            addServiceAction.TabIndex = 2;
            // 
            // addServiceTextHost
            // 
            addServiceTextHost.BackColor = Color.White;
            addServiceTextHost.Controls.Add(addServiceDesc);
            addServiceTextHost.Controls.Add(addServiceTitle);
            addServiceTextHost.Cursor = Cursors.Hand;
            addServiceTextHost.Dock = DockStyle.Fill;
            addServiceTextHost.Location = new Point(0, 0);
            addServiceTextHost.Name = "addServiceTextHost";
            addServiceTextHost.Padding = new Padding(0, 8, 0, 0);
            addServiceTextHost.Size = new Size(200, 54);
            addServiceTextHost.TabIndex = 0;
            // 
            // addServiceDesc
            // 
            addServiceDesc.BackColor = Color.White;
            addServiceDesc.Dock = DockStyle.Top;
            addServiceDesc.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addServiceDesc.ForeColor = Color.FromArgb(122, 136, 156);
            addServiceDesc.Location = new Point(0, 0);
            addServiceDesc.Name = "addServiceDesc";
            addServiceDesc.Size = new Size(200, 16);
            addServiceDesc.TabIndex = 0;
            addServiceDesc.Text = "Create a new service listing";
            addServiceDesc.TextAlign = ContentAlignment.MiddleLeft;
            addServiceDesc.UseMnemonic = false;
            // 
            // addServiceTitle
            // 
            addServiceTitle.BackColor = Color.White;
            addServiceTitle.Dock = DockStyle.Top;
            addServiceTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addServiceTitle.ForeColor = Color.FromArgb(31, 42, 60);
            addServiceTitle.Location = new Point(0, 0);
            addServiceTitle.Name = "addServiceTitle";
            addServiceTitle.Size = new Size(200, 18);
            addServiceTitle.TabIndex = 1;
            addServiceTitle.Text = "Add New Service";
            addServiceTitle.TextAlign = ContentAlignment.MiddleLeft;
            addServiceTitle.UseMnemonic = false;
            // 
            // addServiceIconHost
            // 
            addServiceIconHost.BackColor = Color.White;
            addServiceIconHost.Controls.Add(addServiceIcon);
            addServiceIconHost.Cursor = Cursors.Hand;
            addServiceIconHost.Dock = DockStyle.Left;
            addServiceIconHost.Location = new Point(0, 0);
            addServiceIconHost.Name = "addServiceIconHost";
            addServiceIconHost.Size = new Size(44, 54);
            addServiceIconHost.TabIndex = 1;
            // 
            // addServiceIcon
            // 
            addServiceIcon.BackColor = Color.White;
            addServiceIcon.Circle = true;
            addServiceIcon.FillColor = Color.FromArgb(234, 242, 253);
            addServiceIcon.ForeColor = Color.FromArgb(58, 123, 213);
            addServiceIcon.Glyph = "\uE710";
            addServiceIcon.GlyphSize = 10F;
            addServiceIcon.Location = new Point(0, 10);
            addServiceIcon.Name = "addServiceIcon";
            addServiceIcon.Radius = 8;
            addServiceIcon.Size = new Size(32, 32);
            addServiceIcon.TabIndex = 0;
            addServiceIcon.TabStop = false;
            // 
            // quickHeader
            // 
            quickHeader.BackColor = Color.White;
            quickHeader.Controls.Add(quickTitle);
            quickHeader.Controls.Add(quickIcon);
            quickHeader.Dock = DockStyle.Top;
            quickHeader.Location = new Point(0, 0);
            quickHeader.Name = "quickHeader";
            quickHeader.Size = new Size(248, 30);
            quickHeader.TabIndex = 3;
            // 
            // quickTitle
            // 
            quickTitle.BackColor = Color.White;
            quickTitle.Dock = DockStyle.Fill;
            quickTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickTitle.ForeColor = Color.FromArgb(31, 42, 60);
            quickTitle.Location = new Point(0, 0);
            quickTitle.Name = "quickTitle";
            quickTitle.Size = new Size(224, 30);
            quickTitle.TabIndex = 0;
            quickTitle.Text = "QUICK ACTIONS";
            quickTitle.TextAlign = ContentAlignment.MiddleLeft;
            quickTitle.UseMnemonic = false;
            // 
            // quickIcon
            // 
            quickIcon.BackColor = Color.White;
            quickIcon.Dock = DockStyle.Left;
            quickIcon.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            quickIcon.ForeColor = Color.FromArgb(58, 123, 213);
            quickIcon.Location = new Point(0, 0);
            quickIcon.Name = "quickIcon";
            quickIcon.Size = new Size(24, 30);
            quickIcon.TabIndex = 1;
            quickIcon.Text = "\uE945";
            quickIcon.TextAlign = ContentAlignment.MiddleCenter;
            quickIcon.UseMnemonic = false;
            // 
            // ServiceCatalogPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            Controls.Add(pageHost);
            Name = "ServiceCatalogPage";
            Size = new Size(1036, 732);
            pageHost.ResumeLayout(false);
            pageGrid.ResumeLayout(false);
            leftColumn.ResumeLayout(false);
            serviceTableCard.ResumeLayout(false);
            serviceTableArea.ResumeLayout(false);
            serviceRows.ResumeLayout(false);
            serviceColumnsHead.ResumeLayout(false);
            serviceColumnsGrid.ResumeLayout(false);
            serviceFooter.ResumeLayout(false);
            servicePager.ResumeLayout(false);
            serviceToolbar.ResumeLayout(false);
            viewToggleHost.ResumeLayout(false);
            serviceStats.ResumeLayout(false);
            svcTotalTile.ResumeLayout(false);
            svcPriceTile.ResumeLayout(false);
            svcDurationTile.ResumeLayout(false);
            svcBookingsTile.ResumeLayout(false);
            catalogHeaderCard.ResumeLayout(false);
            catalogHeaderText.ResumeLayout(false);
            catalogIconHost.ResumeLayout(false);
            addServiceHost.ResumeLayout(false);
            rightColumn.ResumeLayout(false);
            categoriesCard.ResumeLayout(false);
            catOthersRow.ResumeLayout(false);
            catAddOnsRow.ResumeLayout(false);
            catHealthRow.ResumeLayout(false);
            catGroomingRow.ResumeLayout(false);
            categoriesHeader.ResumeLayout(false);
            popularCard.ResumeLayout(false);
            popularHeader.ResumeLayout(false);
            quickCard.ResumeLayout(false);
            timeSlotsAction.ResumeLayout(false);
            timeSlotsTextHost.ResumeLayout(false);
            timeSlotsIconHost.ResumeLayout(false);
            pricingAction.ResumeLayout(false);
            pricingTextHost.ResumeLayout(false);
            pricingIconHost.ResumeLayout(false);
            addServiceAction.ResumeLayout(false);
            addServiceTextHost.ResumeLayout(false);
            addServiceIconHost.ResumeLayout(false);
            quickHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pageHost;
        private System.Windows.Forms.TableLayoutPanel pageGrid;
        private System.Windows.Forms.Panel leftColumn;
        private MyApp.Controls.CardPanel serviceTableCard;
        private System.Windows.Forms.Panel serviceTableArea;
        private System.Windows.Forms.Panel serviceRows;
        private MyApp.Controls.EmptyState serviceEmpty;
        private System.Windows.Forms.Panel serviceColumnsHead;
        private System.Windows.Forms.TableLayoutPanel serviceColumnsGrid;
        private MyApp.Controls.CheckMark serviceHeaderCheck;
        private System.Windows.Forms.Label serviceColService;
        private System.Windows.Forms.Label serviceColCategory;
        private System.Windows.Forms.Label serviceColSize;
        private System.Windows.Forms.Label serviceColDuration;
        private System.Windows.Forms.Label serviceColPrice;
        private System.Windows.Forms.Label serviceColStatus;
        private System.Windows.Forms.Label serviceColActions;
        private System.Windows.Forms.Panel serviceFooter;
        private System.Windows.Forms.Label serviceShowing;
        private System.Windows.Forms.Panel servicePager;
        private MyApp.Controls.IconButton servicePrevPage;
        private MyApp.Controls.PrimaryButton servicePageChip;
        private MyApp.Controls.IconButton serviceNextPage;
        private System.Windows.Forms.Panel serviceToolbar;
        private MyApp.Controls.InputField serviceSearch;
        private MyApp.Controls.SelectField categoryFilter;
        private System.Windows.Forms.Panel toolbarGapC;
        private MyApp.Controls.SelectField sizeFilter;
        private System.Windows.Forms.Panel toolbarGapS;
        private MyApp.Controls.SelectField statusFilter;
        private System.Windows.Forms.Panel toolbarGapT;
        private System.Windows.Forms.Panel viewToggleHost;
        private MyApp.Controls.IconBadge listViewToggle;
        private MyApp.Controls.IconBadge gridViewToggle;
        private System.Windows.Forms.Panel gapTwo;
        private System.Windows.Forms.TableLayoutPanel serviceStats;
        private MyApp.Controls.CardPanel svcTotalTile;
        private System.Windows.Forms.Label svcTotalDelta;
        private System.Windows.Forms.Label svcTotalValue;
        private System.Windows.Forms.Label svcTotalLabel;
        private MyApp.Controls.IconBadge svcTotalIcon;
        private MyApp.Controls.CardPanel svcPriceTile;
        private System.Windows.Forms.Label svcPriceDelta;
        private System.Windows.Forms.Label svcPriceValue;
        private System.Windows.Forms.Label svcPriceLabel;
        private MyApp.Controls.IconBadge svcPriceIcon;
        private MyApp.Controls.CardPanel svcDurationTile;
        private System.Windows.Forms.Label svcDurationDelta;
        private System.Windows.Forms.Label svcDurationValue;
        private System.Windows.Forms.Label svcDurationLabel;
        private MyApp.Controls.IconBadge svcDurationIcon;
        private MyApp.Controls.CardPanel svcBookingsTile;
        private System.Windows.Forms.Label svcBookingsDelta;
        private System.Windows.Forms.Label svcBookingsValue;
        private System.Windows.Forms.Label svcBookingsLabel;
        private MyApp.Controls.IconBadge svcBookingsIcon;
        private System.Windows.Forms.Panel gapOne;
        private MyApp.Controls.CardPanel catalogHeaderCard;
        private System.Windows.Forms.Panel catalogHeaderText;
        private System.Windows.Forms.Label catalogSubtitle;
        private System.Windows.Forms.Label catalogTitle;
        private System.Windows.Forms.Panel catalogIconHost;
        private MyApp.Controls.IconBadge catalogIcon;
        private System.Windows.Forms.Panel addServiceHost;
        private MyApp.Controls.PrimaryButton addServiceButton;
        private System.Windows.Forms.TableLayoutPanel rightColumn;
        private MyApp.Controls.CardPanel categoriesCard;
        private System.Windows.Forms.Panel catOthersRow;
        private System.Windows.Forms.Label catOthersName;
        private MyApp.Controls.Dot catOthersDot;
        private System.Windows.Forms.Label catOthersCount;
        private System.Windows.Forms.Panel catAddOnsRow;
        private System.Windows.Forms.Label catAddOnsName;
        private MyApp.Controls.Dot catAddOnsDot;
        private System.Windows.Forms.Label catAddOnsCount;
        private System.Windows.Forms.Panel catHealthRow;
        private System.Windows.Forms.Label catHealthName;
        private MyApp.Controls.Dot catHealthDot;
        private System.Windows.Forms.Label catHealthCount;
        private System.Windows.Forms.Panel catGroomingRow;
        private System.Windows.Forms.Label catGroomingName;
        private MyApp.Controls.Dot catGroomingDot;
        private System.Windows.Forms.Label catGroomingCount;
        private System.Windows.Forms.Panel categoriesHeader;
        private System.Windows.Forms.Label categoriesTitle;
        private System.Windows.Forms.Label categoriesIcon;
        private MyApp.Controls.CardPanel popularCard;
        private MyApp.Controls.EmptyState popularEmpty;
        private System.Windows.Forms.Panel popularHeader;
        private System.Windows.Forms.Label popularTitle;
        private System.Windows.Forms.Label popularIcon;
        private MyApp.Controls.CardPanel quickCard;
        private System.Windows.Forms.Panel timeSlotsAction;
        private System.Windows.Forms.Panel timeSlotsTextHost;
        private System.Windows.Forms.Label timeSlotsDesc;
        private System.Windows.Forms.Label timeSlotsTitle;
        private System.Windows.Forms.Panel timeSlotsIconHost;
        private MyApp.Controls.IconBadge timeSlotsIcon;
        private System.Windows.Forms.Panel pricingAction;
        private System.Windows.Forms.Panel pricingTextHost;
        private System.Windows.Forms.Label pricingDesc;
        private System.Windows.Forms.Label pricingTitle;
        private System.Windows.Forms.Panel pricingIconHost;
        private MyApp.Controls.IconBadge pricingIcon;
        private System.Windows.Forms.Panel addServiceAction;
        private System.Windows.Forms.Panel addServiceTextHost;
        private System.Windows.Forms.Label addServiceDesc;
        private System.Windows.Forms.Label addServiceTitle;
        private System.Windows.Forms.Panel addServiceIconHost;
        private MyApp.Controls.IconBadge addServiceIcon;
        private System.Windows.Forms.Panel quickHeader;
        private System.Windows.Forms.Label quickTitle;
        private System.Windows.Forms.Label quickIcon;
    }
}
