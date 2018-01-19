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

namespace libpixy.net.Controls.Timeline
{
    #region Enumeration

    /// <summary>
    /// 
    /// </summary>
    public enum TimelineAlignment
    {
        Left = 0,
        Right,
        Top,
        Bottom,
        Center
    }

    #endregion

    //[ToolboxBitmap(typeof(Ruler), "Ruler.bmp")]
    //[ToolboxItem(true)]
    public class TimelineHeader : System.Windows.Forms.Control
    {
        #region Fields

        private Tool.Mouse m_mouse = new Tool.Mouse();
        private Bitmap m_offscreen = null;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container m_components = null;

        private TimelineAlignment m_markAlignment = TimelineAlignment.Bottom;
        private TimelineAlignment m_numberAlignment = TimelineAlignment.Center;
        private TimelineAlignment m_numberVerticalAlignment = TimelineAlignment.Top;
        private Border3DStyle m_3dBorderStyle = Border3DStyle.Etched;
        private Border3DSide m_3dBorderSide = Border3DSide.All;
        private Padding m_borderSize = new Padding(0, 0, 0, 0);
        private Color m_borderColor = Color.FromArgb(0, 0, 0);
        private int m_startTime = 1;
        private int m_endTime = 100;
        private int m_showNumberInterval = 10;
        private int m_numberOfDivisions = -1;
        private int m_intervalMarkFactor = 1;
        private int m_divisionMarkFactor = 5;
        private int m_headPosition = -1;
        private int m_headPositionWidth = 3;
        private bool m_verticalNumbers = true;
        private bool m_showHeadPosition = true;
        private bool m_showHeadPositionFrame = true;
        private bool m_showRuler = true;
        private bool m_enableHeadPositionDrag = true;
        private bool m_updateOffscreen = true;
        private Color m_markColor;
        private Color m_headPositionColor;
        private Color m_headPositionFrameColor;

        #endregion

        #region Events

        public class HeadPositionChangedEventArgs : Tools.TEventArgs<int>
        {
            public HeadPositionChangedEventArgs()
            {
            }

            public HeadPositionChangedEventArgs(int value)
                : base(value)
            {
            }
        }

        public delegate void HeadPositionChangedEvent(object sender, HeadPositionChangedEventArgs e);
        public event HeadPositionChangedEvent HeadPositionChanged;
        public void RaiseHeadPositionChanged()
        {
            if (HeadPositionChanged != null)
            {
                HeadPositionChanged(this, new HeadPositionChangedEventArgs(this.HeadPosition));
            }
        }

        #endregion

        #region Ctor/Dtor

        public TimelineHeader()
        {
            base.BackColor = System.Drawing.Color.White;
            base.ForeColor = System.Drawing.Color.Black;
            this.m_markColor = base.BackColor;
            this.m_headPositionColor = Color.FromArgb(173, 163, 152);
            this.m_headPositionFrameColor = Color.FromArgb(92, 92, 92);

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public double PixelToValue(int offset)
        {
            return CalculateValue(offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleValue"></param>
        /// <returns></returns>
        public int ValueToPixel(double scaleValue)
        {
            return CalculatePixel(scaleValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleValue"></param>
        /// <returns></returns>
        public int ValueToPixel(int value)
        {
            return CalculatePixel((double)value);
        }

        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //
            // Timeline
            //
            this.Name = "TimelineHeader";
            this.MouseMove += new MouseEventHandler(TimelineHeader_MouseMove);
            this.MouseUp += new MouseEventHandler(TimelineHeader_MouseUp);
            this.MouseDown += new MouseEventHandler(TimelineHeader_MouseDown);
            this.MouseClick += new MouseEventHandler(TimelineHeader_MouseClick);
        }
        #endregion

        #region Overrides

        protected override void Dispose(bool disposing)
        {
            if (disposing && m_components != null)
            {
                m_components.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Take private resize action here
            RecalcParam();
            m_offscreen = null;
            this.Invalidate();
        }

        [Description("Draws the ruler marks in the scale requested.")]
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawControl(e.Graphics);
        }

        #endregion

        #region Event Handlers

        private void TimelineHeader_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_mouse.Down(e);

            if (m_showHeadPosition && m_enableHeadPositionDrag)
            {
                int value = (int)CalculateValue(e.X);
                if (value >= 0)
                {
                    m_mouse.BeginDrag();
                }
            }
        }

        private void TimelineHeader_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_mouse.Up(e);

            if (m_mouse.Drag.Valid)
            {
                m_mouse.EndDrag();
            }
            else if (m_enableHeadPositionDrag)
            {
                int value = (int)CalculateValue(e.X);
                if (value >= 0)
                {
                    this.HeadPosition = value;
                    RaiseHeadPositionChanged();
                }
            }

            if ((Control.MouseButtons & MouseButtons.Right) != 0)
            {
                this.ContextMenu.Show(this, PointToClient(Control.MousePosition));
            }
            else
            {
                EventArgs eClick = new EventArgs();
                this.OnClick(eClick);
            }
        }

        private void TimelineHeader_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_mouse.Move(e);

            if (m_mouse.Drag.Valid)
            {
                int value = (int)CalculateValue(e.X);
                if (value >= 0)
                {
                    this.HeadPosition = value;
                    RaiseHeadPositionChanged();
                }
            }
        }

        private void TimelineHeader_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //TODO: ドラッグ変更できなければマウスクリックにも反応しないようになっている。
            if (m_enableHeadPositionDrag)
            {
                int value = (int)CalculateValue(e.X);
                if (value >= 0)
                {
                    this.HeadPosition = value;
                    RaiseHeadPositionChanged();
                }
            }
        }

