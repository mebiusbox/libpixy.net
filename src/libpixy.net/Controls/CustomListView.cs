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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;

namespace libpixy.net.Controls
{
    public partial class CustomListView : ListView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("user32",
            SetLastError = true,
            CharSet = CharSet.Auto)]
        public static extern bool GetClientRect(
            IntPtr hwnd,
            ref libpixy.net.Shell.API.RECT rect);

        // Contains statistical data about an open storage, stream, or byte-array object
        [StructLayout(LayoutKind.Sequential)]
        public struct HDITEM
        {
            public HDI mask;
            public int cxy;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public HDF fmt;
            public IntPtr lParam;
            public int iImage;
            public int iOrder;
            public uint type;
            public IntPtr pvFilter;
        }

        [Flags]
        public enum HDI : uint
        {
            WIDTH = 0x0001,
            HEIGHT = 0x0001,
            TEXT = 0x0002,
            FORMAT = 0x0004,
            LPARAM = 0x0008,
            BITMAP = 0x0010,
            IMAGE = 0x0020,
            DI_SETITEM = 0x0040,
            ORDER = 0x0080,
            FILTER = 0x0100
        }

        [Flags]
        public enum HDF : uint
        {
            LEFT = 0x0000,
            RIGHT = 0x0001,
            CENTER = 0x0002,
            JUSTIFYMASK = 0x0003,
            RTLREADING = 0x0004,
            OWNERDRAW = 0x8000,
            STRING = 0x4000,
            BITMAP = 0x2000,
            BITMAP_ON_RIGHT = 0x1000,
            IMAGE = 0x0800,
            SORTUP = 0x0400,
            SORTDOWN = 0x0200
        }

        [Flags]
        public enum HDM : uint
        {
            FIRST = 0x1200,
            GETITEMA = FIRST + 3,
            SETITEMA = FIRST + 4,
            GETITEMW = FIRST + 11,
            SETITEMW = FIRST + 12
        }

        /// <summary>
        /// 
        /// </summary>
        public class ListColumnHeaderSortByDisplayIndex : IComparer<ColumnHeader>
        {
            public ListColumnHeaderSortByDisplayIndex()
            {
            }

            public int Compare(ColumnHeader x, ColumnHeader y)
            {
                if (x.DisplayIndex < y.DisplayIndex)
                {
                    return -1;
                }
                else if (x.DisplayIndex > y.DisplayIndex)
                {
                    return +1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ColumnHeaderClickEventArgs
        {
            public ColumnHeader Item = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public delegate void ColumnHeaderClickEvent(ColumnHeaderClickEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        public ColumnHeaderClickEvent ColumnHeaderClick;

        /// <summary>
        /// 
        /// </summary>
        private int columnHeight = 0;

        /// <summary>
        /// 
        /// </summary>
        public CustomListView()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            DrawItem += new DrawListViewItemEventHandler(BrowserListView_DrawItem);
            DrawSubItem += new DrawListViewSubItemEventHandler(BrowserListView_DrawSubItem);
            DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(BrowserListView_DrawColumnHeader);
        }

        #region Owner Draw

        void BrowserListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        void BrowserListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        void BrowserListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            columnHeight = e.Bounds.Height;
        }

        #endregion

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: カスタム ペイント コードをここに追加します

            // 基本クラス OnPaint を呼び出しています
            base.OnPaint(pe);
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)libpixy.net.Shell.API.WM.CONTEXTMENU)
            {
                if (this.ColumnHeaderClick != null)
                {
#if false
                    const int LVM_GETHEADER = 0x101F;
                    IntPtr hdrHandle = libpixy.net.Shell.API.SendMessage(this.Handle, (libpixy.net.Shell.API.WM)LVM_GETHEADER, 0, IntPtr.Zero);
                    libpixy.net.Shell.API.RECT rc = new libpixy.net.Shell.API.RECT();
                    GetClientRect(hdrHandle, ref rc);
#endif

                    int x = (int)libpixy.net.Shell.ShellHelper.LoWord(m.LParam);
                    int y = (int)libpixy.net.Shell.ShellHelper.HiWord(m.LParam);
                    Point loc = this.PointToClient(new Point(x, y));
                    List<ColumnHeader> cols = new List<ColumnHeader>();
                    foreach (ColumnHeader hdr in this.Columns)
                    {
                        cols.Add(hdr);
                    }
                    cols.Sort(new ListColumnHeaderSortByDisplayIndex());
                    bool raised = false;
                    int left = 0;
                    foreach (ColumnHeader hdr in cols)
                    {
                        if (left <= loc.X && loc.X <= left + hdr.Width &&
                            loc.Y < this.columnHeight)
                        {
                            ColumnHeaderClickEventArgs e = new ColumnHeaderClickEventArgs();
                            e.Item = hdr;
                            this.ColumnHeaderClick(e);
                            raised = true;
                            break;
                        }

                        left += hdr.Width;
                    }

                    if (!raised)
                    {
                        this.ColumnHeaderClick(new ColumnHeaderClickEventArgs());
                    }
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hdf"></param>
        public void SetColumnHeaderSort(int index, HDF hdf)
        {
            const int LVM_GETHEADER = 0x101F;
            IntPtr hdrHandle = libpixy.net.Shell.API.SendMessage(this.Handle, (libpixy.net.Shell.API.WM)LVM_GETHEADER, 0, IntPtr.Zero);
            HDITEM item = new HDITEM();
            item.mask = HDI.FORMAT;

            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(item));
            Marshal.StructureToPtr(item, ptr, false);
            libpixy.net.Shell.API.SendMessage(hdrHandle, (libpixy.net.Shell.API.WM)HDM.GETITEMW, index, ptr);
            item = (HDITEM)Marshal.PtrToStructure(ptr, typeof(HDITEM));

            item.fmt &= ~(HDF.SORTDOWN | HDF.SORTUP);
            item.fmt |= hdf;

            Marshal.StructureToPtr(item, ptr, false);
            libpixy.net.Shell.API.SendMessage(hdrHandle, (libpixy.net.Shell.API.WM)HDM.SETITEMW, index, ptr);
            Marshal.FreeCoTaskMem(ptr);
        }
    }
}
