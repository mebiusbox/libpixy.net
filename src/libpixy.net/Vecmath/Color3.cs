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
    public class Color3
    {
        #region Fields

        public float R;
        public float G;
        public float B;

        #endregion//Fields

        #region Properties
        #endregion//Properties

        #region Attributes
        #endregion//Attributes

        #region Constructor/Destructor

        public Color3()
        {
        }

        public Color3(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        #endregion//Constructor/Destructor

        #region Convert

        /// <summary>
        /// System.Drawing.Color 形式に変換
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb(
                (byte)(R * 255.0f),
                (byte)(G * 255.0f),
                (byte)(B * 255.0f));
        }

        #endregion Convert

        #region Static
        public static readonly Color3 White = new Color3(1.0f, 1.0f, 1.0f);
        public static readonly Color3 Black = new Color3(0.0f, 0.0f, 0.0f);
        public static readonly Color3 Red = new Color3(1.0f, 0.0f, 0.0f);
        public static readonly Color3 Green = new Color3(0.0f, 1.0f, 0.0f);
        public static readonly Color3 Blue = new Color3(0.0f, 0.0f, 1.0f);
        #endregion Static

        #region Overload operators

        public static Color3 operator +(Color3 lhs, Color3 rhs)
        {
            return new Color3(lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B);
        }

        public static Color3 operator -(Color3 lhs, Color3 rhs)
        {
            return new Color3(lhs.R-rhs.R, lhs.G-rhs.G, lhs.B - rhs.B);
        }

        public static Color3 operator *(Color3 lhs, float s)
        {
            return new Color3(lhs.R *s, lhs.G *s, lhs.B*s);
        }

        public static Color3 operator *(float s, Color3 rhs)
        {
            return new Color3(rhs.R * s, rhs.G * s, rhs.B * s);
        }

        public static Color3 operator /(Color3 lhs, float s)
        {
            float oneOverA = 1.0f / s;
            return new Color3(lhs.R * oneOverA, lhs.G * oneOverA, lhs.B * oneOverA);
        }

        #endregion//Overload opertors

        #region Methods

        public void Set(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public void Clamp(float min, float max)
        {
            if (R < min) R = min;
            if (R > max) R = max;
            if (G < min) G = min;
            if (G > max) G = max;
            if (B < min) B = min;
            if (B > max) B = max;
        }

        public void Satulate()
        {
            Clamp(0.0f, 1.0f);
        }

        public void AlphaBlend(Color3 c, float alpha)
        {
            AlphaBlend(this, c, alpha);
        }

        public void AlphaBlend(Color3 c1, Color3 c2, float alpha)
        {
            Set((c1.R * (1.0f - alpha)) + (c2.R * alpha),
                (c1.G * (1.0f - alpha)) + (c2.G * alpha),
                (c1.B * (1.0f - alpha)) + (c2.B * alpha));
        }

        #endregion
    }
}
