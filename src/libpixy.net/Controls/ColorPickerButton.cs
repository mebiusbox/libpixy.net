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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libpixy.net.Controls.Standard
{
    public partial class ColorPickerButton : Control
    {
        #region Private Member Variables

        private bool m_mouseCapture = false;

        #endregion // Private Member Variables

        #region Public Properties

        public Image CheckedImage { get; set; }
        public Image Image { get; set; }
        public Color OuterColor { get; set; }
        public Color InnerColor { get; set; }
        public Color CheckedInnerColor { get; set; }
        public Color CheckedOuterColor { get; set; }
        public bool Checked { get; set; }
        public bool Rounded { get; set; }

        #endregion // Public Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ColorPickerButton()
        {
            this.Rounded = true;
            this.Checked = false;
            this.OuterColor = Color.Black;
            this.InnerColor = Color.FromArgb(100, 100, 100);
            this.CheckedOuterColor = this.OuterColor;
            this.CheckedInnerColor = this.InnerColor;
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        #endregion // Constructor

        #region Events

        public event EventHandler ValueChanged;

        #endregion // Events

        #region Event Handler
    

        protected override void OnPaint(PaintEventArgs pe)
        {
            //base.OnPaint(pe);

            Paint_Background(pe.Graphics);
            Paint_ButtonInner(pe.Graphics);
            Paint_ButtonOuter(pe.Graphics);
            Paint_Image(pe.Graphics);
        }

        private void Paint_Background(Graphics g)
        {
            using (Brush brush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void Paint_ButtonInner(Graphics g)
        {
            Rectangle rc = this.ClientRectangle;
            Color c = (this.Checked) ? this.CheckedInnerColor : this.InnerColor;
            using (Brush brush = new SolidBrush(c))
            {
                if (this.Rounded)
                {
                    libpixy.net.Controls.Diagram.Draw.FillRoundedRectangleSmall(g, rc, 2, brush);
                }
                else
                {
                    g.FillRectangle(brush, rc);
                }
            }
        }

        private void Paint_ButtonOuter(Graphics g)
        {
            Rectangle rc = this.ClientRectangle;
            rc.Size = new Size(this.Width - 1, this.Height - 1);
            Color c = (this.Checked) ? this.CheckedOuterColor : this.OuterColor;
            using (Pen pen = new Pen(c, 1))
            {
                if (this.Rounded)
                {
                    libpixy.net.Controls.Diagram.Draw.DrawRoundedRectangle(g, rc, 4, pen);
                }
                else
                {
                    g.DrawRectangle(pen, rc);
                }
            }
        }

        private void Paint_Image(Graphics g)
        {
            Image img = this.Image;
            if (this.Checked)
            {
                img = this.CheckedImage;
            }

            if (img == null)
            {
                return;
            }

            int w = img.Width;
            int h = img.Height;
            int x = this.Width/2 - w/2;
            int y = this.Height / 2 - h / 2;
            g.DrawImage(img, new Rectangle(x,y,w,h));
        }

        private void BitmapButton_Click(object sender, EventArgs e)
        {
            if (m_mouseCapture)
            {
                this.Checked = !this.Checked;
                Invalidate();
                libpixy.net.Vecmath.Utils.RaiseEvent(this.ValueChanged, this);
            }
        }

        private void BitmapButton_MouseEnter(object sender, EventArgs e)
        {
            m_mouseCapture = true;
            this.Cursor = Cursors.Hand;
        }

        private void BitmapButton_MouseLeave(object sender, EventArgs e)
        {
            m_mouseCapture = false;
            this.Cursor = Cursors.Default;
        }

        #endregion // Event Handler
    }
}
