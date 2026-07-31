using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AppLuncher.Helpers
{
    public static class IconLoader
    {
        public static Bitmap LoadBitmap(string iconPath, int size, bool folder)
        {
            if (!string.IsNullOrWhiteSpace(iconPath) && File.Exists(iconPath))
            {
                try
                {
                    using (Icon source = LoadIcon(iconPath, size))
                    using (Icon resized = new Icon(source, new Size(size, size)))
                    {
                        return resized.ToBitmap();
                    }
                }
                catch (Exception exception)
                {
                    if (exception is OutOfMemoryException)
                    {
                        throw;
                    }
                }
            }

            using (Icon fallback = folder ? LoadFolderIcon(size) : (Icon)SystemIcons.Application.Clone())
            using (Icon resized = new Icon(fallback, new Size(size, size)))
            {
                return resized.ToBitmap();
            }
        }

        public static void AddImage(ImageList imageList, string key, string iconPath, bool folder)
        {
            imageList.Images.Add(key, LoadBitmap(iconPath, imageList.ImageSize.Width, folder));
        }

        private static Icon LoadIcon(string path, int size)
        {
            string extension = Path.GetExtension(path);
            if (string.Equals(extension, ".ico", StringComparison.OrdinalIgnoreCase))
            {
                return new Icon(path, new Size(size, size));
            }

            if (string.Equals(extension, ".exe", StringComparison.OrdinalIgnoreCase))
            {
                Icon associatedIcon = Icon.ExtractAssociatedIcon(path);
                if (associatedIcon != null)
                {
                    return associatedIcon;
                }
            }

            if (string.Equals(extension, ".exe", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".dll", StringComparison.OrdinalIgnoreCase))
            {
                IntPtr[] largeIcons = new IntPtr[1];
                IntPtr[] smallIcons = new IntPtr[1];
                uint extractedCount = ExtractIconEx(path, 0, largeIcons, smallIcons, 1);
                IntPtr selectedHandle = size <= 24 ? smallIcons[0] : largeIcons[0];
                IntPtr unusedHandle = size <= 24 ? largeIcons[0] : smallIcons[0];

                try
                {
                    if (extractedCount > 0 && selectedHandle != IntPtr.Zero)
                    {
                        return (Icon)Icon.FromHandle(selectedHandle).Clone();
                    }
                }
                finally
                {
                    if (selectedHandle != IntPtr.Zero)
                    {
                        DestroyIcon(selectedHandle);
                    }

                    if (unusedHandle != IntPtr.Zero)
                    {
                        DestroyIcon(unusedHandle);
                    }
                }
            }

            throw new ArgumentException("The selected file does not contain a readable icon.", "path");
        }

        private static Icon LoadFolderIcon(int size)
        {
            ShFileInfo fileInfo = new ShFileInfo();
            uint flags = ShgfiIcon | ShgfiUseFileAttributes |
                (size <= 24 ? ShgfiSmallIcon : ShgfiLargeIcon);

            IntPtr result = SHGetFileInfo(
                "Folder",
                FileAttributeDirectory,
                ref fileInfo,
                (uint)Marshal.SizeOf(typeof(ShFileInfo)),
                flags);

            if (result == IntPtr.Zero || fileInfo.IconHandle == IntPtr.Zero)
            {
                return (Icon)SystemIcons.Application.Clone();
            }

            try
            {
                return (Icon)Icon.FromHandle(fileInfo.IconHandle).Clone();
            }
            finally
            {
                DestroyIcon(fileInfo.IconHandle);
            }
        }

        private const uint ShgfiIcon = 0x000000100;
        private const uint ShgfiLargeIcon = 0x000000000;
        private const uint ShgfiSmallIcon = 0x000000001;
        private const uint ShgfiUseFileAttributes = 0x000000010;
        private const uint FileAttributeDirectory = 0x00000010;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct ShFileInfo
        {
            public IntPtr IconHandle;
            public int IconIndex;
            public uint Attributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string DisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string TypeName;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(
            string path,
            uint fileAttributes,
            ref ShFileInfo fileInfo,
            uint fileInfoSize,
            uint flags);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern uint ExtractIconEx(
            string fileName,
            int iconIndex,
            IntPtr[] largeIcons,
            IntPtr[] smallIcons,
            uint iconCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr iconHandle);
    }
}
