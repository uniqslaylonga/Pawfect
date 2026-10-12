#nullable disable
namespace MyApp
{
    partial class BillingPage
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
            components = new System.ComponentModel.Container();
            mainGrid = new TableLayoutPanel();
            leftColumn = new TableLayoutPanel();
            select1Card = new MyApp.Controls.CardPanel();
            select1Grid = new TableLayoutPanel();
            select1Title = new Label();
            customerMode = new MyApp.Controls.SegmentedControl();
            customerSearch = new MyApp.Controls.InputField();
            customerInfoHost = new Panel();
            customerEmpty = new MyApp.Controls.EmptyState();
            customerResults = new Panel();
            customerCard = new MyApp.Controls.CustomerCardView();
            itemsCard = new MyApp.Controls.CardPanel();
            itemsGrid = new TableLayoutPanel();
            itemsTitle = new Label();
            catalogTabs = new MyApp.Controls.SegmentedControl();
            catalogSearchRow = new TableLayoutPanel();
            catalogSearch = new MyApp.Controls.InputField();
            resetFiltersButton = new MyApp.Controls.OutlineButton();
            chipsFlow = new FlowLayoutPanel();
            allChip = new MyApp.Controls.TabPill();
            catalogList = new Panel();
            catalogEmpty = new MyApp.Controls.EmptyState();
            cartCard = new MyApp.Controls.CardPanel();
            cartGrid = new TableLayoutPanel();
            cartHeader = new Panel();
            cartTitle = new Label();
            clearCartButton = new MyApp.Controls.OutlineButton();
            cartColumns = new Panel();
            cartColumnsGrid = new TableLayoutPanel();
            colCartItem = new Label();
            colCartType = new Label();
            colCartPrice = new Label();
            colCartQty = new Label();
            colCartSubtotal = new Label();
            cartRowsHost = new Panel();
            cartEmpty = new MyApp.Controls.EmptyState();
            notesPanel = new Panel();
            notesCard = new MyApp.Controls.CardPanel();
            notesBox = new MyApp.Controls.HintTextBox();
            notesLabel = new Label();
            summaryPanel = new MyApp.Controls.EdgePanel();
            summaryGrid = new TableLayoutPanel();
            subtotalLabel = new Label();
            subtotalValue = new Label();
            discountLabel = new Label();
            discountMode = new MyApp.Controls.OutlineButton();
            discountCard = new MyApp.Controls.CardPanel();
            discountBox = new TextBox();
            discountValue = new Label();
            totalLabel = new Label();
            totalValue = new Label();
            paymentCard = new MyApp.Controls.CardPanel();
            payGrid = new TableLayoutPanel();
            payTitle = new Label();
            paymentMethod = new MyApp.Controls.SegmentedControl();
            totalDueRow = new TableLayoutPanel();
            totalDueLabel = new Label();
            totalDueValue = new Label();
            receivedLabel = new Label();
            receivedCard = new MyApp.Controls.CardPanel();
            receivedHost = new Panel();
            receivedBox = new TextBox();
            pesoLabel = new Label();
            quickGrid = new TableLayoutPanel();
            quick1Button = new MyApp.Controls.OutlineButton();
            quick2Button = new MyApp.Controls.OutlineButton();
            quick3Button = new MyApp.Controls.OutlineButton();
            quick4Button = new MyApp.Controls.OutlineButton();
            changeCard = new MyApp.Controls.CardPanel();
            changeValue = new Label();
            changeLabel = new Label();
            printButton = new MyApp.Controls.PrimaryButton();
            receiptPreview = new MyApp.Controls.ReceiptPreview();
            recentHost = new Panel();
            recentCard = new MyApp.Controls.CardPanel();
            recentRowsHost = new Panel();
            recentEmpty = new MyApp.Controls.EmptyState();
            recentColumns = new Panel();
            recentColumnsGrid = new TableLayoutPanel();
            colRecReceipt = new Label();
            colRecDate = new Label();
            colRecCustomer = new Label();
            colRecItems = new Label();
            colRecTotal = new Label();
            colRecMethod = new Label();
            colRecStatus = new Label();
            colRecActions = new Label();
            recentHeader = new Panel();
            recentViewAll = new Label();
            recentTitle = new Label();
            recentIcon = new Label();
            actionsBar = new Panel();
            actionsFlow = new FlowLayoutPanel();
            newTransactionButton = new MyApp.Controls.OutlineButton();
            ongoingButton = new MyApp.Controls.OutlineButton();
            historyButton = new MyApp.Controls.OutlineButton();
            dateTimePanel = new Panel();
            dateIcon = new Label();
            dateLabel = new Label();
            timeIcon = new Label();
            timeLabel = new Label();
            dateTimer = new System.Windows.Forms.Timer(components);
            mainGrid.SuspendLayout();
            leftColumn.SuspendLayout();
            select1Card.SuspendLayout();
            select1Grid.SuspendLayout();
            customerInfoHost.SuspendLayout();
            itemsCard.SuspendLayout();
            itemsGrid.SuspendLayout();
            catalogSearchRow.SuspendLayout();
            chipsFlow.SuspendLayout();
            catalogList.SuspendLayout();
            cartCard.SuspendLayout();
            cartGrid.SuspendLayout();
            cartHeader.SuspendLayout();
            cartColumns.SuspendLayout();
            cartColumnsGrid.SuspendLayout();
            cartRowsHost.SuspendLayout();
            notesPanel.SuspendLayout();
            notesCard.SuspendLayout();
            summaryPanel.SuspendLayout();
            summaryGrid.SuspendLayout();
            discountCard.SuspendLayout();
            paymentCard.SuspendLayout();
            payGrid.SuspendLayout();
            totalDueRow.SuspendLayout();
            receivedCard.SuspendLayout();
            receivedHost.SuspendLayout();
            quickGrid.SuspendLayout();
            changeCard.SuspendLayout();
            recentHost.SuspendLayout();
            recentCard.SuspendLayout();
            recentRowsHost.SuspendLayout();
            recentColumns.SuspendLayout();
            recentColumnsGrid.SuspendLayout();
            recentHeader.SuspendLayout();
            actionsBar.SuspendLayout();
            actionsFlow.SuspendLayout();
            dateTimePanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainGrid
            // 
            mainGrid.BackColor = Color.FromArgb(243, 247, 252);
            mainGrid.ColumnCount = 3;
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 296F));
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 296F));
            mainGrid.Controls.Add(leftColumn, 0, 0);
            mainGrid.Controls.Add(cartCard, 1, 0);
            mainGrid.Controls.Add(paymentCard, 2, 0);
            mainGrid.Dock = DockStyle.Fill;
            mainGrid.Location = new Point(20, 80);
            mainGrid.Margin = new Padding(0);
            mainGrid.Name = "mainGrid";
            mainGrid.RowCount = 1;
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainGrid.Size = new Size(960, 810);
            mainGrid.TabIndex = 0;
            // 
            // leftColumn
            // 
            leftColumn.BackColor = Color.FromArgb(243, 247, 252);
            leftColumn.ColumnCount = 1;
            leftColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            leftColumn.Controls.Add(select1Card, 0, 0);
            leftColumn.Controls.Add(itemsCard, 0, 1);
            leftColumn.Dock = DockStyle.Fill;
            leftColumn.Location = new Point(0, 0);
            leftColumn.Margin = new Padding(0, 0, 16, 0);
            leftColumn.Name = "leftColumn";
            leftColumn.RowCount = 2;
            leftColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 292F));
            leftColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            leftColumn.Size = new Size(280, 810);
            leftColumn.TabIndex = 0;
            // 
            // select1Card
            // 
            select1Card.BorderColor = Color.FromArgb(227, 233, 242);
            select1Card.Controls.Add(select1Grid);
            select1Card.CornerColor = Color.FromArgb(243, 247, 252);
            select1Card.Dock = DockStyle.Fill;
            select1Card.FillColor = Color.White;
            select1Card.Location = new Point(0, 0);
            select1Card.Margin = new Padding(0, 0, 0, 16);
            select1Card.Name = "select1Card";
            select1Card.Padding = new Padding(16, 14, 16, 14);
            select1Card.Radius = 10;
            select1Card.Size = new Size(280, 276);
            select1Card.TabIndex = 0;
            // 
            // select1Grid
            // 
            select1Grid.BackColor = Color.White;
            select1Grid.ColumnCount = 1;
            select1Grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            select1Grid.Controls.Add(select1Title, 0, 0);
            select1Grid.Controls.Add(customerMode, 0, 1);
            select1Grid.Controls.Add(customerSearch, 0, 2);
            select1Grid.Controls.Add(customerInfoHost, 0, 3);
            select1Grid.Dock = DockStyle.Fill;
            select1Grid.Location = new Point(16, 14);
            select1Grid.Margin = new Padding(0);
            select1Grid.Name = "select1Grid";
            select1Grid.RowCount = 4;
            select1Grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            select1Grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            select1Grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            select1Grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            select1Grid.Size = new Size(248, 248);
            select1Grid.TabIndex = 0;
            // 
            // select1Title
            // 
            select1Title.BackColor = Color.White;
            select1Title.Dock = DockStyle.Fill;
            select1Title.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            select1Title.ForeColor = Color.FromArgb(31, 42, 60);
            select1Title.Location = new Point(3, 0);
            select1Title.Name = "select1Title";
            select1Title.Size = new Size(242, 28);
            select1Title.TabIndex = 0;
            select1Title.Text = "1. Select Customer / Appointment";
            select1Title.TextAlign = ContentAlignment.MiddleLeft;
            select1Title.UseMnemonic = false;
            // 
            // customerMode
            // 
            customerMode.BackColor = Color.White;
            customerMode.Dock = DockStyle.Fill;
            customerMode.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            customerMode.Location = new Point(0, 28);
            customerMode.Margin = new Padding(0, 0, 0, 10);
            customerMode.Name = "customerMode";
            customerMode.Segments = new string[]
    {
    "Walk-in (Retail Only)",
    "From Appointment"
    };
            customerMode.SelectedIndex = 1;
            customerMode.Size = new Size(248, 36);
            customerMode.TabIndex = 1;
            customerMode.SelectedIndexChanged += customerMode_SelectedIndexChanged;
            // 
            // customerSearch
            // 
            customerSearch.BorderColor = Color.FromArgb(227, 233, 242);
            customerSearch.CornerColor = Color.White;
            customerSearch.Dock = DockStyle.Fill;
            customerSearch.FillColor = Color.White;
            customerSearch.Glyph = "";
            customerSearch.Location = new Point(0, 74);
            customerSearch.Margin = new Padding(0, 0, 0, 10);
            customerSearch.Name = "customerSearch";
            customerSearch.Padding = new Padding(10, 3, 10, 3);
            customerSearch.Placeholder = "Search customer or appointment...";
            customerSearch.Radius = 8;
            customerSearch.Size = new Size(248, 36);
            customerSearch.TabIndex = 2;
            // 
            // customerInfoHost
            // 
            customerInfoHost.BackColor = Color.White;
            customerInfoHost.Controls.Add(customerEmpty);
            customerInfoHost.Controls.Add(customerResults);
            customerInfoHost.Controls.Add(customerCard);
            customerInfoHost.Dock = DockStyle.Fill;
            customerInfoHost.Location = new Point(0, 120);
            customerInfoHost.Margin = new Padding(0);
            customerInfoHost.Name = "customerInfoHost";
            customerInfoHost.Size = new Size(248, 128);
            customerInfoHost.TabIndex = 3;
            // 
            // customerEmpty
            // 
            customerEmpty.BackColor = Color.White;
            customerEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            customerEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            customerEmpty.Dock = DockStyle.Fill;
            customerEmpty.Glyph = "";
            customerEmpty.Hint = "Search above to attach an appointment to this sale.";
            customerEmpty.Location = new Point(0, 0);
            customerEmpty.Name = "customerEmpty";
            customerEmpty.Size = new Size(248, 128);
            customerEmpty.TabIndex = 0;
            customerEmpty.TabStop = false;
            customerEmpty.Title = "No customer selected";
            // 
            // customerResults
            // 
            customerResults.AutoScroll = true;
            customerResults.BackColor = Color.White;
            customerResults.Dock = DockStyle.Fill;
            customerResults.Location = new Point(0, 0);
            customerResults.Name = "customerResults";
            customerResults.Size = new Size(248, 128);
            customerResults.TabIndex = 1;
            // 
            // customerCard
            // 
            customerCard.BackColor = Color.White;
            customerCard.Dock = DockStyle.Fill;
            customerCard.Location = new Point(0, 0);
            customerCard.Name = "customerCard";
            customerCard.Size = new Size(248, 128);
            customerCard.TabIndex = 2;
            customerCard.TabStop = false;
            // 
            // itemsCard
            // 
            itemsCard.BorderColor = Color.FromArgb(227, 233, 242);
            itemsCard.Controls.Add(itemsGrid);
            itemsCard.CornerColor = Color.FromArgb(243, 247, 252);
            itemsCard.Dock = DockStyle.Fill;
            itemsCard.FillColor = Color.White;
            itemsCard.Location = new Point(0, 292);
            itemsCard.Margin = new Padding(0);
            itemsCard.Name = "itemsCard";
            itemsCard.Padding = new Padding(16, 14, 16, 14);
            itemsCard.Radius = 10;
            itemsCard.Size = new Size(280, 518);
            itemsCard.TabIndex = 1;
            // 
            // itemsGrid
            // 
            itemsGrid.BackColor = Color.White;
            itemsGrid.ColumnCount = 1;
            itemsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            itemsGrid.Controls.Add(itemsTitle, 0, 0);
            itemsGrid.Controls.Add(catalogTabs, 0, 1);
            itemsGrid.Controls.Add(catalogSearchRow, 0, 2);
            itemsGrid.Controls.Add(chipsFlow, 0, 3);
            itemsGrid.Controls.Add(catalogList, 0, 4);
            itemsGrid.Dock = DockStyle.Fill;
            itemsGrid.Location = new Point(16, 14);
            itemsGrid.Margin = new Padding(0);
            itemsGrid.Name = "itemsGrid";
            itemsGrid.RowCount = 5;
            itemsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            itemsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            itemsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            itemsGrid.RowStyles.Add(new RowStyle());
            itemsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            itemsGrid.Size = new Size(248, 490);
            itemsGrid.TabIndex = 0;
            // 
            // itemsTitle
            // 
            itemsTitle.BackColor = Color.White;
            itemsTitle.Dock = DockStyle.Fill;
            itemsTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            itemsTitle.ForeColor = Color.FromArgb(31, 42, 60);
            itemsTitle.Location = new Point(3, 0);
            itemsTitle.Name = "itemsTitle";
            itemsTitle.Size = new Size(242, 28);
            itemsTitle.TabIndex = 0;
            itemsTitle.Text = "2. Add Items / Services";
            itemsTitle.TextAlign = ContentAlignment.MiddleLeft;
            itemsTitle.UseMnemonic = false;
            // 
            // catalogTabs
            // 
            catalogTabs.BackColor = Color.White;
            catalogTabs.Dock = DockStyle.Fill;
            catalogTabs.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            catalogTabs.Location = new Point(0, 28);
            catalogTabs.Margin = new Padding(0, 0, 0, 10);
            catalogTabs.Name = "catalogTabs";
            catalogTabs.Segments = new string[]
    {
    "Retail Items",
    "Services (Optional)"
    };
            catalogTabs.Size = new Size(248, 36);
            catalogTabs.TabIndex = 1;
            catalogTabs.SelectedIndexChanged += catalogTabs_SelectedIndexChanged;
            // 
            // catalogSearchRow
            // 
            catalogSearchRow.BackColor = Color.White;
            catalogSearchRow.ColumnCount = 2;
            catalogSearchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            catalogSearchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 38F));
            catalogSearchRow.Controls.Add(catalogSearch, 0, 0);
            catalogSearchRow.Controls.Add(resetFiltersButton, 1, 0);
            catalogSearchRow.Dock = DockStyle.Fill;
            catalogSearchRow.Location = new Point(0, 74);
            catalogSearchRow.Margin = new Padding(0, 0, 0, 10);
            catalogSearchRow.Name = "catalogSearchRow";
            catalogSearchRow.RowCount = 1;
            catalogSearchRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            catalogSearchRow.Size = new Size(248, 38);
            catalogSearchRow.TabIndex = 2;
            // 
            // catalogSearch
            // 
            catalogSearch.BorderColor = Color.FromArgb(227, 233, 242);
            catalogSearch.CornerColor = Color.White;
            catalogSearch.Dock = DockStyle.Fill;
            catalogSearch.FillColor = Color.White;
            catalogSearch.Glyph = "";
            catalogSearch.Location = new Point(0, 0);
            catalogSearch.Margin = new Padding(0, 0, 8, 0);
            catalogSearch.Name = "catalogSearch";
            catalogSearch.Padding = new Padding(10, 3, 10, 3);
            catalogSearch.Placeholder = "Search by product name or code...";
            catalogSearch.Radius = 8;
            catalogSearch.Size = new Size(202, 38);
            catalogSearch.TabIndex = 0;
            // 
            // resetFiltersButton
            // 
            resetFiltersButton.BackColor = Color.White;
            resetFiltersButton.Dock = DockStyle.Fill;
            resetFiltersButton.Font = new Font("Segoe UI", 8.5F);
            resetFiltersButton.Glyph = "";
            resetFiltersButton.Kind = MyApp.Controls.ButtonKind.Primary;
            resetFiltersButton.Location = new Point(210, 0);
            resetFiltersButton.Margin = new Padding(0);
            resetFiltersButton.Name = "resetFiltersButton";
            resetFiltersButton.Size = new Size(38, 38);
            resetFiltersButton.TabIndex = 1;
            resetFiltersButton.Click += resetFiltersButton_Click;
            // 
            // chipsFlow
            // 
            chipsFlow.AutoSize = true;
            chipsFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            chipsFlow.BackColor = Color.White;
            chipsFlow.Controls.Add(allChip);
            chipsFlow.Dock = DockStyle.Fill;
            chipsFlow.Location = new Point(0, 122);
            chipsFlow.Margin = new Padding(0, 0, 0, 4);
            chipsFlow.Name = "chipsFlow";
            chipsFlow.Size = new Size(248, 32);
            chipsFlow.TabIndex = 3;
            // 
            // allChip
            // 
            allChip.Active = true;
            allChip.BackColor = Color.White;
            allChip.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            allChip.Location = new Point(0, 0);
            allChip.Margin = new Padding(0, 0, 6, 6);
            allChip.Name = "allChip";
            allChip.Size = new Size(45, 26);
            allChip.TabIndex = 0;
            allChip.Text = "All";
            allChip.Click += categoryChip_Click;
            // 
            // catalogList
            // 
            catalogList.AutoScroll = true;
            catalogList.BackColor = Color.White;
            catalogList.Controls.Add(catalogEmpty);
            catalogList.Dock = DockStyle.Fill;
            catalogList.Location = new Point(3, 161);
            catalogList.Name = "catalogList";
            catalogList.Size = new Size(242, 326);
            catalogList.TabIndex = 4;
            // 
            // catalogEmpty
            // 
            catalogEmpty.BackColor = Color.White;
            catalogEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            catalogEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            catalogEmpty.Dock = DockStyle.Fill;
            catalogEmpty.Glyph = "";
            catalogEmpty.Hint = "Products will show up here once they are added.";
            catalogEmpty.Location = new Point(0, 0);
            catalogEmpty.Name = "catalogEmpty";
            catalogEmpty.Size = new Size(242, 326);
            catalogEmpty.TabIndex = 0;
            catalogEmpty.TabStop = false;
            catalogEmpty.Title = "No products yet";
            // 
            // cartCard
            // 
            cartCard.BorderColor = Color.FromArgb(227, 233, 242);
            cartCard.Controls.Add(cartGrid);
            cartCard.CornerColor = Color.FromArgb(243, 247, 252);
            cartCard.Dock = DockStyle.Fill;
            cartCard.FillColor = Color.White;
            cartCard.Location = new Point(296, 0);
            cartCard.Margin = new Padding(0, 0, 16, 0);
            cartCard.Name = "cartCard";
            cartCard.Padding = new Padding(16, 14, 16, 14);
            cartCard.Radius = 10;
            cartCard.Size = new Size(352, 810);
            cartCard.TabIndex = 1;
            // 
            // cartGrid
            // 
            cartGrid.BackColor = Color.White;
            cartGrid.ColumnCount = 1;
            cartGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cartGrid.Controls.Add(cartHeader, 0, 0);
            cartGrid.Controls.Add(cartColumns, 0, 1);
            cartGrid.Controls.Add(cartRowsHost, 0, 2);
            cartGrid.Controls.Add(notesPanel, 0, 3);
            cartGrid.Controls.Add(summaryPanel, 0, 4);
            cartGrid.Dock = DockStyle.Fill;
            cartGrid.Location = new Point(16, 14);
            cartGrid.Margin = new Padding(0);
            cartGrid.Name = "cartGrid";
            cartGrid.RowCount = 5;
            cartGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            cartGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            cartGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cartGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            cartGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));
            cartGrid.Size = new Size(320, 782);
            cartGrid.TabIndex = 0;
            // 
            // cartHeader
            // 
            cartHeader.BackColor = Color.White;
            cartHeader.Controls.Add(cartTitle);
            cartHeader.Controls.Add(clearCartButton);
            cartHeader.Dock = DockStyle.Fill;
            cartHeader.Location = new Point(0, 0);
            cartHeader.Margin = new Padding(0);
            cartHeader.Name = "cartHeader";
            cartHeader.Padding = new Padding(0, 4, 0, 4);
            cartHeader.Size = new Size(320, 40);
            cartHeader.TabIndex = 0;
            // 
            // cartTitle
            // 
            cartTitle.BackColor = Color.White;
            cartTitle.Dock = DockStyle.Fill;
            cartTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cartTitle.ForeColor = Color.FromArgb(31, 42, 60);
            cartTitle.Location = new Point(0, 4);
            cartTitle.Name = "cartTitle";
            cartTitle.Size = new Size(220, 32);
            cartTitle.TabIndex = 0;
            cartTitle.Text = "3. Transaction Cart";
            cartTitle.TextAlign = ContentAlignment.MiddleLeft;
            cartTitle.UseMnemonic = false;
            // 
            // clearCartButton
            // 
            clearCartButton.BackColor = Color.White;
            clearCartButton.Dock = DockStyle.Right;
            clearCartButton.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            clearCartButton.Glyph = "";
            clearCartButton.Kind = MyApp.Controls.ButtonKind.Danger;
            clearCartButton.Location = new Point(220, 4);
            clearCartButton.Name = "clearCartButton";
            clearCartButton.Size = new Size(100, 32);
            clearCartButton.TabIndex = 1;
            clearCartButton.Text = "Clear Cart";
            clearCartButton.Click += clearCartButton_Click;
            // 
            // cartColumns
            // 
            cartColumns.BackColor = Color.FromArgb(246, 248, 251);
            cartColumns.Controls.Add(cartColumnsGrid);
            cartColumns.Dock = DockStyle.Fill;
            cartColumns.Location = new Point(0, 40);
            cartColumns.Margin = new Padding(0);
            cartColumns.Name = "cartColumns";
            cartColumns.Size = new Size(320, 34);
            cartColumns.TabIndex = 1;
            // 
            // cartColumnsGrid
            // 
            cartColumnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            cartColumnsGrid.ColumnCount = 6;
            cartColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            cartColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            cartColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            cartColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            cartColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            cartColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
            cartColumnsGrid.Controls.Add(colCartItem, 0, 0);
            cartColumnsGrid.Controls.Add(colCartType, 1, 0);
            cartColumnsGrid.Controls.Add(colCartPrice, 2, 0);
            cartColumnsGrid.Controls.Add(colCartQty, 3, 0);
            cartColumnsGrid.Controls.Add(colCartSubtotal, 4, 0);
            cartColumnsGrid.Dock = DockStyle.Fill;
            cartColumnsGrid.Location = new Point(0, 0);
            cartColumnsGrid.Margin = new Padding(0);
            cartColumnsGrid.Name = "cartColumnsGrid";
            cartColumnsGrid.RowCount = 1;
            cartColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cartColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            cartColumnsGrid.Size = new Size(320, 34);
            cartColumnsGrid.TabIndex = 0;
            cartColumnsGrid.Paint += cartColumnsGrid_Paint;
            // 
            // colCartItem
            // 
            colCartItem.BackColor = Color.FromArgb(246, 248, 251);
            colCartItem.Dock = DockStyle.Fill;
            colCartItem.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colCartItem.ForeColor = Color.FromArgb(68, 83, 106);
            colCartItem.Location = new Point(0, 0);
            colCartItem.Margin = new Padding(0);
            colCartItem.Name = "colCartItem";
            colCartItem.Padding = new Padding(4, 0, 0, 0);
            colCartItem.Size = new Size(87, 34);
            colCartItem.TabIndex = 0;
            colCartItem.Text = "Item / Service";
            colCartItem.TextAlign = ContentAlignment.MiddleLeft;
            colCartItem.UseMnemonic = false;
            // 
            // colCartType
            // 
            colCartType.BackColor = Color.FromArgb(246, 248, 251);
            colCartType.Dock = DockStyle.Fill;
            colCartType.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colCartType.ForeColor = Color.FromArgb(68, 83, 106);
            colCartType.Location = new Point(87, 0);
            colCartType.Margin = new Padding(0);
            colCartType.Name = "colCartType";
            colCartType.Size = new Size(43, 34);
            colCartType.TabIndex = 1;
            colCartType.Text = "Type";
            colCartType.TextAlign = ContentAlignment.MiddleCenter;
            colCartType.UseMnemonic = false;
            // 
            // colCartPrice
            // 
            colCartPrice.BackColor = Color.FromArgb(246, 248, 251);
            colCartPrice.Dock = DockStyle.Fill;
            colCartPrice.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colCartPrice.ForeColor = Color.FromArgb(68, 83, 106);
            colCartPrice.Location = new Point(130, 0);
            colCartPrice.Margin = new Padding(0);
            colCartPrice.Name = "colCartPrice";
            colCartPrice.Size = new Size(40, 34);
            colCartPrice.TabIndex = 2;
            colCartPrice.Text = "Price";
            colCartPrice.TextAlign = ContentAlignment.MiddleCenter;
            colCartPrice.UseMnemonic = false;
            // 
            // colCartQty
            // 
            colCartQty.BackColor = Color.FromArgb(246, 248, 251);
            colCartQty.Dock = DockStyle.Fill;
            colCartQty.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colCartQty.ForeColor = Color.FromArgb(68, 83, 106);
            colCartQty.Location = new Point(170, 0);
            colCartQty.Margin = new Padding(0);
            colCartQty.Name = "colCartQty";
            colCartQty.Size = new Size(46, 34);
            colCartQty.TabIndex = 3;
            colCartQty.Text = "Qty";
            colCartQty.TextAlign = ContentAlignment.MiddleCenter;
            colCartQty.UseMnemonic = false;
            // 
            // colCartSubtotal
            // 
            colCartSubtotal.BackColor = Color.FromArgb(246, 248, 251);
            colCartSubtotal.Dock = DockStyle.Fill;
            colCartSubtotal.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colCartSubtotal.ForeColor = Color.FromArgb(68, 83, 106);
            colCartSubtotal.Location = new Point(216, 0);
            colCartSubtotal.Margin = new Padding(0);
            colCartSubtotal.Name = "colCartSubtotal";
            colCartSubtotal.Padding = new Padding(0, 0, 6, 0);
            colCartSubtotal.Size = new Size(73, 34);
            colCartSubtotal.TabIndex = 4;
            colCartSubtotal.Text = "Subtotal";
            colCartSubtotal.TextAlign = ContentAlignment.MiddleRight;
            colCartSubtotal.UseMnemonic = false;
            // 
            // cartRowsHost
            // 
            cartRowsHost.AutoScroll = true;
            cartRowsHost.BackColor = Color.White;
            cartRowsHost.Controls.Add(cartEmpty);
            cartRowsHost.Dock = DockStyle.Fill;
            cartRowsHost.Location = new Point(0, 74);
            cartRowsHost.Margin = new Padding(0);
            cartRowsHost.Name = "cartRowsHost";
            cartRowsHost.Size = new Size(320, 480);
            cartRowsHost.TabIndex = 2;
            // 
            // cartEmpty
            // 
            cartEmpty.BackColor = Color.White;
            cartEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            cartEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            cartEmpty.Dock = DockStyle.Fill;
            cartEmpty.Glyph = "";
            cartEmpty.Hint = "Add products or services from the list on the left.";
            cartEmpty.Location = new Point(0, 0);
            cartEmpty.Name = "cartEmpty";
            cartEmpty.Size = new Size(320, 480);
            cartEmpty.TabIndex = 0;
            cartEmpty.TabStop = false;
            cartEmpty.Title = "The cart is empty";
            // 
            // notesPanel
            // 
            notesPanel.BackColor = Color.White;
            notesPanel.Controls.Add(notesCard);
            notesPanel.Controls.Add(notesLabel);
            notesPanel.Dock = DockStyle.Fill;
            notesPanel.Location = new Point(0, 554);
            notesPanel.Margin = new Padding(0, 0, 0, 8);
            notesPanel.Name = "notesPanel";
            notesPanel.Size = new Size(320, 104);
            notesPanel.TabIndex = 3;
            // 
            // notesCard
            // 
            notesCard.BorderColor = Color.FromArgb(227, 233, 242);
            notesCard.Controls.Add(notesBox);
            notesCard.CornerColor = Color.White;
            notesCard.Dock = DockStyle.Fill;
            notesCard.FillColor = Color.White;
            notesCard.Location = new Point(0, 26);
            notesCard.Margin = new Padding(0);
            notesCard.Name = "notesCard";
            notesCard.Padding = new Padding(10, 8, 10, 8);
            notesCard.Radius = 8;
            notesCard.Size = new Size(320, 78);
            notesCard.TabIndex = 0;
            // 
            // notesBox
            // 
            notesBox.BackColor = Color.White;
            notesBox.BorderStyle = BorderStyle.None;
            notesBox.Dock = DockStyle.Fill;
            notesBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            notesBox.ForeColor = Color.FromArgb(31, 42, 60);
            notesBox.Hint = "Add notes here... (e.g., special instructions, concerns, etc.)";
            notesBox.Location = new Point(10, 8);
            notesBox.Multiline = true;
            notesBox.Name = "notesBox";
            notesBox.Size = new Size(300, 62);
            notesBox.TabIndex = 0;
            // 
            // notesLabel
            // 
            notesLabel.BackColor = Color.White;
            notesLabel.Dock = DockStyle.Top;
            notesLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            notesLabel.ForeColor = Color.FromArgb(68, 83, 106);
            notesLabel.Location = new Point(0, 0);
            notesLabel.Name = "notesLabel";
            notesLabel.Size = new Size(320, 26);
            notesLabel.TabIndex = 1;
            notesLabel.Text = "Customer Notes (Optional)";
            notesLabel.TextAlign = ContentAlignment.MiddleLeft;
            notesLabel.UseMnemonic = false;
            // 
            // summaryPanel
            // 
            summaryPanel.BackColor = Color.White;
            summaryPanel.Controls.Add(summaryGrid);
            summaryPanel.Dock = DockStyle.Fill;
            summaryPanel.EdgeColor = Color.FromArgb(227, 233, 242);
            summaryPanel.Location = new Point(0, 666);
            summaryPanel.Margin = new Padding(0);
            summaryPanel.Name = "summaryPanel";
            summaryPanel.Padding = new Padding(0, 8, 0, 0);
            summaryPanel.Side = MyApp.Controls.Edge.Top;
            summaryPanel.Size = new Size(320, 116);
            summaryPanel.TabIndex = 4;
            // 
            // summaryGrid
            // 
            summaryGrid.BackColor = Color.White;
            summaryGrid.ColumnCount = 4;
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 36F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 124F));
            summaryGrid.Controls.Add(subtotalLabel, 0, 0);
            summaryGrid.Controls.Add(subtotalValue, 3, 0);
            summaryGrid.Controls.Add(discountLabel, 0, 1);
            summaryGrid.Controls.Add(discountMode, 1, 1);
            summaryGrid.Controls.Add(discountCard, 2, 1);
            summaryGrid.Controls.Add(discountValue, 3, 1);
            summaryGrid.Controls.Add(totalLabel, 0, 2);
            summaryGrid.Controls.Add(totalValue, 3, 2);
            summaryGrid.Dock = DockStyle.Fill;
            summaryGrid.Location = new Point(0, 8);
            summaryGrid.Margin = new Padding(0);
            summaryGrid.Name = "summaryGrid";
            summaryGrid.RowCount = 3;
            summaryGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            summaryGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            summaryGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            summaryGrid.Size = new Size(320, 108);
            summaryGrid.TabIndex = 0;
            // 
            // subtotalLabel
            // 
            subtotalLabel.BackColor = Color.White;
            summaryGrid.SetColumnSpan(subtotalLabel, 3);
            subtotalLabel.Dock = DockStyle.Fill;
            subtotalLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            subtotalLabel.ForeColor = Color.FromArgb(122, 136, 156);
            subtotalLabel.Location = new Point(3, 0);
            subtotalLabel.Name = "subtotalLabel";
            subtotalLabel.Size = new Size(190, 24);
            subtotalLabel.TabIndex = 0;
            subtotalLabel.Text = "Subtotal";
            subtotalLabel.TextAlign = ContentAlignment.MiddleLeft;
            subtotalLabel.UseMnemonic = false;
            // 
            // subtotalValue
            // 
            subtotalValue.BackColor = Color.White;
            subtotalValue.Dock = DockStyle.Fill;
            subtotalValue.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            subtotalValue.ForeColor = Color.FromArgb(31, 42, 60);
            subtotalValue.Location = new Point(199, 0);
            subtotalValue.Name = "subtotalValue";
            subtotalValue.Size = new Size(118, 24);
            subtotalValue.TabIndex = 1;
            subtotalValue.TextAlign = ContentAlignment.MiddleRight;
            subtotalValue.UseMnemonic = false;
            // 
            // discountLabel
            // 
            discountLabel.BackColor = Color.White;
            discountLabel.Dock = DockStyle.Fill;
            discountLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            discountLabel.ForeColor = Color.FromArgb(122, 136, 156);
            discountLabel.Location = new Point(3, 24);
            discountLabel.Name = "discountLabel";
            discountLabel.Size = new Size(96, 34);
            discountLabel.TabIndex = 2;
            discountLabel.Text = "Discount";
            discountLabel.TextAlign = ContentAlignment.MiddleLeft;
            discountLabel.UseMnemonic = false;
            // 
            // discountMode
            // 
            discountMode.BackColor = Color.White;
            discountMode.Dock = DockStyle.Fill;
            discountMode.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            discountMode.Kind = MyApp.Controls.ButtonKind.Soft;
            discountMode.Location = new Point(102, 28);
            discountMode.Margin = new Padding(0, 4, 4, 4);
            discountMode.Name = "discountMode";
            discountMode.Size = new Size(32, 26);
            discountMode.TabIndex = 3;
            discountMode.Text = "%";
            discountMode.Click += discountMode_Click;
            // 
            // discountCard
            // 
            discountCard.BorderColor = Color.FromArgb(227, 233, 242);
            discountCard.Controls.Add(discountBox);
            discountCard.CornerColor = Color.White;
            discountCard.Dock = DockStyle.Fill;
            discountCard.FillColor = Color.White;
            discountCard.Location = new Point(138, 28);
            discountCard.Margin = new Padding(0, 4, 8, 4);
            discountCard.Name = "discountCard";
            discountCard.Padding = new Padding(6, 5, 6, 2);
            discountCard.Radius = 6;
            discountCard.Size = new Size(50, 26);
            discountCard.TabIndex = 4;
            // 
            // discountBox
            // 
            discountBox.BackColor = Color.White;
            discountBox.BorderStyle = BorderStyle.None;
            discountBox.Dock = DockStyle.Fill;
            discountBox.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            discountBox.ForeColor = Color.FromArgb(31, 42, 60);
            discountBox.Location = new Point(6, 5);
            discountBox.Name = "discountBox";
            discountBox.Size = new Size(38, 16);
            discountBox.TabIndex = 0;
            discountBox.Text = "0";
            discountBox.TextAlign = HorizontalAlignment.Center;
            discountBox.TextChanged += discountBox_TextChanged;
            // 
            // discountValue
            // 
            discountValue.BackColor = Color.White;
            discountValue.Dock = DockStyle.Fill;
            discountValue.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            discountValue.ForeColor = Color.FromArgb(31, 42, 60);
            discountValue.Location = new Point(199, 24);
            discountValue.Name = "discountValue";
            discountValue.Size = new Size(118, 34);
            discountValue.TabIndex = 5;
            discountValue.TextAlign = ContentAlignment.MiddleRight;
            discountValue.UseMnemonic = false;
            // 
            // totalLabel
            // 
            totalLabel.BackColor = Color.White;
            summaryGrid.SetColumnSpan(totalLabel, 3);
            totalLabel.Dock = DockStyle.Fill;
            totalLabel.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalLabel.ForeColor = Color.FromArgb(31, 42, 60);
            totalLabel.Location = new Point(3, 58);
            totalLabel.Name = "totalLabel";
            totalLabel.Size = new Size(190, 50);
            totalLabel.TabIndex = 6;
            totalLabel.Text = "Total Amount";
            totalLabel.TextAlign = ContentAlignment.MiddleLeft;
            totalLabel.UseMnemonic = false;
            // 
            // totalValue
            // 
            totalValue.BackColor = Color.White;
            totalValue.Dock = DockStyle.Fill;
            totalValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalValue.ForeColor = Color.FromArgb(31, 42, 60);
            totalValue.Location = new Point(199, 58);
            totalValue.Name = "totalValue";
            totalValue.Size = new Size(118, 50);
            totalValue.TabIndex = 7;
            totalValue.TextAlign = ContentAlignment.MiddleRight;
            totalValue.UseMnemonic = false;
            // 
            // paymentCard
            // 
            paymentCard.BorderColor = Color.FromArgb(227, 233, 242);
            paymentCard.Controls.Add(payGrid);
            paymentCard.CornerColor = Color.FromArgb(243, 247, 252);
            paymentCard.Dock = DockStyle.Fill;
            paymentCard.FillColor = Color.White;
            paymentCard.Location = new Point(664, 0);
            paymentCard.Margin = new Padding(0);
            paymentCard.Name = "paymentCard";
            paymentCard.Padding = new Padding(16, 14, 16, 14);
            paymentCard.Radius = 10;
            paymentCard.Size = new Size(296, 810);
            paymentCard.TabIndex = 2;
            // 
            // payGrid
            // 
            payGrid.BackColor = Color.White;
            payGrid.ColumnCount = 1;
            payGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            payGrid.Controls.Add(payTitle, 0, 0);
            payGrid.Controls.Add(paymentMethod, 0, 1);
            payGrid.Controls.Add(totalDueRow, 0, 2);
            payGrid.Controls.Add(receivedLabel, 0, 3);
            payGrid.Controls.Add(receivedCard, 0, 4);
            payGrid.Controls.Add(quickGrid, 0, 5);
            payGrid.Controls.Add(changeCard, 0, 6);
            payGrid.Controls.Add(printButton, 0, 7);
            payGrid.Controls.Add(receiptPreview, 0, 8);
            payGrid.Dock = DockStyle.Fill;
            payGrid.Location = new Point(16, 14);
            payGrid.Margin = new Padding(0);
            payGrid.Name = "payGrid";
            payGrid.RowCount = 9;
            payGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            payGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            payGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            payGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            payGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            payGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            payGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            payGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            payGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            payGrid.Size = new Size(264, 782);
            payGrid.TabIndex = 0;
            // 
            // payTitle
            // 
            payTitle.BackColor = Color.White;
            payTitle.Dock = DockStyle.Fill;
            payTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            payTitle.ForeColor = Color.FromArgb(31, 42, 60);
            payTitle.Location = new Point(3, 0);
            payTitle.Name = "payTitle";
            payTitle.Size = new Size(258, 34);
            payTitle.TabIndex = 0;
            payTitle.Text = "4. Payment Details";
            payTitle.TextAlign = ContentAlignment.MiddleLeft;
            payTitle.UseMnemonic = false;
            // 
            // paymentMethod
            // 
            paymentMethod.BackColor = Color.White;
            paymentMethod.Dock = DockStyle.Fill;
            paymentMethod.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            paymentMethod.Location = new Point(0, 34);
            paymentMethod.Margin = new Padding(0, 0, 0, 10);
            paymentMethod.Name = "paymentMethod";
            paymentMethod.Segments = new string[]
    {
    "Cash",
    "E-Wallet"
    };
            paymentMethod.Size = new Size(264, 36);
            paymentMethod.TabIndex = 1;
            paymentMethod.SelectedIndexChanged += paymentMethod_SelectedIndexChanged;
            // 
            // totalDueRow
            // 
            totalDueRow.BackColor = Color.White;
            totalDueRow.ColumnCount = 2;
            totalDueRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            totalDueRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            totalDueRow.Controls.Add(totalDueLabel, 0, 0);
            totalDueRow.Controls.Add(totalDueValue, 1, 0);
            totalDueRow.Dock = DockStyle.Fill;
            totalDueRow.Location = new Point(0, 80);
            totalDueRow.Margin = new Padding(0);
            totalDueRow.Name = "totalDueRow";
            totalDueRow.RowCount = 1;
            totalDueRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            totalDueRow.Size = new Size(264, 38);
            totalDueRow.TabIndex = 2;
            // 
            // totalDueLabel
            // 
            totalDueLabel.BackColor = Color.White;
            totalDueLabel.Dock = DockStyle.Fill;
            totalDueLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalDueLabel.ForeColor = Color.FromArgb(122, 136, 156);
            totalDueLabel.Location = new Point(3, 0);
            totalDueLabel.Name = "totalDueLabel";
            totalDueLabel.Size = new Size(126, 38);
            totalDueLabel.TabIndex = 0;
            totalDueLabel.Text = "Total Amount";
            totalDueLabel.TextAlign = ContentAlignment.MiddleLeft;
            totalDueLabel.UseMnemonic = false;
            totalDueLabel.Click += totalDueLabel_Click;
            // 
            // totalDueValue
            // 
            totalDueValue.BackColor = Color.White;
            totalDueValue.Dock = DockStyle.Fill;
            totalDueValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalDueValue.ForeColor = Color.FromArgb(31, 42, 60);
            totalDueValue.Location = new Point(135, 0);
            totalDueValue.Name = "totalDueValue";
            totalDueValue.Size = new Size(126, 38);
            totalDueValue.TabIndex = 1;
            totalDueValue.TextAlign = ContentAlignment.MiddleRight;
            totalDueValue.UseMnemonic = false;
            // 
            // receivedLabel
            // 
            receivedLabel.BackColor = Color.White;
            receivedLabel.Dock = DockStyle.Fill;
            receivedLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            receivedLabel.ForeColor = Color.FromArgb(31, 42, 60);
            receivedLabel.Location = new Point(3, 118);
            receivedLabel.Name = "receivedLabel";
            receivedLabel.Size = new Size(258, 24);
            receivedLabel.TabIndex = 3;
            receivedLabel.Text = "Amount Received";
            receivedLabel.TextAlign = ContentAlignment.MiddleLeft;
            receivedLabel.UseMnemonic = false;
            // 
            // receivedCard
            // 
            receivedCard.BorderColor = Color.FromArgb(227, 233, 242);
            receivedCard.Controls.Add(receivedHost);
            receivedCard.Controls.Add(pesoLabel);
            receivedCard.CornerColor = Color.White;
            receivedCard.Dock = DockStyle.Fill;
            receivedCard.FillColor = Color.White;
            receivedCard.Location = new Point(0, 142);
            receivedCard.Margin = new Padding(0, 0, 0, 8);
            receivedCard.Name = "receivedCard";
            receivedCard.Padding = new Padding(10, 3, 10, 3);
            receivedCard.Radius = 8;
            receivedCard.Size = new Size(264, 40);
            receivedCard.TabIndex = 4;
            // 
            // receivedHost
            // 
            receivedHost.BackColor = Color.White;
            receivedHost.Controls.Add(receivedBox);
            receivedHost.Dock = DockStyle.Fill;
            receivedHost.Location = new Point(34, 3);
            receivedHost.Name = "receivedHost";
            receivedHost.Padding = new Padding(0, 8, 0, 0);
            receivedHost.Size = new Size(220, 34);
            receivedHost.TabIndex = 0;
            // 
            // receivedBox
            // 
            receivedBox.BackColor = Color.White;
            receivedBox.BorderStyle = BorderStyle.None;
            receivedBox.Dock = DockStyle.Fill;
            receivedBox.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            receivedBox.ForeColor = Color.FromArgb(31, 42, 60);
            receivedBox.Location = new Point(0, 8);
            receivedBox.Name = "receivedBox";
            receivedBox.Size = new Size(220, 20);
            receivedBox.TabIndex = 0;
            receivedBox.TextChanged += receivedBox_TextChanged;
            // 
            // pesoLabel
            // 
            pesoLabel.BackColor = Color.White;
            pesoLabel.Dock = DockStyle.Left;
            pesoLabel.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pesoLabel.ForeColor = Color.FromArgb(58, 123, 213);
            pesoLabel.Location = new Point(10, 3);
            pesoLabel.Name = "pesoLabel";
            pesoLabel.Size = new Size(24, 34);
            pesoLabel.TabIndex = 1;
            pesoLabel.Text = "₱";
            pesoLabel.TextAlign = ContentAlignment.MiddleCenter;
            pesoLabel.UseMnemonic = false;
            // 
            // quickGrid
            // 
            quickGrid.BackColor = Color.White;
            quickGrid.ColumnCount = 4;
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            quickGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            quickGrid.Controls.Add(quick1Button, 0, 0);
            quickGrid.Controls.Add(quick2Button, 1, 0);
            quickGrid.Controls.Add(quick3Button, 2, 0);
            quickGrid.Controls.Add(quick4Button, 3, 0);
            quickGrid.Dock = DockStyle.Fill;
            quickGrid.Location = new Point(0, 190);
            quickGrid.Margin = new Padding(0, 0, 0, 8);
            quickGrid.Name = "quickGrid";
            quickGrid.RowCount = 1;
            quickGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            quickGrid.Size = new Size(264, 38);
            quickGrid.TabIndex = 5;
            // 
            // quick1Button
            // 
            quick1Button.BackColor = Color.White;
            quick1Button.Dock = DockStyle.Fill;
            quick1Button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quick1Button.Location = new Point(0, 0);
            quick1Button.Margin = new Padding(0, 0, 6, 0);
            quick1Button.Name = "quick1Button";
            quick1Button.Size = new Size(60, 38);
            quick1Button.TabIndex = 0;
            quick1Button.Click += quickAmount_Click;
            // 
            // quick2Button
            // 
            quick2Button.BackColor = Color.White;
            quick2Button.Dock = DockStyle.Fill;
            quick2Button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quick2Button.Location = new Point(66, 0);
            quick2Button.Margin = new Padding(0, 0, 6, 0);
            quick2Button.Name = "quick2Button";
            quick2Button.Size = new Size(60, 38);
            quick2Button.TabIndex = 1;
            quick2Button.Click += quickAmount_Click;
            // 
            // quick3Button
            // 
            quick3Button.BackColor = Color.White;
            quick3Button.Dock = DockStyle.Fill;
            quick3Button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quick3Button.Location = new Point(132, 0);
            quick3Button.Margin = new Padding(0, 0, 6, 0);
            quick3Button.Name = "quick3Button";
            quick3Button.Size = new Size(60, 38);
            quick3Button.TabIndex = 2;
            quick3Button.Click += quickAmount_Click;
            // 
            // quick4Button
            // 
            quick4Button.BackColor = Color.White;
            quick4Button.Dock = DockStyle.Fill;
            quick4Button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quick4Button.Location = new Point(198, 0);
            quick4Button.Margin = new Padding(0);
            quick4Button.Name = "quick4Button";
            quick4Button.Size = new Size(66, 38);
            quick4Button.TabIndex = 3;
            quick4Button.Click += quickAmount_Click;
            // 
            // changeCard
            // 
            changeCard.BorderColor = Color.FromArgb(191, 229, 206);
            changeCard.Controls.Add(changeValue);
            changeCard.Controls.Add(changeLabel);
            changeCard.CornerColor = Color.White;
            changeCard.Dock = DockStyle.Fill;
            changeCard.FillColor = Color.FromArgb(229, 246, 236);
            changeCard.Location = new Point(0, 236);
            changeCard.Margin = new Padding(0, 0, 0, 8);
            changeCard.Name = "changeCard";
            changeCard.Padding = new Padding(14, 0, 14, 0);
            changeCard.Radius = 8;
            changeCard.Size = new Size(264, 50);
            changeCard.TabIndex = 6;
            // 
            // changeValue
            // 
            changeValue.BackColor = Color.FromArgb(229, 246, 236);
            changeValue.Dock = DockStyle.Fill;
            changeValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            changeValue.ForeColor = Color.FromArgb(46, 158, 91);
            changeValue.Location = new Point(124, 0);
            changeValue.Name = "changeValue";
            changeValue.Size = new Size(126, 50);
            changeValue.TabIndex = 0;
            changeValue.TextAlign = ContentAlignment.MiddleRight;
            changeValue.UseMnemonic = false;
            // 
            // changeLabel
            // 
            changeLabel.BackColor = Color.FromArgb(229, 246, 236);
            changeLabel.Dock = DockStyle.Left;
            changeLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            changeLabel.ForeColor = Color.FromArgb(31, 42, 60);
            changeLabel.Location = new Point(14, 0);
            changeLabel.Name = "changeLabel";
            changeLabel.Size = new Size(110, 50);
            changeLabel.TabIndex = 1;
            changeLabel.Text = "Change";
            changeLabel.TextAlign = ContentAlignment.MiddleLeft;
            changeLabel.UseMnemonic = false;
            // 
            // printButton
            // 
            printButton.BackColor = Color.White;
            printButton.Dock = DockStyle.Fill;
            printButton.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            printButton.Location = new Point(0, 294);
            printButton.Margin = new Padding(0, 0, 0, 10);
            printButton.Name = "printButton";
            printButton.Size = new Size(264, 44);
            printButton.TabIndex = 7;
            printButton.Text = "Print Receipt";
            printButton.Click += printButton_Click;
            // 
            // receiptPreview
            // 
            receiptPreview.BackColor = Color.White;
            receiptPreview.Dock = DockStyle.Fill;
            receiptPreview.Location = new Point(3, 351);
            receiptPreview.Name = "receiptPreview";
            receiptPreview.Size = new Size(258, 428);
            receiptPreview.TabIndex = 8;
            receiptPreview.TabStop = false;
            // 
            // recentHost
            // 
            recentHost.BackColor = Color.FromArgb(243, 247, 252);
            recentHost.Controls.Add(recentCard);
            recentHost.Dock = DockStyle.Bottom;
            recentHost.Location = new Point(20, 890);
            recentHost.Name = "recentHost";
            recentHost.Padding = new Padding(0, 16, 0, 0);
            recentHost.Size = new Size(960, 266);
            recentHost.TabIndex = 1;
            // 
            // recentCard
            // 
            recentCard.BorderColor = Color.FromArgb(227, 233, 242);
            recentCard.Controls.Add(recentRowsHost);
            recentCard.Controls.Add(recentColumns);
            recentCard.Controls.Add(recentHeader);
            recentCard.CornerColor = Color.FromArgb(243, 247, 252);
            recentCard.Dock = DockStyle.Fill;
            recentCard.FillColor = Color.White;
            recentCard.Location = new Point(0, 16);
            recentCard.Margin = new Padding(0);
            recentCard.Name = "recentCard";
            recentCard.Padding = new Padding(16, 10, 16, 10);
            recentCard.Radius = 10;
            recentCard.Size = new Size(960, 250);
            recentCard.TabIndex = 0;
            // 
            // recentRowsHost
            // 
            recentRowsHost.AutoScroll = true;
            recentRowsHost.BackColor = Color.White;
            recentRowsHost.Controls.Add(recentEmpty);
            recentRowsHost.Dock = DockStyle.Fill;
            recentRowsHost.Location = new Point(16, 82);
            recentRowsHost.Name = "recentRowsHost";
            recentRowsHost.Size = new Size(928, 158);
            recentRowsHost.TabIndex = 0;
            // 
            // recentEmpty
            // 
            recentEmpty.BackColor = Color.White;
            recentEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            recentEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            recentEmpty.Dock = DockStyle.Fill;
            recentEmpty.Glyph = "";
            recentEmpty.Hint = "Completed sales will show up here.";
            recentEmpty.Location = new Point(0, 0);
            recentEmpty.Name = "recentEmpty";
            recentEmpty.Size = new Size(928, 158);
            recentEmpty.TabIndex = 0;
            recentEmpty.TabStop = false;
            recentEmpty.Title = "No transactions yet";
            // 
            // recentColumns
            // 
            recentColumns.BackColor = Color.FromArgb(246, 248, 251);
            recentColumns.Controls.Add(recentColumnsGrid);
            recentColumns.Dock = DockStyle.Top;
            recentColumns.Location = new Point(16, 48);
            recentColumns.Name = "recentColumns";
            recentColumns.Size = new Size(928, 34);
            recentColumns.TabIndex = 1;
            // 
            // recentColumnsGrid
            // 
            recentColumnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            recentColumnsGrid.ColumnCount = 8;
            recentColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            recentColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            recentColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            recentColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            recentColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
            recentColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
            recentColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
            recentColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7F));
            recentColumnsGrid.Controls.Add(colRecReceipt, 0, 0);
            recentColumnsGrid.Controls.Add(colRecDate, 1, 0);
            recentColumnsGrid.Controls.Add(colRecCustomer, 2, 0);
            recentColumnsGrid.Controls.Add(colRecItems, 3, 0);
            recentColumnsGrid.Controls.Add(colRecTotal, 4, 0);
            recentColumnsGrid.Controls.Add(colRecMethod, 5, 0);
            recentColumnsGrid.Controls.Add(colRecStatus, 6, 0);
            recentColumnsGrid.Controls.Add(colRecActions, 7, 0);
            recentColumnsGrid.Dock = DockStyle.Fill;
            recentColumnsGrid.Location = new Point(0, 0);
            recentColumnsGrid.Margin = new Padding(0);
            recentColumnsGrid.Name = "recentColumnsGrid";
            recentColumnsGrid.RowCount = 1;
            recentColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            recentColumnsGrid.Size = new Size(928, 34);
            recentColumnsGrid.TabIndex = 0;
            // 
            // colRecReceipt
            // 
            colRecReceipt.BackColor = Color.FromArgb(246, 248, 251);
            colRecReceipt.Dock = DockStyle.Fill;
            colRecReceipt.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colRecReceipt.ForeColor = Color.FromArgb(68, 83, 106);
            colRecReceipt.Location = new Point(0, 0);
            colRecReceipt.Margin = new Padding(0);
            colRecReceipt.Name = "colRecReceipt";
            colRecReceipt.Padding = new Padding(4, 0, 0, 0);
            colRecReceipt.Size = new Size(139, 34);
            colRecReceipt.TabIndex = 0;
            colRecReceipt.Text = "Receipt No.";
            colRecReceipt.TextAlign = ContentAlignment.MiddleLeft;
            colRecReceipt.UseMnemonic = false;
            // 
            // colRecDate
            // 
            colRecDate.BackColor = Color.FromArgb(246, 248, 251);
            colRecDate.Dock = DockStyle.Fill;
            colRecDate.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colRecDate.ForeColor = Color.FromArgb(68, 83, 106);
            colRecDate.Location = new Point(139, 0);
            colRecDate.Margin = new Padding(0);
            colRecDate.Name = "colRecDate";
            colRecDate.Padding = new Padding(4, 0, 0, 0);
            colRecDate.Size = new Size(157, 34);
            colRecDate.TabIndex = 1;
            colRecDate.Text = "Date & Time";
            colRecDate.TextAlign = ContentAlignment.MiddleLeft;
            colRecDate.UseMnemonic = false;
            // 
            // colRecCustomer
            // 
            colRecCustomer.BackColor = Color.FromArgb(246, 248, 251);
            colRecCustomer.Dock = DockStyle.Fill;
            colRecCustomer.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colRecCustomer.ForeColor = Color.FromArgb(68, 83, 106);
            colRecCustomer.Location = new Point(296, 0);
            colRecCustomer.Margin = new Padding(0);
            colRecCustomer.Name = "colRecCustomer";
            colRecCustomer.Padding = new Padding(4, 0, 0, 0);
            colRecCustomer.Size = new Size(129, 34);
            colRecCustomer.TabIndex = 2;
            colRecCustomer.Text = "Customer";
            colRecCustomer.TextAlign = ContentAlignment.MiddleLeft;
            colRecCustomer.UseMnemonic = false;
            // 
            // colRecItems
            // 
            colRecItems.BackColor = Color.FromArgb(246, 248, 251);
            colRecItems.Dock = DockStyle.Fill;
            colRecItems.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colRecItems.ForeColor = Color.FromArgb(68, 83, 106);
            colRecItems.Location = new Point(425, 0);
            colRecItems.Margin = new Padding(0);
            colRecItems.Name = "colRecItems";
            colRecItems.Padding = new Padding(4, 0, 0, 0);
            colRecItems.Size = new Size(148, 34);
            colRecItems.TabIndex = 3;
            colRecItems.Text = "Items / Services";
            colRecItems.TextAlign = ContentAlignment.MiddleLeft;
            colRecItems.UseMnemonic = false;
            // 
            // colRecTotal
            // 
            colRecTotal.BackColor = Color.FromArgb(246, 248, 251);
            colRecTotal.Dock = DockStyle.Fill;
            colRecTotal.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colRecTotal.ForeColor = Color.FromArgb(68, 83, 106);
            colRecTotal.Location = new Point(573, 0);
            colRecTotal.Margin = new Padding(0);
            colRecTotal.Name = "colRecTotal";
            colRecTotal.Padding = new Padding(4, 0, 0, 0);
            colRecTotal.Size = new Size(102, 34);
            colRecTotal.TabIndex = 4;
            colRecTotal.Text = "Total Amount";
            colRecTotal.TextAlign = ContentAlignment.MiddleLeft;
            colRecTotal.UseMnemonic = false;
            // 
            // colRecMethod
            // 
            colRecMethod.BackColor = Color.FromArgb(246, 248, 251);
            colRecMethod.Dock = DockStyle.Fill;
            colRecMethod.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colRecMethod.ForeColor = Color.FromArgb(68, 83, 106);
            colRecMethod.Location = new Point(675, 0);
            colRecMethod.Margin = new Padding(0);
            colRecMethod.Name = "colRecMethod";
            colRecMethod.Size = new Size(102, 34);
            colRecMethod.TabIndex = 5;
            colRecMethod.Text = "Payment Method";
            colRecMethod.TextAlign = ContentAlignment.MiddleCenter;
            colRecMethod.UseMnemonic = false;
            // 
            // colRecStatus
            // 
            colRecStatus.BackColor = Color.FromArgb(246, 248, 251);
            colRecStatus.Dock = DockStyle.Fill;
            colRecStatus.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colRecStatus.ForeColor = Color.FromArgb(68, 83, 106);
            colRecStatus.Location = new Point(777, 0);
            colRecStatus.Margin = new Padding(0);
            colRecStatus.Name = "colRecStatus";
            colRecStatus.Size = new Size(83, 34);
            colRecStatus.TabIndex = 6;
            colRecStatus.Text = "Status";
            colRecStatus.TextAlign = ContentAlignment.MiddleCenter;
            colRecStatus.UseMnemonic = false;
            // 
            // colRecActions
            // 
            colRecActions.BackColor = Color.FromArgb(246, 248, 251);
            colRecActions.Dock = DockStyle.Fill;
            colRecActions.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colRecActions.ForeColor = Color.FromArgb(68, 83, 106);
            colRecActions.Location = new Point(860, 0);
            colRecActions.Margin = new Padding(0);
            colRecActions.Name = "colRecActions";
            colRecActions.Size = new Size(68, 34);
            colRecActions.TabIndex = 7;
            colRecActions.Text = "Actions";
            colRecActions.TextAlign = ContentAlignment.MiddleCenter;
            colRecActions.UseMnemonic = false;
            // 
            // recentHeader
            // 
            recentHeader.BackColor = Color.White;
            recentHeader.Controls.Add(recentViewAll);
            recentHeader.Controls.Add(recentTitle);
            recentHeader.Controls.Add(recentIcon);
            recentHeader.Dock = DockStyle.Top;
            recentHeader.Location = new Point(16, 10);
            recentHeader.Name = "recentHeader";
            recentHeader.Size = new Size(928, 38);
            recentHeader.TabIndex = 2;
            // 
            // recentViewAll
            // 
            recentViewAll.BackColor = Color.White;
            recentViewAll.Cursor = Cursors.Hand;
            recentViewAll.Dock = DockStyle.Right;
            recentViewAll.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recentViewAll.ForeColor = Color.FromArgb(58, 123, 213);
            recentViewAll.Location = new Point(838, 0);
            recentViewAll.Name = "recentViewAll";
            recentViewAll.Size = new Size(90, 38);
            recentViewAll.TabIndex = 0;
            recentViewAll.Text = "View All →";
            recentViewAll.TextAlign = ContentAlignment.MiddleRight;
            recentViewAll.UseMnemonic = false;
            recentViewAll.Click += recentViewAll_Click;
            // 
            // recentTitle
            // 
            recentTitle.BackColor = Color.White;
            recentTitle.Dock = DockStyle.Left;
            recentTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recentTitle.ForeColor = Color.FromArgb(31, 42, 60);
            recentTitle.Location = new Point(30, 0);
            recentTitle.Name = "recentTitle";
            recentTitle.Size = new Size(200, 38);
            recentTitle.TabIndex = 1;
            recentTitle.Text = "Recent Transactions";
            recentTitle.TextAlign = ContentAlignment.MiddleLeft;
            recentTitle.UseMnemonic = false;
            // 
            // recentIcon
            // 
            recentIcon.BackColor = Color.White;
            recentIcon.Dock = DockStyle.Left;
            recentIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            recentIcon.ForeColor = Color.FromArgb(58, 123, 213);
            recentIcon.Location = new Point(0, 0);
            recentIcon.Name = "recentIcon";
            recentIcon.Size = new Size(30, 38);
            recentIcon.TabIndex = 2;
            recentIcon.Text = "";
            recentIcon.TextAlign = ContentAlignment.MiddleCenter;
            recentIcon.UseMnemonic = false;
            // 
            // actionsBar
            // 
            actionsBar.BackColor = Color.FromArgb(243, 247, 252);
            actionsBar.Controls.Add(actionsFlow);
            actionsBar.Controls.Add(dateTimePanel);
            actionsBar.Dock = DockStyle.Top;
            actionsBar.Location = new Point(20, 20);
            actionsBar.Name = "actionsBar";
            actionsBar.Size = new Size(960, 60);
            actionsBar.TabIndex = 2;
            // 
            // actionsFlow
            // 
            actionsFlow.BackColor = Color.FromArgb(243, 247, 252);
            actionsFlow.Controls.Add(newTransactionButton);
            actionsFlow.Controls.Add(ongoingButton);
            actionsFlow.Controls.Add(historyButton);
            actionsFlow.Dock = DockStyle.Fill;
            actionsFlow.Location = new Point(0, 0);
            actionsFlow.Name = "actionsFlow";
            actionsFlow.Padding = new Padding(0, 10, 0, 0);
            actionsFlow.Size = new Size(728, 60);
            actionsFlow.TabIndex = 0;
            actionsFlow.WrapContents = false;
            // 
            // newTransactionButton
            // 
            newTransactionButton.BackColor = Color.FromArgb(243, 247, 252);
            newTransactionButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            newTransactionButton.Glyph = "";
            newTransactionButton.Kind = MyApp.Controls.ButtonKind.Primary;
            newTransactionButton.Location = new Point(0, 10);
            newTransactionButton.Margin = new Padding(0, 0, 10, 0);
            newTransactionButton.Name = "newTransactionButton";
            newTransactionButton.Size = new Size(150, 38);
            newTransactionButton.TabIndex = 0;
            newTransactionButton.Text = "New Transaction";
            newTransactionButton.Click += newTransactionButton_Click;
            // 
            // ongoingButton
            // 
            ongoingButton.BackColor = Color.FromArgb(243, 247, 252);
            ongoingButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ongoingButton.Glyph = "";
            ongoingButton.Location = new Point(160, 10);
            ongoingButton.Margin = new Padding(0, 0, 10, 0);
            ongoingButton.Name = "ongoingButton";
            ongoingButton.Size = new Size(180, 38);
            ongoingButton.TabIndex = 1;
            ongoingButton.Text = "Ongoing Appointments";
            ongoingButton.Click += ongoingButton_Click;
            // 
            // historyButton
            // 
            historyButton.BackColor = Color.FromArgb(243, 247, 252);
            historyButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            historyButton.Glyph = "";
            historyButton.Location = new Point(350, 10);
            historyButton.Margin = new Padding(0, 0, 10, 0);
            historyButton.Name = "historyButton";
            historyButton.Size = new Size(160, 38);
            historyButton.TabIndex = 2;
            historyButton.Text = "Transaction History";
            historyButton.Click += historyButton_Click;
            // 
            // dateTimePanel
            // 
            dateTimePanel.BackColor = Color.FromArgb(243, 247, 252);
            dateTimePanel.Controls.Add(dateIcon);
            dateTimePanel.Controls.Add(dateLabel);
            dateTimePanel.Controls.Add(timeIcon);
            dateTimePanel.Controls.Add(timeLabel);
            dateTimePanel.Dock = DockStyle.Right;
            dateTimePanel.Location = new Point(728, 0);
            dateTimePanel.Name = "dateTimePanel";
            dateTimePanel.Size = new Size(232, 60);
            dateTimePanel.TabIndex = 1;
            // 
            // dateIcon
            // 
            dateIcon.BackColor = Color.FromArgb(243, 247, 252);
            dateIcon.Font = new Font("Segoe MDL2 Assets", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateIcon.ForeColor = Color.FromArgb(122, 136, 156);
            dateIcon.Location = new Point(0, 18);
            dateIcon.Name = "dateIcon";
            dateIcon.Size = new Size(20, 24);
            dateIcon.TabIndex = 0;
            dateIcon.Text = "";
            dateIcon.TextAlign = ContentAlignment.MiddleCenter;
            dateIcon.UseMnemonic = false;
            // 
            // dateLabel
            // 
            dateLabel.BackColor = Color.FromArgb(243, 247, 252);
            dateLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dateLabel.ForeColor = Color.FromArgb(68, 83, 106);
            dateLabel.Location = new Point(22, 18);
            dateLabel.Name = "dateLabel";
            dateLabel.Size = new Size(100, 24);
            dateLabel.TabIndex = 1;
            dateLabel.TextAlign = ContentAlignment.MiddleLeft;
            dateLabel.UseMnemonic = false;
            // 
            // timeIcon
            // 
            timeIcon.BackColor = Color.FromArgb(243, 247, 252);
            timeIcon.Font = new Font("Segoe MDL2 Assets", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            timeIcon.ForeColor = Color.FromArgb(122, 136, 156);
            timeIcon.Location = new Point(128, 18);
            timeIcon.Name = "timeIcon";
            timeIcon.Size = new Size(20, 24);
            timeIcon.TabIndex = 2;
            timeIcon.Text = "";
            timeIcon.TextAlign = ContentAlignment.MiddleCenter;
            timeIcon.UseMnemonic = false;
            // 
            // timeLabel
            // 
            timeLabel.BackColor = Color.FromArgb(243, 247, 252);
            timeLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            timeLabel.ForeColor = Color.FromArgb(68, 83, 106);
            timeLabel.Location = new Point(150, 18);
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new Size(76, 24);
            timeLabel.TabIndex = 3;
            timeLabel.TextAlign = ContentAlignment.MiddleLeft;
            timeLabel.UseMnemonic = false;
            // 
            // dateTimer
            // 
            dateTimer.Interval = 15000;
            dateTimer.Tick += dateTimer_Tick;
            // 
            // BillingPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScroll = true;
            AutoScrollMinSize = new Size(960, 910);
            BackColor = Color.FromArgb(243, 247, 252);
            Controls.Add(mainGrid);
            Controls.Add(recentHost);
            Controls.Add(actionsBar);
            Name = "BillingPage";
            Padding = new Padding(20);
            Size = new Size(951, 715);
            mainGrid.ResumeLayout(false);
            leftColumn.ResumeLayout(false);
            select1Card.ResumeLayout(false);
            select1Grid.ResumeLayout(false);
            customerInfoHost.ResumeLayout(false);
            itemsCard.ResumeLayout(false);
            itemsGrid.ResumeLayout(false);
            itemsGrid.PerformLayout();
            catalogSearchRow.ResumeLayout(false);
            chipsFlow.ResumeLayout(false);
            catalogList.ResumeLayout(false);
            cartCard.ResumeLayout(false);
            cartGrid.ResumeLayout(false);
            cartHeader.ResumeLayout(false);
            cartColumns.ResumeLayout(false);
            cartColumnsGrid.ResumeLayout(false);
            cartRowsHost.ResumeLayout(false);
            notesPanel.ResumeLayout(false);
            notesCard.ResumeLayout(false);
            notesCard.PerformLayout();
            summaryPanel.ResumeLayout(false);
            summaryGrid.ResumeLayout(false);
            discountCard.ResumeLayout(false);
            discountCard.PerformLayout();
            paymentCard.ResumeLayout(false);
            payGrid.ResumeLayout(false);
            totalDueRow.ResumeLayout(false);
            receivedCard.ResumeLayout(false);
            receivedHost.ResumeLayout(false);
            receivedHost.PerformLayout();
            quickGrid.ResumeLayout(false);
            changeCard.ResumeLayout(false);
            recentHost.ResumeLayout(false);
            recentCard.ResumeLayout(false);
            recentRowsHost.ResumeLayout(false);
            recentColumns.ResumeLayout(false);
            recentColumnsGrid.ResumeLayout(false);
            recentHeader.ResumeLayout(false);
            actionsBar.ResumeLayout(false);
            actionsFlow.ResumeLayout(false);
            dateTimePanel.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainGrid;
        private System.Windows.Forms.TableLayoutPanel leftColumn;
        private MyApp.Controls.CardPanel select1Card;
        private System.Windows.Forms.TableLayoutPanel select1Grid;
        private System.Windows.Forms.Label select1Title;
        private MyApp.Controls.SegmentedControl customerMode;
        private MyApp.Controls.InputField customerSearch;
        private System.Windows.Forms.Panel customerInfoHost;
        private MyApp.Controls.EmptyState customerEmpty;
        private System.Windows.Forms.Panel customerResults;
        private MyApp.Controls.CustomerCardView customerCard;
        private MyApp.Controls.CardPanel itemsCard;
        private System.Windows.Forms.TableLayoutPanel itemsGrid;
        private System.Windows.Forms.Label itemsTitle;
        private MyApp.Controls.SegmentedControl catalogTabs;
        private System.Windows.Forms.TableLayoutPanel catalogSearchRow;
        private MyApp.Controls.InputField catalogSearch;
        private MyApp.Controls.OutlineButton resetFiltersButton;
        private System.Windows.Forms.FlowLayoutPanel chipsFlow;
        private MyApp.Controls.TabPill allChip;
        private System.Windows.Forms.Panel catalogList;
        private MyApp.Controls.EmptyState catalogEmpty;
        private MyApp.Controls.CardPanel cartCard;
        private System.Windows.Forms.TableLayoutPanel cartGrid;
        private System.Windows.Forms.Panel cartHeader;
        private System.Windows.Forms.Label cartTitle;
        private MyApp.Controls.OutlineButton clearCartButton;
        private System.Windows.Forms.Panel cartColumns;
        private System.Windows.Forms.TableLayoutPanel cartColumnsGrid;
        private System.Windows.Forms.Label colCartItem;
        private System.Windows.Forms.Label colCartType;
        private System.Windows.Forms.Label colCartPrice;
        private System.Windows.Forms.Label colCartQty;
        private System.Windows.Forms.Label colCartSubtotal;
        private System.Windows.Forms.Panel cartRowsHost;
        private MyApp.Controls.EmptyState cartEmpty;
        private System.Windows.Forms.Panel notesPanel;
        private MyApp.Controls.CardPanel notesCard;
        private MyApp.Controls.HintTextBox notesBox;
        private System.Windows.Forms.Label notesLabel;
        private MyApp.Controls.EdgePanel summaryPanel;
        private System.Windows.Forms.TableLayoutPanel summaryGrid;
        private System.Windows.Forms.Label subtotalLabel;
        private System.Windows.Forms.Label subtotalValue;
        private System.Windows.Forms.Label discountLabel;
        private MyApp.Controls.OutlineButton discountMode;
        private MyApp.Controls.CardPanel discountCard;
        private System.Windows.Forms.TextBox discountBox;
        private System.Windows.Forms.Label discountValue;
        private System.Windows.Forms.Label totalLabel;
        private System.Windows.Forms.Label totalValue;
        private MyApp.Controls.CardPanel paymentCard;
        private System.Windows.Forms.TableLayoutPanel payGrid;
        private System.Windows.Forms.Label payTitle;
        private MyApp.Controls.SegmentedControl paymentMethod;
        private System.Windows.Forms.TableLayoutPanel totalDueRow;
        private System.Windows.Forms.Label totalDueLabel;
        private System.Windows.Forms.Label totalDueValue;
        private System.Windows.Forms.Label receivedLabel;
        private MyApp.Controls.CardPanel receivedCard;
        private System.Windows.Forms.Panel receivedHost;
        private System.Windows.Forms.TextBox receivedBox;
        private System.Windows.Forms.Label pesoLabel;
        private System.Windows.Forms.TableLayoutPanel quickGrid;
        private MyApp.Controls.OutlineButton quick1Button;
        private MyApp.Controls.OutlineButton quick2Button;
        private MyApp.Controls.OutlineButton quick3Button;
        private MyApp.Controls.OutlineButton quick4Button;
        private MyApp.Controls.CardPanel changeCard;
        private System.Windows.Forms.Label changeValue;
        private System.Windows.Forms.Label changeLabel;
        private MyApp.Controls.PrimaryButton printButton;
        private MyApp.Controls.ReceiptPreview receiptPreview;
        private System.Windows.Forms.Panel recentHost;
        private MyApp.Controls.CardPanel recentCard;
        private System.Windows.Forms.Panel recentRowsHost;
        private MyApp.Controls.EmptyState recentEmpty;
        private System.Windows.Forms.Panel recentColumns;
        private System.Windows.Forms.TableLayoutPanel recentColumnsGrid;
        private System.Windows.Forms.Label colRecReceipt;
        private System.Windows.Forms.Label colRecDate;
        private System.Windows.Forms.Label colRecCustomer;
        private System.Windows.Forms.Label colRecItems;
        private System.Windows.Forms.Label colRecTotal;
        private System.Windows.Forms.Label colRecMethod;
        private System.Windows.Forms.Label colRecStatus;
        private System.Windows.Forms.Label colRecActions;
        private System.Windows.Forms.Panel recentHeader;
        private System.Windows.Forms.Label recentViewAll;
        private System.Windows.Forms.Label recentTitle;
        private System.Windows.Forms.Label recentIcon;
        private System.Windows.Forms.Panel actionsBar;
        private System.Windows.Forms.FlowLayoutPanel actionsFlow;
        private MyApp.Controls.OutlineButton newTransactionButton;
        private MyApp.Controls.OutlineButton ongoingButton;
        private MyApp.Controls.OutlineButton historyButton;
        private System.Windows.Forms.Panel dateTimePanel;
        private System.Windows.Forms.Label dateIcon;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label timeIcon;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Timer dateTimer;
    }
}
