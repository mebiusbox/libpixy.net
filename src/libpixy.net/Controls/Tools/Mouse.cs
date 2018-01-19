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
using System.Drawing;
using System.Windows.Forms;

namespace libpixy.net.Controls.Tool
{
    /// <summary>
    /// マウス
    /// </summary>
    public class Mouse
    {
        public Point Location;
        public Point OldLocation;
        public Point DragLocation;
        public bool Press;
        public bool Slide;
        public bool Click;
        public Keys ModifierKeys;
        public MouseButtons Button;
        public MouseDrag Drag;
        public MouseSelection Selection;
        public bool EnableDrag;
        public bool EnableSelection;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Mouse()
        {
            this.Location = new Point();
            this.OldLocation = new Point();
            this.DragLocation = new Point();
            this.Press = false;
            this.Slide = false;
            this.Click = false;
            this.Button = MouseButtons.None;
            this.Drag = new MouseDrag();
            this.Selection = new MouseSelection();
            this.EnableDrag = false;
            this.EnableSelection = false;
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            this.Press = false;
            this.Button = MouseButtons.None;
            this.Drag.Clear();
            this.Selection.Clear();
        }

        /// <summary>
        /// マウスの移動
        /// </summary>
        /// <param name="e"></param>
        public void Move(MouseEventArgs e)
        {
            if (this.Press)
            {
                this.Slide = true;
            }

            Update(e);
        }

        /// <summary>
        /// マウスボタンが押された
        /// </summary>
        /// <param name="e"></param>
        public void Down(MouseEventArgs e)
        {
            this.Button = e.Button;
            this.ModifierKeys = Control.ModifierKeys;
            this.Press = true;
            Update(e);
        }

        /// <summary>
        /// マウスボタンを離した
        /// </summary>
        /// <param name="e"></param>
        public void Up(MouseEventArgs e)
        {
            this.Press = false;
            this.Slide = false;
            Update(e);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="e"></param>
        public void Update(MouseEventArgs e)
        {
            this.OldLocation = this.Location;
            this.Location = e.Location;
            if (this.Drag.Valid && EnableDrag)
            {
                this.Drag.OldLocation = this.OldLocation;
                this.Drag.Location = this.Location;
                if (this.Selection.Valid && EnableSelection)
                {
                    this.Selection.EndPos = e.Location;
                }
            }
        }

        /// <summary>
        /// ドラッグ開始.
        /// </summary>
        public void BeginDrag()
        {
            this.Drag.Valid = true;
            this.Drag.Location = this.Location;
            this.Drag.OldLocation = this.Location;
            this.EnableDrag = true;
        }

        /// <summary>
        /// ドラッグ終了.
        /// </summary>
        public void EndDrag()
        {
            this.Drag.Valid = false;
            this.EnableDrag = false;
            this.Slide = false;
            this.Press = false;
        }

        /// <summary>
        /// 移動した距離
        /// </summary>
        /// <returns></returns>
        public Point GetMoved()
        {
            return new Point(
                this.Location.X - this.OldLocation.X,
                this.Location.Y - this.OldLocation.Y);
        }
    }
}
