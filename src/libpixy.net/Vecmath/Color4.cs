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
    /// RGBA
    /// </summary>
    public class Color4
    {
        #region Fields

        public float R;
        public float G;
        public float B;
        public float A;

        #endregion//Fields

        #region Properties
        #endregion//Properties

        #region Attributes
        #endregion//Attributes

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Color4()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public Color4(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c"></param>
        public Color4(System.Drawing.Color c)
        {
            Set(c);
        }

        #endregion Constructor, Destructor

        #region Convert

        /// <summary>
        /// System.Drawing.Color 形式に変換
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb(
                (byte)(A * 255.0f),
                (byte)(R * 255.0f),
                (byte)(G * 255.0f),
                (byte)(B * 255.0f));
        }

        /// <summary>
        /// System.Drawing.Color 形式に変換
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color ToRgbColor()
        {
            return System.Drawing.Color.FromArgb(
                255,
                (byte)(R * 255.0f),
                (byte)(G * 255.0f),
                (byte)(B * 255.0f));
        }

        #endregion

        #region Constants
        public static Color4 White { get { return new Color4(1.0f, 1.0f, 1.0f, 1.0f); } }
        public static Color4 Black { get { return new Color4(0.0f, 0.0f, 0.0f, 1.0f); } }
        public static Color4 Red { get { return new Color4(1.0f, 0.0f, 0.0f, 1.0f); } }
        public static Color4 Green { get { return new Color4(0.0f, 1.0f, 0.0f, 1.0f); } }
        public static Color4 Blue { get { return new Color4(0.0f, 0.0f, 1.0f, 1.0f); } }
        public static Color4 Zero { get { return new Color4(0.0f, 0.0f, 0.0f, 0.0f); } }
        #endregion Constants

        #region Static

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Color4 From(float r, float g, float b, float a)
        {
            return new Color4(r, g, b, a);
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Color4 From(Color4 c)
        {
            return new Color4(c.R, c.G, c.B, c.A);
        }

        #endregion Static

        #region Overload operators

        public static Color4 operator +(Color4 lhs, Color4 rhs)
        {
            return new Color4(lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B, lhs.A + rhs.A);
        }

        public static Color4 operator -(Color4 lhs, Color4 rhs)
        {
            return new Color4(lhs.R-rhs.R, lhs.G-rhs.G, lhs.B - rhs.B, lhs.A - rhs.A);
        }

        public static Color4 operator *(Color4 lhs, Color4 rhs)
        {
            return new Color4(lhs.R * rhs.R, lhs.G * rhs.G, lhs.B * rhs.B, lhs.A * rhs.A);
        }

        public static Color4 operator *(Color4 lhs, float s)
        {
            return new Color4(lhs.R *s, lhs.G *s, lhs.B*s, lhs.A*s);
        }

        public static Color4 operator *(float s, Color4 rhs)
        {
            return new Color4(rhs.R * s, rhs.G * s, rhs.B * s, rhs.A * s);
        }

        public static Color4 operator /(Color4 lhs, float s)
        {
            float oneOverA = 1.0f / s;
            return new Color4(lhs.R * oneOverA, lhs.G * oneOverA, lhs.B * oneOverA, lhs.A * oneOverA);
        }

        #endregion//Overload opertors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public void Set(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public void Set(System.Drawing.Color c)
        {
            R = (float)c.R / 255.0f;
            G = (float)c.G / 255.0f;
            B = (float)c.B / 255.0f;
            A = (float)c.A / 255.0f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Clamp(float min, float max)
        {
            if (R < min) R = min;
            if (R > max) R = max;
            if (G < min) G = min;
            if (G > max) G = max;
            if (B < min) B = min;
            if (B > max) B = max;
            if (A < min) A = min;
            if (A > max) A = max;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Satulate()
        {
            Clamp(0.0f, 1.0f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="alpha"></param>
        public void AlphaBlend(Color4 c, float alpha)
        {
            AlphaBlend(this, c, alpha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="alpha"></param>
        public void AlphaBlend(Color4 c1, Color4 c2, float alpha)
        {
            Set((c1.R * (1.0f - alpha)) + (c2.R * alpha),
                (c1.G * (1.0f - alpha)) + (c2.G * alpha),
                (c1.B * (1.0f - alpha)) + (c2.B * alpha), 1.0f);
        }

        /// <summary>
        /// 線型補間
        /// </summary>
        /// <param name="c"></param>
        /// <param name="t"></param>
        public void Leap(Color4 c, float t)
        {
            Leap(this, c, t);
        }

        /// <summary>
        /// 線型補間
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="t"></param>
        public static Color4 Leap(Color4 c1, Color4 c2, float t)
        {
            return new libpixy.net.Vecmath.Color4(
                (c2.R - c1.R) * t + c1.R,
                (c2.G - c1.G) * t + c1.G,
                (c2.B - c1.B) * t + c1.B,
                (c2.A - c1.A) * t + c1.A);
        }

        /// <summary>
        /// クローン
        /// </summary>
        /// <returns></returns>
        public Color4 Clone()
        {
            return new Color4(R, G, B, A);
        }

        #endregion
    }
}
