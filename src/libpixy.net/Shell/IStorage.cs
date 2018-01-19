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
    [Guid("0000000b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStorage
    {
        [PreserveSig]
        Int32 CreateStream(
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsName,
            libpixy.net.Shell.API.STGM grfMode,
            int reserved1,
            int reserved2,
            out IntPtr ppstm);

        [PreserveSig]
        Int32 OpenStream(
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsName,
            IntPtr reserved1,
            libpixy.net.Shell.API.STGM grfMode,
            int reserved2,
            out IntPtr ppstm);

        [PreserveSig]
        Int32 CreateStorage(
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsName,
            libpixy.net.Shell.API.STGM grfMode,
            int reserved1,
            int reserved2,
            out IntPtr ppstg);

        [PreserveSig]
        Int32 OpenStorage(
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsName,
            IStorage pstgPriority,
            libpixy.net.Shell.API.STGM grfMode,
            IntPtr snbExclude,
            int reserved,
            out IntPtr ppstg);

        [PreserveSig]
        Int32 CopyTo(
            int ciidExclude,
            ref Guid rgiidExclude,
            IntPtr snbExclude,
            IStorage pstgDest);

        [PreserveSig]
        Int32 MoveElementTo(
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsName,
            IStorage pstgDest,
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsNewName,
            libpixy.net.Shell.API.STGMOVE grfFlags);

        [PreserveSig]
        Int32 Commit(
            libpixy.net.Shell.API.STGC grfCommitFlags);

        [PreserveSig]
        Int32 Revert();

        [PreserveSig]
        Int32 EnumElements(
            int reserved1,
            IntPtr reserved2,
            int reserved3,
            out IntPtr ppenum);

        [PreserveSig]
        Int32 DestroyElement(
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsName);

        [PreserveSig]
        Int32 RenameElement(
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsOldName,
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsNewName);

        [PreserveSig]
        Int32 SetElementTimes(
            [MarshalAs(UnmanagedType.LPWStr)]
            string pwcsName,
            libpixy.net.Shell.API.FILETIME pctime,
            libpixy.net.Shell.API.FILETIME patime,
            libpixy.net.Shell.API.FILETIME pmtime);

        [PreserveSig]
        Int32 SetClass(
            ref Guid clsid);

        [PreserveSig]
        Int32 SetStateBits(
            int grfStateBits,
            int grfMask);

        [PreserveSig]
        Int32 Stat(
            out libpixy.net.Shell.API.STATSTG pstatstg,
            libpixy.net.Shell.API.STATFLAG grfStatFlag);

    }
}
