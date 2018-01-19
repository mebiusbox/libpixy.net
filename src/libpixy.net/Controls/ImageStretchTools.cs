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
using System.Linq;
using System.Text;
using System.Drawing;

namespace libpixy.net.Controls
{
    public class ImageStretchTools
    {
        #region Globals

        /// <summary>
        /// 
        /// </summary>
        public enum StretchModes
        {
            repeat,
            stretch
        };

        #endregion

        #region Fields
        private int m_partLeftWidth = 5;
        private int m_partRightWidth = 5;
        private int m_partUpperHeight = 5;
        private int m_partLowerHeight = 5;
        private StretchModes m_stretchMode = StretchModes.repeat;
        #endregion

        #region Properties

        /// <summary>
        /// 左パーツの幅
        /// </summary>
        [Browsable(true)]
        public int PartLeftWidth
        {
            get { return m_partLeftWidth; }
            set { m_partLeftWidth = value; }
        }

        /// <summary>
        /// 右パーツの幅
        /// </summary>
        [Browsable(true)]
        public int PartRightWidth
        {
            get { return m_partRightWidth; }
            set { m_partRightWidth = value; }
        }

        /// <summary>
        /// 上パーツの高さ
        /// </summary>
        [Browsable(true)]
        public int PartUpperHeight
        {
            get { return m_partUpperHeight; }
            set { m_partUpperHeight = value; }
        }

        /// <summary>
        /// 下パーツの高さ
        /// </summary>
        [Browsable(true)]
        public int PartLowerHeight
        {
            get { return m_partLowerHeight; }
            set { m_partLowerHeight = value; }
        }

        /// <summary>
        /// 画像を引き伸ばすときのモード
        /// </summary>
        [Browsable(true)]
        public StretchModes StretchMode
        {
            get { return m_stretchMode; }
            set { m_stretchMode = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="image"></param>
        public void Draw(System.Drawing.Graphics gfx, System.Drawing.Image image, System.Drawing.Rectangle rc)
        {
            if (image == null)
            {
                return;
            }

            int lw = m_partLeftWidth;
            int rw = m_partRightWidth;
            int cw = image.Width - (lw + rw);
            int uh = m_partUpperHeight;
            int lh = m_partLowerHeight;
            int ch = image.Height - (uh + lh);
            int lx = 0;
            int cx = m_partLeftWidth;
            int rx = image.Width - m_partRightWidth;
            int uy = 0;
            int cy = m_partUpperHeight;
            int ly = image.Height - m_partLowerHeight;

            // Left-Top
            Draw(gfx, image, 0, 0, lw, uh, lx, uy, lx + lw, uy + uh);
            // Right-Top
            Draw(gfx, image, rc.Width - rw, 0, rc.Width, uh, rx, uy, rx + rw, uy + uh);
            // Left-Bottom
            Draw(gfx, image, 0, rc.Height - lh, lw, rc.Height, lx, ly, lx + lw, ly + lh);
            // Right-Bottom
            Draw(gfx, image, rc.Width - rw, rc.Height - lh, rc.Width, rc.Height, rx, ly, rx + rw, ly + lh);
            // Top
            Draw(gfx, image, lw, 0, rc.Width - rw, uh, cx, uy, cx + cw, uy + uh);
            // Left
            Draw(gfx, image, 0, cy, lw, rc.Height - lh, lx, cy, lx + lw, cy + ch);
            // Center
            Draw(gfx, image, lw, cy, rc.Width - rw, rc.Height - lh, cx, cy, cx + cw, cy + ch);
            // Right
            Draw(gfx, image, rc.Width - rw, cy, rc.Width, rc.Height - lh, rx, cy, rx + rw, cy + ch);
            // Bottom
            Draw(gfx, image, lw, rc.Height - lh, rc.Width - rw, rc.Height, cx, ly, cx + cw, ly + lh);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="image"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="u1"></param>
        /// <param name="v1"></param>
        /// <param name="u2"></param>
        /// <param name="v2"></param>
        private void Draw(
            System.Drawing.Graphics gfx,
            System.Drawing.Image image,
            int x1, int y1, int x2, int y2,
            int u1, int v1, int u2, int v2)
        {
            if (m_stretchMode == StretchModes.repeat)
            {
                int uw = u2 - u1;
                int vh = v2 - v1;
                int a1 = x1;
                int a2 = x2;
                int b1 = y1;
                int b2 = y2;
                while (b1 < b2)
                {
                    a1 = x1;
                    while (a1 < a2)
                    {
                        Rectangle rcDst = new Rectangle(a1, b1, Math.Min(uw, a2 - a1), Math.Min(vh, b2 - b1));
                        Rectangle rcSrc = new Rectangle(u1, v1, Math.Min(uw, a2 - a1), Math.Min(vh, b2 - b1));
                        gfx.DrawImage(image, rcDst, rcSrc, GraphicsUnit.Pixel);
                        a1 += uw;
                    }

                    b1 += vh;
                }
            }
            else if (m_stretchMode == StretchModes.stretch)
            {
                gfx.DrawImage(
                    image,
                    new Rectangle(x1, y1, x2 - x1, y2 - y1),
                    new Rectangle(u1, v1, u2 - u1, v2 - v1),
                    GraphicsUnit.Pixel);
            }
        }

        #endregion
    }
}
