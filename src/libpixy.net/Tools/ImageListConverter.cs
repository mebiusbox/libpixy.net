﻿/*
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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace libpixy.net.Tools
{
    public static class ImageListConverter
    {
        public static ImageList FromHandle(IntPtr listHandle)
        {
            ImageList imgList = new ImageList();
            int listCount = ImageList_GetImageCount(listHandle);
            for (int i = 0; i < listCount; ++i)
            {
                IntPtr iconHandle = ImageList_GetIcon(listHandle, i, 0);
                if (iconHandle != IntPtr.Zero)
                {
                    Icon icon = Icon.FromHandle(iconHandle);
                    imgList.Images.Add(icon);
                    DestroyIcon(iconHandle);
                }
            }
            return imgList;
        }
        private const string COMCTLDLL = "comctl32.dll";

        [DllImport(COMCTLDLL, CharSet = CharSet.Unicode)]
        private static extern IntPtr ImageList_GetIcon(IntPtr hImageList, int index, uint flags);
        [DllImport(COMCTLDLL, CharSet = CharSet.Unicode)]
        private static extern int ImageList_GetImageCount(IntPtr hImageList);

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr hIcon);
    } 
}