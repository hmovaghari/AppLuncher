using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppLuncher.Forms;
using AppLuncher.Helpers;
using AppLuncher.Models;
using AppLuncher.Services;
using Newtonsoft.Json;

namespace AppLuncher
{
    public partial class Form1 : Form
    {
        private const string ClipboardFormat = "AppLuncher.ClipboardPayload.v1";
        private readonly decimal Version = 1.2M;
        private readonly JsonDatabaseService databaseService = new JsonDatabaseService();
        private readonly LauncherExecutionService executionService = new LauncherExecutionService();
        private readonly UpdateService updateService = new UpdateService();
        private readonly CancellationTokenSource shutdownTokenSource = new CancellationTokenSource();
        private AppDatabase database;
        private string databasePath;
        private bool initialized;
        private ViewMode currentViewMode;
        private bool useDarkTheme;
        private bool restoringUserInterfaceSettings;

        public Form1()
        {
            InitializeComponent();
            RestoreUserInterfaceSettings();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SaveUserInterfaceSettings();
            shutdownTokenSource.Cancel();
            shutdownTokenSource.Dispose();
            base.OnFormClosed(e);
        }

        protected override bool ProcessCmdKey(ref Message message, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                searchTextBox.Focus();
                searchTextBox.SelectAll();
                return true;
            }

            if (keyData == Keys.Escape && !string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Clear();
                groupsTreeView.Focus();
                return true;
            }

            return base.ProcessCmdKey(ref message, keyData);
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            BeginInvoke(new MethodInvoker(RestoreMainSplitterDistance));
            if (!InitializeDatabase(Properties.Settings.Default.JsonDatabasePath))
            {
                Close();
                return;
            }

            if (Properties.Settings.Default.CheckForUpdatesAtStartup)
            {
                await CheckForUpdatesAsync(false);
            }
        }

