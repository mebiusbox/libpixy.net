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
using System.Runtime.InteropServices;

namespace libpixy.net.Shell
{
    [ComImport]
    [Guid("000214E6-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellFolder
    {
        // Translate a file object's or folder's display name into an item identifier list.
        // Return value: error code, if any
        [PreserveSig]
        Int32 ParseDisplayName(
            IntPtr hwnd,
            IntPtr pdc,
            [MarshalAs(UnmanagedType.LPWStr)]
            string pszDisplayName,
            ref uint pchEaten,
            out IntPtr ppidl,
            ref libpixy.net.Shell.API.SFGAO pdwAttributes);

        // Allows a client to determine the contents of a folder by creating an item
        // identifier enumeration object and returning its IEnumIDList interface.
        // Return value: error code, if any
        [PreserveSig]
        Int32 EnumObjects(
            IntPtr hwnd,
            libpixy.net.Shell.API.SHCONTF grfFlags,
            out IntPtr enumIDList);

        // Retrieves an IShellFolder object for a subfolder.
        // Return value: error code, if any
        [PreserveSig]
        Int32 BindToObject(
            IntPtr pidl,
            IntPtr pdc,
            ref Guid riid,
            out IntPtr ppv);

        // Requests a pointer to an object's storage interface.
        // Return value: error code, if any
        [PreserveSig]
        Int32 BindToStorage(
            IntPtr pidl,
            IntPtr pdc,
            ref Guid riid,
            out IntPtr ppv);

        // Determines the retrive order of two file objects or folders, given their
        // item identifier lists. Return value: If this method is succeessful, the
        // CODE field of the HRESULT contains one of the following values (the code
        // can be retrived using the helper function GetHResultCode): Negative A
        // nagative return value indicates that the first item should precede
        // the second (pidl1 < pidl2).

        // Positive A positive return value indicates that the first item should
        // follow the second (pidl1 > pidl2). Zero A return value of zero
        // indicates that the two items are the same (pidl1 = pidl2)
        [PreserveSig]
        Int32 CompareIDs(
            IntPtr lParam,
            IntPtr pidl1,
            IntPtr pidl2);

        // Requests an object that can be used to obtain information from or interact
        // with a folder object.
        // Return value: error code, if any
        [PreserveSig]
        Int32 CreateViewObject(
            IntPtr hwndOwner,
            Guid riid,
            out IntPtr ppv);

        // Retrieves the attributes of one or more file objects or subfolders.
        // Return value: error code, if any
        [PreserveSig]
        Int32 GetAttributesOf(
            uint cidl,
            [MarshalAs(UnmanagedType.LPArray)]
            IntPtr[] apidl,
            ref libpixy.net.Shell.API.SFGAO rgfInOut);

        // Retrieves an OLE interface that can be used to carry out actions on the
        // specified file objects or folders.
        // Return value: error code, if any
        [PreserveSig]
        Int32 GetUIObjectOf(
            IntPtr hwndOwner,
            uint cidl,
            [MarshalAs(UnmanagedType.LPArray)]
            IntPtr[] apidl,
            ref Guid riid,
            IntPtr rgfReserved,
            out IntPtr ppv);

        // Retrieves the display name for the specified file object or subfolder.
        // Return value: error code, if any
        [PreserveSig]
        Int32 GetDisplayNameOf(
            IntPtr pidl,
            libpixy.net.Shell.API.SHGNO uFlags,
            IntPtr lpName);

        // Sets the display name of a file object or subfolder, changing the item
        // identifier in the process.
        // Return value: error code, if ay
        [PreserveSig]
        Int32 SetNameOf(
            IntPtr hwnd,
            IntPtr pidl,
            [MarshalAs(UnmanagedType.LPWStr)]
            string pszName,
            libpixy.net.Shell.API.SHGNO uFlags,
            out IntPtr ppidlOut);
    }
}
