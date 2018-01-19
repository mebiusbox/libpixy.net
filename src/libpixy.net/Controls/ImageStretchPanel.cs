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
using System.Windows.Forms;
using System.Drawing;

namespace libpixy.net.Controls.Standard
{
    public class ImageStretchPanel : System.Windows.Forms.Panel
    {
        #region Fields
        #endregion

        #region Attributes
        private ImageStretchTools m_imageStretch = new ImageStretchTools();
        #endregion Attributes

        #region Properties

        /// <summary>
        /// 左パーツの幅
        /// </summary>
        [Browsable(true)]
        public int PartLeftWidth
        {
            get { return m_imageStretch.PartLeftWidth; }
            set { m_imageStretch.PartLeftWidth = value; }
        }

        /// <summary>
        /// 右パーツの幅
        /// </summary>
        [Browsable(true)]
        public int PartRightWidth
        {
            get { return m_imageStretch.PartRightWidth; }
            set { m_imageStretch.PartRightWidth = value; }
        }

        /// <summary>
        /// 上パーツの高さ
        /// </summary>
        [Browsable(true)]
        public int PartUpperHeight
        {
            get { return m_imageStretch.PartUpperHeight; }
            set { m_imageStretch.PartUpperHeight = value; }
        }

        /// <summary>
        /// 下パーツの高さ
        /// </summary>
        [Browsable(true)]
        public int PartLowerHeight
        {
            get { return m_imageStretch.PartLowerHeight; }
            set { m_imageStretch.PartLowerHeight = value; }
        }

        /// <summary>
        /// 画像を引き伸ばすときのモード
        /// </summary>
        [Browsable(true)]
        public ImageStretchTools.StretchModes StretchMode
        {
            get { return m_imageStretch.StretchMode; }
            set { m_imageStretch.StretchMode = value; }
        }

        #endregion Properties

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageStretchPanel()
            : base()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.BackgroundImage == null)
            {
                return;
            }

            m_imageStretch.Draw(e.Graphics, this.BackgroundImage, this.ClientRectangle);

            //base.OnPaint(e);
        }
    }
}
