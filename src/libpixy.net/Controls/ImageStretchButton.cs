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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libpixy.net.Controls
{
    public partial class ImageStretchButton : Button
    {
        #region Private Member Variables
        
        private Image m_overBackgroundImage = null;
        private Image m_pressedBackgroundImage = null;
        private Image m_switchBackgroundImage = null;
        private Image m_switchOverBackgroundImage = null;
        private Image m_switchImage = null;
        private bool m_switchMode = false;
        private bool m_switchState = false;
        private bool m_pressed = false;
        private bool m_bInRegion = false;
        private bool m_colorFillImage = false;
        private Padding m_colorFillMargin = new Padding(3, 3, 3, 3);
        private bool m_colorFillRounded = false;
        private bool m_colorFillAlpha = false;
        private ImageStretchTools m_imageStretch = new ImageStretchTools();

        #endregion // Private Member Variables

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        public Image OverBackgroundImage
        {
            get { return this.m_overBackgroundImage; }
            set { this.m_overBackgroundImage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        public Image PressedBackgroundImage
        {
            get { return this.m_pressedBackgroundImage; }
            set { this.m_pressedBackgroundImage = value; }
        }

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

        /// <summary>
        /// スイッチモード
        /// </summary>
        [Browsable(true)]
        public bool SwitchMode
        {
            get { return m_switchMode; }
            set { m_switchMode = value; }
        }

        /// <summary>
        /// スイッチ状態
        /// </summary>
        [Browsable(true)]
        public bool SwitchState
        {
            get { return m_switchState; }
            set { m_switchState = value; }
        }

        /// <summary>
        /// スイッチがＯＮになっているときに表示するイメージ
        /// </summary>
        [Browsable(true)]
        public Image SwitchImage
        {
            get { return m_switchImage; }
            set { m_switchImage = value; }
        }

        /// <summary>
        /// スイッチがＯＮになっているときに表示するイメージ
        /// </summary>
        [Browsable(true)]
        public Image SwitchBackgroundImage
        {
            get { return m_switchBackgroundImage; }
            set { m_switchBackgroundImage = value; }
        }

        /// <summary>
        /// スイッチがＯＮになっているときにマウスオーバーで表示するイメージ
        /// </summary>
        [Browsable(true)]
        public Image SwitchOverBackgroundImage
        {
            get { return m_switchOverBackgroundImage; }
            set { m_switchOverBackgroundImage = value; }
        }

        /// <summary>
        /// 内部の前景色で描画するかどうか.
        /// </summary>
        [Browsable(true)]
        public bool ColorFillImage
        {
            get { return m_colorFillImage; }
            set { m_colorFillImage = value; }
        }

        /// <summary>
        /// 内部の前景色のアルファ成分を描画するかどうか
        /// </summary>
        [Browsable(true)]
        public bool ColorFillAlpha
        {
            get { return m_colorFillAlpha; }
            set { m_colorFillAlpha = value; }
        }

        /// <summary>
        /// 内部を前景色で描画するときのマージン.
        /// </summary>
        [Browsable(true)]
        public Padding ColorFillMargin
        {
            get { return m_colorFillMargin; }
            set { m_colorFillMargin = value; }
        }

        /// <summary>
        /// 内部を前景色で描画するときに角を丸くするかどうか.
        /// </summary>
        [Browsable(true)]
        public bool ColorFillRounded
        {
            get { return m_colorFillRounded; }
            set { m_colorFillRounded = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public ImageStretchButton()
        {
            InitializeComponent();
        }
        
        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (m_pressed == false && m_switchMode)
            {
                m_switchState = !m_switchState;
            }

            this.m_pressed = true;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.m_pressed = false;
            this.Invalidate();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            this.m_bInRegion = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            this.m_bInRegion = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle bounds = this.ClientRectangle;
            e.Graphics.Clear(this.BackColor);

            bool drawButton = false;
            if (m_switchMode)
            {
                if (m_switchState)
                {
                    if (m_bInRegion)
                    {
                        if (m_switchOverBackgroundImage != null)
                        {
                            m_imageStretch.Draw(e.Graphics, m_switchOverBackgroundImage, bounds);
                            drawButton = true;
                        }
                    }
                    else if (m_switchBackgroundImage != null) 
                    {
                        m_imageStretch.Draw(e.Graphics, m_switchBackgroundImage, bounds);
                        drawButton = true;
                    }
                }
            }

            if (drawButton == false)
            {
                if (this.m_pressed && this.m_pressedBackgroundImage != null)
                {
                    m_imageStretch.Draw(e.Graphics, this.m_pressedBackgroundImage, bounds);
                }
                else if (this.m_bInRegion && this.m_overBackgroundImage != null)
                {
                    m_imageStretch.Draw(e.Graphics, this.m_overBackgroundImage, bounds);
                }
                else if (this.BackgroundImage != null)
                {
                    m_imageStretch.Draw(e.Graphics, this.BackgroundImage, bounds);
                }
            }

            if (this.Image != null)
            {
                Image buttonImage = this.Image;
                if (m_switchMode && m_switchState && m_switchImage != null)
                {
                    buttonImage = m_switchImage;
                }

                switch (this.ImageAlign)
                {
                    case ContentAlignment.TopLeft:
                        e.Graphics.DrawImage(buttonImage, new Point(0, 0));
                        break;

                    case ContentAlignment.TopCenter:
                        e.Graphics.DrawImage(buttonImage, new Point(this.ClientSize.Width/2 - buttonImage.Width/2, 0));
                        break;

                    case ContentAlignment.TopRight:
                        e.Graphics.DrawImage(buttonImage, new Point(this.ClientSize.Width - buttonImage.Width, 0));
                        break;

                    case ContentAlignment.MiddleLeft:
                        e.Graphics.DrawImage(buttonImage, new Point(0, this.ClientSize.Height/2 - buttonImage.Height/2));
                        break;

                    case ContentAlignment.MiddleCenter:
                        e.Graphics.DrawImage(buttonImage, new Point(this.ClientSize.Width / 2 - buttonImage.Width / 2, this.ClientSize.Height / 2 - buttonImage.Height / 2));
                        break;

                    case ContentAlignment.MiddleRight:
                        e.Graphics.DrawImage(buttonImage, new Point(this.ClientSize.Width - buttonImage.Width, this.ClientSize.Height / 2 - buttonImage.Height / 2));
                        break;

                    case ContentAlignment.BottomLeft:
                        e.Graphics.DrawImage(buttonImage, new Point(0, this.ClientSize.Height - buttonImage.Height));
                        break;

                    case ContentAlignment.BottomCenter:
                        e.Graphics.DrawImage(buttonImage, new Point(this.ClientSize.Width / 2 - buttonImage.Width / 2, this.ClientSize.Height - buttonImage.Height));
                        break;

                    case ContentAlignment.BottomRight:
                        e.Graphics.DrawImage(buttonImage, new Point(this.ClientSize.Width - buttonImage.Width, this.ClientSize.Height - buttonImage.Height));
                        break;
                }
            }
            else if (m_colorFillImage)
            {
                Rectangle area = new Rectangle(
                    m_colorFillMargin.Left,
                    m_colorFillMargin.Top,
                    this.ClientSize.Width - m_colorFillMargin.Horizontal - 1,
                    this.ClientSize.Height - m_colorFillMargin.Vertical - 1);

                if (m_colorFillRounded)
                {
                    using (Brush brush = new SolidBrush(this.ForeColor))
                    {
                        libpixy.net.Controls.Diagram.Draw.FillRoundedRectangle(e.Graphics, area, 6, brush);
                    }

                    using (Pen pen = new Pen(Color.Black))
                    {
                        libpixy.net.Controls.Diagram.Draw.DrawRoundedRectangle(e.Graphics, area, 6, pen);
                    }
                }
                else
                {
                    if (m_colorFillAlpha)
                    {
                        byte r = this.ForeColor.R;
                        byte g = this.ForeColor.G;
                        byte b = this.ForeColor.B;
                        byte a = this.ForeColor.A;
                        using (Brush brush = new SolidBrush(Color.FromArgb(r,g,b)))
                        {
                            e.Graphics.FillRectangle(brush, area);
                        }
                        using (Brush brush = new SolidBrush(Color.FromArgb(a,a,a)))
                        {
                            Point[] points = {
                            new Point(area.Right, area.Top),
                            new Point(area.Left, area.Bottom),
                            new Point(area.Right, area.Bottom)};
                            e.Graphics.FillPolygon(brush, points, System.Drawing.Drawing2D.FillMode.Winding);
                        }
                    }
                    else
                    {
                        using (Brush brush = new SolidBrush(this.ForeColor))
                        {
                            e.Graphics.FillRectangle(brush, area);
                        }
                    }

                    using (Pen pen = new Pen(Color.Black))
                    {
                        e.Graphics.DrawRectangle(pen, area);
                    }
                }
            }

            // Draw the text if there is any.
            if (this.Text.Length > 0)
            {
                SizeF size = e.Graphics.MeasureString(this.Text, this.Font);

                // Center the text inside the client area of the PictureButton.
                e.Graphics.DrawString(this.Text,
                    this.Font,
                    new SolidBrush(this.ForeColor),
                    (this.ClientSize.Width - size.Width) / 2,
                    (this.ClientSize.Height - size.Height) / 2);
            }

            // Draw a border around the outside of the 
            // control to look like Pocket PC buttons.
            // e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, 
            //	this.ClientSize.Width - 1, this.ClientSize.Height - 1);

            // base.OnPaint(e);
        }

        #endregion
    }
}