        private void Popup_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem)sender;
            Refresh();
        }

#if false
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            Invalidate();
        }
#endif

#if false
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            Invalidate();
        }
#endif

#if false
        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Popup");
        }
#endif

        #endregion

        #region Properties

        [DefaultValue(typeof(Border3DStyle), "Etched")]
        [Description("The border style use the Windows.Forms.Border3DStyle type")]
        [Category("Timeline")]
        public Border3DStyle BorderStyle
        {
            get
            {
                return m_3dBorderStyle;
            }
            set
            {
                m_3dBorderStyle = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Border3DSide), "All")]
        [Description("The border side use the Windows.Forms.Border3DSide type")]
        [Category("Timeline")]
        public Border3DSide BorderSide
        {
            get
            {
                return m_3dBorderSide;
            }
            set
            {
                m_3dBorderSide = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("The border side use the Windows.Forms.Border3DSide type")]
        [Category("Timeline")]
        public Padding BorderSize
        {
            get
            {
                return m_borderSize;
            }
            set
            {
                m_borderSize = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("The border side use the Windows.Forms.Border3DSide type")]
        [Category("Timeline")]
        public Color BorderColor
        {
            get
            {
                return m_borderColor;
            }
            set
            {
                m_borderColor = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("A value from which the ruler marking should be shown. Deafult is zero.")]
        [Category("Timeline")]
        public int StartTime
        {
            get { return m_startTime; }
            set
            {
                m_startTime = value;
                m_updateOffscreen = true;
                RecalcParam();
                Invalidate();
            }
        }

        [Description("A value from which the ruler marking should be shown. Deafult is zero.")]
        [Category("Timeline")]
        public int EndTime
        {
            get { return m_endTime; }
            set
            {
                m_endTime = value;
                m_updateOffscreen = true;
                RecalcParam();
                Invalidate();
            }
        }

        public int Duration
        {
            get { return m_endTime - m_startTime; }
        }

        [Description("The value of the major interval. When displaying inches, 1 is a typical value. When displaying Points, 36 or 72 might good values.")]
        [Category("Timeline")]
        public int ShowNumberInterval
        {
            get { return m_showNumberInterval; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The major interval value cannot be less than one");
                }

                m_showNumberInterval = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

           
        [Description("How many divisions should be shown between each major interval")]
        [Category("Timeline")]
        public int Divisions
        {
            get
            {
                return m_numberOfDivisions;
            }
        }


        [Description("The height or with of this control multiplied by the reciprocal of this number will be used to compute the height of the interval division marks.")]
        [Category("Timeline")]
        public int IntervalMarkFactor
        {
            get { return m_intervalMarkFactor; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The Interval Mark Factor cannot be less than one");
                }

                m_intervalMarkFactor = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("The height or with of this control multiplied by the reciprocal of this number will be used to compute the height of the non-middle division marks.")]
        [Category("Timeline")]
        public int DivisionMarkFactor
        {
            get { return m_divisionMarkFactor; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The Division Mark Factor cannot be less than one");
                }

                m_divisionMarkFactor = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("The value of the current mouse position expressed in unit of the scale set (centimetres, inches, etc...)")]
        [Category("Timeline")]
        public double ScaleValue
        {
            get { return CalculateValue(m_mouse.Location.X); }
        }

        [Description("The font used to display the division number")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Return the mouse position as number of pixels from the top or left of the control. -1 means that the mouse is positioned before or after the control.")]
        [Category("Timeline")]
        public int MouseLocation
        {
            get { return m_mouse.Location.X; }
        }

        [DefaultValue(typeof(Color), "ControlDarkDark")]
        [Description("The color used to lines and numbers on the ruler")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "White")]
        [Description("The color used to paint the background of the ruler")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Mark color")]
        [Category("Timeline")]
        public Color MarkColor
        {
            get { return m_markColor; }
            set
            {
                m_markColor = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Head Position Color")]
        [Category("Timeline")]
        public Color HeadPositionColor
        {
            get { return m_headPositionColor; }
            set
            {
                m_headPositionColor = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Head Position Frame Color")]
        [Category("Timeline")]
        public Color HeadPositionFrameColor
        {
            get { return m_headPositionFrameColor; }
            set
            {
                m_headPositionFrameColor = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Head Position Width")]
        [Category("Timeline")]
        public int HeadPositionWidth
        {
            get { return m_headPositionWidth; }
            set { m_headPositionWidth = value; }
        }

        [Description("")]
        [Category("Timeline")]
        public bool VerticalNumbers
        {
            get { return m_verticalNumbers; }
            set
            {
                m_verticalNumbers = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Determines how the ruler markings are displayed")]
        [Category("Timeline")]
        public TimelineAlignment MarkAlignment
        {
            get { return m_markAlignment; }
            set
            {
                if (m_markAlignment == value)
                {
                    return;
                }

                m_markAlignment = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Determines how the interval numbers are displayed")]
        [Category("Timeline")]
        public TimelineAlignment NumberAlignment
        {
            get { return m_numberAlignment; }
            set
            {
                if (this.m_numberAlignment == value)
                {
                    return;
                }

                m_numberAlignment = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Determines how the vertical interval numbers are displayed")]
        [Category("Timeline")]
        public TimelineAlignment NumberVerticalAlignment
        {
            get { return m_numberVerticalAlignment; }
            set
            {
                if (this.m_numberVerticalAlignment == value)
                {
                    return;
                }

                m_numberVerticalAlignment = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Display head position")]
        [Category("Timeline")]
        public bool ShowHeadPosition
        {
            get { return m_showHeadPosition; }
            set
            {
                m_showHeadPosition = value;
                m_updateOffscreen = true;
                Invalidate();
            }
        }

        [Description("Display Head Position Frame")]
        [Category("Timeline")]
        public bool ShowHeadPositionFrame
        {
            get { return m_showHeadPositionFrame; }
            set { m_showHeadPositionFrame = value; }
        }

        [Description("Head position")]
        [Category("Timeline")]
        public int HeadPosition
        {
            get { return m_headPosition; }
            set 
            {
                m_headPosition = value;
                Refresh();
            }
        }

        [Description("Enable head position dragging")]
        [Category("Timeline")]
        public bool EnableHeadPositionDrag
        {
            get { return m_enableHeadPositionDrag; }
            set { m_enableHeadPositionDrag = value; }
        }

        [Description("Display Ruler")]
        [Category("Timeline")]
        public bool ShowRuler
        {
            get { return m_showRuler; }
            set { m_showRuler = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PixelPerTime
        {
            get
            {
                int w = this.Width - Offset();
                return (double)w / (double)Duration;
            }
        }


        #endregion

        #region Private functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private double CalculateValue(int offset)
        {
            if (offset < 0)
            {
                return m_startTime;
            }

            int w = (this.Width - Start()*2);
            double value = ((double)(offset - Start()) / (double)w) * (double)this.Duration + (double)m_startTime;
            return Math.Min(value, (double)this.Duration + (double)m_startTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleValue"></param>
        /// <returns></returns>
        [Description("May not return zero even when a -ve scale number is given as the returned value needs to allow for the border thickness")]
        private int CalculatePixel(double scaleValue)
        {
            double value = Convert.ToInt32(scaleValue - m_startTime);
            if (value <= 0)
            {
                return Start();
            }

            int w = this.Width - Offset();
            return Start() + Convert.ToInt32(value / (double)this.Duration * (double)w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public void RenderHeadPosition(Graphics g)
        {
            if (m_showHeadPosition == false)
            {
                return;
            }

            int start = m_startTime;
            int end = m_endTime;
            if (start <= m_headPosition && m_headPosition < end)
            {
                int x1 = ValueToPixel(m_headPosition);
#if false
                int x2 = ValueToPixel(m_headPosition+1);
                Rectangle rc = new Rectangle(x1, 0, x2-x1, this.Height-1);
                Pen pen = new Pen(m_headPositionFrameColor);
                Brush brush = new SolidBrush(m_headPositionColor);
                g.FillRectangle(brush, rc);
                g.DrawRectangle(pen, rc);
                brush.Dispose();
                pen.Dispose();
#endif
                int x2 = x1 + m_headPositionWidth;
                Rectangle rc = new Rectangle(x1, 0, x2 - x1, this.Height);
                using (Brush brush = new SolidBrush(m_headPositionColor))
                {
                    g.FillRectangle(brush, rc);
                }

                if (m_showHeadPositionFrame)
                {
                    DrawValue(g, this.Font, m_headPositionColor, m_headPosition, x1, 3);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawControl(Graphics g)
        {
            if (this.Visible == false)
            {
                return;
            }

            if (this.Width < 1 || this.Height < 1)
            {
#if DEBUG
                System.Diagnostics.Trace.WriteLine("Minimized?");
#endif
                return;
            }

            DrawControlToBitmap();

            try
            {
                // Always draw the bitmap
                g.DrawImage(m_offscreen, this.ClientRectangle);
                RenderHeadPosition(g);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawControlToBitmap()
        {
            if (m_offscreen == null)
            {
                // Create a bitmap
                m_offscreen = new Bitmap(this.Width, this.Height);
                m_updateOffscreen = true;
            }

            if (m_updateOffscreen == false)
            {
                return;
            }

            Graphics g = Graphics.FromImage(m_offscreen);

            try
            {
                // Wash the background with BackColor
                g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, m_offscreen.Width, m_offscreen.Height);

                if (m_showRuler)
                {
                    int value = m_startTime;
                    int valueMax = Math.Min(value + this.Duration, m_endTime);
                    int range = valueMax - value;

                    double fvalue = (double)value;
                    int x;

                    // Draw marks
                    value = m_startTime;
                    fvalue = (double)value;

                    for (int i = value; i < valueMax; ++i)
                    {
                        if (CheckDrawIntervalMark(i))
                        {
                            x = ValueToPixel((double)i);
                            DivisionMark(g, x, m_intervalMarkFactor);
                            DrawValue(g, this.Font, this.ForeColor, i, x, 3);
                        }
                        else if (CheckDrawDivisionMark(i))
                        {
                            x = ValueToPixel((double)i);
                            DivisionMark(g, x, m_divisionMarkFactor);
                        }
                    }
                }

                if (m_3dBorderStyle != Border3DStyle.Flat)
                {
                    ControlPaint.DrawBorder3D(g, this.ClientRectangle, this.m_3dBorderStyle, this.m_3dBorderSide);
                }

                if (m_borderSize.Top > 0 || m_borderSize.Left > 0 || m_borderSize.Right > 0 || m_borderSize.Bottom > 0)
                {
                    using (Brush brush = new SolidBrush(this.BorderColor))
                    {
                        if (m_borderSize.Top > 0) {
                            g.FillRectangle(brush, new Rectangle(0, 0, m_offscreen.Width, m_borderSize.Top));
                        }
                        if (m_borderSize.Bottom > 0) {
                            g.FillRectangle(brush, new Rectangle(0, m_offscreen.Height-m_borderSize.Bottom, m_offscreen.Width, m_borderSize.Bottom));
                        }
                        if (m_borderSize.Left > 0) {
                            g.FillRectangle(brush, new Rectangle(0, 0, m_borderSize.Left, m_offscreen.Height));
                        }
                        if (m_borderSize.Right > 0) {
                            g.FillRectangle(brush, new Rectangle(m_offscreen.Width-m_borderSize.Right, 0, m_borderSize.Right, m_offscreen.Height));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                g.Dispose();
            }

            m_updateOffscreen = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="position"></param>
        /// <param name="proportion"></param>
        private void DivisionMark(Graphics g, int position, int proportion)
        {
            // This function is affected by the RulerAlignment setting
            int markStart = 0;
            int markEnd = 0;

            switch (m_markAlignment)
            {
                case TimelineAlignment.Bottom:
                    markStart = this.Height - this.Height / proportion;
                    markEnd = this.Height;
                    break;

                case TimelineAlignment.Center:
                    markStart = (this.Height - this.Height / proportion) / 2 - 1;
                    markEnd = markStart + this.Height / proportion;
                    break;

                case TimelineAlignment.Top:
                    markStart = 0;
                    markEnd = this.Height / proportion;
                    break;
            }

            Line(g, position, markStart, position, markEnd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <param name="spaceAvailable"></param>
        private void DrawValue(Graphics g, Font font, Color color, int value, int position, int spaceAvailable)
        {
            // The sizing operation is common to all options
            StringFormat format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
            if (m_verticalNumbers)
            {
                format.FormatFlags |= StringFormatFlags.DirectionVertical;
            }

            SizeF size = g.MeasureString((value).ToString(), this.Font, spaceAvailable, format);
            Point drawingPoint;
            int x = 0;
            int y = 0;
            switch (m_numberVerticalAlignment)
            {
                case TimelineAlignment.Bottom:
                    y = this.Height - 2 - (int)size.Height;
                    break;

                case TimelineAlignment.Center:
                    y = (this.Height - (int)size.Height) / 2 - 2;
                    break;

                case TimelineAlignment.Top:
                    y = 2;
                    break;
            }

            switch (m_numberAlignment)
            {
                case TimelineAlignment.Right:
                    x = position + spaceAvailable - (int)size.Width - 2;
                    break;

                case TimelineAlignment.Center:
                    x = position + spaceAvailable - (int)size.Width / 2;
                    break;

                case TimelineAlignment.Left:
                    x = position + spaceAvailable + 2;
                    break;
            }

            drawingPoint = new Point(x, y);

            // The drawstring function is common to all operations

            g.DrawString(value.ToString(), font, new SolidBrush(color), drawingPoint, format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool CheckDrawIntervalMark(int value)
        {
            if ((value % m_showNumberInterval) == 0)
            {
                return true;
            }

            if (value == m_startTime)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool CheckDrawDivisionMark(int value)
        {
            if ((value % m_numberOfDivisions) == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void Line(Graphics g, int x1, int y1, int x2, int y2)
        {
            using (SolidBrush brush = new SolidBrush(this.MarkColor))
            {
                using (Pen pen = new Pen(brush))
                {
                    g.DrawLine(pen, x1, y1, x2, y2);
                    pen.Dispose();
                    brush.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int Offset()
        {
            int offset = 0;
            switch (m_3dBorderStyle)
            {
                case Border3DStyle.Flat: offset = 0; break;
                case Border3DStyle.Adjust: offset = 0; break;
                case Border3DStyle.Sunken: offset = 2; break;
                case Border3DStyle.Bump: offset = 2; break;
                case Border3DStyle.Etched: offset = 2; break;
                case Border3DStyle.Raised: offset = 2; break;
                case Border3DStyle.RaisedInner: offset = 1; break;
                case Border3DStyle.RaisedOuter: offset = 1; break;
                case Border3DStyle.SunkenInner: offset = 1; break;
                case Border3DStyle.SunkenOuter: offset = 1; break;
                default: offset = 0; break;
            }

            return offset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int Start()
        {
            int start = 0;
            switch (m_3dBorderStyle)
            {
                case Border3DStyle.Flat: start = 0; break;
                case Border3DStyle.Adjust: start = 0; break;
                case Border3DStyle.Sunken: start = 1; break;
                case Border3DStyle.Bump: start = 1; break;
                case Border3DStyle.Etched: start = 1; break;
                case Border3DStyle.Raised: start = 1; break;
                case Border3DStyle.RaisedInner: start = 0; break;
                case Border3DStyle.RaisedOuter: start = 0; break;
                case Border3DStyle.SunkenInner: start = 0; break;
                case Border3DStyle.SunkenOuter: start = 0; break;
                default: start = 0; break;
            }

            return start;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPosition"></param>
        private void ChangeMousePosition(int newPosition)
        {
            m_mouse.OldLocation.X = m_mouse.Location.X;
            m_mouse.Location.X = newPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RecalcParam()
        {
            if (this.Duration > 0)
            {
                int w = this.Width - Offset();
                int div = 1;
                double pixel;
                for (;;)
                {
                    pixel = (double)w / ((double)this.Duration / (double)div);
                    if (pixel >= 5)
                    {
                        m_numberOfDivisions = div;
                        break;
                    }

                    div *= 2;
                    if (div >= this.Duration)
                    {
                        m_numberOfDivisions = this.Duration;
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
