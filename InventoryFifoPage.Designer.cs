#nullable disable
namespace MyApp
{
    partial class InventoryFifoPage
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
            inventoryTableCard = new MyApp.Controls.CardPanel();
            inventoryTableArea = new System.Windows.Forms.Panel();
            inventoryRows = new System.Windows.Forms.Panel();
            inventoryEmpty = new MyApp.Controls.EmptyState();
            inventoryColumnsHead = new System.Windows.Forms.Panel();
            inventoryColumnsGrid = new System.Windows.Forms.TableLayoutPanel();
            inventoryHeaderCheck = new MyApp.Controls.CheckMark();
            inventoryColProduct = new System.Windows.Forms.Label();
            inventoryColCategory = new System.Windows.Forms.Label();
            inventoryColBatch = new System.Windows.Forms.Label();
            inventoryColSupplier = new System.Windows.Forms.Label();
            inventoryColReceived = new System.Windows.Forms.Label();
            inventoryColExpiry = new System.Windows.Forms.Label();
            inventoryColQuantity = new System.Windows.Forms.Label();
            inventoryColStatus = new System.Windows.Forms.Label();
            inventoryColActions = new System.Windows.Forms.Label();
            inventoryFooter = new System.Windows.Forms.Panel();
            inventoryShowing = new System.Windows.Forms.Label();
            inventoryPager = new System.Windows.Forms.Panel();
            inventoryPrevPage = new MyApp.Controls.IconButton();
            inventoryPageChip = new MyApp.Controls.PrimaryButton();
            inventoryNextPage = new MyApp.Controls.IconButton();
            inventoryToolbar = new System.Windows.Forms.Panel();
            inventorySearch = new MyApp.Controls.InputField();
            categoryFilter = new MyApp.Controls.SelectField();
            toolbarGapC = new System.Windows.Forms.Panel();
            stockStatusFilter = new MyApp.Controls.SelectField();
            toolbarGapS = new System.Windows.Forms.Panel();
            supplierFilter = new MyApp.Controls.SelectField();
            toolbarGapT = new System.Windows.Forms.Panel();
            viewToggleHost = new System.Windows.Forms.Panel();
            listViewToggle = new MyApp.Controls.IconBadge();
            gridViewToggle = new MyApp.Controls.IconBadge();
            gapTwo = new System.Windows.Forms.Panel();
            inventoryStats = new System.Windows.Forms.TableLayoutPanel();
            invProductsTile = new MyApp.Controls.CardPanel();
            invProductsDelta = new System.Windows.Forms.Label();
            invProductsValue = new System.Windows.Forms.Label();
            invProductsTop = new System.Windows.Forms.Panel();
            invProductsLabel = new System.Windows.Forms.Label();
            invProductsIconHost = new System.Windows.Forms.Panel();
            invProductsIcon = new MyApp.Controls.IconBadge();
            invQuantityTile = new MyApp.Controls.CardPanel();
            invQuantityDelta = new System.Windows.Forms.Label();
            invQuantityValue = new System.Windows.Forms.Label();
            invQuantityTop = new System.Windows.Forms.Panel();
            invQuantityLabel = new System.Windows.Forms.Label();
            invQuantityIconHost = new System.Windows.Forms.Panel();
            invQuantityIcon = new MyApp.Controls.IconBadge();
            invLowStockTile = new MyApp.Controls.CardPanel();
            invLowStockDelta = new System.Windows.Forms.Label();
            invLowStockValue = new System.Windows.Forms.Label();
            invLowStockTop = new System.Windows.Forms.Panel();
            invLowStockLabel = new System.Windows.Forms.Label();
            invLowStockIconHost = new System.Windows.Forms.Panel();
            invLowStockIcon = new MyApp.Controls.IconBadge();
            invExpiryTile = new MyApp.Controls.CardPanel();
            invExpiryDelta = new System.Windows.Forms.Label();
            invExpiryValue = new System.Windows.Forms.Label();
            invExpiryTop = new System.Windows.Forms.Panel();
            invExpiryLabel = new System.Windows.Forms.Label();
            invExpiryIconHost = new System.Windows.Forms.Panel();
            invExpiryIcon = new MyApp.Controls.IconBadge();
            gapOne = new System.Windows.Forms.Panel();
            overviewHeaderCard = new MyApp.Controls.CardPanel();
            overviewHeaderText = new System.Windows.Forms.Panel();
            overviewSubtitle = new System.Windows.Forms.Label();
            overviewTitle = new System.Windows.Forms.Label();
            overviewIconHost = new System.Windows.Forms.Panel();
            overviewIcon = new MyApp.Controls.IconBadge();
            addProductHost = new System.Windows.Forms.Panel();
            addProductButton = new MyApp.Controls.PrimaryButton();
            rightColumn = new System.Windows.Forms.TableLayoutPanel();
            alertsCard = new MyApp.Controls.CardPanel();
            alertsEmpty = new MyApp.Controls.EmptyState();
            alertsHeader = new System.Windows.Forms.Panel();
            alertsTitle = new System.Windows.Forms.Label();
            alertsIcon = new System.Windows.Forms.Label();
            alertsLink = new System.Windows.Forms.Label();
            lowStockCard = new MyApp.Controls.CardPanel();
            lowStockEmpty = new MyApp.Controls.EmptyState();
            lowStockHeader = new System.Windows.Forms.Panel();
            lowStockTitle = new System.Windows.Forms.Label();
            lowStockIcon = new System.Windows.Forms.Label();
            lowStockLink = new System.Windows.Forms.Label();
            quickCard = new MyApp.Controls.CardPanel();
            quickGrid = new System.Windows.Forms.TableLayoutPanel();
            addProductTile = new MyApp.Controls.CardPanel();
            addProductIconHost = new System.Windows.Forms.Panel();
            addProductIcon = new MyApp.Controls.IconBadge();
            addProductLabel = new System.Windows.Forms.Label();
            updateStockTile = new MyApp.Controls.CardPanel();
            updateStockIconHost = new System.Windows.Forms.Panel();
            updateStockIcon = new MyApp.Controls.IconBadge();
            updateStockLabel = new System.Windows.Forms.Label();
            viewBatchesTile = new MyApp.Controls.CardPanel();
            viewBatchesIconHost = new System.Windows.Forms.Panel();
            viewBatchesIcon = new MyApp.Controls.IconBadge();
            viewBatchesLabel = new System.Windows.Forms.Label();
            generateReportTile = new MyApp.Controls.CardPanel();
            generateReportIconHost = new System.Windows.Forms.Panel();
            generateReportIcon = new MyApp.Controls.IconBadge();
            generateReportLabel = new System.Windows.Forms.Label();
            quickHeader = new System.Windows.Forms.Panel();
            quickTitle = new System.Windows.Forms.Label();
            quickIcon = new System.Windows.Forms.Label();
            pageHost.SuspendLayout();
            pageGrid.SuspendLayout();
            leftColumn.SuspendLayout();
            inventoryTableCard.SuspendLayout();
            inventoryTableArea.SuspendLayout();
            inventoryRows.SuspendLayout();
            inventoryColumnsHead.SuspendLayout();
            inventoryColumnsGrid.SuspendLayout();
            inventoryFooter.SuspendLayout();
            inventoryPager.SuspendLayout();
            inventoryToolbar.SuspendLayout();
            viewToggleHost.SuspendLayout();
            inventoryStats.SuspendLayout();
            invProductsTile.SuspendLayout();
            invProductsTop.SuspendLayout();
            invProductsIconHost.SuspendLayout();
            invQuantityTile.SuspendLayout();
            invQuantityTop.SuspendLayout();
            invQuantityIconHost.SuspendLayout();
            invLowStockTile.SuspendLayout();
            invLowStockTop.SuspendLayout();
            invLowStockIconHost.SuspendLayout();
            invExpiryTile.SuspendLayout();
            invExpiryTop.SuspendLayout();
            invExpiryIconHost.SuspendLayout();
            overviewHeaderCard.SuspendLayout();
            overviewHeaderText.SuspendLayout();
            overviewIconHost.SuspendLayout();
            addProductHost.SuspendLayout();
            rightColumn.SuspendLayout();
            alertsCard.SuspendLayout();
            alertsHeader.SuspendLayout();
            lowStockCard.SuspendLayout();
            lowStockHeader.SuspendLayout();
            quickCard.SuspendLayout();
            quickGrid.SuspendLayout();
            addProductTile.SuspendLayout();
            addProductIconHost.SuspendLayout();
            updateStockTile.SuspendLayout();
            updateStockIconHost.SuspendLayout();
            viewBatchesTile.SuspendLayout();
            viewBatchesIconHost.SuspendLayout();
            generateReportTile.SuspendLayout();
            generateReportIconHost.SuspendLayout();
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
            leftColumn.Controls.Add(inventoryTableCard);
            leftColumn.Controls.Add(gapTwo);
            leftColumn.Controls.Add(inventoryStats);
            leftColumn.Controls.Add(gapOne);
            leftColumn.Controls.Add(overviewHeaderCard);
            leftColumn.Dock = DockStyle.Fill;
            leftColumn.Location = new Point(0, 0);
            leftColumn.Margin = new Padding(0, 0, 0, 0);
            leftColumn.Name = "leftColumn";
            leftColumn.Size = new Size(700, 692);
            leftColumn.TabIndex = 0;
            // 
            // inventoryTableCard
            // 
            inventoryTableCard.BorderColor = Color.FromArgb(227, 233, 242);
            inventoryTableCard.Controls.Add(inventoryTableArea);
            inventoryTableCard.Controls.Add(inventoryFooter);
            inventoryTableCard.Controls.Add(inventoryToolbar);
            inventoryTableCard.CornerColor = Color.FromArgb(243, 247, 252);
            inventoryTableCard.Dock = DockStyle.Fill;
            inventoryTableCard.FillColor = Color.White;
            inventoryTableCard.Location = new Point(0, 0);
            inventoryTableCard.Name = "inventoryTableCard";
            inventoryTableCard.Padding = new Padding(16, 14, 16, 10);
            inventoryTableCard.Radius = 10;
            inventoryTableCard.Size = new Size(700, 420);
            inventoryTableCard.TabIndex = 0;
            // 
            // inventoryTableArea
            // 
            inventoryTableArea.BackColor = Color.White;
            inventoryTableArea.Controls.Add(inventoryRows);
            inventoryTableArea.Controls.Add(inventoryColumnsHead);
            inventoryTableArea.Dock = DockStyle.Fill;
            inventoryTableArea.Location = new Point(0, 0);
            inventoryTableArea.Name = "inventoryTableArea";
            inventoryTableArea.Size = new Size(636, 286);
            inventoryTableArea.TabIndex = 0;
            // 
            // inventoryRows
            // 
            inventoryRows.AutoScroll = true;
            inventoryRows.BackColor = Color.White;
            inventoryRows.Controls.Add(inventoryEmpty);
            inventoryRows.Dock = DockStyle.Fill;
            inventoryRows.Location = new Point(0, 36);
            inventoryRows.Name = "inventoryRows";
            inventoryRows.Size = new Size(636, 250);
            inventoryRows.TabIndex = 0;
            // 
            // inventoryEmpty
            // 
            inventoryEmpty.BackColor = Color.White;
            inventoryEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            inventoryEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            inventoryEmpty.Dock = DockStyle.Fill;
            inventoryEmpty.Glyph = "\uE7B8";
            inventoryEmpty.Hint = "Products and their stock batches will appear here once they are added.";
            inventoryEmpty.Location = new Point(0, 0);
            inventoryEmpty.Name = "inventoryEmpty";
            inventoryEmpty.Size = new Size(636, 250);
            inventoryEmpty.TabIndex = 0;
            inventoryEmpty.TabStop = false;
            inventoryEmpty.Title = "No products found";
            // 
            // inventoryColumnsHead
            // 
            inventoryColumnsHead.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColumnsHead.Controls.Add(inventoryColumnsGrid);
            inventoryColumnsHead.Dock = DockStyle.Top;
            inventoryColumnsHead.Location = new Point(0, 0);
            inventoryColumnsHead.Name = "inventoryColumnsHead";
            inventoryColumnsHead.Size = new Size(636, 36);
            inventoryColumnsHead.TabIndex = 1;
            // 
            // inventoryColumnsGrid
            // 
            inventoryColumnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColumnsGrid.ColumnCount = 10;
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
            inventoryColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 84F));
            inventoryColumnsGrid.Controls.Add(inventoryHeaderCheck, 0, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColProduct, 1, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColCategory, 2, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColBatch, 3, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColSupplier, 4, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColReceived, 5, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColExpiry, 6, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColQuantity, 7, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColStatus, 8, 0);
            inventoryColumnsGrid.Controls.Add(inventoryColActions, 9, 0);
            inventoryColumnsGrid.Dock = DockStyle.Fill;
            inventoryColumnsGrid.Location = new Point(0, 0);
            inventoryColumnsGrid.Margin = new Padding(0, 0, 0, 0);
            inventoryColumnsGrid.Name = "inventoryColumnsGrid";
            inventoryColumnsGrid.RowCount = 1;
            inventoryColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            inventoryColumnsGrid.Size = new Size(636, 36);
            inventoryColumnsGrid.TabIndex = 0;
            // 
            // inventoryHeaderCheck
            // 
            inventoryHeaderCheck.Anchor = AnchorStyles.None;
            inventoryHeaderCheck.BackColor = Color.FromArgb(246, 248, 251);
            inventoryHeaderCheck.Location = new Point(13, 9);
            inventoryHeaderCheck.Name = "inventoryHeaderCheck";
            inventoryHeaderCheck.Size = new Size(18, 18);
            inventoryHeaderCheck.TabIndex = 0;
            inventoryHeaderCheck.TabStop = false;
            // 
            // inventoryColProduct
            // 
            inventoryColProduct.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColProduct.Dock = DockStyle.Fill;
            inventoryColProduct.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColProduct.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColProduct.Location = new Point(0, 0);
            inventoryColProduct.Margin = new Padding(0, 0, 0, 0);
            inventoryColProduct.Name = "inventoryColProduct";
            inventoryColProduct.Padding = new Padding(8, 0, 0, 0);
            inventoryColProduct.Size = new Size(60, 36);
            inventoryColProduct.TabIndex = 1;
            inventoryColProduct.Text = "PRODUCT NAME";
            inventoryColProduct.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColProduct.UseMnemonic = false;
            // 
            // inventoryColCategory
            // 
            inventoryColCategory.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColCategory.Dock = DockStyle.Fill;
            inventoryColCategory.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColCategory.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColCategory.Location = new Point(0, 0);
            inventoryColCategory.Margin = new Padding(0, 0, 0, 0);
            inventoryColCategory.Name = "inventoryColCategory";
            inventoryColCategory.Padding = new Padding(8, 0, 0, 0);
            inventoryColCategory.Size = new Size(60, 36);
            inventoryColCategory.TabIndex = 2;
            inventoryColCategory.Text = "CATEGORY";
            inventoryColCategory.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColCategory.UseMnemonic = false;
            // 
            // inventoryColBatch
            // 
            inventoryColBatch.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColBatch.Dock = DockStyle.Fill;
            inventoryColBatch.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColBatch.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColBatch.Location = new Point(0, 0);
            inventoryColBatch.Margin = new Padding(0, 0, 0, 0);
            inventoryColBatch.Name = "inventoryColBatch";
            inventoryColBatch.Padding = new Padding(8, 0, 0, 0);
            inventoryColBatch.Size = new Size(60, 36);
            inventoryColBatch.TabIndex = 3;
            inventoryColBatch.Text = "BATCH #";
            inventoryColBatch.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColBatch.UseMnemonic = false;
            // 
            // inventoryColSupplier
            // 
            inventoryColSupplier.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColSupplier.Dock = DockStyle.Fill;
            inventoryColSupplier.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColSupplier.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColSupplier.Location = new Point(0, 0);
            inventoryColSupplier.Margin = new Padding(0, 0, 0, 0);
            inventoryColSupplier.Name = "inventoryColSupplier";
            inventoryColSupplier.Padding = new Padding(8, 0, 0, 0);
            inventoryColSupplier.Size = new Size(60, 36);
            inventoryColSupplier.TabIndex = 4;
            inventoryColSupplier.Text = "SUPPLIER";
            inventoryColSupplier.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColSupplier.UseMnemonic = false;
            // 
            // inventoryColReceived
            // 
            inventoryColReceived.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColReceived.Dock = DockStyle.Fill;
            inventoryColReceived.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColReceived.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColReceived.Location = new Point(0, 0);
            inventoryColReceived.Margin = new Padding(0, 0, 0, 0);
            inventoryColReceived.Name = "inventoryColReceived";
            inventoryColReceived.Padding = new Padding(8, 0, 0, 0);
            inventoryColReceived.Size = new Size(60, 36);
            inventoryColReceived.TabIndex = 5;
            inventoryColReceived.Text = "RECEIVED DATE";
            inventoryColReceived.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColReceived.UseMnemonic = false;
            // 
            // inventoryColExpiry
            // 
            inventoryColExpiry.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColExpiry.Dock = DockStyle.Fill;
            inventoryColExpiry.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColExpiry.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColExpiry.Location = new Point(0, 0);
            inventoryColExpiry.Margin = new Padding(0, 0, 0, 0);
            inventoryColExpiry.Name = "inventoryColExpiry";
            inventoryColExpiry.Padding = new Padding(8, 0, 0, 0);
            inventoryColExpiry.Size = new Size(60, 36);
            inventoryColExpiry.TabIndex = 6;
            inventoryColExpiry.Text = "EXPIRY DATE";
            inventoryColExpiry.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColExpiry.UseMnemonic = false;
            // 
            // inventoryColQuantity
            // 
            inventoryColQuantity.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColQuantity.Dock = DockStyle.Fill;
            inventoryColQuantity.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColQuantity.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColQuantity.Location = new Point(0, 0);
            inventoryColQuantity.Margin = new Padding(0, 0, 0, 0);
            inventoryColQuantity.Name = "inventoryColQuantity";
            inventoryColQuantity.Padding = new Padding(8, 0, 0, 0);
            inventoryColQuantity.Size = new Size(60, 36);
            inventoryColQuantity.TabIndex = 7;
            inventoryColQuantity.Text = "QUANTITY";
            inventoryColQuantity.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColQuantity.UseMnemonic = false;
            // 
            // inventoryColStatus
            // 
            inventoryColStatus.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColStatus.Dock = DockStyle.Fill;
            inventoryColStatus.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColStatus.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColStatus.Location = new Point(0, 0);
            inventoryColStatus.Margin = new Padding(0, 0, 0, 0);
            inventoryColStatus.Name = "inventoryColStatus";
            inventoryColStatus.Padding = new Padding(8, 0, 0, 0);
            inventoryColStatus.Size = new Size(60, 36);
            inventoryColStatus.TabIndex = 8;
            inventoryColStatus.Text = "STATUS";
            inventoryColStatus.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColStatus.UseMnemonic = false;
            // 
            // inventoryColActions
            // 
            inventoryColActions.BackColor = Color.FromArgb(246, 248, 251);
            inventoryColActions.Dock = DockStyle.Fill;
            inventoryColActions.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryColActions.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryColActions.Location = new Point(0, 0);
            inventoryColActions.Margin = new Padding(0, 0, 0, 0);
            inventoryColActions.Name = "inventoryColActions";
            inventoryColActions.Padding = new Padding(8, 0, 0, 0);
            inventoryColActions.Size = new Size(60, 36);
            inventoryColActions.TabIndex = 9;
            inventoryColActions.Text = "ACTIONS";
            inventoryColActions.TextAlign = ContentAlignment.MiddleLeft;
            inventoryColActions.UseMnemonic = false;
            // 
            // inventoryFooter
            // 
            inventoryFooter.BackColor = Color.White;
            inventoryFooter.Controls.Add(inventoryShowing);
            inventoryFooter.Controls.Add(inventoryPager);
            inventoryFooter.Dock = DockStyle.Bottom;
            inventoryFooter.Location = new Point(0, 0);
            inventoryFooter.Name = "inventoryFooter";
            inventoryFooter.Size = new Size(668, 44);
            inventoryFooter.TabIndex = 1;
            // 
            // inventoryShowing
            // 
            inventoryShowing.BackColor = Color.White;
            inventoryShowing.Dock = DockStyle.Fill;
            inventoryShowing.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            inventoryShowing.ForeColor = Color.FromArgb(122, 136, 156);
            inventoryShowing.Location = new Point(0, 0);
            inventoryShowing.Name = "inventoryShowing";
            inventoryShowing.Size = new Size(500, 44);
            inventoryShowing.TabIndex = 0;
            inventoryShowing.Text = "Showing 0 products";
            inventoryShowing.TextAlign = ContentAlignment.MiddleLeft;
            inventoryShowing.UseMnemonic = false;
            // 
            // inventoryPager
            // 
            inventoryPager.BackColor = Color.White;
            inventoryPager.Controls.Add(inventoryPrevPage);
            inventoryPager.Controls.Add(inventoryPageChip);
            inventoryPager.Controls.Add(inventoryNextPage);
            inventoryPager.Dock = DockStyle.Right;
            inventoryPager.Location = new Point(0, 0);
            inventoryPager.Name = "inventoryPager";
            inventoryPager.Size = new Size(116, 44);
            inventoryPager.TabIndex = 1;
            // 
            // inventoryPrevPage
            // 
            inventoryPrevPage.BackColor = Color.White;
            inventoryPrevPage.Enabled = false;
            inventoryPrevPage.Glyph = "\uE76B";
            inventoryPrevPage.Location = new Point(4, 7);
            inventoryPrevPage.Name = "inventoryPrevPage";
            inventoryPrevPage.Size = new Size(30, 30);
            inventoryPrevPage.TabIndex = 0;
            // 
            // inventoryPageChip
            // 
            inventoryPageChip.BackColor = Color.White;
            inventoryPageChip.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inventoryPageChip.Location = new Point(42, 7);
            inventoryPageChip.Name = "inventoryPageChip";
            inventoryPageChip.Size = new Size(30, 30);
            inventoryPageChip.TabIndex = 1;
            inventoryPageChip.Text = "1";
            // 
            // inventoryNextPage
            // 
            inventoryNextPage.BackColor = Color.White;
            inventoryNextPage.Enabled = false;
            inventoryNextPage.Glyph = "\uE76C";
            inventoryNextPage.Location = new Point(80, 7);
            inventoryNextPage.Name = "inventoryNextPage";
            inventoryNextPage.Size = new Size(30, 30);
            inventoryNextPage.TabIndex = 2;
            // 
            // inventoryToolbar
            // 
            inventoryToolbar.BackColor = Color.White;
            inventoryToolbar.Controls.Add(inventorySearch);
            inventoryToolbar.Controls.Add(categoryFilter);
            inventoryToolbar.Controls.Add(toolbarGapC);
            inventoryToolbar.Controls.Add(stockStatusFilter);
            inventoryToolbar.Controls.Add(toolbarGapS);
            inventoryToolbar.Controls.Add(supplierFilter);
            inventoryToolbar.Controls.Add(toolbarGapT);
            inventoryToolbar.Controls.Add(viewToggleHost);
            inventoryToolbar.Dock = DockStyle.Top;
            inventoryToolbar.Location = new Point(0, 0);
            inventoryToolbar.Name = "inventoryToolbar";
            inventoryToolbar.Padding = new Padding(0, 0, 0, 14);
            inventoryToolbar.Size = new Size(668, 52);
            inventoryToolbar.TabIndex = 2;
            // 
            // inventorySearch
            // 
            inventorySearch.BorderColor = Color.FromArgb(227, 233, 242);
            inventorySearch.CornerColor = Color.White;
            inventorySearch.Dock = DockStyle.Fill;
            inventorySearch.FillColor = Color.White;
            inventorySearch.Glyph = "\uE721";
            inventorySearch.Location = new Point(0, 0);
            inventorySearch.Name = "inventorySearch";
            inventorySearch.Padding = new Padding(10, 3, 10, 3);
            inventorySearch.Placeholder = "Search product, batch number...";
            inventorySearch.Radius = 8;
            inventorySearch.Size = new Size(200, 38);
            inventorySearch.TabIndex = 0;
            // 
            // categoryFilter
            // 
            categoryFilter.BackColor = Color.White;
            categoryFilter.Dock = DockStyle.Right;
            categoryFilter.Location = new Point(0, 0);
            categoryFilter.Name = "categoryFilter";
            categoryFilter.Size = new Size(128, 38);
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
            // stockStatusFilter
            // 
            stockStatusFilter.BackColor = Color.White;
            stockStatusFilter.Dock = DockStyle.Right;
            stockStatusFilter.Location = new Point(0, 0);
            stockStatusFilter.Name = "stockStatusFilter";
            stockStatusFilter.Size = new Size(134, 38);
            stockStatusFilter.TabIndex = 3;
            stockStatusFilter.Text = "All Stock Status";
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
            // supplierFilter
            // 
            supplierFilter.BackColor = Color.White;
            supplierFilter.Dock = DockStyle.Right;
            supplierFilter.Location = new Point(0, 0);
            supplierFilter.Name = "supplierFilter";
            supplierFilter.Size = new Size(120, 38);
            supplierFilter.TabIndex = 5;
            supplierFilter.Text = "All Suppliers";
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
            // inventoryStats
            // 
            inventoryStats.BackColor = Color.FromArgb(243, 247, 252);
            inventoryStats.ColumnCount = 7;
            inventoryStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            inventoryStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            inventoryStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            inventoryStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            inventoryStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            inventoryStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            inventoryStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            inventoryStats.Controls.Add(invProductsTile, 0, 0);
            inventoryStats.Controls.Add(invQuantityTile, 2, 0);
            inventoryStats.Controls.Add(invLowStockTile, 4, 0);
            inventoryStats.Controls.Add(invExpiryTile, 6, 0);
            inventoryStats.Dock = DockStyle.Top;
            inventoryStats.Location = new Point(0, 0);
            inventoryStats.Margin = new Padding(0, 0, 0, 0);
            inventoryStats.Name = "inventoryStats";
            inventoryStats.RowCount = 1;
            inventoryStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            inventoryStats.Size = new Size(700, 100);
            inventoryStats.TabIndex = 2;
            // 
            // invProductsTile
            // 
            invProductsTile.BorderColor = Color.FromArgb(227, 233, 242);
            invProductsTile.Controls.Add(invProductsDelta);
            invProductsTile.Controls.Add(invProductsValue);
            invProductsTile.Controls.Add(invProductsTop);
            invProductsTile.CornerColor = Color.FromArgb(243, 247, 252);
            invProductsTile.Dock = DockStyle.Fill;
            invProductsTile.FillColor = Color.White;
            invProductsTile.Location = new Point(0, 0);
            invProductsTile.Margin = new Padding(0, 0, 0, 0);
            invProductsTile.Name = "invProductsTile";
            invProductsTile.Padding = new Padding(12, 10, 12, 8);
            invProductsTile.Radius = 10;
            invProductsTile.Size = new Size(160, 100);
            invProductsTile.TabIndex = 0;
            // 
            // invProductsDelta
            // 
            invProductsDelta.BackColor = Color.White;
            invProductsDelta.Dock = DockStyle.Top;
            invProductsDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            invProductsDelta.ForeColor = Color.FromArgb(122, 136, 156);
            invProductsDelta.Location = new Point(0, 0);
            invProductsDelta.Name = "invProductsDelta";
            invProductsDelta.Size = new Size(130, 16);
            invProductsDelta.TabIndex = 0;
            invProductsDelta.Text = "\u2014 vs. last month";
            invProductsDelta.TextAlign = ContentAlignment.MiddleLeft;
            invProductsDelta.UseMnemonic = false;
            // 
            // invProductsValue
            // 
            invProductsValue.BackColor = Color.White;
            invProductsValue.Dock = DockStyle.Top;
            invProductsValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            invProductsValue.ForeColor = Color.FromArgb(31, 42, 60);
            invProductsValue.Location = new Point(0, 0);
            invProductsValue.Name = "invProductsValue";
            invProductsValue.Size = new Size(130, 28);
            invProductsValue.TabIndex = 1;
            invProductsValue.Text = "0";
            invProductsValue.TextAlign = ContentAlignment.MiddleLeft;
            invProductsValue.UseMnemonic = false;
            // 
            // invProductsTop
            // 
            invProductsTop.BackColor = Color.White;
            invProductsTop.Controls.Add(invProductsLabel);
            invProductsTop.Controls.Add(invProductsIconHost);
            invProductsTop.Dock = DockStyle.Top;
            invProductsTop.Location = new Point(0, 0);
            invProductsTop.Name = "invProductsTop";
            invProductsTop.Size = new Size(130, 34);
            invProductsTop.TabIndex = 2;
            // 
            // invProductsLabel
            // 
            invProductsLabel.BackColor = Color.White;
            invProductsLabel.Dock = DockStyle.Fill;
            invProductsLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            invProductsLabel.ForeColor = Color.FromArgb(68, 83, 106);
            invProductsLabel.Location = new Point(0, 0);
            invProductsLabel.Name = "invProductsLabel";
            invProductsLabel.Size = new Size(90, 34);
            invProductsLabel.TabIndex = 0;
            invProductsLabel.Text = "Total Products";
            invProductsLabel.TextAlign = ContentAlignment.MiddleLeft;
            invProductsLabel.UseMnemonic = false;
            // 
            // invProductsIconHost
            // 
            invProductsIconHost.BackColor = Color.White;
            invProductsIconHost.Controls.Add(invProductsIcon);
            invProductsIconHost.Dock = DockStyle.Left;
            invProductsIconHost.Location = new Point(0, 0);
            invProductsIconHost.Name = "invProductsIconHost";
            invProductsIconHost.Size = new Size(38, 34);
            invProductsIconHost.TabIndex = 1;
            // 
            // invProductsIcon
            // 
            invProductsIcon.BackColor = Color.White;
            invProductsIcon.FillColor = Color.FromArgb(234, 242, 253);
            invProductsIcon.ForeColor = Color.FromArgb(58, 123, 213);
            invProductsIcon.Glyph = "\uE7B8";
            invProductsIcon.GlyphSize = 10F;
            invProductsIcon.Location = new Point(0, 2);
            invProductsIcon.Name = "invProductsIcon";
            invProductsIcon.Radius = 8;
            invProductsIcon.Size = new Size(30, 30);
            invProductsIcon.TabIndex = 0;
            invProductsIcon.TabStop = false;
            // 
            // invQuantityTile
            // 
            invQuantityTile.BorderColor = Color.FromArgb(227, 233, 242);
            invQuantityTile.Controls.Add(invQuantityDelta);
            invQuantityTile.Controls.Add(invQuantityValue);
            invQuantityTile.Controls.Add(invQuantityTop);
            invQuantityTile.CornerColor = Color.FromArgb(243, 247, 252);
            invQuantityTile.Dock = DockStyle.Fill;
            invQuantityTile.FillColor = Color.White;
            invQuantityTile.Location = new Point(0, 0);
            invQuantityTile.Margin = new Padding(0, 0, 0, 0);
            invQuantityTile.Name = "invQuantityTile";
            invQuantityTile.Padding = new Padding(12, 10, 12, 8);
            invQuantityTile.Radius = 10;
            invQuantityTile.Size = new Size(160, 100);
            invQuantityTile.TabIndex = 1;
            // 
            // invQuantityDelta
            // 
            invQuantityDelta.BackColor = Color.White;
            invQuantityDelta.Dock = DockStyle.Top;
            invQuantityDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            invQuantityDelta.ForeColor = Color.FromArgb(122, 136, 156);
            invQuantityDelta.Location = new Point(0, 0);
            invQuantityDelta.Name = "invQuantityDelta";
            invQuantityDelta.Size = new Size(130, 16);
            invQuantityDelta.TabIndex = 0;
            invQuantityDelta.Text = "\u2014 vs. last month";
            invQuantityDelta.TextAlign = ContentAlignment.MiddleLeft;
            invQuantityDelta.UseMnemonic = false;
            // 
            // invQuantityValue
            // 
            invQuantityValue.BackColor = Color.White;
            invQuantityValue.Dock = DockStyle.Top;
            invQuantityValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            invQuantityValue.ForeColor = Color.FromArgb(31, 42, 60);
            invQuantityValue.Location = new Point(0, 0);
            invQuantityValue.Name = "invQuantityValue";
            invQuantityValue.Size = new Size(130, 28);
            invQuantityValue.TabIndex = 1;
            invQuantityValue.Text = "0";
            invQuantityValue.TextAlign = ContentAlignment.MiddleLeft;
            invQuantityValue.UseMnemonic = false;
            // 
            // invQuantityTop
            // 
            invQuantityTop.BackColor = Color.White;
            invQuantityTop.Controls.Add(invQuantityLabel);
            invQuantityTop.Controls.Add(invQuantityIconHost);
            invQuantityTop.Dock = DockStyle.Top;
            invQuantityTop.Location = new Point(0, 0);
            invQuantityTop.Name = "invQuantityTop";
            invQuantityTop.Size = new Size(130, 34);
            invQuantityTop.TabIndex = 2;
            // 
            // invQuantityLabel
            // 
            invQuantityLabel.BackColor = Color.White;
            invQuantityLabel.Dock = DockStyle.Fill;
            invQuantityLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            invQuantityLabel.ForeColor = Color.FromArgb(68, 83, 106);
            invQuantityLabel.Location = new Point(0, 0);
            invQuantityLabel.Name = "invQuantityLabel";
            invQuantityLabel.Size = new Size(90, 34);
            invQuantityLabel.TabIndex = 0;
            invQuantityLabel.Text = "Total Stock Quantity";
            invQuantityLabel.TextAlign = ContentAlignment.MiddleLeft;
            invQuantityLabel.UseMnemonic = false;
            // 
            // invQuantityIconHost
            // 
            invQuantityIconHost.BackColor = Color.White;
            invQuantityIconHost.Controls.Add(invQuantityIcon);
            invQuantityIconHost.Dock = DockStyle.Left;
            invQuantityIconHost.Location = new Point(0, 0);
            invQuantityIconHost.Name = "invQuantityIconHost";
            invQuantityIconHost.Size = new Size(38, 34);
            invQuantityIconHost.TabIndex = 1;
            // 
            // invQuantityIcon
            // 
            invQuantityIcon.BackColor = Color.White;
            invQuantityIcon.FillColor = Color.FromArgb(234, 242, 253);
            invQuantityIcon.ForeColor = Color.FromArgb(58, 123, 213);
            invQuantityIcon.Glyph = "\uE9D9";
            invQuantityIcon.GlyphSize = 10F;
            invQuantityIcon.Location = new Point(0, 2);
            invQuantityIcon.Name = "invQuantityIcon";
            invQuantityIcon.Radius = 8;
            invQuantityIcon.Size = new Size(30, 30);
            invQuantityIcon.TabIndex = 0;
            invQuantityIcon.TabStop = false;
            // 
            // invLowStockTile
            // 
            invLowStockTile.BorderColor = Color.FromArgb(227, 233, 242);
            invLowStockTile.Controls.Add(invLowStockDelta);
            invLowStockTile.Controls.Add(invLowStockValue);
            invLowStockTile.Controls.Add(invLowStockTop);
            invLowStockTile.CornerColor = Color.FromArgb(243, 247, 252);
            invLowStockTile.Dock = DockStyle.Fill;
            invLowStockTile.FillColor = Color.White;
            invLowStockTile.Location = new Point(0, 0);
            invLowStockTile.Margin = new Padding(0, 0, 0, 0);
            invLowStockTile.Name = "invLowStockTile";
            invLowStockTile.Padding = new Padding(12, 10, 12, 8);
            invLowStockTile.Radius = 10;
            invLowStockTile.Size = new Size(160, 100);
            invLowStockTile.TabIndex = 2;
            // 
            // invLowStockDelta
            // 
            invLowStockDelta.BackColor = Color.White;
            invLowStockDelta.Dock = DockStyle.Top;
            invLowStockDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            invLowStockDelta.ForeColor = Color.FromArgb(122, 136, 156);
            invLowStockDelta.Location = new Point(0, 0);
            invLowStockDelta.Name = "invLowStockDelta";
            invLowStockDelta.Size = new Size(130, 16);
            invLowStockDelta.TabIndex = 0;
            invLowStockDelta.Text = "\u2014 vs. last month";
            invLowStockDelta.TextAlign = ContentAlignment.MiddleLeft;
            invLowStockDelta.UseMnemonic = false;
            // 
            // invLowStockValue
            // 
            invLowStockValue.BackColor = Color.White;
            invLowStockValue.Dock = DockStyle.Top;
            invLowStockValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            invLowStockValue.ForeColor = Color.FromArgb(31, 42, 60);
            invLowStockValue.Location = new Point(0, 0);
            invLowStockValue.Name = "invLowStockValue";
            invLowStockValue.Size = new Size(130, 28);
            invLowStockValue.TabIndex = 1;
            invLowStockValue.Text = "0";
            invLowStockValue.TextAlign = ContentAlignment.MiddleLeft;
            invLowStockValue.UseMnemonic = false;
            // 
            // invLowStockTop
            // 
            invLowStockTop.BackColor = Color.White;
            invLowStockTop.Controls.Add(invLowStockLabel);
            invLowStockTop.Controls.Add(invLowStockIconHost);
            invLowStockTop.Dock = DockStyle.Top;
            invLowStockTop.Location = new Point(0, 0);
            invLowStockTop.Name = "invLowStockTop";
            invLowStockTop.Size = new Size(130, 34);
            invLowStockTop.TabIndex = 2;
            // 
            // invLowStockLabel
            // 
            invLowStockLabel.BackColor = Color.White;
            invLowStockLabel.Dock = DockStyle.Fill;
            invLowStockLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            invLowStockLabel.ForeColor = Color.FromArgb(68, 83, 106);
            invLowStockLabel.Location = new Point(0, 0);
            invLowStockLabel.Name = "invLowStockLabel";
            invLowStockLabel.Size = new Size(90, 34);
            invLowStockLabel.TabIndex = 0;
            invLowStockLabel.Text = "Low Stock Items";
            invLowStockLabel.TextAlign = ContentAlignment.MiddleLeft;
            invLowStockLabel.UseMnemonic = false;
            // 
            // invLowStockIconHost
            // 
            invLowStockIconHost.BackColor = Color.White;
            invLowStockIconHost.Controls.Add(invLowStockIcon);
            invLowStockIconHost.Dock = DockStyle.Left;
            invLowStockIconHost.Location = new Point(0, 0);
            invLowStockIconHost.Name = "invLowStockIconHost";
            invLowStockIconHost.Size = new Size(38, 34);
            invLowStockIconHost.TabIndex = 1;
            // 
            // invLowStockIcon
            // 
            invLowStockIcon.BackColor = Color.White;
            invLowStockIcon.FillColor = Color.FromArgb(234, 242, 253);
            invLowStockIcon.ForeColor = Color.FromArgb(58, 123, 213);
            invLowStockIcon.Glyph = "\uE7BA";
            invLowStockIcon.GlyphSize = 10F;
            invLowStockIcon.Location = new Point(0, 2);
            invLowStockIcon.Name = "invLowStockIcon";
            invLowStockIcon.Radius = 8;
            invLowStockIcon.Size = new Size(30, 30);
            invLowStockIcon.TabIndex = 0;
            invLowStockIcon.TabStop = false;
            // 
            // invExpiryTile
            // 
            invExpiryTile.BorderColor = Color.FromArgb(227, 233, 242);
            invExpiryTile.Controls.Add(invExpiryDelta);
            invExpiryTile.Controls.Add(invExpiryValue);
            invExpiryTile.Controls.Add(invExpiryTop);
            invExpiryTile.CornerColor = Color.FromArgb(243, 247, 252);
            invExpiryTile.Dock = DockStyle.Fill;
            invExpiryTile.FillColor = Color.White;
            invExpiryTile.Location = new Point(0, 0);
            invExpiryTile.Margin = new Padding(0, 0, 0, 0);
            invExpiryTile.Name = "invExpiryTile";
            invExpiryTile.Padding = new Padding(12, 10, 12, 8);
            invExpiryTile.Radius = 10;
            invExpiryTile.Size = new Size(160, 100);
            invExpiryTile.TabIndex = 3;
            // 
            // invExpiryDelta
            // 
            invExpiryDelta.BackColor = Color.White;
            invExpiryDelta.Dock = DockStyle.Top;
            invExpiryDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            invExpiryDelta.ForeColor = Color.FromArgb(122, 136, 156);
            invExpiryDelta.Location = new Point(0, 0);
            invExpiryDelta.Name = "invExpiryDelta";
            invExpiryDelta.Size = new Size(130, 16);
            invExpiryDelta.TabIndex = 0;
            invExpiryDelta.Text = "\u2014 vs. last month";
            invExpiryDelta.TextAlign = ContentAlignment.MiddleLeft;
            invExpiryDelta.UseMnemonic = false;
            // 
            // invExpiryValue
            // 
            invExpiryValue.BackColor = Color.White;
            invExpiryValue.Dock = DockStyle.Top;
            invExpiryValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            invExpiryValue.ForeColor = Color.FromArgb(31, 42, 60);
            invExpiryValue.Location = new Point(0, 0);
            invExpiryValue.Name = "invExpiryValue";
            invExpiryValue.Size = new Size(130, 28);
            invExpiryValue.TabIndex = 1;
            invExpiryValue.Text = "0";
            invExpiryValue.TextAlign = ContentAlignment.MiddleLeft;
            invExpiryValue.UseMnemonic = false;
            // 
            // invExpiryTop
            // 
            invExpiryTop.BackColor = Color.White;
            invExpiryTop.Controls.Add(invExpiryLabel);
            invExpiryTop.Controls.Add(invExpiryIconHost);
            invExpiryTop.Dock = DockStyle.Top;
            invExpiryTop.Location = new Point(0, 0);
            invExpiryTop.Name = "invExpiryTop";
            invExpiryTop.Size = new Size(130, 34);
            invExpiryTop.TabIndex = 2;
            // 
            // invExpiryLabel
            // 
            invExpiryLabel.BackColor = Color.White;
            invExpiryLabel.Dock = DockStyle.Fill;
            invExpiryLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            invExpiryLabel.ForeColor = Color.FromArgb(68, 83, 106);
            invExpiryLabel.Location = new Point(0, 0);
            invExpiryLabel.Name = "invExpiryLabel";
            invExpiryLabel.Size = new Size(90, 34);
            invExpiryLabel.TabIndex = 0;
            invExpiryLabel.Text = "Expired / Near Expiry";
            invExpiryLabel.TextAlign = ContentAlignment.MiddleLeft;
            invExpiryLabel.UseMnemonic = false;
            // 
            // invExpiryIconHost
            // 
            invExpiryIconHost.BackColor = Color.White;
            invExpiryIconHost.Controls.Add(invExpiryIcon);
            invExpiryIconHost.Dock = DockStyle.Left;
            invExpiryIconHost.Location = new Point(0, 0);
            invExpiryIconHost.Name = "invExpiryIconHost";
            invExpiryIconHost.Size = new Size(38, 34);
            invExpiryIconHost.TabIndex = 1;
            // 
            // invExpiryIcon
            // 
            invExpiryIcon.BackColor = Color.White;
            invExpiryIcon.FillColor = Color.FromArgb(234, 242, 253);
            invExpiryIcon.ForeColor = Color.FromArgb(58, 123, 213);
            invExpiryIcon.Glyph = "\uE823";
            invExpiryIcon.GlyphSize = 10F;
            invExpiryIcon.Location = new Point(0, 2);
            invExpiryIcon.Name = "invExpiryIcon";
            invExpiryIcon.Radius = 8;
            invExpiryIcon.Size = new Size(30, 30);
            invExpiryIcon.TabIndex = 0;
            invExpiryIcon.TabStop = false;
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
            // overviewHeaderCard
            // 
            overviewHeaderCard.BorderColor = Color.FromArgb(227, 233, 242);
            overviewHeaderCard.Controls.Add(overviewHeaderText);
            overviewHeaderCard.Controls.Add(overviewIconHost);
            overviewHeaderCard.Controls.Add(addProductHost);
            overviewHeaderCard.CornerColor = Color.FromArgb(243, 247, 252);
            overviewHeaderCard.Dock = DockStyle.Top;
            overviewHeaderCard.FillColor = Color.White;
            overviewHeaderCard.Location = new Point(0, 0);
            overviewHeaderCard.Name = "overviewHeaderCard";
            overviewHeaderCard.Padding = new Padding(16, 12, 16, 12);
            overviewHeaderCard.Radius = 10;
            overviewHeaderCard.Size = new Size(700, 84);
            overviewHeaderCard.TabIndex = 4;
            // 
            // overviewHeaderText
            // 
            overviewHeaderText.BackColor = Color.White;
            overviewHeaderText.Controls.Add(overviewSubtitle);
            overviewHeaderText.Controls.Add(overviewTitle);
            overviewHeaderText.Dock = DockStyle.Fill;
            overviewHeaderText.Location = new Point(0, 0);
            overviewHeaderText.Name = "overviewHeaderText";
            overviewHeaderText.Padding = new Padding(0, 6, 0, 0);
            overviewHeaderText.Size = new Size(440, 60);
            overviewHeaderText.TabIndex = 0;
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
            overviewSubtitle.Size = new Size(440, 18);
            overviewSubtitle.TabIndex = 0;
            overviewSubtitle.Text = "Total stock, low stock alerts, and batch information.";
            overviewSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            overviewSubtitle.UseMnemonic = false;
            // 
            // overviewTitle
            // 
            overviewTitle.BackColor = Color.White;
            overviewTitle.Dock = DockStyle.Top;
            overviewTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            overviewTitle.ForeColor = Color.FromArgb(31, 42, 60);
            overviewTitle.Location = new Point(0, 0);
            overviewTitle.Name = "overviewTitle";
            overviewTitle.Size = new Size(440, 24);
            overviewTitle.TabIndex = 1;
            overviewTitle.Text = "Inventory Overview";
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
            overviewIconHost.Size = new Size(56, 60);
            overviewIconHost.TabIndex = 1;
            // 
            // overviewIcon
            // 
            overviewIcon.Anchor = AnchorStyles.Left;
            overviewIcon.BackColor = Color.White;
            overviewIcon.FillColor = Color.FromArgb(234, 242, 253);
            overviewIcon.ForeColor = Color.FromArgb(58, 123, 213);
            overviewIcon.Glyph = "\uE7B8";
            overviewIcon.GlyphSize = 14F;
            overviewIcon.Location = new Point(0, 10);
            overviewIcon.Name = "overviewIcon";
            overviewIcon.Radius = 10;
            overviewIcon.Size = new Size(40, 40);
            overviewIcon.TabIndex = 0;
            overviewIcon.TabStop = false;
            // 
            // addProductHost
            // 
            addProductHost.BackColor = Color.White;
            addProductHost.Controls.Add(addProductButton);
            addProductHost.Dock = DockStyle.Right;
            addProductHost.Location = new Point(0, 0);
            addProductHost.Name = "addProductHost";
            addProductHost.Size = new Size(148, 60);
            addProductHost.TabIndex = 2;
            // 
            // addProductButton
            // 
            addProductButton.Anchor = AnchorStyles.Right;
            addProductButton.BackColor = Color.White;
            addProductButton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addProductButton.Location = new Point(16, 12);
            addProductButton.Name = "addProductButton";
            addProductButton.Size = new Size(132, 36);
            addProductButton.TabIndex = 0;
            addProductButton.Text = "+ Add Product";
            // 
            // rightColumn
            // 
            rightColumn.BackColor = Color.FromArgb(243, 247, 252);
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightColumn.Controls.Add(alertsCard, 0, 0);
            rightColumn.Controls.Add(lowStockCard, 0, 1);
            rightColumn.Controls.Add(quickCard, 0, 2);
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(0, 0);
            rightColumn.Margin = new Padding(0, 0, 0, 0);
            rightColumn.Name = "rightColumn";
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 210F));
            rightColumn.Size = new Size(280, 692);
            rightColumn.TabIndex = 1;
            // 
            // alertsCard
            // 
            alertsCard.BorderColor = Color.FromArgb(227, 233, 242);
            alertsCard.Controls.Add(alertsEmpty);
            alertsCard.Controls.Add(alertsHeader);
            alertsCard.CornerColor = Color.FromArgb(243, 247, 252);
            alertsCard.Dock = DockStyle.Fill;
            alertsCard.FillColor = Color.White;
            alertsCard.Location = new Point(0, 0);
            alertsCard.Margin = new Padding(0, 0, 0, 16);
            alertsCard.Name = "alertsCard";
            alertsCard.Padding = new Padding(16, 12, 16, 12);
            alertsCard.Radius = 10;
            alertsCard.Size = new Size(280, 250);
            alertsCard.TabIndex = 0;
            // 
            // alertsEmpty
            // 
            alertsEmpty.BackColor = Color.White;
            alertsEmpty.BadgeFill = Color.FromArgb(229, 246, 236);
            alertsEmpty.BadgeFore = Color.FromArgb(46, 158, 91);
            alertsEmpty.Dock = DockStyle.Fill;
            alertsEmpty.Glyph = "\uE73E";
            alertsEmpty.Hint = "Low stock, near-expiry and expired alerts will appear here.";
            alertsEmpty.Location = new Point(0, 0);
            alertsEmpty.Name = "alertsEmpty";
            alertsEmpty.Size = new Size(248, 190);
            alertsEmpty.TabIndex = 0;
            alertsEmpty.TabStop = false;
            alertsEmpty.Title = "No alerts right now";
            // 
            // alertsHeader
            // 
            alertsHeader.BackColor = Color.White;
            alertsHeader.Controls.Add(alertsTitle);
            alertsHeader.Controls.Add(alertsIcon);
            alertsHeader.Controls.Add(alertsLink);
            alertsHeader.Dock = DockStyle.Top;
            alertsHeader.Location = new Point(0, 0);
            alertsHeader.Name = "alertsHeader";
            alertsHeader.Size = new Size(248, 34);
            alertsHeader.TabIndex = 1;
            // 
            // alertsTitle
            // 
            alertsTitle.BackColor = Color.White;
            alertsTitle.Dock = DockStyle.Fill;
            alertsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            alertsTitle.ForeColor = Color.FromArgb(31, 42, 60);
            alertsTitle.Location = new Point(26, 0);
            alertsTitle.Name = "alertsTitle";
            alertsTitle.Size = new Size(160, 34);
            alertsTitle.TabIndex = 0;
            alertsTitle.Text = "Alerts";
            alertsTitle.TextAlign = ContentAlignment.MiddleLeft;
            alertsTitle.UseMnemonic = false;
            // 
            // alertsIcon
            // 
            alertsIcon.BackColor = Color.White;
            alertsIcon.Dock = DockStyle.Left;
            alertsIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            alertsIcon.ForeColor = Color.FromArgb(58, 123, 213);
            alertsIcon.Location = new Point(0, 0);
            alertsIcon.Name = "alertsIcon";
            alertsIcon.Size = new Size(26, 34);
            alertsIcon.TabIndex = 1;
            alertsIcon.Text = "\uEA8F";
            alertsIcon.TextAlign = ContentAlignment.MiddleCenter;
            alertsIcon.UseMnemonic = false;
            // 
            // alertsLink
            // 
            alertsLink.BackColor = Color.White;
            alertsLink.Cursor = Cursors.Hand;
            alertsLink.Dock = DockStyle.Right;
            alertsLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            alertsLink.ForeColor = Color.FromArgb(58, 123, 213);
            alertsLink.Location = new Point(0, 0);
            alertsLink.Name = "alertsLink";
            alertsLink.Size = new Size(62, 34);
            alertsLink.TabIndex = 2;
            alertsLink.Text = "View All \u2192";
            alertsLink.TextAlign = ContentAlignment.MiddleRight;
            alertsLink.UseMnemonic = false;
            // 
            // lowStockCard
            // 
            lowStockCard.BorderColor = Color.FromArgb(227, 233, 242);
            lowStockCard.Controls.Add(lowStockEmpty);
            lowStockCard.Controls.Add(lowStockHeader);
            lowStockCard.CornerColor = Color.FromArgb(243, 247, 252);
            lowStockCard.Dock = DockStyle.Fill;
            lowStockCard.FillColor = Color.White;
            lowStockCard.Location = new Point(0, 0);
            lowStockCard.Margin = new Padding(0, 0, 0, 16);
            lowStockCard.Name = "lowStockCard";
            lowStockCard.Padding = new Padding(16, 12, 16, 12);
            lowStockCard.Radius = 10;
            lowStockCard.Size = new Size(280, 250);
            lowStockCard.TabIndex = 1;
            // 
            // lowStockEmpty
            // 
            lowStockEmpty.BackColor = Color.White;
            lowStockEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            lowStockEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            lowStockEmpty.Dock = DockStyle.Fill;
            lowStockEmpty.Glyph = "\uE7B8";
            lowStockEmpty.Hint = "Products running low will appear here.";
            lowStockEmpty.Location = new Point(0, 0);
            lowStockEmpty.Name = "lowStockEmpty";
            lowStockEmpty.Size = new Size(248, 190);
            lowStockEmpty.TabIndex = 0;
            lowStockEmpty.TabStop = false;
            lowStockEmpty.Title = "No low stock items";
            // 
            // lowStockHeader
            // 
            lowStockHeader.BackColor = Color.White;
            lowStockHeader.Controls.Add(lowStockTitle);
            lowStockHeader.Controls.Add(lowStockIcon);
            lowStockHeader.Controls.Add(lowStockLink);
            lowStockHeader.Dock = DockStyle.Top;
            lowStockHeader.Location = new Point(0, 0);
            lowStockHeader.Name = "lowStockHeader";
            lowStockHeader.Size = new Size(248, 34);
            lowStockHeader.TabIndex = 1;
            // 
            // lowStockTitle
            // 
            lowStockTitle.BackColor = Color.White;
            lowStockTitle.Dock = DockStyle.Fill;
            lowStockTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lowStockTitle.ForeColor = Color.FromArgb(31, 42, 60);
            lowStockTitle.Location = new Point(26, 0);
            lowStockTitle.Name = "lowStockTitle";
            lowStockTitle.Size = new Size(160, 34);
            lowStockTitle.TabIndex = 0;
            lowStockTitle.Text = "Low Stock Items";
            lowStockTitle.TextAlign = ContentAlignment.MiddleLeft;
            lowStockTitle.UseMnemonic = false;
            // 
            // lowStockIcon
            // 
            lowStockIcon.BackColor = Color.White;
            lowStockIcon.Dock = DockStyle.Left;
            lowStockIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lowStockIcon.ForeColor = Color.FromArgb(58, 123, 213);
            lowStockIcon.Location = new Point(0, 0);
            lowStockIcon.Name = "lowStockIcon";
            lowStockIcon.Size = new Size(26, 34);
            lowStockIcon.TabIndex = 1;
            lowStockIcon.Text = "\uE7BA";
            lowStockIcon.TextAlign = ContentAlignment.MiddleCenter;
            lowStockIcon.UseMnemonic = false;
            // 
            // lowStockLink
            // 
            lowStockLink.BackColor = Color.White;
            lowStockLink.Cursor = Cursors.Hand;
            lowStockLink.Dock = DockStyle.Right;
            lowStockLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lowStockLink.ForeColor = Color.FromArgb(58, 123, 213);
            lowStockLink.Location = new Point(0, 0);
            lowStockLink.Name = "lowStockLink";
            lowStockLink.Size = new Size(62, 34);
            lowStockLink.TabIndex = 2;
            lowStockLink.Text = "View All \u2192";
            lowStockLink.TextAlign = ContentAlignment.MiddleRight;
            lowStockLink.UseMnemonic = false;
            // 
            // quickCard
            // 
            quickCard.BorderColor = Color.FromArgb(227, 233, 242);
            quickCard.Controls.Add(quickGrid);
            quickCard.Controls.Add(quickHeader);
            quickCard.CornerColor = Color.FromArgb(243, 247, 252);
            quickCard.Dock = DockStyle.Fill;
            quickCard.FillColor = Color.White;
            quickCard.Location = new Point(0, 0);
            quickCard.Margin = new Padding(0, 0, 0, 0);
            quickCard.Name = "quickCard";
            quickCard.Padding = new Padding(16, 12, 16, 12);
            quickCard.Radius = 10;
            quickCard.Size = new Size(280, 210);
            quickCard.TabIndex = 2;
            // 
            // quickGrid
            // 
            quickGrid.BackColor = Color.White;
            quickGrid.ColumnCount = 3;
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            quickGrid.Controls.Add(addProductTile, 0, 0);
            quickGrid.Controls.Add(updateStockTile, 2, 0);
            quickGrid.Controls.Add(viewBatchesTile, 0, 2);
            quickGrid.Controls.Add(generateReportTile, 2, 2);
            quickGrid.Dock = DockStyle.Fill;
            quickGrid.Location = new Point(0, 0);
            quickGrid.Margin = new Padding(0, 0, 0, 0);
            quickGrid.Name = "quickGrid";
            quickGrid.RowCount = 3;
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F));
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            quickGrid.Size = new Size(248, 136);
            quickGrid.TabIndex = 0;
            // 
            // addProductTile
            // 
            addProductTile.BorderColor = Color.FromArgb(227, 233, 242);
            addProductTile.Controls.Add(addProductIconHost);
            addProductTile.Controls.Add(addProductLabel);
            addProductTile.CornerColor = Color.White;
            addProductTile.Cursor = Cursors.Hand;
            addProductTile.Dock = DockStyle.Fill;
            addProductTile.FillColor = Color.White;
            addProductTile.Location = new Point(0, 0);
            addProductTile.Margin = new Padding(0, 0, 0, 0);
            addProductTile.Name = "addProductTile";
            addProductTile.Padding = new Padding(4, 6, 4, 4);
            addProductTile.Radius = 10;
            addProductTile.Size = new Size(110, 62);
            addProductTile.TabIndex = 0;
            // 
            // addProductIconHost
            // 
            addProductIconHost.BackColor = Color.White;
            addProductIconHost.Controls.Add(addProductIcon);
            addProductIconHost.Cursor = Cursors.Hand;
            addProductIconHost.Dock = DockStyle.Fill;
            addProductIconHost.Location = new Point(0, 0);
            addProductIconHost.Name = "addProductIconHost";
            addProductIconHost.Size = new Size(108, 36);
            addProductIconHost.TabIndex = 0;
            // 
            // addProductIcon
            // 
            addProductIcon.Anchor = AnchorStyles.None;
            addProductIcon.BackColor = Color.White;
            addProductIcon.Circle = true;
            addProductIcon.FillColor = Color.FromArgb(234, 242, 253);
            addProductIcon.ForeColor = Color.FromArgb(58, 123, 213);
            addProductIcon.Glyph = "\uE710";
            addProductIcon.GlyphSize = 10F;
            addProductIcon.Location = new Point(40, 4);
            addProductIcon.Name = "addProductIcon";
            addProductIcon.Radius = 8;
            addProductIcon.Size = new Size(28, 28);
            addProductIcon.TabIndex = 0;
            addProductIcon.TabStop = false;
            // 
            // addProductLabel
            // 
            addProductLabel.BackColor = Color.White;
            addProductLabel.Cursor = Cursors.Hand;
            addProductLabel.Dock = DockStyle.Bottom;
            addProductLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addProductLabel.ForeColor = Color.FromArgb(68, 83, 106);
            addProductLabel.Location = new Point(0, 0);
            addProductLabel.Name = "addProductLabel";
            addProductLabel.Size = new Size(108, 22);
            addProductLabel.TabIndex = 1;
            addProductLabel.Text = "Add Product";
            addProductLabel.TextAlign = ContentAlignment.MiddleCenter;
            addProductLabel.UseMnemonic = false;
            // 
            // updateStockTile
            // 
            updateStockTile.BorderColor = Color.FromArgb(227, 233, 242);
            updateStockTile.Controls.Add(updateStockIconHost);
            updateStockTile.Controls.Add(updateStockLabel);
            updateStockTile.CornerColor = Color.White;
            updateStockTile.Cursor = Cursors.Hand;
            updateStockTile.Dock = DockStyle.Fill;
            updateStockTile.FillColor = Color.White;
            updateStockTile.Location = new Point(0, 0);
            updateStockTile.Margin = new Padding(0, 0, 0, 0);
            updateStockTile.Name = "updateStockTile";
            updateStockTile.Padding = new Padding(4, 6, 4, 4);
            updateStockTile.Radius = 10;
            updateStockTile.Size = new Size(110, 62);
            updateStockTile.TabIndex = 1;
            // 
            // updateStockIconHost
            // 
            updateStockIconHost.BackColor = Color.White;
            updateStockIconHost.Controls.Add(updateStockIcon);
            updateStockIconHost.Cursor = Cursors.Hand;
            updateStockIconHost.Dock = DockStyle.Fill;
            updateStockIconHost.Location = new Point(0, 0);
            updateStockIconHost.Name = "updateStockIconHost";
            updateStockIconHost.Size = new Size(108, 36);
            updateStockIconHost.TabIndex = 0;
            // 
            // updateStockIcon
            // 
            updateStockIcon.Anchor = AnchorStyles.None;
            updateStockIcon.BackColor = Color.White;
            updateStockIcon.Circle = true;
            updateStockIcon.FillColor = Color.FromArgb(234, 242, 253);
            updateStockIcon.ForeColor = Color.FromArgb(58, 123, 213);
            updateStockIcon.Glyph = "\uE72C";
            updateStockIcon.GlyphSize = 10F;
            updateStockIcon.Location = new Point(40, 4);
            updateStockIcon.Name = "updateStockIcon";
            updateStockIcon.Radius = 8;
            updateStockIcon.Size = new Size(28, 28);
            updateStockIcon.TabIndex = 0;
            updateStockIcon.TabStop = false;
            // 
            // updateStockLabel
            // 
            updateStockLabel.BackColor = Color.White;
            updateStockLabel.Cursor = Cursors.Hand;
            updateStockLabel.Dock = DockStyle.Bottom;
            updateStockLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            updateStockLabel.ForeColor = Color.FromArgb(68, 83, 106);
            updateStockLabel.Location = new Point(0, 0);
            updateStockLabel.Name = "updateStockLabel";
            updateStockLabel.Size = new Size(108, 22);
            updateStockLabel.TabIndex = 1;
            updateStockLabel.Text = "Update Stock";
            updateStockLabel.TextAlign = ContentAlignment.MiddleCenter;
            updateStockLabel.UseMnemonic = false;
            // 
            // viewBatchesTile
            // 
            viewBatchesTile.BorderColor = Color.FromArgb(227, 233, 242);
            viewBatchesTile.Controls.Add(viewBatchesIconHost);
            viewBatchesTile.Controls.Add(viewBatchesLabel);
            viewBatchesTile.CornerColor = Color.White;
            viewBatchesTile.Cursor = Cursors.Hand;
            viewBatchesTile.Dock = DockStyle.Fill;
            viewBatchesTile.FillColor = Color.White;
            viewBatchesTile.Location = new Point(0, 0);
            viewBatchesTile.Margin = new Padding(0, 0, 0, 0);
            viewBatchesTile.Name = "viewBatchesTile";
            viewBatchesTile.Padding = new Padding(4, 6, 4, 4);
            viewBatchesTile.Radius = 10;
            viewBatchesTile.Size = new Size(110, 62);
            viewBatchesTile.TabIndex = 2;
            // 
            // viewBatchesIconHost
            // 
            viewBatchesIconHost.BackColor = Color.White;
            viewBatchesIconHost.Controls.Add(viewBatchesIcon);
            viewBatchesIconHost.Cursor = Cursors.Hand;
            viewBatchesIconHost.Dock = DockStyle.Fill;
            viewBatchesIconHost.Location = new Point(0, 0);
            viewBatchesIconHost.Name = "viewBatchesIconHost";
            viewBatchesIconHost.Size = new Size(108, 36);
            viewBatchesIconHost.TabIndex = 0;
            // 
            // viewBatchesIcon
            // 
            viewBatchesIcon.Anchor = AnchorStyles.None;
            viewBatchesIcon.BackColor = Color.White;
            viewBatchesIcon.Circle = true;
            viewBatchesIcon.FillColor = Color.FromArgb(234, 242, 253);
            viewBatchesIcon.ForeColor = Color.FromArgb(58, 123, 213);
            viewBatchesIcon.Glyph = "\uE8FD";
            viewBatchesIcon.GlyphSize = 10F;
            viewBatchesIcon.Location = new Point(40, 4);
            viewBatchesIcon.Name = "viewBatchesIcon";
            viewBatchesIcon.Radius = 8;
            viewBatchesIcon.Size = new Size(28, 28);
            viewBatchesIcon.TabIndex = 0;
            viewBatchesIcon.TabStop = false;
            // 
            // viewBatchesLabel
            // 
            viewBatchesLabel.BackColor = Color.White;
            viewBatchesLabel.Cursor = Cursors.Hand;
            viewBatchesLabel.Dock = DockStyle.Bottom;
            viewBatchesLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            viewBatchesLabel.ForeColor = Color.FromArgb(68, 83, 106);
            viewBatchesLabel.Location = new Point(0, 0);
            viewBatchesLabel.Name = "viewBatchesLabel";
            viewBatchesLabel.Size = new Size(108, 22);
            viewBatchesLabel.TabIndex = 1;
            viewBatchesLabel.Text = "View Batches";
            viewBatchesLabel.TextAlign = ContentAlignment.MiddleCenter;
            viewBatchesLabel.UseMnemonic = false;
            // 
            // generateReportTile
            // 
            generateReportTile.BorderColor = Color.FromArgb(227, 233, 242);
            generateReportTile.Controls.Add(generateReportIconHost);
            generateReportTile.Controls.Add(generateReportLabel);
            generateReportTile.CornerColor = Color.White;
            generateReportTile.Cursor = Cursors.Hand;
            generateReportTile.Dock = DockStyle.Fill;
            generateReportTile.FillColor = Color.White;
            generateReportTile.Location = new Point(0, 0);
            generateReportTile.Margin = new Padding(0, 0, 0, 0);
            generateReportTile.Name = "generateReportTile";
            generateReportTile.Padding = new Padding(4, 6, 4, 4);
            generateReportTile.Radius = 10;
            generateReportTile.Size = new Size(110, 62);
            generateReportTile.TabIndex = 3;
            // 
            // generateReportIconHost
            // 
            generateReportIconHost.BackColor = Color.White;
            generateReportIconHost.Controls.Add(generateReportIcon);
            generateReportIconHost.Cursor = Cursors.Hand;
            generateReportIconHost.Dock = DockStyle.Fill;
            generateReportIconHost.Location = new Point(0, 0);
            generateReportIconHost.Name = "generateReportIconHost";
            generateReportIconHost.Size = new Size(108, 36);
            generateReportIconHost.TabIndex = 0;
            // 
            // generateReportIcon
            // 
            generateReportIcon.Anchor = AnchorStyles.None;
            generateReportIcon.BackColor = Color.White;
            generateReportIcon.Circle = true;
            generateReportIcon.FillColor = Color.FromArgb(234, 242, 253);
            generateReportIcon.ForeColor = Color.FromArgb(58, 123, 213);
            generateReportIcon.Glyph = "\uE9F9";
            generateReportIcon.GlyphSize = 10F;
            generateReportIcon.Location = new Point(40, 4);
            generateReportIcon.Name = "generateReportIcon";
            generateReportIcon.Radius = 8;
            generateReportIcon.Size = new Size(28, 28);
            generateReportIcon.TabIndex = 0;
            generateReportIcon.TabStop = false;
            // 
            // generateReportLabel
            // 
            generateReportLabel.BackColor = Color.White;
            generateReportLabel.Cursor = Cursors.Hand;
            generateReportLabel.Dock = DockStyle.Bottom;
            generateReportLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            generateReportLabel.ForeColor = Color.FromArgb(68, 83, 106);
            generateReportLabel.Location = new Point(0, 0);
            generateReportLabel.Name = "generateReportLabel";
            generateReportLabel.Size = new Size(108, 22);
            generateReportLabel.TabIndex = 1;
            generateReportLabel.Text = "Generate Report";
            generateReportLabel.TextAlign = ContentAlignment.MiddleCenter;
            generateReportLabel.UseMnemonic = false;
            // 
            // quickHeader
            // 
            quickHeader.BackColor = Color.White;
            quickHeader.Controls.Add(quickTitle);
            quickHeader.Controls.Add(quickIcon);
            quickHeader.Dock = DockStyle.Top;
            quickHeader.Location = new Point(0, 0);
            quickHeader.Name = "quickHeader";
            quickHeader.Size = new Size(248, 34);
            quickHeader.TabIndex = 1;
            // 
            // quickTitle
            // 
            quickTitle.BackColor = Color.White;
            quickTitle.Dock = DockStyle.Fill;
            quickTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickTitle.ForeColor = Color.FromArgb(31, 42, 60);
            quickTitle.Location = new Point(26, 0);
            quickTitle.Name = "quickTitle";
            quickTitle.Size = new Size(160, 34);
            quickTitle.TabIndex = 0;
            quickTitle.Text = "Quick Actions";
            quickTitle.TextAlign = ContentAlignment.MiddleLeft;
            quickTitle.UseMnemonic = false;
            // 
            // quickIcon
            // 
            quickIcon.BackColor = Color.White;
            quickIcon.Dock = DockStyle.Left;
            quickIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            quickIcon.ForeColor = Color.FromArgb(58, 123, 213);
            quickIcon.Location = new Point(0, 0);
            quickIcon.Name = "quickIcon";
            quickIcon.Size = new Size(26, 34);
            quickIcon.TabIndex = 1;
            quickIcon.Text = "\uE945";
            quickIcon.TextAlign = ContentAlignment.MiddleCenter;
            quickIcon.UseMnemonic = false;
            // 
            // InventoryFifoPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            Controls.Add(pageHost);
            Name = "InventoryFifoPage";
            Size = new Size(1036, 732);
            pageHost.ResumeLayout(false);
            pageGrid.ResumeLayout(false);
            leftColumn.ResumeLayout(false);
            inventoryTableCard.ResumeLayout(false);
            inventoryTableArea.ResumeLayout(false);
            inventoryRows.ResumeLayout(false);
            inventoryColumnsHead.ResumeLayout(false);
            inventoryColumnsGrid.ResumeLayout(false);
            inventoryFooter.ResumeLayout(false);
            inventoryPager.ResumeLayout(false);
            inventoryToolbar.ResumeLayout(false);
            viewToggleHost.ResumeLayout(false);
            inventoryStats.ResumeLayout(false);
            invProductsTile.ResumeLayout(false);
            invProductsTop.ResumeLayout(false);
            invProductsIconHost.ResumeLayout(false);
            invQuantityTile.ResumeLayout(false);
            invQuantityTop.ResumeLayout(false);
            invQuantityIconHost.ResumeLayout(false);
            invLowStockTile.ResumeLayout(false);
            invLowStockTop.ResumeLayout(false);
            invLowStockIconHost.ResumeLayout(false);
            invExpiryTile.ResumeLayout(false);
            invExpiryTop.ResumeLayout(false);
            invExpiryIconHost.ResumeLayout(false);
            overviewHeaderCard.ResumeLayout(false);
            overviewHeaderText.ResumeLayout(false);
            overviewIconHost.ResumeLayout(false);
            addProductHost.ResumeLayout(false);
            rightColumn.ResumeLayout(false);
            alertsCard.ResumeLayout(false);
            alertsHeader.ResumeLayout(false);
            lowStockCard.ResumeLayout(false);
            lowStockHeader.ResumeLayout(false);
            quickCard.ResumeLayout(false);
            quickGrid.ResumeLayout(false);
            addProductTile.ResumeLayout(false);
            addProductIconHost.ResumeLayout(false);
            updateStockTile.ResumeLayout(false);
            updateStockIconHost.ResumeLayout(false);
            viewBatchesTile.ResumeLayout(false);
            viewBatchesIconHost.ResumeLayout(false);
            generateReportTile.ResumeLayout(false);
            generateReportIconHost.ResumeLayout(false);
            quickHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pageHost;
        private System.Windows.Forms.TableLayoutPanel pageGrid;
        private System.Windows.Forms.Panel leftColumn;
        private MyApp.Controls.CardPanel inventoryTableCard;
        private System.Windows.Forms.Panel inventoryTableArea;
        private System.Windows.Forms.Panel inventoryRows;
        private MyApp.Controls.EmptyState inventoryEmpty;
        private System.Windows.Forms.Panel inventoryColumnsHead;
        private System.Windows.Forms.TableLayoutPanel inventoryColumnsGrid;
        private MyApp.Controls.CheckMark inventoryHeaderCheck;
        private System.Windows.Forms.Label inventoryColProduct;
        private System.Windows.Forms.Label inventoryColCategory;
        private System.Windows.Forms.Label inventoryColBatch;
        private System.Windows.Forms.Label inventoryColSupplier;
        private System.Windows.Forms.Label inventoryColReceived;
        private System.Windows.Forms.Label inventoryColExpiry;
        private System.Windows.Forms.Label inventoryColQuantity;
        private System.Windows.Forms.Label inventoryColStatus;
        private System.Windows.Forms.Label inventoryColActions;
        private System.Windows.Forms.Panel inventoryFooter;
        private System.Windows.Forms.Label inventoryShowing;
        private System.Windows.Forms.Panel inventoryPager;
        private MyApp.Controls.IconButton inventoryPrevPage;
        private MyApp.Controls.PrimaryButton inventoryPageChip;
        private MyApp.Controls.IconButton inventoryNextPage;
        private System.Windows.Forms.Panel inventoryToolbar;
        private MyApp.Controls.InputField inventorySearch;
        private MyApp.Controls.SelectField categoryFilter;
        private System.Windows.Forms.Panel toolbarGapC;
        private MyApp.Controls.SelectField stockStatusFilter;
        private System.Windows.Forms.Panel toolbarGapS;
        private MyApp.Controls.SelectField supplierFilter;
        private System.Windows.Forms.Panel toolbarGapT;
        private System.Windows.Forms.Panel viewToggleHost;
        private MyApp.Controls.IconBadge listViewToggle;
        private MyApp.Controls.IconBadge gridViewToggle;
        private System.Windows.Forms.Panel gapTwo;
        private System.Windows.Forms.TableLayoutPanel inventoryStats;
        private MyApp.Controls.CardPanel invProductsTile;
        private System.Windows.Forms.Label invProductsDelta;
        private System.Windows.Forms.Label invProductsValue;
        private System.Windows.Forms.Panel invProductsTop;
        private System.Windows.Forms.Label invProductsLabel;
        private System.Windows.Forms.Panel invProductsIconHost;
        private MyApp.Controls.IconBadge invProductsIcon;
        private MyApp.Controls.CardPanel invQuantityTile;
        private System.Windows.Forms.Label invQuantityDelta;
        private System.Windows.Forms.Label invQuantityValue;
        private System.Windows.Forms.Panel invQuantityTop;
        private System.Windows.Forms.Label invQuantityLabel;
        private System.Windows.Forms.Panel invQuantityIconHost;
        private MyApp.Controls.IconBadge invQuantityIcon;
        private MyApp.Controls.CardPanel invLowStockTile;
        private System.Windows.Forms.Label invLowStockDelta;
        private System.Windows.Forms.Label invLowStockValue;
        private System.Windows.Forms.Panel invLowStockTop;
        private System.Windows.Forms.Label invLowStockLabel;
        private System.Windows.Forms.Panel invLowStockIconHost;
        private MyApp.Controls.IconBadge invLowStockIcon;
        private MyApp.Controls.CardPanel invExpiryTile;
        private System.Windows.Forms.Label invExpiryDelta;
        private System.Windows.Forms.Label invExpiryValue;
        private System.Windows.Forms.Panel invExpiryTop;
        private System.Windows.Forms.Label invExpiryLabel;
        private System.Windows.Forms.Panel invExpiryIconHost;
        private MyApp.Controls.IconBadge invExpiryIcon;
        private System.Windows.Forms.Panel gapOne;
        private MyApp.Controls.CardPanel overviewHeaderCard;
        private System.Windows.Forms.Panel overviewHeaderText;
        private System.Windows.Forms.Label overviewSubtitle;
        private System.Windows.Forms.Label overviewTitle;
        private System.Windows.Forms.Panel overviewIconHost;
        private MyApp.Controls.IconBadge overviewIcon;
        private System.Windows.Forms.Panel addProductHost;
        private MyApp.Controls.PrimaryButton addProductButton;
        private System.Windows.Forms.TableLayoutPanel rightColumn;
        private MyApp.Controls.CardPanel alertsCard;
        private MyApp.Controls.EmptyState alertsEmpty;
        private System.Windows.Forms.Panel alertsHeader;
        private System.Windows.Forms.Label alertsTitle;
        private System.Windows.Forms.Label alertsIcon;
        private System.Windows.Forms.Label alertsLink;
        private MyApp.Controls.CardPanel lowStockCard;
        private MyApp.Controls.EmptyState lowStockEmpty;
        private System.Windows.Forms.Panel lowStockHeader;
        private System.Windows.Forms.Label lowStockTitle;
        private System.Windows.Forms.Label lowStockIcon;
        private System.Windows.Forms.Label lowStockLink;
        private MyApp.Controls.CardPanel quickCard;
        private System.Windows.Forms.TableLayoutPanel quickGrid;
        private MyApp.Controls.CardPanel addProductTile;
        private System.Windows.Forms.Panel addProductIconHost;
        private MyApp.Controls.IconBadge addProductIcon;
        private System.Windows.Forms.Label addProductLabel;
        private MyApp.Controls.CardPanel updateStockTile;
        private System.Windows.Forms.Panel updateStockIconHost;
        private MyApp.Controls.IconBadge updateStockIcon;
        private System.Windows.Forms.Label updateStockLabel;
        private MyApp.Controls.CardPanel viewBatchesTile;
        private System.Windows.Forms.Panel viewBatchesIconHost;
        private MyApp.Controls.IconBadge viewBatchesIcon;
        private System.Windows.Forms.Label viewBatchesLabel;
        private MyApp.Controls.CardPanel generateReportTile;
        private System.Windows.Forms.Panel generateReportIconHost;
        private MyApp.Controls.IconBadge generateReportIcon;
        private System.Windows.Forms.Label generateReportLabel;
        private System.Windows.Forms.Panel quickHeader;
        private System.Windows.Forms.Label quickTitle;
        private System.Windows.Forms.Label quickIcon;
    }
}
