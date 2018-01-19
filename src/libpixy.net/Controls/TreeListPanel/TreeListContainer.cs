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

namespace libpixy.net.Controls.TreeListPanel
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeListContainer : Panel
    {
        #region Private Member Variables

        //private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private List<TreeListItem> m_items;
        private Container m_components;
        private Size m_panelSpacing;
        private TreeListItem m_selectedItem = null;

        #endregion // Private Member Variables

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            //logger.Debug("libpixy.net.Controls.TreeListPanel.TreeListContainer: Dispose");

            if (disposing)
            {
                this.m_items.Clear();
                this.m_selectedItem = null;
            }

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
            this.MouseClick += new MouseEventHandler(TreeListContainer_MouseClick);
            this.DragDrop += new DragEventHandler(TreeListContainer_DragDrop);
            this.DragOver += new DragEventHandler(TreeListContainer_DragOver);
            this.DragEnter += new DragEventHandler(TreeListContainer_DragEnter);
            this.DragLeave += new EventHandler(TreeListContainer_DragLeave);
        }

        void TreeListContainer_DragEnter(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("libpixy.net.Controls.TreeListPanel.TreeListContainer: DragEnter");
            //throw new NotImplementedException();

            if (e.Data.GetDataPresent("libpixy.net.TreeListItem"))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void TreeListContainer_DragLeave(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("libpixy.net.Controls.TreeListPanel.TreeListContainer: DragLeave");
            //throw new NotImplementedException();
        }

        void TreeListContainer_DragOver(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("libpixy.net.Controls.TreeListPanel.TreeListContainer: DragOver");
            //throw new NotImplementedException();

            if (e.Data.GetDataPresent("libpixy.net.TreeListItem"))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void TreeListContainer_DragDrop(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("libpixy.net.Controls.TreeListPanel.TreeListContainer: DragDrop");
            //throw new NotImplementedException();

            if (e.Data.GetDataPresent("libpixy.net.TreeListItem"))
            {
                e.Effect = DragDropEffects.Move;
                RaiseItemDropEvent(null, (TreeListItem)e.Data.GetData("libpixy.net.TreeListItem"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void  TreeListContainer_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_selectedItem != null)
            {
                m_selectedItem.Selected = false;
                m_selectedItem = null;
                RaiseAfterSelectEvent(m_selectedItem);
                this.Refresh();
            }
        }

        #endregion // Private Methods

        #region Public Properties

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

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public TreeListItem SelectedItem
        {
            get { return m_selectedItem; }
            set
            {
                if (m_selectedItem != null)
                {
                    m_selectedItem.Selected = false;
                }

                m_selectedItem = value;

                if (m_selectedItem != null)
                {
                    m_selectedItem.Selected = true;
                }

                RaiseAfterSelectEvent(m_selectedItem);

                this.Refresh();
            }
        }

        #endregion // Public Properties

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void UpdateGroupPositions()
        {
            int width = m_panelSpacing.Width;
            int y = m_panelSpacing.Height + base.AutoScrollPosition.Y;
            foreach (TreeListItem panel in m_items)
            {
                panel.SetBounds(width, y, 0, 0, BoundsSpecified.Location);
                panel.SetBounds(0, 0, base.ClientSize.Width - (2 * m_panelSpacing.Width), 0, BoundsSpecified.Width);
                panel.UpdateButtonPositions();
                y = (y + panel.Height) + m_panelSpacing.Height;
            }
        }

        #endregion // Public Methods

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public TreeListContainer()
        {
            m_components = null;
            m_panelSpacing = new Size(4, 4);
            m_items = new List<TreeListItem>();
            InitializeComponent();
            this.AutoScroll = true;
        }

        #endregion // Constructor

        #region Event

        public class TreeListEventArgs
        {
            public TreeListItem Item;

            public TreeListEventArgs(TreeListItem item)
            {
                this.Item = item;
            }
        }

        public delegate void TreeListEvent(object sender, TreeListEventArgs e);
        public event TreeListEvent AfterSelect;
        public event TreeListEvent AfterExpand;
        public event TreeListEvent AfterCollapse;

        public void RaiseAfterSelectEvent(TreeListItem item)
        {
            if (this.AfterSelect != null)
            {
                this.AfterSelect(this, new TreeListEventArgs(item));
            }
        }

        public void RaiseAfterExpandEvent(TreeListItem item)
        {
            if (this.AfterExpand != null)
            {
                this.AfterExpand(this, new TreeListEventArgs(item));
            }
        }

        public void RaiseAfterCollapseEvent(TreeListItem item)
        {
            if (this.AfterCollapse != null)
            {
                this.AfterCollapse(this, new TreeListEventArgs(item));
            }
        }

        public class ItemDropEventArgs
        {
            public TreeListItem Target;
            public TreeListItem Source;

            public ItemDropEventArgs(TreeListItem target, TreeListItem source)
            {
                this.Target = target;
                this.Source = source;
            }
        }

        public delegate void ItemDroppedEvent(object sender, ItemDropEventArgs e);
        public event ItemDroppedEvent ItemDropped;

        public void RaiseItemDropEvent(TreeListItem target, TreeListItem source)
        {
            if (this.ItemDropped != null)
            {
                this.ItemDropped(this, new ItemDropEventArgs(target, source));
            }
        }

        #endregion // Event

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            //logger.Debug("libpixy.net.Controls.TreeListPanel.TreeListContainer: Added");

            base.OnControlAdded(e);

            if (e.Control.GetType() == typeof(TreeListItem) || e.Control.GetType().IsSubclassOf(typeof(TreeListItem)))
            {
                TreeListItem control = (TreeListItem)e.Control;
                this.m_items.Add(control);
                control.GroupContainer = this;
                this.UpdateGroupPositions();
            }
            else
            {
                MessageBox.Show("You can only add group buttons to an group buttons Container");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            //logger.Debug("libpixy.net.Controls.TreeListPanel.TreeListContainer: Removed");

            if (e.Control.GetType() == typeof(TreeListItem) || e.Control.GetType().IsSubclassOf(typeof(TreeListItem)))
            {
                TreeListItem control = (TreeListItem)e.Control;
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

        #endregion // Event Handlers
    }
}
