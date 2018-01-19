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
using System.Runtime.InteropServices;

namespace libpixy.net.Shell
{
    public static class ShellHelper
    {
        #region Low/High Word

        /// <summary>
        /// Retrieves the High Word of a WParam of a WindowMessage
        /// </summary>
        /// <param name="ptr">The pointer to the WParam</param>
        /// <returns>The unsigned integer for the High Word</returns>
        public static uint HiWord(IntPtr ptr)
        {
            if (((uint)ptr & 0x80000000) == 0x80000000)
                return ((uint)ptr >> 16);
            else
                return ((uint)ptr >> 16) & 0xffff;
        }

        /// <summary>
        /// Retrieves the Low Word of a WParam of a WindowMessage
        /// </summary>
        /// <param name="ptr">The pointer to the WParam</param>
        /// <returns>The unsigned integer for the Low Word</returns>
        public static uint LoWord(IntPtr ptr)
        {
            return (uint)ptr & 0xffff;
        }

        #endregion

        #region IStream / IStorage

        public static bool GetIStream(
            libpixy.net.Shell.ShellItem item,
            out IntPtr streamPtr,
            out IStream stream)
        {
            if (item.ParentItem.ShellFolder.BindToStorage(
                item.PIDLRel.Ptr,
                IntPtr.Zero,
                ref libpixy.net.Shell.API.IID_IStream,
                out streamPtr) == libpixy.net.Shell.API.S_OK)
            {
                stream = (IStream)Marshal.GetTypedObjectForIUnknown(streamPtr, typeof(IStream));
                return true;
            }
            else
            {
                stream = null;
                streamPtr = IntPtr.Zero;
                return false;
            }
        }

        public static bool GetIStorage(
            libpixy.net.Shell.ShellItem item,
            out IntPtr storagePtr,
            out IStorage storage)
        {
            if (item.ParentItem.ShellFolder.BindToStorage(
                item.PIDLRel.Ptr,
                IntPtr.Zero,
                ref libpixy.net.Shell.API.IID_IStorage,
                out storagePtr) == libpixy.net.Shell.API.S_OK)
            {
                storage = (IStorage)Marshal.GetTypedObjectForIUnknown(storagePtr, typeof(IStorage));
                return true;
            }
            else
            {
                storage = null;
                storagePtr = IntPtr.Zero;
                return false;
            }
        }

        #endregion

        #region Drag/Drop

