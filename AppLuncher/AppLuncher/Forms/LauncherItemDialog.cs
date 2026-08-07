using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AppLuncher.Helpers;
using AppLuncher.Models;

namespace AppLuncher.Forms
{
    public sealed class LauncherItemDialog : Form
    {
        private readonly TextBox nameTextBox;
        private readonly TextBox iconPathTextBox;
        private readonly CheckBox runAsAdministratorCheckBox;
        private readonly PictureBox iconPreview;
        private readonly Label embeddedIconsLabel;
        private readonly ListView embeddedIconsListView;
        private readonly ImageList embeddedIconsImageList;
        private readonly ListView actionsListView;
        private readonly List<LaunchAction> actions;
        private int selectedIconIndex;

        public LauncherItemDialog(LauncherItem item)
        {
            LauncherItem source = item == null ? new LauncherItem() : ModelCloner.Clone(item);
            actions = source.Actions ?? new List<LaunchAction>();

            Text = item == null ? "Create Launcher" : "Edit Launcher";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(760, 740);
            Size = new Size(850, 740);
            ShowInTaskbar = false;
            selectedIconIndex = Math.Max(0, source.IconIndex);

            Label nameLabel = new Label { AutoSize = true, Location = new Point(16, 20), Text = "Name:" };
            nameTextBox = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(95, 16),
                Size = new Size(630, 23),
                Text = source.Name
            };

            Label iconLabel = new Label { AutoSize = true, Location = new Point(16, 56), Text = "Icon source:" };
            iconPathTextBox = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(95, 52),
                Size = new Size(545, 23),
                Text = source.IconPath ?? string.Empty
            };
            iconPathTextBox.TextChanged += IconPathTextBox_TextChanged;

