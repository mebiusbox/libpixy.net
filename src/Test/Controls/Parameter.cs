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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Test.Controls
{
    public partial class Parameter : Controls.ParameterBase
    {
        private string label = "Label";
        private float value = 0.0f;
        private float defaultValue = 0.0f;
        private bool showCurveButton = true;
        private bool showLinkButton = false;

        public Parameter()
        {
            this.InitializeComponent();
        }

        #region =========== Properties ==========
        #endregion

        [Browsable(true)]
        public string Label
        {
            get 
            {
                return this.label; 
            }

            set
            {
                this.label = value;
                this.uiLabel.Text = this.label;
            }
        }

        [Browsable(true)]
        public bool ShowCurveButton
        {
            get 
            {
                return this.showCurveButton; 
            }

            set
            {
                this.showCurveButton = value;
                this.uiCurve.Visible = this.showCurveButton;
            }
        }

        [Browsable(true)]
        public bool ShowLinkButton
        {
            get 
            {
                return this.showLinkButton; 
            }

            set
            {
                this.showLinkButton = value;
                this.uiLink.Visible = this.showLinkButton;
            }
        }

        [Browsable(true)]
        public float uidValue
        {
            get
            {
                return this.value; 
            }

            set
            {
                this.value = value;
                this.uiValue.Text = this.value.ToString("0.0000");
                this.UpdateValueControlColor();
            }
        }

        [Browsable(true)]
        public float uidDefaultValue
        {
            get { return this.defaultValue; }
            set { this.defaultValue = value; }
        }

        [Browsable(false)]
        public override libpixy.net.Controls.Standard.FrameButton CurveButton
        {
            get
            {
                return this.showCurveButton ? this.uiCurve : null;
            }
        }

        [Browsable(false)]
        public override libpixy.net.Controls.Standard.FrameButton LinkButton
        {
            get
            {
                return this.showLinkButton ? this.uiLink : null;
            }
        }

        #region =========== Public Methods ==========
        #endregion

        public override void LoadValue()
        {
        }

        public override void SaveValue()
        {
        }

        public void UpdateValueControlColor()
        {
            if (this.DesignMode)
            {
                return;
            }

            if (this.value != this.defaultValue)
            {
                this.uiValue.BackColor = Color.FromArgb(154, 0, 0);
                this.uiValuePanel.BackgroundImage = Properties.Resources.bordered_textbox_changed;
            }
            else
            {
                this.uiValue.BackColor = Color.FromArgb(51, 51, 51);
                this.uiValuePanel.BackgroundImage = Properties.Resources.bordered_textbox;
            }
        }

        #region =========== UI Events ==========
        #endregion

        private void uiCurve_Click(object sender, EventArgs e)
        {
            this.RaiseClickedEvent("key");
            this.UpdateButtons();
        }

        private void uiLink_Click(object sender, EventArgs e)
        {
            this.RaiseValueChangedEvent("value", new ParameterChangedEventArgs(this.uiLink.Checked));
            this.UpdateButtons();
        }

        private void uiValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (Control.ModifierKeys == Keys.Shift)
                {
                    this.uidValue = this.uidValue + 10.0f;
                }
                else
                {
                    this.uidValue = this.uidValue + 1.0f;
                }

                this.uiValue.Modified = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (Control.ModifierKeys == Keys.Shift)
                {
                    this.uidValue = this.uidValue - 10.0f;
                }
                else
                {
                    this.uidValue = this.uidValue - 1.0f;
                }

                this.uiValue.Modified = true;
            }
        }

        private void uiValue_ModifiedChanged(object sender, EventArgs e)
        {
            if (this.uiValue.Modified)
            {
                float value;
                if (float.TryParse(this.uiValue.Text, out value))
                {
                    this.value = value;
                    this.UpdateValueControlColor();
                    this.RaiseValueChangedEvent("value", new ParameterChangedEventArgs(value));
                }

                this.uiValue.Modified = false;
            }
        }

        private void Parameter_Load(object sender, EventArgs e)
        {
            this.ChangedEventEnable = true;
        }
    }
}
