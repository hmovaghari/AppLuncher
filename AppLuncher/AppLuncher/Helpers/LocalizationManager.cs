using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AppLuncher.Helpers
{
    public static class LocalizationManager
    {
        private static readonly IDictionary<string, string> PersianTexts =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "New Group", "گروه جدید" },
                { "New Launcher", "لانچر جدید" },
                { "Up", "بالا" },
                { "Edit", "ویرایش" },
                { "Delete", "حذف" },
                { "Copy", "کپی" },
                { "Paste", "چسباندن" },
                { "View", "نمایش" },
                { "Change Database", "تغییر پایگاه داده" },
                { "Support", "پشتیبانی" },
                { "Check for Updates", "بررسی به‌روزرسانی" },
                { "Check for updates at startup", "بررسی به‌روزرسانی هنگام شروع" },
                { "Theme", "تم" },
                { "Settings", "تنظیمات" },
                { "Light", "روشن" },
                { "Dark", "تاریک" },
                { "Search Options", "گزینه‌های جستجو" },
                { "Search:", "جستجو:" },
                { "Large icons", "آیکن‌های بزرگ" },
                { "Medium icons", "آیکن‌های متوسط" },
                { "Small icons", "آیکن‌های کوچک" },
                { "List", "فهرست" },
                { "Details", "جزئیات" },
                { "Open", "باز کردن" },
                { "Properties", "ویژگی‌ها" },
                { "New group...", "گروه جدید..." },
                { "New launcher...", "لانچر جدید..." },
                { "Rename...", "تغییر نام..." },
                { "Edit...", "ویرایش..." },
                { "Name", "نام" },
                { "Type", "نوع" },
                { "Actions", "اکشن‌ها" },
                { "Location", "مسیر" },
                { "Group", "گروه" },
                { "Launcher", "لانچر" },
                { "Create Group", "ایجاد گروه" },
                { "Edit Group", "ویرایش گروه" },
                { "Group name:", "نام گروه:" },
                { "OK", "تأیید" },
                { "Cancel", "انصراف" },
                { "Add Executable Action", "افزودن اکشن اجرایی" },
                { "Edit Executable Action", "ویرایش اکشن اجرایی" },
                { "Program:", "برنامه:" },
                { "Arguments:", "آرگومان‌ها:" },
                { "Working directory:", "پوشه کاری:" },
                { "Wait for this program to exit before running the next action", "تا پایان برنامه، اکشن بعدی اجرا نشود" },
                { "Delay after execution (ms):", "تأخیر پس از اجرا (میلی‌ثانیه):" },
                { "Browse...", "انتخاب..." },
                { "Create Launcher", "ایجاد لانچر" },
                { "Edit Launcher", "ویرایش لانچر" },
                { "Icon source:", "منبع آیکن:" },
                { "Executable actions (run in order)", "اکشن‌های اجرایی (به ترتیب اجرا)" },
                { "Add...", "افزودن..." },
                { "Move Up", "انتقال به بالا" },
                { "Move Down", "انتقال به پایین" },
                { "Program", "برنامه" },
                { "Arguments", "آرگومان‌ها" },
                { "Working directory", "پوشه کاری" },
                { "Wait", "انتظار" },
                { "Delay", "تأخیر" },
                { "Yes", "بله" },
                { "No", "خیر" },
                { "Include program path, arguments, and working directory", "شامل مسیر برنامه، آرگومان‌ها و پوشه کاری" },
                { "Language", "زبان" },
                { "English", "انگلیسی" },
                { "Persian", "فارسی" }
            };

        public static bool IsPersian
        {
            get
            {
                return string.Equals(
                    Properties.Settings.Default.ApplicationLanguage,
                    "fa",
                    StringComparison.OrdinalIgnoreCase);
            }
        }

        public static string Translate(string text)
        {
            string translatedText;
            if (text == null)
            {
                return null;
            }

            if (IsPersian)
            {
                return PersianTexts.TryGetValue(text, out translatedText) ? translatedText : text;
            }

            foreach (KeyValuePair<string, string> textPair in PersianTexts)
            {
                if (string.Equals(textPair.Value, text, StringComparison.Ordinal))
                {
                    return textPair.Key;
                }
            }

            return text;
        }

        public static void Apply(Form form)
        {
            form.RightToLeft = IsPersian ? RightToLeft.Yes : RightToLeft.No;
            form.RightToLeftLayout = IsPersian;
            form.Text = Translate(form.Text);
            ApplyControls(form.Controls);
        }

        public static void ApplyToolStrip(ToolStrip toolStrip)
        {
            foreach (ToolStripItem item in toolStrip.Items)
            {
                item.Text = Translate(item.Text);

                ToolStripDropDownItem dropDownItem = item as ToolStripDropDownItem;
                if (dropDownItem != null)
                {
                    ApplyToolStrip(dropDownItem.DropDown);
                }
            }
        }

        private static void ApplyControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                control.Text = Translate(control.Text);
                ApplyControls(control.Controls);
            }
        }
    }
}
