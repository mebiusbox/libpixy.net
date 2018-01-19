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
using libpixy.net.Vecmath;

namespace libpixy.net.Controls.CurveEditor
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlPoint : EditableObject, IMovable
    {
        public Vector2 Position = new Vector2();
        public Vector2 Value = new Vector2();
        public Curve Curve = new Curve();
        public HandlePoint Handle1 = new HandlePoint();
        public HandlePoint Handle2 = new HandlePoint();

        #region Constructor/Destructor

        public ControlPoint()
            : base()
        {
            this.Handle1.Position = new Vector2(20.0f, 0.0f);
            this.Handle2.Position = new Vector2(-20.0f, 0.0f);
        }

        #endregion

        #region Properties

        /// <summary>
        /// ハンドル1の絶対位置
        /// </summary>
        public Vector2 Handle1Position
        {
            get { return Position + Handle1.Position; }
            set { Handle1.Position = value - Position; }
        }

        /// <summary>
        /// ハンドル2の絶対位置
        /// </summary>
        public Vector2 Handle2Position
        {
            get { return Position + Handle2.Position; }
            set { Handle2.Position = value - Position; }
        }

        #endregion

        #region Method

        /// <summary>
        /// 相対移動
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void Move(float dx, float dy)
        {
            this.Position.X += dx;
            this.Position.Y += dy;
        }

        /// <summary>
        /// 絶対移動
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(float x, float y)
        {
            this.Position.X = x;
            this.Position.Y = y;
        }

        #endregion Method
    }
}
