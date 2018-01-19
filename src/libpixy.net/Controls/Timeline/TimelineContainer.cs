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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace libpixy.net.Controls.Timeline
{
    /// <summary>
    /// 
    /// </summary>
    public class TimelineContainer : Panel
    {
        #region Private Member Variables

        private List<TimelineLane> m_lanes;
        private TimelineLane m_selectedLane = null;
        private Container m_components;
        private int m_laneHeight;
        private int m_startTime;
        private int m_endTime;
        private int m_indicatorPosition;
        private Color m_indicatorColor;
        private double m_pixelPerTime;
        private TimelineItem m_mouseDragTarget;
        private Tool.Mouse m_mouse = new libpixy.net.Controls.Tool.Mouse();
        private Bitmap m_offscreen;
        private bool m_updateOffscreen;

        #endregion // Private Member Variables
        
        #region Private Methods

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Take private resize action here
//            m_offscreen.Dispose();
            m_offscreen = null;
            this.Invalidate();
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

            this.m_lanes.Clear();
            base.Dispose(disposing);
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
        /// <param name="g"></param>
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

            DrawToOffscreen();

            try
            {
                g.DrawImage(m_offscreen, this.ClientRectangle);
                DrawIndicator(g);
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
        private void DrawToOffscreen()
        {
            if (m_offscreen == null)
            {
                m_offscreen = new Bitmap(this.Width, this.Height);
                m_updateOffscreen = true;
            }

            if (m_updateOffscreen)
            {
                Graphics g = Graphics.FromImage(m_offscreen);

                try
                {
                    DrawBackground(g);
                    DrawLanes(g);
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="lane"></param>
        /// <param name="item"></param>
        private void DrawDurationBar(System.Drawing.Graphics g, TimelineLane lane, TimelineItem item)
        {
            Rectangle bounds = lane.Bounds;
            int x1 = ValueToPixel(item.StartTime);
            int x2 = ValueToPixel(item.EndTime);
            int y1 = bounds.Top + 2;
            int y2 = bounds.Bottom - 3;

#if false
            using (Brush brush = new SolidBrush(libpixy.net.Utils.ColorUtils.Blend(lane.BackColor, item.Color, 50)))
            {
                g.FillRectangle(brush, new Rectangle(x1, y1, x2 - x1, y2 - y1));
            }
            using (Pen pen = new Pen(libpixy.net.Utils.ColorUtils.Blend(lane.LightColor, item.Color, 50)))
            {
                g.DrawLine(pen, x1, y1, x2, y1);
            }
            using (Pen pen = new Pen(lane.ShadowColor))
            {
                g.DrawLine(pen, x1, y1, x1, y2 - 1);
                g.DrawLine(pen, x2, y1, x2, y2 - 1);
            }
#else
            Color color = libpixy.net.Utils.ColorUtils.Blend(lane.BackColor, item.Color, 70);
            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, new Rectangle(x1, y1, x2 - x1, y2 - y1));
            }
            using (Pen pen = new Pen(libpixy.net.Utils.ColorUtils.Blend(color, Color.Black, 70)))
            {
                g.DrawRectangle(pen, new Rectangle(x1, y1, x2 - x1, y2 - y1 - 1));
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="lane"></param>
        /// <param name="key"></param>
        private void DrawKey(System.Drawing.Graphics g, TimelineLane lane, TimelineKey key)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawIndicator(System.Drawing.Graphics g)
        {
            using (Pen pen = new Pen(IndicatorColor))
            {
                int x = ValueToPixel(this.IndicatorPosition);
                g.DrawLine(pen, x, 0, x, this.ClientSize.Height);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawBackground(System.Drawing.Graphics g)
        {
            using (Brush brush = new SolidBrush(base.BackColor))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawLanes(System.Drawing.Graphics g)
        {
            foreach (TimelineLane lane in m_lanes)
            {
                DrawLane(g, lane);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="lane"></param>
        private void DrawLane(System.Drawing.Graphics g, TimelineLane lane)
        {
            if (lane.Selected)
            {
                using (Brush brush = new SolidBrush(lane.SelectColor))
                {
                    g.FillRectangle(brush, lane.Bounds);
                }
            }
            else
            {
                using (Brush brush = new SolidBrush(lane.BackColor))
                {
                    g.FillRectangle(brush, lane.Bounds);
                }
            }
#if false
            using (Pen pen = new Pen(lane.LightColor))
            {
                g.DrawLine(pen, lane.Bounds.Left, lane.Bounds.Top, lane.Bounds.Right, lane.Bounds.Top);
            }
#endif

            using (Pen pen = new Pen(lane.ShadowColor))
            {
                g.DrawLine(pen, lane.Bounds.Left, lane.Bounds.Bottom - 1, lane.Bounds.Right, lane.Bounds.Bottom - 1);
            }

            foreach (TimelineItem item in lane.Items)
            {
                DrawDurationBar(g, lane, item);
            }

            foreach (TimelineKey key in lane.Keys)
            {
                DrawKey(g, lane, key);
            }
        }

        private void CalcPixel()
        {
            int w = this.Width;
            int range = this.EndTime - this.StartTime;
            m_pixelPerTime = (double)w / (double)range;
        }

        #endregion // Private Methods

        #region Public Properties

        [Browsable(true)]
        public int LaneHeight
        {
            get { return m_laneHeight; }
            set { m_laneHeight = value; }
        }

        [Browsable(true)]
        public int StartTime
        {
            get { return m_startTime; }
            set
            {
                m_startTime = value;
                CalcPixel();
            }
        }

        [Browsable(true)]
        public int EndTime
        {
            get { return m_endTime; }
            set
            {
                m_endTime = value;
                CalcPixel();
            }
        }

        public int Duration
        {
            get { return m_endTime - m_startTime; }
        }

        [Browsable(true)]
        public int IndicatorPosition
        {
            get { return m_indicatorPosition; }
            set 
            {
                m_indicatorPosition = value;
                base.Refresh();
            }
        }

        [Browsable(true)]
        public Color IndicatorColor
        {
            get { return m_indicatorColor; }
            set { m_indicatorColor = value; }
        }

        [Browsable(true)]
        public double PixelPerTime
        {
            get { return m_pixelPerTime; }
            set
            {
                m_pixelPerTime = value;
                Refresh();
            }
        }

        [Browsable(false)]
        public TimelineLane SelectedLane
        {
            get { return m_selectedLane; }
            set
            {
                if (m_selectedLane == value)
                {
                    return;
                }

                if (m_selectedLane != null)
                {
                    m_selectedLane.SetSelected(false);
                }

                m_selectedLane = value;
                if (m_selectedLane != null)
                {
                    m_selectedLane.SetSelected(true);
                }

                RaiseAfterSelectEvent(m_selectedLane);
                this.m_updateOffscreen = true;
                this.Refresh();
            }
        }

        #endregion // Public Properties

        #region Public Methods

        public override void Refresh()
        {
            m_updateOffscreen = true;
            base.Refresh();
        }

        public TimelineItem HitItem(Point location)
        {
            foreach (TimelineLane lane in m_lanes)
            {
                if (lane.Bounds.Contains(location))
                {
                    foreach (TimelineItem item in lane.Items)
                    {
                        int x1 = ValueToPixel(item.StartTime);
                        int x2 = ValueToPixel(item.EndTime);
                        Rectangle bounds = new Rectangle(x1, lane.Bounds.Top, x2 - x1, lane.Bounds.Height);
                        if (bounds.Contains(location))
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void UpdateLanePositions()
        {
            int y = base.AutoScrollPosition.Y;
            foreach (TimelineLane lane in m_lanes)
            {
                lane.SetBounds(0, y, base.ClientSize.Width, m_laneHeight, BoundsSpecified.All);
                y = y + m_laneHeight;
            }
            m_updateOffscreen = true;
            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int ValueToPixel(int value)
        {
            return Convert.ToInt32((double)(value - m_startTime) * m_pixelPerTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public int PixelToValue(int pixel)
        {
            return Convert.ToInt32((double)pixel / m_pixelPerTime) + m_startTime;
        }

        #endregion // Public Methods

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public TimelineContainer()
        {
            m_components = null;
            m_lanes = new List<TimelineLane>();
            m_laneHeight = 20;
            m_startTime = 1;
            m_endTime = 100;
            m_indicatorColor = Color.Red;
            m_indicatorPosition = 0 ;
            m_pixelPerTime = 10.0;
            m_updateOffscreen = true;
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseDown += new MouseEventHandler(TimelineContainer_MouseDown);
            this.MouseUp += new MouseEventHandler(TimelineContainer_MouseUp);
            this.MouseMove += new MouseEventHandler(TimelineContainer_MouseMove);
            this.AutoScroll = true;
        }

        #endregion // Constructor

        #region Event

        public class TimelineEventArgs : EventArgs
        {
            public TimelineLane Lane;
            public TimelineItem Item;

            public TimelineEventArgs(TimelineLane lane, TimelineItem item)
            {
                this.Lane = lane;
                this.Item = item;
            }
        }

        public delegate void TimelineItemEvent(object sender, TimelineEventArgs e);
        public event TimelineItemEvent ItemDragged;
        public event TimelineItemEvent ItemClicked;
        public event TimelineItemEvent AfterSelected;

        public void RaiseItemDragEvent(TimelineItem item)
        {
            if (this.ItemDragged != null) {
                this.ItemDragged(this, new TimelineEventArgs(null, item));
            }
        }

        public void RaiseItemClickEvent(TimelineItem item)
        {
            if (this.ItemClicked != null)
            {
                this.ItemClicked(this, new TimelineEventArgs(null, item));
            }
        }

        public void RaiseAfterSelectEvent(TimelineLane lane)
        {
            if (this.AfterSelected != null)
            {
                this.AfterSelected(this, new TimelineEventArgs(lane, null));
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimelineContainer_MouseMove(object sender, MouseEventArgs e)
        {
            m_mouse.Move(e);

            if (m_mouse.Drag.Valid)
            {
                int dx = PixelToValue(e.X) - PixelToValue(m_mouse.Drag.OldLocation.X);
                int time = Math.Abs(dx);
                if (dx < 0)
                {
                    if (m_mouseDragTarget.StartTime - time < m_startTime)
                    {
                        int duration = m_mouseDragTarget.Duration;
                        m_mouseDragTarget.StartTime = m_startTime;
                        m_mouseDragTarget.Duration = duration;
                    }
                    else
                    {
                        m_mouseDragTarget.StartTime -= time;
                        m_mouseDragTarget.EndTime -= time;
                    }

                    RaiseItemDragEvent(m_mouseDragTarget);
                    Refresh();
                }
                else
                {
                    m_mouseDragTarget.StartTime += time;
                    m_mouseDragTarget.EndTime += time;
                    RaiseItemDragEvent(m_mouseDragTarget);
                    Refresh();
                }
            }
            else if (m_mouse.Press)
            {
                TimelineItem item = HitItem(e.Location);
                if (item != null)
                {
                    m_mouse.BeginDrag();
                    m_mouseDragTarget = item;
                    RaiseItemDragEvent(m_mouseDragTarget);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimelineContainer_MouseUp(object sender, MouseEventArgs e)
        {
            m_mouse.Up(e);
            m_mouse.EndDrag();
            m_mouseDragTarget = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimelineContainer_MouseDown(object sender, MouseEventArgs e)
        {
            m_mouse.Down(e);

            TimelineItem item = HitItem(e.Location);
            if (item != null)
            {
                RaiseItemClickEvent(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control.GetType() != typeof(TimelineLane))
            {
                MessageBox.Show("You can only add group buttons to an group buttons Container");
            }
            else
            {
                TimelineLane control = (TimelineLane)e.Control;
                this.m_lanes.Add(control);
                control.TimelineContainer = this;
                this.UpdateLanePositions();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(TimelineLane))
            {
                TimelineLane control = (TimelineLane)e.Control;
                this.m_lanes.Remove(control);
                control.TimelineContainer = null;
                this.UpdateLanePositions();
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
            this.UpdateLanePositions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawControl(e.Graphics);
        }

        #endregion // Event Handlers
    }
}
