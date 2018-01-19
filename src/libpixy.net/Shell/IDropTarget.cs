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
using System.Windows.Forms;

namespace libpixy.net.Shell
{
    [ComImport]
    [Guid("00000122-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDropTarget
    {
        // Determines whether a drop can be accepted and its effect if it is accepted
        [PreserveSig]
        Int32 DragEnter(
            IntPtr pDataObj,
            libpixy.net.Shell.API.MK grfKeyState,
            libpixy.net.Shell.API.POINT pt,
            ref DragDropEffects pdwEffect);

        // Provides target feedback to the user through the DoDragDrop function
        [PreserveSig]
        Int32 DragOver(
            libpixy.net.Shell.API.MK grfKeyState,
            libpixy.net.Shell.API.POINT pt,
            ref DragDropEffects pdwEffect);

        // Causes the drop target to suspend its feedback actions
        [PreserveSig]
        Int32 DragLeave();

        // Drops the data into the target window
        [PreserveSig]
        Int32 DragDrop(
            IntPtr pDataObj,
            libpixy.net.Shell.API.MK grfKeyState,
            libpixy.net.Shell.API.POINT pt,
            ref DragDropEffects pdwEffect);
    }
}
