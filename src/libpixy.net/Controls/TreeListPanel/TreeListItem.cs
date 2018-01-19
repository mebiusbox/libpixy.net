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

namespace libpixy.net.Controls.TreeListPanel
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeListItem : UserControl
    {
        #region Private Member Variables

        //private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private TreeListItem m_parentItem;
        private Container m_components;
        private TreeListContainer m_container;
        private Font m_boldFont;
        private Font m_underlineFont;
        private Color m_buttonBackColor = Color.White;
        private Color m_buttonForeColor = Color.FromArgb(0, 0, 120);
        private Color m_buttonLineColor = Color.Navy;
        private Color m_lightColor = Color.FromArgb(87,87,87);
        private Color m_shadowColor = Color.Black;
        private Color m_selectedColor = Color.FromArgb(0, 114, 188);
        private Rectangle m_buttonRectangle;
        private int m_nameHeight;
        private string m_name;
        private Rectangle m_nameBounds;
        private Rectangle m_allBounds;
        private Size m_sizeSpacing;
        private Font m_nameFont;
        private bool m_expanded;
        private bool m_selected;
        private int m_nameOffset;
        private bool m_dropTarget;
        private Image m_bulletToggleOpenImage = null;
        private Image m_bulletToggleCloseImage = null;
        private Image m_iconImage = null;
        private Controls.Tool.Mouse m_mouse = new libpixy.net.Controls.Tool.Mouse();
        private List<TreeListItem> m_items;

        #endregion // Private Member Variables

        #region Public Properties

        [Browsable(true)]
        public TreeListContainer GroupContainer
        {
            get { return m_container; }
            set
            {
                m_container = value;
                foreach (TreeListItem item in m_items)
                {
                    item.GroupContainer = value;
                }
            }
        }

        [Browsable(true)]
        public Color ShadowColor
        {
            get { return m_shadowColor; }
            set { m_shadowColor = value; }
        }

        [Browsable(true)]
        public Color LightColor
        {
            get { return m_lightColor; }
            set { m_lightColor = value; }
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
        public Color SelectBackColor
        {
            get { return m_selectedColor; }
            set { m_selectedColor = value; }
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
        public int LabelHeight
        {
            get { return m_nameHeight; }
            set 
            {
                m_nameHeight = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public Image BulletToggleOpenImage
        {
            get { return m_bulletToggleOpenImage; }
            set { m_bulletToggleOpenImage = value; }
        }

        [Browsable(true)]
        public Image BulletToggleCloseImage
        {
            get { return m_bulletToggleCloseImage; }
            set { m_bulletToggleCloseImage = value; }
        }

        [Browsable(true)]
        public Image IconImage
        {
            get { return m_iconImage; }
            set { m_iconImage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeListItem ParentItem
        {
            get { return m_parentItem; }
            set { m_parentItem = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LabelOffset
        {
            get { return m_nameOffset; }
            set 
            {
                m_nameOffset = value; 
                m_nameBounds = new Rectangle(-1, -1, -1, -1);
                foreach (TreeListItem item in m_items)
                {
                    item.LabelOffset = this.LabelOffset + 20;
                }
            }
        }

        public int IndentLevel
        {
            get
            {
                int level = 0;
                TreeListItem parent = this.ParentItem;
                while (parent != null)
                {
                    level++;
                    parent = parent.ParentItem;
                }
                return level;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ItemHeight
        {
            get { return this.Margin.Vertical + m_nameHeight + m_sizeSpacing.Height * 2; }
        }

        /// <summary>
        /// 展開されているかどうか.
        /// </summary>
        public bool IsExpaneded
        {
            get { return m_expanded; }
        }

        /// <summary>
        /// 選択されているかどうか.
        /// </summary>
        public bool Selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<TreeListItem> Items
        {
            get { return m_items; }
        }

        #endregion // Public Properties

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            //logger.Debug("libpixy.net.Controls.TreeListItem: Create");

            this.Margin = new Padding(0, 0, 0, 0);
            m_components = new Container();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            //logger.Debug("libpixy.net.Controls.TreeListItem: Dispose");

            if (disposing)
            {
                if (m_bulletToggleCloseImage != null)
                {
                    m_bulletToggleCloseImage.Dispose();
                    m_bulletToggleCloseImage = null;
                }

                if (m_bulletToggleOpenImage != null)
                {
                    m_bulletToggleOpenImage.Dispose();
                    m_bulletToggleOpenImage = null;
                }

                if (m_iconImage != null)
                {
                    m_iconImage.Dispose();
                }

                Items.Clear();

                m_boldFont.Dispose();
                m_boldFont = null;
                m_nameFont = null;//m_boldFont alias

                m_underlineFont.Dispose();
                m_underlineFont = null;
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
        /// <param name="e"></param>
        protected void PaintButton(PaintEventArgs e)
        {
            if (m_bulletToggleOpenImage != null)
            {
                if (m_expanded)
                {
                    e.Graphics.DrawImage(m_bulletToggleOpenImage, m_buttonRectangle);
                }
                else
                {
                    if (m_bulletToggleCloseImage != null)
                    {
                        e.Graphics.DrawImage(m_bulletToggleCloseImage, m_buttonRectangle);
                    }
                    else
                    {
                        e.Graphics.DrawImage(m_bulletToggleOpenImage, m_buttonRectangle);
                    }
                }
            }
            else
            {
                Pen pen1 = new Pen(m_buttonForeColor);
                Pen pen2 = new Pen(m_buttonLineColor);
                SolidBrush brush = new SolidBrush(m_buttonBackColor);

                try
                {
                    e.Graphics.FillRectangle(brush, m_buttonRectangle);
                    e.Graphics.DrawRectangle(pen1, m_buttonRectangle);
                    int num = m_buttonRectangle.Left + 2;
                    int num2 = m_buttonRectangle.Right - 2;
                    int num3 = m_buttonRectangle.Top + 2;
                    int num4 = m_buttonRectangle.Bottom - 2;
                    int num5 = m_buttonRectangle.Top + (m_buttonRectangle.Height / 2);
                    int num6 = m_buttonRectangle.Left + (m_buttonRectangle.Width / 2);
                    e.Graphics.DrawLine(pen2, num, num5, num2, num5);
                    if (m_expanded == false)
                    {
                        e.Graphics.DrawLine(pen2, num6, num3, num6, num4);
                    }
                }
                finally
                {
                    pen1.Dispose();
                    pen2.Dispose();
                    brush.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ChangeOpenState()
        {
            if (m_expanded)
            {
                m_expanded = false;
                base.SetBounds(0, 0, 0, this.ItemHeight, BoundsSpecified.Height);
            }
            else
            {
                m_expanded = true;
                base.SetBounds(0, 0, 0, m_allBounds.Height, BoundsSpecified.Height);
            }

            //System.Diagnostics.Debug.WriteLine(base.Bounds.ToString());

            if (m_container != null)
            {
                m_container.UpdateGroupPositions();
                if (m_expanded)
                {
                    m_container.RaiseAfterExpandEvent(this);
                }
                else
                {
                    m_container.RaiseAfterCollapseEvent(this);
                }
            }

            base.Invalidate();
        }

        #endregion // Private Methods

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public TreeListItem()
        {
            m_items = new List<TreeListItem>();
            this.m_components = null;
            this.m_container = null;
            this.m_name = "Group";
            this.m_nameBounds = new Rectangle(-1, -1, -1, -1);
            this.m_allBounds = new Rectangle(-1, -1, -1, -1);
            this.m_buttonRectangle = new Rectangle(0, 0, 8, 8);
            this.m_expanded = true;
            this.m_selected = false;
            this.m_nameHeight = 20;
            this.m_nameOffset = 0;
            this.m_sizeSpacing = new Size(0, 0);
            this.InitializeComponent();
            this.m_boldFont = new Font("Tahoma", 8f, FontStyle.Bold);
            this.m_underlineFont = new Font("Tahoma", 8f, FontStyle.Underline | FontStyle.Bold);
            this.m_nameFont = this.m_boldFont;
            this.m_dropTarget = false;
            this.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
            this.DoubleBuffered = true;
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(TreeListItem_DragEnter);
            this.DragLeave += new EventHandler(TreeListItem_DragLeave);
            this.DragDrop += new DragEventHandler(TreeListItem_DragDrop);
            this.DragOver += new DragEventHandler(TreeListItem_DragOver);
            this.MouseClick += new MouseEventHandler(TreeListItem_MouseClick);
        }

        #endregion // Constructor

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsChild(TreeListItem item, bool recursive)
        {
            foreach (TreeListItem tmp in m_items)
            {
                if (tmp == item)
                {
                    return true;
                }
                else
                {
                    if (recursive && tmp.IsChild(item, true))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateButtonPositions()
        {
            //System.Diagnostics.Debug.WriteLine("ToolboxGroup.UpdateButtonPositions");

            m_allBounds = new Rectangle(0, 0, this.ClientSize.Width - this.Margin.Horizontal, this.ItemHeight);

            int x = this.Margin.Left + this.m_sizeSpacing.Width;
            int y = this.ItemHeight;
            int w = this.ClientSize.Width - (this.Margin.Horizontal + this.m_sizeSpacing.Width * 2);

            foreach (TreeListItem btn in Items)
            {
                btn.UpdateButtonPositions();
                int h = btn.Height + this.m_sizeSpacing.Height * 2;
                m_allBounds.Height += h;
                btn.Bounds = new Rectangle(x, y, w, h);
                y += h;
            }

            if (m_expanded)
            {
                base.SetBounds(0, 0, 0, m_allBounds.Height, BoundsSpecified.Height);
            }
            else
            {
                base.SetBounds(0, 0, 0, this.ItemHeight, BoundsSpecified.Height);
            }
        }

        public void Expand()
        {
            m_expanded = true;
        }

        public void Collapse(bool ignoreChild)
        {
            m_expanded = false;

            if (ignoreChild == false)
            {
                foreach (TreeListItem item in Items)
                {
                    item.Collapse(ignoreChild);
                }
            }
        }

        public void ClearItems()
        {
            List<TreeListItem> items = new List<TreeListItem>(this.Items);
            foreach (TreeListItem item in items)
            {
                this.Controls.Remove(item);
            }

            items = null;
            this.m_items.Clear();
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreeListItem_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("libpixy.net.TreeListItem"))
            {
                if (m_mouse.Drag.Valid)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                TreeListItem item = (TreeListItem)e.Data.GetData("libpixy.net.TreeListItem");
                if (item != null)
                {
                    if (item == this || item.IsChild(this, true) || IsChild(item,false))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                m_dropTarget = true;
                Refresh();
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
        void TreeListItem_DragLeave(object sender, EventArgs e)
        {
            if (m_dropTarget)
            {
                m_dropTarget = false;
                Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreeListItem_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (e.Data.GetDataPresent("libpixy.net.TreeListItem"))
            {
                m_dropTarget = false;
                Refresh();
                e.Effect = DragDropEffects.Move;
                m_container.RaiseItemDropEvent(this, (TreeListItem)e.Data.GetData("libpixy.net.TreeListItem"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreeListItem_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("libpixy.net.TreeListItem"))
            {
                if (m_mouse.Drag.Valid)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                TreeListItem item = (TreeListItem)e.Data.GetData("libpixy.net.TreeListItem");
                if (item != null)
                {
                    if (item == this || item.IsChild(this,true) || IsChild(item,false))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                m_dropTarget = true;
                Refresh();
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
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control.GetType() == typeof(TreeListItem) || e.Control.GetType().IsSubclassOf(typeof(TreeListItem)))
            {
                TreeListItem control = (TreeListItem)e.Control;
                control.ParentItem = this;
                control.LabelOffset = this.LabelOffset + 20;
                //control.LabelOffset = 80 + control.IndentLevel * 20;
                //System.Diagnostics.Debug.WriteLine(string.Format("indent={0}", control.IndentLevel));
                control.GroupContainer = this.GroupContainer;
                m_items.Add(control);
                this.UpdateButtonPositions();
                this.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(TreeListItem) || e.Control.GetType().IsSubclassOf(typeof(TreeListItem)))
            {
                TreeListItem control = (TreeListItem)e.Control;
                control.ParentItem = null;
                control.GroupContainer = null;
                m_items.Remove(control);
                this.UpdateButtonPositions();
                this.Refresh();
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
            this.UpdateButtonPositions();
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

        public void CalcLabelBounds(Graphics g)
        {
            if (m_nameBounds.Left == -1)
            {
                if (m_bulletToggleOpenImage != null)
                {
                    m_buttonRectangle = new Rectangle(0, 0, m_bulletToggleOpenImage.Width, m_bulletToggleOpenImage.Height);
                }
                else
                {
                    m_buttonRectangle = new Rectangle(0, 0, 8, 8);
                }

                m_buttonRectangle.Offset(m_nameOffset + 4, Utils.DrawUtils.Offset(m_buttonRectangle.Height, this.ItemHeight) - 1);

                m_nameBounds.X = m_buttonRectangle.Right + 3;
                SizeF ef = g.MeasureString(m_name, m_boldFont);
                m_nameBounds.Width = (int)ef.Width;
                m_nameBounds.Height = (int)ef.Height;
                m_nameBounds.Y = Utils.DrawUtils.Offset(m_nameBounds.Height, this.ItemHeight) - 1;
            }
        }

        public Rectangle GetLabelBounds()
        {
            int namePos = m_nameBounds.Left;
            if (m_iconImage != null)
            {
                namePos += m_iconImage.Width + 2;
            }

            return new Rectangle(namePos, m_nameBounds.Top, m_nameBounds.Width, m_nameBounds.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            SolidBrush brushBack;
            SolidBrush brushFore = new SolidBrush(this.ForeColor);
            Color lightColor = m_lightColor;

            if (m_dropTarget)
            {
                brushBack = new SolidBrush(libpixy.net.Utils.ColorUtils.Blend(this.BackColor, Color.Red, 50));
                lightColor = libpixy.net.Utils.ColorUtils.Blend(lightColor, Color.Red, 50);
            }
            else if (m_mouse.Drag.Valid)
            {
                brushBack = new SolidBrush(libpixy.net.Utils.ColorUtils.Blend(this.BackColor, Color.Yellow, 50));
                lightColor = libpixy.net.Utils.ColorUtils.Blend(lightColor, Color.Yellow, 50);
            }
            else if (m_selected)
            {
                brushBack = new SolidBrush(this.m_selectedColor);
                lightColor = libpixy.net.Utils.ColorUtils.Blend(lightColor, this.m_selectedColor, 50);
            }
            else
            {
                brushBack = new SolidBrush(this.BackColor);
            }
            
            try
            {
                int h = this.Margin.Top + this.m_sizeSpacing.Height + this.m_nameHeight + this.Margin.Bottom;
                Rectangle bounds = new Rectangle(0, 0, this.ClientRectangle.Width, h);
                e.Graphics.FillRectangle(brushBack, bounds);

                //e.Graphics.DrawRectangle(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Right - 1, e.ClipRectangle.Height - 1);
                //e.Graphics.DrawRectangle(pen, this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Right - 1, this.ClientRectangle.Height - 1);
                using (Pen pen = new Pen(m_shadowColor))
                {
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
                }
                using (Pen pen = new Pen(lightColor))
                {
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
                }

                if (m_nameBounds.Left == -1)
                {
                    if (m_bulletToggleOpenImage != null)
                    {
                        m_buttonRectangle = new Rectangle(0, 0, m_bulletToggleOpenImage.Width, m_bulletToggleOpenImage.Height);
                    }
                    else
                    {
                        m_buttonRectangle = new Rectangle(0, 0, 8, 8);
                    }

                    m_buttonRectangle.Offset(m_nameOffset+4, Utils.DrawUtils.Offset(m_buttonRectangle.Height, this.ItemHeight) - 1);

                    m_nameBounds.X = m_buttonRectangle.Right + 3;
                    SizeF ef = e.Graphics.MeasureString(m_name, m_boldFont);
                    m_nameBounds.Width = (int)ef.Width;
                    m_nameBounds.Height = (int)ef.Height;
                    m_nameBounds.Y = Utils.DrawUtils.Offset(m_nameBounds.Height, this.ItemHeight) - 1;
                }

                int namePos = m_nameBounds.Left;
                if (m_iconImage != null)
                {
                    int iconY = Utils.DrawUtils.Offset(m_iconImage.Height, this.ItemHeight) - 1;
                    e.Graphics.DrawImage(m_iconImage, new Point(namePos, iconY));
                    namePos += m_iconImage.Width + 2;
                }

                e.Graphics.DrawString(m_name, m_nameFont, brushFore, new PointF((float)namePos, (float)m_nameBounds.Top));
            }
            finally
            {
                brushBack.Dispose();
                brushFore.Dispose();
            }

            if (m_items.Count > 0)
            {
                this.PaintButton(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            m_mouse.Move(e);

            if (m_mouse.Slide)
            {
                m_mouse.BeginDrag();
                Refresh();
                DataFormats.Format fmt = DataFormats.GetFormat("libpixy.net.TreeListItem");
                DataObject dataObj = new DataObject(fmt.Name, this);
                DoDragDrop(dataObj, DragDropEffects.All);
                m_mouse.EndDrag();
                Refresh();
            }

            if (m_mouse.Drag.Valid == false)
            {
                if (m_nameBounds.X != -1)
                {
                    Font nameFont = m_nameFont;
                    if (m_nameBounds.Contains(e.X, e.Y))
                    {
                        m_nameFont = m_underlineFont;
                        if (this.Cursor != Cursors.Hand)
                        {
                            this.Cursor = Cursors.Hand;
                        }
                    }
                    else
                    {
                        m_nameFont = m_boldFont;
                        if (this.Cursor != Cursors.Default)
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }

                    if (m_nameFont != nameFont)
                    {
                        base.Invalidate();
                    }
                }
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_nameFont != m_boldFont)
            {
                m_nameFont = m_boldFont;
                base.Invalidate();
            }

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_mouse.Down(e);
            m_mouse.Press = false;

            if (m_buttonRectangle.Contains(e.X, e.Y))
            {
                this.ChangeOpenState();
            }
            else if (m_nameBounds.Contains(e.X, e.Y))
            {
                m_mouse.Press = true;
            }
            else
            {
                m_mouse.Click = true;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_mouse.Up(e);
            m_mouse.Click = false;
            base.OnMouseUp(e);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreeListItem_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_mouse.Click && m_container != null)
            {
                m_container.SelectedItem = this;
            }
        }

        #endregion // EventHandlers
    }
}
