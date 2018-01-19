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
    /// <summary>
    /// 
    /// </summary>
    public class TimelineLane : Panel
    {
        #region Private Member Variables

        private Container m_components;
        private TimelineContainer m_container;
        private Color m_shadowColor = Color.FromArgb(32,32,32);
        private Color m_lightColor = Color.FromArgb(87,87,87);
        private Color m_selectedColor = Color.FromArgb(0, 114, 188);
        private Rectangle m_allBounds;
        private Size m_sizeSpacing;
        private bool m_drag;
        private bool m_dropTarget;
        private bool m_mouseDown;
        private bool m_selected;

        public List<TimelineItem> Items;
        public List<TimelineKey> Keys;

        #endregion // Private Member Varibles

        #region Private Methods

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

            Items.Clear();
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
        /// <param name="value"></param>
        internal void SetSelected(bool value)
        {
            m_selected = value;
        }

        #endregion // Private Methods

        #region Public Properties

        [Browsable(true)]
        public TimelineContainer TimelineContainer
        {
            get { return m_container; }
            set { m_container = value; }
        }

        [Browsable(true)]
        public Color ShadowColor
        {
            get { return m_shadowColor; }
            set { m_shadowColor = value; }
        }

        [Browsable(true)]
        public Color SelectColor
        {
            get { return m_selectedColor; }
            set { m_selectedColor = value; }
        }

        [Browsable(true)]
        public Color LightColor
        {
            get { return m_lightColor; }
            set { m_lightColor = value; }
        }

        [Browsable(false)]
        public bool Selected
        {
            get { return m_selected; }
        }

        #endregion // Public Properties

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public TimelineLane()
        {
            this.Items = new List<TimelineItem>();
            this.Keys = new List<TimelineKey>();
            this.m_components = null;
            this.m_container = null;
            this.m_allBounds = new Rectangle(-1, -1, -1, -1);
            this.m_sizeSpacing = new Size(0, 0);
            this.InitializeComponent();
            this.Visible = false;
            this.m_drag = false;
            this.m_dropTarget = false;
            this.m_mouseDown = false;
            this.m_selected = false;
            this.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
            this.DoubleBuffered = true;
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(TimelineLane_DragEnter);
            this.DragLeave += new EventHandler(TimelineLane_DragLeave);
            this.DragDrop += new DragEventHandler(TimelineLane_DragDrop);
            this.DragOver += new DragEventHandler(TimelineLane_DragOver);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        #endregion // Constructor

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimelineLane_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("libpixy.net.TimelineLane"))
            {
                if (m_drag)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                TimelineLane item = (TimelineLane)e.Data.GetData("libpixy.net.TimelineLane");
                if (item != null)
                {
                    //if (item == this || item.IsChild(this, true) || IsChild(item,false))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                m_dropTarget = true;
                this.Invalidate();
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimelineLane_DragLeave(object sender, EventArgs e)
        {
            if (m_dropTarget)
            {
                m_dropTarget = false;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimelineLane_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (e.Data.GetDataPresent("libpixy.net.TimelineLane"))
            {
                m_dropTarget = false;
                this.Invalidate();
                e.Effect = DragDropEffects.Move;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimelineLane_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("libpixy.net.TimelineLane"))
            {
                if (m_drag)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                TimelineLane item = (TimelineLane)e.Data.GetData("libpixy.net.TimelineLane");
                if (item != null)
                {
                    //if (item == this || item.IsChild(this,true) || IsChild(item,false))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                m_dropTarget = true;
                this.Invalidate();
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
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
#if false
            base.OnPaint(e);

            SolidBrush brushBack;
            SolidBrush brushFore = new SolidBrush(this.ForeColor);
            Color lightColor = Color.FromArgb(87,87,87);

            if (m_dropTarget)
            {
                brushBack = new SolidBrush(Utils.ColorUtils.Blend(this.BackColor, Color.Red, 50));
                lightColor = Utils.ColorUtils.Blend(lightColor, Color.Red, 50);
            }
            else if (m_drag)
            {
                brushBack = new SolidBrush(Utils.ColorUtils.Blend(this.BackColor, Color.Yellow, 50));
                lightColor = Utils.ColorUtils.Blend(lightColor, Color.Yellow, 50);
            }
            else
            {
                brushBack = new SolidBrush(this.BackColor);
            }

            try
            {
                int h = this.Margin.Top + this.m_sizeSpacing.Height + this.Margin.Bottom;
                Rectangle bounds = new Rectangle(0, 0, this.ClientRectangle.Width, h);
                e.Graphics.FillRectangle(brushBack, bounds);

                //e.Graphics.DrawRectangle(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Right - 1, e.ClipRectangle.Height - 1);
                //e.Graphics.DrawRectangle(pen, this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Right - 1, this.ClientRectangle.Height - 1);
                using (Pen pen = new Pen(m_lineColor))
                {
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
                }
                using (Pen pen = new Pen(lightColor))
                {
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
                }
            }
            finally
            {
                brushBack.Dispose();
                brushFore.Dispose();
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_mouseDown)
            {
                m_drag = true;
                Invalidate();
                DataFormats.Format fmt = DataFormats.GetFormat("libpixy.net.TimelineLane");
                DataObject dataObj = new DataObject(fmt.Name, this);
                DoDragDrop(dataObj, DragDropEffects.All);
                m_drag = false;
                Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //MARK: デュレーションバーをつかんでいたらドラッグ開始
            //if (Contains(e.X, e.Y))
            {
                m_mouseDown = true;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (m_mouseDown)
            {
                m_mouseDown = false;
            }

            base.OnMouseUp(e);
        }

        #endregion // Event Handlers

    }
}
