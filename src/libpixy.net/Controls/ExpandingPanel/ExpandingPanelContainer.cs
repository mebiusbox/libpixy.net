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
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace libpixy.net.Controls.ExpandingPanel
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpandingPanelContainer : Panel
    {
        #region Fields

        private ArrayList m_panels;
        private Container m_components;
        private Size m_panelSpacing;
        private bool m_sizePanels;
        private bool m_singleExpand;

        #endregion

        #region Properties

        [Description("パネル間の空白")]
        [Category("ExpandingPanel")]
        public Size PanelSpacing
        {
            get { return m_panelSpacing; }
            set
            {
                m_panelSpacing = value;
                this.Invalidate();
            }
        }

        [Description("所有しているパネルのサイズを調整するかどうか")]
        [Category("ExpandingPanel")]
        public bool PanelResize
        {
            get { return m_sizePanels; }
            set
            {
                m_sizePanels = value;
            }
        }

        [Description("展開できるパネルが１つだけかどうか")]
        [Category("ExpandingPanel")]
        public bool PanelSingleExpand
        {
            get { return m_singleExpand; }
            set { m_singleExpand = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public ExpandingPanelContainer()
        {
            m_components = null;
            m_panelSpacing = new Size(4, 4);
            m_sizePanels = true;
            m_singleExpand = false;
            m_panels = new ArrayList();
            InitializeComponent();
            this.BackColor = Color.FromKnownColor(KnownColor.ControlLightLight);
            this.AutoScroll = true;
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
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control.GetType() != typeof(ExpandingPanel))
            {
                MessageBox.Show("You can only add Expanding Panels to an Expanding Panel Container");
            }
            else
            {
                ExpandingPanel control = (ExpandingPanel)e.Control;
                this.m_panels.Add(control);
                control.PanelContainer = this;
                this.UpdatePanelPositions();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(ExpandingPanel))
            {
                ExpandingPanel control = (ExpandingPanel)e.Control;
                m_panels.Remove(control);
                control.PanelContainer = null;
                this.UpdatePanelPositions();
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //nop
            //base.OnPaint(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void PaintButton(PaintEventArgs e)
        {
            //nop
            //base.OnPaint(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panel"></param>
        public virtual void ExpandPanel(ExpandingPanel panel)
        {
            if (m_singleExpand)
            {
                foreach (ExpandingPanel ep in m_panels)
                {
                    if (ep != panel && ep.IsExpanded)
                    {
                        ep.LockUpdate = true;
                        ep.Collapse();
                        ep.LockUpdate = false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdatePanelPositions()
        {
            int width = m_panelSpacing.Width;
            int y = m_panelSpacing.Height + base.AutoScrollPosition.Y;
            foreach (ExpandingPanel panel in m_panels)
            {
                panel.SetBounds(width, y, 0, 0, BoundsSpecified.Location);
                if (m_sizePanels)
                {
                    panel.SetBounds(0, 0, base.ClientSize.Width - (2 * m_panelSpacing.Width), 0, BoundsSpecified.Width);
                }

                y = (y + panel.Height) + m_panelSpacing.Height;
            }

            base.Invalidate();
        }

        /// <summary>
        /// 指定のパネルを表示
        /// </summary>
        /// <param name="panel"></param>
        public virtual void EnsureVisible(ExpandingPanel panel)
        {
            if (Controls.Contains(panel))
            {
                if (ClientRectangle.Contains(panel.Bounds) == false)
                {
                    int offset = panel.Bounds.Y - AutoScrollPosition.Y;
                    AutoScrollPosition = new Point(AutoScrollPosition.X, offset);
                    Invalidate();
                }
            }
        }

        #endregion

        #region Native Method

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool LockWindowUpdate(IntPtr hwndLock);

        #endregion
    }
}
