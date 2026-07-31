using System.Drawing;
using System.Windows.Forms;

namespace AppLuncher.Helpers
{
    public static class ThemeManager
    {
        public static void Apply(Form form, bool darkTheme)
        {
            Color backColor = darkTheme ? Color.FromArgb(32, 32, 32) : SystemColors.Window;
            Color surfaceColor = darkTheme ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
            Color foreColor = darkTheme ? Color.Gainsboro : SystemColors.ControlText;

            ApplyControl(form, backColor, surfaceColor, foreColor, darkTheme);
        }

        private static void ApplyControl(
            Control control,
            Color backColor,
            Color surfaceColor,
            Color foreColor,
            bool darkTheme)
        {
            if (control is TextBox || control is ListView || control is TreeView ||
                control is NumericUpDown || control is ComboBox)
            {
                control.BackColor = backColor;
                control.ForeColor = foreColor;
            }
            else
            {
                control.BackColor = surfaceColor;
                control.ForeColor = foreColor;
            }

            ContextMenuStrip contextMenu = control.ContextMenuStrip;
            if (contextMenu != null)
            {
                ApplyToolStrip(contextMenu, surfaceColor, foreColor);
            }

            foreach (Control child in control.Controls)
            {
                ApplyControl(child, backColor, surfaceColor, foreColor, darkTheme);
            }
        }

        public static void ApplyToolStrip(ToolStrip toolStrip, Color backColor, Color foreColor)
        {
            toolStrip.BackColor = backColor;
            toolStrip.ForeColor = foreColor;

            foreach (ToolStripItem item in toolStrip.Items)
            {
                item.BackColor = backColor;
                item.ForeColor = foreColor;

                ToolStripDropDownItem dropDownItem = item as ToolStripDropDownItem;
                if (dropDownItem != null)
                {
                    ApplyToolStrip(dropDownItem.DropDown, backColor, foreColor);
                }
            }
        }
    }
}
