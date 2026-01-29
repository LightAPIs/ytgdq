using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace WindowsFormsApplication2
{
    /// <summary>
    /// Helper class for creating Windows shortcuts (.lnk files) using Shell32 COM interfaces.
    /// Replaces the IWshRuntimeLibrary COM reference which is not supported in .NET 8.
    /// </summary>
    public static class ShortcutHelper
    {
        public static void CreateShortcut(
            string shortcutPath,
            string targetPath,
            string workingDirectory = null,
            string description = null,
            string iconLocation = null,
            int windowStyle = 1)
        {
            var link = (IShellLink)new ShellLink();

            link.SetPath(targetPath);

            if (!string.IsNullOrEmpty(workingDirectory))
                link.SetWorkingDirectory(workingDirectory);

            if (!string.IsNullOrEmpty(description))
                link.SetDescription(description);

            if (!string.IsNullOrEmpty(iconLocation))
                link.SetIconLocation(iconLocation, 0);

            link.SetShowCmd(windowStyle);

            var file = (IPersistFile)link;
            file.Save(shortcutPath, false);
        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        private class ShellLink
        {
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        private interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
                int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName,
                int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
                int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs,
                int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath,
                int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }
    }
}
