#nullable disable
namespace MyApp
{
    partial class EmployeeManagementPage
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
            pageScroll = new System.Windows.Forms.Panel();
            userSection = new MyApp.Controls.CardPanel();
            userTable = new MyApp.Controls.CardPanel();
            userRows = new System.Windows.Forms.Panel();
            userEmpty = new MyApp.Controls.EmptyState();
            userColumnsHead = new System.Windows.Forms.Panel();
            userColumnsGrid = new System.Windows.Forms.TableLayoutPanel();
            userHeaderCheck = new MyApp.Controls.CheckMark();
            userColId = new System.Windows.Forms.Label();
            userColUsername = new System.Windows.Forms.Label();
            userColFullName = new System.Windows.Forms.Label();
            userColRole = new System.Windows.Forms.Label();
            userColEmail = new System.Windows.Forms.Label();
            userColStatus = new System.Windows.Forms.Label();
            userColLastLogin = new System.Windows.Forms.Label();
            userColActions = new System.Windows.Forms.Label();
            userFooter = new System.Windows.Forms.Panel();
            userShowing = new System.Windows.Forms.Label();
            userPager = new System.Windows.Forms.Panel();
            userPrevPage = new MyApp.Controls.IconButton();
            userPageChip = new MyApp.Controls.PrimaryButton();
            userNextPage = new MyApp.Controls.IconButton();
            userToolbar = new System.Windows.Forms.Panel();
            userSearch = new MyApp.Controls.InputField();
            userToolbarGapA = new System.Windows.Forms.Panel();
            userRoleFilter = new MyApp.Controls.SelectField();
            userToolbarGapB = new System.Windows.Forms.Panel();
            userStatusFilter = new MyApp.Controls.SelectField();
            userStats = new System.Windows.Forms.TableLayoutPanel();
            userTotalTile = new MyApp.Controls.CardPanel();
            userTotalDelta = new System.Windows.Forms.Label();
            userTotalValue = new System.Windows.Forms.Label();
            userTotalLabel = new System.Windows.Forms.Label();
            userTotalIconHost = new System.Windows.Forms.Panel();
            userTotalIcon = new MyApp.Controls.IconBadge();
            userAdminTile = new MyApp.Controls.CardPanel();
            userAdminDelta = new System.Windows.Forms.Label();
            userAdminValue = new System.Windows.Forms.Label();
            userAdminLabel = new System.Windows.Forms.Label();
            userAdminIconHost = new System.Windows.Forms.Panel();
            userAdminIcon = new MyApp.Controls.IconBadge();
            userStaffTile = new MyApp.Controls.CardPanel();
            userStaffDelta = new System.Windows.Forms.Label();
            userStaffValue = new System.Windows.Forms.Label();
            userStaffLabel = new System.Windows.Forms.Label();
            userStaffIconHost = new System.Windows.Forms.Panel();
            userStaffIcon = new MyApp.Controls.IconBadge();
            userCustomerTile = new MyApp.Controls.CardPanel();
            userCustomerDelta = new System.Windows.Forms.Label();
            userCustomerValue = new System.Windows.Forms.Label();
            userCustomerLabel = new System.Windows.Forms.Label();
            userCustomerIconHost = new System.Windows.Forms.Panel();
            userCustomerIcon = new MyApp.Controls.IconBadge();
            userHeader = new System.Windows.Forms.Panel();
            userHeaderText = new System.Windows.Forms.Panel();
            userSubtitle = new System.Windows.Forms.Label();
            userTitle = new System.Windows.Forms.Label();
            userHeaderIconHost = new System.Windows.Forms.Panel();
            userHeaderIcon = new MyApp.Controls.IconBadge();
            userAddHost = new System.Windows.Forms.Panel();
            userAddButton = new MyApp.Controls.PrimaryButton();
            sectionGap = new System.Windows.Forms.Panel();
            employeeSection = new MyApp.Controls.CardPanel();
            employeeTable = new MyApp.Controls.CardPanel();
            employeeRows = new System.Windows.Forms.Panel();
            employeeEmpty = new MyApp.Controls.EmptyState();
            employeeColumnsHead = new System.Windows.Forms.Panel();
            employeeColumnsGrid = new System.Windows.Forms.TableLayoutPanel();
            employeeHeaderCheck = new MyApp.Controls.CheckMark();
            employeeColId = new System.Windows.Forms.Label();
            employeeColName = new System.Windows.Forms.Label();
            employeeColRole = new System.Windows.Forms.Label();
            employeeColDepartment = new System.Windows.Forms.Label();
            employeeColEmail = new System.Windows.Forms.Label();
            employeeColStatus = new System.Windows.Forms.Label();
            employeeColActions = new System.Windows.Forms.Label();
            employeeFooter = new System.Windows.Forms.Panel();
            employeeShowing = new System.Windows.Forms.Label();
            employeePager = new System.Windows.Forms.Panel();
            employeePrevPage = new MyApp.Controls.IconButton();
            employeePageChip = new MyApp.Controls.PrimaryButton();
            employeeNextPage = new MyApp.Controls.IconButton();
            employeeToolbar = new System.Windows.Forms.Panel();
            employeeSearch = new MyApp.Controls.InputField();
            employeeToolbarGapA = new System.Windows.Forms.Panel();
            employeeRoleFilter = new MyApp.Controls.SelectField();
            employeeToolbarGapB = new System.Windows.Forms.Panel();
            employeeStatusFilter = new MyApp.Controls.SelectField();
            employeeStats = new System.Windows.Forms.TableLayoutPanel();
            employeeTotalTile = new MyApp.Controls.CardPanel();
            employeeTotalDelta = new System.Windows.Forms.Label();
            employeeTotalValue = new System.Windows.Forms.Label();
            employeeTotalLabel = new System.Windows.Forms.Label();
            employeeTotalIconHost = new System.Windows.Forms.Panel();
            employeeTotalIcon = new MyApp.Controls.IconBadge();
            employeeAdminTile = new MyApp.Controls.CardPanel();
            employeeAdminDelta = new System.Windows.Forms.Label();
            employeeAdminValue = new System.Windows.Forms.Label();
            employeeAdminLabel = new System.Windows.Forms.Label();
            employeeAdminIconHost = new System.Windows.Forms.Panel();
            employeeAdminIcon = new MyApp.Controls.IconBadge();
            employeeCashierTile = new MyApp.Controls.CardPanel();
            employeeCashierDelta = new System.Windows.Forms.Label();
            employeeCashierValue = new System.Windows.Forms.Label();
            employeeCashierLabel = new System.Windows.Forms.Label();
            employeeCashierIconHost = new System.Windows.Forms.Panel();
            employeeCashierIcon = new MyApp.Controls.IconBadge();
            employeeFrontDeskTile = new MyApp.Controls.CardPanel();
            employeeFrontDeskDelta = new System.Windows.Forms.Label();
            employeeFrontDeskValue = new System.Windows.Forms.Label();
            employeeFrontDeskLabel = new System.Windows.Forms.Label();
            employeeFrontDeskIconHost = new System.Windows.Forms.Panel();
            employeeFrontDeskIcon = new MyApp.Controls.IconBadge();
            employeeGroomersTile = new MyApp.Controls.CardPanel();
            employeeGroomersDelta = new System.Windows.Forms.Label();
            employeeGroomersValue = new System.Windows.Forms.Label();
            employeeGroomersLabel = new System.Windows.Forms.Label();
            employeeGroomersIconHost = new System.Windows.Forms.Panel();
            employeeGroomersIcon = new MyApp.Controls.IconBadge();
            employeeHeader = new System.Windows.Forms.Panel();
            employeeHeaderText = new System.Windows.Forms.Panel();
            employeeSubtitle = new System.Windows.Forms.Label();
            employeeTitle = new System.Windows.Forms.Label();
            employeeHeaderIconHost = new System.Windows.Forms.Panel();
            employeeHeaderIcon = new MyApp.Controls.IconBadge();
            employeeAddHost = new System.Windows.Forms.Panel();
            employeeAddButton = new MyApp.Controls.PrimaryButton();
            pageScroll.SuspendLayout();
            userSection.SuspendLayout();
            userTable.SuspendLayout();
            userRows.SuspendLayout();
            userColumnsHead.SuspendLayout();
            userColumnsGrid.SuspendLayout();
            userFooter.SuspendLayout();
            userPager.SuspendLayout();
            userToolbar.SuspendLayout();
            userStats.SuspendLayout();
            userTotalTile.SuspendLayout();
            userTotalIconHost.SuspendLayout();
            userAdminTile.SuspendLayout();
            userAdminIconHost.SuspendLayout();
            userStaffTile.SuspendLayout();
            userStaffIconHost.SuspendLayout();
            userCustomerTile.SuspendLayout();
            userCustomerIconHost.SuspendLayout();
            userHeader.SuspendLayout();
            userHeaderText.SuspendLayout();
            userHeaderIconHost.SuspendLayout();
            userAddHost.SuspendLayout();
            employeeSection.SuspendLayout();
            employeeTable.SuspendLayout();
            employeeRows.SuspendLayout();
            employeeColumnsHead.SuspendLayout();
            employeeColumnsGrid.SuspendLayout();
            employeeFooter.SuspendLayout();
            employeePager.SuspendLayout();
            employeeToolbar.SuspendLayout();
            employeeStats.SuspendLayout();
            employeeTotalTile.SuspendLayout();
            employeeTotalIconHost.SuspendLayout();
            employeeAdminTile.SuspendLayout();
            employeeAdminIconHost.SuspendLayout();
            employeeCashierTile.SuspendLayout();
            employeeCashierIconHost.SuspendLayout();
            employeeFrontDeskTile.SuspendLayout();
            employeeFrontDeskIconHost.SuspendLayout();
            employeeGroomersTile.SuspendLayout();
            employeeGroomersIconHost.SuspendLayout();
            employeeHeader.SuspendLayout();
            employeeHeaderText.SuspendLayout();
            employeeHeaderIconHost.SuspendLayout();
            employeeAddHost.SuspendLayout();
            SuspendLayout();
            // 
            // pageScroll
            // 
            pageScroll.AutoScroll = true;
            pageScroll.BackColor = Color.FromArgb(243, 247, 252);
            pageScroll.Controls.Add(userSection);
            pageScroll.Controls.Add(sectionGap);
            pageScroll.Controls.Add(employeeSection);
            pageScroll.Dock = DockStyle.Fill;
            pageScroll.Location = new Point(0, 0);
            pageScroll.Name = "pageScroll";
            pageScroll.Padding = new Padding(20, 20, 20, 20);
            pageScroll.Size = new Size(1036, 732);
            pageScroll.TabIndex = 0;
            // 
            // userSection
            // 
            userSection.BorderColor = Color.FromArgb(227, 233, 242);
            userSection.Controls.Add(userTable);
            userSection.Controls.Add(userFooter);
            userSection.Controls.Add(userToolbar);
            userSection.Controls.Add(userStats);
            userSection.Controls.Add(userHeader);
            userSection.CornerColor = Color.FromArgb(243, 247, 252);
            userSection.Dock = DockStyle.Top;
            userSection.FillColor = Color.White;
            userSection.Location = new Point(0, 540);
            userSection.Name = "userSection";
            userSection.Padding = new Padding(20, 16, 20, 12);
            userSection.Radius = 10;
            userSection.Size = new Size(996, 520);
            userSection.TabIndex = 0;
            // 
            // userTable
            // 
            userTable.BorderColor = Color.FromArgb(227, 233, 242);
            userTable.Controls.Add(userRows);
            userTable.Controls.Add(userColumnsHead);
            userTable.CornerColor = Color.White;
            userTable.Dock = DockStyle.Fill;
            userTable.FillColor = Color.White;
            userTable.Location = new Point(0, 0);
            userTable.Name = "userTable";
            userTable.Padding = new Padding(1, 1, 1, 1);
            userTable.Radius = 8;
            userTable.Size = new Size(956, 236);
            userTable.TabIndex = 0;
            // 
            // userRows
            // 
            userRows.AutoScroll = true;
            userRows.BackColor = Color.White;
            userRows.Controls.Add(userEmpty);
            userRows.Dock = DockStyle.Fill;
            userRows.Location = new Point(0, 34);
            userRows.Name = "userRows";
            userRows.Size = new Size(954, 200);
            userRows.TabIndex = 0;
            // 
            // userEmpty
            // 
            userEmpty.BackColor = Color.White;
            userEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            userEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            userEmpty.Dock = DockStyle.Fill;
            userEmpty.Glyph = "\uE77B";
            userEmpty.Hint = "System accounts will appear here once they are created.";
            userEmpty.Location = new Point(0, 0);
            userEmpty.Name = "userEmpty";
            userEmpty.Size = new Size(954, 200);
            userEmpty.TabIndex = 0;
            userEmpty.TabStop = false;
            userEmpty.Title = "No users found";
            // 
            // userColumnsHead
            // 
            userColumnsHead.BackColor = Color.FromArgb(246, 248, 251);
            userColumnsHead.Controls.Add(userColumnsGrid);
            userColumnsHead.Dock = DockStyle.Top;
            userColumnsHead.Location = new Point(0, 0);
            userColumnsHead.Name = "userColumnsHead";
            userColumnsHead.Size = new Size(954, 34);
            userColumnsHead.TabIndex = 1;
            // 
            // userColumnsGrid
            // 
            userColumnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            userColumnsGrid.ColumnCount = 9;
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 84F));
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            userColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8F));
            userColumnsGrid.Controls.Add(userHeaderCheck, 0, 0);
            userColumnsGrid.Controls.Add(userColId, 1, 0);
            userColumnsGrid.Controls.Add(userColUsername, 2, 0);
            userColumnsGrid.Controls.Add(userColFullName, 3, 0);
            userColumnsGrid.Controls.Add(userColRole, 4, 0);
            userColumnsGrid.Controls.Add(userColEmail, 5, 0);
            userColumnsGrid.Controls.Add(userColStatus, 6, 0);
            userColumnsGrid.Controls.Add(userColLastLogin, 7, 0);
            userColumnsGrid.Controls.Add(userColActions, 8, 0);
            userColumnsGrid.Dock = DockStyle.Fill;
            userColumnsGrid.Location = new Point(0, 0);
            userColumnsGrid.Margin = new Padding(0, 0, 0, 0);
            userColumnsGrid.Name = "userColumnsGrid";
            userColumnsGrid.RowCount = 1;
            userColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            userColumnsGrid.Size = new Size(954, 34);
            userColumnsGrid.TabIndex = 0;
            // 
            // userHeaderCheck
            // 
            userHeaderCheck.Anchor = AnchorStyles.None;
            userHeaderCheck.BackColor = Color.FromArgb(246, 248, 251);
            userHeaderCheck.Location = new Point(13, 8);
            userHeaderCheck.Name = "userHeaderCheck";
            userHeaderCheck.Size = new Size(18, 18);
            userHeaderCheck.TabIndex = 0;
            userHeaderCheck.TabStop = false;
            // 
            // userColId
            // 
            userColId.BackColor = Color.FromArgb(246, 248, 251);
            userColId.Dock = DockStyle.Fill;
            userColId.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userColId.ForeColor = Color.FromArgb(122, 136, 156);
            userColId.Location = new Point(0, 0);
            userColId.Margin = new Padding(0, 0, 0, 0);
            userColId.Name = "userColId";
            userColId.Padding = new Padding(8, 0, 0, 0);
            userColId.Size = new Size(80, 34);
            userColId.TabIndex = 1;
            userColId.Text = "ID";
            userColId.TextAlign = ContentAlignment.MiddleLeft;
            userColId.UseMnemonic = false;
            // 
            // userColUsername
            // 
            userColUsername.BackColor = Color.FromArgb(246, 248, 251);
            userColUsername.Dock = DockStyle.Fill;
            userColUsername.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userColUsername.ForeColor = Color.FromArgb(122, 136, 156);
            userColUsername.Location = new Point(0, 0);
            userColUsername.Margin = new Padding(0, 0, 0, 0);
            userColUsername.Name = "userColUsername";
            userColUsername.Padding = new Padding(8, 0, 0, 0);
            userColUsername.Size = new Size(80, 34);
            userColUsername.TabIndex = 2;
            userColUsername.Text = "USERNAME";
            userColUsername.TextAlign = ContentAlignment.MiddleLeft;
            userColUsername.UseMnemonic = false;
            // 
            // userColFullName
            // 
            userColFullName.BackColor = Color.FromArgb(246, 248, 251);
            userColFullName.Dock = DockStyle.Fill;
            userColFullName.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userColFullName.ForeColor = Color.FromArgb(122, 136, 156);
            userColFullName.Location = new Point(0, 0);
            userColFullName.Margin = new Padding(0, 0, 0, 0);
            userColFullName.Name = "userColFullName";
            userColFullName.Padding = new Padding(8, 0, 0, 0);
            userColFullName.Size = new Size(80, 34);
            userColFullName.TabIndex = 3;
            userColFullName.Text = "FULL NAME";
            userColFullName.TextAlign = ContentAlignment.MiddleLeft;
            userColFullName.UseMnemonic = false;
            // 
            // userColRole
            // 
            userColRole.BackColor = Color.FromArgb(246, 248, 251);
            userColRole.Dock = DockStyle.Fill;
            userColRole.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userColRole.ForeColor = Color.FromArgb(122, 136, 156);
            userColRole.Location = new Point(0, 0);
            userColRole.Margin = new Padding(0, 0, 0, 0);
            userColRole.Name = "userColRole";
            userColRole.Padding = new Padding(8, 0, 0, 0);
            userColRole.Size = new Size(80, 34);
            userColRole.TabIndex = 4;
            userColRole.Text = "ROLE";
            userColRole.TextAlign = ContentAlignment.MiddleLeft;
            userColRole.UseMnemonic = false;
            // 
            // userColEmail
            // 
            userColEmail.BackColor = Color.FromArgb(246, 248, 251);
            userColEmail.Dock = DockStyle.Fill;
            userColEmail.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userColEmail.ForeColor = Color.FromArgb(122, 136, 156);
            userColEmail.Location = new Point(0, 0);
            userColEmail.Margin = new Padding(0, 0, 0, 0);
            userColEmail.Name = "userColEmail";
            userColEmail.Padding = new Padding(8, 0, 0, 0);
            userColEmail.Size = new Size(80, 34);
            userColEmail.TabIndex = 5;
            userColEmail.Text = "EMAIL";
            userColEmail.TextAlign = ContentAlignment.MiddleLeft;
            userColEmail.UseMnemonic = false;
            // 
            // userColStatus
            // 
            userColStatus.BackColor = Color.FromArgb(246, 248, 251);
            userColStatus.Dock = DockStyle.Fill;
            userColStatus.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userColStatus.ForeColor = Color.FromArgb(122, 136, 156);
            userColStatus.Location = new Point(0, 0);
            userColStatus.Margin = new Padding(0, 0, 0, 0);
            userColStatus.Name = "userColStatus";
            userColStatus.Padding = new Padding(8, 0, 0, 0);
            userColStatus.Size = new Size(80, 34);
            userColStatus.TabIndex = 6;
            userColStatus.Text = "STATUS";
            userColStatus.TextAlign = ContentAlignment.MiddleLeft;
            userColStatus.UseMnemonic = false;
            // 
            // userColLastLogin
            // 
            userColLastLogin.BackColor = Color.FromArgb(246, 248, 251);
            userColLastLogin.Dock = DockStyle.Fill;
            userColLastLogin.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userColLastLogin.ForeColor = Color.FromArgb(122, 136, 156);
            userColLastLogin.Location = new Point(0, 0);
            userColLastLogin.Margin = new Padding(0, 0, 0, 0);
            userColLastLogin.Name = "userColLastLogin";
            userColLastLogin.Padding = new Padding(8, 0, 0, 0);
            userColLastLogin.Size = new Size(80, 34);
            userColLastLogin.TabIndex = 7;
            userColLastLogin.Text = "LAST LOGIN";
            userColLastLogin.TextAlign = ContentAlignment.MiddleLeft;
            userColLastLogin.UseMnemonic = false;
            // 
            // userColActions
            // 
            userColActions.BackColor = Color.FromArgb(246, 248, 251);
            userColActions.Dock = DockStyle.Fill;
            userColActions.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userColActions.ForeColor = Color.FromArgb(122, 136, 156);
            userColActions.Location = new Point(0, 0);
            userColActions.Margin = new Padding(0, 0, 0, 0);
            userColActions.Name = "userColActions";
            userColActions.Padding = new Padding(8, 0, 0, 0);
            userColActions.Size = new Size(80, 34);
            userColActions.TabIndex = 8;
            userColActions.Text = "ACTIONS";
            userColActions.TextAlign = ContentAlignment.MiddleLeft;
            userColActions.UseMnemonic = false;
            // 
            // userFooter
            // 
            userFooter.BackColor = Color.White;
            userFooter.Controls.Add(userShowing);
            userFooter.Controls.Add(userPager);
            userFooter.Dock = DockStyle.Bottom;
            userFooter.Location = new Point(0, 0);
            userFooter.Name = "userFooter";
            userFooter.Size = new Size(956, 44);
            userFooter.TabIndex = 1;
            // 
            // userShowing
            // 
            userShowing.BackColor = Color.White;
            userShowing.Dock = DockStyle.Fill;
            userShowing.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userShowing.ForeColor = Color.FromArgb(122, 136, 156);
            userShowing.Location = new Point(0, 0);
            userShowing.Name = "userShowing";
            userShowing.Size = new Size(600, 44);
            userShowing.TabIndex = 0;
            userShowing.Text = "Showing 0 users";
            userShowing.TextAlign = ContentAlignment.MiddleLeft;
            userShowing.UseMnemonic = false;
            // 
            // userPager
            // 
            userPager.BackColor = Color.White;
            userPager.Controls.Add(userPrevPage);
            userPager.Controls.Add(userPageChip);
            userPager.Controls.Add(userNextPage);
            userPager.Dock = DockStyle.Right;
            userPager.Location = new Point(0, 0);
            userPager.Name = "userPager";
            userPager.Size = new Size(116, 44);
            userPager.TabIndex = 1;
            // 
            // userPrevPage
            // 
            userPrevPage.BackColor = Color.White;
            userPrevPage.Enabled = false;
            userPrevPage.Glyph = "\uE76B";
            userPrevPage.Location = new Point(4, 7);
            userPrevPage.Name = "userPrevPage";
            userPrevPage.Size = new Size(30, 30);
            userPrevPage.TabIndex = 0;
            // 
            // userPageChip
            // 
            userPageChip.BackColor = Color.White;
            userPageChip.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userPageChip.Location = new Point(42, 7);
            userPageChip.Name = "userPageChip";
            userPageChip.Size = new Size(30, 30);
            userPageChip.TabIndex = 1;
            userPageChip.Text = "1";
            // 
            // userNextPage
            // 
            userNextPage.BackColor = Color.White;
            userNextPage.Enabled = false;
            userNextPage.Glyph = "\uE76C";
            userNextPage.Location = new Point(80, 7);
            userNextPage.Name = "userNextPage";
            userNextPage.Size = new Size(30, 30);
            userNextPage.TabIndex = 2;
            // 
            // userToolbar
            // 
            userToolbar.BackColor = Color.White;
            userToolbar.Controls.Add(userSearch);
            userToolbar.Controls.Add(userToolbarGapA);
            userToolbar.Controls.Add(userRoleFilter);
            userToolbar.Controls.Add(userToolbarGapB);
            userToolbar.Controls.Add(userStatusFilter);
            userToolbar.Dock = DockStyle.Top;
            userToolbar.Location = new Point(0, 0);
            userToolbar.Name = "userToolbar";
            userToolbar.Padding = new Padding(0, 4, 0, 8);
            userToolbar.Size = new Size(956, 52);
            userToolbar.TabIndex = 2;
            // 
            // userSearch
            // 
            userSearch.BorderColor = Color.FromArgb(227, 233, 242);
            userSearch.CornerColor = Color.White;
            userSearch.Dock = DockStyle.Fill;
            userSearch.FillColor = Color.White;
            userSearch.Glyph = "\uE721";
            userSearch.Location = new Point(0, 4);
            userSearch.Name = "userSearch";
            userSearch.Padding = new Padding(10, 3, 10, 3);
            userSearch.Placeholder = "Search users...";
            userSearch.Radius = 8;
            userSearch.Size = new Size(560, 40);
            userSearch.TabIndex = 0;
            // 
            // userToolbarGapA
            // 
            userToolbarGapA.BackColor = Color.White;
            userToolbarGapA.Dock = DockStyle.Right;
            userToolbarGapA.Location = new Point(0, 0);
            userToolbarGapA.Name = "userToolbarGapA";
            userToolbarGapA.Size = new Size(12, 40);
            userToolbarGapA.TabIndex = 1;
            // 
            // userRoleFilter
            // 
            userRoleFilter.BackColor = Color.White;
            userRoleFilter.Dock = DockStyle.Right;
            userRoleFilter.Location = new Point(0, 0);
            userRoleFilter.Name = "userRoleFilter";
            userRoleFilter.Size = new Size(140, 40);
            userRoleFilter.TabIndex = 2;
            userRoleFilter.Text = "All Roles";
            // 
            // userToolbarGapB
            // 
            userToolbarGapB.BackColor = Color.White;
            userToolbarGapB.Dock = DockStyle.Right;
            userToolbarGapB.Location = new Point(0, 0);
            userToolbarGapB.Name = "userToolbarGapB";
            userToolbarGapB.Size = new Size(10, 40);
            userToolbarGapB.TabIndex = 3;
            // 
            // userStatusFilter
            // 
            userStatusFilter.BackColor = Color.White;
            userStatusFilter.Dock = DockStyle.Right;
            userStatusFilter.Location = new Point(0, 0);
            userStatusFilter.Name = "userStatusFilter";
            userStatusFilter.Size = new Size(140, 40);
            userStatusFilter.TabIndex = 4;
            userStatusFilter.Text = "All Status";
            // 
            // userStats
            // 
            userStats.BackColor = Color.White;
            userStats.ColumnCount = 7;
            userStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            userStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            userStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            userStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            userStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            userStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            userStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            userStats.Controls.Add(userTotalTile, 0, 0);
            userStats.Controls.Add(userAdminTile, 2, 0);
            userStats.Controls.Add(userStaffTile, 4, 0);
            userStats.Controls.Add(userCustomerTile, 6, 0);
            userStats.Dock = DockStyle.Top;
            userStats.Location = new Point(0, 0);
            userStats.Margin = new Padding(0, 0, 0, 0);
            userStats.Name = "userStats";
            userStats.RowCount = 1;
            userStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            userStats.Size = new Size(956, 96);
            userStats.TabIndex = 3;
            // 
            // userTotalTile
            // 
            userTotalTile.BorderColor = Color.FromArgb(227, 233, 242);
            userTotalTile.Controls.Add(userTotalDelta);
            userTotalTile.Controls.Add(userTotalValue);
            userTotalTile.Controls.Add(userTotalLabel);
            userTotalTile.Controls.Add(userTotalIconHost);
            userTotalTile.CornerColor = Color.White;
            userTotalTile.Dock = DockStyle.Fill;
            userTotalTile.FillColor = Color.White;
            userTotalTile.Location = new Point(0, 0);
            userTotalTile.Margin = new Padding(0, 0, 0, 12);
            userTotalTile.Name = "userTotalTile";
            userTotalTile.Padding = new Padding(12, 8, 12, 8);
            userTotalTile.Radius = 8;
            userTotalTile.Size = new Size(180, 84);
            userTotalTile.TabIndex = 0;
            // 
            // userTotalDelta
            // 
            userTotalDelta.BackColor = Color.White;
            userTotalDelta.Dock = DockStyle.Top;
            userTotalDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userTotalDelta.ForeColor = Color.FromArgb(122, 136, 156);
            userTotalDelta.Location = new Point(0, 0);
            userTotalDelta.Name = "userTotalDelta";
            userTotalDelta.Size = new Size(110, 14);
            userTotalDelta.TabIndex = 0;
            userTotalDelta.Text = "\u2014 vs. last month";
            userTotalDelta.TextAlign = ContentAlignment.MiddleLeft;
            userTotalDelta.UseMnemonic = false;
            // 
            // userTotalValue
            // 
            userTotalValue.BackColor = Color.White;
            userTotalValue.Dock = DockStyle.Top;
            userTotalValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userTotalValue.ForeColor = Color.FromArgb(31, 42, 60);
            userTotalValue.Location = new Point(0, 0);
            userTotalValue.Name = "userTotalValue";
            userTotalValue.Size = new Size(110, 26);
            userTotalValue.TabIndex = 1;
            userTotalValue.Text = "0";
            userTotalValue.TextAlign = ContentAlignment.MiddleLeft;
            userTotalValue.UseMnemonic = false;
            // 
            // userTotalLabel
            // 
            userTotalLabel.BackColor = Color.White;
            userTotalLabel.Dock = DockStyle.Top;
            userTotalLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userTotalLabel.ForeColor = Color.FromArgb(68, 83, 106);
            userTotalLabel.Location = new Point(0, 0);
            userTotalLabel.Name = "userTotalLabel";
            userTotalLabel.Size = new Size(110, 18);
            userTotalLabel.TabIndex = 2;
            userTotalLabel.Text = "Total Users";
            userTotalLabel.TextAlign = ContentAlignment.MiddleLeft;
            userTotalLabel.UseMnemonic = false;
            // 
            // userTotalIconHost
            // 
            userTotalIconHost.BackColor = Color.White;
            userTotalIconHost.Controls.Add(userTotalIcon);
            userTotalIconHost.Dock = DockStyle.Left;
            userTotalIconHost.Location = new Point(0, 0);
            userTotalIconHost.Name = "userTotalIconHost";
            userTotalIconHost.Size = new Size(44, 68);
            userTotalIconHost.TabIndex = 3;
            // 
            // userTotalIcon
            // 
            userTotalIcon.BackColor = Color.White;
            userTotalIcon.FillColor = Color.FromArgb(234, 242, 253);
            userTotalIcon.ForeColor = Color.FromArgb(58, 123, 213);
            userTotalIcon.Glyph = "\uE716";
            userTotalIcon.GlyphSize = 11F;
            userTotalIcon.Location = new Point(0, 2);
            userTotalIcon.Name = "userTotalIcon";
            userTotalIcon.Radius = 8;
            userTotalIcon.Size = new Size(34, 34);
            userTotalIcon.TabIndex = 0;
            userTotalIcon.TabStop = false;
            // 
            // userAdminTile
            // 
            userAdminTile.BorderColor = Color.FromArgb(227, 233, 242);
            userAdminTile.Controls.Add(userAdminDelta);
            userAdminTile.Controls.Add(userAdminValue);
            userAdminTile.Controls.Add(userAdminLabel);
            userAdminTile.Controls.Add(userAdminIconHost);
            userAdminTile.CornerColor = Color.White;
            userAdminTile.Dock = DockStyle.Fill;
            userAdminTile.FillColor = Color.White;
            userAdminTile.Location = new Point(0, 0);
            userAdminTile.Margin = new Padding(0, 0, 0, 12);
            userAdminTile.Name = "userAdminTile";
            userAdminTile.Padding = new Padding(12, 8, 12, 8);
            userAdminTile.Radius = 8;
            userAdminTile.Size = new Size(180, 84);
            userAdminTile.TabIndex = 1;
            // 
            // userAdminDelta
            // 
            userAdminDelta.BackColor = Color.White;
            userAdminDelta.Dock = DockStyle.Top;
            userAdminDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userAdminDelta.ForeColor = Color.FromArgb(122, 136, 156);
            userAdminDelta.Location = new Point(0, 0);
            userAdminDelta.Name = "userAdminDelta";
            userAdminDelta.Size = new Size(110, 14);
            userAdminDelta.TabIndex = 0;
            userAdminDelta.Text = "\u2014 vs. last month";
            userAdminDelta.TextAlign = ContentAlignment.MiddleLeft;
            userAdminDelta.UseMnemonic = false;
            // 
            // userAdminValue
            // 
            userAdminValue.BackColor = Color.White;
            userAdminValue.Dock = DockStyle.Top;
            userAdminValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userAdminValue.ForeColor = Color.FromArgb(31, 42, 60);
            userAdminValue.Location = new Point(0, 0);
            userAdminValue.Name = "userAdminValue";
            userAdminValue.Size = new Size(110, 26);
            userAdminValue.TabIndex = 1;
            userAdminValue.Text = "0";
            userAdminValue.TextAlign = ContentAlignment.MiddleLeft;
            userAdminValue.UseMnemonic = false;
            // 
            // userAdminLabel
            // 
            userAdminLabel.BackColor = Color.White;
            userAdminLabel.Dock = DockStyle.Top;
            userAdminLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userAdminLabel.ForeColor = Color.FromArgb(68, 83, 106);
            userAdminLabel.Location = new Point(0, 0);
            userAdminLabel.Name = "userAdminLabel";
            userAdminLabel.Size = new Size(110, 18);
            userAdminLabel.TabIndex = 2;
            userAdminLabel.Text = "Admin";
            userAdminLabel.TextAlign = ContentAlignment.MiddleLeft;
            userAdminLabel.UseMnemonic = false;
            // 
            // userAdminIconHost
            // 
            userAdminIconHost.BackColor = Color.White;
            userAdminIconHost.Controls.Add(userAdminIcon);
            userAdminIconHost.Dock = DockStyle.Left;
            userAdminIconHost.Location = new Point(0, 0);
            userAdminIconHost.Name = "userAdminIconHost";
            userAdminIconHost.Size = new Size(44, 68);
            userAdminIconHost.TabIndex = 3;
            // 
            // userAdminIcon
            // 
            userAdminIcon.BackColor = Color.White;
            userAdminIcon.FillColor = Color.FromArgb(234, 242, 253);
            userAdminIcon.ForeColor = Color.FromArgb(58, 123, 213);
            userAdminIcon.Glyph = "\uE77B";
            userAdminIcon.GlyphSize = 11F;
            userAdminIcon.Location = new Point(0, 2);
            userAdminIcon.Name = "userAdminIcon";
            userAdminIcon.Radius = 8;
            userAdminIcon.Size = new Size(34, 34);
            userAdminIcon.TabIndex = 0;
            userAdminIcon.TabStop = false;
            // 
            // userStaffTile
            // 
            userStaffTile.BorderColor = Color.FromArgb(227, 233, 242);
            userStaffTile.Controls.Add(userStaffDelta);
            userStaffTile.Controls.Add(userStaffValue);
            userStaffTile.Controls.Add(userStaffLabel);
            userStaffTile.Controls.Add(userStaffIconHost);
            userStaffTile.CornerColor = Color.White;
            userStaffTile.Dock = DockStyle.Fill;
            userStaffTile.FillColor = Color.White;
            userStaffTile.Location = new Point(0, 0);
            userStaffTile.Margin = new Padding(0, 0, 0, 12);
            userStaffTile.Name = "userStaffTile";
            userStaffTile.Padding = new Padding(12, 8, 12, 8);
            userStaffTile.Radius = 8;
            userStaffTile.Size = new Size(180, 84);
            userStaffTile.TabIndex = 2;
            // 
            // userStaffDelta
            // 
            userStaffDelta.BackColor = Color.White;
            userStaffDelta.Dock = DockStyle.Top;
            userStaffDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userStaffDelta.ForeColor = Color.FromArgb(122, 136, 156);
            userStaffDelta.Location = new Point(0, 0);
            userStaffDelta.Name = "userStaffDelta";
            userStaffDelta.Size = new Size(110, 14);
            userStaffDelta.TabIndex = 0;
            userStaffDelta.Text = "\u2014 vs. last month";
            userStaffDelta.TextAlign = ContentAlignment.MiddleLeft;
            userStaffDelta.UseMnemonic = false;
            // 
            // userStaffValue
            // 
            userStaffValue.BackColor = Color.White;
            userStaffValue.Dock = DockStyle.Top;
            userStaffValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userStaffValue.ForeColor = Color.FromArgb(31, 42, 60);
            userStaffValue.Location = new Point(0, 0);
            userStaffValue.Name = "userStaffValue";
            userStaffValue.Size = new Size(110, 26);
            userStaffValue.TabIndex = 1;
            userStaffValue.Text = "0";
            userStaffValue.TextAlign = ContentAlignment.MiddleLeft;
            userStaffValue.UseMnemonic = false;
            // 
            // userStaffLabel
            // 
            userStaffLabel.BackColor = Color.White;
            userStaffLabel.Dock = DockStyle.Top;
            userStaffLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userStaffLabel.ForeColor = Color.FromArgb(68, 83, 106);
            userStaffLabel.Location = new Point(0, 0);
            userStaffLabel.Name = "userStaffLabel";
            userStaffLabel.Size = new Size(110, 18);
            userStaffLabel.TabIndex = 2;
            userStaffLabel.Text = "Staff";
            userStaffLabel.TextAlign = ContentAlignment.MiddleLeft;
            userStaffLabel.UseMnemonic = false;
            // 
            // userStaffIconHost
            // 
            userStaffIconHost.BackColor = Color.White;
            userStaffIconHost.Controls.Add(userStaffIcon);
            userStaffIconHost.Dock = DockStyle.Left;
            userStaffIconHost.Location = new Point(0, 0);
            userStaffIconHost.Name = "userStaffIconHost";
            userStaffIconHost.Size = new Size(44, 68);
            userStaffIconHost.TabIndex = 3;
            // 
            // userStaffIcon
            // 
            userStaffIcon.BackColor = Color.White;
            userStaffIcon.FillColor = Color.FromArgb(234, 242, 253);
            userStaffIcon.ForeColor = Color.FromArgb(58, 123, 213);
            userStaffIcon.Glyph = "\uE8FA";
            userStaffIcon.GlyphSize = 11F;
            userStaffIcon.Location = new Point(0, 2);
            userStaffIcon.Name = "userStaffIcon";
            userStaffIcon.Radius = 8;
            userStaffIcon.Size = new Size(34, 34);
            userStaffIcon.TabIndex = 0;
            userStaffIcon.TabStop = false;
            // 
            // userCustomerTile
            // 
            userCustomerTile.BorderColor = Color.FromArgb(227, 233, 242);
            userCustomerTile.Controls.Add(userCustomerDelta);
            userCustomerTile.Controls.Add(userCustomerValue);
            userCustomerTile.Controls.Add(userCustomerLabel);
            userCustomerTile.Controls.Add(userCustomerIconHost);
            userCustomerTile.CornerColor = Color.White;
            userCustomerTile.Dock = DockStyle.Fill;
            userCustomerTile.FillColor = Color.White;
            userCustomerTile.Location = new Point(0, 0);
            userCustomerTile.Margin = new Padding(0, 0, 0, 12);
            userCustomerTile.Name = "userCustomerTile";
            userCustomerTile.Padding = new Padding(12, 8, 12, 8);
            userCustomerTile.Radius = 8;
            userCustomerTile.Size = new Size(180, 84);
            userCustomerTile.TabIndex = 3;
            // 
            // userCustomerDelta
            // 
            userCustomerDelta.BackColor = Color.White;
            userCustomerDelta.Dock = DockStyle.Top;
            userCustomerDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userCustomerDelta.ForeColor = Color.FromArgb(122, 136, 156);
            userCustomerDelta.Location = new Point(0, 0);
            userCustomerDelta.Name = "userCustomerDelta";
            userCustomerDelta.Size = new Size(110, 14);
            userCustomerDelta.TabIndex = 0;
            userCustomerDelta.Text = "\u2014 vs. last month";
            userCustomerDelta.TextAlign = ContentAlignment.MiddleLeft;
            userCustomerDelta.UseMnemonic = false;
            // 
            // userCustomerValue
            // 
            userCustomerValue.BackColor = Color.White;
            userCustomerValue.Dock = DockStyle.Top;
            userCustomerValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userCustomerValue.ForeColor = Color.FromArgb(31, 42, 60);
            userCustomerValue.Location = new Point(0, 0);
            userCustomerValue.Name = "userCustomerValue";
            userCustomerValue.Size = new Size(110, 26);
            userCustomerValue.TabIndex = 1;
            userCustomerValue.Text = "0";
            userCustomerValue.TextAlign = ContentAlignment.MiddleLeft;
            userCustomerValue.UseMnemonic = false;
            // 
            // userCustomerLabel
            // 
            userCustomerLabel.BackColor = Color.White;
            userCustomerLabel.Dock = DockStyle.Top;
            userCustomerLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userCustomerLabel.ForeColor = Color.FromArgb(68, 83, 106);
            userCustomerLabel.Location = new Point(0, 0);
            userCustomerLabel.Name = "userCustomerLabel";
            userCustomerLabel.Size = new Size(110, 18);
            userCustomerLabel.TabIndex = 2;
            userCustomerLabel.Text = "Customer";
            userCustomerLabel.TextAlign = ContentAlignment.MiddleLeft;
            userCustomerLabel.UseMnemonic = false;
            // 
            // userCustomerIconHost
            // 
            userCustomerIconHost.BackColor = Color.White;
            userCustomerIconHost.Controls.Add(userCustomerIcon);
            userCustomerIconHost.Dock = DockStyle.Left;
            userCustomerIconHost.Location = new Point(0, 0);
            userCustomerIconHost.Name = "userCustomerIconHost";
            userCustomerIconHost.Size = new Size(44, 68);
            userCustomerIconHost.TabIndex = 3;
            // 
            // userCustomerIcon
            // 
            userCustomerIcon.BackColor = Color.White;
            userCustomerIcon.FillColor = Color.FromArgb(234, 242, 253);
            userCustomerIcon.ForeColor = Color.FromArgb(58, 123, 213);
            userCustomerIcon.Glyph = "\uE716";
            userCustomerIcon.GlyphSize = 11F;
            userCustomerIcon.Location = new Point(0, 2);
            userCustomerIcon.Name = "userCustomerIcon";
            userCustomerIcon.Radius = 8;
            userCustomerIcon.Size = new Size(34, 34);
            userCustomerIcon.TabIndex = 0;
            userCustomerIcon.TabStop = false;
            // 
            // userHeader
            // 
            userHeader.BackColor = Color.White;
            userHeader.Controls.Add(userHeaderText);
            userHeader.Controls.Add(userHeaderIconHost);
            userHeader.Controls.Add(userAddHost);
            userHeader.Dock = DockStyle.Top;
            userHeader.Location = new Point(0, 0);
            userHeader.Name = "userHeader";
            userHeader.Size = new Size(956, 60);
            userHeader.TabIndex = 4;
            // 
            // userHeaderText
            // 
            userHeaderText.BackColor = Color.White;
            userHeaderText.Controls.Add(userSubtitle);
            userHeaderText.Controls.Add(userTitle);
            userHeaderText.Dock = DockStyle.Fill;
            userHeaderText.Location = new Point(0, 0);
            userHeaderText.Name = "userHeaderText";
            userHeaderText.Padding = new Padding(0, 8, 0, 0);
            userHeaderText.Size = new Size(760, 60);
            userHeaderText.TabIndex = 0;
            // 
            // userSubtitle
            // 
            userSubtitle.BackColor = Color.White;
            userSubtitle.Dock = DockStyle.Top;
            userSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userSubtitle.ForeColor = Color.FromArgb(122, 136, 156);
            userSubtitle.Location = new Point(0, 0);
            userSubtitle.Name = "userSubtitle";
            userSubtitle.Size = new Size(760, 18);
            userSubtitle.TabIndex = 0;
            userSubtitle.Text = "Manage system users and their access levels.";
            userSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            userSubtitle.UseMnemonic = false;
            // 
            // userTitle
            // 
            userTitle.BackColor = Color.White;
            userTitle.Dock = DockStyle.Top;
            userTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userTitle.ForeColor = Color.FromArgb(31, 42, 60);
            userTitle.Location = new Point(0, 0);
            userTitle.Name = "userTitle";
            userTitle.Size = new Size(760, 24);
            userTitle.TabIndex = 1;
            userTitle.Text = "User Management";
            userTitle.TextAlign = ContentAlignment.MiddleLeft;
            userTitle.UseMnemonic = false;
            // 
            // userHeaderIconHost
            // 
            userHeaderIconHost.BackColor = Color.White;
            userHeaderIconHost.Controls.Add(userHeaderIcon);
            userHeaderIconHost.Dock = DockStyle.Left;
            userHeaderIconHost.Location = new Point(0, 0);
            userHeaderIconHost.Name = "userHeaderIconHost";
            userHeaderIconHost.Size = new Size(48, 60);
            userHeaderIconHost.TabIndex = 1;
            // 
            // userHeaderIcon
            // 
            userHeaderIcon.BackColor = Color.White;
            userHeaderIcon.FillColor = Color.FromArgb(234, 242, 253);
            userHeaderIcon.ForeColor = Color.FromArgb(58, 123, 213);
            userHeaderIcon.Glyph = "\uE77B";
            userHeaderIcon.GlyphSize = 12F;
            userHeaderIcon.Location = new Point(0, 8);
            userHeaderIcon.Name = "userHeaderIcon";
            userHeaderIcon.Radius = 8;
            userHeaderIcon.Size = new Size(36, 36);
            userHeaderIcon.TabIndex = 0;
            userHeaderIcon.TabStop = false;
            // 
            // userAddHost
            // 
            userAddHost.BackColor = Color.White;
            userAddHost.Controls.Add(userAddButton);
            userAddHost.Dock = DockStyle.Right;
            userAddHost.Location = new Point(0, 0);
            userAddHost.Name = "userAddHost";
            userAddHost.Size = new Size(148, 60);
            userAddHost.TabIndex = 2;
            // 
            // userAddButton
            // 
            userAddButton.Anchor = AnchorStyles.Right;
            userAddButton.BackColor = Color.White;
            userAddButton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            userAddButton.Location = new Point(16, 12);
            userAddButton.Name = "userAddButton";
            userAddButton.Size = new Size(132, 36);
            userAddButton.TabIndex = 0;
            userAddButton.Text = "+ Add User";
            // 
            // sectionGap
            // 
            sectionGap.BackColor = Color.FromArgb(243, 247, 252);
            sectionGap.Dock = DockStyle.Top;
            sectionGap.Location = new Point(0, 520);
            sectionGap.Name = "sectionGap";
            sectionGap.Size = new Size(996, 20);
            sectionGap.TabIndex = 1;
            // 
            // employeeSection
            // 
            employeeSection.BorderColor = Color.FromArgb(227, 233, 242);
            employeeSection.Controls.Add(employeeTable);
            employeeSection.Controls.Add(employeeFooter);
            employeeSection.Controls.Add(employeeToolbar);
            employeeSection.Controls.Add(employeeStats);
            employeeSection.Controls.Add(employeeHeader);
            employeeSection.CornerColor = Color.FromArgb(243, 247, 252);
            employeeSection.Dock = DockStyle.Top;
            employeeSection.FillColor = Color.White;
            employeeSection.Location = new Point(0, 0);
            employeeSection.Name = "employeeSection";
            employeeSection.Padding = new Padding(20, 16, 20, 12);
            employeeSection.Radius = 10;
            employeeSection.Size = new Size(996, 520);
            employeeSection.TabIndex = 2;
            // 
            // employeeTable
            // 
            employeeTable.BorderColor = Color.FromArgb(227, 233, 242);
            employeeTable.Controls.Add(employeeRows);
            employeeTable.Controls.Add(employeeColumnsHead);
            employeeTable.CornerColor = Color.White;
            employeeTable.Dock = DockStyle.Fill;
            employeeTable.FillColor = Color.White;
            employeeTable.Location = new Point(0, 0);
            employeeTable.Name = "employeeTable";
            employeeTable.Padding = new Padding(1, 1, 1, 1);
            employeeTable.Radius = 8;
            employeeTable.Size = new Size(956, 236);
            employeeTable.TabIndex = 0;
            // 
            // employeeRows
            // 
            employeeRows.AutoScroll = true;
            employeeRows.BackColor = Color.White;
            employeeRows.Controls.Add(employeeEmpty);
            employeeRows.Dock = DockStyle.Fill;
            employeeRows.Location = new Point(0, 34);
            employeeRows.Name = "employeeRows";
            employeeRows.Size = new Size(954, 200);
            employeeRows.TabIndex = 0;
            // 
            // employeeEmpty
            // 
            employeeEmpty.BackColor = Color.White;
            employeeEmpty.BadgeFill = Color.FromArgb(234, 242, 253);
            employeeEmpty.BadgeFore = Color.FromArgb(58, 123, 213);
            employeeEmpty.Dock = DockStyle.Fill;
            employeeEmpty.Glyph = "\uE716";
            employeeEmpty.Hint = "Employee records will appear here once they are added.";
            employeeEmpty.Location = new Point(0, 0);
            employeeEmpty.Name = "employeeEmpty";
            employeeEmpty.Size = new Size(954, 200);
            employeeEmpty.TabIndex = 0;
            employeeEmpty.TabStop = false;
            employeeEmpty.Title = "No employees found";
            // 
            // employeeColumnsHead
            // 
            employeeColumnsHead.BackColor = Color.FromArgb(246, 248, 251);
            employeeColumnsHead.Controls.Add(employeeColumnsGrid);
            employeeColumnsHead.Dock = DockStyle.Top;
            employeeColumnsHead.Location = new Point(0, 0);
            employeeColumnsHead.Name = "employeeColumnsHead";
            employeeColumnsHead.Size = new Size(954, 34);
            employeeColumnsHead.TabIndex = 1;
            // 
            // employeeColumnsGrid
            // 
            employeeColumnsGrid.BackColor = Color.FromArgb(246, 248, 251);
            employeeColumnsGrid.ColumnCount = 8;
            employeeColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            employeeColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 84F));
            employeeColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            employeeColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            employeeColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            employeeColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            employeeColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            employeeColumnsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            employeeColumnsGrid.Controls.Add(employeeHeaderCheck, 0, 0);
            employeeColumnsGrid.Controls.Add(employeeColId, 1, 0);
            employeeColumnsGrid.Controls.Add(employeeColName, 2, 0);
            employeeColumnsGrid.Controls.Add(employeeColRole, 3, 0);
            employeeColumnsGrid.Controls.Add(employeeColDepartment, 4, 0);
            employeeColumnsGrid.Controls.Add(employeeColEmail, 5, 0);
            employeeColumnsGrid.Controls.Add(employeeColStatus, 6, 0);
            employeeColumnsGrid.Controls.Add(employeeColActions, 7, 0);
            employeeColumnsGrid.Dock = DockStyle.Fill;
            employeeColumnsGrid.Location = new Point(0, 0);
            employeeColumnsGrid.Margin = new Padding(0, 0, 0, 0);
            employeeColumnsGrid.Name = "employeeColumnsGrid";
            employeeColumnsGrid.RowCount = 1;
            employeeColumnsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            employeeColumnsGrid.Size = new Size(954, 34);
            employeeColumnsGrid.TabIndex = 0;
            // 
            // employeeHeaderCheck
            // 
            employeeHeaderCheck.Anchor = AnchorStyles.None;
            employeeHeaderCheck.BackColor = Color.FromArgb(246, 248, 251);
            employeeHeaderCheck.Location = new Point(13, 8);
            employeeHeaderCheck.Name = "employeeHeaderCheck";
            employeeHeaderCheck.Size = new Size(18, 18);
            employeeHeaderCheck.TabIndex = 0;
            employeeHeaderCheck.TabStop = false;
            // 
            // employeeColId
            // 
            employeeColId.BackColor = Color.FromArgb(246, 248, 251);
            employeeColId.Dock = DockStyle.Fill;
            employeeColId.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeColId.ForeColor = Color.FromArgb(122, 136, 156);
            employeeColId.Location = new Point(0, 0);
            employeeColId.Margin = new Padding(0, 0, 0, 0);
            employeeColId.Name = "employeeColId";
            employeeColId.Padding = new Padding(8, 0, 0, 0);
            employeeColId.Size = new Size(80, 34);
            employeeColId.TabIndex = 1;
            employeeColId.Text = "ID";
            employeeColId.TextAlign = ContentAlignment.MiddleLeft;
            employeeColId.UseMnemonic = false;
            // 
            // employeeColName
            // 
            employeeColName.BackColor = Color.FromArgb(246, 248, 251);
            employeeColName.Dock = DockStyle.Fill;
            employeeColName.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeColName.ForeColor = Color.FromArgb(122, 136, 156);
            employeeColName.Location = new Point(0, 0);
            employeeColName.Margin = new Padding(0, 0, 0, 0);
            employeeColName.Name = "employeeColName";
            employeeColName.Padding = new Padding(8, 0, 0, 0);
            employeeColName.Size = new Size(80, 34);
            employeeColName.TabIndex = 2;
            employeeColName.Text = "NAME";
            employeeColName.TextAlign = ContentAlignment.MiddleLeft;
            employeeColName.UseMnemonic = false;
            // 
            // employeeColRole
            // 
            employeeColRole.BackColor = Color.FromArgb(246, 248, 251);
            employeeColRole.Dock = DockStyle.Fill;
            employeeColRole.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeColRole.ForeColor = Color.FromArgb(122, 136, 156);
            employeeColRole.Location = new Point(0, 0);
            employeeColRole.Margin = new Padding(0, 0, 0, 0);
            employeeColRole.Name = "employeeColRole";
            employeeColRole.Padding = new Padding(8, 0, 0, 0);
            employeeColRole.Size = new Size(80, 34);
            employeeColRole.TabIndex = 3;
            employeeColRole.Text = "ROLE";
            employeeColRole.TextAlign = ContentAlignment.MiddleLeft;
            employeeColRole.UseMnemonic = false;
            // 
            // employeeColDepartment
            // 
            employeeColDepartment.BackColor = Color.FromArgb(246, 248, 251);
            employeeColDepartment.Dock = DockStyle.Fill;
            employeeColDepartment.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeColDepartment.ForeColor = Color.FromArgb(122, 136, 156);
            employeeColDepartment.Location = new Point(0, 0);
            employeeColDepartment.Margin = new Padding(0, 0, 0, 0);
            employeeColDepartment.Name = "employeeColDepartment";
            employeeColDepartment.Padding = new Padding(8, 0, 0, 0);
            employeeColDepartment.Size = new Size(80, 34);
            employeeColDepartment.TabIndex = 4;
            employeeColDepartment.Text = "DEPARTMENT";
            employeeColDepartment.TextAlign = ContentAlignment.MiddleLeft;
            employeeColDepartment.UseMnemonic = false;
            // 
            // employeeColEmail
            // 
            employeeColEmail.BackColor = Color.FromArgb(246, 248, 251);
            employeeColEmail.Dock = DockStyle.Fill;
            employeeColEmail.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeColEmail.ForeColor = Color.FromArgb(122, 136, 156);
            employeeColEmail.Location = new Point(0, 0);
            employeeColEmail.Margin = new Padding(0, 0, 0, 0);
            employeeColEmail.Name = "employeeColEmail";
            employeeColEmail.Padding = new Padding(8, 0, 0, 0);
            employeeColEmail.Size = new Size(80, 34);
            employeeColEmail.TabIndex = 5;
            employeeColEmail.Text = "EMAIL";
            employeeColEmail.TextAlign = ContentAlignment.MiddleLeft;
            employeeColEmail.UseMnemonic = false;
            // 
            // employeeColStatus
            // 
            employeeColStatus.BackColor = Color.FromArgb(246, 248, 251);
            employeeColStatus.Dock = DockStyle.Fill;
            employeeColStatus.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeColStatus.ForeColor = Color.FromArgb(122, 136, 156);
            employeeColStatus.Location = new Point(0, 0);
            employeeColStatus.Margin = new Padding(0, 0, 0, 0);
            employeeColStatus.Name = "employeeColStatus";
            employeeColStatus.Padding = new Padding(8, 0, 0, 0);
            employeeColStatus.Size = new Size(80, 34);
            employeeColStatus.TabIndex = 6;
            employeeColStatus.Text = "STATUS";
            employeeColStatus.TextAlign = ContentAlignment.MiddleLeft;
            employeeColStatus.UseMnemonic = false;
            // 
            // employeeColActions
            // 
            employeeColActions.BackColor = Color.FromArgb(246, 248, 251);
            employeeColActions.Dock = DockStyle.Fill;
            employeeColActions.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeColActions.ForeColor = Color.FromArgb(122, 136, 156);
            employeeColActions.Location = new Point(0, 0);
            employeeColActions.Margin = new Padding(0, 0, 0, 0);
            employeeColActions.Name = "employeeColActions";
            employeeColActions.Padding = new Padding(8, 0, 0, 0);
            employeeColActions.Size = new Size(80, 34);
            employeeColActions.TabIndex = 7;
            employeeColActions.Text = "ACTIONS";
            employeeColActions.TextAlign = ContentAlignment.MiddleLeft;
            employeeColActions.UseMnemonic = false;
            // 
            // employeeFooter
            // 
            employeeFooter.BackColor = Color.White;
            employeeFooter.Controls.Add(employeeShowing);
            employeeFooter.Controls.Add(employeePager);
            employeeFooter.Dock = DockStyle.Bottom;
            employeeFooter.Location = new Point(0, 0);
            employeeFooter.Name = "employeeFooter";
            employeeFooter.Size = new Size(956, 44);
            employeeFooter.TabIndex = 1;
            // 
            // employeeShowing
            // 
            employeeShowing.BackColor = Color.White;
            employeeShowing.Dock = DockStyle.Fill;
            employeeShowing.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeShowing.ForeColor = Color.FromArgb(122, 136, 156);
            employeeShowing.Location = new Point(0, 0);
            employeeShowing.Name = "employeeShowing";
            employeeShowing.Size = new Size(600, 44);
            employeeShowing.TabIndex = 0;
            employeeShowing.Text = "Showing 0 employees";
            employeeShowing.TextAlign = ContentAlignment.MiddleLeft;
            employeeShowing.UseMnemonic = false;
            // 
            // employeePager
            // 
            employeePager.BackColor = Color.White;
            employeePager.Controls.Add(employeePrevPage);
            employeePager.Controls.Add(employeePageChip);
            employeePager.Controls.Add(employeeNextPage);
            employeePager.Dock = DockStyle.Right;
            employeePager.Location = new Point(0, 0);
            employeePager.Name = "employeePager";
            employeePager.Size = new Size(116, 44);
            employeePager.TabIndex = 1;
            // 
            // employeePrevPage
            // 
            employeePrevPage.BackColor = Color.White;
            employeePrevPage.Enabled = false;
            employeePrevPage.Glyph = "\uE76B";
            employeePrevPage.Location = new Point(4, 7);
            employeePrevPage.Name = "employeePrevPage";
            employeePrevPage.Size = new Size(30, 30);
            employeePrevPage.TabIndex = 0;
            // 
            // employeePageChip
            // 
            employeePageChip.BackColor = Color.White;
            employeePageChip.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeePageChip.Location = new Point(42, 7);
            employeePageChip.Name = "employeePageChip";
            employeePageChip.Size = new Size(30, 30);
            employeePageChip.TabIndex = 1;
            employeePageChip.Text = "1";
            // 
            // employeeNextPage
            // 
            employeeNextPage.BackColor = Color.White;
            employeeNextPage.Enabled = false;
            employeeNextPage.Glyph = "\uE76C";
            employeeNextPage.Location = new Point(80, 7);
            employeeNextPage.Name = "employeeNextPage";
            employeeNextPage.Size = new Size(30, 30);
            employeeNextPage.TabIndex = 2;
            // 
            // employeeToolbar
            // 
            employeeToolbar.BackColor = Color.White;
            employeeToolbar.Controls.Add(employeeSearch);
            employeeToolbar.Controls.Add(employeeToolbarGapA);
            employeeToolbar.Controls.Add(employeeRoleFilter);
            employeeToolbar.Controls.Add(employeeToolbarGapB);
            employeeToolbar.Controls.Add(employeeStatusFilter);
            employeeToolbar.Dock = DockStyle.Top;
            employeeToolbar.Location = new Point(0, 0);
            employeeToolbar.Name = "employeeToolbar";
            employeeToolbar.Padding = new Padding(0, 4, 0, 8);
            employeeToolbar.Size = new Size(956, 52);
            employeeToolbar.TabIndex = 2;
            // 
            // employeeSearch
            // 
            employeeSearch.BorderColor = Color.FromArgb(227, 233, 242);
            employeeSearch.CornerColor = Color.White;
            employeeSearch.Dock = DockStyle.Fill;
            employeeSearch.FillColor = Color.White;
            employeeSearch.Glyph = "\uE721";
            employeeSearch.Location = new Point(0, 4);
            employeeSearch.Name = "employeeSearch";
            employeeSearch.Padding = new Padding(10, 3, 10, 3);
            employeeSearch.Placeholder = "Search employees...";
            employeeSearch.Radius = 8;
            employeeSearch.Size = new Size(560, 40);
            employeeSearch.TabIndex = 0;
            // 
            // employeeToolbarGapA
            // 
            employeeToolbarGapA.BackColor = Color.White;
            employeeToolbarGapA.Dock = DockStyle.Right;
            employeeToolbarGapA.Location = new Point(0, 0);
            employeeToolbarGapA.Name = "employeeToolbarGapA";
            employeeToolbarGapA.Size = new Size(12, 40);
            employeeToolbarGapA.TabIndex = 1;
            // 
            // employeeRoleFilter
            // 
            employeeRoleFilter.BackColor = Color.White;
            employeeRoleFilter.Dock = DockStyle.Right;
            employeeRoleFilter.Location = new Point(0, 0);
            employeeRoleFilter.Name = "employeeRoleFilter";
            employeeRoleFilter.Size = new Size(140, 40);
            employeeRoleFilter.TabIndex = 2;
            employeeRoleFilter.Text = "All Roles";
            // 
            // employeeToolbarGapB
            // 
            employeeToolbarGapB.BackColor = Color.White;
            employeeToolbarGapB.Dock = DockStyle.Right;
            employeeToolbarGapB.Location = new Point(0, 0);
            employeeToolbarGapB.Name = "employeeToolbarGapB";
            employeeToolbarGapB.Size = new Size(10, 40);
            employeeToolbarGapB.TabIndex = 3;
            // 
            // employeeStatusFilter
            // 
            employeeStatusFilter.BackColor = Color.White;
            employeeStatusFilter.Dock = DockStyle.Right;
            employeeStatusFilter.Location = new Point(0, 0);
            employeeStatusFilter.Name = "employeeStatusFilter";
            employeeStatusFilter.Size = new Size(140, 40);
            employeeStatusFilter.TabIndex = 4;
            employeeStatusFilter.Text = "All Status";
            // 
            // employeeStats
            // 
            employeeStats.BackColor = Color.White;
            employeeStats.ColumnCount = 9;
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            employeeStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            employeeStats.Controls.Add(employeeTotalTile, 0, 0);
            employeeStats.Controls.Add(employeeAdminTile, 2, 0);
            employeeStats.Controls.Add(employeeCashierTile, 4, 0);
            employeeStats.Controls.Add(employeeFrontDeskTile, 6, 0);
            employeeStats.Controls.Add(employeeGroomersTile, 8, 0);
            employeeStats.Dock = DockStyle.Top;
            employeeStats.Location = new Point(0, 0);
            employeeStats.Margin = new Padding(0, 0, 0, 0);
            employeeStats.Name = "employeeStats";
            employeeStats.RowCount = 1;
            employeeStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            employeeStats.Size = new Size(956, 96);
            employeeStats.TabIndex = 3;
            // 
            // employeeTotalTile
            // 
            employeeTotalTile.BorderColor = Color.FromArgb(227, 233, 242);
            employeeTotalTile.Controls.Add(employeeTotalDelta);
            employeeTotalTile.Controls.Add(employeeTotalValue);
            employeeTotalTile.Controls.Add(employeeTotalLabel);
            employeeTotalTile.Controls.Add(employeeTotalIconHost);
            employeeTotalTile.CornerColor = Color.White;
            employeeTotalTile.Dock = DockStyle.Fill;
            employeeTotalTile.FillColor = Color.White;
            employeeTotalTile.Location = new Point(0, 0);
            employeeTotalTile.Margin = new Padding(0, 0, 0, 12);
            employeeTotalTile.Name = "employeeTotalTile";
            employeeTotalTile.Padding = new Padding(12, 8, 12, 8);
            employeeTotalTile.Radius = 8;
            employeeTotalTile.Size = new Size(180, 84);
            employeeTotalTile.TabIndex = 0;
            // 
            // employeeTotalDelta
            // 
            employeeTotalDelta.BackColor = Color.White;
            employeeTotalDelta.Dock = DockStyle.Top;
            employeeTotalDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeTotalDelta.ForeColor = Color.FromArgb(122, 136, 156);
            employeeTotalDelta.Location = new Point(0, 0);
            employeeTotalDelta.Name = "employeeTotalDelta";
            employeeTotalDelta.Size = new Size(110, 14);
            employeeTotalDelta.TabIndex = 0;
            employeeTotalDelta.Text = "\u2014 vs. last month";
            employeeTotalDelta.TextAlign = ContentAlignment.MiddleLeft;
            employeeTotalDelta.UseMnemonic = false;
            // 
            // employeeTotalValue
            // 
            employeeTotalValue.BackColor = Color.White;
            employeeTotalValue.Dock = DockStyle.Top;
            employeeTotalValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeTotalValue.ForeColor = Color.FromArgb(31, 42, 60);
            employeeTotalValue.Location = new Point(0, 0);
            employeeTotalValue.Name = "employeeTotalValue";
            employeeTotalValue.Size = new Size(110, 26);
            employeeTotalValue.TabIndex = 1;
            employeeTotalValue.Text = "0";
            employeeTotalValue.TextAlign = ContentAlignment.MiddleLeft;
            employeeTotalValue.UseMnemonic = false;
            // 
            // employeeTotalLabel
            // 
            employeeTotalLabel.BackColor = Color.White;
            employeeTotalLabel.Dock = DockStyle.Top;
            employeeTotalLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeTotalLabel.ForeColor = Color.FromArgb(68, 83, 106);
            employeeTotalLabel.Location = new Point(0, 0);
            employeeTotalLabel.Name = "employeeTotalLabel";
            employeeTotalLabel.Size = new Size(110, 18);
            employeeTotalLabel.TabIndex = 2;
            employeeTotalLabel.Text = "Total Employees";
            employeeTotalLabel.TextAlign = ContentAlignment.MiddleLeft;
            employeeTotalLabel.UseMnemonic = false;
            // 
            // employeeTotalIconHost
            // 
            employeeTotalIconHost.BackColor = Color.White;
            employeeTotalIconHost.Controls.Add(employeeTotalIcon);
            employeeTotalIconHost.Dock = DockStyle.Left;
            employeeTotalIconHost.Location = new Point(0, 0);
            employeeTotalIconHost.Name = "employeeTotalIconHost";
            employeeTotalIconHost.Size = new Size(44, 68);
            employeeTotalIconHost.TabIndex = 3;
            // 
            // employeeTotalIcon
            // 
            employeeTotalIcon.BackColor = Color.White;
            employeeTotalIcon.FillColor = Color.FromArgb(234, 242, 253);
            employeeTotalIcon.ForeColor = Color.FromArgb(58, 123, 213);
            employeeTotalIcon.Glyph = "\uE716";
            employeeTotalIcon.GlyphSize = 11F;
            employeeTotalIcon.Location = new Point(0, 2);
            employeeTotalIcon.Name = "employeeTotalIcon";
            employeeTotalIcon.Radius = 8;
            employeeTotalIcon.Size = new Size(34, 34);
            employeeTotalIcon.TabIndex = 0;
            employeeTotalIcon.TabStop = false;
            // 
            // employeeAdminTile
            // 
            employeeAdminTile.BorderColor = Color.FromArgb(227, 233, 242);
            employeeAdminTile.Controls.Add(employeeAdminDelta);
            employeeAdminTile.Controls.Add(employeeAdminValue);
            employeeAdminTile.Controls.Add(employeeAdminLabel);
            employeeAdminTile.Controls.Add(employeeAdminIconHost);
            employeeAdminTile.CornerColor = Color.White;
            employeeAdminTile.Dock = DockStyle.Fill;
            employeeAdminTile.FillColor = Color.White;
            employeeAdminTile.Location = new Point(0, 0);
            employeeAdminTile.Margin = new Padding(0, 0, 0, 12);
            employeeAdminTile.Name = "employeeAdminTile";
            employeeAdminTile.Padding = new Padding(12, 8, 12, 8);
            employeeAdminTile.Radius = 8;
            employeeAdminTile.Size = new Size(180, 84);
            employeeAdminTile.TabIndex = 1;
            // 
            // employeeAdminDelta
            // 
            employeeAdminDelta.BackColor = Color.White;
            employeeAdminDelta.Dock = DockStyle.Top;
            employeeAdminDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeAdminDelta.ForeColor = Color.FromArgb(122, 136, 156);
            employeeAdminDelta.Location = new Point(0, 0);
            employeeAdminDelta.Name = "employeeAdminDelta";
            employeeAdminDelta.Size = new Size(110, 14);
            employeeAdminDelta.TabIndex = 0;
            employeeAdminDelta.Text = "\u2014 vs. last month";
            employeeAdminDelta.TextAlign = ContentAlignment.MiddleLeft;
            employeeAdminDelta.UseMnemonic = false;
            // 
            // employeeAdminValue
            // 
            employeeAdminValue.BackColor = Color.White;
            employeeAdminValue.Dock = DockStyle.Top;
            employeeAdminValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeAdminValue.ForeColor = Color.FromArgb(31, 42, 60);
            employeeAdminValue.Location = new Point(0, 0);
            employeeAdminValue.Name = "employeeAdminValue";
            employeeAdminValue.Size = new Size(110, 26);
            employeeAdminValue.TabIndex = 1;
            employeeAdminValue.Text = "0";
            employeeAdminValue.TextAlign = ContentAlignment.MiddleLeft;
            employeeAdminValue.UseMnemonic = false;
            // 
            // employeeAdminLabel
            // 
            employeeAdminLabel.BackColor = Color.White;
            employeeAdminLabel.Dock = DockStyle.Top;
            employeeAdminLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeAdminLabel.ForeColor = Color.FromArgb(68, 83, 106);
            employeeAdminLabel.Location = new Point(0, 0);
            employeeAdminLabel.Name = "employeeAdminLabel";
            employeeAdminLabel.Size = new Size(110, 18);
            employeeAdminLabel.TabIndex = 2;
            employeeAdminLabel.Text = "Admin";
            employeeAdminLabel.TextAlign = ContentAlignment.MiddleLeft;
            employeeAdminLabel.UseMnemonic = false;
            // 
            // employeeAdminIconHost
            // 
            employeeAdminIconHost.BackColor = Color.White;
            employeeAdminIconHost.Controls.Add(employeeAdminIcon);
            employeeAdminIconHost.Dock = DockStyle.Left;
            employeeAdminIconHost.Location = new Point(0, 0);
            employeeAdminIconHost.Name = "employeeAdminIconHost";
            employeeAdminIconHost.Size = new Size(44, 68);
            employeeAdminIconHost.TabIndex = 3;
            // 
            // employeeAdminIcon
            // 
            employeeAdminIcon.BackColor = Color.White;
            employeeAdminIcon.FillColor = Color.FromArgb(234, 242, 253);
            employeeAdminIcon.ForeColor = Color.FromArgb(58, 123, 213);
            employeeAdminIcon.Glyph = "\uE77B";
            employeeAdminIcon.GlyphSize = 11F;
            employeeAdminIcon.Location = new Point(0, 2);
            employeeAdminIcon.Name = "employeeAdminIcon";
            employeeAdminIcon.Radius = 8;
            employeeAdminIcon.Size = new Size(34, 34);
            employeeAdminIcon.TabIndex = 0;
            employeeAdminIcon.TabStop = false;
            // 
            // employeeCashierTile
            // 
            employeeCashierTile.BorderColor = Color.FromArgb(227, 233, 242);
            employeeCashierTile.Controls.Add(employeeCashierDelta);
            employeeCashierTile.Controls.Add(employeeCashierValue);
            employeeCashierTile.Controls.Add(employeeCashierLabel);
            employeeCashierTile.Controls.Add(employeeCashierIconHost);
            employeeCashierTile.CornerColor = Color.White;
            employeeCashierTile.Dock = DockStyle.Fill;
            employeeCashierTile.FillColor = Color.White;
            employeeCashierTile.Location = new Point(0, 0);
            employeeCashierTile.Margin = new Padding(0, 0, 0, 12);
            employeeCashierTile.Name = "employeeCashierTile";
            employeeCashierTile.Padding = new Padding(12, 8, 12, 8);
            employeeCashierTile.Radius = 8;
            employeeCashierTile.Size = new Size(180, 84);
            employeeCashierTile.TabIndex = 2;
            // 
            // employeeCashierDelta
            // 
            employeeCashierDelta.BackColor = Color.White;
            employeeCashierDelta.Dock = DockStyle.Top;
            employeeCashierDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeCashierDelta.ForeColor = Color.FromArgb(122, 136, 156);
            employeeCashierDelta.Location = new Point(0, 0);
            employeeCashierDelta.Name = "employeeCashierDelta";
            employeeCashierDelta.Size = new Size(110, 14);
            employeeCashierDelta.TabIndex = 0;
            employeeCashierDelta.Text = "\u2014 vs. last month";
            employeeCashierDelta.TextAlign = ContentAlignment.MiddleLeft;
            employeeCashierDelta.UseMnemonic = false;
            // 
            // employeeCashierValue
            // 
            employeeCashierValue.BackColor = Color.White;
            employeeCashierValue.Dock = DockStyle.Top;
            employeeCashierValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeCashierValue.ForeColor = Color.FromArgb(31, 42, 60);
            employeeCashierValue.Location = new Point(0, 0);
            employeeCashierValue.Name = "employeeCashierValue";
            employeeCashierValue.Size = new Size(110, 26);
            employeeCashierValue.TabIndex = 1;
            employeeCashierValue.Text = "0";
            employeeCashierValue.TextAlign = ContentAlignment.MiddleLeft;
            employeeCashierValue.UseMnemonic = false;
            // 
            // employeeCashierLabel
            // 
            employeeCashierLabel.BackColor = Color.White;
            employeeCashierLabel.Dock = DockStyle.Top;
            employeeCashierLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeCashierLabel.ForeColor = Color.FromArgb(68, 83, 106);
            employeeCashierLabel.Location = new Point(0, 0);
            employeeCashierLabel.Name = "employeeCashierLabel";
            employeeCashierLabel.Size = new Size(110, 18);
            employeeCashierLabel.TabIndex = 2;
            employeeCashierLabel.Text = "Cashier";
            employeeCashierLabel.TextAlign = ContentAlignment.MiddleLeft;
            employeeCashierLabel.UseMnemonic = false;
            // 
            // employeeCashierIconHost
            // 
            employeeCashierIconHost.BackColor = Color.White;
            employeeCashierIconHost.Controls.Add(employeeCashierIcon);
            employeeCashierIconHost.Dock = DockStyle.Left;
            employeeCashierIconHost.Location = new Point(0, 0);
            employeeCashierIconHost.Name = "employeeCashierIconHost";
            employeeCashierIconHost.Size = new Size(44, 68);
            employeeCashierIconHost.TabIndex = 3;
            // 
            // employeeCashierIcon
            // 
            employeeCashierIcon.BackColor = Color.White;
            employeeCashierIcon.FillColor = Color.FromArgb(234, 242, 253);
            employeeCashierIcon.ForeColor = Color.FromArgb(58, 123, 213);
            employeeCashierIcon.Glyph = "\uE8C7";
            employeeCashierIcon.GlyphSize = 11F;
            employeeCashierIcon.Location = new Point(0, 2);
            employeeCashierIcon.Name = "employeeCashierIcon";
            employeeCashierIcon.Radius = 8;
            employeeCashierIcon.Size = new Size(34, 34);
            employeeCashierIcon.TabIndex = 0;
            employeeCashierIcon.TabStop = false;
            // 
            // employeeFrontDeskTile
            // 
            employeeFrontDeskTile.BorderColor = Color.FromArgb(227, 233, 242);
            employeeFrontDeskTile.Controls.Add(employeeFrontDeskDelta);
            employeeFrontDeskTile.Controls.Add(employeeFrontDeskValue);
            employeeFrontDeskTile.Controls.Add(employeeFrontDeskLabel);
            employeeFrontDeskTile.Controls.Add(employeeFrontDeskIconHost);
            employeeFrontDeskTile.CornerColor = Color.White;
            employeeFrontDeskTile.Dock = DockStyle.Fill;
            employeeFrontDeskTile.FillColor = Color.White;
            employeeFrontDeskTile.Location = new Point(0, 0);
            employeeFrontDeskTile.Margin = new Padding(0, 0, 0, 12);
            employeeFrontDeskTile.Name = "employeeFrontDeskTile";
            employeeFrontDeskTile.Padding = new Padding(12, 8, 12, 8);
            employeeFrontDeskTile.Radius = 8;
            employeeFrontDeskTile.Size = new Size(180, 84);
            employeeFrontDeskTile.TabIndex = 3;
            // 
            // employeeFrontDeskDelta
            // 
            employeeFrontDeskDelta.BackColor = Color.White;
            employeeFrontDeskDelta.Dock = DockStyle.Top;
            employeeFrontDeskDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeFrontDeskDelta.ForeColor = Color.FromArgb(122, 136, 156);
            employeeFrontDeskDelta.Location = new Point(0, 0);
            employeeFrontDeskDelta.Name = "employeeFrontDeskDelta";
            employeeFrontDeskDelta.Size = new Size(110, 14);
            employeeFrontDeskDelta.TabIndex = 0;
            employeeFrontDeskDelta.Text = "\u2014 vs. last month";
            employeeFrontDeskDelta.TextAlign = ContentAlignment.MiddleLeft;
            employeeFrontDeskDelta.UseMnemonic = false;
            // 
            // employeeFrontDeskValue
            // 
            employeeFrontDeskValue.BackColor = Color.White;
            employeeFrontDeskValue.Dock = DockStyle.Top;
            employeeFrontDeskValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeFrontDeskValue.ForeColor = Color.FromArgb(31, 42, 60);
            employeeFrontDeskValue.Location = new Point(0, 0);
            employeeFrontDeskValue.Name = "employeeFrontDeskValue";
            employeeFrontDeskValue.Size = new Size(110, 26);
            employeeFrontDeskValue.TabIndex = 1;
            employeeFrontDeskValue.Text = "0";
            employeeFrontDeskValue.TextAlign = ContentAlignment.MiddleLeft;
            employeeFrontDeskValue.UseMnemonic = false;
            // 
            // employeeFrontDeskLabel
            // 
            employeeFrontDeskLabel.BackColor = Color.White;
            employeeFrontDeskLabel.Dock = DockStyle.Top;
            employeeFrontDeskLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeFrontDeskLabel.ForeColor = Color.FromArgb(68, 83, 106);
            employeeFrontDeskLabel.Location = new Point(0, 0);
            employeeFrontDeskLabel.Name = "employeeFrontDeskLabel";
            employeeFrontDeskLabel.Size = new Size(110, 18);
            employeeFrontDeskLabel.TabIndex = 2;
            employeeFrontDeskLabel.Text = "Front Desk";
            employeeFrontDeskLabel.TextAlign = ContentAlignment.MiddleLeft;
            employeeFrontDeskLabel.UseMnemonic = false;
            // 
            // employeeFrontDeskIconHost
            // 
            employeeFrontDeskIconHost.BackColor = Color.White;
            employeeFrontDeskIconHost.Controls.Add(employeeFrontDeskIcon);
            employeeFrontDeskIconHost.Dock = DockStyle.Left;
            employeeFrontDeskIconHost.Location = new Point(0, 0);
            employeeFrontDeskIconHost.Name = "employeeFrontDeskIconHost";
            employeeFrontDeskIconHost.Size = new Size(44, 68);
            employeeFrontDeskIconHost.TabIndex = 3;
            // 
            // employeeFrontDeskIcon
            // 
            employeeFrontDeskIcon.BackColor = Color.White;
            employeeFrontDeskIcon.FillColor = Color.FromArgb(234, 242, 253);
            employeeFrontDeskIcon.ForeColor = Color.FromArgb(58, 123, 213);
            employeeFrontDeskIcon.Glyph = "\uE717";
            employeeFrontDeskIcon.GlyphSize = 11F;
            employeeFrontDeskIcon.Location = new Point(0, 2);
            employeeFrontDeskIcon.Name = "employeeFrontDeskIcon";
            employeeFrontDeskIcon.Radius = 8;
            employeeFrontDeskIcon.Size = new Size(34, 34);
            employeeFrontDeskIcon.TabIndex = 0;
            employeeFrontDeskIcon.TabStop = false;
            // 
            // employeeGroomersTile
            // 
            employeeGroomersTile.BorderColor = Color.FromArgb(227, 233, 242);
            employeeGroomersTile.Controls.Add(employeeGroomersDelta);
            employeeGroomersTile.Controls.Add(employeeGroomersValue);
            employeeGroomersTile.Controls.Add(employeeGroomersLabel);
            employeeGroomersTile.Controls.Add(employeeGroomersIconHost);
            employeeGroomersTile.CornerColor = Color.White;
            employeeGroomersTile.Dock = DockStyle.Fill;
            employeeGroomersTile.FillColor = Color.White;
            employeeGroomersTile.Location = new Point(0, 0);
            employeeGroomersTile.Margin = new Padding(0, 0, 0, 12);
            employeeGroomersTile.Name = "employeeGroomersTile";
            employeeGroomersTile.Padding = new Padding(12, 8, 12, 8);
            employeeGroomersTile.Radius = 8;
            employeeGroomersTile.Size = new Size(180, 84);
            employeeGroomersTile.TabIndex = 4;
            // 
            // employeeGroomersDelta
            // 
            employeeGroomersDelta.BackColor = Color.White;
            employeeGroomersDelta.Dock = DockStyle.Top;
            employeeGroomersDelta.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeGroomersDelta.ForeColor = Color.FromArgb(122, 136, 156);
            employeeGroomersDelta.Location = new Point(0, 0);
            employeeGroomersDelta.Name = "employeeGroomersDelta";
            employeeGroomersDelta.Size = new Size(110, 14);
            employeeGroomersDelta.TabIndex = 0;
            employeeGroomersDelta.Text = "\u2014 vs. last month";
            employeeGroomersDelta.TextAlign = ContentAlignment.MiddleLeft;
            employeeGroomersDelta.UseMnemonic = false;
            // 
            // employeeGroomersValue
            // 
            employeeGroomersValue.BackColor = Color.White;
            employeeGroomersValue.Dock = DockStyle.Top;
            employeeGroomersValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeGroomersValue.ForeColor = Color.FromArgb(31, 42, 60);
            employeeGroomersValue.Location = new Point(0, 0);
            employeeGroomersValue.Name = "employeeGroomersValue";
            employeeGroomersValue.Size = new Size(110, 26);
            employeeGroomersValue.TabIndex = 1;
            employeeGroomersValue.Text = "0";
            employeeGroomersValue.TextAlign = ContentAlignment.MiddleLeft;
            employeeGroomersValue.UseMnemonic = false;
            // 
            // employeeGroomersLabel
            // 
            employeeGroomersLabel.BackColor = Color.White;
            employeeGroomersLabel.Dock = DockStyle.Top;
            employeeGroomersLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeGroomersLabel.ForeColor = Color.FromArgb(68, 83, 106);
            employeeGroomersLabel.Location = new Point(0, 0);
            employeeGroomersLabel.Name = "employeeGroomersLabel";
            employeeGroomersLabel.Size = new Size(110, 18);
            employeeGroomersLabel.TabIndex = 2;
            employeeGroomersLabel.Text = "Groomers";
            employeeGroomersLabel.TextAlign = ContentAlignment.MiddleLeft;
            employeeGroomersLabel.UseMnemonic = false;
            // 
            // employeeGroomersIconHost
            // 
            employeeGroomersIconHost.BackColor = Color.White;
            employeeGroomersIconHost.Controls.Add(employeeGroomersIcon);
            employeeGroomersIconHost.Dock = DockStyle.Left;
            employeeGroomersIconHost.Location = new Point(0, 0);
            employeeGroomersIconHost.Name = "employeeGroomersIconHost";
            employeeGroomersIconHost.Size = new Size(44, 68);
            employeeGroomersIconHost.TabIndex = 3;
            // 
            // employeeGroomersIcon
            // 
            employeeGroomersIcon.BackColor = Color.White;
            employeeGroomersIcon.FillColor = Color.FromArgb(234, 242, 253);
            employeeGroomersIcon.ForeColor = Color.FromArgb(58, 123, 213);
            employeeGroomersIcon.Glyph = "\uE8C6";
            employeeGroomersIcon.GlyphSize = 11F;
            employeeGroomersIcon.Location = new Point(0, 2);
            employeeGroomersIcon.Name = "employeeGroomersIcon";
            employeeGroomersIcon.Radius = 8;
            employeeGroomersIcon.Size = new Size(34, 34);
            employeeGroomersIcon.TabIndex = 0;
            employeeGroomersIcon.TabStop = false;
            // 
            // employeeHeader
            // 
            employeeHeader.BackColor = Color.White;
            employeeHeader.Controls.Add(employeeHeaderText);
            employeeHeader.Controls.Add(employeeHeaderIconHost);
            employeeHeader.Controls.Add(employeeAddHost);
            employeeHeader.Dock = DockStyle.Top;
            employeeHeader.Location = new Point(0, 0);
            employeeHeader.Name = "employeeHeader";
            employeeHeader.Size = new Size(956, 60);
            employeeHeader.TabIndex = 4;
            // 
            // employeeHeaderText
            // 
            employeeHeaderText.BackColor = Color.White;
            employeeHeaderText.Controls.Add(employeeSubtitle);
            employeeHeaderText.Controls.Add(employeeTitle);
            employeeHeaderText.Dock = DockStyle.Fill;
            employeeHeaderText.Location = new Point(0, 0);
            employeeHeaderText.Name = "employeeHeaderText";
            employeeHeaderText.Padding = new Padding(0, 8, 0, 0);
            employeeHeaderText.Size = new Size(760, 60);
            employeeHeaderText.TabIndex = 0;
            // 
            // employeeSubtitle
            // 
            employeeSubtitle.BackColor = Color.White;
            employeeSubtitle.Dock = DockStyle.Top;
            employeeSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            employeeSubtitle.ForeColor = Color.FromArgb(122, 136, 156);
            employeeSubtitle.Location = new Point(0, 0);
            employeeSubtitle.Name = "employeeSubtitle";
            employeeSubtitle.Size = new Size(760, 18);
            employeeSubtitle.TabIndex = 0;
            employeeSubtitle.Text = "View, add, edit, and manage employee records.";
            employeeSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            employeeSubtitle.UseMnemonic = false;
            // 
            // employeeTitle
            // 
            employeeTitle.BackColor = Color.White;
            employeeTitle.Dock = DockStyle.Top;
            employeeTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeTitle.ForeColor = Color.FromArgb(31, 42, 60);
            employeeTitle.Location = new Point(0, 0);
            employeeTitle.Name = "employeeTitle";
            employeeTitle.Size = new Size(760, 24);
            employeeTitle.TabIndex = 1;
            employeeTitle.Text = "Employee Management";
            employeeTitle.TextAlign = ContentAlignment.MiddleLeft;
            employeeTitle.UseMnemonic = false;
            // 
            // employeeHeaderIconHost
            // 
            employeeHeaderIconHost.BackColor = Color.White;
            employeeHeaderIconHost.Controls.Add(employeeHeaderIcon);
            employeeHeaderIconHost.Dock = DockStyle.Left;
            employeeHeaderIconHost.Location = new Point(0, 0);
            employeeHeaderIconHost.Name = "employeeHeaderIconHost";
            employeeHeaderIconHost.Size = new Size(48, 60);
            employeeHeaderIconHost.TabIndex = 1;
            // 
            // employeeHeaderIcon
            // 
            employeeHeaderIcon.BackColor = Color.White;
            employeeHeaderIcon.FillColor = Color.FromArgb(234, 242, 253);
            employeeHeaderIcon.ForeColor = Color.FromArgb(58, 123, 213);
            employeeHeaderIcon.Glyph = "\uE716";
            employeeHeaderIcon.GlyphSize = 12F;
            employeeHeaderIcon.Location = new Point(0, 8);
            employeeHeaderIcon.Name = "employeeHeaderIcon";
            employeeHeaderIcon.Radius = 8;
            employeeHeaderIcon.Size = new Size(36, 36);
            employeeHeaderIcon.TabIndex = 0;
            employeeHeaderIcon.TabStop = false;
            // 
            // employeeAddHost
            // 
            employeeAddHost.BackColor = Color.White;
            employeeAddHost.Controls.Add(employeeAddButton);
            employeeAddHost.Dock = DockStyle.Right;
            employeeAddHost.Location = new Point(0, 0);
            employeeAddHost.Name = "employeeAddHost";
            employeeAddHost.Size = new Size(148, 60);
            employeeAddHost.TabIndex = 2;
            // 
            // employeeAddButton
            // 
            employeeAddButton.Anchor = AnchorStyles.Right;
            employeeAddButton.BackColor = Color.White;
            employeeAddButton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            employeeAddButton.Location = new Point(16, 12);
            employeeAddButton.Name = "employeeAddButton";
            employeeAddButton.Size = new Size(132, 36);
            employeeAddButton.TabIndex = 0;
            employeeAddButton.Text = "+ Add Employee";
            // 
            // EmployeeManagementPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            Controls.Add(pageScroll);
            Name = "EmployeeManagementPage";
            Size = new Size(1036, 732);
            pageScroll.ResumeLayout(false);
            userSection.ResumeLayout(false);
            userTable.ResumeLayout(false);
            userRows.ResumeLayout(false);
            userColumnsHead.ResumeLayout(false);
            userColumnsGrid.ResumeLayout(false);
            userFooter.ResumeLayout(false);
            userPager.ResumeLayout(false);
            userToolbar.ResumeLayout(false);
            userStats.ResumeLayout(false);
            userTotalTile.ResumeLayout(false);
            userTotalIconHost.ResumeLayout(false);
            userAdminTile.ResumeLayout(false);
            userAdminIconHost.ResumeLayout(false);
            userStaffTile.ResumeLayout(false);
            userStaffIconHost.ResumeLayout(false);
            userCustomerTile.ResumeLayout(false);
            userCustomerIconHost.ResumeLayout(false);
            userHeader.ResumeLayout(false);
            userHeaderText.ResumeLayout(false);
            userHeaderIconHost.ResumeLayout(false);
            userAddHost.ResumeLayout(false);
            employeeSection.ResumeLayout(false);
            employeeTable.ResumeLayout(false);
            employeeRows.ResumeLayout(false);
            employeeColumnsHead.ResumeLayout(false);
            employeeColumnsGrid.ResumeLayout(false);
            employeeFooter.ResumeLayout(false);
            employeePager.ResumeLayout(false);
            employeeToolbar.ResumeLayout(false);
            employeeStats.ResumeLayout(false);
            employeeTotalTile.ResumeLayout(false);
            employeeTotalIconHost.ResumeLayout(false);
            employeeAdminTile.ResumeLayout(false);
            employeeAdminIconHost.ResumeLayout(false);
            employeeCashierTile.ResumeLayout(false);
            employeeCashierIconHost.ResumeLayout(false);
            employeeFrontDeskTile.ResumeLayout(false);
            employeeFrontDeskIconHost.ResumeLayout(false);
            employeeGroomersTile.ResumeLayout(false);
            employeeGroomersIconHost.ResumeLayout(false);
            employeeHeader.ResumeLayout(false);
            employeeHeaderText.ResumeLayout(false);
            employeeHeaderIconHost.ResumeLayout(false);
            employeeAddHost.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pageScroll;
        private MyApp.Controls.CardPanel userSection;
        private MyApp.Controls.CardPanel userTable;
        private System.Windows.Forms.Panel userRows;
        private MyApp.Controls.EmptyState userEmpty;
        private System.Windows.Forms.Panel userColumnsHead;
        private System.Windows.Forms.TableLayoutPanel userColumnsGrid;
        private MyApp.Controls.CheckMark userHeaderCheck;
        private System.Windows.Forms.Label userColId;
        private System.Windows.Forms.Label userColUsername;
        private System.Windows.Forms.Label userColFullName;
        private System.Windows.Forms.Label userColRole;
        private System.Windows.Forms.Label userColEmail;
        private System.Windows.Forms.Label userColStatus;
        private System.Windows.Forms.Label userColLastLogin;
        private System.Windows.Forms.Label userColActions;
        private System.Windows.Forms.Panel userFooter;
        private System.Windows.Forms.Label userShowing;
        private System.Windows.Forms.Panel userPager;
        private MyApp.Controls.IconButton userPrevPage;
        private MyApp.Controls.PrimaryButton userPageChip;
        private MyApp.Controls.IconButton userNextPage;
        private System.Windows.Forms.Panel userToolbar;
        private MyApp.Controls.InputField userSearch;
        private System.Windows.Forms.Panel userToolbarGapA;
        private MyApp.Controls.SelectField userRoleFilter;
        private System.Windows.Forms.Panel userToolbarGapB;
        private MyApp.Controls.SelectField userStatusFilter;
        private System.Windows.Forms.TableLayoutPanel userStats;
        private MyApp.Controls.CardPanel userTotalTile;
        private System.Windows.Forms.Label userTotalDelta;
        private System.Windows.Forms.Label userTotalValue;
        private System.Windows.Forms.Label userTotalLabel;
        private System.Windows.Forms.Panel userTotalIconHost;
        private MyApp.Controls.IconBadge userTotalIcon;
        private MyApp.Controls.CardPanel userAdminTile;
        private System.Windows.Forms.Label userAdminDelta;
        private System.Windows.Forms.Label userAdminValue;
        private System.Windows.Forms.Label userAdminLabel;
        private System.Windows.Forms.Panel userAdminIconHost;
        private MyApp.Controls.IconBadge userAdminIcon;
        private MyApp.Controls.CardPanel userStaffTile;
        private System.Windows.Forms.Label userStaffDelta;
        private System.Windows.Forms.Label userStaffValue;
        private System.Windows.Forms.Label userStaffLabel;
        private System.Windows.Forms.Panel userStaffIconHost;
        private MyApp.Controls.IconBadge userStaffIcon;
        private MyApp.Controls.CardPanel userCustomerTile;
        private System.Windows.Forms.Label userCustomerDelta;
        private System.Windows.Forms.Label userCustomerValue;
        private System.Windows.Forms.Label userCustomerLabel;
        private System.Windows.Forms.Panel userCustomerIconHost;
        private MyApp.Controls.IconBadge userCustomerIcon;
        private System.Windows.Forms.Panel userHeader;
        private System.Windows.Forms.Panel userHeaderText;
        private System.Windows.Forms.Label userSubtitle;
        private System.Windows.Forms.Label userTitle;
        private System.Windows.Forms.Panel userHeaderIconHost;
        private MyApp.Controls.IconBadge userHeaderIcon;
        private System.Windows.Forms.Panel userAddHost;
        private MyApp.Controls.PrimaryButton userAddButton;
        private System.Windows.Forms.Panel sectionGap;
        private MyApp.Controls.CardPanel employeeSection;
        private MyApp.Controls.CardPanel employeeTable;
        private System.Windows.Forms.Panel employeeRows;
        private MyApp.Controls.EmptyState employeeEmpty;
        private System.Windows.Forms.Panel employeeColumnsHead;
        private System.Windows.Forms.TableLayoutPanel employeeColumnsGrid;
        private MyApp.Controls.CheckMark employeeHeaderCheck;
        private System.Windows.Forms.Label employeeColId;
        private System.Windows.Forms.Label employeeColName;
        private System.Windows.Forms.Label employeeColRole;
        private System.Windows.Forms.Label employeeColDepartment;
        private System.Windows.Forms.Label employeeColEmail;
        private System.Windows.Forms.Label employeeColStatus;
        private System.Windows.Forms.Label employeeColActions;
        private System.Windows.Forms.Panel employeeFooter;
        private System.Windows.Forms.Label employeeShowing;
        private System.Windows.Forms.Panel employeePager;
        private MyApp.Controls.IconButton employeePrevPage;
        private MyApp.Controls.PrimaryButton employeePageChip;
        private MyApp.Controls.IconButton employeeNextPage;
        private System.Windows.Forms.Panel employeeToolbar;
        private MyApp.Controls.InputField employeeSearch;
        private System.Windows.Forms.Panel employeeToolbarGapA;
        private MyApp.Controls.SelectField employeeRoleFilter;
        private System.Windows.Forms.Panel employeeToolbarGapB;
        private MyApp.Controls.SelectField employeeStatusFilter;
        private System.Windows.Forms.TableLayoutPanel employeeStats;
        private MyApp.Controls.CardPanel employeeTotalTile;
        private System.Windows.Forms.Label employeeTotalDelta;
        private System.Windows.Forms.Label employeeTotalValue;
        private System.Windows.Forms.Label employeeTotalLabel;
        private System.Windows.Forms.Panel employeeTotalIconHost;
        private MyApp.Controls.IconBadge employeeTotalIcon;
        private MyApp.Controls.CardPanel employeeAdminTile;
        private System.Windows.Forms.Label employeeAdminDelta;
        private System.Windows.Forms.Label employeeAdminValue;
        private System.Windows.Forms.Label employeeAdminLabel;
        private System.Windows.Forms.Panel employeeAdminIconHost;
        private MyApp.Controls.IconBadge employeeAdminIcon;
        private MyApp.Controls.CardPanel employeeCashierTile;
        private System.Windows.Forms.Label employeeCashierDelta;
        private System.Windows.Forms.Label employeeCashierValue;
        private System.Windows.Forms.Label employeeCashierLabel;
        private System.Windows.Forms.Panel employeeCashierIconHost;
        private MyApp.Controls.IconBadge employeeCashierIcon;
        private MyApp.Controls.CardPanel employeeFrontDeskTile;
        private System.Windows.Forms.Label employeeFrontDeskDelta;
        private System.Windows.Forms.Label employeeFrontDeskValue;
        private System.Windows.Forms.Label employeeFrontDeskLabel;
        private System.Windows.Forms.Panel employeeFrontDeskIconHost;
        private MyApp.Controls.IconBadge employeeFrontDeskIcon;
        private MyApp.Controls.CardPanel employeeGroomersTile;
        private System.Windows.Forms.Label employeeGroomersDelta;
        private System.Windows.Forms.Label employeeGroomersValue;
        private System.Windows.Forms.Label employeeGroomersLabel;
        private System.Windows.Forms.Panel employeeGroomersIconHost;
        private MyApp.Controls.IconBadge employeeGroomersIcon;
        private System.Windows.Forms.Panel employeeHeader;
        private System.Windows.Forms.Panel employeeHeaderText;
        private System.Windows.Forms.Label employeeSubtitle;
        private System.Windows.Forms.Label employeeTitle;
        private System.Windows.Forms.Panel employeeHeaderIconHost;
        private MyApp.Controls.IconBadge employeeHeaderIcon;
        private System.Windows.Forms.Panel employeeAddHost;
        private MyApp.Controls.PrimaryButton employeeAddButton;
    }
}
