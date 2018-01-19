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
using System.Drawing;

namespace libpixy.net.Controls.CurveEditor
{
    /// <summary>
    /// 
    /// </summary>
    class ColorSet
    {
        public Color Base;
        public Color Touch;
        public Color Select;
    }

    /// <summary>
    /// 
    /// </summary>
    class PenSet
    {
        public Pen Base = null;
        public Pen Touch = null;
        public Pen Select = null;

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="c"></param>
        public void Setup(ColorSet c)
        {
            if (Base == null)
            {
                Base = new Pen(c.Base);
            }

            if (Touch == null)
            {
                Touch = new Pen(c.Touch);
            }

            if (Select == null)
            {
                Select = new Pen(c.Select);
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            if (Base != null)
            {
                Base.Dispose();
                Base = null;
            }

            if (Touch != null)
            {
                Touch.Dispose();
                Touch = null;
            }

            if (Select != null)
            {
                Select.Dispose();
                Select = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class BrushSet
    {
        public Brush Base = null;
        public Brush Touch = null;
        public Brush Select = null;

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="c"></param>
        public void Setup(ColorSet c)
        {
            if (Base == null)
            {
                Base = new SolidBrush(c.Base);
            }

            if (Touch == null)
            {
                Touch = new SolidBrush(c.Touch);
            }

            if (Select == null)
            {
                Select = new SolidBrush(c.Select);
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            if (Base != null)
            {
                Base.Dispose();
                Base = null;
            }

            if (Touch != null)
            {
                Touch.Dispose();
                Touch = null;
            }

            if (Select != null)
            {
                Select.Dispose();
                Select = null;
            }
        }
    }
}
