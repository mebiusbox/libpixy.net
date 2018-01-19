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

namespace libpixy.net.Vecmath
{
    public class Vector2
    {
        #region Fields

        public float X;
        public float Y;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Vector2()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
            : base()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pt"></param>
        public Vector2(PointF pt)
            : base()
        {
            X = pt.X;
            Y = pt.Y;
        }

        #endregion

        #region Overload operators

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X-rhs.X, lhs.Y-rhs.Y);
        }

        public static Vector2 operator *(Vector2 lhs, float s)
        {
            return new Vector2(lhs.X *s, lhs.Y *s);
        }

        public static Vector2 operator *(float s, Vector2 rhs)
        {
            return new Vector2(s*rhs.X, s*rhs.Y);
        }

        public static Vector2 operator /(Vector2 lhs, float s)
        {
            return new Vector2(lhs.X / s, lhs.Y * s);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        #endregion

        #region Public Methods

        public void Set(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Add(Vector2 v)
        {
            X += v.X;
            Y += v.Y;
        }

        public void Sub(Vector2 v)
        {
            X -= v.X;
            Y -= v.Y;
        }

        public void Mul(float s)
        {
            X *= s;
            Y *= s;
        }

        public void Div(float s)
        {
            X /= s;
            Y /= s;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public float LengthSq()
        {
            return X * X + Y * Y;
        }

        public float Length(Vector2 v)
        {
            return (float)Math.Sqrt(LengthSq(v));
        }

        public float LengthSq(Vector2 v)
        {
            return (v.X - X) * (v.X - X) + (v.Y - Y) * (v.Y - Y);
        }

        public void Normalize()
        {
            Div(Length());
        }

        public void Normalize(Vector2 v)
        {
            Set(v.X, v.Y);
            Normalize();
        }

        public float Distance(Vector2 v)
        {
            return Distance(this, v);
        }

        public float DistanceSq(Vector2 v)
        {
            return DistanceSq(this, v);
        }

        /// <summary>
        /// PointF 型に変換
        /// </summary>
        /// <returns></returns>
        public PointF ToPointF()
        {
            return new PointF(this.X, this.Y);
        }

        /// <summary>
        /// Point 型に変換
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            return new Point((int)this.X, (int)this.Y);
        }

        #endregion

        #region Static Functions

        /// <summary>
        /// 位置ベクトル v1 と位置ベクトル v2 との距離を求める
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float Distance(Vector2 v1, Vector2 v2)
        {
            return v1.Length(v2);
        }

        /// <summary>
        /// 位置ベクトル v1 と位置ベクトル v2 との距離（べき乗）を求める
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float DistanceSq(Vector2 v1, Vector2 v2)
        {
            return v1.LengthSq(v2);
        }

        /// <summary>
        /// 内積を求める
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static float Dot(float x1, float y1, float x2, float y2)
        {
            return x1 * x2 + y1 * y2;
        }

        /// <summary>
        /// 内積を求める
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return Dot(v1.X, v1.Y, v2.X, v2.Y);
        }

        #endregion

    }
}
