using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AppLuncher.Helpers;
using AppLuncher.Models;

namespace AppLuncher.Forms
{
    public sealed class ActionDialog : Form
    {
        private readonly TextBox programTextBox;
        private readonly TextBox argumentsTextBox;
        private readonly TextBox workingDirectoryTextBox;
        private readonly CheckBox waitForExitCheckBox;
        private readonly NumericUpDown delayNumeric;

        public ActionDialog(LaunchAction action)
        {
            LaunchAction source = action ?? new LaunchAction();

            Text = action == null ? "Add Executable Action" : "Edit Executable Action";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(620, 292);

            programTextBox = AddFileRow("Program:", source.ProgramPath, 15, BrowseProgram_Click);
            argumentsTextBox = AddTextRow("Arguments:", source.Arguments, 72);
            workingDirectoryTextBox = AddFileRow("Working directory:", source.WorkingDirectory, 129, BrowseDirectory_Click);

            waitForExitCheckBox = new CheckBox
            {
                AutoSize = true,
                Checked = source.WaitForExit,
                Location = new Point(130, 177),
                Text = "Wait for this program to exit before running the next action"
            };

            Label delayLabel = new Label
            {
                AutoSize = true,
                Location = new Point(15, 220),
                Text = "Delay after execution (ms):"
            };

            delayNumeric = new NumericUpDown
            {
                Location = new Point(190, 216),
                Maximum = 86400000,
                Size = new Size(130, 23),
                ThousandsSeparator = true,
                Value = Math.Max(0, source.DelayAfterMs)
            };

            Button okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Location = new Point(448, 252),
                Size = new Size(75, 28),
                Text = "OK"
            };
            okButton.Click += OkButton_Click;

            Button cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(529, 252),
                Size = new Size(75, 28),
                Text = "Cancel"
            };

            Controls.Add(waitForExitCheckBox);
            Controls.Add(delayLabel);
            Controls.Add(delayNumeric);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            AcceptButton = okButton;
            CancelButton = cancelButton;
            LocalizationManager.Apply(this);
            ThemeManager.Apply(this, Properties.Settings.Default.UseDarkTheme);
        }

        public LaunchAction CreateAction(Guid id, int order)
        {
            return new LaunchAction
            {
                Id = id == Guid.Empty ? Guid.NewGuid() : id,
                ProgramPath = programTextBox.Text.Trim(),
                Arguments = argumentsTextBox.Text,
                WorkingDirectory = workingDirectoryTextBox.Text.Trim(),
                WaitForExit = waitForExitCheckBox.Checked,
                DelayAfterMs = Decimal.ToInt32(delayNumeric.Value),
                Order = order
            };
        }

        private TextBox AddTextRow(string labelText, string value, int top)
        {
            Label label = new Label
            {
                AutoSize = true,
                Location = new Point(15, top + 4),
                Text = labelText
            };

            TextBox textBox = new TextBox
            {
                Location = new Point(130, top),
                Size = new Size(474, 23),
                Text = value ?? string.Empty
            };

            Controls.Add(label);
            Controls.Add(textBox);
            return textBox;
        }

        private TextBox AddFileRow(string labelText, string value, int top, EventHandler browseHandler)
        {
            Label label = new Label
            {
                AutoSize = true,
                Location = new Point(15, top + 4),
                Text = labelText
            };

            TextBox textBox = new TextBox
            {
                Location = new Point(130, top),
                Size = new Size(393, 23),
                Text = value ?? string.Empty
            };

            Button browseButton = new Button
            {
                Location = new Point(529, top - 1),
                Size = new Size(75, 25),
                Text = "Browse..."
            };
            browseButton.Click += browseHandler;

            Controls.Add(label);
            Controls.Add(textBox);
            Controls.Add(browseButton);
            return textBox;
        }

        private void BrowseProgram_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Programs (*.exe;*.bat;*.cmd;*.com)|*.exe;*.bat;*.cmd;*.com|All files (*.*)|*.*";
                dialog.CheckFileExists = true;
                dialog.FileName = programTextBox.Text;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    programTextBox.Text = dialog.FileName;
                    if (string.IsNullOrWhiteSpace(workingDirectoryTextBox.Text))
                    {
                        workingDirectoryTextBox.Text = Path.GetDirectoryName(dialog.FileName);
                    }
                }
            }
        }

        private void BrowseDirectory_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the working directory.";
                dialog.SelectedPath = workingDirectoryTextBox.Text;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    workingDirectoryTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(programTextBox.Text))
            {
                MessageBox.Show(this, "Select a program to execute.", "AppLuncher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
            }
        }
    }
}
