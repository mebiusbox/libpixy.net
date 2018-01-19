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
using System.ComponentModel;
using System.Windows.Forms;

namespace libpixy.net.Controls.ToolboxPanel
{
    public class ToolboxButton : Panel
    {
        #region Fields

        private ToolboxGroup m_group;
        private Container m_components;
        private string m_name;
        private Image m_icon = null;
        private Color m_buttonBackColor = Color.FromKnownColor(KnownColor.Control);
        private Color m_buttonForeColor = Color.FromArgb(0, 0, 0);
        private Color m_buttonLineColor = Color.FromKnownColor(KnownColor.Control);
        private Color m_focusBackColor = Color.FromArgb(193, 210, 238);
        private Color m_focusForeColor = Color.FromArgb(0, 0, 0);
        private Color m_focusLineColor = Color.FromArgb(49, 106, 197);
        private bool m_rounded = false;
        private bool m_focus = false;
        private bool m_drag = false;

        #endregion

        #region Properties

        [Browsable(true)]
        public Image Icon
        {
            get { return m_icon; }
            set 
            {
                m_icon = value;
                this.Invalidate();
            }
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
        public Color FocusBackColor
        {
            get { return m_focusBackColor; }
            set { m_focusBackColor = value; }
        }

        [Browsable(true)]
        public Color FocusForeColor
        {
            get { return m_focusForeColor; }
            set { m_focusForeColor = value; }
        }

        [Browsable(true)]
        public Color FocusLineColor
        {
            get { return m_focusLineColor; }
            set { m_focusLineColor = value; }
        }

        [Browsable(true)]
        public bool Rounded
        {
            get { return m_rounded; }
            set { m_rounded = value; }
        }

        [Browsable(true)]
        public ToolboxGroup Group
        {
            get { return m_group; }
            set { m_group = value; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ToolboxButton()
        {
            this.m_components = null;
            this.InitializeComponent();
            this.DoubleBuffered = true;
            this.SetBounds(0, 0, 0, 24, BoundsSpecified.Height);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitializeComponent()
        {
            this.m_components = new Container();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            Rectangle rc = this.ClientRectangle;
            rc.Width -= 1;
            rc.Height -= 1;

            Color backColor = this.ButtonBackColor;
            Color foreColor = this.ButtonForeColor;
            Color lineColor = this.ButtonLineColor;

            if (this.m_focus)
            {
                backColor = this.FocusBackColor;
                foreColor = this.FocusForeColor;
                lineColor = this.FocusLineColor;
            }

            if (this.Rounded)
            {
                Rectangle rcInner = rc;
                libpixy.net.Controls.Diagram.Draw.FillRoundedRectangle(e.Graphics, rcInner, 10, backColor);
                libpixy.net.Controls.Diagram.Draw.DrawRoundedRectangle(e.Graphics, rcInner, 10, ToolboxGroup.GetMiddleColor(backColor, this.BackColor), 2);
                rcInner.Inflate(-1, -1);
                libpixy.net.Controls.Diagram.Draw.DrawRoundedRectangle(e.Graphics, rcInner, 10, lineColor, 1);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    using (Pen pen = new Pen(lineColor))
                    {
                        e.Graphics.FillRectangle(brush, rc);
                        e.Graphics.DrawRectangle(pen, rc);
                    }
                }
            }

            ToolboxGroup.AdjustRectangle(ref rc, 10, 0);

            if (this.Icon != null)
            {
                Size sz = this.Icon.Size;
                int x = rc.X;
                int y = (rc.Y + rc.Height / 2) - (sz.Height / 2);
                e.Graphics.DrawImage(this.Icon, x, y);
                ToolboxGroup.AdjustRectangle(ref rc, sz.Width + 2, 0);
            }

            using (SolidBrush brush = new SolidBrush(foreColor))
            {
                e.Graphics.DrawString(this.Label, this.Font, brush, rc, sf);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.m_drag)
            {
                DataFormats.Format fmt = DataFormats.GetFormat("libpixy.net.Toolbox.Button");
                DataObject dataObj = new DataObject(fmt.Name, this);
                DoDragDrop(dataObj, DragDropEffects.All);
                this.m_drag = false;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.m_focus = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            this.m_focus = false;
            this.m_drag = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.m_drag = true;
            base.OnMouseDown(e);
        }
    }
}
