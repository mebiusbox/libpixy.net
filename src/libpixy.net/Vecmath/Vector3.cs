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

namespace libpixy.net.Vecmath
{
    /// <summary>
    /// ３次元ベクトル
    /// </summary>
    public class Vector3
    {
        #region Fields

        public float X;
        public float Y;
        public float Z;

        #endregion

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Vector3() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(float x, float y, float z)
            : base()
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="v"></param>
        public Vector3(Vector3 v)
            : base()
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        #endregion Constructor, Destructor

        #region Overload operators

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X-rhs.X, lhs.Y-rhs.Y, lhs.Z - rhs.Z);
        }

        public static Vector3 operator *(Vector3 lhs, float s)
        {
            return new Vector3(lhs.X *s, lhs.Y *s, lhs.Z*s);
        }

        public static Vector3 operator *(float s, Vector3 rhs)
        {
            return new Vector3(rhs.X * s, rhs.Y * s, rhs.Z * s);
        }

        public static Vector3 operator *(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
        }

        public static Vector3 operator /(Vector3 lhs, float s)
        {
            float oneOverA = 1.0f / s;
            return new Vector3(lhs.X * oneOverA, lhs.Y * oneOverA, lhs.Z * oneOverA);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Set(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// 加算
        /// </summary>
        /// <param name="v"></param>
        public void Add(Vector3 v)
        {
            X += v.X;
            Y += v.Y;
            Z += v.Z;
        }

        /// <summary>
        /// 減算
        /// </summary>
        /// <param name="v"></param>
        public void Sub(Vector3 v)
        {
            X -= v.X;
            Y -= v.Y;
            Z -= v.Z;
        }

        /// <summary>
        /// 乗算
        /// </summary>
        /// <param name="v"></param>
        public void Mul(Vector3 v)
        {
            X *= v.X;
            Y *= v.Y;
            Z *= v.Z;
        }

        /// <summary>
        /// 乗算
        /// </summary>
        /// <param name="s"></param>
        public void Mul(float s)
        {
            X *= s;
            Y *= s;
            Z *= s;
        }

        /// <summary>
        /// 除算
        /// </summary>
        /// <param name="s"></param>
        public void Div(float s)
        {
            X /= s;
            Y /= s;
            Z /= s;
        }

        /// <summary>
        /// 長さ
        /// </summary>
        /// <returns></returns>
        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>
        /// 長さ（２乗）
        /// </summary>
        /// <returns></returns>
        public float LengthSq()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// 長さを設定する
        /// </summary>
        /// <param name="l"></param>
        public void SetLength(float l)
        {
            float len_sq = LengthSq();
            if (len_sq > 0.0f)
            {
                float len = l * ( 1.0f / (float)Math.Sqrt(len_sq));
                this.X *= len;
                this.Y *= len;
                this.Z *= len;
            }
        }

        /// <summary>
        /// 反転
        /// </summary>
        public void Reverse()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// 正規化
        /// </summary>
        public void Normalize()
        {
            float q = LengthSq();
            if (q > 0.0f)
            {
                Mul(1.0f / (float)Math.Sqrt(q));
            }
        }

        /// <summary>
        /// 正規化
        /// </summary>
        /// <param name="v"></param>
        public void Normalize(Vector3 v)
        {
            Set(v.X, v.Y, v.Z);
            Normalize();
        }

        /// <summary>
        /// 内積
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public float Dot(Vector3 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        /// <summary>
        /// 外積
        /// </summary>
        /// <param name="v"></param>
        public void Cross(Vector3 v)
        {
            Set(Y * v.Z - Z * v.Y, Z * v.Y - X * v.Z, X * v.Y - Y * v.X);
        }

        /// <summary>
        /// 外積
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public void Cross(Vector3 v1, Vector3 v2)
        {
            Set(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.Y - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        /// <summary>
        /// 回転（Ｘ軸）
        /// </summary>
        /// <param name="angle"></param>
        public void RotX(float angle)
        {
            float xx = X;
            float yy = Y;
            float zz = Z;
            float pp = Constant.kPiOver180;
            X = xx;
            Y = yy * (float)Math.Cos(pp * angle) - zz * (float)Math.Sin(pp * angle);
            Z = yy * (float)Math.Sin(pp * angle) + zz * (float)Math.Cos(pp * angle);
        }

        /// <summary>
        /// 回転（Ｙ軸）
        /// </summary>
        /// <param name="angle"></param>
        public void RotY(float angle)
        {
            float xx = X;
            float yy = Y;
            float zz = Z;
            float pp = Constant.kPiOver180;
            X = xx * (float)Math.Cos(pp * angle) + zz * (float)Math.Sin(pp * angle);
            Y = yy;
            Z = -xx * (float)Math.Sin(pp * angle) + zz * (float)Math.Cos(pp * angle);
        }

        /// <summary>
        /// 回転（Ｚ軸）
        /// </summary>
        /// <param name="angle"></param>
        public void RotZ(float angle)
        {
            float xx = X;
            float yy = Y;
            float zz = Z;
            float pp = Constant.kPiOver180;
            X = xx * (float)Math.Cos(pp * angle) - yy * (float)Math.Sin(pp * angle);
            Y = xx * (float)Math.Sin(pp * angle) + yy * (float)Math.Cos(pp * angle);
            Z = zz;
        }

        /// <summary>
        /// 回転（任意軸）
        /// </summary>
        /// <param name="v"></param>
        /// <param name="elr"></param>
        /// <returns></returns>
        public Vector3 Rotate(Vector3 v, Vector3 elr)
        {
            Vector3 ret = new Vector3();
            ret.Set(v.X, v.Y, v.Z);
            ret.RotX(elr.X);
            ret.RotY(elr.Y);
            ret.RotZ(elr.Z);
            return ret;
        }

        /// <summary>
        /// 線型補間
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Leap(Vector3 v1, Vector3 v2, float t)
        {
            return new Vector3(
                (v2.X - v1.X) * t + v1.X,
                (v2.Y - v1.Y) * t + v1.Y,
                (v2.Z - v1.Z) * t + v1.Z);
        }

        #endregion

        #region Constants
        public static Vector3 Unit { get { return new Vector3(1.0f, 1.0f, 1.0f); } }
        public static Vector3 Zero { get { return new Vector3(0.0f, 0.0f, 0.0f); } }
        public static Vector3 XAxis { get { return new Vector3(1.0f, 0.0f, 0.0f); } }
        public static Vector3 YAxis { get { return new Vector3(0.0f, 1.0f, 0.0f); } }
        public static Vector3 ZAxis { get { return new Vector3(0.0f, 0.0f, 1.0f); } }
        #endregion Constants

        #region Static Methods

        /// <summary>
        /// 内積
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DotProduct(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// ベクトルの大きさ
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float Magnitude(Vector3 a)
        {
            return (float)Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
        }

        /// <summary>
        /// ２つのベクトル間の大きさ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Magnitude(Vector3 a, Vector3 b)
        {
            return Distance(a,b);
        }

        /// <summary>
        /// 外積
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        /// <summary>
        /// ２つのベクトル間の距離
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Distance(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt(DistanceSq(a, b));
        }

        /// <summary>
        /// ２つのベクトル間の距離（平方）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DistanceSq(Vector3 a, Vector3 b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            float dz = a.Z - b.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        /// <summary>
        /// ２つのベクトルのなす角
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GetAngle(Vector3 a, Vector3 b)
        {
            float ang = DotProduct(a, b) / (a.Length() * b.Length());
            if (ang >= 1.0f) {
                ang = 0.0f;
            }
            else if (ang <= -1.0f)
            {
                ang = Constant.kPi;
            }
            else
            {
                ang = (float)Math.Acos(ang);
            }

            return ang;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 GetEulerX(Vector3 a, Vector3 b)
        {
            float cx = b.X - a.X;
            float cy = b.Y - a.Y;
            float cz = b.Z - a.Z;
            float len = Distance(a, b);
            Vector3 ret = new Vector3();
            ret.X = 0.0f;
            if (cz >= len)
            {
                ret.Y = -90.0f;
            }
            else if (cz <= -len)
            {
                ret.Y = 90.0f;
            }
            else
            {
                ret.Y = -((float)Math.Asin(cz / len) * 180.0f / Constant.kPi);
            }

            if (Math.Abs(cx) <= 0.0001 && Math.Abs(cy) <= 0.0001)
            {
                ret.Z = 0.0f;
            }
            else
            {
                ret.Z = ((float)Math.Atan2(cy, cx) * 180.0f / Constant.kPi);
            }

            return ret;
        }

        /// <summary>
        /// ベクトル a から ベクトル b への方向ベクトル（単位ベクトル）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 GetNormal(Vector3 a, Vector3 b)
        {
            Vector3 t = b - a;
            t.Normalize();
            return t;
        }

        #endregion

        /// <summary>
        /// クローン
        /// </summary>
        /// <returns></returns>
        public libpixy.net.Vecmath.Vector3 Clone()
        {
            return new Vector3(X, Y, Z);
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("X = {0}, Y = {1}, Z = {2}", X, Y, Z);
        }
    }
}
