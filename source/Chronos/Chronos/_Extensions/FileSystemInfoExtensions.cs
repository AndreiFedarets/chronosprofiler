using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Chronos
{
    public enum IconSize
    {
        Small,
        Large,
        ExtraLarge,
    }

    public static class FileSystemInfoExtensions
    {
        private static readonly Dictionary<string, Icon> IconCache;

        static FileSystemInfoExtensions()
        {
            IconCache = new Dictionary<string, Icon>();
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string path, uint fileAttributes, ref ShellFileInfo info, uint sizeFileInfo, uint flags);

        [StructLayout(LayoutKind.Sequential)]
        private struct ShellFileInfo
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        private const uint ShgfiIcon = 0x100;
        private const uint ShgfiSmallIcon = 0x1;
        private const uint ShgfiSysiConIndex = 16384;
        private const uint ShgfiUseFileAttributes = 16;

        public static Icon GetIcon(string fullName, IconSize size)
        {
            Icon icon;
            string key = GetItemKey(fullName, size);

            if (!IconCache.TryGetValue(key, out icon))
            {
                ShellFileInfo info = new ShellFileInfo();
                uint flags = ShgfiSysiConIndex;
                if (fullName.IndexOf(":") == -1)
                {
                    flags = flags | ShgfiUseFileAttributes;
                }
                if (size == IconSize.Small)
                {
                    flags = flags | ShgfiIcon | ShgfiSmallIcon;
                }
                else
                {
                    flags = flags | ShgfiIcon;
                }
                SHGetFileInfo(fullName, 0, ref info, (uint)Marshal.SizeOf(info), flags);
                icon = Icon.FromHandle(info.hIcon);
                IconCache.Add(key, icon);
            }
            return icon;
        }

        private static string GetItemKey(string fileName, IconSize size)
        {
            string key = fileName.ToLowerInvariant();
            switch (size)
            {
                case IconSize.ExtraLarge:
                    key += "+XL"; 
                    break;
                case IconSize.Large: 
                    key += "+L"; 
                    break;
                case IconSize.Small:
                    key += "+S"; 
                    break;
            }
            return key;
        }
    }
}
