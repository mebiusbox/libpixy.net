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
using System.ComponentModel;

namespace libpixy.net.Controls.EdgePanel
{
    /// <summary>
    /// 
    /// </summary>
    public class EdgePanel : Panel
    {
        #region Private Member Variables

        private Font m_boldFont;
        private Font m_underlineFont;
        private Brush m_buttonFrameBrush = null;
        private Brush m_buttonLineBrush = null;
        private Brush m_buttonBackBrush = null;
        private Color m_buttonFrameColor = Color.FromArgb(192, 192,192);
        private Color m_buttonLineColor = Color.FromArgb(192, 192, 192);
        private Color m_buttonBackColor = Color.FromArgb(72,72,72);
        private Color m_frameColor = Color.FromArgb(8,8,8);
        private Rectangle m_buttonRectangle;
        private Container m_components;
        private int m_expandHeight;
        private string m_name;
        private Rectangle m_nameBounds;
        private Font m_nameFont;
        private bool m_open;
        private EdgePanelContainer m_panelContainer;
        private bool m_sizeWidthToContainer;
        private bool m_lockUpdate;

        #endregion // Private Member Variables

        #region Constructor

        /// <summary>
        /// ctor
        /// </summary>
        public EdgePanel()
        {
            this.m_components = null;
            this.m_name = "Panel";
            this.m_nameBounds = new Rectangle(-1, -1, -1, -1);
            this.m_buttonRectangle = new Rectangle(4, 4, 8, 8);
            this.m_open = true;
            this.m_buttonFrameBrush = new SolidBrush(m_buttonFrameColor);
            this.m_buttonLineBrush = new SolidBrush(m_buttonLineColor);
            this.m_buttonBackBrush = new SolidBrush(m_buttonBackColor);
            this.m_expandHeight = -1;
            this.m_panelContainer = null;
            this.m_sizeWidthToContainer = false;
            this.m_lockUpdate = false;
            this.InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint|ControlStyles.UserPaint|ControlStyles.OptimizedDoubleBuffer,true);
            this.m_boldFont = new Font("Tahoma", 8f, FontStyle.Bold);
            this.m_underlineFont = new Font("Tahoma", 8f, FontStyle.Underline|FontStyle.Bold);
            this.m_nameFont = this.m_boldFont;
            this.BackColor = Color.FromArgb(72, 72, 72);
            this.DoubleBuffered = true;
        }

        #endregion // Constructor

        #region Public Properties

        /// <summary>
        /// アップデート停止
        /// </summary>
        public bool LockUpdate
        {
            get { return m_lockUpdate; }
            set { m_lockUpdate = value; }
        }

        [Browsable(true)]
        [Description("パネルコンテナ")]
        public EdgePanelContainer PanelContainer
        {
            get
            {
                return m_panelContainer;
            }

            set
            {
                m_panelContainer = value;
                if ((value != null) && m_sizeWidthToContainer)
                {
                    base.SetBounds(0, 0, m_panelContainer.Width - 10, 0, BoundsSpecified.Width);
                }

                base.Invalidate();
            }

        }

        [Browsable(true)]
        [Description("パネル名")]
        public string PanelName
        {
            get
            {
                return m_name;
            }

            set
            {
                m_name = value;
                m_nameBounds.X = -1;
                base.Invalidate();
            }
        }

        [Browsable(true)]
        [Description("パネルのサイズをコンテナのサイズに調整するかどうか")]
        public bool SizeWidthToContainer
        {
            get
            {
                return m_sizeWidthToContainer;
            }

            set
            {
                m_sizeWidthToContainer = value;
            }
        }

