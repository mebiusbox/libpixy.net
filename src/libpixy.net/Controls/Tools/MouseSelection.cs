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

namespace libpixy.net.Controls.Tool
{
    /// <summary>
    /// マウス選択
    /// </summary>
    public class MouseSelection
    {
        /// <summary>
        /// 開始位置
        /// </summary>
        public Point StartPos = new Point();

        /// <summary>
        /// 終了位置
        /// </summary>
        public Point EndPos = new Point();

        /// <summary>
        /// 選択領域が有効かどうか
        /// </summary>
        public bool Valid = false;

        /// <summary>
        /// 選択領域を取得
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRect()
        {
            Rectangle rc = new Rectangle(
                0,
                0,
                Math.Abs(this.EndPos.X - this.StartPos.X),
                Math.Abs(this.EndPos.Y - this.StartPos.Y));

            // 正規化
            if (this.StartPos.X > this.EndPos.X)
            {
                rc.X = this.EndPos.X;
            }
            else
            {
                rc.X = this.StartPos.X;
            }

            if (this.StartPos.Y > this.EndPos.Y)
            {
                rc.Y = this.EndPos.Y;
            }
            else
            {
                rc.Y = this.StartPos.Y;
            }

            return rc;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MouseSelection()
        {
            Clear();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            this.Valid = false;
        }
    };
}
