using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AppLuncher.Helpers
{
    public static class IconLoader
    {
        public static Bitmap LoadBitmap(string iconPath, int iconIndex, int size, bool folder)
        {
            if (!string.IsNullOrWhiteSpace(iconPath) && File.Exists(iconPath))
            {
                try
                {
                    if (IsImageIconSource(iconPath))
                    {
                        return LoadImage(iconPath, size);
                    }

                    using (Icon source = LoadIcon(iconPath, iconIndex, size))
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

        public static void AddImage(ImageList imageList, string key, string iconPath, int iconIndex, bool folder)
        {
            imageList.Images.Add(key, LoadBitmap(iconPath, iconIndex, imageList.ImageSize.Width, folder));
        }

        public static bool IsSupportedIconSource(string path)
        {
            string extension = Path.GetExtension(path);
            return string.Equals(extension, ".ico", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".webp", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".exe", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".dll", StringComparison.OrdinalIgnoreCase);
        }

        public static int GetEmbeddedIconCount(string path)
        {
            if (!File.Exists(path) || !IsExecutableIconSource(path))
            {
                return 0;
            }

            return unchecked((int)ExtractIconEx(path, -1, null, null, 0));
        }

        private static Icon LoadIcon(string path, int iconIndex, int size)
        {
            string extension = Path.GetExtension(path);
            if (string.Equals(extension, ".ico", StringComparison.OrdinalIgnoreCase))
            {
                return new Icon(path, new Size(size, size));
            }

            if (IsExecutableIconSource(path))
            {
                IntPtr[] largeIcons = new IntPtr[1];
                IntPtr[] smallIcons = new IntPtr[1];
                uint extractedCount = ExtractIconEx(path, Math.Max(0, iconIndex), largeIcons, smallIcons, 1);
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

        private static Bitmap LoadImage(string path, int size)
        {
            string extension = Path.GetExtension(path);
            if (string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase))
            {
                using (Image source = Image.FromFile(path))
                {
                    return ResizeImage(source, size);
                }
            }

            return LoadShellThumbnail(path, size);
        }

        private static Bitmap LoadShellThumbnail(string path, int size)
        {
            Guid shellItemImageFactoryId = typeof(IShellItemImageFactory).GUID;
            IShellItemImageFactory imageFactory;
            int result = SHCreateItemFromParsingName(
                Path.GetFullPath(path),
                IntPtr.Zero,
                ref shellItemImageFactoryId,
                out imageFactory);

            if (result != 0)
            {
                Marshal.ThrowExceptionForHR(result);
            }

            IntPtr bitmapHandle = IntPtr.Zero;
            try
            {
                imageFactory.GetImage(
                    new NativeSize(size, size),
                    ShellItemImageFactoryFlags.ThumbnailOnly | ShellItemImageFactoryFlags.ScaleUp,
                    out bitmapHandle);

                using (Bitmap source = Image.FromHbitmap(bitmapHandle))
                {
                    return ResizeImage(source, size);
                }
            }
            finally
            {
                if (bitmapHandle != IntPtr.Zero)
                {
                    DeleteObject(bitmapHandle);
                }

                if (imageFactory != null)
                {
                    Marshal.FinalReleaseComObject(imageFactory);
                }
            }
        }

        private static Bitmap ResizeImage(Image source, int size)
        {
            Bitmap result = new Bitmap(size, size, PixelFormat.Format32bppArgb);
            float scale = Math.Min((float)size / source.Width, (float)size / source.Height);
            int width = Math.Max(1, (int)Math.Round(source.Width * scale));
            int height = Math.Max(1, (int)Math.Round(source.Height * scale));
            int left = (size - width) / 2;
            int top = (size - height) / 2;

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.Clear(Color.Transparent);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(source, new Rectangle(left, top, width, height));
            }

            return result;
        }

        private static bool IsImageIconSource(string path)
        {
            string extension = Path.GetExtension(path);
            return string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".webp", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsExecutableIconSource(string path)
        {
            string extension = Path.GetExtension(path);
            return string.Equals(extension, ".exe", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".dll", StringComparison.OrdinalIgnoreCase);
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

        [Flags]
        private enum ShellItemImageFactoryFlags
        {
            ThumbnailOnly = 0x00000008,
            ScaleUp = 0x00000100
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeSize
        {
            public NativeSize(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public int Width;
            public int Height;
        }

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

        [ComImport]
        [Guid("BCC18B79-BA16-442F-80C4-8A59C30C463B")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItemImageFactory
        {
            void GetImage(
                NativeSize size,
                ShellItemImageFactoryFlags flags,
                out IntPtr bitmapHandle);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        private static extern int SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            IntPtr bindContext,
            ref Guid interfaceId,
            [MarshalAs(UnmanagedType.Interface)] out IShellItemImageFactory imageFactory);

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

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr objectHandle);
    }
}
