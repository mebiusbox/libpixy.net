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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace libpixy.net.Controls
{
    public partial class FlatComboBox : ComboBox
    {
        #region Globals

        /// <summary>
        /// Enum with all the possible styles
        /// </summary>
        public enum styles
        {
            officeXP,
            office2003,
            Dark
        }

        /// <summary>
        /// Enum with all the possible states
        /// </summary>
        public enum states
        {
            normal,
            focused,
            dropeddown,
            disabled
        }

        #endregion

        #region Variables

        /// <summary>
        /// Variable to save the current style
        /// </summary>
        styles style = styles.officeXP;

        /// <summary>
        /// Variable to save the current state
        /// </summary>
        states state = states.normal;

        /// <summary>
        /// All the pen and brushes needed
        /// </summary>
        Pen BorderPen;
        Brush ArrowBrush;
        Brush ButtonBrush;
        Brush TextBrush;

        /// <summary>
        /// The rectangle, surrounding the hole control
        /// </summary>
        Rectangle MainRect;

        /// <summary>
        /// The rectangle, surrounding the hole button
        /// </summary>
        Rectangle ButtonSurRect;

        /// <summary>
        /// The rectangle for the button
        /// </summary>
        Rectangle ButtonRect;

        /// <summary>
        /// The three points of the arrow
        /// </summary>
        PointF[] pntArrow;

        /// <summary>
        /// The middle of the button, used to center the arrow
        /// </summary>
        int VerticalMiddle;

        /// <summary>
        /// The path for the arrow
        /// </summary>
        GraphicsPath ArrowPath = new GraphicsPath();

        /// <summary>
        /// The location for drawing the text (in case dropdownstyle=dropdownlist)
        /// </summary>
        PointF TextLocation;

        /// <summary>
        /// The graphics
        /// </summary>
        Graphics m_gfx = null;

        Bitmap m_offscreen = null;

        #endregion

        #region Override methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Take private resize action here
            if (m_offscreen != null)
            {
                m_offscreen.Dispose();
                m_offscreen = null;
            }

            this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        private const int WM_PAINT = 0x000F;
        private const int WM_SETFOCUS = 0x0007;
        private const int WM_KILLFOCUS = 0x0008;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MOUSELEAVE = 0x02A3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_PAINT:   // WM_PAINT

                    // 'Simple' is not currently supported
                    if (this.DropDownStyle == ComboBoxStyle.Simple)
                    {
                        return;
                    }

                    // Start drawing...

#if false
                    if (m_offscreen == null)
                    {
                        m_offscreen = new Bitmap(this.Width, this.Height);
                    }

                    Graphics off_gfx = Graphics.FromImage(m_offscreen);
                    System.Diagnostics.Debug.Assert(off_gfx != null);

                    // clear everything
                    if (this.Enabled)
                    {
                        off_gfx.Clear(this.BackColor);
                    }
                    else 
                    {
                        off_gfx.Clear(Color.FromName("control"));
                    }

                    // call the darwing functions
                    if (this.HighlightImage != null || this.Image != null)
                    {
                        DrawImage(off_gfx);
                    }
                    else
                    {
                        DrawButton(off_gfx);
                        DrawArrow(off_gfx);
                        DrawBorder(off_gfx);
                    }

                    DrawText(off_gfx);

                    m_gfx = this.CreateGraphics();
                    m_gfx.DrawImage(m_offscreen, this.ClientRectangle);
                    m_gfx.Dispose();
                    m_gfx = null;

                    off_gfx.Dispose();
                    off_gfx = null;
#else
                    m_gfx = this.CreateGraphics();

                    // clear everything
                    if (this.Enabled)
                    {
                        m_gfx.Clear(this.BackColor);
                    }
                    else
                    {
                        m_gfx.Clear(Color.FromName("control"));
                    }

                    // call the darwing functions
                    if (this.HighlightImage != null || this.Image != null)
                    {
                        DrawImage(m_gfx);
                    }
                    else
                    {
                        DrawButton(m_gfx);
                        DrawArrow(m_gfx);
                        DrawBorder(m_gfx);
                    }

                    DrawText(m_gfx);

                    m_gfx.Dispose();
                    m_gfx = null;
#endif

                    break;

                case WM_SETFOCUS:
                case WM_KILLFOCUS:
                case WM_MOUSEMOVE:
                case WM_MOUSELEAVE: // if you move the mouse fast over the combobox, mouseleave doesn't always react
                    UpdateState();
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public FlatComboBox()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.Opaque, true);
            timer1.Interval = 100;
            timer1.Enabled = true;
            this.CenterPartOffset = 5;
            this.RightPartOffset = 10;
            this.Image = null;
            this.HighlightImage = null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateState()
        {
            states temp = state;
            if (this.Enabled)
            {
                if (this.DroppedDown)
                {
                    this.state = states.dropeddown;
                }
                else
                {
                    Point mouse_position = Control.MousePosition;
                    Point client_position = PointToClient(mouse_position);
                    System.Diagnostics.Debug.Assert(this.ClientRectangle != null);
                    if (this.ClientRectangle.Contains(client_position))
                    {
                        this.state = states.focused;
                    }
                    else if (this.Focused)
                    {
                        this.state = states.focused;
                    }
                    else
                    {
                        this.state = states.normal;
                    }
                }
            }
            else
            {
                this.state = states.disabled;
            }

            if (state != temp)
            {
                this.Invalidate();
            }
        }

        #endregion

        #region Public Property's

        /// <summary>
        /// Property to let the user change the style
        /// </summary>
        [Browsable(true)]
        public styles FlatComboStyle
        {
            get { return style; }
            set { style = value; }
        }

        [Browsable(true)]
        public Image HighlightImage { get; set; }

        [Browsable(true)]
        public Image Image { get; set; }

        [Browsable(true)]
        public int CenterPartOffset { get; set; }

        [Browsable(true)]
        public int RightPartOffset { get; set; }
        
        #endregion

        #region Drawing Functions

        /// <summary>
        /// Draw a button
        /// </summary>
        /// <param name="g"></param>
        private void DrawButton(Graphics g)
        {
            if (this.RightToLeft == RightToLeft.No)
            {
                ButtonRect = new Rectangle(this.Width - 18, 1, 17, this.Height - 2);
            }
            else
            {
                ButtonRect = new Rectangle(1, 1, 17, this.Height - 2);
            }

            if (state == states.normal)
            {
                if (style == styles.officeXP)
                {
                    ButtonBrush = new SolidBrush(Color.FromName("control"));
                }
                else if (style == styles.office2003)
                {
                    ButtonBrush = new LinearGradientBrush(ButtonRect, Color.FromArgb(214, 232, 253), Color.FromArgb(156, 189, 235), LinearGradientMode.Vertical);
                }
                else if (style == styles.Dark)
                {
                    ButtonBrush = new SolidBrush(Color.FromArgb(40, 40, 40));
                }
            }
            else if (state == states.focused)
            {
                if (style == styles.officeXP)
                {
                    ButtonBrush = new SolidBrush(Color.FromArgb(193, 210, 238));
                }
                else if (style == styles.office2003)
                {
                    ButtonBrush = new LinearGradientBrush(ButtonRect, Color.FromArgb(255, 242, 200), Color.FromArgb(255, 210, 148), LinearGradientMode.Vertical);
                }
                else if (style == styles.Dark)
                {
                    ButtonBrush = new SolidBrush(Color.FromArgb(60, 60, 60));
                }
            }
            else if (state == states.dropeddown)
            {
                if (style == styles.officeXP)
                {
                    ButtonBrush = new SolidBrush(Color.FromArgb(152, 181, 226));
                }
                else if (style == styles.office2003)
                {
                    ButtonBrush = new LinearGradientBrush(ButtonRect, Color.FromArgb(254, 149, 82), Color.FromArgb(255, 207, 139), LinearGradientMode.Vertical);
                }
                else if (style == styles.Dark)
                {
                    ButtonBrush = new SolidBrush(Color.FromArgb(60, 60, 60));
                }
            }
            else if (state == states.disabled)
            {
                ButtonBrush = new SolidBrush(Color.FromName("control"));
            }

            g.FillRectangle(ButtonBrush, ButtonRect);

            if (this.ButtonBrush != null)
            {
                this.ButtonBrush.Dispose();
                this.ButtonBrush = null;
            }
        }

        /// <summary>
        /// Draw a arrow
        /// </summary>
        /// <param name="g"></param>
        private void DrawArrow(Graphics g)
        {
            this.VerticalMiddle = this.Height/2;
            if (this.RightToLeft == RightToLeft.No)
            {
                pntArrow = new PointF[3];
                pntArrow[0] = new PointF(this.Width - 12, this.VerticalMiddle-1);
                pntArrow[1] = new PointF(this.Width - 10, this.VerticalMiddle+2);
                pntArrow[2] = new PointF(this.Width - 7, this.VerticalMiddle-1);
            }
            else
            {
                pntArrow = new PointF[3];
                pntArrow[0] = new PointF(7, this.VerticalMiddle-1);
                pntArrow[1] = new PointF(9, this.VerticalMiddle+2);
                pntArrow[2] = new PointF(12, this.VerticalMiddle-1);
            }

            if (state == states.normal ||
                state == states.focused)
            {
                if (style == styles.Dark)
                {
                    ArrowBrush = new SolidBrush(Color.White);
                }
                else
                {
                    ArrowBrush = new SolidBrush(Color.Black);
                }
            }
            else if (state == states.dropeddown)
            {
                if (style == styles.officeXP)
                {
                    ArrowBrush = new SolidBrush(Color.FromArgb(73, 73, 73));
                }
                else if (style == styles.office2003)
                {
                    ArrowBrush = new SolidBrush(Color.Black);
                }
                else if (style == styles.Dark)
                {
                    ArrowBrush = new SolidBrush(Color.White);
                }
            }
            else if (state == states.disabled)
            {
                ArrowBrush = new SolidBrush(Color.DarkGray);
            }

            g.FillPolygon(ArrowBrush, pntArrow);

            if (this.ArrowBrush != null)
            {
                this.ArrowBrush.Dispose();
                this.ArrowBrush = null;
            }
        }

        /// <summary>
        /// Draw a border
        /// </summary>
        /// <param name="g"></param>
        private void DrawBorder(Graphics g)
        {
            MainRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            if (this.RightToLeft != RightToLeft.No)
            {
                ButtonSurRect = new Rectangle(0, 0, ButtonRect.Width + 1, ButtonRect.Height + 1);
            }
            else
            {
                ButtonSurRect = new Rectangle(ButtonRect.X - 1, ButtonRect.Y - 1, ButtonRect.Width + 2, ButtonRect.Height + 2);
            }

            if (state == states.focused ||
                state == states.dropeddown)
            {
                if (style == styles.officeXP)
                {
                    BorderPen = new Pen(Color.FromArgb(49, 106, 197));
                }
                else if (style == styles.office2003)
                {
                    BorderPen = new Pen(Color.FromArgb(0, 0, 128));
                }
                else if (style == styles.Dark)
                {
                    BorderPen = new Pen(Color.FromArgb(20,20,20));
                }
            }
            else if (state == states.disabled)
            {
                BorderPen = new Pen(Color.DarkGray);
            }
            else if (style == styles.Dark)
            {
                BorderPen = new Pen(Color.FromArgb(20,20,20));
            }
            else
            {
                return;
            }

            if (state != states.disabled)
            {
                g.DrawRectangle(BorderPen, ButtonSurRect);
            }

            g.DrawRectangle(BorderPen, MainRect);

            if (this.BorderPen != null)
            {
                this.BorderPen.Dispose();
                this.BorderPen = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawImage(Graphics g)
        {
            if (state == states.focused || state == states.dropeddown)
            {
                if (this.HighlightImage != null)
                {
                    DrawImageCombo(g, this.HighlightImage);
                }

            }
            else if (this.Image != null)
            {
                DrawImageCombo(g, this.Image);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        private void DrawImageCombo(Graphics g, Image image)
        {
            int leftPartWidth = this.CenterPartOffset;
            int centerPartWidth = this.RightPartOffset - this.CenterPartOffset;
            int rightPartWidth = image.Width - this.RightPartOffset;

            int x = 0;
            int w = leftPartWidth;
            // left part
            g.DrawImage(image, new Rectangle(0, 0, w, image.Height), new Rectangle(0, 0, w, image.Height), GraphicsUnit.Pixel);
            
            x += w;
            w = this.Width - (leftPartWidth + rightPartWidth);

            // center part
            g.DrawImage(image, new Rectangle(x, 0, w, image.Height), new Rectangle(this.CenterPartOffset, 0, centerPartWidth, image.Height), GraphicsUnit.Pixel);

            x += w;
            w = rightPartWidth;

            // right part
            g.DrawImage(image, new Rectangle(x, 0, w, image.Height), new Rectangle(this.RightPartOffset, 0, w, image.Height), GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Draw a text
        /// </summary>
        /// <param name="g"></param>
        private void DrawText(Graphics g)
        {
            if (this.DropDownStyle != ComboBoxStyle.DropDownList)
            {
                return;
            }

            string text = "";
            if (state == states.normal || state == states.focused || state == states.dropeddown)
            {
                TextBrush = new SolidBrush(this.ForeColor);
            }
            else if (state == states.disabled)
            {
                TextBrush = new SolidBrush(Color.DarkGray);
            }

            if (g.MeasureString(this.Text, this.Font).Width > this.Width - 30)
            {
                int i = -1;
                while (true)
                {
                    i += 1;
                    if (g.MeasureString(text, this.Font).Width > this.Width - 30)
                    {
                        break;
                    }
                    text += this.Text.Substring(i, 1);
                }
            }
            else
            {
                text = this.Text;
            }

            if (this.RightToLeft == RightToLeft.No)
            {
                TextLocation = new PointF(1, 4);
            }
            else
            {
                Single temp = this.Width - (g.MeasureString(text, this.Font).Width);
                TextLocation = new PointF(temp, 4);
            }

            g.DrawString(text, this.Font, TextBrush, TextLocation);

            if (this.TextBrush != null)
            {
                this.TextBrush.Dispose();
                this.TextBrush = null;
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlatComboBox_EnabledChanged(object sender, EventArgs e)
        {
            UpdateState();
        }

        #endregion
    }
}
