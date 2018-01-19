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

namespace libpixy.net.Controls.ToolboxPanel
{
    /// <summary>
    /// 
    /// </summary>
    public class ToolboxContainer : Panel
    {
        #region Fields

        private List<ToolboxGroup> m_items;
        private Container m_components;
        private Size m_panelSpacing;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        public Size PanelSpacing
        {
            get { return m_panelSpacing; }
            set 
            {
                m_panelSpacing = value;
                this.UpdateGroupPositions();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public ToolboxContainer()
        {
            m_components = null;
            m_panelSpacing = new Size(4, 4);
            m_items = new List<ToolboxGroup>();
            InitializeComponent();
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

            this.m_items.Clear();
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

            if (e.Control.GetType() != typeof(ToolboxGroup))
            {
                MessageBox.Show("You can only add group buttons to an group buttons Container");
            }
            else
            {
                ToolboxGroup control = (ToolboxGroup)e.Control;
                this.m_items.Add(control);
                control.GroupContainer = this;
                this.UpdateGroupPositions();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(ToolboxGroup))
            {
                ToolboxGroup control = (ToolboxGroup)e.Control;
                this.m_items.Remove(control);
                control.GroupContainer = null;
                this.UpdateGroupPositions();
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
            this.UpdateGroupPositions();
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
        public void UpdateGroupPositions()
        {
            int width = m_panelSpacing.Width;
            int y = m_panelSpacing.Height + base.AutoScrollPosition.Y;
            foreach (ToolboxGroup panel in m_items)
            {
                panel.SetBounds(width, y, 0, 0, BoundsSpecified.Location);
                panel.SetBounds(0, 0, base.ClientSize.Width - (2 * m_panelSpacing.Width), 0, BoundsSpecified.Width);
                panel.UpdateButtonPositions();
                y = (y + panel.Height) + m_panelSpacing.Height;
            }
        }

        #endregion
    }
}
