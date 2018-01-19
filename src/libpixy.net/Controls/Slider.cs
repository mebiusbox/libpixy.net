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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libpixy.net.Controls.Standard
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Slider : UserControl
    {
        #region Fields

        private float m_lower = 0.0f;
        private float m_upper = 1.0f;
        private float m_value = 1.0f;
        private bool m_drag = false;
        private int m_dragX = 0;
        private TextBox m_edit = null;
        //private bool m_changed = false;
        //private bool m_integer = false;
        private System.Drawing.Color borderColor = Color.Black;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 上限値
        /// </summary>
        [Browsable(true)]
        public float Upper
        {
            get { return m_upper; }
            set { m_upper = value; }
        }

        /// <summary>
        /// 下限値
        /// </summary>
        [Browsable(true)]
        public float Lower
        {
            get { return m_lower; }
            set { m_lower = value; }
        }

        /// <summary>
        /// 現在値
        /// </summary>
        [Browsable(true)]
        public float Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// 関連付けされたテキストボックス
        /// </summary>
        [Browsable(true)]
        public TextBox Edit
        {
            get { return m_edit; }

            set 
            {
                if (m_edit != null)
                {
                    m_edit.TextChanged -= new EventHandler(m_edit_TextChanged);
                }

                m_edit = value;

                if (m_edit != null)
                {
                    m_edit.TextChanged += new EventHandler(m_edit_TextChanged);
                }
            }
        }

        [Browsable(true)]
        public System.Drawing.Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        #endregion

        #region EventHandler

        public event EventHandler<EventArgs> ValueChanged;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public Slider()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.Opaque,
                true);
        }

        #region Methods

        /// <summary>
        /// 関連付けされたテキストボックスを更新する
        /// </summary>
        public void UpdateText()
        {
            if (m_edit != null)
            {
                m_edit.Text = m_value.ToString("0.0#####");
            }
        }

        /// <summary>
        /// ValueChanged イベントを起こす
        /// </summary>
        private void RaiseValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        #endregion Methods

        private void Slider_Paint(object sender, PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            using (Pen pen = new Pen(this.borderColor,1.0f))
            {
                Rectangle rc = this.ClientRectangle;
                e.Graphics.DrawRectangle(pen, rc.Left, rc.Top, rc.Width-1, rc.Height-1);
            }

            using (SolidBrush brush = new SolidBrush(this.ForeColor))
            {
                Rectangle rc = this.ClientRectangle;
                float rate = m_value / (m_upper - m_lower);
                int w = (int)((float)(this.ClientRectangle.Width-2) * rate);
                int h = this.ClientRectangle.Height - 2;
                e.Graphics.FillRectangle(brush, new Rectangle(1, 1, w, h));
            }
        }

        private void Slider_Load(object sender, EventArgs e)
        {
            //nop
        }

        private void Slider_MouseDown(object sender, MouseEventArgs e)
        {
            m_drag = true;
            m_dragX = e.X;
            float rate = ((float)e.X / ((float)this.ClientRectangle.Width));
            m_value = (m_upper - m_lower) * rate;
            Invalidate();
            UpdateText();
            RaiseValueChanged();
        }

        private void Slider_MouseUp(object sender, MouseEventArgs e)
        {
            m_drag = false;
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_drag)
            {
                float rate = 0.0f;
                if (e.X < 0)
                {
                    rate = 0.0f;
                }
                else if (e.X > this.ClientRectangle.Right)
                {
                    rate = 1.0f;
                }
                else
                {
                    rate = (float)e.X / ((float)this.ClientRectangle.Width);
                }

                m_value = ((m_upper - m_lower) * rate);
                Invalidate();
                UpdateText();
                RaiseValueChanged();
            }
        }

        private void Slider_MouseEnter(object sender, EventArgs e)
        {
            //nop
        }

        private void Slider_MouseLeave(object sender, EventArgs e)
        {
            //nop
        }

        private void m_edit_TextChanged(Object sender, EventArgs e)
        {
            try
            {
                float rate = 0.0f;
                float value = (float)Convert.ToDouble(m_edit.Text);
                if (value < m_lower)
                {
                    rate = 0.0f;
                }
                else if (value > m_upper)
                {
                    rate = 1.0f;
                }
                else
                {
                    rate = (value / (m_upper - m_lower));
                }

                m_value = (m_upper - m_lower) * rate;
                //m_changed = true;
                Invalidate();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
