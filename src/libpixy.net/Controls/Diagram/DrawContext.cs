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

namespace libpixy.net.Controls.Diagram
{
    /// <summary>
    /// 
    /// </summary>
    public class DrawContext : IDisposable
    {
        #region Fields
        public Font fontHead = null;
        public Font fontSubText = null;
        public Font fontPort = null;
        public Brush backBrush = null;
        public Brush foreBrush = null;
        public Pen forePen = null;
        public Pen penPort = null;
        public Pen linePen = null;
        public StringFormat sf = null;
        #endregion Fields

        /// <summary>
        /// 
        /// </summary>
        public DrawContext()
        {
            this.fontHead = new Font("Tahoma", 10, FontStyle.Bold);
            this.fontPort = new Font("Tahoma", 9);
            this.fontSubText = new Font("Tahoma", 8);
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fore"></param>
        /// <param name="back"></param>
        /// <param name="split"></param>
        public void Reset(Color fore, Color back, Color split)
        {
            Clear();

            backBrush = new SolidBrush(back);
            foreBrush = new SolidBrush(fore);
            forePen = new Pen(fore, 1);
            penPort = new Pen(Brushes.Black, 1);
            linePen = new Pen(split, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            if (this.backBrush != null)
            {
                this.backBrush.Dispose();
                this.backBrush = null;
            }

            if (this.foreBrush != null)
            {
                this.foreBrush.Dispose();
                this.foreBrush = null;
            }

            if (this.forePen != null)
            {
                this.forePen.Dispose();
                this.forePen = null;
            }

            if (this.penPort != null)
            {
                this.penPort.Dispose();
                this.penPort = null;
            }

            if (this.linePen != null)
            {
                this.linePen.Dispose();
                this.linePen = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.fontHead != null)
            {
                this.fontHead.Dispose();
                this.fontHead = null;
            }

            if (this.fontPort != null)
            {
                this.fontPort.Dispose();
                this.fontPort = null;
            }

            Clear();
        }
    }
}