            Button browseIconButton = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(646, 51),
                Size = new Size(79, 25),
                Text = "Browse..."
            };
            browseIconButton.Click += BrowseIconButton_Click;

            iconPreview = new PictureBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(741, 16),
                Size = new Size(64, 64),
                SizeMode = PictureBoxSizeMode.CenterImage
            };

            embeddedIconsLabel = new Label
            {
                AutoSize = true,
                Location = new Point(16, 96),
                Text = "Embedded icons:"
            };

            embeddedIconsImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(48, 48)
            };

            embeddedIconsListView = new ListView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                FullRowSelect = true,
                HideSelection = false,
                LargeImageList = embeddedIconsImageList,
                Location = new Point(95, 92),
                MultiSelect = false,
                Size = new Size(710, 76),
                View = View.LargeIcon
            };
            embeddedIconsListView.SelectedIndexChanged += EmbeddedIconsListView_SelectedIndexChanged;

            GroupBox actionsGroup = new GroupBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(14, 210),
                Size = new Size(791, 435),
                Text = "Executable actions (run in order)"
            };

            actionsListView = new ListView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false,
                Location = new Point(12, 25),
                MultiSelect = false,
                Size = new Size(650, 394),
                View = View.Details
            };
            actionsListView.Columns.Add("#", 42);
            actionsListView.Columns.Add("Program", 235);
            actionsListView.Columns.Add("Arguments", 150);
            actionsListView.Columns.Add("Working directory", 150);
            actionsListView.Columns.Add("Wait", 55);
            actionsListView.Columns.Add("Delay", 65);
            actionsListView.DoubleClick += EditActionButton_Click;

            Button addActionButton = CreateActionButton("Add...", 22, AddActionButton_Click);
            Button editActionButton = CreateActionButton("Edit...", 57, EditActionButton_Click);
            Button deleteActionButton = CreateActionButton("Delete", 92, DeleteActionButton_Click);
            Button moveUpButton = CreateActionButton("Move Up", 145, MoveUpButton_Click);
            Button moveDownButton = CreateActionButton("Move Down", 180, MoveDownButton_Click);

            runAsAdministratorCheckBox = new CheckBox
            {
                AutoSize = true,
                Checked = source.RunAsAdministrator,
                Location = new Point(95, 178),
                Text = "Run this launcher as administrator by default"
            };

            actionsGroup.Controls.Add(actionsListView);
            actionsGroup.Controls.Add(addActionButton);
            actionsGroup.Controls.Add(editActionButton);
            actionsGroup.Controls.Add(deleteActionButton);
            actionsGroup.Controls.Add(moveUpButton);
            actionsGroup.Controls.Add(moveDownButton);

            Button okButton = new Button
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                DialogResult = DialogResult.OK,
                Location = new Point(649, 658),
                Size = new Size(75, 29),
                Text = "OK"
            };
            okButton.Click += OkButton_Click;

            Button cancelButton = new Button
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                DialogResult = DialogResult.Cancel,
                Location = new Point(730, 658),
                Size = new Size(75, 29),
                Text = "Cancel"
            };

            Controls.Add(nameLabel);
            Controls.Add(nameTextBox);
            Controls.Add(iconLabel);
            Controls.Add(iconPathTextBox);
            Controls.Add(browseIconButton);
            Controls.Add(iconPreview);
            Controls.Add(embeddedIconsLabel);
            Controls.Add(embeddedIconsListView);
            Controls.Add(runAsAdministratorCheckBox);
            Controls.Add(actionsGroup);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            AcceptButton = okButton;
            CancelButton = cancelButton;

            RefreshActions();
            RefreshEmbeddedIcons();
            RefreshIconPreview();
            LocalizationManager.Apply(this);
            ThemeManager.Apply(this, Properties.Settings.Default.UseDarkTheme);
        }

        public LauncherItem CreateLauncherItem(Guid id)
        {
            return new LauncherItem
            {
                Id = id == Guid.Empty ? Guid.NewGuid() : id,
                Name = nameTextBox.Text.Trim(),
                IconPath = iconPathTextBox.Text.Trim(),
                IconIndex = selectedIconIndex,
                RunAsAdministrator = runAsAdministratorCheckBox.Checked,
                Actions = actions.OrderBy(action => action.Order).ToList()
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && iconPreview != null && iconPreview.Image != null)
            {
                iconPreview.Image.Dispose();
            }

            if (disposing && embeddedIconsImageList != null)
            {
                embeddedIconsImageList.Dispose();
            }

            base.Dispose(disposing);
        }

        private Button CreateActionButton(string text, int top, EventHandler clickHandler)
        {
            Button button = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(674, top),
                Size = new Size(101, 29),
                Text = text
            };
            button.Click += clickHandler;
            return button;
        }

        private void BrowseIconButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter =
                    "Supported icon sources (*.ico;*.png;*.webp;*.exe;*.dll)|*.ico;*.png;*.webp;*.exe;*.dll|" +
                    "Image files (*.png;*.webp)|*.png;*.webp|" +
                    "Icon files (*.ico)|*.ico|" +
                    "Executable files (*.exe)|*.exe|" +
                    "Library files (*.dll)|*.dll";
                dialog.CheckFileExists = true;
                dialog.FileName = iconPathTextBox.Text;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    selectedIconIndex = 0;
                    iconPathTextBox.Text = dialog.FileName;
                    RefreshEmbeddedIcons();
                    RefreshIconPreview();
                }
            }
        }

        private void AddActionButton_Click(object sender, EventArgs e)
        {
            using (ActionDialog dialog = new ActionDialog(null))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    actions.Add(dialog.CreateAction(Guid.NewGuid(), actions.Count + 1));
                    RefreshActions();
                    SelectAction(actions.Count - 1);
                }
            }
        }

        private void EditActionButton_Click(object sender, EventArgs e)
        {
            int index = SelectedActionIndex;
            if (index < 0)
            {
                return;
            }

            LaunchAction current = actions[index];
            using (ActionDialog dialog = new ActionDialog(current))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    actions[index] = dialog.CreateAction(current.Id, current.Order);
                    RefreshActions();
                    SelectAction(index);
                }
            }
        }

        private void DeleteActionButton_Click(object sender, EventArgs e)
        {
            int index = SelectedActionIndex;
            if (index < 0)
            {
                return;
            }

            if (MessageBox.Show(this, "Delete the selected executable action?", "AppLuncher",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                actions.RemoveAt(index);
                NormalizeOrder();
                RefreshActions();
                SelectAction(Math.Min(index, actions.Count - 1));
            }
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            MoveAction(-1);
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            MoveAction(1);
        }

        private void MoveAction(int offset)
        {
            int index = SelectedActionIndex;
            int targetIndex = index + offset;
            if (index < 0 || targetIndex < 0 || targetIndex >= actions.Count)
            {
                return;
            }

            LaunchAction action = actions[index];
            actions.RemoveAt(index);
            actions.Insert(targetIndex, action);
            NormalizeOrder();
            RefreshActions();
            SelectAction(targetIndex);
        }

        private int SelectedActionIndex
        {
            get
            {
                return actionsListView.SelectedIndices.Count == 0
                    ? -1
                    : actionsListView.SelectedIndices[0];
            }
        }

        private void NormalizeOrder()
        {
            for (int index = 0; index < actions.Count; index++)
            {
                actions[index].Order = index + 1;
            }
        }

        private void RefreshActions()
        {
            NormalizeOrder();
            actionsListView.BeginUpdate();
            actionsListView.Items.Clear();

            foreach (LaunchAction action in actions)
            {
                ListViewItem row = new ListViewItem(action.Order.ToString());
                row.SubItems.Add(action.ProgramPath ?? string.Empty);
                row.SubItems.Add(action.Arguments ?? string.Empty);
                row.SubItems.Add(action.WorkingDirectory ?? string.Empty);
                row.SubItems.Add(action.WaitForExit ? "Yes" : "No");
                row.SubItems.Add(action.DelayAfterMs.ToString());
                actionsListView.Items.Add(row);
            }

            actionsListView.EndUpdate();
        }

        private void SelectAction(int index)
        {
            if (index >= 0 && index < actionsListView.Items.Count)
            {
                actionsListView.Items[index].Selected = true;
                actionsListView.Items[index].Focused = true;
                actionsListView.EnsureVisible(index);
            }
        }

        private void RefreshIconPreview()
        {
            if (iconPreview.Image != null)
            {
                iconPreview.Image.Dispose();
            }

            iconPreview.Image = IconLoader.LoadBitmap(
                iconPathTextBox.Text.Trim(), selectedIconIndex, 48, false);
        }

        private void IconPathTextBox_TextChanged(object sender, EventArgs e)
        {
            selectedIconIndex = 0;
            RefreshEmbeddedIcons();
            RefreshIconPreview();
        }

        private void RefreshEmbeddedIcons()
        {
            embeddedIconsListView.BeginUpdate();
            embeddedIconsListView.Items.Clear();
            embeddedIconsImageList.Images.Clear();

            string iconPath = iconPathTextBox.Text.Trim();
            int iconCount = 0;
            try
            {
                iconCount = IconLoader.GetEmbeddedIconCount(iconPath);
            }
            catch (Exception)
            {
                iconCount = 0;
            }

            bool showEmbeddedIcons = iconCount > 0;
            embeddedIconsLabel.Visible = showEmbeddedIcons;
            embeddedIconsListView.Visible = showEmbeddedIcons;

            if (!showEmbeddedIcons)
            {
                selectedIconIndex = 0;
                embeddedIconsListView.EndUpdate();
                return;
            }

            selectedIconIndex = Math.Min(selectedIconIndex, iconCount - 1);
            for (int iconIndex = 0; iconIndex < iconCount; iconIndex++)
            {
                string imageKey = "icon-" + iconIndex;
                embeddedIconsImageList.Images.Add(
                    imageKey,
                    IconLoader.LoadBitmap(iconPath, iconIndex, embeddedIconsImageList.ImageSize.Width, false));

                ListViewItem item = new ListViewItem("Icon " + iconIndex)
                {
                    ImageKey = imageKey,
                    Tag = iconIndex
                };
                embeddedIconsListView.Items.Add(item);

                if (iconIndex == selectedIconIndex)
                {
                    item.Selected = true;
                    item.Focused = true;
                }
            }

            embeddedIconsListView.EndUpdate();
        }

        private void EmbeddedIconsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (embeddedIconsListView.SelectedItems.Count == 0)
            {
                return;
            }

            int iconIndex = (int)embeddedIconsListView.SelectedItems[0].Tag;
            if (selectedIconIndex != iconIndex)
            {
                selectedIconIndex = iconIndex;
                RefreshIconPreview();
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show(this, "Enter a launcher name.", "AppLuncher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }

            string iconPath = iconPathTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(iconPath) &&
                (!File.Exists(iconPath) || !IconLoader.IsSupportedIconSource(iconPath)))
            {
                MessageBox.Show(this,
                    "The icon source must be an existing .ico, .png, .webp, .exe, or .dll file.",
                    "AppLuncher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }

            if (actions.Count == 0)
            {
                MessageBox.Show(this, "Add at least one executable action.", "AppLuncher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
            }
        }
    }
}