        private bool InitializeDatabase(string preferredPath)
        {
            string selectedPath = preferredPath;

            while (true)
            {
                if (string.IsNullOrWhiteSpace(selectedPath))
                {
                    selectedPath = SelectDatabasePath();
                    if (string.IsNullOrWhiteSpace(selectedPath))
                    {
                        return false;
                    }
                }

                try
                {
                    AppDatabase loadedDatabase = databaseService.LoadOrCreate(selectedPath);
                    database = loadedDatabase;
                    databasePath = selectedPath;
                    Properties.Settings.Default.JsonDatabasePath = selectedPath;
                    Properties.Settings.Default.Save();
                    RebuildTree(null);
                    databaseStatusLabel.Text = selectedPath;
                    databaseStatusLabel.ToolTipText = selectedPath;
                    Text = string.Format("AppLuncher - {0}", Path.GetFileName(selectedPath));
                    return true;
                }
                catch (Exception exception)
                {
                    DialogResult result = MessageBox.Show(this,
                        "The launcher database could not be opened.\r\n\r\n" + exception.Message +
                        "\r\n\r\nWould you like to choose another JSON file?",
                        "AppLuncher", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (result != DialogResult.Yes)
                    {
                        return false;
                    }

                    selectedPath = null;
                }
            }
        }

        private string SelectDatabasePath()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.AddExtension = true;
                dialog.DefaultExt = "json";
                dialog.Filter = "JSON database (*.json)|*.json|All files (*.*)|*.*";
                dialog.OverwritePrompt = false;
                dialog.Title = "Choose or create an AppLuncher database";
                dialog.FileName = "AppLuncher.json";

                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.FileName : null;
            }
        }

        private void RebuildTree(Guid? groupToSelect)
        {
            groupsTreeView.BeginUpdate();
            groupsTreeView.Nodes.Clear();

            TreeNode rootNode = new TreeNode("AppLuncher")
            {
                Name = "Root",
                Tag = null
            };

            foreach (LauncherGroup group in database.RootGroups.OrderBy(value => value.Name))
            {
                rootNode.Nodes.Add(CreateGroupNode(group));
            }

            groupsTreeView.Nodes.Add(rootNode);
            rootNode.Expand();
            groupsTreeView.SelectedNode = groupToSelect.HasValue
                ? FindGroupNode(rootNode, groupToSelect.Value) ?? rootNode
                : rootNode;
            UpdateNavigationControls();
            groupsTreeView.EndUpdate();
            RefreshContents();
        }

        private TreeNode CreateGroupNode(LauncherGroup group)
        {
            TreeNode node = new TreeNode(group.Name) { Tag = group };
            foreach (LauncherGroup child in group.ChildGroups.OrderBy(value => value.Name))
            {
                node.Nodes.Add(CreateGroupNode(child));
            }

            return node;
        }

        private static TreeNode FindGroupNode(TreeNode parent, Guid id)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                LauncherGroup group = node.Tag as LauncherGroup;
                if (group != null && group.Id == id)
                {
                    return node;
                }

                TreeNode childMatch = FindGroupNode(node, id);
                if (childMatch != null)
                {
                    return childMatch;
                }
            }

            return null;
        }

        private void RefreshContents()
        {
            if (database == null || groupsTreeView.SelectedNode == null)
            {
                return;
            }

            contentsListView.BeginUpdate();
            contentsListView.Items.Clear();
            largeImageList.Images.Clear();
            mediumImageList.Images.Clear();
            smallImageList.Images.Clear();

            string searchText = searchTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                RefreshSearchResults(searchText);
                contentsListView.EndUpdate();
                locationStatusLabel.Text = string.Format(
                    "Search results for \"{0}\" — {1} item(s)",
                    searchText,
                    contentsListView.Items.Count);
                return;
            }

            LauncherGroup selectedGroup = SelectedGroup;
            IEnumerable<LauncherGroup> groups = selectedGroup == null
                ? database.RootGroups
                : selectedGroup.ChildGroups;

            foreach (LauncherGroup group in groups.OrderBy(value => value.Name))
            {
                string key = "group-" + group.Id.ToString("N");
                AddImages(key, null, 0, true);
                ListViewItem row = new ListViewItem(group.Name)
                {
                    ImageKey = key,
                    Tag = new ContentEntry(group),
                    ToolTipText = "Group"
                };
                row.SubItems.Add(LocalizationManager.Translate("Group"));
                row.SubItems.Add(string.Empty);
                row.SubItems.Add(BuildGroupPath(group));
                contentsListView.Items.Add(row);
            }

            if (selectedGroup != null)
            {
                foreach (LauncherItem item in selectedGroup.Items.OrderBy(value => value.Name))
                {
                    string key = "launcher-" + item.Id.ToString("N");
                    AddImages(key, item.IconPath, item.IconIndex, false);
                    ListViewItem row = new ListViewItem(item.Name)
                    {
                        ImageKey = key,
                        Tag = new ContentEntry(item, selectedGroup),
                        ToolTipText = string.Format("{0} executable action(s)", item.Actions.Count)
                    };
                    row.SubItems.Add(LocalizationManager.Translate("Launcher"));
                    row.SubItems.Add(item.Actions.Count.ToString());
                    row.SubItems.Add(BuildGroupPath(selectedGroup));
                    contentsListView.Items.Add(row);
                }
            }

            contentsListView.EndUpdate();
            locationStatusLabel.Text = BuildLocationText();
        }

        private void RefreshSearchResults(string searchText)
        {
            foreach (LauncherGroup group in database.RootGroups.OrderBy(value => value.Name))
            {
                AddSearchResults(group, null, "AppLuncher", searchText);
            }
        }

        private void AddSearchResults(
            LauncherGroup group,
            LauncherGroup parentGroup,
            string parentPath,
            string searchText)
        {
            string groupPath = parentPath + " > " + group.Name;
            if (ContainsSearchText(group.Name, searchText))
            {
                string key = "group-" + group.Id.ToString("N");
                AddImages(key, null, 0, true);
                ListViewItem row = new ListViewItem(group.Name)
                {
                    ImageKey = key,
                    Tag = new ContentEntry(group, parentGroup),
                    ToolTipText = groupPath
                };
                row.SubItems.Add(LocalizationManager.Translate("Group"));
                row.SubItems.Add(string.Empty);
                row.SubItems.Add(parentPath);
                contentsListView.Items.Add(row);
            }

            foreach (LauncherItem item in group.Items.OrderBy(value => value.Name))
            {
                if (!LauncherMatches(item, searchText))
                {
                    continue;
                }

                string key = "launcher-" + item.Id.ToString("N");
                AddImages(key, item.IconPath, item.IconIndex, false);
                ListViewItem row = new ListViewItem(item.Name)
                {
                    ImageKey = key,
                    Tag = new ContentEntry(item, group),
                    ToolTipText = groupPath
                };
                row.SubItems.Add(LocalizationManager.Translate("Launcher"));
                row.SubItems.Add(item.Actions.Count.ToString());
                row.SubItems.Add(groupPath);
                contentsListView.Items.Add(row);
            }

            foreach (LauncherGroup childGroup in group.ChildGroups.OrderBy(value => value.Name))
            {
                AddSearchResults(childGroup, group, groupPath, searchText);
            }
        }

        private bool LauncherMatches(LauncherItem item, string searchText)
        {
            if (ContainsSearchText(item.Name, searchText))
            {
                return true;
            }

            return searchActionDetailsMenuItem.Checked &&
                   item.Actions.Any(action =>
                       ContainsSearchText(action.ProgramPath, searchText) ||
                       ContainsSearchText(action.Arguments, searchText) ||
                       ContainsSearchText(action.WorkingDirectory, searchText));
        }

        private static bool ContainsSearchText(string value, string searchText)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   value.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private string BuildGroupPath(LauncherGroup targetGroup)
        {
            if (targetGroup == null)
            {
                return "AppLuncher";
            }

            List<string> path = new List<string> { "AppLuncher" };
            if (TryBuildGroupPath(database.RootGroups, targetGroup.Id, path))
            {
                return string.Join(" > ", path);
            }

            return targetGroup.Name;
        }

        private static bool TryBuildGroupPath(
            IEnumerable<LauncherGroup> groups,
            Guid targetId,
            List<string> path)
        {
            foreach (LauncherGroup group in groups)
            {
                path.Add(group.Name);
                if (group.Id == targetId || TryBuildGroupPath(group.ChildGroups, targetId, path))
                {
                    return true;
                }

                path.RemoveAt(path.Count - 1);
            }

            return false;
        }

        private void AddImages(string key, string iconPath, int iconIndex, bool folder)
        {
            IconLoader.AddImage(largeImageList, key, iconPath, iconIndex, folder);
            IconLoader.AddImage(mediumImageList, key, iconPath, iconIndex, folder);
            IconLoader.AddImage(smallImageList, key, iconPath, iconIndex, folder);
        }

        private string BuildLocationText()
        {
            Stack<string> parts = new Stack<string>();
            TreeNode node = groupsTreeView.SelectedNode;
            while (node != null)
            {
                parts.Push(node.Text);
                node = node.Parent;
            }

            return string.Join(" > ", parts.ToArray());
        }

        private LauncherGroup SelectedGroup
        {
            get { return groupsTreeView.SelectedNode == null ? null : groupsTreeView.SelectedNode.Tag as LauncherGroup; }
        }

        private ContentEntry SelectedContent
        {
            get
            {
                return contentsListView.SelectedItems.Count == 0
                    ? null
                    : contentsListView.SelectedItems[0].Tag as ContentEntry;
            }
        }

        private void GroupsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateNavigationControls();
            RefreshContents();
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = groupsTreeView.SelectedNode;
            if (selectedNode != null && selectedNode.Parent != null)
            {
                groupsTreeView.SelectedNode = selectedNode.Parent;
                selectedNode.Parent.EnsureVisible();
            }
        }

        private void UpdateNavigationControls()
        {
            upButton.Enabled = groupsTreeView.SelectedNode != null &&
                               groupsTreeView.SelectedNode.Parent != null;
        }

        private void GroupsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                groupsTreeView.SelectedNode = e.Node;
            }
        }

        private void NewGroup_Click(object sender, EventArgs e)
        {
            using (GroupDialog dialog = new GroupDialog(null))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                LauncherGroup group = new LauncherGroup { Name = dialog.GroupName };
                LauncherGroup parent = SelectedGroup;
                if (parent == null)
                {
                    database.RootGroups.Add(group);
                }
                else
                {
                    parent.ChildGroups.Add(group);
                }

                SaveAndRefresh(group.Id);
            }
        }

        private void NewLauncher_Click(object sender, EventArgs e)
        {
            LauncherGroup group = SelectedGroup;
            if (group == null)
            {
                MessageBox.Show(this, "Select a group before creating a launcher.", "AppLuncher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (LauncherItemDialog dialog = new LauncherItemDialog(null))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    group.Items.Add(dialog.CreateLauncherItem(Guid.NewGuid()));
                    SaveAndRefresh(group.Id);
                }
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            ContentEntry entry = SelectedContent;
            if (entry == null)
            {
                EditGroup(SelectedGroup);
            }
            else if (entry.Group != null)
            {
                EditGroup(entry.Group);
            }
            else
            {
                EditLauncher(entry.Item, entry.ParentGroup);
            }
        }

        private void EditTreeGroup_Click(object sender, EventArgs e)
        {
            EditGroup(SelectedGroup);
        }

        private void EditGroup(LauncherGroup group)
        {
            if (group == null)
            {
                return;
            }

            using (GroupDialog dialog = new GroupDialog(group.Name))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    group.Name = dialog.GroupName;
                    SaveAndRefresh(group.Id);
                }
            }
        }

        private void EditLauncher(LauncherItem item, LauncherGroup parentGroup)
        {
            if (item == null)
            {
                return;
            }

            using (LauncherItemDialog dialog = new LauncherItemDialog(item))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    LauncherItem edited = dialog.CreateLauncherItem(item.Id);
                    ModelCloner.CopyLauncherItem(edited, item);
                    SaveAndRefresh(parentGroup == null ? (Guid?)null : parentGroup.Id);
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            ContentEntry entry = SelectedContent;
            if (entry == null)
            {
                DeleteGroup(SelectedGroup);
            }
            else if (entry.Group != null)
            {
                DeleteGroup(entry.Group);
            }
            else
            {
                DeleteLauncher(entry.Item, entry.ParentGroup);
            }
        }

        private void DeleteTreeGroup_Click(object sender, EventArgs e)
        {
            DeleteGroup(SelectedGroup);
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            ContentEntry entry = SelectedContent;
            if (entry != null)
            {
                if (entry.Group != null)
                {
                    CopyGroup(entry.Group);
                }
                else
                {
                    CopyLauncher(entry.Item);
                }

                return;
            }

            CopyGroup(SelectedGroup);
        }

        private void CopyTreeGroup_Click(object sender, EventArgs e)
        {
            CopyGroup(SelectedGroup);
        }

        private void CopyGroup(LauncherGroup group)
        {
            if (group == null)
            {
                return;
            }

            SetClipboardPayload(new ClipboardPayload
            {
                Type = ClipboardPayloadType.Group,
                Group = ModelCloner.Clone(group)
            }, "group", group.Name);
        }

        private void CopyLauncher(LauncherItem item)
        {
            if (item == null)
            {
                return;
            }

            SetClipboardPayload(new ClipboardPayload
            {
                Type = ClipboardPayloadType.Launcher,
                LauncherItem = ModelCloner.Clone(item)
            }, "launcher", item.Name);
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            LauncherGroup destinationGroup = SelectedGroup;
            ClipboardPayload payload = GetClipboardPayload();

            if (payload == null)
            {
                return;
            }

            if (payload.Type == ClipboardPayloadType.Group)
            {
                LauncherGroup groupCopy = ModelCloner.DuplicateGroup(payload.Group);
                if (destinationGroup == null)
                {
                    database.RootGroups.Add(groupCopy);
                }
                else
                {
                    destinationGroup.ChildGroups.Add(groupCopy);
                }

                SaveAndRefresh(groupCopy.Id);
                return;
            }

            if (payload.Type == ClipboardPayloadType.Launcher)
            {
                if (destinationGroup == null)
                {
                    MessageBox.Show(this, "Select a destination group before pasting a launcher.",
                        "AppLuncher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                destinationGroup.Items.Add(ModelCloner.DuplicateLauncherItem(payload.LauncherItem));
                SaveAndRefresh(destinationGroup.Id);
            }
        }

        private void SetClipboardPayload(ClipboardPayload payload, string itemType, string itemName)
        {
            try
            {
                Clipboard.SetData(ClipboardFormat, JsonConvert.SerializeObject(payload));
                locationStatusLabel.Text = string.Format("Copied {0}: {1}", itemType, itemName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this,
                    "The item could not be copied to the Windows clipboard.\r\n\r\n" + exception.Message,
                    "AppLuncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static ClipboardPayload GetClipboardPayload()
        {
            try
            {
                if (!Clipboard.ContainsData(ClipboardFormat))
                {
                    return null;
                }

                string serializedPayload = Clipboard.GetData(ClipboardFormat) as string;
                if (string.IsNullOrWhiteSpace(serializedPayload))
                {
                    return null;
                }

                ClipboardPayload payload = JsonConvert.DeserializeObject<ClipboardPayload>(serializedPayload);
                return payload != null && payload.IsValid ? payload : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private void DeleteGroup(LauncherGroup group)
        {
            if (group == null)
            {
                return;
            }

            if (MessageBox.Show(this,
                string.Format("Delete '{0}' and all of its nested groups and launchers?", group.Name),
                "AppLuncher", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            Guid? parentId = FindParentGroupId(group.Id);
            RemoveGroup(database.RootGroups, group.Id);
            SaveAndRefresh(parentId);
        }

        private void DeleteLauncher(LauncherItem item, LauncherGroup group)
        {
            if (group == null || item == null)
            {
                return;
            }

            if (MessageBox.Show(this, string.Format("Delete launcher '{0}'?", item.Name),
                "AppLuncher", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                group.Items.RemoveAll(value => value.Id == item.Id);
                SaveAndRefresh(group.Id);
            }
        }

        private Guid? FindParentGroupId(Guid groupId)
        {
            return FindParentGroupId(database.RootGroups, groupId, null);
        }

        private static Guid? FindParentGroupId(IEnumerable<LauncherGroup> groups, Guid groupId, Guid? parentId)
        {
            foreach (LauncherGroup group in groups)
            {
                if (group.Id == groupId)
                {
                    return parentId;
                }

                Guid? found = FindParentGroupId(group.ChildGroups, groupId, group.Id);
                if (found.HasValue)
                {
                    return found;
                }
            }

            return null;
        }

        private static bool RemoveGroup(ICollection<LauncherGroup> groups, Guid groupId)
        {
            LauncherGroup match = groups.FirstOrDefault(group => group.Id == groupId);
            if (match != null)
            {
                groups.Remove(match);
                return true;
            }

            return groups.Any(group => RemoveGroup(group.ChildGroups, groupId));
        }

        private async void ContentsListView_DoubleClick(object sender, EventArgs e)
        {
            await OpenSelectedContentAsync();
        }

        private async void OpenMenuItem_Click(object sender, EventArgs e)
        {
            await OpenSelectedContentAsync();
        }

        private async Task OpenSelectedContentAsync()
        {
            ContentEntry entry = SelectedContent;
            if (entry == null)
            {
                return;
            }

            if (entry.Group != null)
            {
                TreeNode node = FindGroupNode(groupsTreeView.Nodes[0], entry.Group.Id);
                if (node != null)
                {
                    groupsTreeView.SelectedNode = node;
                    node.EnsureVisible();
                }
                return;
            }

            if (entry.Item.Actions.Count == 0)
            {
                MessageBox.Show(this, "This launcher has no executable actions.", "AppLuncher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                UseWaitCursor = true;
                locationStatusLabel.Text = "Launching " + entry.Item.Name + "...";
                await executionService.ExecuteAsync(entry.Item, shutdownTokenSource.Token);
                locationStatusLabel.Text = "Launched " + entry.Item.Name;
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                locationStatusLabel.Text = BuildLocationText();
                MessageBox.Show(this, exception.Message, "Launch failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        private void SaveAndRefresh(Guid? groupToSelect)
        {
            try
            {
                databaseService.Save(databasePath, database);
                RebuildTree(groupToSelect);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "Changes could not be saved.\r\n\r\n" + exception.Message,
                    "AppLuncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefreshContents();
            }
        }

        private void ChangeDatabaseButton_Click(object sender, EventArgs e)
        {
            string selectedPath = SelectDatabasePath();
            if (!string.IsNullOrWhiteSpace(selectedPath))
            {
                InitializeDatabase(selectedPath);
            }
        }

        private void SupportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "http://hmovaghari.ir/eng/#contact",
                    UseShellExecute = true
                });
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "The support page could not be opened.\r\n\r\n" + exception.Message,
                    "AppLuncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CheckForUpdatesMenuItem_Click(object sender, EventArgs e)
        {
            await CheckForUpdatesAsync(true);
        }

        private void CheckForUpdatesAtStartupMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (restoringUserInterfaceSettings)
            {
                return;
            }

            Properties.Settings.Default.CheckForUpdatesAtStartup =
                checkForUpdatesAtStartupMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private async Task CheckForUpdatesAsync(bool showNoUpdateMessage)
        {
            try
            {
                UseWaitCursor = true;
                checkForUpdatesMenuItem.Enabled = false;

                UpdateInfo update = await updateService.GetLatestUpdateAsync();
                if (update.Version <= Version)
                {
                    if (showNoUpdateMessage)
                    {
                        MessageBox.Show(
                            this,
                            LocalizationManager.IsPersian
                                ? "شما از آخرین نسخه‌ی AppLuncher استفاده می‌کنید."
                                : "You are using the latest version of AppLuncher.",
                            "AppLuncher",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    return;
                }

                string message = LocalizationManager.IsPersian
                    ? string.Format(
                        "نسخه‌ی جدید {0} موجود است. نسخه‌ی فعلی شما {1} است.\r\n\r\nآیا صفحه‌ی دانلود باز شود؟",
                        update.Version,
                        Version)
                    : string.Format(
                        "Version {0} is available. Your current version is {1}.\r\n\r\nWould you like to open the download page?",
                        update.Version,
                        Version);

                DialogResult result = MessageBox.Show(
                    this,
                    message,
                    "AppLuncher",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = update.DownloadUrl,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception exception)
            {
                if (showNoUpdateMessage)
                {
                    string message = LocalizationManager.IsPersian
                        ? "بررسی به‌روزرسانی انجام نشد.\r\n\r\n" + exception.Message
                        : "The update check could not be completed.\r\n\r\n" + exception.Message;

                    MessageBox.Show(
                        this,
                        message,
                        "AppLuncher",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            finally
            {
                checkForUpdatesMenuItem.Enabled = true;
                UseWaitCursor = false;
            }
        }

        private void LightThemeMenuItem_Click(object sender, EventArgs e)
        {
            ApplyTheme(false);
        }

        private void DarkThemeMenuItem_Click(object sender, EventArgs e)
        {
            ApplyTheme(true);
        }

        private void ApplyTheme(bool darkTheme)
        {
            useDarkTheme = darkTheme;
            lightThemeMenuItem.Checked = !darkTheme;
            darkThemeMenuItem.Checked = darkTheme;

            ThemeManager.Apply(this, darkTheme);

            Color stripColor = darkTheme ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
            Color foreColor = darkTheme ? Color.Gainsboro : SystemColors.ControlText;
            ThemeManager.ApplyToolStrip(mainToolStrip, stripColor, foreColor);
            ThemeManager.ApplyToolStrip(mainStatusStrip, stripColor, foreColor);
            ThemeManager.ApplyToolStrip(contentContextMenu, stripColor, foreColor);
            ThemeManager.ApplyToolStrip(treeContextMenu, stripColor, foreColor);
        }

        private void EnglishLanguageMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ApplicationLanguage = "en";
            Properties.Settings.Default.Save();
            ApplyLanguage();
        }

        private void PersianLanguageMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ApplicationLanguage = "fa";
            Properties.Settings.Default.Save();
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            englishLanguageMenuItem.Checked = !LocalizationManager.IsPersian;
            persianLanguageMenuItem.Checked = LocalizationManager.IsPersian;

            LocalizationManager.Apply(this);
            LocalizationManager.ApplyToolStrip(mainToolStrip);
            LocalizationManager.ApplyToolStrip(mainStatusStrip);
            LocalizationManager.ApplyToolStrip(contentContextMenu);
            LocalizationManager.ApplyToolStrip(treeContextMenu);
            nameColumn.Text = LocalizationManager.Translate("Name");
            typeColumn.Text = LocalizationManager.Translate("Type");
            actionsColumn.Text = LocalizationManager.Translate("Actions");
            locationColumn.Text = LocalizationManager.Translate("Location");
            RefreshContents();
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            RefreshContents();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && contentsListView.Items.Count > 0)
            {
                contentsListView.Items[0].Selected = true;
                contentsListView.Items[0].Focused = true;
                contentsListView.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void SearchActionDetailsMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                RefreshContents();
            }
        }

        private void SetViewMode(ViewMode mode)
        {
            currentViewMode = mode;
            largeIconsMenuItem.Checked = mode == ViewMode.LargeIcons;
            mediumIconsMenuItem.Checked = mode == ViewMode.MediumIcons;
            smallIconsMenuItem.Checked = mode == ViewMode.SmallIcons;
            listMenuItem.Checked = mode == ViewMode.List;
            detailsMenuItem.Checked = mode == ViewMode.Details;

            if (mode == ViewMode.LargeIcons)
            {
                contentsListView.LargeImageList = largeImageList;
                contentsListView.View = View.LargeIcon;
            }
            else if (mode == ViewMode.MediumIcons)
            {
                contentsListView.LargeImageList = mediumImageList;
                contentsListView.View = View.LargeIcon;
            }
            else if (mode == ViewMode.SmallIcons)
            {
                contentsListView.SmallImageList = smallImageList;
                contentsListView.View = View.SmallIcon;
            }
            else if (mode == ViewMode.List)
            {
                contentsListView.SmallImageList = smallImageList;
                contentsListView.View = View.List;
            }
            else
            {
                contentsListView.SmallImageList = smallImageList;
                contentsListView.View = View.Details;
            }
        }

        private void RestoreUserInterfaceSettings()
        {
            Properties.Settings settings = Properties.Settings.Default;
            restoringUserInterfaceSettings = true;
            Size savedSize = settings.WindowSize;
            if (savedSize.Width >= MinimumSize.Width && savedSize.Height >= MinimumSize.Height)
            {
                Size = savedSize;
            }

            Point savedLocation = settings.WindowLocation;
            if (savedLocation.X >= 0 && savedLocation.Y >= 0 &&
                Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(
                    new Rectangle(savedLocation, savedSize))))
            {
                StartPosition = FormStartPosition.Manual;
                Location = savedLocation;
            }

            ViewMode restoredViewMode;
            if (!Enum.TryParse(settings.ContentViewMode, true, out restoredViewMode) ||
                !Enum.IsDefined(typeof(ViewMode), restoredViewMode))
            {
                restoredViewMode = ViewMode.MediumIcons;
            }

            SetViewMode(restoredViewMode);
            ApplyTheme(settings.UseDarkTheme);
            ApplyLanguage();

            if (settings.IsWindowMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }

            checkForUpdatesAtStartupMenuItem.Checked = settings.CheckForUpdatesAtStartup;
            restoringUserInterfaceSettings = false;
        }

        private void RestoreMainSplitterDistance()
        {
            int minimumDistance = mainSplitContainer.Panel1MinSize;
            int maximumDistance = Math.Max(
                minimumDistance,
                mainSplitContainer.ClientSize.Width -
                mainSplitContainer.SplitterWidth -
                mainSplitContainer.Panel2MinSize);

            mainSplitContainer.SplitterDistance = Math.Max(
                minimumDistance,
                Math.Min(Properties.Settings.Default.MainSplitterDistance, maximumDistance));
        }

        private void SaveUserInterfaceSettings()
        {
            try
            {
                Properties.Settings settings = Properties.Settings.Default;
                FormWindowState currentWindowState = WindowState;

                settings.IsWindowMaximized = currentWindowState == FormWindowState.Maximized;
                settings.ContentViewMode = currentViewMode.ToString();
                settings.MainSplitterDistance = mainSplitContainer.SplitterDistance;
                settings.UseDarkTheme = useDarkTheme;

                if (currentWindowState == FormWindowState.Normal)
                {
                    settings.WindowLocation = Location;
                    settings.WindowSize = Size;
                }
                else
                {
                    settings.WindowLocation = RestoreBounds.Location;
                    settings.WindowSize = RestoreBounds.Size;
                }

                settings.Save();
            }
            catch (Exception)
            {
            }
        }

        private void LargeIconsMenuItem_Click(object sender, EventArgs e)
        {
            SetViewMode(ViewMode.LargeIcons);
        }

        private void MediumIconsMenuItem_Click(object sender, EventArgs e)
        {
            SetViewMode(ViewMode.MediumIcons);
        }

        private void SmallIconsMenuItem_Click(object sender, EventArgs e)
        {
            SetViewMode(ViewMode.SmallIcons);
        }

        private void ListMenuItem_Click(object sender, EventArgs e)
        {
            SetViewMode(ViewMode.List);
        }

        private void DetailsMenuItem_Click(object sender, EventArgs e)
        {
            SetViewMode(ViewMode.Details);
        }

        private void ContentsListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Delete_Click(sender, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.F2)
            {
                Edit_Click(sender, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                OpenMenuItem_Click(sender, EventArgs.Empty);
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                Copy_Click(sender, EventArgs.Empty);
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                Paste_Click(sender, EventArgs.Empty);
            }
        }

        private void GroupsTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteTreeGroup_Click(sender, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.F2)
            {
                EditTreeGroup_Click(sender, EventArgs.Empty);
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                CopyTreeGroup_Click(sender, EventArgs.Empty);
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                Paste_Click(sender, EventArgs.Empty);
            }
        }

        private void ContentContextMenu_Opening(object sender, CancelEventArgs e)
        {
            bool hasSelection = SelectedContent != null;
            openMenuItem.Enabled = hasSelection;
            editMenuItem.Enabled = hasSelection;
            deleteMenuItem.Enabled = hasSelection;
            copyMenuItem.Enabled = hasSelection;
            pasteMenuItem.Enabled = CanPasteIntoSelectedGroup();
            newLauncherMenuItem.Enabled = SelectedGroup != null;
        }

        private void TreeContextMenu_Opening(object sender, CancelEventArgs e)
        {
            bool isGroup = SelectedGroup != null;
            treeNewLauncherMenuItem.Enabled = isGroup;
            treeRenameMenuItem.Enabled = isGroup;
            treeDeleteMenuItem.Enabled = isGroup;
            treeCopyMenuItem.Enabled = isGroup;
            treePasteMenuItem.Enabled = CanPasteIntoSelectedGroup();
        }

        private bool CanPasteIntoSelectedGroup()
        {
            ClipboardPayload payload = GetClipboardPayload();
            return payload != null && (payload.Type == ClipboardPayloadType.Group || SelectedGroup != null);
        }

        private enum ViewMode
        {
            LargeIcons,
            MediumIcons,
            SmallIcons,
            List,
            Details
        }

        private sealed class ContentEntry
        {
            public ContentEntry(LauncherGroup group, LauncherGroup parentGroup = null)
            {
                Group = group;
                ParentGroup = parentGroup;
            }

            public ContentEntry(LauncherItem item, LauncherGroup parentGroup)
            {
                Item = item;
                ParentGroup = parentGroup;
            }

            public LauncherGroup Group { get; private set; }

            public LauncherItem Item { get; private set; }

            public LauncherGroup ParentGroup { get; private set; }
        }

        private enum ClipboardPayloadType
        {
            Group,
            Launcher
        }

        private sealed class ClipboardPayload
        {
            public ClipboardPayloadType Type { get; set; }

            public LauncherGroup Group { get; set; }

            public LauncherItem LauncherItem { get; set; }

            [JsonIgnore]
            public bool IsValid
            {
                get
                {
                    return (Type == ClipboardPayloadType.Group && Group != null) ||
                        (Type == ClipboardPayloadType.Launcher && LauncherItem != null);
                }
            }
        }
    }
}
