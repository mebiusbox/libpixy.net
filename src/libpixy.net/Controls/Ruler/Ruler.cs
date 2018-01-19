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

namespace libpixy.net.Controls.Ruler
{
    #region Enumeration

    /// <summary>
    /// 
    /// </summary>
    public enum RulerOrientation
    {
        Horizontal = 0,
        Vertical
    }

    /// <summary>
    /// 
    /// </summary>
    public enum RulerScaleMode
    {
        Points = 0,
        Pixels,
        Centimetres,
        Inches,
        Millimeters
    }

    /// <summary>
    /// 
    /// </summary>
    public enum RulerAlignment
    {
        TopOrLeft = 0,
        Middle,
        BottomOrRight
    }

    /// <summary>
    /// 
    /// </summary>
    internal enum Msg
    {
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSELEAVE = 0x02A3,
        WM_NCMOUSELEAVE = 0x02A2,
    }

    #endregion

    //[ToolboxBitmap(typeof(Ruler), "Ruler.bmp")]
    //[ToolboxItem(true)]
    public class Ruler : System.Windows.Forms.Control, IMessageFilter
    {
        #region Internal Variables

        private int  m_scale;
        private bool m_drawLine = false;
        private bool m_inControl = false;
        private int m_mousePosition = 1;
        private int m_oldMousePosition = -1;
        private Bitmap m_bitmap = null;

        #endregion

        #region Property variable

        private RulerOrientation m_orientation;
        private RulerScaleMode m_scaleMode;
        private RulerAlignment m_rulerAlignment = RulerAlignment.BottomOrRight;
        private Border3DStyle m_3dBorderStyle = Border3DStyle.Etched;
        private int m_majorInterval = 100;
        private int m_numberOfDivisions = 10;
        private int m_intervalMarkFactor = 1;
        private int m_divisionMarkFactor = 5;
        private int m_middleMarkFactor = 3;
        private double m_zoomFactor = 1;
        private double m_startValue = 0;
        private bool m_mouseTrackingOn = false;
        private bool m_verticalNumbers = true;
        private bool m_actualSize = true;
        private float m_dpiX = 96;
        private Color m_markColor;
        private RulerAlignment m_numberAlignment = RulerAlignment.Middle;
        private RulerAlignment m_numberVerticalAlignment = RulerAlignment.TopOrLeft;

        #endregion

        #region Event Arguments

        public class ScaleModeChangedEventArgs : EventArgs
        {
            public RulerScaleMode Mode;

            public ScaleModeChangedEventArgs(RulerScaleMode Mode)
                : base()
            {
                this.Mode = Mode;
            }
        }

        public class HooverValueEventArgs : EventArgs
        {
            public double Value;

            public HooverValueEventArgs(double Value)
                : base()
            {
                this.Value = Value;
            }
        }

        #endregion

        #region Delegates

        public delegate void ScaleModeChangedEvent(object sender, ScaleModeChangedEventArgs e);
        public delegate void HooverValueEvent(object sender, HooverValueEventArgs e);

        #endregion

        #region Events

        public event ScaleModeChangedEvent ScaleModeChanged;
        public event HooverValueEvent HooverValue;

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container m_components = null;

        #region Ctor/Dtor

        public Ruler()
        {
            base.BackColor = System.Drawing.Color.White;
            base.ForeColor = System.Drawing.Color.Black;
            this.m_markColor = base.BackColor;

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            Graphics g = this.CreateGraphics();
            m_dpiX = g.DpiX;
            m_scaleMode = RulerScaleMode.Points;
        }

        #endregion

