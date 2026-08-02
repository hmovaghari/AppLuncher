namespace AppLuncher
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton newGroupButton;
        private System.Windows.Forms.ToolStripButton newLauncherButton;
        private System.Windows.Forms.ToolStripButton upButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton editButton;
        private System.Windows.Forms.ToolStripButton deleteButton;
        private System.Windows.Forms.ToolStripButton copyButton;
        private System.Windows.Forms.ToolStripButton pasteButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton viewButton;
        private System.Windows.Forms.ToolStripMenuItem largeIconsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediumIconsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallIconsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsMenuItem;
        private System.Windows.Forms.ToolStripButton changeDatabaseButton;
        private System.Windows.Forms.ToolStripButton supportButton;
        private System.Windows.Forms.ToolStripDropDownButton settingsButton;
        private System.Windows.Forms.ToolStripSeparator settingsSeparator1;
        private System.Windows.Forms.ToolStripSeparator settingsSeparator2;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesAtStartupMenuItem;
        private System.Windows.Forms.ToolStripSeparator settingsSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton themeButton;
        private System.Windows.Forms.ToolStripMenuItem lightThemeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkThemeMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton languageButton;
        private System.Windows.Forms.ToolStripMenuItem englishLanguageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem persianLanguageMenuItem;
        private System.Windows.Forms.ToolStripLabel searchLabel;
        private System.Windows.Forms.ToolStripTextBox searchTextBox;
        private System.Windows.Forms.ToolStripDropDownButton searchOptionsButton;
        private System.Windows.Forms.ToolStripMenuItem searchActionDetailsMenuItem;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.TreeView groupsTreeView;
        private System.Windows.Forms.ListView contentsListView;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.ColumnHeader actionsColumn;
        private System.Windows.Forms.ColumnHeader locationColumn;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel locationStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel databaseStatusLabel;
        private System.Windows.Forms.ImageList largeImageList;
        private System.Windows.Forms.ImageList mediumImageList;
        private System.Windows.Forms.ImageList smallImageList;
        private System.Windows.Forms.ContextMenuStrip contentContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGroupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newLauncherMenuItem;
        private System.Windows.Forms.ToolStripSeparator contextSeparator1;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteMenuItem;
        private System.Windows.Forms.ContextMenuStrip treeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem treeNewGroupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treeNewLauncherMenuItem;
        private System.Windows.Forms.ToolStripSeparator treeSeparator;
        private System.Windows.Forms.ToolStripMenuItem treeRenameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treeDeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treeCopyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treePasteMenuItem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.newGroupButton = new System.Windows.Forms.ToolStripButton();
            this.newLauncherButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.upButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editButton = new System.Windows.Forms.ToolStripButton();
            this.copyButton = new System.Windows.Forms.ToolStripButton();
            this.pasteButton = new System.Windows.Forms.ToolStripButton();
            this.deleteButton = new System.Windows.Forms.ToolStripButton();
            this.viewButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.largeIconsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumIconsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallIconsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.changeDatabaseButton = new System.Windows.Forms.ToolStripButton();
            this.settingsSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.themeButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.lightThemeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkThemeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.englishLanguageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.persianLanguageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.checkForUpdatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesAtStartupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.supportButton = new System.Windows.Forms.ToolStripButton();
            this.searchOptionsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.searchActionDetailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.searchLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.groupsTreeView = new System.Windows.Forms.TreeView();
            this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.treeNewGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeNewLauncherMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.treeRenameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treePasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeDeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsListView = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actionsColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.locationColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newLauncherMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumImageList = new System.Windows.Forms.ImageList(this.components);
            this.smallImageList = new System.Windows.Forms.ImageList(this.components);
            this.largeImageList = new System.Windows.Forms.ImageList(this.components);
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.locationStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.databaseStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.treeContextMenu.SuspendLayout();
            this.contentContextMenu.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGroupButton,
            this.newLauncherButton,
            this.toolStripSeparator2,
            this.upButton,
            this.toolStripSeparator1,
            this.editButton,
            this.copyButton,
            this.pasteButton,
            this.deleteButton,
            this.viewButton,
            this.settingsButton,
            this.searchOptionsButton,
            this.searchTextBox,
            this.searchLabel,
            this.toolStripSeparator3});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.mainToolStrip.Size = new System.Drawing.Size(917, 32);
            this.mainToolStrip.TabIndex = 0;
            // 
            // newGroupButton
            // 
            this.newGroupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newGroupButton.Name = "newGroupButton";
            this.newGroupButton.Size = new System.Drawing.Size(71, 21);
            this.newGroupButton.Text = "New Group";
            this.newGroupButton.Click += new System.EventHandler(this.NewGroup_Click);
            // 
            // newLauncherButton
            // 
            this.newLauncherButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newLauncherButton.Name = "newLauncherButton";
            this.newLauncherButton.Size = new System.Drawing.Size(87, 21);
            this.newLauncherButton.Text = "New Launcher";
            this.newLauncherButton.Click += new System.EventHandler(this.NewLauncher_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 24);
            // 
            // upButton
            // 
            this.upButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(26, 21);
            this.upButton.Text = "Up";
            this.upButton.ToolTipText = "Go to the parent group";
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // editButton
            // 
            this.editButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(31, 21);
            this.editButton.Text = "Edit";
            this.editButton.Click += new System.EventHandler(this.Edit_Click);
            // 
            // copyButton
            // 
            this.copyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(39, 21);
            this.copyButton.Text = "Copy";
            this.copyButton.Click += new System.EventHandler(this.Copy_Click);
            // 
            // pasteButton
            // 
            this.pasteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Size = new System.Drawing.Size(39, 21);
            this.pasteButton.Text = "Paste";
            this.pasteButton.Click += new System.EventHandler(this.Paste_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(44, 21);
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.Delete_Click);
            // 
            // viewButton
            // 
            this.viewButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.viewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.viewButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.largeIconsMenuItem,
            this.mediumIconsMenuItem,
            this.smallIconsMenuItem,
            this.listMenuItem,
            this.detailsMenuItem});
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(45, 21);
            this.viewButton.Text = "View";
            // 
            // largeIconsMenuItem
            // 
            this.largeIconsMenuItem.Name = "largeIconsMenuItem";
            this.largeIconsMenuItem.Size = new System.Drawing.Size(150, 22);
            this.largeIconsMenuItem.Text = "Large icons";
            this.largeIconsMenuItem.Click += new System.EventHandler(this.LargeIconsMenuItem_Click);
            // 
            // mediumIconsMenuItem
            // 
            this.mediumIconsMenuItem.Name = "mediumIconsMenuItem";
            this.mediumIconsMenuItem.Size = new System.Drawing.Size(150, 22);
            this.mediumIconsMenuItem.Text = "Medium icons";
            this.mediumIconsMenuItem.Click += new System.EventHandler(this.MediumIconsMenuItem_Click);
            // 
            // smallIconsMenuItem
            // 
            this.smallIconsMenuItem.Name = "smallIconsMenuItem";
            this.smallIconsMenuItem.Size = new System.Drawing.Size(150, 22);
            this.smallIconsMenuItem.Text = "Small icons";
            this.smallIconsMenuItem.Click += new System.EventHandler(this.SmallIconsMenuItem_Click);
            // 
            // listMenuItem
            // 
            this.listMenuItem.Name = "listMenuItem";
            this.listMenuItem.Size = new System.Drawing.Size(150, 22);
            this.listMenuItem.Text = "List";
            this.listMenuItem.Click += new System.EventHandler(this.ListMenuItem_Click);
            // 
            // detailsMenuItem
            // 
            this.detailsMenuItem.Name = "detailsMenuItem";
            this.detailsMenuItem.Size = new System.Drawing.Size(150, 22);
            this.detailsMenuItem.Text = "Details";
            this.detailsMenuItem.Click += new System.EventHandler(this.DetailsMenuItem_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.settingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.settingsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeDatabaseButton,
            this.settingsSeparator1,
            this.themeButton,
            this.languageButton,
            this.settingsSeparator2,
            this.checkForUpdatesMenuItem,
            this.checkForUpdatesAtStartupMenuItem,
            this.settingsSeparator3,
            this.supportButton});
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(62, 21);
            this.settingsButton.Text = "Settings";
            // 
            // changeDatabaseButton
            // 
            this.changeDatabaseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.changeDatabaseButton.Name = "changeDatabaseButton";
            this.changeDatabaseButton.Size = new System.Drawing.Size(103, 19);
            this.changeDatabaseButton.Text = "Change Database";
            this.changeDatabaseButton.Click += new System.EventHandler(this.ChangeDatabaseButton_Click);
            // 
            // settingsSeparator1
            // 
            this.settingsSeparator1.Name = "settingsSeparator1";
            this.settingsSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // themeButton
            // 
            this.themeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.themeButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lightThemeMenuItem,
            this.darkThemeMenuItem});
            this.themeButton.Name = "themeButton";
            this.themeButton.Size = new System.Drawing.Size(57, 19);
            this.themeButton.Text = "Theme";
            // 
            // lightThemeMenuItem
            // 
            this.lightThemeMenuItem.Name = "lightThemeMenuItem";
            this.lightThemeMenuItem.Size = new System.Drawing.Size(101, 22);
            this.lightThemeMenuItem.Text = "Light";
            this.lightThemeMenuItem.Click += new System.EventHandler(this.LightThemeMenuItem_Click);
            // 
            // darkThemeMenuItem
            // 
            this.darkThemeMenuItem.Name = "darkThemeMenuItem";
            this.darkThemeMenuItem.Size = new System.Drawing.Size(101, 22);
            this.darkThemeMenuItem.Text = "Dark";
            this.darkThemeMenuItem.Click += new System.EventHandler(this.DarkThemeMenuItem_Click);
            // 
            // languageButton
            // 
            this.languageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.languageButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishLanguageMenuItem,
            this.persianLanguageMenuItem});
            this.languageButton.Name = "languageButton";
            this.languageButton.Size = new System.Drawing.Size(72, 19);
            this.languageButton.Text = "Language";
            // 
            // englishLanguageMenuItem
            // 
            this.englishLanguageMenuItem.Name = "englishLanguageMenuItem";
            this.englishLanguageMenuItem.Size = new System.Drawing.Size(112, 22);
            this.englishLanguageMenuItem.Text = "English";
            this.englishLanguageMenuItem.Click += new System.EventHandler(this.EnglishLanguageMenuItem_Click);
            // 
            // persianLanguageMenuItem
            // 
            this.persianLanguageMenuItem.Name = "persianLanguageMenuItem";
            this.persianLanguageMenuItem.Size = new System.Drawing.Size(112, 22);
            this.persianLanguageMenuItem.Text = "Persian";
            this.persianLanguageMenuItem.Click += new System.EventHandler(this.PersianLanguageMenuItem_Click);
            // 
            // settingsSeparator2
            // 
            this.settingsSeparator2.Name = "settingsSeparator2";
            this.settingsSeparator2.Size = new System.Drawing.Size(220, 6);
            // 
            // checkForUpdatesMenuItem
            // 
            this.checkForUpdatesMenuItem.Name = "checkForUpdatesMenuItem";
            this.checkForUpdatesMenuItem.Size = new System.Drawing.Size(223, 22);
            this.checkForUpdatesMenuItem.Text = "Check for Updates";
            this.checkForUpdatesMenuItem.Click += new System.EventHandler(this.CheckForUpdatesMenuItem_Click);
            // 
            // checkForUpdatesAtStartupMenuItem
            // 
            this.checkForUpdatesAtStartupMenuItem.CheckOnClick = true;
            this.checkForUpdatesAtStartupMenuItem.Name = "checkForUpdatesAtStartupMenuItem";
            this.checkForUpdatesAtStartupMenuItem.Size = new System.Drawing.Size(223, 22);
            this.checkForUpdatesAtStartupMenuItem.Text = "Check for updates at startup";
            this.checkForUpdatesAtStartupMenuItem.CheckedChanged += new System.EventHandler(this.CheckForUpdatesAtStartupMenuItem_CheckedChanged);
            // 
            // settingsSeparator3
            // 
            this.settingsSeparator3.Name = "settingsSeparator3";
            this.settingsSeparator3.Size = new System.Drawing.Size(220, 6);
            // 
            // supportButton
            // 
            this.supportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.supportButton.Name = "supportButton";
            this.supportButton.Size = new System.Drawing.Size(53, 19);
            this.supportButton.Text = "Support";
            this.supportButton.ToolTipText = "Open the AppLuncher support page";
            this.supportButton.Click += new System.EventHandler(this.SupportButton_Click);
            // 
            // searchOptionsButton
            // 
            this.searchOptionsButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.searchOptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.searchOptionsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchActionDetailsMenuItem});
            this.searchOptionsButton.Name = "searchOptionsButton";
            this.searchOptionsButton.Size = new System.Drawing.Size(62, 21);
            this.searchOptionsButton.Text = "Options";
            // 
            // searchActionDetailsMenuItem
            // 
            this.searchActionDetailsMenuItem.CheckOnClick = true;
            this.searchActionDetailsMenuItem.Name = "searchActionDetailsMenuItem";
            this.searchActionDetailsMenuItem.Size = new System.Drawing.Size(374, 22);
            this.searchActionDetailsMenuItem.Text = "Include program path, arguments, and working directory";
            this.searchActionDetailsMenuItem.CheckedChanged += new System.EventHandler(this.SearchActionDetailsMenuItem_CheckedChanged);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.searchTextBox.AutoSize = false;
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(190, 24);
            this.searchTextBox.ToolTipText = "Search group and launcher names. Use Options to include action details.";
            this.searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            this.searchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // searchLabel
            // 
            this.searchLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(45, 21);
            this.searchLabel.Text = "Search:";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 24);
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 32);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.groupsTreeView);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.contentsListView);
            this.mainSplitContainer.Size = new System.Drawing.Size(917, 362);
            this.mainSplitContainer.SplitterDistance = 214;
            this.mainSplitContainer.TabIndex = 1;
            // 
            // groupsTreeView
            // 
            this.groupsTreeView.ContextMenuStrip = this.treeContextMenu;
            this.groupsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupsTreeView.HideSelection = false;
            this.groupsTreeView.Location = new System.Drawing.Point(0, 0);
            this.groupsTreeView.Name = "groupsTreeView";
            this.groupsTreeView.Size = new System.Drawing.Size(214, 362);
            this.groupsTreeView.TabIndex = 0;
            this.groupsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.GroupsTreeView_AfterSelect);
            this.groupsTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.GroupsTreeView_NodeMouseClick);
            this.groupsTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GroupsTreeView_KeyDown);
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.treeNewGroupMenuItem,
            this.treeNewLauncherMenuItem,
            this.treeSeparator,
            this.treeRenameMenuItem,
            this.treeCopyMenuItem,
            this.treePasteMenuItem,
            this.treeDeleteMenuItem});
            this.treeContextMenu.Name = "treeContextMenu";
            this.treeContextMenu.Size = new System.Drawing.Size(157, 142);
            this.treeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TreeContextMenu_Opening);
            // 
            // treeNewGroupMenuItem
            // 
            this.treeNewGroupMenuItem.Name = "treeNewGroupMenuItem";
            this.treeNewGroupMenuItem.Size = new System.Drawing.Size(156, 22);
            this.treeNewGroupMenuItem.Text = "New group...";
            this.treeNewGroupMenuItem.Click += new System.EventHandler(this.NewGroup_Click);
            // 
            // treeNewLauncherMenuItem
            // 
            this.treeNewLauncherMenuItem.Name = "treeNewLauncherMenuItem";
            this.treeNewLauncherMenuItem.Size = new System.Drawing.Size(156, 22);
            this.treeNewLauncherMenuItem.Text = "New launcher...";
            this.treeNewLauncherMenuItem.Click += new System.EventHandler(this.NewLauncher_Click);
            // 
            // treeSeparator
            // 
            this.treeSeparator.Name = "treeSeparator";
            this.treeSeparator.Size = new System.Drawing.Size(153, 6);
            // 
            // treeRenameMenuItem
            // 
            this.treeRenameMenuItem.Name = "treeRenameMenuItem";
            this.treeRenameMenuItem.Size = new System.Drawing.Size(156, 22);
            this.treeRenameMenuItem.Text = "Rename...";
            this.treeRenameMenuItem.Click += new System.EventHandler(this.EditTreeGroup_Click);
            // 
            // treeCopyMenuItem
            // 
            this.treeCopyMenuItem.Name = "treeCopyMenuItem";
            this.treeCopyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.treeCopyMenuItem.Size = new System.Drawing.Size(156, 22);
            this.treeCopyMenuItem.Text = "Copy";
            this.treeCopyMenuItem.Click += new System.EventHandler(this.CopyTreeGroup_Click);
            // 
            // treePasteMenuItem
            // 
            this.treePasteMenuItem.Name = "treePasteMenuItem";
            this.treePasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.treePasteMenuItem.Size = new System.Drawing.Size(156, 22);
            this.treePasteMenuItem.Text = "Paste";
            this.treePasteMenuItem.Click += new System.EventHandler(this.Paste_Click);
            // 
            // treeDeleteMenuItem
            // 
            this.treeDeleteMenuItem.Name = "treeDeleteMenuItem";
            this.treeDeleteMenuItem.Size = new System.Drawing.Size(156, 22);
            this.treeDeleteMenuItem.Text = "Delete";
            this.treeDeleteMenuItem.Click += new System.EventHandler(this.DeleteTreeGroup_Click);
            // 
            // contentsListView
            // 
            this.contentsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.typeColumn,
            this.actionsColumn,
            this.locationColumn});
            this.contentsListView.ContextMenuStrip = this.contentContextMenu;
            this.contentsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentsListView.FullRowSelect = true;
            this.contentsListView.HideSelection = false;
            this.contentsListView.LargeImageList = this.mediumImageList;
            this.contentsListView.Location = new System.Drawing.Point(0, 0);
            this.contentsListView.MultiSelect = false;
            this.contentsListView.Name = "contentsListView";
            this.contentsListView.ShowItemToolTips = true;
            this.contentsListView.Size = new System.Drawing.Size(699, 362);
            this.contentsListView.SmallImageList = this.smallImageList;
            this.contentsListView.TabIndex = 0;
            this.contentsListView.UseCompatibleStateImageBehavior = false;
            this.contentsListView.DoubleClick += new System.EventHandler(this.ContentsListView_DoubleClick);
            this.contentsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ContentsListView_KeyDown);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 300;
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            this.typeColumn.Width = 140;
            // 
            // actionsColumn
            // 
            this.actionsColumn.Text = "Actions";
            this.actionsColumn.Width = 80;
            // 
            // locationColumn
            // 
            this.locationColumn.Text = "Location";
            this.locationColumn.Width = 300;
            // 
            // contentContextMenu
            // 
            this.contentContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.newGroupMenuItem,
            this.newLauncherMenuItem,
            this.contextSeparator1,
            this.editMenuItem,
            this.copyMenuItem,
            this.pasteMenuItem,
            this.deleteMenuItem});
            this.contentContextMenu.Name = "contentContextMenu";
            this.contentContextMenu.Size = new System.Drawing.Size(157, 164);
            this.contentContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContentContextMenu_Opening);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(156, 22);
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // newGroupMenuItem
            // 
            this.newGroupMenuItem.Name = "newGroupMenuItem";
            this.newGroupMenuItem.Size = new System.Drawing.Size(156, 22);
            this.newGroupMenuItem.Text = "New group...";
            this.newGroupMenuItem.Click += new System.EventHandler(this.NewGroup_Click);
            // 
            // newLauncherMenuItem
            // 
            this.newLauncherMenuItem.Name = "newLauncherMenuItem";
            this.newLauncherMenuItem.Size = new System.Drawing.Size(156, 22);
            this.newLauncherMenuItem.Text = "New launcher...";
            this.newLauncherMenuItem.Click += new System.EventHandler(this.NewLauncher_Click);
            // 
            // contextSeparator1
            // 
            this.contextSeparator1.Name = "contextSeparator1";
            this.contextSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(156, 22);
            this.editMenuItem.Text = "Edit...";
            this.editMenuItem.Click += new System.EventHandler(this.Edit_Click);
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyMenuItem.Size = new System.Drawing.Size(156, 22);
            this.copyMenuItem.Text = "Copy";
            this.copyMenuItem.Click += new System.EventHandler(this.Copy_Click);
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Name = "pasteMenuItem";
            this.pasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteMenuItem.Size = new System.Drawing.Size(156, 22);
            this.pasteMenuItem.Text = "Paste";
            this.pasteMenuItem.Click += new System.EventHandler(this.Paste_Click);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(156, 22);
            this.deleteMenuItem.Text = "Delete";
            this.deleteMenuItem.Click += new System.EventHandler(this.Delete_Click);
            // 
            // mediumImageList
            // 
            this.mediumImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.mediumImageList.ImageSize = new System.Drawing.Size(48, 48);
            this.mediumImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // smallImageList
            // 
            this.smallImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.smallImageList.ImageSize = new System.Drawing.Size(24, 24);
            this.smallImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // largeImageList
            // 
            this.largeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.largeImageList.ImageSize = new System.Drawing.Size(96, 96);
            this.largeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.locationStatusLabel,
            this.databaseStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 394);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(917, 22);
            this.mainStatusStrip.TabIndex = 2;
            // 
            // locationStatusLabel
            // 
            this.locationStatusLabel.Name = "locationStatusLabel";
            this.locationStatusLabel.Size = new System.Drawing.Size(72, 17);
            this.locationStatusLabel.Text = "AppLuncher";
            // 
            // databaseStatusLabel
            // 
            this.databaseStatusLabel.Name = "databaseStatusLabel";
            this.databaseStatusLabel.Size = new System.Drawing.Size(830, 17);
            this.databaseStatusLabel.Spring = true;
            this.databaseStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 416);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainToolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(794, 171);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AppLuncher";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.treeContextMenu.ResumeLayout(false);
            this.contentContextMenu.ResumeLayout(false);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