        /// <summary>
        /// This method will use the GetUIObjectOf method of IShellFolder to obtain the IDataObject of a ShellItem.
        /// </summary>
        /// <param name="items">The item for which to obtain the IDataObject</param>
        /// <returns>the IDataObject the ShellItem</returns>
        public static IntPtr GetIDataObject(
            libpixy.net.Shell.ShellItem[] items)
        {
            libpixy.net.Shell.ShellItem parent = items[0].ParentItem != null ? items[0].ParentItem : items[0];

            IntPtr[] pidls = new IntPtr[items.Length];
            for (int i = 0; i < items.Length; ++i)
            {
                pidls[i] = items[i].PIDLRel.Ptr;
            }

            IntPtr dataObjectPtr;
            if (parent.ShellFolder.GetUIObjectOf(
                IntPtr.Zero,
                (uint)pidls.Length,
                pidls,
                ref libpixy.net.Shell.API.IID_IDataObject,
                IntPtr.Zero,
                out dataObjectPtr) == libpixy.net.Shell.API.S_OK)
            {
                return dataObjectPtr;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// This method will use the GetUIObjectOf method of IShellFolder to obtain the IDropTarget of a ShellItem
        /// </summary>
        /// <param name="item">The item for which to obtain the IDropTarget</param>
        /// <param name="dropTargetPtr">A pointer to the returned IDropTarget</param>
        /// <param name="dropTarget">The IDropTarget from the ShelItem</param>
        /// <returns></returns>
        public static bool GetIDropTarget(
            libpixy.net.Shell.ShellItem item,
            out IntPtr dropTargetPtr,
            out libpixy.net.Shell.IDropTarget dropTarget)
        {
            libpixy.net.Shell.ShellItem parent = item.ParentItem != null ? item.ParentItem : item;

            if (parent.ShellFolder.GetUIObjectOf(
                IntPtr.Zero,
                1,
                new IntPtr[] { item.PIDLRel.Ptr },
                ref libpixy.net.Shell.API.IID_IDropTarget,
                IntPtr.Zero,
                out dropTargetPtr) == libpixy.net.Shell.API.S_OK)
            {
                dropTarget = (libpixy.net.Shell.IDropTarget)Marshal.GetTypedObjectForIUnknown(dropTargetPtr, typeof(libpixy.net.Shell.IDropTarget));
                return true;
            }
            else
            {
                dropTarget = null;
                dropTargetPtr = IntPtr.Zero;
                return false;
            }
        }

        public static bool GetIDropTargetHelper(
            out IntPtr helperPtr,
            out IDropTargetHelper dropHelper)
        {
            if (libpixy.net.Shell.API.CoCreateInstance(
                ref libpixy.net.Shell.API.CLSID_DragDropHelper,
                IntPtr.Zero,
                libpixy.net.Shell.API.CLSCTX.INPROC_SERVER,
                ref libpixy.net.Shell.API.IID_IDropTargetHelper,
                out helperPtr) == libpixy.net.Shell.API.S_OK)
            {
                dropHelper = (IDropTargetHelper)Marshal.GetTypedObjectForIUnknown(helperPtr, typeof(IDropTargetHelper));
                return true;
            }
            else
            {
                dropHelper = null;
                helperPtr = IntPtr.Zero;
                return false;
            }
        }

        public static DragDropEffects CanDropClipboard(
            libpixy.net.Shell.ShellItem item)
        {
            IntPtr dataObject;
            libpixy.net.Shell.API.OleGetClipboard(out dataObject);

            IntPtr targetPtr;
            libpixy.net.Shell.IDropTarget target;

            DragDropEffects retVal = DragDropEffects.None;
            if (GetIDropTarget(item, out targetPtr, out target))
            {
                #region Check Copy
                DragDropEffects effects = DragDropEffects.Copy;
                if (target.DragEnter(
                    dataObject,
                    libpixy.net.Shell.API.MK.CONTROL,
                    new libpixy.net.Shell.API.POINT(0, 0),
                    ref effects) == libpixy.net.Shell.API.S_OK)
                {
                    if (effects == DragDropEffects.Copy)
                        retVal |= DragDropEffects.Copy;

                    target.DragLeave();
                }
                #endregion

                #region Check Move
                effects = DragDropEffects.Move;
                if (target.DragEnter(
                    dataObject,
                    libpixy.net.Shell.API.MK.SHIFT,
                    new libpixy.net.Shell.API.POINT(0, 0),
                    ref effects) == libpixy.net.Shell.API.S_OK)
                {
                    if (effects == DragDropEffects.Move)
                        retVal |= DragDropEffects.Move;

                    target.DragLeave();
                }
                #endregion

                #region Check Link
                effects = DragDropEffects.Link;
                if (target.DragEnter(
                    dataObject,
                    libpixy.net.Shell.API.MK.ALT,
                    new libpixy.net.Shell.API.POINT(0, 0),
                    ref effects) == libpixy.net.Shell.API.S_OK)
                {
                    if (effects == DragDropEffects.Link)
                        retVal |= DragDropEffects.Link;

                    target.DragLeave();
                }
                #endregion

                Marshal.ReleaseComObject(target);
                Marshal.Release(targetPtr);
            }

            return retVal;
        }

        #endregion

        #region QueryInfo

        public static bool GetIQueryInfo(
            libpixy.net.Shell.ShellItem item,
            out IntPtr iQueryInfoPtr,
            out IQueryInfo iQueryInfo)
        {
            libpixy.net.Shell.ShellItem parent = item.ParentItem != null ? item.ParentItem : item;

            if (parent.ShellFolder.GetUIObjectOf(
                IntPtr.Zero,
                1,
                new IntPtr[] { item.PIDLRel.Ptr },
                ref libpixy.net.Shell.API.IID_IQueryInfo,
                IntPtr.Zero,
                out iQueryInfoPtr) == libpixy.net.Shell.API.S_OK)
            {
                iQueryInfo = (IQueryInfo)Marshal.GetTypedObjectForIUnknown(iQueryInfoPtr, typeof(IQueryInfo));
                return true;
            }
            else
            {
                iQueryInfo = null;
                iQueryInfoPtr = IntPtr.Zero;
                return false;
            }
        }

        #endregion

        #region ParseName

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ShellItem ParseName(string path)
        {
            libpixy.net.Shell.ShellItem desktop = libpixy.net.Shell.ShellItem.DesktopItem;
            uint eaten = 0;
            IntPtr pidl = IntPtr.Zero;
            libpixy.net.Shell.API.SFGAO attrib = 0;
            if (desktop.ShellFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, path, ref eaten, out pidl, ref attrib) == libpixy.net.Shell.API.S_OK)
            {
                return new ShellItem(desktop, pidl);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ShellItem ParseName(ShellItem parent, string path)
        {
            uint eaten = 0;
            IntPtr pidl = IntPtr.Zero;
            libpixy.net.Shell.API.SFGAO attrib = 0;
            if (parent.ShellFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, path, ref eaten, out pidl, ref attrib) == libpixy.net.Shell.API.S_OK)
            {
                return new ShellItem(parent, pidl);
            }

            return null;
        }

        #endregion ParseName
    }
}