        #region Methods

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool PreFilterMessage(ref Message m)
        {
            if (this.m_mouseTrackingOn == false)
            {
                return false;
            }

            if (m.Msg == (int)Msg.WM_MOUSEMOVE)
            {
                int x = 0;
                int y = 0;

                // The mouse coordinate are measured in screen coordinates because thats what
                // Control.MousePosition returns. The Message LParam value is not used because
                // it returns the mouse position relative to the control the mouse is over.
                // Chalk and cheese.

                Point pointScreen = Control.MousePosition;

                // Get the origin of this control in screen coordinates so that later we can 
                // compare it against the mouse point to determine it we've hit this control.

                Point pointClientOrigin = new Point(x, y);
                pointClientOrigin = PointToScreen(pointClientOrigin);

                m_drawLine = false;
                m_inControl = false;

                HooverValueEventArgs hooverEventArgs = null;

                // Work out whether the mouse is within the Y-axis bounds of a vertical ruler or
                // within the X-axis bounds of a horizontal ruler.

                if (m_orientation == RulerOrientation.Horizontal)
                {
                    m_drawLine = (pointScreen.X >= pointClientOrigin.X) && (pointScreen.X <= pointClientOrigin.X + this.Width);
                }
                else
                {
                    m_drawLine = (pointScreen.Y >= pointClientOrigin.Y) && (pointScreen.Y <= pointClientOrigin.Y + this.Height);
                }

                // If the mouse is in valid position...
                if (m_drawLine)
                {
                    // ...workout the position of the mouse relative to the
                    x = pointScreen.X - pointClientOrigin.X;
                    y = pointScreen.Y - pointClientOrigin.Y;

                    // Determine whether the mouse is within the bounds of the control itself
                    m_inControl = (this.ClientRectangle.Contains(new Point(x, y)));

                    // Make the relative mouse position available in pixel relative to this control's origin
                    ChangeMousePosition((m_orientation == RulerOrientation.Horizontal) ? x : y);
                    hooverEventArgs = new HooverValueEventArgs(CalculateValue(m_mousePosition));
                }
                else
                {
                    ChangeMousePosition(-1);
                    hooverEventArgs = new HooverValueEventArgs(m_mousePosition);
                }

                // Paint directly by calling the OnPaint() method. This way the background is not
                // hosed by the call to Invalidate() so painting occurs without the hint of a flicker.
                PaintEventArgs e = null;
                try
                {
                    e = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle);
                    OnPaint(e);
                }
                finally
                {
                    e.Graphics.Dispose();
                }

                OnHooverValue(hooverEventArgs);
            }

            if ((m.Msg == (int)Msg.WM_MOUSELEAVE) ||
                (m.Msg == (int)Msg.WM_NCMOUSELEAVE))
            {
                m_drawLine = false;
                PaintEventArgs e = null;
                try
                {
                    e = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle);
                    OnPaint(e);
                }
                finally
                {
                    e.Graphics.Dispose();
                }
            }

