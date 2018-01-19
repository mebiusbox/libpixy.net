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

namespace libpixy.net.Controls.ToolboxPanel
{
    /// <summary>
    /// 
    /// </summary>
    public class ToolboxGroup : Panel
    {
        #region Fields

        private Container m_components;
        private ToolboxContainer m_container;
        private Font m_boldFont;
        private Font m_underlineFont;
        private Color m_buttonBackColor = Color.White;
        private Color m_buttonForeColor = Color.FromArgb(0, 0, 120);
        private Color m_buttonLineColor = Color.Navy;
        private Color m_lineColor = Color.Navy;
        private Rectangle m_buttonRectangle;
        private int m_nameHeight;
        private string m_name;
        private Rectangle m_nameBounds;
        private Rectangle m_allBounds;
        private Size m_sizeSpacing;
        private Font m_nameFont;
        private bool m_open;

        public List<ToolboxButton> Buttons;

        #endregion Fields

        #region Properties

        [Browsable(true)]
        public ToolboxContainer GroupContainer
        {
            get { return m_container; }
            set { m_container = value; }
        }

        [Browsable(true)]
        public Color LineColor
        {
            get { return m_lineColor; }
            set { m_lineColor = value; }
        }

        [Browsable(true)]
        public Color ButtonBackColor
        {
            get { return m_buttonBackColor; }
            set { m_buttonBackColor = value; }
        }

        [Browsable(true)]
        public Color ButtonForeColor
        {
            get { return m_buttonForeColor; }
            set { m_buttonForeColor = value; }
        }

        [Browsable(true)]
        public Color ButtonLineColor
        {
            get { return m_buttonLineColor; }
            set { m_buttonLineColor = value; }
        }

        [Browsable(true)]
        public string Label
        {
            get { return m_name; }
            set
            {
                m_name = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public int LabelHeight
        {
            get { return m_nameHeight; }
            set 
            {
                m_nameHeight = value;
                this.Invalidate();
            }
        }

        #endregion Properties

        public ToolboxGroup()
        {
            this.Buttons = new List<ToolboxButton>();
            this.m_components = null;
            this.m_container = null;
            this.m_name = "Group";
            this.m_nameBounds = new Rectangle(-1, -1, -1, -1);
            this.m_allBounds = new Rectangle(-1, -1, -1, -1);
            this.m_buttonRectangle = new Rectangle(4, 4, 8, 8);
            this.m_open = true;
            this.m_nameHeight = 20;
            this.m_sizeSpacing = new Size(4, 4);
            this.InitializeComponent();
            this.m_boldFont = new Font("Tahoma", 8f, FontStyle.Bold);
            this.m_underlineFont = new Font("Tahoma", 8f, FontStyle.Underline | FontStyle.Bold);
            this.m_nameFont = this.m_boldFont;
            this.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
            this.DoubleBuffered = true;
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
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

            Buttons.Clear();
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
            Pen pen1 = new Pen(m_buttonForeColor);
            Pen pen2 = new Pen(m_buttonLineColor);
            SolidBrush brush = new SolidBrush(m_buttonBackColor);

            try
            {
                e.Graphics.FillRectangle(brush, m_buttonRectangle);
                e.Graphics.DrawRectangle(pen1, m_buttonRectangle);
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
            finally
            {
                pen1.Dispose();
                pen2.Dispose();
                brush.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            m_components = new Container();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control.GetType() == typeof(ToolboxButton))
            {
                ToolboxButton control = (ToolboxButton)e.Control;
                control.Group = this;
                this.Buttons.Add(control);
                this.UpdateButtonPositions();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(ToolboxButton))
            {
                ToolboxButton control = (ToolboxButton)e.Control;
                control.Group = null;
                this.Buttons.Remove(control);
                this.UpdateButtonPositions();
            }

            base.OnControlRemoved(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levent"></param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            this.UpdateButtonPositions();
        }

#if false
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (m_container != null)
            {
                m_container.UpdateGroupPositions();
            }

            base.OnSizeChanged(e);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            SolidBrush brushBack = new SolidBrush(this.BackColor);
            SolidBrush brushFore = new SolidBrush(this.ForeColor);
            Pen pen = new Pen(m_lineColor);

            try
            {
                e.Graphics.FillRectangle(brushBack, this.ClientRectangle);

                //e.Graphics.DrawRectangle(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Right - 1, e.ClipRectangle.Height - 1);
                e.Graphics.DrawRectangle(pen, this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Right - 1, this.ClientRectangle.Height - 1);

                if (m_nameBounds.Left == -1)
                {
                    m_nameBounds.X = m_buttonRectangle.Right + 3;
                    m_nameBounds.Y = m_buttonRectangle.Top - 2;
                    SizeF ef = e.Graphics.MeasureString(m_name, m_boldFont);
                    m_nameBounds.Width = (int)ef.Width;
                    m_nameBounds.Height = (int)ef.Height;
                }

                e.Graphics.DrawString(m_name, m_nameFont, brushFore, new PointF((float)m_nameBounds.Left, (float)m_nameBounds.Top));
            }
            finally
            {
                brushBack.Dispose();
                brushFore.Dispose();
                pen.Dispose();
            }

            this.PaintButton(e);
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
        protected void ChangeOpenState()
        {
            if (m_open)
            {
                m_open = false;
                base.SetBounds(0, 0, 0, m_nameHeight, BoundsSpecified.Height);
            }
            else
            {
                m_open = true;
                base.SetBounds(0, 0, 0, m_allBounds.Height, BoundsSpecified.Height);
            }

            //System.Diagnostics.Debug.WriteLine(base.Bounds.ToString());

            if (m_container != null)
            {
                m_container.UpdateGroupPositions();
            }

            base.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateButtonPositions()
        {
            //System.Diagnostics.Debug.WriteLine("ToolboxGroup.UpdateButtonPositions");

            m_allBounds = new Rectangle(0, 0, this.ClientSize.Width - this.Margin.Horizontal, 0);
            m_allBounds.Height += this.Margin.Vertical;
            m_allBounds.Height += this.m_nameHeight;

            int x = this.Margin.Left + this.m_sizeSpacing.Width;
            int y = this.Margin.Top + this.m_sizeSpacing.Height + this.m_nameHeight;
            int w = this.ClientSize.Width - (this.Margin.Horizontal + this.m_sizeSpacing.Width*2);

            foreach (ToolboxButton btn in Buttons)
            {
                int h = btn.Height + this.m_sizeSpacing.Height * 2;
                m_allBounds.Height += h;
                btn.Bounds = new Rectangle(x, y, w, btn.Height);
                y += h;
            }

            if (m_open)
            {
                base.SetBounds(0, 0, 0, m_allBounds.Height, BoundsSpecified.Height);
            }
            else
            {
                base.SetBounds(0, 0, 0, m_nameHeight, BoundsSpecified.Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="ofsX"></param>
        /// <param name="ofsY"></param>
        public static void AdjustRectangle(ref Rectangle rc, int ofsX, int ofsY)
        {
            rc.Width = rc.Width - ofsX;
            rc.Height = rc.Height - ofsY;
            rc.X += ofsX;
            rc.Y += ofsY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Color GetMiddleColor(Color c1, Color c2)
        {
            return Color.FromArgb(
                (c1.R + c2.R) / 2,
                (c1.G + c2.G) / 2,
                (c1.B + c2.B) / 2);
        }
    }
}
