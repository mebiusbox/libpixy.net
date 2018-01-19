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
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;

namespace libpixy.net.Shell
{
    public sealed class ShellItem : IEnumerable, IDisposable, IComparable
    {
        #region Fields

        /// <summary>
        /// 汎用フラグ
        /// </summary>
        public struct ItemFlags
        {
            public bool isFolder;
            public bool isLink;
            public bool isShared;
            public bool isFileSystem;
            public bool isHidden;
            public bool hasSubFolders;
            public bool isBrowsable;
            public bool isDisk;
            public bool filesExpanded;
            public bool foldersExpanded;
            public bool canRename;
            public bool canRead;
            public bool updateShellFolder;
        };

        public static ShellItem s_desktopItem = null;
        public static ShellItem DesktopItem
        {
            get
            {
                if (s_desktopItem == null)
                {
                    //Dekstop
                    IntPtr tempPidl = IntPtr.Zero;
                    libpixy.net.Shell.API.SHGetSpecialFolderLocation(IntPtr.Zero, libpixy.net.Shell.API.CSIDL.DESKTOP, out tempPidl);
                    IntPtr desktopFolderPtr;
                    libpixy.net.Shell.API.SHGetDesktopFolder(out desktopFolderPtr);
                    s_desktopItem = new ShellItem(tempPidl, desktopFolderPtr);
                }

                return s_desktopItem;
            }
        }

        private ShellItem    m_parentItem;
        private IShellFolder m_shellFolder;
        private IntPtr m_shellFolderPtr;
        private ShellItemCollection m_files;
        private ShellItemCollection m_folders;
        private PIDL m_pidlRel;
        private short m_sortFlag;
        private int m_imageIndex;
        private int m_selectedImageIndex;
        private ItemFlags m_flags;

        private string m_text;
        private string m_path;
        private string m_type;
        private string m_pname;

