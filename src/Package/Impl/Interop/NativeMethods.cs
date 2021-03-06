﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.VisualStudio.R.Package.Interop {
    internal static class NativeMethods {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint RegisterClipboardFormat(string lpszFormat);

        [DllImport("Oleaut32.dll", PreserveSig = false)]
        public static extern void VariantClear(IntPtr variant);

        [DllImport("Oleaut32.dll", PreserveSig = false)]
        public static extern void VariantInit(IntPtr variant);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public const int
            IDOK = 1,
            IDCANCEL = 2,
            IDABORT = 3,
            IDRETRY = 4,
            IDIGNORE = 5,
            IDYES = 6,
            IDNO = 7,
            IDCLOSE = 8,
            IDHELP = 9,
            IDTRYAGAIN = 10,
            IDCONTINUE = 11;

        [DllImport("shell32.dll")]
        public static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, uint cidl, IntPtr apidl, uint dwFlags);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ILCreateFromPath(string fileName);

        [DllImport("shell32.dll")]
        public static extern void ILFree(IntPtr pidl);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetDriveType([MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName);

        public enum DriveType {
            /// <summary>The drive type cannot be determined.</summary>
            Unknown = 0,
            /// <summary>The root path is invalid, for example, no volume is mounted at the path.</summary>
            Error = 1,
            /// <summary>The drive is a type that has removable media, for example, a floppy drive or removable hard disk.</summary>
            Removable = 2,
            /// <summary>The drive is a type that cannot be removed, for example, a fixed hard drive.</summary>
            Fixed = 3,
            /// <summary>The drive is a remote (network) drive.</summary>
            Remote = 4,
            /// <summary>The drive is a CD-ROM drive.</summary>
            CDROM = 5,
            /// <summary>The drive is a RAM disk.</summary>
            RAMDisk = 6
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        internal static extern bool PathIsUNC([MarshalAs(UnmanagedType.LPWStr), In] string pszPath);
    }
}
