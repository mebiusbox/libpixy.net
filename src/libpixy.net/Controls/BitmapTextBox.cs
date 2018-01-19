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

namespace libpixy.net.Controls.Standard
{
    public class BitmapTextBox : System.Windows.Forms.TextBox
    {
        private Image m_themeImage = null;

        public BitmapTextBox()
        {
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.Opaque, true);
        }

        [Browsable(true)]
        [Category("Appearence")]
        public Image ThemeImage
        {
            get { return m_themeImage; }
            set
            {
                m_themeImage = value;
                Invalidate();
            }
        }

        private void DrawTheme(Graphics g)
        {
            if (ThemeImage != null)
            {
                g.DrawImage(ThemeImage, new Point(0, 0));
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0)), this.ClientRectangle);
            }
        }

        private void DrawText(Graphics g)
        {
            TextFormatFlags tff = TextFormatFlags.VerticalCenter;
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Center:
                    tff |= TextFormatFlags.HorizontalCenter;
                    break;
                case HorizontalAlignment.Left:
                    tff |= TextFormatFlags.Left;
                    break;
                case HorizontalAlignment.Right:
                    tff |= TextFormatFlags.Right;
                    break;
            }
            if (this.Multiline)
            {
                tff |= TextFormatFlags.WordBreak;
            }

            using (Brush backBrush = new SolidBrush(this.BackColor))
            {
                //BackColorで背景を塗りつぶす。
                //これをしないとフォーマット前後の文字列がダブって表示される。
                g.FillRectangle(backBrush, this.ClientRectangle);

                DateTime dtVal;
                decimal dcVal;
                if (DateTime.TryParse(this.Text, out dtVal))
                {
                    TextRenderer.DrawText(g, dtVal.ToString("yyyy/MM/dd"),
                        this.Font, this.ClientRectangle, this.ForeColor, tff);
                }
                else if (decimal.TryParse(this.Text, out dcVal))
                {
                    TextRenderer.DrawText(g, dcVal.ToString("#,##0"),
                        this.Font, this.ClientRectangle, this.ForeColor, tff);
                }
                else
                {
                    TextRenderer.DrawText(g, this.Text,
                        this.Font, this.ClientRectangle, this.ForeColor, tff);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            //DrawTheme(e.Graphics);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 14)
            {
                DrawTheme(this.CreateGraphics());
                return;
            }

            base.WndProc(ref m);

#if false
            if (m.Msg == 15)
            {
                if (this.Focused == false)
                {
                    DrawTheme(this.CreateGraphics());
                    DrawText(this.CreateGraphics());
                    return;
                }
                else
                {
                    DrawText(this.CreateGraphics());
                    return;
                }
            }
#endif
        }

    }
}
