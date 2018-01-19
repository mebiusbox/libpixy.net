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
using System.Windows.Forms;
using System.Drawing;

namespace libpixy.net.Controls.Standard
{
    public class Image9Panel : System.Windows.Forms.Panel
    {
        #region Fields
        #endregion

        #region Attributes
        private Image m_img9 = null;
        private int m_partWidth = 5;
        private int m_partHeight = 5;
        #endregion Attributes

        #region Properties

        /// <summary>
        /// ９個のパーツ（左上、上、右上、左、中央、右、左下、下、右下）を含む画像
        /// </summary>
        public Image Image9
        {
            get { return m_img9; }
            set { m_img9 = value; this.Invalidate(); }
        }

        /// <summary>
        /// パーツの幅
        /// </summary>
        public int PartWidth
        {
            get { return m_partWidth; }
            set { m_partWidth = value; }
        }

        /// <summary>
        /// パーツの高さ
        /// </summary>
        public int PartHeight
        {
            get { return m_partHeight; }
            set { m_partHeight = value; }
        }

        #endregion Properties

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Image9Panel()
            : base()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_img9 == null)
            {
                return;
            }

            Rectangle rc = this.ClientRectangle;

            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            int w = m_partWidth;
            int h = m_partHeight;

            // LeftTop
            e.Graphics.DrawImage(m_img9, new Rectangle(0,0,w,h), 0, 0, w, h, GraphicsUnit.Pixel);

            // Top
            x1 = w;
            y1 = 0;
            x2 = rc.Width - w;
            while (x1 < x2)
            {
                e.Graphics.DrawImage(m_img9, new Rectangle(x1, y1, Math.Min(w,x2-x1), h), w, 0, w, h, GraphicsUnit.Pixel);
                x1 += w;
            }

            // RightTop
            x1 = rc.Width - w;
            y1 = 0;
            e.Graphics.DrawImage(m_img9, new Rectangle(x1,y1,w,h), w*2, 0, w, h, GraphicsUnit.Pixel);

            // Left
            x1 = 0;
            y1 = w;
            y2 = rc.Height - w;
            while (y1 < y2)
            {
                e.Graphics.DrawImage(m_img9, new Rectangle(x1, y1, w, Math.Min(h, y2 - y1)), 0, h, w, h, GraphicsUnit.Pixel);
                y1 += h;
            }

            // Center
            x1 = w;
            y1 = h;
            x2 = rc.Width - w;
            y2 = rc.Height - h;
            while (y1 < y2)
            {
                x1 = w;
                while (x1 < x2)
                {
                    e.Graphics.DrawImage(m_img9, new Rectangle(x1, y1, Math.Min(w, x2 - x1), Math.Min(h, y2 - y1)), w, h, w, h, GraphicsUnit.Pixel);
                    x1 += w;
                }

                y1 += h;
            }

            // Right
            x1 = rc.Width - w;
            y1 = h;
            x2 = rc.Width;
            y2 = rc.Height - h;
            while (y1 < y2)
            {
                e.Graphics.DrawImage(m_img9, new Rectangle(x1, y1, w, Math.Min(h, y2 - y1)), w * 2, h, w, h, GraphicsUnit.Pixel);
                y1 += h;
            }

            // Left-Bottom
            x1 = 0;
            y1 = rc.Height - h;
            x2 = rc.Width;
            e.Graphics.DrawImage(m_img9, new Rectangle(x1, y1, w, h), 0, h * 2, w, h, GraphicsUnit.Pixel);

            // Bottom
            x1 = w;
            y1 = rc.Height - h;
            x2 = rc.Width - w;
            y2 = rc.Height;
            while (x1 < x2)
            {
                e.Graphics.DrawImage(m_img9, new Rectangle(x1, y1, Math.Min(w, x2 - x1), h), w, h * 2, w, h, GraphicsUnit.Pixel);
                x1 += w;
            }

            // Right-Bottom
            x1 = rc.Width - w;
            y1 = rc.Height - h;
            x2 = rc.Width;
            y2 = rc.Height;
            e.Graphics.DrawImage(m_img9, new Rectangle(x1, y1, w, h), w * 2, h * 2, w, h, GraphicsUnit.Pixel);

            base.OnPaint(e);
        }
    }
}
