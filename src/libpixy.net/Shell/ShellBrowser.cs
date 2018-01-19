/*
 * Copyright (c) 2018 mebiusbox software. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Globalization;

namespace libpixy.net.Shell
{
    public class ShellBrowser : Component
    {
        #region Fields

        private ShellItem desktopItem;
        private string mydocsName, mycompName, sysfolderName, mydocsPath;

        #endregion

        public ShellBrowser()
        {
            InitVars();
        }

        private void InitVars()
        {
            IntPtr tempPidl;
            API.SHFILEINFO info;

            //My Computer
            info = new API.SHFILEINFO();
            tempPidl = IntPtr.Zero;
            API.SHGetSpecialFolderLocation(IntPtr.Zero, API.CSIDL.DRIVES, out tempPidl);

            API.SHGetFileInfo(tempPidl, 0, ref info, API.cbFileInfo,
                API.SHGFI.PIDL | API.SHGFI.DISPLAYNAME | API.SHGFI.TYPENAME);

            sysfolderName = info.szTypeName;
            mycompName = info.szDisplayName;
            Marshal.FreeCoTaskMem(tempPidl);
            //

            //Dekstop
            tempPidl = IntPtr.Zero;
            API.SHGetSpecialFolderLocation(IntPtr.Zero, API.CSIDL.DESKTOP, out tempPidl);
            IntPtr desktopFolderPtr;
            API.SHGetDesktopFolder(out desktopFolderPtr);
            desktopItem = new ShellItem(tempPidl, desktopFolderPtr);
            //

            //My Documents
            uint pchEaten = 0;
            API.SFGAO pdwAttributes = 0;
            desktopItem.ShellFolder.ParseDisplayName(
                IntPtr.Zero,
                IntPtr.Zero,
                "::{450d8fba-ad25-11d0-98a8-0800361b1103}",
                ref pchEaten,
                out tempPidl,
                ref pdwAttributes);

            info = new API.SHFILEINFO();
            API.SHGetFileInfo(tempPidl, 0, ref info, API.cbFileInfo,
                API.SHGFI.PIDL | API.SHGFI.DISPLAYNAME);

            mydocsName = info.szDisplayName;
            Marshal.FreeCoTaskMem(tempPidl);

            StringBuilder path = new StringBuilder(API.MAX_PATH);
            API.SHGetFolderPath(
                    IntPtr.Zero, API.CSIDL.PERSONAL,
                    IntPtr.Zero, API.SHGFP.TYPE_CURRENT, path);
            mydocsPath = path.ToString();
            //
        }

        #region Utility Methods

        public ShellItem GetShellItem(PIDL pidlFull)
        {
            ShellItem current = DesktopItem;
            if (pidlFull.Ptr == IntPtr.Zero)
                return current;

            foreach (IntPtr pidlRel in pidlFull)
            {
                int index;
                if ((index = current.IndexOf(pidlRel)) > -1)
                {
                    current = current[index];
                }
                else
                {
                    current = null;
                    break;
                }
            }

            return current;
        }

        public ShellItem[] GetPath(ShellItem item)
        {
            List<ShellItem> pathList = new List<ShellItem>();

            ShellItem currentItem = item;
            while (currentItem.ParentItem != null)
            {
                pathList.Add(currentItem);
                currentItem = currentItem.ParentItem;
            }
            pathList.Add(currentItem);
            pathList.Reverse();

            return pathList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetRealPath(ShellItem item)
        {
            if (item.Equals(DesktopItem))
            {
                return "::{450d8fba-ad25-11d0-98a8-0800361b1103}";
            }
            else if (item.Type == SystemFolderName)
            {
                IntPtr strr = Marshal.AllocCoTaskMem(API.MAX_PATH * 2 + 4);
                Marshal.WriteInt32(strr, 0, 0);
                StringBuilder buf = new StringBuilder(API.MAX_PATH);

                if (item.ParentItem.ShellFolder.GetDisplayNameOf(
                                item.PIDLRel.Ptr,
                                API.SHGNO.FORPARSING,
                                strr) == API.S_OK)
                {
                    API.StrRetToBuf(strr, item.PIDLRel.Ptr, buf, API.MAX_PATH);
                }

                Marshal.FreeCoTaskMem(strr);

                return buf.ToString();
            }
            else
            {
                return item.Path;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ConvertPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            string newPath = path.Trim();

            if (newPath.StartsWith(
                    string.Format(@"{0}\", MyComputerName),
                    false,
                    CultureInfo.InstalledUICulture) && newPath.Length > 12)
                newPath = newPath.Substring(path.IndexOf('\\') + 1);

            if (!newPath.EndsWith(@":\") && newPath.EndsWith(@"\"))
                newPath = newPath.Substring(0, newPath.Length - 1);

            if (newPath.EndsWith(@"\"))
                newPath = newPath.Substring(0, newPath.Length - 1);

            return newPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ShellItem Search(string path)
        {
            string realPath = ConvertPath(path);

            if (string.IsNullOrEmpty(realPath))
                return null;
            else if (string.Compare(path, "desktop", true) == 0)
                return DesktopItem;

            string[] pathParts = realPath.Split('\\');
            ShellItem current = DesktopItem;
            if (pathParts[0].EndsWith(":"))
            {
                DesktopItem.ExpandFolders(IntPtr.Zero);
                current = DesktopItem.SubFolders[MyComputerName];
                pathParts[0] += "\\";
            }

            int i;
            for (i = 0; i < pathParts.Length; ++i)
            {
                current.ExpandFolders(IntPtr.Zero);
                ShellItem next = null;
                foreach (ShellItem item in current.SubFolders)
                {
                    if (string.Equals(item.ParseName, pathParts[i]))
                    {
                        next = item;
                        break;
                    }
                }

                if (next != null)
                {
                    current = next;
                }
                else
                {
                    break;
                }
            }

            if (i < pathParts.Length)
            {
                IShellFolder folder = current.ShellFolder;
                uint eaten = 0;
                IntPtr pidl = IntPtr.Zero;
                API.SFGAO attrib = 0;
                folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, pathParts[i], ref eaten, out pidl, ref attrib);
                if (pidl != IntPtr.Zero)
                {
                    ShellItem item = new ShellItem(current, pidl);
                    return item;
                }
            }
            else if (i == pathParts.Length)
            {
                return current;
            }

            return null;
        }

        #endregion

        #region Properties

        public ShellItem DesktopItem { get { return desktopItem; } }

        public string MyDocumentsName { get { return mydocsName; } }
        public string MyComputerName { get { return mycompName; } }
        public string SystemFolderName { get { return sysfolderName; } }

        public string MyDocumentsPath { get { return mydocsPath; } }

        #endregion
    }
}