        [Browsable(true)]
        public Color ButtonFrameColor
        {
            get { return m_buttonFrameColor; }
            set
            {
                m_buttonFrameColor = value;
                if (m_buttonFrameBrush != null)
                {
                    m_buttonFrameBrush.Dispose();
                    m_buttonFrameBrush = new SolidBrush(m_buttonFrameColor);
                }
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public Color ButtonLineColor
        {
            get { return m_buttonLineColor; }
            set
            {
                m_buttonLineColor = value;
                if (m_buttonLineBrush != null)
                {
                    m_buttonLineBrush.Dispose();
                    m_buttonLineBrush = new SolidBrush(m_buttonLineColor);
                }
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public Color ButtonBackColor
        {
            get { return m_buttonBackColor; }
            set
            {
                m_buttonBackColor = value;
                if (m_buttonBackBrush != null)
                {
                    m_buttonBackBrush.Dispose();
                    m_buttonBackBrush = new SolidBrush(m_buttonBackColor);
                }
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public Color FrameColor
        {
            get { return m_frameColor; }
            set
            {
                m_frameColor = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public Font NameFont
        {
            get { return m_boldFont; }
            set
            {
                m_boldFont = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public Font NameHoverFont
        {
            get { return m_underlineFont; }
            set
            {
                m_underlineFont = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public bool IsExpanded
        {
            get { return m_open; }
        }

        #endregion // Public Properties

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void UpdatePanel()
        {
            if (m_lockUpdate)
            {
                return;
            }

            if (m_panelContainer != null)
            {
                m_panelContainer.RepositionControls();
            }

            base.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ChangeOpenState()
        {
            if (m_open)
            {
                Collapse();
            }
            else
            {
                Expand();
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && m_components != null)
            {
                m_components.Dispose();
            }

            m_boldFont.Dispose();
            m_underlineFont.Dispose();
            m_nameFont = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void PaintButton(PaintEventArgs e)
        {
            Pen pen = new Pen(m_buttonFrameBrush, 1f);
            Pen pen2 = new Pen(m_buttonLineBrush, 1f);
            e.Graphics.FillRectangle(m_buttonBackBrush, m_buttonRectangle);
            e.Graphics.DrawRectangle(pen, m_buttonRectangle);
            int num = m_buttonRectangle.Left + 2;
            int num2 = m_buttonRectangle.Right - 2;
            int num3 = m_buttonRectangle.Top + 2;
            int num4 = m_buttonRectangle.Bottom - 2;
            int num5 = m_buttonRectangle.Top + (m_buttonRectangle.Height / 2);
            int num6 = m_buttonRectangle.Left + (m_buttonRectangle.Width / 2);
            e.Graphics.DrawLine(pen2, num, num5, num2, num5);
            if (m_open == false)
            {
                e.Graphics.DrawLine(pen2, num6, num3, num6, num4);
            }
        }
        
        #endregion // Private Methods

        #region Public Methods

        /// <summary>
        /// 閉じる
        /// </summary>
        public void Collapse()
        {
            if (m_expandHeight == -1)
            {
                m_expandHeight = base.Height;
            }

            m_open = false;
            base.SetBounds(0, 0, 0, (m_buttonRectangle.Bottom + m_buttonRectangle.Top) + 1, BoundsSpecified.Height);

            UpdatePanel();
        }

        /// <summary>
        /// 開く
        /// </summary>
        public void Expand()
        {
            if (m_expandHeight == -1)
            {
                m_expandHeight = base.Height;
            }

            m_open = true;
            base.SetBounds(0, 0, 0, m_expandHeight, BoundsSpecified.Height);

            if (m_lockUpdate == false && m_panelContainer != null)
            {
                m_panelContainer.ExpandPanel(this);
            }

            UpdatePanel();
        }

        public void SetExpand(bool expand)
        {
            if (expand)
            {
                Expand();
            }
            else
            {
                Collapse();
            }
        }

        #endregion // Public Methods

        #region Event

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            m_components = new Container();
            this.SuspendLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.Invalidate();
            base.OnLayout(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLocationChanged(EventArgs e)
        {
            base.Invalidate();
            base.OnLocationChanged(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (m_buttonRectangle.Contains(e.X, e.Y) || m_nameBounds.Contains(e.X, e.Y))
            {
                this.ChangeOpenState();
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_nameFont != m_boldFont)
            {
                m_nameFont = m_boldFont;
                base.Invalidate();
            }

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_nameBounds.X != -1)
            {
                Font nameFont = m_nameFont;
                if (m_nameBounds.Contains(e.X, e.Y))
                {
                    m_nameFont = m_underlineFont;
                    if (this.Cursor != Cursors.Hand)
                    {
                        this.Cursor = Cursors.Hand;
                    }
                }
                else
                {
                    m_nameFont = m_boldFont;
                    if (this.Cursor != Cursors.Default)
                    {
                        this.Cursor = Cursors.Default;
                    }
                }

                if (m_nameFont != nameFont)
                {
                    base.Invalidate();
                }

                base.OnMouseMove(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen pen = new Pen(m_frameColor))
            {
                e.Graphics.DrawRectangle(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Right - 1, e.ClipRectangle.Height - 1);
            }

            using (Pen pen = new Pen(Color.FromArgb(87, 87, 87)))
            {
                e.Graphics.DrawLine(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Right-1, e.ClipRectangle.Top);
                e.Graphics.DrawLine(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Left, e.ClipRectangle.Bottom);
            }

            if (m_nameBounds.Left == -1)
            {
                m_nameBounds.X = m_buttonRectangle.Right + 3;
                m_nameBounds.Y = m_buttonRectangle.Top - 2;
                SizeF ef = e.Graphics.MeasureString(m_name, m_boldFont);
                m_nameBounds.Width = (int)ef.Width;
                m_nameBounds.Height = (int)ef.Height;
            }

            using (Brush brush = new SolidBrush(this.ForeColor))
            {
                e.Graphics.DrawString(m_name, m_nameFont, brush, new PointF((float)m_nameBounds.Left, (float)m_nameBounds.Top));
            }

            this.PaintButton(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (m_panelContainer != null)
            {
                m_panelContainer.RepositionControls();
            }

            base.OnSizeChanged(e);
        }

        #endregion
    }
}
