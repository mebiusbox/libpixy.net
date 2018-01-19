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

namespace Test.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public enum ColorComponent
    {
        R,
        G,
        B,
        A
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ColorPanel : UserControl
    {
        #region Fields

        private ColorPanelController m_controller = null;
        private bool m_enableAlpha = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// α値の編集を有効にするかどうか
        /// </summary>
        [Browsable(true)]
        public bool EnableAlpha
        {
            get { return m_enableAlpha; }
            set
            {
                m_enableAlpha = value;

                if (m_enableAlpha)
                {
                    colorTextBox4.Show();
                    colorLabel4.Show();
                    colorSlider4.Show();
                }
                else
                {
                    colorTextBox4.Hide();
                    colorLabel4.Hide();
                    colorSlider4.Hide();
                }

                this.Invalidate();
            }
        }

        /// <summary>
        /// カラー値
        /// </summary>
        [Browsable(true)]
        public Color Color
        {
            get { return m_controller.GetColor(); }
            set { m_controller.SetColor(value); }
        }

        #endregion Properties

        #region EventHandler

        public event EventHandler<EventArgs> ValueChanged;

        #endregion EventHandler

        /// <summary>
        /// Constructor
        /// </summary>
        public ColorPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            m_controller = new Test.Controls.ColorPanelController(
                colorPictureBox,
                colorSpaceButton,
                colorTextBox1,
                colorTextBox2,
                colorTextBox3,
                colorTextBox4,
                colorLabel1,
                colorLabel2,
                colorLabel3,
                colorSlider1,
                colorSlider2,
                colorSlider3,
                colorSlider4);
            m_controller.UpdatePictureBoxFromSlider();
            m_controller.ValueChanged += new EventHandler<EventArgs>(m_controller_ValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_controller_ValueChanged(object sender, EventArgs e)
        {
            RaiseValueChanged();
        }

        /// <summary>
        /// ValueChanged イベントを起こす
        /// </summary>
        public void RaiseValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new EventArgs());
            }
        }
    }
}
