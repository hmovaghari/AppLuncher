using System;
using System.Drawing;
using System.Windows.Forms;
using AppLuncher.Helpers;

namespace AppLuncher.Forms
{
    public sealed class GroupDialog : Form
    {
        private readonly TextBox nameTextBox;

        public GroupDialog(string currentName)
        {
            Text = string.IsNullOrWhiteSpace(currentName) ? "Create Group" : "Edit Group";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(420, 125);

            Label nameLabel = new Label
            {
                AutoSize = true,
                Location = new Point(12, 18),
                Text = "Group name:"
            };

            nameTextBox = new TextBox
            {
                Location = new Point(15, 41),
                Size = new Size(390, 23),
                Text = currentName ?? string.Empty
            };

            Button okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Location = new Point(249, 82),
                Size = new Size(75, 28),
                Text = "OK"
            };
            okButton.Click += OkButton_Click;

            Button cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(330, 82),
                Size = new Size(75, 28),
                Text = "Cancel"
            };

            Controls.Add(nameLabel);
            Controls.Add(nameTextBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            AcceptButton = okButton;
            CancelButton = cancelButton;
            LocalizationManager.Apply(this);
            ThemeManager.Apply(this, Properties.Settings.Default.UseDarkTheme);
        }

        public string GroupName
        {
            get { return nameTextBox.Text.Trim(); }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            nameTextBox.Focus();
            nameTextBox.SelectAll();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                MessageBox.Show(this, "Enter a group name.", "AppLuncher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
            }
        }
    }
}
