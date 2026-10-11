#nullable disable
namespace MyApp
{
    partial class LookupPage
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
            rootGrid = new TableLayoutPanel();
            tabsRow = new Panel();
            customerTab = new MyApp.Controls.TabPill();
            petTab = new MyApp.Controls.TabPill();
            filterCard = new MyApp.Controls.CardPanel();
            filterGrid = new TableLayoutPanel();
            searchField = new MyApp.Controls.InputField();
            customerFilter = new MyApp.Controls.SelectField();
            petFilter = new MyApp.Controls.SelectField();
            sortFilter = new MyApp.Controls.SelectField();
            searchButton = new MyApp.Controls.PrimaryButton();
            bodyGrid = new TableLayoutPanel();
            customersCard = new MyApp.Controls.CardPanel();
            rowsHost = new Panel();
            customersEmpty = new MyApp.Controls.EmptyState();
            tableFooter = new Panel();
            showingLabel = new Label();
            pager = new Panel();
            nextPageButton = new MyApp.Controls.IconButton();
            pageChip = new MyApp.Controls.PrimaryButton();
            prevPageButton = new MyApp.Controls.IconButton();
            columnsHead = new Panel();
            columnsGrid = new TableLayoutPanel();
            headerCheck = new MyApp.Controls.CheckMark();
            colCustomer = new Label();
            colPets = new Label();
            colPhone = new Label();
            colEmail = new Label();
            colLastVisit = new Label();
            colActions = new Label();
            customersHeader = new Panel();
            customersTitle = new Label();
            customersIcon = new Label();
            rightColumn = new TableLayoutPanel();
            quickFiltersCard = new MyApp.Controls.CardPanel();
            quickFiltersList = new Panel();
            filterNewRow = new Panel();
            filterNewName = new Label();
            filterNewCount = new Label();
            filterNewIcon = new MyApp.Controls.IconBadge();
            filterBronzeRow = new Panel();
            filterBronzeName = new Label();
            filterBronzeCount = new Label();
            filterBronzeIcon = new MyApp.Controls.IconBadge();
            filterSilverRow = new Panel();
            filterSilverName = new Label();
            filterSilverCount = new Label();
            filterSilverIcon = new MyApp.Controls.IconBadge();
            filterGoldRow = new Panel();
            filterGoldName = new Label();
            filterGoldCount = new Label();
            filterGoldIcon = new MyApp.Controls.IconBadge();
            filterAllRow = new Panel();
            filterAllName = new Label();
            filterAllCount = new Label();
            filterAllIcon = new MyApp.Controls.IconBadge();
            quickFiltersHeader = new Panel();
            quickFiltersLink = new Label();
            quickFiltersTitle = new Label();
            quickFiltersIcon = new Label();
            recentCard = new MyApp.Controls.CardPanel();
            recentList = new Panel();
            recentEmpty = new MyApp.Controls.EmptyState();
            recentHeader = new Panel();
            recentLink = new Label();
            recentTitle = new Label();
            recentIcon = new Label();
            exportCard = new MyApp.Controls.CardPanel();
            exportGrid = new TableLayoutPanel();
            exportCustomersTile = new MyApp.Controls.ActionTile();
            exportPetsTile = new MyApp.Controls.ActionTile();
            exportHeader = new Panel();
            exportTitle = new Label();
            exportIcon = new Label();
            rootGrid.SuspendLayout();
            tabsRow.SuspendLayout();
            filterCard.SuspendLayout();
            filterGrid.SuspendLayout();
            bodyGrid.SuspendLayout();
            customersCard.SuspendLayout();
            rowsHost.SuspendLayout();
            tableFooter.SuspendLayout();
            pager.SuspendLayout();
            columnsHead.SuspendLayout();
            columnsGrid.SuspendLayout();
            customersHeader.SuspendLayout();
            rightColumn.SuspendLayout();
            quickFiltersCard.SuspendLayout();
            quickFiltersList.SuspendLayout();
            filterNewRow.SuspendLayout();
            filterBronzeRow.SuspendLayout();
            filterSilverRow.SuspendLayout();
            filterGoldRow.SuspendLayout();
            filterAllRow.SuspendLayout();
            quickFiltersHeader.SuspendLayout();
            recentCard.SuspendLayout();
            recentList.SuspendLayout();
            recentHeader.SuspendLayout();
            exportCard.SuspendLayout();
            exportGrid.SuspendLayout();
            exportHeader.SuspendLayout();
            SuspendLayout();
            // 
            // rootGrid
            // 
            rootGrid.BackColor = Color.FromArgb(243, 247, 252);
            rootGrid.ColumnCount = 1;
            rootGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootGrid.Controls.Add(tabsRow, 0, 0);
            rootGrid.Controls.Add(filterCard, 0, 1);
            rootGrid.Controls.Add(bodyGrid, 0, 2);
            rootGrid.Dock = DockStyle.Fill;
            rootGrid.Location = new Point(0, 0);
            rootGrid.Margin = new Padding(0, 0, 0, 0);
            rootGrid.Name = "rootGrid";
            rootGrid.RowCount = 3;
            rootGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            rootGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            rootGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootGrid.Size = new Size(996, 692);
            rootGrid.TabIndex = 0;
            // 
            // tabsRow
            // 
            tabsRow.Controls.Add(customerTab);
            tabsRow.Controls.Add(petTab);
            tabsRow.BackColor = Color.FromArgb(243, 247, 252);
            tabsRow.Dock = DockStyle.Fill;
            tabsRow.Location = new Point(0, 0);
            tabsRow.Margin = new Padding(0, 0, 0, 0);
            tabsRow.Name = "tabsRow";
            tabsRow.Size = new Size(996, 44);
            tabsRow.TabIndex = 0;
            // 
            // customerTab
            // 
            customerTab.Active = true;
            customerTab.BackColor = Color.FromArgb(243, 247, 252);
            customerTab.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            customerTab.Location = new Point(0, 2);
            customerTab.Name = "customerTab";
            customerTab.Size = new Size(150, 36);
            customerTab.TabIndex = 0;
            customerTab.Text = "Customer Lookup";
            customerTab.Click += customerTab_Click;
            // 
            // petTab
            // 
            petTab.BackColor = Color.FromArgb(243, 247, 252);
            petTab.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            petTab.Location = new Point(158, 2);
            petTab.Name = "petTab";
            petTab.Size = new Size(120, 36);
            petTab.TabIndex = 1;
            petTab.Text = "Pet Lookup";
            petTab.Click += petTab_Click;
            // 
            // filterCard
            // 
            filterCard.Controls.Add(filterGrid);
            filterCard.BorderColor = Color.FromArgb(227, 233, 242);
            filterCard.CornerColor = Color.FromArgb(243, 247, 252);
            filterCard.Dock = DockStyle.Fill;
            filterCard.FillColor = Color.FromArgb(255, 255, 255);
            filterCard.Location = new Point(0, 0);
            filterCard.Margin = new Padding(0, 0, 0, 12);
            filterCard.Name = "filterCard";
            filterCard.Padding = new Padding(12, 10, 12, 10);
            filterCard.Radius = 10;
            filterCard.Size = new Size(996, 64);
            filterCard.TabIndex = 1;
            // 
            // filterGrid
            // 
            filterGrid.BackColor = Color.FromArgb(255, 255, 255);
            filterGrid.ColumnCount = 5;
            filterGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            filterGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            filterGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            filterGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            filterGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            filterGrid.Controls.Add(searchField, 0, 0);
            filterGrid.Controls.Add(customerFilter, 1, 0);
            filterGrid.Controls.Add(petFilter, 2, 0);
            filterGrid.Controls.Add(sortFilter, 3, 0);
            filterGrid.Controls.Add(searchButton, 4, 0);
            filterGrid.Dock = DockStyle.Fill;
            filterGrid.Location = new Point(0, 0);
            filterGrid.Margin = new Padding(0, 0, 0, 0);
            filterGrid.Name = "filterGrid";
            filterGrid.RowCount = 1;
            filterGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            filterGrid.Size = new Size(972, 44);
            filterGrid.TabIndex = 0;
            // 
            // searchField
            // 
            searchField.BorderColor = Color.FromArgb(227, 233, 242);
            searchField.CornerColor = Color.FromArgb(255, 255, 255);
            searchField.Dock = DockStyle.Fill;
            searchField.FillColor = Color.FromArgb(255, 255, 255);
            searchField.Glyph = "\uE721";
            searchField.Location = new Point(0, 0);
            searchField.Margin = new Padding(0, 0, 8, 0);
            searchField.Name = "searchField";
            searchField.Padding = new Padding(10, 3, 10, 3);
            searchField.Placeholder = "Search by name, phone number, or email...";
            searchField.Radius = 8;
            searchField.Size = new Size(448, 44);
            searchField.TabIndex = 0;
            // 
            // customerFilter
            // 
            customerFilter.BackColor = Color.FromArgb(255, 255, 255);
            customerFilter.Dock = DockStyle.Fill;
            customerFilter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customerFilter.Location = new Point(456, 0);
            customerFilter.Margin = new Padding(0, 0, 8, 0);
            customerFilter.Name = "customerFilter";
            customerFilter.Size = new Size(142, 44);
            customerFilter.TabIndex = 1;
            customerFilter.Text = "All Customers";
            // 
            // petFilter
            // 
            petFilter.BackColor = Color.FromArgb(255, 255, 255);
            petFilter.Dock = DockStyle.Fill;
            petFilter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            petFilter.Location = new Point(606, 0);
            petFilter.Margin = new Padding(0, 0, 8, 0);
            petFilter.Name = "petFilter";
            petFilter.Size = new Size(102, 44);
            petFilter.TabIndex = 2;
            petFilter.Text = "All Pets";
            // 
            // sortFilter
            // 
            sortFilter.BackColor = Color.FromArgb(255, 255, 255);
            sortFilter.Dock = DockStyle.Fill;
            sortFilter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            sortFilter.Location = new Point(716, 0);
            sortFilter.Margin = new Padding(0, 0, 8, 0);
            sortFilter.Name = "sortFilter";
            sortFilter.Size = new Size(152, 44);
            sortFilter.TabIndex = 3;
            sortFilter.Text = "Sort by: Newest";
            // 
            // searchButton
            // 
            searchButton.BackColor = Color.FromArgb(255, 255, 255);
            searchButton.Dock = DockStyle.Fill;
            searchButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            searchButton.Location = new Point(876, 0);
            searchButton.Margin = new Padding(0);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(96, 44);
            searchButton.TabIndex = 4;
            searchButton.Text = "Search";
            searchButton.Click += searchButton_Click;
            // 
            // bodyGrid
            // 
            bodyGrid.BackColor = Color.FromArgb(243, 247, 252);
            bodyGrid.ColumnCount = 2;
            bodyGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bodyGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            bodyGrid.Controls.Add(customersCard, 0, 0);
            bodyGrid.Controls.Add(rightColumn, 1, 0);
            bodyGrid.Dock = DockStyle.Fill;
            bodyGrid.Location = new Point(0, 0);
            bodyGrid.Margin = new Padding(0, 0, 0, 0);
            bodyGrid.Name = "bodyGrid";
            bodyGrid.RowCount = 1;
            bodyGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bodyGrid.Size = new Size(996, 572);
            bodyGrid.TabIndex = 2;
            // 
            // customersCard
            // 
            customersCard.Controls.Add(rowsHost);
            customersCard.Controls.Add(tableFooter);
            customersCard.Controls.Add(columnsHead);
            customersCard.Controls.Add(customersHeader);
            customersCard.BorderColor = Color.FromArgb(227, 233, 242);
            customersCard.CornerColor = Color.FromArgb(243, 247, 252);
            customersCard.Dock = DockStyle.Fill;
            customersCard.FillColor = Color.FromArgb(255, 255, 255);
            customersCard.Location = new Point(0, 0);
            customersCard.Margin = new Padding(0, 0, 16, 0);
            customersCard.Name = "customersCard";
            customersCard.Padding = new Padding(16, 12, 16, 12);
            customersCard.Radius = 10;
            customersCard.Size = new Size(660, 572);
            customersCard.TabIndex = 0;
            // 
            // rowsHost
            // 
            rowsHost.Controls.Add(customersEmpty);
            rowsHost.BackColor = Color.FromArgb(255, 255, 255);
            rowsHost.AutoScroll = true;
            rowsHost.Dock = DockStyle.Fill;
            rowsHost.Location = new Point(16, 90);
            rowsHost.Name = "rowsHost";
            rowsHost.Size = new Size(628, 430);
            rowsHost.TabIndex = 0;
            // 
            // customersEmpty
            // 
            customersEmpty.BackColor = Color.FromArgb(255, 255, 255);
            customersEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            customersEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            customersEmpty.Dock = DockStyle.Fill;
            customersEmpty.Glyph = "\uE716";
            customersEmpty.Hint = "Customers will show up here once they are added.";
            customersEmpty.Location = new Point(0, 0);
            customersEmpty.Name = "customersEmpty";
            customersEmpty.Size = new Size(628, 430);
            customersEmpty.TabIndex = 0;
            customersEmpty.TabStop = false;
            customersEmpty.Title = "No customers found";
            // 
            // tableFooter
            // 
            tableFooter.Controls.Add(showingLabel);
            tableFooter.Controls.Add(pager);
            tableFooter.BackColor = Color.FromArgb(255, 255, 255);
            tableFooter.Dock = DockStyle.Bottom;
            tableFooter.Location = new Point(16, 520);
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
            showingLabel.Text = "Showing 0 customers";
            showingLabel.TextAlign = ContentAlignment.MiddleLeft;
            showingLabel.UseMnemonic = false;
            // 
            // pager
            // 
            pager.Controls.Add(nextPageButton);
            pager.Controls.Add(pageChip);
            pager.Controls.Add(prevPageButton);
            pager.BackColor = Color.FromArgb(255, 255, 255);
            pager.Dock = DockStyle.Right;
            pager.Location = new Point(516, 0);
            pager.Name = "pager";
            pager.Size = new Size(112, 40);
            pager.TabIndex = 1;
            // 
            // nextPageButton
            // 
            nextPageButton.BackColor = Color.FromArgb(255, 255, 255);
            nextPageButton.Enabled = false;
            nextPageButton.Glyph = "\uE76C";
            nextPageButton.Location = new Point(78, 5);
            nextPageButton.Name = "nextPageButton";
            nextPageButton.Size = new Size(30, 30);
            nextPageButton.TabIndex = 0;
            // 
            // pageChip
            // 
            pageChip.BackColor = Color.FromArgb(255, 255, 255);
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
            prevPageButton.BackColor = Color.FromArgb(255, 255, 255);
            prevPageButton.Enabled = false;
            prevPageButton.Glyph = "\uE76B";
            prevPageButton.Location = new Point(4, 5);
            prevPageButton.Name = "prevPageButton";
            prevPageButton.Size = new Size(30, 30);
            prevPageButton.TabIndex = 2;
            // 
            // columnsHead
            // 
            columnsHead.Controls.Add(columnsGrid);
            columnsHead.BackColor = Color.FromArgb(246, 248, 251);
            columnsHead.Dock = DockStyle.Top;
            columnsHead.Location = new Point(16, 56);
            columnsHead.Name = "columnsHead";
            columnsHead.Size = new Size(628, 34);
            columnsHead.TabIndex = 2;
            // 
            // columnsGrid
            // 
            columnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            columnsGrid.ColumnCount = 7;
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            columnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            columnsGrid.Controls.Add(headerCheck, 0, 0);
            columnsGrid.Controls.Add(colCustomer, 1, 0);
            columnsGrid.Controls.Add(colPets, 2, 0);
            columnsGrid.Controls.Add(colPhone, 3, 0);
            columnsGrid.Controls.Add(colEmail, 4, 0);
            columnsGrid.Controls.Add(colLastVisit, 5, 0);
            columnsGrid.Controls.Add(colActions, 6, 0);
            columnsGrid.Dock = DockStyle.Fill;
            columnsGrid.Location = new Point(0, 0);
            columnsGrid.Margin = new Padding(0, 0, 0, 0);
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
            // 
            // colCustomer
            // 
            colCustomer.BackColor = Color.FromArgb(246, 248, 251);
            colCustomer.Dock = DockStyle.Fill;
            colCustomer.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colCustomer.ForeColor = Color.FromArgb(122, 136, 156);
            colCustomer.Location = new Point(0, 0);
            colCustomer.Margin = new Padding(0, 0, 0, 0);
            colCustomer.Name = "colCustomer";
            colCustomer.Padding = new Padding(4, 0, 0, 0);
            colCustomer.Size = new Size(80, 34);
            colCustomer.TabIndex = 1;
            colCustomer.Text = "CUSTOMER";
            colCustomer.TextAlign = ContentAlignment.MiddleLeft;
            colCustomer.UseMnemonic = false;
            // 
            // colPets
            // 
            colPets.BackColor = Color.FromArgb(246, 248, 251);
            colPets.Dock = DockStyle.Fill;
            colPets.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colPets.ForeColor = Color.FromArgb(122, 136, 156);
            colPets.Location = new Point(0, 0);
            colPets.Margin = new Padding(0, 0, 0, 0);
            colPets.Name = "colPets";
            colPets.Padding = new Padding(4, 0, 0, 0);
            colPets.Size = new Size(80, 34);
            colPets.TabIndex = 2;
            colPets.Text = "PETS";
            colPets.TextAlign = ContentAlignment.MiddleLeft;
            colPets.UseMnemonic = false;
            // 
            // colPhone
            // 
            colPhone.BackColor = Color.FromArgb(246, 248, 251);
            colPhone.Dock = DockStyle.Fill;
            colPhone.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colPhone.ForeColor = Color.FromArgb(122, 136, 156);
            colPhone.Location = new Point(0, 0);
            colPhone.Margin = new Padding(0, 0, 0, 0);
            colPhone.Name = "colPhone";
            colPhone.Padding = new Padding(4, 0, 0, 0);
            colPhone.Size = new Size(80, 34);
            colPhone.TabIndex = 3;
            colPhone.Text = "PHONE";
            colPhone.TextAlign = ContentAlignment.MiddleLeft;
            colPhone.UseMnemonic = false;
            // 
            // colEmail
            // 
            colEmail.BackColor = Color.FromArgb(246, 248, 251);
            colEmail.Dock = DockStyle.Fill;
            colEmail.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colEmail.ForeColor = Color.FromArgb(122, 136, 156);
            colEmail.Location = new Point(0, 0);
            colEmail.Margin = new Padding(0, 0, 0, 0);
            colEmail.Name = "colEmail";
            colEmail.Padding = new Padding(4, 0, 0, 0);
            colEmail.Size = new Size(80, 34);
            colEmail.TabIndex = 4;
            colEmail.Text = "EMAIL";
            colEmail.TextAlign = ContentAlignment.MiddleLeft;
            colEmail.UseMnemonic = false;
            // 
            // colLastVisit
            // 
            colLastVisit.BackColor = Color.FromArgb(246, 248, 251);
            colLastVisit.Dock = DockStyle.Fill;
            colLastVisit.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colLastVisit.ForeColor = Color.FromArgb(122, 136, 156);
            colLastVisit.Location = new Point(0, 0);
            colLastVisit.Margin = new Padding(0, 0, 0, 0);
            colLastVisit.Name = "colLastVisit";
            colLastVisit.Padding = new Padding(4, 0, 0, 0);
            colLastVisit.Size = new Size(80, 34);
            colLastVisit.TabIndex = 5;
            colLastVisit.Text = "LAST VISIT";
            colLastVisit.TextAlign = ContentAlignment.MiddleLeft;
            colLastVisit.UseMnemonic = false;
            // 
            // colActions
            // 
            colActions.BackColor = Color.FromArgb(246, 248, 251);
            colActions.Dock = DockStyle.Fill;
            colActions.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colActions.ForeColor = Color.FromArgb(122, 136, 156);
            colActions.Location = new Point(0, 0);
            colActions.Margin = new Padding(0, 0, 0, 0);
            colActions.Name = "colActions";
            colActions.Padding = new Padding(4, 0, 0, 0);
            colActions.Size = new Size(80, 34);
            colActions.TabIndex = 6;
            colActions.Text = "ACTIONS";
            colActions.TextAlign = ContentAlignment.MiddleLeft;
            colActions.UseMnemonic = false;
            // 
            // customersHeader
            // 
            customersHeader.Controls.Add(customersTitle);
            customersHeader.Controls.Add(customersIcon);
            customersHeader.BackColor = Color.FromArgb(255, 255, 255);
            customersHeader.Dock = DockStyle.Top;
            customersHeader.Location = new Point(16, 12);
            customersHeader.Name = "customersHeader";
            customersHeader.Size = new Size(628, 44);
            customersHeader.TabIndex = 3;
            // 
            // customersTitle
            // 
            customersTitle.BackColor = Color.FromArgb(255, 255, 255);
            customersTitle.Dock = DockStyle.Left;
            customersTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            customersTitle.ForeColor = Color.FromArgb(31, 42, 60);
            customersTitle.Location = new Point(28, 0);
            customersTitle.Name = "customersTitle";
            customersTitle.Size = new Size(150, 44);
            customersTitle.TabIndex = 0;
            customersTitle.Text = "Customers (0)";
            customersTitle.TextAlign = ContentAlignment.MiddleLeft;
            customersTitle.UseMnemonic = false;
            // 
            // customersIcon
            // 
            customersIcon.BackColor = Color.FromArgb(255, 255, 255);
            customersIcon.Dock = DockStyle.Left;
            customersIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customersIcon.ForeColor = Color.FromArgb(58, 123, 213);
            customersIcon.Location = new Point(0, 0);
            customersIcon.Name = "customersIcon";
            customersIcon.Size = new Size(28, 44);
            customersIcon.TabIndex = 1;
            customersIcon.Text = "\uE716";
            customersIcon.TextAlign = ContentAlignment.MiddleCenter;
            customersIcon.UseMnemonic = false;
            // 
            // rightColumn
            // 
            rightColumn.BackColor = Color.FromArgb(243, 247, 252);
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightColumn.Controls.Add(quickFiltersCard, 0, 0);
            rightColumn.Controls.Add(recentCard, 0, 1);
            rightColumn.Controls.Add(exportCard, 0, 2);
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(0, 0);
            rightColumn.Margin = new Padding(0, 0, 0, 0);
            rightColumn.Name = "rightColumn";
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 248F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));
            rightColumn.Size = new Size(320, 572);
            rightColumn.TabIndex = 1;
            // 
            // quickFiltersCard
            // 
            quickFiltersCard.Controls.Add(quickFiltersList);
            quickFiltersCard.Controls.Add(quickFiltersHeader);
            quickFiltersCard.BorderColor = Color.FromArgb(227, 233, 242);
            quickFiltersCard.CornerColor = Color.FromArgb(243, 247, 252);
            quickFiltersCard.Dock = DockStyle.Fill;
            quickFiltersCard.FillColor = Color.FromArgb(255, 255, 255);
            quickFiltersCard.Location = new Point(0, 0);
            quickFiltersCard.Margin = new Padding(0, 0, 0, 12);
            quickFiltersCard.Name = "quickFiltersCard";
            quickFiltersCard.Padding = new Padding(16, 12, 16, 12);
            quickFiltersCard.Radius = 10;
            quickFiltersCard.Size = new Size(320, 236);
            quickFiltersCard.TabIndex = 0;
            // 
            // quickFiltersList
            // 
            quickFiltersList.Controls.Add(filterNewRow);
            quickFiltersList.Controls.Add(filterBronzeRow);
            quickFiltersList.Controls.Add(filterSilverRow);
            quickFiltersList.Controls.Add(filterGoldRow);
            quickFiltersList.Controls.Add(filterAllRow);
            quickFiltersList.BackColor = Color.FromArgb(255, 255, 255);
            quickFiltersList.Dock = DockStyle.Fill;
            quickFiltersList.Location = new Point(16, 44);
            quickFiltersList.Name = "quickFiltersList";
            quickFiltersList.Size = new Size(288, 180);
            quickFiltersList.TabIndex = 0;
            // 
            // filterNewRow
            // 
            filterNewRow.Controls.Add(filterNewName);
            filterNewRow.Controls.Add(filterNewCount);
            filterNewRow.Controls.Add(filterNewIcon);
            filterNewRow.BackColor = Color.FromArgb(255, 255, 255);
            filterNewRow.Dock = DockStyle.Top;
            filterNewRow.Location = new Point(0, 0);
            filterNewRow.Name = "filterNewRow";
            filterNewRow.Size = new Size(288, 36);
            filterNewRow.TabIndex = 4;
            // 
            // filterNewName
            // 
            filterNewName.BackColor = Color.FromArgb(255, 255, 255);
            filterNewName.Dock = DockStyle.Fill;
            filterNewName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            filterNewName.ForeColor = Color.FromArgb(31, 42, 60);
            filterNewName.Location = new Point(0, 0);
            filterNewName.Name = "filterNewName";
            filterNewName.Padding = new Padding(40, 0, 0, 0);
            filterNewName.Size = new Size(200, 36);
            filterNewName.TabIndex = 0;
            filterNewName.Text = "New Customers";
            filterNewName.TextAlign = ContentAlignment.MiddleLeft;
            filterNewName.UseMnemonic = false;
            // 
            // filterNewCount
            // 
            filterNewCount.BackColor = Color.FromArgb(255, 255, 255);
            filterNewCount.Dock = DockStyle.Right;
            filterNewCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            filterNewCount.ForeColor = Color.FromArgb(31, 42, 60);
            filterNewCount.Location = new Point(240, 0);
            filterNewCount.Name = "filterNewCount";
            filterNewCount.Padding = new Padding(0, 0, 10, 0);
            filterNewCount.Size = new Size(48, 36);
            filterNewCount.TabIndex = 1;
            filterNewCount.Text = "0";
            filterNewCount.TextAlign = ContentAlignment.MiddleRight;
            filterNewCount.UseMnemonic = false;
            // 
            // filterNewIcon
            // 
            filterNewIcon.BackColor = Color.FromArgb(255, 255, 255);
            filterNewIcon.FillColor = Color.FromArgb(229, 246, 236);
            filterNewIcon.ForeColor = Color.FromArgb(46, 158, 91);
            filterNewIcon.Glyph = "\uE710";
            filterNewIcon.GlyphSize = 8.5F;
            filterNewIcon.Location = new Point(6, 5);
            filterNewIcon.Name = "filterNewIcon";
            filterNewIcon.Size = new Size(26, 26);
            filterNewIcon.TabIndex = 2;
            filterNewIcon.TabStop = false;
            // 
            // filterBronzeRow
            // 
            filterBronzeRow.Controls.Add(filterBronzeName);
            filterBronzeRow.Controls.Add(filterBronzeCount);
            filterBronzeRow.Controls.Add(filterBronzeIcon);
            filterBronzeRow.BackColor = Color.FromArgb(255, 255, 255);
            filterBronzeRow.Dock = DockStyle.Top;
            filterBronzeRow.Location = new Point(0, 0);
            filterBronzeRow.Name = "filterBronzeRow";
            filterBronzeRow.Size = new Size(288, 36);
            filterBronzeRow.TabIndex = 3;
            // 
            // filterBronzeName
            // 
            filterBronzeName.BackColor = Color.FromArgb(255, 255, 255);
            filterBronzeName.Dock = DockStyle.Fill;
            filterBronzeName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            filterBronzeName.ForeColor = Color.FromArgb(31, 42, 60);
            filterBronzeName.Location = new Point(0, 0);
            filterBronzeName.Name = "filterBronzeName";
            filterBronzeName.Padding = new Padding(40, 0, 0, 0);
            filterBronzeName.Size = new Size(200, 36);
            filterBronzeName.TabIndex = 0;
            filterBronzeName.Text = "Bronze Members";
            filterBronzeName.TextAlign = ContentAlignment.MiddleLeft;
            filterBronzeName.UseMnemonic = false;
            // 
            // filterBronzeCount
            // 
            filterBronzeCount.BackColor = Color.FromArgb(255, 255, 255);
            filterBronzeCount.Dock = DockStyle.Right;
            filterBronzeCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            filterBronzeCount.ForeColor = Color.FromArgb(31, 42, 60);
            filterBronzeCount.Location = new Point(240, 0);
            filterBronzeCount.Name = "filterBronzeCount";
            filterBronzeCount.Padding = new Padding(0, 0, 10, 0);
            filterBronzeCount.Size = new Size(48, 36);
            filterBronzeCount.TabIndex = 1;
            filterBronzeCount.Text = "0";
            filterBronzeCount.TextAlign = ContentAlignment.MiddleRight;
            filterBronzeCount.UseMnemonic = false;
            // 
            // filterBronzeIcon
            // 
            filterBronzeIcon.BackColor = Color.FromArgb(255, 255, 255);
            filterBronzeIcon.FillColor = Color.FromArgb(253, 236, 231);
            filterBronzeIcon.ForeColor = Color.FromArgb(217, 119, 78);
            filterBronzeIcon.Glyph = "\uE734";
            filterBronzeIcon.GlyphSize = 8.5F;
            filterBronzeIcon.Location = new Point(6, 5);
            filterBronzeIcon.Name = "filterBronzeIcon";
            filterBronzeIcon.Size = new Size(26, 26);
            filterBronzeIcon.TabIndex = 2;
            filterBronzeIcon.TabStop = false;
            // 
            // filterSilverRow
            // 
            filterSilverRow.Controls.Add(filterSilverName);
            filterSilverRow.Controls.Add(filterSilverCount);
            filterSilverRow.Controls.Add(filterSilverIcon);
            filterSilverRow.BackColor = Color.FromArgb(255, 255, 255);
            filterSilverRow.Dock = DockStyle.Top;
            filterSilverRow.Location = new Point(0, 0);
            filterSilverRow.Name = "filterSilverRow";
            filterSilverRow.Size = new Size(288, 36);
            filterSilverRow.TabIndex = 2;
            // 
            // filterSilverName
            // 
            filterSilverName.BackColor = Color.FromArgb(255, 255, 255);
            filterSilverName.Dock = DockStyle.Fill;
            filterSilverName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            filterSilverName.ForeColor = Color.FromArgb(31, 42, 60);
            filterSilverName.Location = new Point(0, 0);
            filterSilverName.Name = "filterSilverName";
            filterSilverName.Padding = new Padding(40, 0, 0, 0);
            filterSilverName.Size = new Size(200, 36);
            filterSilverName.TabIndex = 0;
            filterSilverName.Text = "Silver Members";
            filterSilverName.TextAlign = ContentAlignment.MiddleLeft;
            filterSilverName.UseMnemonic = false;
            // 
            // filterSilverCount
            // 
            filterSilverCount.BackColor = Color.FromArgb(255, 255, 255);
            filterSilverCount.Dock = DockStyle.Right;
            filterSilverCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            filterSilverCount.ForeColor = Color.FromArgb(31, 42, 60);
            filterSilverCount.Location = new Point(240, 0);
            filterSilverCount.Name = "filterSilverCount";
            filterSilverCount.Padding = new Padding(0, 0, 10, 0);
            filterSilverCount.Size = new Size(48, 36);
            filterSilverCount.TabIndex = 1;
            filterSilverCount.Text = "0";
            filterSilverCount.TextAlign = ContentAlignment.MiddleRight;
            filterSilverCount.UseMnemonic = false;
            // 
            // filterSilverIcon
            // 
            filterSilverIcon.BackColor = Color.FromArgb(255, 255, 255);
            filterSilverIcon.FillColor = Color.FromArgb(238, 241, 246);
            filterSilverIcon.ForeColor = Color.FromArgb(122, 136, 156);
            filterSilverIcon.Glyph = "\uE734";
            filterSilverIcon.GlyphSize = 8.5F;
            filterSilverIcon.Location = new Point(6, 5);
            filterSilverIcon.Name = "filterSilverIcon";
            filterSilverIcon.Size = new Size(26, 26);
            filterSilverIcon.TabIndex = 2;
            filterSilverIcon.TabStop = false;
            // 
            // filterGoldRow
            // 
            filterGoldRow.Controls.Add(filterGoldName);
            filterGoldRow.Controls.Add(filterGoldCount);
            filterGoldRow.Controls.Add(filterGoldIcon);
            filterGoldRow.BackColor = Color.FromArgb(255, 255, 255);
            filterGoldRow.Dock = DockStyle.Top;
            filterGoldRow.Location = new Point(0, 0);
            filterGoldRow.Name = "filterGoldRow";
            filterGoldRow.Size = new Size(288, 36);
            filterGoldRow.TabIndex = 1;
            // 
            // filterGoldName
            // 
            filterGoldName.BackColor = Color.FromArgb(255, 255, 255);
            filterGoldName.Dock = DockStyle.Fill;
            filterGoldName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            filterGoldName.ForeColor = Color.FromArgb(31, 42, 60);
            filterGoldName.Location = new Point(0, 0);
            filterGoldName.Name = "filterGoldName";
            filterGoldName.Padding = new Padding(40, 0, 0, 0);
            filterGoldName.Size = new Size(200, 36);
            filterGoldName.TabIndex = 0;
            filterGoldName.Text = "Gold Members";
            filterGoldName.TextAlign = ContentAlignment.MiddleLeft;
            filterGoldName.UseMnemonic = false;
            // 
            // filterGoldCount
            // 
            filterGoldCount.BackColor = Color.FromArgb(255, 255, 255);
            filterGoldCount.Dock = DockStyle.Right;
            filterGoldCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            filterGoldCount.ForeColor = Color.FromArgb(31, 42, 60);
            filterGoldCount.Location = new Point(240, 0);
            filterGoldCount.Name = "filterGoldCount";
            filterGoldCount.Padding = new Padding(0, 0, 10, 0);
            filterGoldCount.Size = new Size(48, 36);
            filterGoldCount.TabIndex = 1;
            filterGoldCount.Text = "0";
            filterGoldCount.TextAlign = ContentAlignment.MiddleRight;
            filterGoldCount.UseMnemonic = false;
            // 
            // filterGoldIcon
            // 
            filterGoldIcon.BackColor = Color.FromArgb(255, 255, 255);
            filterGoldIcon.FillColor = Color.FromArgb(255, 244, 219);
            filterGoldIcon.ForeColor = Color.FromArgb(245, 158, 11);
            filterGoldIcon.Glyph = "\uE734";
            filterGoldIcon.GlyphSize = 8.5F;
            filterGoldIcon.Location = new Point(6, 5);
            filterGoldIcon.Name = "filterGoldIcon";
            filterGoldIcon.Size = new Size(26, 26);
            filterGoldIcon.TabIndex = 2;
            filterGoldIcon.TabStop = false;
            // 
            // filterAllRow
            // 
            filterAllRow.Controls.Add(filterAllName);
            filterAllRow.Controls.Add(filterAllCount);
            filterAllRow.Controls.Add(filterAllIcon);
            filterAllRow.BackColor = Color.FromArgb(234, 242, 253);
            filterAllRow.Dock = DockStyle.Top;
            filterAllRow.Location = new Point(0, 0);
            filterAllRow.Name = "filterAllRow";
            filterAllRow.Size = new Size(288, 36);
            filterAllRow.TabIndex = 0;
            // 
            // filterAllName
            // 
            filterAllName.BackColor = Color.FromArgb(234, 242, 253);
            filterAllName.Dock = DockStyle.Fill;
            filterAllName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            filterAllName.ForeColor = Color.FromArgb(31, 42, 60);
            filterAllName.Location = new Point(0, 0);
            filterAllName.Name = "filterAllName";
            filterAllName.Padding = new Padding(40, 0, 0, 0);
            filterAllName.Size = new Size(200, 36);
            filterAllName.TabIndex = 0;
            filterAllName.Text = "All Customers";
            filterAllName.TextAlign = ContentAlignment.MiddleLeft;
            filterAllName.UseMnemonic = false;
            // 
            // filterAllCount
            // 
            filterAllCount.BackColor = Color.FromArgb(234, 242, 253);
            filterAllCount.Dock = DockStyle.Right;
            filterAllCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            filterAllCount.ForeColor = Color.FromArgb(31, 42, 60);
            filterAllCount.Location = new Point(240, 0);
            filterAllCount.Name = "filterAllCount";
            filterAllCount.Padding = new Padding(0, 0, 10, 0);
            filterAllCount.Size = new Size(48, 36);
            filterAllCount.TabIndex = 1;
            filterAllCount.Text = "0";
            filterAllCount.TextAlign = ContentAlignment.MiddleRight;
            filterAllCount.UseMnemonic = false;
            // 
            // filterAllIcon
            // 
            filterAllIcon.BackColor = Color.FromArgb(234, 242, 253);
            filterAllIcon.FillColor = Color.FromArgb(58, 123, 213);
            filterAllIcon.ForeColor = Color.FromArgb(255, 255, 255);
            filterAllIcon.Glyph = "\uE716";
            filterAllIcon.GlyphSize = 8.5F;
            filterAllIcon.Location = new Point(6, 5);
            filterAllIcon.Name = "filterAllIcon";
            filterAllIcon.Size = new Size(26, 26);
            filterAllIcon.TabIndex = 2;
            filterAllIcon.TabStop = false;
            // 
            // quickFiltersHeader
            // 
            quickFiltersHeader.Controls.Add(quickFiltersLink);
            quickFiltersHeader.Controls.Add(quickFiltersTitle);
            quickFiltersHeader.Controls.Add(quickFiltersIcon);
            quickFiltersHeader.BackColor = Color.FromArgb(255, 255, 255);
            quickFiltersHeader.Dock = DockStyle.Top;
            quickFiltersHeader.Location = new Point(16, 12);
            quickFiltersHeader.Name = "quickFiltersHeader";
            quickFiltersHeader.Size = new Size(288, 32);
            quickFiltersHeader.TabIndex = 1;
            // 
            // quickFiltersLink
            // 
            quickFiltersLink.BackColor = Color.FromArgb(255, 255, 255);
            quickFiltersLink.Dock = DockStyle.Right;
            quickFiltersLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickFiltersLink.ForeColor = Color.FromArgb(58, 123, 213);
            quickFiltersLink.Location = new Point(218, 0);
            quickFiltersLink.Name = "quickFiltersLink";
            quickFiltersLink.Size = new Size(70, 32);
            quickFiltersLink.TabIndex = 0;
            quickFiltersLink.Text = "View All \u2192";
            quickFiltersLink.TextAlign = ContentAlignment.MiddleRight;
            quickFiltersLink.UseMnemonic = false;
            // 
            // quickFiltersTitle
            // 
            quickFiltersTitle.BackColor = Color.FromArgb(255, 255, 255);
            quickFiltersTitle.Dock = DockStyle.Left;
            quickFiltersTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            quickFiltersTitle.ForeColor = Color.FromArgb(31, 42, 60);
            quickFiltersTitle.Location = new Point(28, 0);
            quickFiltersTitle.Name = "quickFiltersTitle";
            quickFiltersTitle.Size = new Size(160, 32);
            quickFiltersTitle.TabIndex = 1;
            quickFiltersTitle.Text = "Quick Filters";
            quickFiltersTitle.TextAlign = ContentAlignment.MiddleLeft;
            quickFiltersTitle.UseMnemonic = false;
            // 
            // quickFiltersIcon
            // 
            quickFiltersIcon.BackColor = Color.FromArgb(255, 255, 255);
            quickFiltersIcon.Dock = DockStyle.Left;
            quickFiltersIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            quickFiltersIcon.ForeColor = Color.FromArgb(58, 123, 213);
            quickFiltersIcon.Location = new Point(0, 0);
            quickFiltersIcon.Name = "quickFiltersIcon";
            quickFiltersIcon.Size = new Size(28, 32);
            quickFiltersIcon.TabIndex = 2;
            quickFiltersIcon.Text = "\uE71C";
            quickFiltersIcon.TextAlign = ContentAlignment.MiddleCenter;
            quickFiltersIcon.UseMnemonic = false;
            // 
            // recentCard
            // 
            recentCard.Controls.Add(recentList);
            recentCard.Controls.Add(recentEmpty);
            recentCard.Controls.Add(recentHeader);
            recentCard.BorderColor = Color.FromArgb(227, 233, 242);
            recentCard.CornerColor = Color.FromArgb(243, 247, 252);
            recentCard.Dock = DockStyle.Fill;
            recentCard.FillColor = Color.FromArgb(255, 255, 255);
            recentCard.Location = new Point(0, 0);
            recentCard.Margin = new Padding(0, 0, 0, 12);
            recentCard.Name = "recentCard";
            recentCard.Padding = new Padding(16, 12, 16, 12);
            recentCard.Radius = 10;
            recentCard.Size = new Size(320, 196);
            recentCard.TabIndex = 1;
            // 
            // recentList
            // 
            recentList.BackColor = Color.FromArgb(255, 255, 255);
            recentList.AutoScroll = true;
            recentList.Dock = DockStyle.Fill;
            recentList.Location = new Point(16, 44);
            recentList.Name = "recentList";
            recentList.Size = new Size(288, 164);
            recentList.TabIndex = 0;
            recentList.Visible = false;
            // 
            // recentEmpty
            // 
            recentEmpty.BackColor = Color.FromArgb(255, 255, 255);
            recentEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            recentEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            recentEmpty.Dock = DockStyle.Fill;
            recentEmpty.Glyph = "\uE81C";
            recentEmpty.Hint = "Customers you look up will show up here.";
            recentEmpty.Location = new Point(16, 44);
            recentEmpty.Name = "recentEmpty";
            recentEmpty.Size = new Size(288, 164);
            recentEmpty.TabIndex = 1;
            recentEmpty.TabStop = false;
            recentEmpty.Title = "No recent lookups";
            // 
            // recentHeader
            // 
            recentHeader.Controls.Add(recentLink);
            recentHeader.Controls.Add(recentTitle);
            recentHeader.Controls.Add(recentIcon);
            recentHeader.BackColor = Color.FromArgb(255, 255, 255);
            recentHeader.Dock = DockStyle.Top;
            recentHeader.Location = new Point(16, 12);
            recentHeader.Name = "recentHeader";
            recentHeader.Size = new Size(288, 32);
            recentHeader.TabIndex = 2;
            // 
            // recentLink
            // 
            recentLink.BackColor = Color.FromArgb(255, 255, 255);
            recentLink.Dock = DockStyle.Right;
            recentLink.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recentLink.ForeColor = Color.FromArgb(58, 123, 213);
            recentLink.Location = new Point(218, 0);
            recentLink.Name = "recentLink";
            recentLink.Size = new Size(70, 32);
            recentLink.TabIndex = 0;
            recentLink.Text = "View All \u2192";
            recentLink.TextAlign = ContentAlignment.MiddleRight;
            recentLink.UseMnemonic = false;
            // 
            // recentTitle
            // 
            recentTitle.BackColor = Color.FromArgb(255, 255, 255);
            recentTitle.Dock = DockStyle.Left;
            recentTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            recentTitle.ForeColor = Color.FromArgb(31, 42, 60);
            recentTitle.Location = new Point(28, 0);
            recentTitle.Name = "recentTitle";
            recentTitle.Size = new Size(160, 32);
            recentTitle.TabIndex = 1;
            recentTitle.Text = "Recent Lookups";
            recentTitle.TextAlign = ContentAlignment.MiddleLeft;
            recentTitle.UseMnemonic = false;
            // 
            // recentIcon
            // 
            recentIcon.BackColor = Color.FromArgb(255, 255, 255);
            recentIcon.Dock = DockStyle.Left;
            recentIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            recentIcon.ForeColor = Color.FromArgb(58, 123, 213);
            recentIcon.Location = new Point(0, 0);
            recentIcon.Name = "recentIcon";
            recentIcon.Size = new Size(28, 32);
            recentIcon.TabIndex = 2;
            recentIcon.Text = "\uE81C";
            recentIcon.TextAlign = ContentAlignment.MiddleCenter;
            recentIcon.UseMnemonic = false;
            // 
            // exportCard
            // 
            exportCard.Controls.Add(exportGrid);
            exportCard.Controls.Add(exportHeader);
            exportCard.BorderColor = Color.FromArgb(227, 233, 242);
            exportCard.CornerColor = Color.FromArgb(243, 247, 252);
            exportCard.Dock = DockStyle.Fill;
            exportCard.FillColor = Color.FromArgb(255, 255, 255);
            exportCard.Location = new Point(0, 0);
            exportCard.Margin = new Padding(0, 0, 0, 0);
            exportCard.Name = "exportCard";
            exportCard.Padding = new Padding(16, 12, 16, 12);
            exportCard.Radius = 10;
            exportCard.Size = new Size(320, 116);
            exportCard.TabIndex = 2;
            // 
            // exportGrid
            // 
            exportGrid.BackColor = Color.FromArgb(255, 255, 255);
            exportGrid.ColumnCount = 2;
            exportGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            exportGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            exportGrid.Controls.Add(exportCustomersTile, 0, 0);
            exportGrid.Controls.Add(exportPetsTile, 1, 0);
            exportGrid.Dock = DockStyle.Fill;
            exportGrid.Location = new Point(16, 44);
            exportGrid.Margin = new Padding(0, 0, 0, 0);
            exportGrid.Name = "exportGrid";
            exportGrid.RowCount = 1;
            exportGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            exportGrid.Size = new Size(288, 60);
            exportGrid.TabIndex = 0;
            // 
            // exportCustomersTile
            // 
            exportCustomersTile.BackColor = Color.FromArgb(255, 255, 255);
            exportCustomersTile.Dock = DockStyle.Fill;
            exportCustomersTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exportCustomersTile.Glyph = "\uE8A5";
            exportCustomersTile.Location = new Point(3, 3);
            exportCustomersTile.Name = "exportCustomersTile";
            exportCustomersTile.Size = new Size(138, 54);
            exportCustomersTile.TabIndex = 0;
            exportCustomersTile.Text = "Export Customer List";
            exportCustomersTile.Click += exportCustomersTile_Click;
            // 
            // exportPetsTile
            // 
            exportPetsTile.BackColor = Color.FromArgb(255, 255, 255);
            exportPetsTile.Dock = DockStyle.Fill;
            exportPetsTile.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exportPetsTile.Glyph = "\uE896";
            exportPetsTile.Location = new Point(3, 3);
            exportPetsTile.Name = "exportPetsTile";
            exportPetsTile.Size = new Size(138, 54);
            exportPetsTile.TabIndex = 1;
            exportPetsTile.Text = "Download Pet Records";
            exportPetsTile.Click += exportPetsTile_Click;
            // 
            // exportHeader
            // 
            exportHeader.Controls.Add(exportTitle);
            exportHeader.Controls.Add(exportIcon);
            exportHeader.BackColor = Color.FromArgb(255, 255, 255);
            exportHeader.Dock = DockStyle.Top;
            exportHeader.Location = new Point(16, 12);
            exportHeader.Name = "exportHeader";
            exportHeader.Size = new Size(288, 32);
            exportHeader.TabIndex = 1;
            // 
            // exportTitle
            // 
            exportTitle.BackColor = Color.FromArgb(255, 255, 255);
            exportTitle.Dock = DockStyle.Left;
            exportTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportTitle.ForeColor = Color.FromArgb(31, 42, 60);
            exportTitle.Location = new Point(28, 0);
            exportTitle.Name = "exportTitle";
            exportTitle.Size = new Size(180, 32);
            exportTitle.TabIndex = 1;
            exportTitle.Text = "Export & Manage";
            exportTitle.TextAlign = ContentAlignment.MiddleLeft;
            exportTitle.UseMnemonic = false;
            // 
            // exportIcon
            // 
            exportIcon.BackColor = Color.FromArgb(255, 255, 255);
            exportIcon.Dock = DockStyle.Left;
            exportIcon.Font = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exportIcon.ForeColor = Color.FromArgb(58, 123, 213);
            exportIcon.Location = new Point(0, 0);
            exportIcon.Name = "exportIcon";
            exportIcon.Size = new Size(28, 32);
            exportIcon.TabIndex = 2;
            exportIcon.Text = "\uE898";
            exportIcon.TextAlign = ContentAlignment.MiddleCenter;
            exportIcon.UseMnemonic = false;
            // 
            // LookupPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            Controls.Add(rootGrid);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "LookupPage";
            Padding = new Padding(20);
            Size = new Size(1036, 732);
            exportHeader.ResumeLayout(false);
            exportGrid.ResumeLayout(false);
            exportCard.ResumeLayout(false);
            recentHeader.ResumeLayout(false);
            recentList.ResumeLayout(false);
            recentCard.ResumeLayout(false);
            quickFiltersHeader.ResumeLayout(false);
            filterAllRow.ResumeLayout(false);
            filterGoldRow.ResumeLayout(false);
            filterSilverRow.ResumeLayout(false);
            filterBronzeRow.ResumeLayout(false);
            filterNewRow.ResumeLayout(false);
            quickFiltersList.ResumeLayout(false);
            quickFiltersCard.ResumeLayout(false);
            rightColumn.ResumeLayout(false);
            customersHeader.ResumeLayout(false);
            columnsGrid.ResumeLayout(false);
            columnsHead.ResumeLayout(false);
            pager.ResumeLayout(false);
            tableFooter.ResumeLayout(false);
            rowsHost.ResumeLayout(false);
            customersCard.ResumeLayout(false);
            bodyGrid.ResumeLayout(false);
            filterGrid.ResumeLayout(false);
            filterCard.ResumeLayout(false);
            tabsRow.ResumeLayout(false);
            rootGrid.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel rootGrid;
        private System.Windows.Forms.Panel tabsRow;
        private MyApp.Controls.TabPill customerTab;
        private MyApp.Controls.TabPill petTab;
        private MyApp.Controls.CardPanel filterCard;
        private System.Windows.Forms.TableLayoutPanel filterGrid;
        private MyApp.Controls.InputField searchField;
        private MyApp.Controls.SelectField customerFilter;
        private MyApp.Controls.SelectField petFilter;
        private MyApp.Controls.SelectField sortFilter;
        private MyApp.Controls.PrimaryButton searchButton;
        private System.Windows.Forms.TableLayoutPanel bodyGrid;
        private MyApp.Controls.CardPanel customersCard;
        private System.Windows.Forms.Panel rowsHost;
        private MyApp.Controls.EmptyState customersEmpty;
        private System.Windows.Forms.Panel tableFooter;
        private System.Windows.Forms.Label showingLabel;
        private System.Windows.Forms.Panel pager;
        private MyApp.Controls.IconButton nextPageButton;
        private MyApp.Controls.PrimaryButton pageChip;
        private MyApp.Controls.IconButton prevPageButton;
        private System.Windows.Forms.Panel columnsHead;
        private System.Windows.Forms.TableLayoutPanel columnsGrid;
        private MyApp.Controls.CheckMark headerCheck;
        private System.Windows.Forms.Label colCustomer;
        private System.Windows.Forms.Label colPets;
        private System.Windows.Forms.Label colPhone;
        private System.Windows.Forms.Label colEmail;
        private System.Windows.Forms.Label colLastVisit;
        private System.Windows.Forms.Label colActions;
        private System.Windows.Forms.Panel customersHeader;
        private System.Windows.Forms.Label customersTitle;
        private System.Windows.Forms.Label customersIcon;
        private System.Windows.Forms.TableLayoutPanel rightColumn;
        private MyApp.Controls.CardPanel quickFiltersCard;
        private System.Windows.Forms.Panel quickFiltersList;
        private System.Windows.Forms.Panel filterNewRow;
        private System.Windows.Forms.Label filterNewName;
        private System.Windows.Forms.Label filterNewCount;
        private MyApp.Controls.IconBadge filterNewIcon;
        private System.Windows.Forms.Panel filterBronzeRow;
        private System.Windows.Forms.Label filterBronzeName;
        private System.Windows.Forms.Label filterBronzeCount;
        private MyApp.Controls.IconBadge filterBronzeIcon;
        private System.Windows.Forms.Panel filterSilverRow;
        private System.Windows.Forms.Label filterSilverName;
        private System.Windows.Forms.Label filterSilverCount;
        private MyApp.Controls.IconBadge filterSilverIcon;
        private System.Windows.Forms.Panel filterGoldRow;
        private System.Windows.Forms.Label filterGoldName;
        private System.Windows.Forms.Label filterGoldCount;
        private MyApp.Controls.IconBadge filterGoldIcon;
        private System.Windows.Forms.Panel filterAllRow;
        private System.Windows.Forms.Label filterAllName;
        private System.Windows.Forms.Label filterAllCount;
        private MyApp.Controls.IconBadge filterAllIcon;
        private System.Windows.Forms.Panel quickFiltersHeader;
        private System.Windows.Forms.Label quickFiltersLink;
        private System.Windows.Forms.Label quickFiltersTitle;
        private System.Windows.Forms.Label quickFiltersIcon;
        private MyApp.Controls.CardPanel recentCard;
        private System.Windows.Forms.Panel recentList;
        private MyApp.Controls.EmptyState recentEmpty;
        private System.Windows.Forms.Panel recentHeader;
        private System.Windows.Forms.Label recentLink;
        private System.Windows.Forms.Label recentTitle;
        private System.Windows.Forms.Label recentIcon;
        private MyApp.Controls.CardPanel exportCard;
        private System.Windows.Forms.TableLayoutPanel exportGrid;
        private MyApp.Controls.ActionTile exportCustomersTile;
        private MyApp.Controls.ActionTile exportPetsTile;
        private System.Windows.Forms.Panel exportHeader;
        private System.Windows.Forms.Label exportTitle;
        private System.Windows.Forms.Label exportIcon;
    }
}