        private bool m_disposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// 「デスクトップ」を設定する
        /// </summary>
        /// <param name="pidl"></param>
        /// <param name="shellFolderPtr"></param>
        internal ShellItem(
            IntPtr pidl,
            IntPtr shellFolderPtr)
        {
            m_shellFolderPtr = shellFolderPtr;
            m_shellFolder = (IShellFolder)Marshal.GetTypedObjectForIUnknown(shellFolderPtr, typeof(IShellFolder));
            m_files = new ShellItemCollection(this);
            m_folders = new ShellItemCollection(this);
            m_flags = new ItemFlags();
            m_pidlRel = new PIDL(pidl, false);
            m_text = "Desktop";
            m_path = "Desktop";
            m_pname = "::{00021400-0000-0000-C000-000000000046}";
            SetAttributesDesktop(this);

            libpixy.net.Shell.API.SHFILEINFO info = new API.SHFILEINFO();
            libpixy.net.Shell.API.SHGetFileInfo(m_pidlRel.Ptr, 0, ref info, libpixy.net.Shell.API.cbFileInfo,
                libpixy.net.Shell.API.SHGFI.PIDL | libpixy.net.Shell.API.SHGFI.TYPENAME | libpixy.net.Shell.API.SHGFI.SYSICONINDEX);

            m_type = info.szTypeName;

            libpixy.net.Shell.ShellImageList.SetIconIndex(this, info.iIcon, false);
            libpixy.net.Shell.ShellImageList.SetIconIndex(this, info.iIcon, true);

            m_sortFlag = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="pidl"></param>
        /// <param name="shellFolderPtr"></param>
        public ShellItem(
            ShellItem parentItem,
            IntPtr pidl,
            IntPtr shellFolderPtr)
        {
            m_parentItem = parentItem;
            m_shellFolderPtr = shellFolderPtr;
            m_shellFolder = (IShellFolder)Marshal.GetTypedObjectForIUnknown(shellFolderPtr, typeof(IShellFolder));
            m_files = new ShellItemCollection(this);
            m_folders = new ShellItemCollection(this);
            m_flags = new ItemFlags();
            m_pidlRel = new PIDL(pidl, false);

            SetText(this);
            SetPath(this);
            SetAttributesFolder(this);
            SetInfo(this);

            m_sortFlag = MakeSortFlag(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="pidl"></param>
        public ShellItem(
            ShellItem parentItem,
            IntPtr pidl)
        {
            m_parentItem = parentItem;
            m_flags = new ItemFlags();
            m_pidlRel = new PIDL(pidl, false);

            SetText(this);
            SetPath(this);
            SetAttributesFile(this);
            SetInfo(this);

            m_sortFlag = MakeSortFlag(this);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ShellItem()
        {
            ((IDisposable)this).Dispose();
        }

        #endregion

        #region Init Methods

        private static void SetText(ShellItem item)
        {
            IntPtr strr = Marshal.AllocCoTaskMem(libpixy.net.Shell.API.MAX_PATH * 2 + 4);
            Marshal.WriteInt32(strr, 0, 0);
            StringBuilder buf = new StringBuilder(libpixy.net.Shell.API.MAX_PATH);
            if (item.ParentItem.ShellFolder.GetDisplayNameOf(
                item.PIDLRel.Ptr,
                libpixy.net.Shell.API.SHGNO.INFOLDER,
                strr) == libpixy.net.Shell.API.S_OK)
            {
                libpixy.net.Shell.API.StrRetToBuf(strr, item.PIDLRel.Ptr, buf, libpixy.net.Shell.API.MAX_PATH);
                item.m_text = buf.ToString();
            }

            if (item.ParentItem.ShellFolder.GetDisplayNameOf(
                item.PIDLRel.Ptr,
                libpixy.net.Shell.API.SHGNO.INFOLDER | libpixy.net.Shell.API.SHGNO.FORPARSING,
                strr) == libpixy.net.Shell.API.S_OK)
            {
                libpixy.net.Shell.API.StrRetToBuf(strr, item.PIDLRel.Ptr, buf, libpixy.net.Shell.API.MAX_PATH);
                item.m_pname = buf.ToString();
            }

            if (item.ParentItem == ShellItem.DesktopItem)
            {
                API.SHFILEINFO info = new API.SHFILEINFO();
                API.SHGetFileInfo(item.PIDLRel.Ptr, 0, ref info, API.cbFileInfo,
                    API.SHGFI.PIDL | API.SHGFI.DISPLAYNAME | API.SHGFI.TYPENAME);
                item.m_text = info.szDisplayName;
                item.m_type = info.szTypeName;
            }

            Marshal.FreeCoTaskMem(strr);
        }

        private static void SetPath(ShellItem item)
        {
            IntPtr strr = Marshal.AllocCoTaskMem(libpixy.net.Shell.API.MAX_PATH * 2 + 4);
            Marshal.WriteInt32(strr, 0, 0);
            StringBuilder buf = new StringBuilder(libpixy.net.Shell.API.MAX_PATH);
            if (item.ParentItem.ShellFolder.GetDisplayNameOf(
                item.PIDLRel.Ptr,
                libpixy.net.Shell.API.SHGNO.FORADDRESSBAR | libpixy.net.Shell.API.SHGNO.FORPARSING,
                strr) == libpixy.net.Shell.API.S_OK)
            {
                libpixy.net.Shell.API.StrRetToBuf(strr, item.PIDLRel.Ptr, buf, libpixy.net.Shell.API.MAX_PATH);
                item.m_path = buf.ToString();
            }

            Marshal.FreeCoTaskMem(strr);
        }

        private static void SetInfo(ShellItem item)
        {
            PIDL pidlFull = item.PIDLFull;
            libpixy.net.Shell.API.SHFILEINFO info = new API.SHFILEINFO();
            libpixy.net.Shell.API.SHGetFileInfo(pidlFull.Ptr, 0, ref info, libpixy.net.Shell.API.cbFileInfo,
                libpixy.net.Shell.API.SHGFI.PIDL | libpixy.net.Shell.API.SHGFI.TYPENAME | libpixy.net.Shell.API.SHGFI.SYSICONINDEX);
            pidlFull.Free();

            libpixy.net.Shell.ShellImageList.SetIconIndex(item, info.iIcon, false);
            libpixy.net.Shell.ShellImageList.SetIconIndex(item, info.iIcon, true);

            item.m_type = info.szTypeName;
        }

        private static void SetAttributesDesktop(ShellItem item)
        {
            item.m_flags.isFolder = true;
            item.m_flags.isLink = false;
            item.m_flags.isShared = false;
            item.m_flags.isFileSystem = true;
            item.m_flags.isHidden = false;
            item.m_flags.hasSubFolders = true;
            item.m_flags.isBrowsable = true;
            item.m_flags.canRename = false;
            item.m_flags.canRead = true;
        }

        private static void SetAttributesFolder(ShellItem item)
        {
            libpixy.net.Shell.API.SFGAO attribs =
                libpixy.net.Shell.API.SFGAO.SHARE |
                libpixy.net.Shell.API.SFGAO.FILESYSTEM |
                libpixy.net.Shell.API.SFGAO.HIDDEN |
                libpixy.net.Shell.API.SFGAO.HASSUBFOLDER |
                libpixy.net.Shell.API.SFGAO.BROWSABLE |
                libpixy.net.Shell.API.SFGAO.CANRENAME |
                libpixy.net.Shell.API.SFGAO.STORAGE;
            item.ParentItem.ShellFolder.GetAttributesOf(1, new IntPtr[] { item.PIDLRel.Ptr }, ref attribs);

            item.m_flags.isFolder = true;
            item.m_flags.isLink = false;
            item.m_flags.isShared = (attribs & API.SFGAO.SHARE) != 0;
            item.m_flags.isFileSystem = (attribs & API.SFGAO.FILESYSTEM) != 0;
            item.m_flags.isHidden = (attribs & API.SFGAO.HIDDEN) != 0;
            item.m_flags.hasSubFolders = (attribs & API.SFGAO.HASSUBFOLDER) != 0;
            item.m_flags.isBrowsable = (attribs & API.SFGAO.BROWSABLE) != 0;
            item.m_flags.canRename = (attribs & API.SFGAO.CANRENAME) != 0;
            item.m_flags.canRead = (attribs & API.SFGAO.STORAGE) != 0;
            item.m_flags.isDisk = (item.Path.Length == 3 && item.Path.EndsWith(":\\"));
        }

        private static void SetAttributesFile(ShellItem item)
        {
            libpixy.net.Shell.API.SFGAO attribs =
                libpixy.net.Shell.API.SFGAO.LINK |
                libpixy.net.Shell.API.SFGAO.SHARE |
                libpixy.net.Shell.API.SFGAO.FILESYSTEM |
                libpixy.net.Shell.API.SFGAO.HIDDEN |
                libpixy.net.Shell.API.SFGAO.CANRENAME |
                libpixy.net.Shell.API.SFGAO.STREAM;
            item.ParentItem.ShellFolder.GetAttributesOf(1, new IntPtr[] { item.PIDLRel.Ptr }, ref attribs);

            item.m_flags.isFolder = false;
            item.m_flags.isLink = (attribs & API.SFGAO.LINK) != 0;
            item.m_flags.isShared = (attribs & API.SFGAO.SHARE) != 0;
            item.m_flags.isFileSystem = (attribs & API.SFGAO.FILESYSTEM) != 0;
            item.m_flags.isHidden = (attribs & API.SFGAO.HIDDEN) != 0;
            item.m_flags.hasSubFolders = false;
            item.m_flags.isBrowsable = false;
            item.m_flags.canRename = (attribs & API.SFGAO.CANRENAME) != 0;
            item.m_flags.canRead = (attribs & API.SFGAO.STREAM) != 0;
            item.m_flags.isDisk = false;
        }

        #endregion

        #region Browse Methods

        public bool ExpandFiles(
            IntPtr winHandle)
        {
            if (!Flags.filesExpanded)
            {
                IntPtr fileEnumPtr = IntPtr.Zero;
                IEnumIDList fileEnum = null;
                IntPtr pidlSubItem;
                int celtFetched;

                libpixy.net.Shell.API.SHCONTF contf =
                    libpixy.net.Shell.API.SHCONTF.FOLDERS |
                    libpixy.net.Shell.API.SHCONTF.NONFOLDERS |
                    libpixy.net.Shell.API.SHCONTF.INCLUDEHIDDEN;

                try
                {
                    if (this.Equals(DesktopItem) || m_parentItem.Equals(DesktopItem))
                    {
                        if (ShellFolder.EnumObjects(
                            winHandle,
                            contf,
                            out fileEnumPtr) == libpixy.net.Shell.API.S_OK)
                        {
                            fileEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(fileEnumPtr, typeof(IEnumIDList));
                            libpixy.net.Shell.API.SFGAO attribs = libpixy.net.Shell.API.SFGAO.FOLDER | libpixy.net.Shell.API.SFGAO.STREAM;
                            while (fileEnum.Next(1, out pidlSubItem, out celtFetched) == libpixy.net.Shell.API.S_OK && celtFetched == 1)
                            {
                                ShellFolder.GetAttributesOf(1, new IntPtr[] { pidlSubItem }, ref attribs);
                                if ((attribs & libpixy.net.Shell.API.SFGAO.FOLDER) == 0)
                                {
                                    ShellItem newItem = new ShellItem(this, pidlSubItem);
                                    if (!SubFolders.Contains(newItem.Text))
                                    {
                                        SubFiles.Add(newItem);
                                    }
                                }
                                else if ((attribs & libpixy.net.Shell.API.SFGAO.STREAM) == libpixy.net.Shell.API.SFGAO.STREAM)
                                {// zip
                                    ShellItem newItem = new ShellItem(this, pidlSubItem);
                                    if (!SubFolders.Contains(newItem.Text))
                                    {
                                        SubFiles.Add(newItem);
                                    }
                                }
                                else
                                {
                                    Marshal.FreeCoTaskMem(pidlSubItem);
                                }
                            }

                            SubFiles.Sort();
                            m_flags.filesExpanded = true;
                        }
                    }
                    else
                    {
                        if (ShellFolder.EnumObjects(
                            winHandle,
                            contf,
                            out fileEnumPtr) == libpixy.net.Shell.API.S_OK)
                        {
                            fileEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(fileEnumPtr, typeof(IEnumIDList));
                            while (fileEnum.Next(1, out pidlSubItem, out celtFetched) == libpixy.net.Shell.API.S_OK && celtFetched == 1)
                            {
                                ShellItem newItem = new ShellItem(this, pidlSubItem);
                                SubFiles.Add(newItem);
                            }

                            SubFiles.Sort();
                            m_flags.filesExpanded = true;
                        }
                    }
                }
                catch (Exception) { }
                finally
                {
                    #region Free
                    if (fileEnum != null)
                    {
                        Marshal.ReleaseComObject(fileEnum);
                        Marshal.Release(fileEnumPtr);
                    }
                    #endregion
                }
            }

            return Flags.filesExpanded;
        }

        public bool ExpandFolders(
            IntPtr winHandle)
        {
            if (!Flags.foldersExpanded)
            {
                IntPtr fileEnumPtr = IntPtr.Zero;
                IEnumIDList fileEnum = null;
                IntPtr pidlSubItem;
                int celtFetched;

                libpixy.net.Shell.API.SHCONTF contf =
                    libpixy.net.Shell.API.SHCONTF.FOLDERS |
                    libpixy.net.Shell.API.SHCONTF.INCLUDEHIDDEN;

                try
                {
                     if (ShellFolder.EnumObjects(
                            winHandle,
                            contf,
                            out fileEnumPtr) == libpixy.net.Shell.API.S_OK)
                    {
                        fileEnum = (IEnumIDList)Marshal.GetTypedObjectForIUnknown(fileEnumPtr, typeof(IEnumIDList));
                        while (fileEnum.Next(1, out pidlSubItem, out celtFetched) == libpixy.net.Shell.API.S_OK && celtFetched == 1)
                        {
                            libpixy.net.Shell.API.SFGAO attribs = libpixy.net.Shell.API.SFGAO.FOLDER | libpixy.net.Shell.API.SFGAO.STREAM;
                            ShellFolder.GetAttributesOf(1, new IntPtr[] { pidlSubItem }, ref attribs);
                            if ((attribs & API.SFGAO.STREAM) == API.SFGAO.STREAM)
                            {
                                //zip
                                Marshal.FreeCoTaskMem(pidlSubItem);
                                continue;
                            }

                            IntPtr shellFolderPtr;
                            if (ShellFolder.BindToObject(
                                pidlSubItem,
                                IntPtr.Zero,
                                ref libpixy.net.Shell.API.IID_IShellFolder,
                                out shellFolderPtr) == libpixy.net.Shell.API.S_OK)
                            {
                                ShellItem newItem = new ShellItem(
                                    this,
                                    pidlSubItem,
                                    shellFolderPtr);
                                SubFolders.Add(newItem);
                            }
                        }

                        SubFolders.Sort();
                        m_flags.foldersExpanded = true;
                    }
                }
                catch (Exception) { }
                finally
                {
                    #region Free
                    if (fileEnum != null)
                    {
                        Marshal.ReleaseComObject(fileEnum);
                        Marshal.Release(fileEnumPtr);
                    }
                    #endregion
                }
            }

            return Flags.foldersExpanded;
        }

        public void Clear()
        {
            try
            {
                foreach (IDisposable item in m_files)
                {
                    item.Dispose();
                }

                foreach (IDisposable item in m_folders)
                {
                    item.Dispose();
                }

                m_flags.filesExpanded = false;
                m_flags.foldersExpanded = false;
            }
            catch (Exception) { }
        }

        #endregion

        #region Properties

        public ShellItem ParentItem { get { return m_parentItem; } }
        public ShellItemCollection SubFiles { get { return m_files; } }
        public ShellItemCollection SubFolders { get { return m_folders; } }
        public IShellFolder ShellFolder
        {
            get
            {
                if (m_flags.updateShellFolder)
                {
                    Marshal.ReleaseComObject(m_shellFolder);
                    Marshal.Release(m_shellFolderPtr);

                    if (ParentItem.ShellFolder.BindToObject(
                        m_pidlRel.Ptr,
                        IntPtr.Zero,
                        ref libpixy.net.Shell.API.IID_IShellFolder,
                        out m_shellFolderPtr) == libpixy.net.Shell.API.S_OK)
                    {
                        m_shellFolder = (IShellFolder)Marshal.GetTypedObjectForIUnknown(m_shellFolderPtr, typeof(IShellFolder));
                    }

                    m_flags.updateShellFolder = false;
                }

                return m_shellFolder;
            }
        }

        public int ImageIndex
        {
            get { return m_imageIndex; }
            set { m_imageIndex = value; }
        }

        public int SelectedImageIndex
        {
            get { return m_selectedImageIndex; }
            set { m_selectedImageIndex = value; }
        }

        public PIDL PIDLRel { get { return m_pidlRel; } }
        public PIDL PIDLFull
        {
            get
            {
                PIDL pidlFull = new PIDL(m_pidlRel.Ptr, true);
                ShellItem current = ParentItem;
                while (current != null)
                {
                    pidlFull.Insert(current.PIDLRel.Ptr);
                    current = current.ParentItem;
                }

                return pidlFull;
            }
        }

        public string Text { get { return m_text; } }
        public string Path { get { return m_path; } }
        public string Type { get { return m_type; } }
        public string ParseName { get { return m_pname; } }
        public short SortFlag { get { return m_sortFlag; } }
        public ItemFlags Flags { get { return m_flags; } }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return new ShellItemEnumerator(this);
        }

        #endregion

        #region IDisposable

        void IDisposable.Dispose()
        {
            if (!m_disposed)
            {
                DisposeShellItem();
                GC.SuppressFinalize(this);
            }
        }

        private void DisposeShellItem()
        {
            m_disposed = true;

            if (ShellFolder != null)
            {
                Marshal.ReleaseComObject(ShellFolder);
                m_shellFolder = null;
            }

            if (m_shellFolderPtr != IntPtr.Zero)
            {
                try
                {
                    Marshal.Release(m_shellFolderPtr);
                }
                catch (Exception) { }
                finally
                {
                    m_shellFolderPtr = IntPtr.Zero;
                }
            }

            PIDLRel.Free();
        }

        #endregion

        #region IComparable

        private static short MakeSortFlag(ShellItem item)
        {
            if (item.Flags.isFolder)
            {
                if (item.Flags.isDisk)
                    return 1;
                /*
                if (item.Text == item.browser.MyDocumentsName &&
                    item.Type == item.Browser.SystemFolderName)
                    return 2;
                else if (item.Text == item.browser.MyComputerName)
                    return 3;
                else if (item.Type == item.Browser.SystemFolderName)
                {
                    if (!item.IsBrowsable)
                        return 4;
                    else
                        return 5;
                }
                else if (item.IsFolder && !item.IsBrowsable)
                    return 6;
                else
                    return 7;
                 * */
                else if (item.Flags.isFolder && !item.Flags.isBrowsable)
                    return 6;
                else
                    return 7;

            }
            else
                return 8;
        }

        public int CompareTo(object obj)
        {
            ShellItem other = (ShellItem)obj;

            if (SortFlag != other.SortFlag)
                return ((SortFlag > other.SortFlag) ? 1 : -1);
            else if (m_flags.isDisk)
                return string.Compare(Path, other.Path);
            else
                return string.Compare(Text, other.Text);
        }

        #endregion

        #region IList Members

        internal bool Contains(ShellItem value)
        {
            return (SubFolders.Contains(value) || SubFiles.Contains(value));
        }

        internal bool Contains(string name)
        {
            return (SubFolders.Contains(name) || SubFiles.Contains(name));
        }

        internal bool Contains(IntPtr pidl)
        {
            return (SubFolders.Contains(pidl) || SubFiles.Contains(pidl));
        }

        internal int IndexOf(ShellItem value)
        {
            int index;
            index = SubFolders.IndexOf(value);

            if (index > -1)
                return index;

            index = SubFiles.IndexOf(value);

            if (index > -1)
                return SubFolders.Count + index;

            return -1;
        }

        internal int IndexOf(string name)
        {
            int index;
            index = SubFolders.IndexOf(name);

            if (index > -1)
                return index;

            index = SubFiles.IndexOf(name);

            if (index > -1)
                return SubFolders.Count + index;

            return -1;
        }

        internal int IndexOf(IntPtr pidl)
        {
            int index;
            index = SubFolders.IndexOf(pidl);

            if (index > -1)
                return index;

            index = SubFiles.IndexOf(pidl);

            if (index > -1)
                return SubFolders.Count + index;

            return -1;
        }

        internal ShellItem this[int index]
        {
            get
            {
                if (index >= 0 && index < SubFolders.Count)
                    return SubFolders[index];
                else if (index >= 0 && index - SubFolders.Count < SubFiles.Count)
                    return SubFiles[index - SubFolders.Count];
                else
                    throw new IndexOutOfRangeException();
            }
            set
            {
                if (index >= 0 && index < SubFolders.Count)
                    SubFolders[index] = value;
                else if (index >= 0 && index - SubFolders.Count < SubFiles.Count)
                    SubFiles[index - SubFolders.Count] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        internal ShellItem this[string name]
        {
            get
            {
                ShellItem temp = SubFolders[name];

                if (temp != null)
                    return temp;
                else
                    return SubFiles[name];
            }
            set
            {
                ShellItem temp = SubFolders[name];

                if (temp != null)
                    SubFolders[name] = value;
                else
                    SubFiles[name] = value;
            }
        }

        internal ShellItem this[IntPtr pidl]
        {
            get
            {
                ShellItem temp = SubFolders[pidl];

                if (temp != null)
                    return temp;
                else
                    return SubFiles[pidl];
            }
            set
            {
                ShellItem temp = SubFolders[pidl];

                if (temp != null)
                    SubFolders[pidl] = value;
                else
                    SubFiles[pidl] = value;
            }
        }

        internal int Count
        {
            get { return SubFolders.Count + SubFiles.Count; }
        }

        #endregion

        public override string ToString()
        {
            return Text;
        }

        #region Item Enumeration

        public class ShellItemEnumerator : IEnumerator
        {
            private ShellItem parent;
            private int index;

            public ShellItemEnumerator(ShellItem parent)
            {
                this.parent = parent;
                index = -1;
            }

            #region IEnumerator Members

            public object Current
            {
                get
                {
                    return parent[index];
                }
            }

            public bool MoveNext()
            {
                index++;
                return (index < parent.Count);
            }

            public void Reset()
            {
                index = -1;
            }

            #endregion
        }

        public class ShellItemCollection : IEnumerable
        {
            private ArrayList items;
            private ShellItem shellItem;

            public ShellItemCollection(ShellItem shellItem)
            {
                this.shellItem = shellItem;
                items = new ArrayList();
            }

            public ShellItem ShellItem { get { return shellItem; } }

            #region ArrayList Members

            public int Count
            {
                get { return items.Count; }
            }

            public void Sort()
            {
                items.Sort();
            }

            internal int Capacity
            {
                get { return items.Capacity; }
                set { items.Capacity = value; }
            }

            #endregion

            #region IList Members

            internal int Add(ShellItem value)
            {
                return items.Add(value);
            }

            internal void Clear()
            {
                items.Clear();
            }

            public bool Contains(ShellItem value)
            {
                return items.Contains(value);
            }

            public bool Contains(string name)
            {
                foreach (ShellItem item in this)
                {
                    if (string.Compare(item.Text, name, true) == 0)
                        return true;
                }

                return false;
            }

            public bool Contains(IntPtr pidl)
            {
                foreach (ShellItem item in this)
                {
                    if (item.PIDLRel.Equals(pidl))
                        return true;
                }

                return false;
            }

            public int IndexOf(ShellItem value)
            {
                return items.IndexOf(value);
            }

            public int IndexOf(string name)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (string.Compare(this[i].Text, name, true) == 0)
                        return i;
                }

                return -1;
            }

            public int IndexOf(IntPtr pidl)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (this[i].PIDLRel.Equals(pidl))
                        return i;
                }

                return -1;
            }

            internal void Insert(int index, ShellItem value)
            {
                items.Insert(index, value);
            }

            public bool IsFixedSize
            {
                get { return items.IsFixedSize; }
            }

            public bool IsReadOnly
            {
                get { return items.IsReadOnly; }
            }

            internal void Remove(ShellItem value)
            {
                items.Remove(value);
            }

            internal void Remove(string name)
            {
                int index;

                if ((index = IndexOf(name)) > -1)
                    RemoveAt(index);
            }

            internal void RemoveAt(int index)
            {
                items.RemoveAt(index);
            }

            public ShellItem this[int index]
            {
                get
                {
                    try
                    {
                        return (ShellItem)items[index];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return null;
                    }
                }
                set
                {
                    items[index] = value;
                }
            }

            public ShellItem this[string name]
            {
                get
                {
                    int index;
                    if ((index = IndexOf(name)) > -1)
                        return (ShellItem)items[index];
                    else
                        return null;
                }
                set
                {
                    int index;
                    if ((index = IndexOf(name)) > -1)
                        items[index] = value;
                }
            }

            public ShellItem this[IntPtr pidl]
            {
                get
                {
                    int index;
                    if ((index = IndexOf(pidl)) > -1)
                        return (ShellItem)items[index];
                    else
                        return null;
                }
                set
                {
                    int index;
                    if ((index = IndexOf(pidl)) > -1)
                        items[index] = value;
                }
            }

            #endregion

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return items.GetEnumerator();
            }

            #endregion
        }

        #endregion
    }
}
