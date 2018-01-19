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
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace libpixy.net.Shell
{
    public static class ShellImageList
    {
        private static IntPtr smallImageListHandle = IntPtr.Zero, largeImageListHandle = IntPtr.Zero;
        private static Hashtable imageTable;
        
        private const int TVSIL_NORMAL = 0;
        private const int TVSIL_SMALL = 1;

        static ShellImageList()
        {
            imageTable = new Hashtable();

            libpixy.net.Shell.API.SHGFI flag = libpixy.net.Shell.API.SHGFI.USEFILEATTRIBUTES | libpixy.net.Shell.API.SHGFI.SYSICONINDEX | libpixy.net.Shell.API.SHGFI.SMALLICON;
            libpixy.net.Shell.API.SHFILEINFO shfiSmall = new libpixy.net.Shell.API.SHFILEINFO();
            smallImageListHandle = libpixy.net.Shell.API.SHGetFileInfo(".txt", libpixy.net.Shell.API.FILE_ATTRIBUTE.NORMAL, ref shfiSmall, libpixy.net.Shell.API.cbFileInfo, flag);

            flag = libpixy.net.Shell.API.SHGFI.USEFILEATTRIBUTES | libpixy.net.Shell.API.SHGFI.SYSICONINDEX | libpixy.net.Shell.API.SHGFI.LARGEICON;
            libpixy.net.Shell.API.SHFILEINFO shfiLarge = new libpixy.net.Shell.API.SHFILEINFO();
            largeImageListHandle = libpixy.net.Shell.API.SHGetFileInfo(".txt", libpixy.net.Shell.API.FILE_ATTRIBUTE.NORMAL, ref shfiLarge, libpixy.net.Shell.API.cbFileInfo, flag);
        }

        internal static void SetIconIndex(ShellItem item, int index, bool SelectedIcon)
        {
            bool HasOverlay = false; //true if it's an overlay
            int rVal = 0; //The returned Index

            libpixy.net.Shell.API.SHGFI dwflag = libpixy.net.Shell.API.SHGFI.SYSICONINDEX | libpixy.net.Shell.API.SHGFI.PIDL | libpixy.net.Shell.API.SHGFI.ICON;
            libpixy.net.Shell.API.FILE_ATTRIBUTE dwAttr = 0;
            //build Key into HashTable for this Item
            int Key = index * 256;
            if (item.Flags.isLink)
            {
                Key = Key | 1;
                dwflag = dwflag | libpixy.net.Shell.API.SHGFI.LINKOVERLAY;
                HasOverlay = true;
            }
            if (item.Flags.isShared)
            {
                Key = Key | 2;
                dwflag = dwflag | libpixy.net.Shell.API.SHGFI.ADDOVERLAYS;
                HasOverlay = true;
            }
            if (SelectedIcon)
            {
                Key = Key | 4;
                dwflag = dwflag | libpixy.net.Shell.API.SHGFI.OPENICON;
                HasOverlay = true; //not really an overlay, but handled the same
            }
            
            if (imageTable.ContainsKey(Key))
            {
                rVal = (int)imageTable[Key];
            }
            else if (!HasOverlay && !item.Flags.isHidden) //for non-overlay icons, we already have
            {                
                rVal = (int)System.Math.Floor((double)Key / 256); // the right index -- put in table
                imageTable[Key] = rVal;
            }
            else //don't have iconindex for an overlay, get it.
            {
                if (item.Flags.isFileSystem & !item.Flags.isDisk & !item.Flags.isFolder)
                {
                    dwflag = dwflag | libpixy.net.Shell.API.SHGFI.USEFILEATTRIBUTES;
                    dwAttr = dwAttr | libpixy.net.Shell.API.FILE_ATTRIBUTE.NORMAL;
                }

                PIDL pidlFull = item.PIDLFull;

                libpixy.net.Shell.API.SHFILEINFO shfiSmall = new libpixy.net.Shell.API.SHFILEINFO();
                libpixy.net.Shell.API.SHGetFileInfo(pidlFull.Ptr, dwAttr, ref shfiSmall, libpixy.net.Shell.API.cbFileInfo, dwflag | libpixy.net.Shell.API.SHGFI.SMALLICON);

                libpixy.net.Shell.API.SHFILEINFO shfiLarge = new libpixy.net.Shell.API.SHFILEINFO();
                libpixy.net.Shell.API.SHGetFileInfo(pidlFull.Ptr, dwAttr, ref shfiLarge, libpixy.net.Shell.API.cbFileInfo, dwflag | libpixy.net.Shell.API.SHGFI.LARGEICON);

                Marshal.FreeCoTaskMem(pidlFull.Ptr);

                lock (imageTable)
                {
                    rVal = libpixy.net.Shell.API.ImageList_ReplaceIcon(smallImageListHandle, -1, shfiSmall.hIcon);
                    libpixy.net.Shell.API.ImageList_ReplaceIcon(largeImageListHandle, -1, shfiLarge.hIcon);
                }

                libpixy.net.Shell.API.DestroyIcon(shfiSmall.hIcon);
                libpixy.net.Shell.API.DestroyIcon(shfiLarge.hIcon);
                imageTable[Key] = rVal;
            }

            if (SelectedIcon)
                item.SelectedImageIndex = rVal;
            else
                item.ImageIndex = rVal;
        }

        public static Icon GetIcon(int index, bool small)
        {
            IntPtr iconPtr;

            if (small)
                iconPtr = libpixy.net.Shell.API.ImageList_GetIcon(smallImageListHandle, index, libpixy.net.Shell.API.ILD.NORMAL);
            else
                iconPtr = libpixy.net.Shell.API.ImageList_GetIcon(largeImageListHandle, index, libpixy.net.Shell.API.ILD.NORMAL);

            if (iconPtr != IntPtr.Zero)
            {
                Icon icon = Icon.FromHandle(iconPtr);
                Icon retVal = (Icon)icon.Clone();
                libpixy.net.Shell.API.DestroyIcon(iconPtr);
                return retVal;
            }
            else
                return null;
        }

        public static IntPtr SmallImageList { get { return smallImageListHandle; } }
        public static IntPtr LargeImageList { get { return largeImageListHandle; } }

        #region Set Small Handle

        public static void SetSmallImageList(TreeView treeView)
        {
            libpixy.net.Shell.API.SendMessage(treeView.Handle, libpixy.net.Shell.API.WM.TVM_SETIMAGELIST, TVSIL_NORMAL, smallImageListHandle);
        }

        public static void SetSmallImageList(ListView listView)
        {
            libpixy.net.Shell.API.SendMessage(listView.Handle, libpixy.net.Shell.API.WM.LVM_SETIMAGELIST, TVSIL_SMALL, smallImageListHandle);
        }

        #endregion

        #region Set Large Handle

        public static void SetLargeImageList(ListView listView)
        {
            libpixy.net.Shell.API.SendMessage(listView.Handle, libpixy.net.Shell.API.WM.LVM_SETIMAGELIST, TVSIL_NORMAL, largeImageListHandle);
        }

        #endregion
    }
}
