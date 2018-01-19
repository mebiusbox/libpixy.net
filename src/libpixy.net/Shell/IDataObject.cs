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
    [Guid("0000010e-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDataObject
    {
        // Renders the data described in a FORMATETC structure
        // and transfer it through the STGMEDIUM structure
        [PreserveSig]
        Int32 GetData(
            ref libpixy.net.Shell.API.FORMATETC pformatetcIn,
            ref libpixy.net.Shell.API.STGMEDIUM pmedim);

        // Renders the data described in a FORMATETC structure
        // and transfer it through the STGMEDIUM structure allocated by the caller
        [PreserveSig]
        Int32 GetDataHere(
            ref libpixy.net.Shell.API.FORMATETC pformatetcIn,
            ref libpixy.net.Shell.API.STGMEDIUM pmedim);

        // Determines whether the data object is capable of 
        // rendering the data described in the FORMATETC structure
        [PreserveSig]
        Int32 QueryGetData(
            ref libpixy.net.Shell.API.FORMATETC pformatetc);

        // Provides a potentially different but logically equivalent FORMATETC structure
        [PreserveSig]
        Int32 GetCannonicalFormatEtc(
            ref libpixy.net.Shell.API.FORMATETC pformatetc,
            ref libpixy.net.Shell.API.FORMATETC pformatetcout);

        // Provides the source data object with data described by a
        // FORMATETC strcture and an STGMEDIM structure
        [PreserveSig]
        Int32 SetData(
            ref libpixy.net.Shell.API.FORMATETC pformatetcIn,
            ref libpixy.net.Shell.API.STGMEDIUM pmedium,
            bool frelease);

        // Creates and returns a pointer to an object to enumerate the
        // FORMATETC supported by the data object
        [PreserveSig]
        Int32 EnumFormatEtc(
            int dwDirection,
            ref libpixy.net.Shell.IEnumFORMATETC ppenumFormatEtc);

        // Creates a connection between a data object and an advise sink so
        // the advise sink can receive notifications of changes in the data object
        [PreserveSig]
        Int32 DAdvise(
            ref libpixy.net.Shell.API.FORMATETC pformatetc,
            ref libpixy.net.Shell.API.ADVF advf,
            ref libpixy.net.Shell.IAdviseSink pAdvSink,
            ref int pdwConnection);

        // Destroys a notificaiton previously set up with the DAdvise method
        [PreserveSig]
        Int32 DUnadvise(
            int dwConnection);

        // Creates and returns a pointer to an object to enumerate the current advisory connections
        [PreserveSig]
        Int32 EnumDAdvise(
            ref object ppenumAdvise);
    }
}
