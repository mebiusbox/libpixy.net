using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace libpixy.net.Controls.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Toolbox : UserControl
    {
        #region Field

        private Boolean m_drag = false;
        private Color m_groupBackColor = Color.FromArgb(0, 128, 255);
        private Color m_groupLineColor = Color.FromArgb(0, 128, 255);
        private Color m_groupForeColor = Color.FromArgb(255, 255, 255);
        private Color m_buttonBackColor = Color.FromKnownColor(KnownColor.Control);
        private Color m_buttonForeColor = Color.FromArgb(0, 0, 0);
        private Color m_buttonLineColor = Color.FromKnownColor(KnownColor.Control);
        private Color m_focusBackColor = Color.FromArgb(193, 210, 238);
        private Color m_focusForeColor = Color.FromArgb(0, 0, 0);
        private Color m_focusLineColor = Color.FromArgb(49, 106, 197);
        private Color m_selectBackColor = Color.FromArgb(225, 230, 232);
        private Color m_selectForeColor = Color.FromArgb(0, 0, 0);
        private Color m_selectLineColor = Color.FromArgb(49, 106, 197);
        private ImageList m_imageList;
        private Font m_boldFont;
        private Font m_underlineFont;
        private IToolboxItem m_selectItem = null;
        private IToolboxItem m_focusItem = null;
        private bool m_rounded = false;

        #endregion

        #region Properties

        public System.Collections.Generic.List<ToolboxGroup> Groups;

        [Browsable(true)]
        public ImageList ImageList
        {
            get { return m_imageList; }
            set { m_imageList = value; }
        }

        [Browsable(true)]
        public Font GroupFont
        {
            get { return m_boldFont; }
            set { m_boldFont = value; }
        }

        [Browsable(true)]
        public IToolboxItem SelectItem
        {
            get { return m_selectItem; }
            set { m_selectItem = value; }
        }

        [Browsable(true)]
        public IToolboxItem FocusItem
        {
            get { return m_focusItem; }
            set { m_focusItem = value; }
        }

        [Browsable(true)]
        public Color GroupBackColor
        {
            get { return m_groupBackColor; }
            set { m_groupBackColor = value; }
        }

        [Browsable(true)]
        public Color GroupForeColor
        {
            get { return m_groupForeColor; }
            set { m_groupForeColor = value; }
        }

        [Browsable(true)]
        public Color GroupLineColor
        {
            get { return m_groupLineColor; }
            set { m_groupLineColor = value; }
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
        public Color FocusBackColor
        {
            get { return m_focusBackColor; }
            set { m_focusBackColor = value; }
        }

        [Browsable(true)]
        public Color FocusForeColor
        {
            get { return m_focusForeColor; }
            set { m_focusForeColor = value; }
        }

        [Browsable(true)]
        public Color FocusLineColor
        {
            get { return m_focusLineColor; }
            set { m_focusLineColor = value; }
        }

        [Browsable(true)]
        public Color SelectBackColor
        {
            get { return m_selectBackColor; }
            set { m_selectBackColor = value; }
        }

        [Browsable(true)]
        public Color SelectForeColor
        {
            get { return m_selectForeColor; }
            set { m_selectForeColor = value; }
        }

        [Browsable(true)]
        public Color SelectLineColor
        {
            get { return m_selectLineColor; }
            set { m_selectLineColor = value; }
        }

        [Browsable(true)]
        public bool Rounded
        {
            get { return m_rounded; }
            set { m_rounded = value; }
        }

        #endregion

        #region Constants

        public const int MGN_X = 2;
        public const int MGN_Y = 2;
        public const int PAD_X = 2;
        public const int PAD_Y = 2;
        public const int B_OFS_X = 16;
        public const int B_OFS_Y = 0;
        public const int LINE_H = 24;

        #endregion

        #region EventHandler

        public delegate void ToolboxEventHandler(Object sender, ToolboxEventArgs args);
        public event ToolboxEventHandler ItemSelected;
        public event ToolboxEventHandler ItemDrag;

        #endregion

        #region Constructor / Destructor

        /// <summary>
        /// 
        /// </summary>
        public Toolbox()
        {
            InitializeComponent();
            Groups = new List<ToolboxGroup>();
            m_boldFont = new Font("Verdana", 8, FontStyle.Bold);
            m_underlineFont = new Font("Verdana", 8, FontStyle.Bold|FontStyle.Underline);
            m_imageList = new ImageList();
        }

        /// <summary>
        /// 
        /// </summary>
        ~Toolbox()
        {
            this.m_boldFont.Dispose();
            this.m_underlineFont.Dispose();
        }

        #endregion

        #region Misc

        /// <summary>
        /// 
        /// </summary>
        public void RecalcLayout()
        {
            Graphics gfx = this.CreateGraphics();

            int ofsX = 0;
            int ofsY = 0;
            foreach (ToolboxGroup g in Groups)
            {
                Rectangle rc = new Rectangle();
                rc.X = ofsX + MGN_X;
                rc.Y = ofsY + MGN_Y;
                rc.Width = this.ClientSize.Width - (rc.X + MGN_X);
                rc.Height = LINE_H;
                g.Bounds = rc;

                if (g.NameBounds.Left == -1)
                {
                    SizeF ef = gfx.MeasureString(g.Text, m_boldFont);
                    g.NameBounds = new Rectangle(
                        g.GetExpandBoxRect().Right + 3,
                        g.GetExpandBoxRect().Top - 2,
                        (int)ef.Width,
                        (int)ef.Height);
                }

                ofsY += rc.Height + MGN_Y;

                if (g.Expand)
                {
                    foreach (ToolboxButton btn in g.Buttons)
                    {
                        Rectangle rcBtn = new Rectangle();
                        rcBtn.X = ofsX + MGN_X;// + B_OFS_X;
                        rcBtn.Y = ofsY + MGN_Y;// + B_OFS_Y;
                        rcBtn.Width = this.ClientSize.Width - (rcBtn.X + MGN_X);
                        rcBtn.Height = LINE_H;
                        btn.Bounds = rcBtn;
                        ofsY += rcBtn.Height + MGN_Y;
                    }
                }
                else
                {
                    Rectangle rcZero = new Rectangle(0,0,0,0);
                    foreach (ToolboxButton btn in g.Buttons)
                    {
                        btn.Bounds = rcZero;
                    }
                }
            }

            if (this.Parent != null)
            {
                Size szClient = this.Parent.ClientSize;
                Size content = new Size();
                content.Width = szClient.Width;
                content.Height = (szClient.Height < ofsY) ? ofsY : szClient.Height;
                this.Size = content;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="ofsX"></param>
        /// <param name="ofsY"></param>
        private void AdjustRectangle(ref Rectangle rc, int ofsX, int ofsY)
        {
            rc.Width = rc.Width - ofsX;
            rc.Height = rc.Height - ofsY;
            rc.X += ofsX;
            rc.Y += ofsY;
        }

        #endregion

        #region Draw

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toolbox_Paint(object sender, PaintEventArgs e)
        {
            foreach (ToolboxGroup g in Groups)
            {
                DrawToolboxGroup(e.Graphics, g);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="g"></param>
        private void DrawToolboxGroup(Graphics gfx, ToolboxGroup g)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            Rectangle rc = new Rectangle();
            rc = g.Bounds;

            // Draw background

            using (SolidBrush brush = new SolidBrush(this.GroupBackColor))
            {
                gfx.FillRectangle(brush, rc);
            }   

            using (Pen pen = new Pen(this.GroupLineColor, 1))
            {
                gfx.DrawRectangle(pen, rc);
            }

            // Draw expand box

            rc = g.GetExpandBoxRect();
            DrawExpandBox(rc, g.Expand, gfx);

            // Draw text

            rc = g.GetLabelRect();

            using (SolidBrush brush = new SolidBrush(this.GroupForeColor))
            {
                if (g.Focus)
                {
                    gfx.DrawString(g.Text, m_underlineFont, brush, rc, sf);
                }
                else
                {
                    gfx.DrawString(g.Text, m_boldFont, brush, rc, sf);
                }
            }

            if (g.Expand)
            {
                foreach (ToolboxButton btn in g.Buttons)
                {
                    DrawToolboxButton(gfx, btn);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="btn"></param>
        private void DrawToolboxButton(Graphics gfx, ToolboxButton btn)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            Rectangle rc = new Rectangle();
            rc = btn.Bounds;

            Color backColor = this.ButtonBackColor;
            Color foreColor = this.ButtonForeColor;
            Color lineColor = this.ButtonLineColor;

            if (btn.Focus)
            {
                backColor = this.FocusBackColor;
                foreColor = this.FocusForeColor;
                lineColor = this.FocusLineColor;
            }
            else if (btn.Select)
            {
                backColor = this.SelectBackColor;
                foreColor = this.SelectForeColor;
                lineColor = this.SelectLineColor;
            }

            if (this.Rounded)
            {
                Rectangle rcInner = rc;
                libpixy.net.Controls.Diagram.Draw.FillRoundedRectangle(gfx, rcInner, 10, backColor);
                libpixy.net.Controls.Diagram.Draw.DrawRoundedRectangle(gfx, rcInner, 10, GetMiddleColor(backColor, this.BackColor), 2);
                rcInner.Inflate(-1, -1);
                libpixy.net.Controls.Diagram.Draw.DrawRoundedRectangle(gfx, rcInner, 10, lineColor, 1);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    using (Pen pen = new Pen(lineColor))
                    {
                        gfx.FillRectangle(brush, rc);
                        gfx.DrawRectangle(pen, rc);
                    }
                }
            }

            rc.Inflate(-PAD_X, -PAD_Y);

            if (btn.ImageIndex >= 0 && btn.ImageIndex < ImageList.Images.Count)
            {
                Size sz = ImageList.Images[btn.ImageIndex].Size;
                int x = rc.X;
                int y = (rc.Y + rc.Height / 2) - (sz.Height / 2);
                ImageList.Draw(gfx, x, y, btn.ImageIndex);
                AdjustRectangle(ref rc, sz.Width + 2, 0);
            }

            using (SolidBrush brush = new SolidBrush(foreColor))
            {
                Rectangle rcText = rc;
                rcText.X += B_OFS_X;
                gfx.DrawString(btn.Text, this.Font, brush, rcText, sf);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="expand"></param>
        /// <param name="gfx"></param>
        private void DrawExpandBox(Rectangle rc, Boolean expand, Graphics gfx)
        {
            gfx.FillRectangle(Brushes.White, rc);
            gfx.DrawRectangle(Pens.Black, rc);
            gfx.DrawLine(Pens.Black,
                new Point(rc.Left + 2, rc.Top + rc.Height/2),
                new Point(rc.Right - 2, rc.Top + rc.Height/2));
            if (!expand)
            {
                gfx.DrawLine(Pens.Black,
                    new Point(rc.Left + rc.Width/2, rc.Top + 2),
                    new Point(rc.Left + rc.Width / 2, rc.Bottom - 2));
            }
        }

        #endregion

        #region HitTest

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        private IToolboxItem HitTestToolboxItem(Point loc)
        {
            foreach (ToolboxGroup g in Groups)
            {
                IToolboxItem item = HitTestToolboxGroup(loc, g);
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        private IToolboxItem HitTestToolboxGroup(Point loc, ToolboxGroup g)
        {
            if (g.Bounds.Contains(loc))
            {
                return g;
            }

            if (g.Expand)
            {
                foreach (ToolboxButton btn in g.Buttons)
                {
                    if (btn.Bounds.Contains(loc))
                    {
                        return btn;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Event

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toolbox_MouseEnter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toolbox_MouseLeave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toolbox_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_drag)
            {
                if (ItemDrag != null)
                {
                    ItemDrag(this, new ToolboxEventArgs(SelectItem));
                }

                DataFormats.Format fmt = DataFormats.GetFormat("libpixy.net.Toolbox.Button");
                DataObject dataObj = new DataObject(fmt.Name, SelectItem);
                DoDragDrop(dataObj, DragDropEffects.All);
                m_drag = false;
                return;
            }
            
            Point pos = Cursor.Position;
            Point loc = PointToClient(pos);
            IToolboxItem item = HitTestToolboxItem(loc);
            if (item != null)
            {
                if (FocusItem != item)
                {
                    if (FocusItem != null)
                    {
                        FocusItem.Focus = false;
                    }

                    FocusItem = item;
                    FocusItem.Focus = true;
                    Invalidate();
                }
            }
            else if (FocusItem != null)
            {
                FocusItem.Focus = false;
                FocusItem = null;
                Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toolbox_MouseDown(object sender, MouseEventArgs e)
        {
            Point pos = Cursor.Position;
            Point loc = PointToClient(pos);
            IToolboxItem old = SelectItem;
            SelectItem = HitTestToolboxItem(loc);
            if (SelectItem != null)
            {
                SelectItem.Select = true;

                if (SelectItem is ToolboxGroup)
                {
                    ToolboxGroup g = (ToolboxGroup)SelectItem;
                    //if (g.GetExpandBoxRect().Contains(loc))
                    if (g.Bounds.Contains(loc))
                    {
                        g.Expand = !g.Expand;
                        g.Select = false;
                        RecalcLayout();
                        Invalidate();
                        SelectItem = old;
                        return;
                    }
                }
            }

            if (old != SelectItem)
            {
                if (old != null)
                {
                    old.Select = false;
                }

                if (ItemSelected != null)
                {
                    ItemSelected(this, new ToolboxEventArgs(SelectItem));
                }

                Invalidate();
            }

            if (SelectItem != null)
            {
                m_drag = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toolbox_MouseUp(object sender, MouseEventArgs e)
        {

        }

        #endregion

        private void Toolbox_ClientSizeChanged(object sender, EventArgs e)
        {
            RecalcLayout();
            Invalidate();
        }

        public static Color GetMiddleColor(Color c1, Color c2)
        {
            return Color.FromArgb(
                (c1.R + c2.R) / 2,
                (c1.G + c2.G) / 2,
                (c1.B + c2.B) / 2);
        }
    }
}
