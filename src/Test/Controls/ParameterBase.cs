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
    public partial class ParameterBase : UserControl
    {
        private string scriptName = string.Empty;

        public ParameterBase()
        {
            this.InitializeComponent();
            this.ChangedEventEnable = false;
        }

        #region =========== Properties ==========
        #endregion

        /// <summary>
        /// ボタンまたはメニューがクリックされたときに呼ばれるデリゲート定義.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="id"></param>
        public delegate void ClickedEvent(object sender, string scriptName, string eventName);

        /// <summary>
        /// 値が変更されたときに呼ばれるデリゲート定義.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="id"></param>
        public delegate void ValueChangedEvent(object sender, string id, ParameterChangedEventArgs hint);

        /// <summary>
        /// 値が変更されたときに発生するイベント.
        /// </summary>
        public event ClickedEvent Clicked;

        /// <summary>
        /// 値が変更されたときに発生するイベント.
        /// </summary>
        public event ValueChangedEvent ValueChanged;

        [Browsable(true)]
        public string ScriptName
        {
            get { return this.scriptName; }
            set { this.scriptName = value; }
        }

        [Browsable(false)]
        public bool ChangedEventEnable { get; set; }

        [Browsable(false)]
        public virtual libpixy.net.Controls.Standard.FrameButton CurveButton
        {
            get { return null; }
        }

        [Browsable(false)]
        public virtual libpixy.net.Controls.Standard.FrameButton LinkButton
        {
            get { return null; }
        }
        
        #region =========== Public Methocs ==========
        #endregion

        public virtual void CurrentFrameChanged()
        {
            ////stub
        }

        /// <summary>
        /// ScriptName に対応した属性値を取得する
        /// </summary>
        public virtual void LoadValue()
        {
            ////stub
        }

        /// <summary>
        /// ScriptName に対応した属性値を設定する
        /// </summary>
        public virtual void SaveValue()
        {
            ////stub
        }

        /// <summary>
        /// キーカーブボタンとリンクボタンを更新する
        /// </summary>
        public void UpdateButtons()
        {
            this.RedrawButtons();
        }

        /// <summary>
        /// ボタンを再描画
        /// </summary>
        public void RedrawButtons()
        {
            if (this.CurveButton != null)
            {
                this.CurveButton.Refresh();
            }
        }

        /// <summary>
        /// ValueChanged イベントを発生する.
        /// </summary>
        /// <param name="id"></param>
        public void RaiseClickedEvent(string eventName)
        {
            if (this.Clicked != null)
            {
                this.Clicked(this, this.scriptName, eventName);
            }
        }

        /// <summary>
        /// ValueChanged イベントを発生する.
        /// </summary>
        /// <param name="id"></param>
        public void RaiseValueChangedEvent(string id, ParameterChangedEventArgs hint)
        {
            if (this.ChangedEventEnable && this.ValueChanged != null)
            {
                this.ValueChanged(this, this.scriptName + "." + id, hint);
            }
        }

        #region =========== Private Methods ==========
        #endregion

        protected void InitContextMenu()
        {
            if (this.CurveButton != null)
            {
                this.uiMenuAnimationEditor.Enabled = true;
                this.uiMenuRemoveAllKey.Enabled = true;
            }
            else
            {
                this.uiMenuAnimationEditor.Enabled = false;
                this.uiMenuRemoveAllKey.Enabled = false;
            }
        }

        #region =========== Events ==========
        #endregion

        private void uiContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == this.uiMenuRemoveAllKey)
            {
                this.RaiseClickedEvent("remove_all_key");
                this.UpdateButtons();
            }
            else if (e.ClickedItem == this.uiMenuAnimationEditor)
            {
                this.RaiseClickedEvent("curve");
            }
        }

        private void ParameterBase_Load(object sender, EventArgs e)
        {
            this.InitContextMenu();
        }
    }
}
