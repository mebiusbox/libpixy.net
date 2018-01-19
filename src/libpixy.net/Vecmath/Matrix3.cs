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
    public class Matrix3
    {
        #region Fields

        public float m11;
        public float m12;
        public float m13;
        public float m21;
        public float m22;
        public float m23;
        public float m31;
        public float m32;
        public float m33;

        #endregion

        #region Constructor/Destructor

        public Matrix3() : base()
        {
        }

        public Matrix3(
            float n11, float n12, float n13,
            float n21, float n22, float n23,
            float n31, float n32, float n33)
            : base()
        {
            m11 = n11; m12 = n12; m13 = n13;
            m21 = n21; m22 = n22; m23 = n23;
            m31 = n31; m32 = n32; m33 = n33;
        }

        #endregion

        #region Overloads operator

        public static Matrix3 operator -(Matrix3 m)
        {
            return new Matrix3(
                -m.m11, -m.m12, -m.m13,
                -m.m21, -m.m22, -m.m23,
                -m.m31, -m.m32, -m.m33);
        }

        public static Matrix3 operator +(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs.m11 + rhs.m11,
                lhs.m12 + rhs.m12,
                lhs.m13 + rhs.m13,
                lhs.m21 + rhs.m21,
                lhs.m22 + rhs.m22,
                lhs.m23 + rhs.m23,
                lhs.m31 + rhs.m31,
                lhs.m32 + rhs.m32,
                lhs.m33 + rhs.m33);
        }

        public static Matrix3 operator -(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs.m11 - rhs.m11,
                lhs.m12 - rhs.m12,
                lhs.m13 - rhs.m13,
                lhs.m21 - rhs.m21,
                lhs.m22 - rhs.m22,
                lhs.m23 - rhs.m23,
                lhs.m31 - rhs.m31,
                lhs.m32 - rhs.m32,
                lhs.m33 - rhs.m33);
        }

        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31,
                lhs.m11 * rhs.m21 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32,
                lhs.m11 * rhs.m31 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33,

                lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31,
                lhs.m21 * rhs.m21 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32,
                lhs.m21 * rhs.m31 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33,

                lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31,
                lhs.m31 * rhs.m21 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32,
                lhs.m31 * rhs.m31 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33);
        }

        public static Matrix3 operator *(Matrix3 lhs, float s)
        {
            return new Matrix3(
                lhs.m11 * s, lhs.m12 * s, lhs.m13 * s,
                lhs.m21 * s, lhs.m22 * s, lhs.m23 * s,
                lhs.m31 * s, lhs.m32 * s, lhs.m33 * s);
        }

        public static Vector3 operator *(Matrix3 lhs, Vector3 v)
        {
            return new Vector3(
                lhs.m11 * v.X + lhs.m12 * v.Y + lhs.m13 * v.Z,
                lhs.m21 * v.X + lhs.m22 * v.Y + lhs.m23 * v.Z,
                lhs.m31 * v.X + lhs.m32 * v.Y + lhs.m33 * v.Z);
        }

        public static Matrix3 operator /(Matrix3 lhs, float s)
        {
            float q = 1.0f / s;
            return new Matrix3(
                lhs.m11 * q, lhs.m12 * q, lhs.m13 * q,
                lhs.m21 * q, lhs.m22 * q, lhs.m23 * q,
                lhs.m31 * q, lhs.m32 * q, lhs.m33 * q);
        }

        #endregion

        #region Method

        public void Set(
            float n11, float n12, float n13,
            float n21, float n22, float n23,
            float n31, float n32, float n33)
        {
            m11 = n11; m12 = n12; m13 = n13;
            m21 = n21; m22 = n22; m23 = n23;
            m31 = n31; m32 = n32; m33 = n33;
        }

        public void SetIdentity()
        {
            m11 = m22 = m33 = 1.0f;
            m12 = m13 = 0.0f;
            m21 = m23 = 0.0f;
            m31 = m32 = 0.0f;
        }

        public void SetScaling(float sx, float sy, float sz)
        {
            SetIdentity();
            m11 = sx;
            m22 = sy;
            m33 = sz;
        }

        public void SetRotationX(float rad)
        {
            SetIdentity();
            float c = (float)Math.Cos(rad);
            float s = (float)Math.Sin(rad);
            m22 = c;
            m23 = -s;
            m32 = s;
            m33 = c;
        }

        public void SetRotationY(float rad)
        {
            SetIdentity();
            float c = (float)Math.Cos(rad);
            float s = (float)Math.Sin(rad);
            m11 = c;
            m13 = s;
            m31 = -s;
            m33 = c;
        }

        public void SetRotationZ(float rad)
        {
            SetIdentity();
            float c = (float)Math.Cos(rad);
            float s = (float)Math.Sin(rad);
            m11 = c;
            m12 = -s;
            m21 = s;
            m22 = c;
        }

        public void Transpose()
        {
            float t = m21; m21 = m12; m12 = t;
            t = m13; m13 = m31; m31 = t;
            t = m23; m23 = m32; m32 = t;
        }

        public Vector3 Transform(Vector3 v)
        {
            return new Vector3(
                m11 * v.X + m12 * v.Y + m13 * v.Z,
                m21 * v.X + m22 * v.Y + m23 * v.Z,
                m31 * v.X + m32 * v.Y + m33 * v.Z);
        }

        /// <summary>
        /// 行列式を求める
        /// </summary>
        /// <returns></returns>
        public float Determinant()
        {
            return (m11 * m22 * m33 + m12 * m23 * m31 + m13 * m32 * m21 - m11 * m32 * m23 - m12 * m21 * m33 - m31 * m22 * m13);
        }

        /// <summary>
        /// 逆行列を求める
        /// </summary>
        /// <returns></returns>
        public Matrix3 Inverse()
        {
            float d = Determinant();
            if (d == 0.0f)
            {
                d = 1.0f;
            }

            return new Matrix3(
                (m22 * m33 - m23 - m32) / d, -(m12 * m33 - m13 * m32) / d, (m12 * m23 - m13 * m22) / d,
                -(m21 * m33 - m23 * m31) / d, (m11 * m33 - m13 * m31) / d, -(m11 * m23 - m13 * m21) / d,
                (m21 * m32 - m22 * m31) / d, -(m11 * m32 - m12 * m31) / d, (m11 * m22 - m12 * m21) / d);
        }

        #endregion

    }
}
