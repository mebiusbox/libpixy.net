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

namespace libpixy.net.Controls.EdgePanel
{
    /// <summary>
    /// 
    /// </summary>
    public class EdgePanelContainer : Panel
    {
        #region Private Member Variables

        private ArrayList m_panels;
        private Container m_components;
        private Size m_panelSpacing;
        private bool m_sizePanels;
        private bool m_singleExpand;
        private bool m_lockPanelLayout;
        private int m_panelWidth;
        private PanelLayoutType m_panelLayout = PanelLayoutType.Vertical;

        #endregion // Private Member Variables

        #region Public Properties

        public enum PanelLayoutType
        {
            Vertical,
            Tile
        }

        [Description("パネルのレイアウト")]
        [Category("EdgePanel")]
        public PanelLayoutType PanelLayout
        {
            get { return m_panelLayout; }
            set
            {
                m_panelLayout = value;
                RepositionControls();
            }
        }

        [Description("パネル間の空白")]
        [Category("EdgePanel")]
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
        [Category("EdgePanel")]
        public bool PanelResize
        {
            get { return m_sizePanels; }
            set
            {
                m_sizePanels = value;
            }
        }

        [Description("展開できるパネルが１つだけかどうか")]
        [Category("EdgePanel")]
        public bool PanelSingleExpand
        {
            get { return m_singleExpand; }
            set { m_singleExpand = value; }
        }

        [Description("パネルの横幅")]
        [Category("EdgePanel")]
        public int PanelWidth
        {
            get { return m_panelWidth; }
            set { m_panelWidth = value; }
        }

        #endregion // Public Properties

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public EdgePanelContainer()
        {
            m_components = null;
            m_panelSpacing = new Size(4, 4);
            m_sizePanels = true;
            m_singleExpand = false;
            m_panelWidth = 300;
            m_lockPanelLayout = false;
            m_panels = new ArrayList();
            InitializeComponent();
            this.BackColor = Color.FromKnownColor(KnownColor.ControlLightLight);
            this.AutoScroll = true;
        }

        #endregion // Constructor

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
            if (e.Control.GetType() == typeof(EdgePanel) ||
                e.Control.GetType().IsSubclassOf(typeof(EdgePanel)))
            {
                EdgePanel control = (EdgePanel)e.Control;
                this.m_panels.Add(control);
                control.PanelContainer = this;
                //this.RepositionControls();
            }
            else
            {
                MessageBox.Show("You can only add Expanding Panels to an Expanding Panel Container");
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(EdgePanel))
            {
                EdgePanel control = (EdgePanel)e.Control;
                m_panels.Remove(control);
                control.PanelContainer = null;
                this.RepositionControls();
            }
            base.OnControlRemoved(e);
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            // フォーカスが移ると AutuScrollPosition が (0,0) にリセットされる
            // 回避するために ScrollToControl をオーバーライドする.
            //return base.ScrollToControl(activeControl);
            return this.AutoScrollPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levent"></param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            RepositionControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //nop
            base.OnPaint(e);
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

        #endregion // Private Methods

        #region Public Methods

        public void SuspendPanelLayout()
        {
            m_lockPanelLayout = true;
        }

        public void ResumePanelLayout()
        {
            m_lockPanelLayout = false;
        }

        public void PerformPanelLayout()
        {
            RepositionControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panel"></param>
        public virtual void ExpandPanel(EdgePanel panel)
        {
            if (m_singleExpand)
            {
                foreach (EdgePanel ep in m_panels)
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
        public virtual void RepositionControls()
        {
            if (m_lockPanelLayout)
            {
                return;
            }

            int y = base.AutoScrollPosition.Y;

            if (m_panelLayout == PanelLayoutType.Vertical)
            {
                if (this.DesignMode)
                {
                    y += 20;//デザイン用に余白
                }

                foreach (EdgePanel panel in m_panels)
                {
                    panel.SetBounds(0, y, 0, 0, BoundsSpecified.Location);
                    if (m_sizePanels)
                    {
                        panel.SetBounds(0, 0, base.ClientSize.Width, 0, BoundsSpecified.Width);
                    }

                    y += panel.Height;
                }
            }
            else if (m_panelLayout == PanelLayoutType.Tile)
            {
                int x = base.AutoScrollPosition.X;
                foreach (EdgePanel panel in m_panels)
                {
                    if (y + panel.Height > this.ClientSize.Height)
                    {
                        y = base.AutoScrollPosition.Y;
                        x += m_panelWidth;
                    }

                    panel.SetBounds(x, y, 0, 0, BoundsSpecified.Location);
                    panel.SetBounds(0, 0, m_panelWidth, 0, BoundsSpecified.Width);
                    y += panel.Height;
                }
            }

            //base.SetBounds(base.Bounds.X, base.Bounds.Y, base.Bounds.Width, y, BoundsSpecified.All);
            base.Invalidate();
        }

        /// <summary>
        /// 指定のパネルを表示
        /// </summary>
        /// <param name="panel"></param>
        public virtual void EnsureVisible(EdgePanel panel)
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

        #endregion // Public Methods
    }
}