            return false; // Whether or not the message is filtered.
        }

        public double PixelToScaleValue(int offset)
        {
            return this.CalculateValue(offset);
        }

        public int ScaleValueToPixel(double scaleValue)
        {
            return CalculatePixel(scaleValue);
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
            // Ruler
            //
            this.Name = "Ruler";
            this.MouseUp += new MouseEventHandler(Ruler_MouseUp);
            //base.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
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
            m_bitmap = null;
            this.Invalidate();
        }

        public override void Refresh()
        {
            base.Refresh();
            this.Invalidate();
        }

        [Description("Draws the ruler marks in the scale requested.")]
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawControl(e.Graphics);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.Visible)
            {
                if (m_mouseTrackingOn)
                {
                    Application.AddMessageFilter(this);
                }
            }
            else
            {
                // Don't change the tracking state so that the filter will be added again when the control is made visible again;
                if (m_mouseTrackingOn)
                {
                    RemoveMessageFilter();
                }
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            RemoveMessageFilter();
            m_mouseTrackingOn = false;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            RemoveMessageFilter();
            m_mouseTrackingOn = false;
        }

        private void RemoveMessageFilter()
        {
            try
            {
                if (m_mouseTrackingOn)
                {
                    Application.RemoveMessageFilter(this);
                }
            }
            catch { }
        }

        #endregion

        #region Event Handlers

        private void Ruler_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (e.Button.Equals(MouseButtons.Right))
        }

        private void Ruler_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
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

        private void Popup_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem)sender;
            m_scaleMode = (RulerScaleMode)item.Index;
            m_bitmap = null;
            Invalidate();
        }

        protected void OnHooverValue(HooverValueEventArgs e)
        {
            if (HooverValue != null)
            {
                HooverValue(this, e);
            }
        }

        protected void OnScaleModeChanged(ScaleModeChangedEventArgs e)
        {
            if (ScaleModeChanged != null)
            {
                ScaleModeChanged(this, e);
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            m_drawLine = false;
            Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            Invalidate();
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Popup");
        }

        #endregion

        #region Properties

        [DefaultValue(typeof(Border3DStyle), "Etched")]
        [Description("The border style use the Windows.Forms.Border3DStyle type")]
        [Category("Ruler")]
        public Border3DStyle BorderStyle
        {
            get
            {
                return m_3dBorderStyle;
            }
            set
            {
                m_3dBorderStyle = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("Horizontal or vertical layout")]
        [Category("Ruler")]
        public RulerOrientation Orientation
        {
            get { return m_orientation; }
            set
            {
                m_orientation = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("A value from which the ruler marking should be shown. Deafult is zero.")]
        [Category("Ruler")]
        public double StartValue
        {
            get { return m_startValue; }
            set
            {
                m_startValue = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("The scale to use")]
        [Category("Ruler")]
        public RulerScaleMode ScaleMode
        {
            get { return m_scaleMode; }
            set
            {
                RulerScaleMode oldScaleMode = m_scaleMode;
                m_scaleMode = value;

                if (m_majorInterval == DefaultMajorInterval(oldScaleMode))
                {
                    // Set the default Scale and MajorInterval value
                    m_scale = DefaultScale(m_scaleMode);
                    m_majorInterval = DefaultMajorInterval(m_scaleMode);
                }
                else
                {
                    MajorInterval = m_majorInterval;
                }

                // Use the current start value (if there is one)
                this.StartValue = this.m_startValue;

                ScaleModeChangedEventArgs e = new ScaleModeChangedEventArgs(value);
                this.OnScaleModeChanged(e);
            }
        }

        [Description("The value of the major interval. When displaying inches, 1 is a typical value. When displaying Points, 36 or 72 might good values.")]
        [Category("Ruler")]
        public int MajorInterval
        {
            get { return m_majorInterval; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The major interval value cannot be less than one");
                }

                m_majorInterval = value;
                m_scale = DefaultScale(m_scaleMode) * m_majorInterval / DefaultMajorInterval(m_scaleMode);
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("How many divisions should be shown between each major interval")]
        [Category("Ruler")]
        public int Divisions
        {
            get { return m_numberOfDivisions; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The number of divisions cannot be less than one");
                }
                m_numberOfDivisions = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("The height or with of this control multiplied by the reciprocal of this number will be used to compute the height of the interval division marks.")]
        [Category("Ruler")]
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
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("The height or with of this control multiplied by the reciprocal of this number will be used to compute the height of the non-middle division marks.")]
        [Category("Ruler")]
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
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("The height or with of this control multiplied by the reciprocal of this number will be used to compute the height of the middle division marks.")]
        [Category("Ruler")]
        public int MiddleMarkFactor
        {
            get { return m_middleMarkFactor; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The Middle Mark Factor cannot be less than one");
                }

                m_middleMarkFactor = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("The value of the current mouse position expressed in unit of the scale set (centimetres, inches, etc...)")]
        [Category("Ruler")]
        public double ScaleValue
        {
            get { return CalculateValue(m_mousePosition); }
        }

        [Description("TRUE if a line is displayed to track the current position of the mouse and events are generated as the mouse moves.")]
        [Category("Ruler")]
        public bool MouseTrackingOn
        {
            get { return m_mouseTrackingOn; }
            set
            {
                if (value == m_mouseTrackingOn) {
                    return;
                }

                if (value)
                {
                    // Tracking is being enabled so add the message filter hook
                    if (this.Visible)
                    {
                        Application.AddMessageFilter(this);
                    }
                }
                else
                {
                    // tracking is being disabled so remove the message filter hook
                    Application.RemoveMessageFilter(this);
                    ChangeMousePosition(-1);
                }

                m_mouseTrackingOn = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("The font used to display the division number")]
        public override Font  Font
        {
	        get 
	        { 
		         return base.Font;
	        }
	        set 
	        { 
		        base.Font = value;
                m_bitmap = null;
                Invalidate();
	        }
        }

        [Description("Return the mouse position as number of pixels from the top or left of the control. -1 means that the mouse is positioned before or after the control.")]
        [Category("Ruler")]
        public int MouseLocation
        {
            get { return m_mousePosition; }
        }

        [DefaultValue(typeof(Color), "ControlDarkDark")]
        [Description("The color used to lines and numbers on the ruler")]
        public override Color  ForeColor
        {
	        get 
	        { 
		         return base.ForeColor;
	        }
	        set 
	        { 
		        base.ForeColor = value;
                m_bitmap = null;
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
                m_bitmap = null;
                Invalidate();
	        }
        }

        [Description("")]
        [Category("Ruler")]
        public bool VerticalNumbers
        {
            get { return m_verticalNumbers; }
            set
            {
                m_verticalNumbers = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("A factor between 0.1 and 10 by which the displayed scale will be zoomed.")]
        [Category("Ruler")]
        public double ZoomFactor
        {
            get { return m_zoomFactor; }
            set
            {
                if ((value < 0.1) || (value > 10))
                {
                    throw new Exception("Zoom factor can be between 10% and 1000%");
                }

                if (m_zoomFactor == value) {
                    return;
                }

                m_zoomFactor = value;
                this.ScaleMode = m_scaleMode;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("True if the ruler measurement is to be based on the systems pixels per inch figure")]
        [Category("Ruler")]
        public bool ActualSize
        {
            get { return m_actualSize; }
            set
            {
                if (m_actualSize == value) {
                    return;
                }

                m_actualSize = value;
                this.ScaleMode = m_scaleMode;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("Determines how the ruler markings are displayed")]
        [Category("Ruler")]
        public RulerAlignment RulerAlignment
        {
            get { return m_rulerAlignment; }
            set
            {
                if (this.RulerAlignment == value) {
                    return;
                }

                m_rulerAlignment = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("Determines how the interval numbers are displayed")]
        [Category("Ruler")]
        public RulerAlignment NumberAlignment
        {
            get { return m_numberAlignment; }
            set
            {
                if (this.m_numberAlignment == value)
                {
                    return;
                }

                m_numberAlignment = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("Determines how the vertical interval numbers are displayed")]
        [Category("Ruler")]
        public RulerAlignment NumberVerticalAlignment
        {
            get { return m_numberVerticalAlignment; }
            set
            {
                if (this.m_numberVerticalAlignment == value)
                {
                    return;
                }

                m_numberVerticalAlignment = value;
                m_bitmap = null;
                Invalidate();
            }
        }

        [Description("Mark color")]
        [Category("Ruler")]
        public Color MarkColor
        {
            get { return m_markColor; }
            set
            {
                m_markColor = value;
                m_bitmap = null;
                Invalidate();
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
                return 0;
            }

            double value = ((double)offset - Start()) / (double)m_scale * (double)m_majorInterval;
            return value + m_startValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleValue"></param>
        /// <returns></returns>
        [Description("May not return zero even when a -ve scale number is given as the returned value needs to allow for the border thickness")]
        private int CalculatePixel(double scaleValue)
        {
            double value = scaleValue - m_startValue;
            if (value < 0)
            {
                return Start();
            }

            int offset = Convert.ToInt32(value / (double)m_majorInterval * (double)m_scale);
            return offset + Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public void RenderTrackLine(Graphics g)
        {
            if (m_mouseTrackingOn && m_drawLine)
            {
                int offset = Offset();

                // Optionally render Mouse tracking line
                switch (Orientation)
                {
                    case RulerOrientation.Horizontal:
                        Line(g, m_mousePosition, offset, m_mousePosition, this.Height - offset);
                        break;
                        
                    case RulerOrientation.Vertical:
                        Line(g, offset, m_mousePosition, this.Width - offset, m_mousePosition);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawControl(Graphics g)
        {
            if (this.Visible == false) {
                return;
            }

            if (this.Width < 1 || this.Height < 1)
            {
#if DEBUG
                System.Diagnostics.Trace.WriteLine("Minimised?");
#endif
                return;
            }

            if (m_bitmap == null)
            {
                DrawControlToBitmap();
            }

            try
            {
                // Always draw the bitmap
                g.DrawImage(m_bitmap, this.ClientRectangle);
                RenderTrackLine(g);
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
            int valueOffset = 0;
            int scaleStartValue;

            // Create a bitmap
            m_bitmap = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(m_bitmap);

            try
            {
                // Wash the background with BackColor
                g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, m_bitmap.Width, m_bitmap.Height);
                if (this.StartValue >= 0)
                {
                    scaleStartValue = Convert.ToInt32(m_startValue * m_scale / m_majorInterval);//Convert value to pixels
                }
                else
                {
                    // If the start value is -ve then assume that we are starting just above zero
                    // For example if the requested value -1.1 then make believe that the start is
                    // +0.9. We can fix up the printing of numbers later.
                    double startValue = Math.Ceiling(Math.Abs(m_startValue)) - Math.Abs(m_startValue);

                    // Compute the offset that is to be used with the start point is -ve
                    // This will be subtracted from the number calculated for the display numeral
                    scaleStartValue = Convert.ToInt32(startValue * m_scale / m_majorInterval);// Convert value to pixels
                    valueOffset = Convert.ToInt32(Math.Ceiling(Math.Abs(m_startValue)));
                }

                // Paint the lines on the image
                int scale = m_scale;
                int start = Start();
                int end = (this.Orientation == RulerOrientation.Horizontal) ? this.Width : this.Height;

                for (int j = start; j <= end; j += scale)
                {
                    int left = m_scale;
                    int offset = j + scaleStartValue;
                    scale = ((offset - start) % m_scale);

                    // If it is, draw big line
                    if (scale == 0)
                    {
                        if (m_rulerAlignment != RulerAlignment.Middle)
                        {
                            DivisionMark(g, j, m_intervalMarkFactor);
                        }

                        left = m_scale;//Set the for loop increment
                    }
                    else
                    {
                        left = m_scale - Math.Abs(scale);// Set the for loop increment
                    }

                    scale = left;
                    int value = (((offset + start) / m_scale) + 1) * m_majorInterval;

                    // Accommodate the offset if the starting point is -ve
                    value -= valueOffset;
                    DrawValue(g, value, j - start, scale);

                    int used = 0;

                    // TODO: This must be wrong when the start is negative and not a whole number
                    // Draw small lines
                    for (int i = 0; i < m_numberOfDivisions; ++i)
                    {
                        // Get the increment for the next mark
                        int x = Convert.ToInt32(Math.Round((double)(m_scale - used) / (double)(m_numberOfDivisions - i), 0));//Use a spreading algorithm rather that using expensive floating point numbers

                        // So the next mark will have used up
                        used += x;

                        if (used >= (m_scale - left))
                        {
                            x = used + j - (m_scale - left);

                            // Is it an even number and, if so, is it the middle value?
                            bool middleMark = ((m_numberOfDivisions & 0x1) == 0) & (i + 1 == m_numberOfDivisions / 2);
                            bool showMiddleMark = middleMark;
                            bool lastDivisionMark = (i + 1 == m_numberOfDivisions);
                            bool lastAlignMiddleDivisionMark = (lastDivisionMark && (m_rulerAlignment == RulerAlignment.Middle));
                            bool showDivisionMark = (middleMark == false) && (lastAlignMiddleDivisionMark == false);
                            if (showMiddleMark)
                            {
                                DivisionMark(g, x, m_middleMarkFactor);//Height of Width will be 1/3
                            }
                            else if (showDivisionMark)
                            {
                                DivisionMark(g, x, m_divisionMarkFactor);//Height or Width will be 1/5
                            }
                        }
                    }
                }

                if (m_3dBorderStyle != Border3DStyle.Flat)
                {
                    ControlPaint.DrawBorder3D(g, this.ClientRectangle, this.m_3dBorderStyle);
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

            if (this.Orientation == RulerOrientation.Horizontal)
            {
                switch (m_rulerAlignment)
                {
                    case RulerAlignment.BottomOrRight:
                        markStart = this.Height - this.Height / proportion;
                        markEnd = this.Height;
                        break;

                    case RulerAlignment.Middle:
                        markStart = (this.Height - this.Height / proportion)/2 -1;
                        markEnd = markStart + this.Height / proportion;
                        break;

                    case RulerAlignment.TopOrLeft:
                        markStart = 0;
                        markEnd = this.Height / proportion;
                        break;
                }

                Line(g, position, markStart, position, markEnd);
            }
            else
            {
                switch (m_rulerAlignment)
                {
                    case RulerAlignment.BottomOrRight:
                        markStart = this.Width - this.Width / proportion;
                        markEnd = this.Width;
                        break;

                    case RulerAlignment.Middle:
                        markStart = (this.Width - this.Width / proportion)/2 -1;
                        markEnd = markStart + this.Width / proportion;
                        break;

                    case RulerAlignment.TopOrLeft:
                        markStart = 0;
                        markEnd = this.Width / proportion;
                        break;
                }

                Line(g, markStart, position, markEnd, position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <param name="spaceAvailable"></param>
        private void DrawValue(Graphics g, int value, int position, int spaceAvailable)
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
            if (this.Orientation == RulerOrientation.Horizontal)
            {
                switch (m_numberVerticalAlignment)
                {
                    case RulerAlignment.BottomOrRight:
                        y = this.Height - 2 - (int)size.Height;
                        break;

                    case RulerAlignment.Middle:
                        y = (this.Height - (int)size.Height)/2 - 2;
                        break;

                    case RulerAlignment.TopOrLeft:
                        y = 2;
                        break;
                }

                switch (m_numberAlignment)
                {
                    case RulerAlignment.BottomOrRight:
                        x = position + spaceAvailable - (int)size.Width - 2;
                        break;

                    case RulerAlignment.Middle:
                        x = position + spaceAvailable - (int)size.Width / 2;
                        break;

                    case RulerAlignment.TopOrLeft:
                        x = position + spaceAvailable + 2;
                        break;
                }

                drawingPoint = new Point(x,y);
            }
            else
            {
                switch (m_numberVerticalAlignment)
                {
                    case RulerAlignment.BottomOrRight:
                        x = this.Width - 2 - (int)size.Width;
                        break;

                    case RulerAlignment.Middle:
                        x = (this.Width - (int)size.Width)/2 - 2;
                        break;

                    case RulerAlignment.TopOrLeft:
                        x = 2;
                        break;
                }

                switch (m_numberAlignment)
                {
                    case RulerAlignment.BottomOrRight:
                        y = position + spaceAvailable - (int)size.Height - 2;
                        break;

                    case RulerAlignment.Middle:
                        y = position + spaceAvailable - (int)size.Height / 2;
                        break;

                    case RulerAlignment.TopOrLeft:
                        y = position + spaceAvailable - (int)size.Height - 2;
                        break;
                }

                drawingPoint = new Point(x,y);
            }

            // The drawstring function is common to all operations

            g.DrawString(value.ToString(), this.Font, new SolidBrush(this.ForeColor), drawingPoint, format);
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
        /// <param name="scaleMode"></param>
        /// <returns></returns>
        private int DefaultScale(RulerScaleMode scaleMode)
        {
            int scale = 100;
            // Set scaling
            switch (scaleMode)
            {
                    // Determines the *relative* proportions of each scale
                case RulerScaleMode.Points:
                    scale = 660;//132
                    break;

                case RulerScaleMode.Pixels:
                    scale = 100;
                    break;

                case RulerScaleMode.Centimetres:
                    scale = 262;//53
                    break;
                    
                case RulerScaleMode.Inches:
                    scale = 660;//132
                    break;

                case RulerScaleMode.Millimeters:
                    scale = 27;
                    break;

                    // Points: 96
                    // Pixels: 100
                    // Centimeters: 38
                    // Inches: 96
                    // Millimeters: 4
            }

            if (scaleMode == RulerScaleMode.Pixels)
            {
                return Convert.ToInt32((double)scale * m_zoomFactor);
            }
            else
            {
                return Convert.ToInt32((double)scale * m_zoomFactor * (double)(m_actualSize ? (double)m_dpiX/(float)480 : 0.2));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleMode"></param>
        /// <returns></returns>
        private int DefaultMajorInterval(RulerScaleMode scaleMode)
        {
            int interval = 10;

            // Set scaling
            switch (scaleMode)
            {
                case RulerScaleMode.Points:
                    interval = 72;
                    break;

                case RulerScaleMode.Pixels:
                    interval = 100;
                    break;

                case RulerScaleMode.Centimetres:
                    interval = 1;
                    break;

                case RulerScaleMode.Inches:
                    interval = 1;
                    break;

                case RulerScaleMode.Millimeters:
                    interval = 1;
                    break;
            }

            return interval;
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
                case Border3DStyle.Flat:    offset = 0;break;
                case Border3DStyle.Adjust:  offset = 0;break;
                case Border3DStyle.Sunken:  offset = 2;break;
                case Border3DStyle.Bump:    offset = 2;break;
                case Border3DStyle.Etched:  offset = 2;break;
                case Border3DStyle.Raised:  offset = 2;break;
                case Border3DStyle.RaisedInner: offset = 1;break;
                case Border3DStyle.RaisedOuter: offset = 1;break;
                case Border3DStyle.SunkenInner: offset = 1;break;
                case Border3DStyle.SunkenOuter: offset = 1;break;
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
                case Border3DStyle.Flat:    start = 0;break;
                case Border3DStyle.Adjust:  start = 0;break;
                case Border3DStyle.Sunken:  start = 1;break;
                case Border3DStyle.Bump:    start = 1;break;
                case Border3DStyle.Etched:  start = 1;break;
                case Border3DStyle.Raised:  start = 1;break;
                case Border3DStyle.RaisedInner: start = 0;break;
                case Border3DStyle.RaisedOuter: start = 0;break;
                case Border3DStyle.SunkenInner: start = 0;break;
                case Border3DStyle.SunkenOuter: start = 0;break;
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
            m_oldMousePosition = m_mousePosition;
            m_mousePosition = newPosition;
        }

        #endregion
    }
}
