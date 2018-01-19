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

namespace libpixy.net.Utils
{
    public class ColorUtils
    {
        /// <summary>
        /// アルファブレンド
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="alpha">0 .. 100</param>
        /// <returns></returns>
        public static Color Blend(Color c1, Color c2, int alpha)
        {
            // color = (1.0 - alpha) * c1 + alpha * c2
            int r = (100 - alpha) * (int)c1.R + alpha * (int)c2.R;
            int g = (100 - alpha) * (int)c1.G + alpha * (int)c2.G;
            int b = (100 - alpha) * (int)c1.B + alpha * (int)c2.B;
            return Color.FromArgb(r / 100, g / 100, b / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Color Middle(Color c1, Color c2)
        {
            return Color.FromArgb(
                (c1.R + c2.R) / 2,
                (c1.G + c2.G) / 2,
                (c1.B + c2.B) / 2);
        }

        /// <summary>
        /// 明度調整.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="bright"></param>
        /// <returns></returns>
        public static Color Bright(Color c, int bright)
        {
            double Y = (double)c.R * 0.299 + (double)c.G * 0.587 + (double)c.B * 0.114;
            double Cr = (double)c.R * 0.500 - (double)c.G * 0.419 - (double)c.B * 0.081;
            double Cb = (double)c.R * -0.169 - (double)c.G * 0.332 + (double)c.B * 0.500;
            Y = Y * (double)bright / 100.0;
            double R = Y + 1.402 * Cr;
            double G = Y - 0.714 * Cr - 0.344 * Cb;
            double B = Y + 1.772 * Cb;
            int r = System.Math.Min(System.Math.Max(Convert.ToInt32(R), 0), 255);
            int g = System.Math.Min(System.Math.Max(Convert.ToInt32(G), 0), 255);
            int b = System.Math.Min(System.Math.Max(Convert.ToInt32(B), 0), 255);
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 加算.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="bright"></param>
        /// <returns></returns>
        public static Color Sub(Color c1, Color c2)
        {
            int r = (c1.R < c2.R) ? 0 : c1.R - c2.R;
            int g = (c1.G < c2.G) ? 0 : c1.G - c2.G;
            int b = (c1.B < c2.B) ? 0 : c1.B - c2.B;
            return Color.FromArgb(r, g, b);
        }


        /// <summary>
        /// 減算.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="bright"></param>
        /// <returns></returns>
        public static Color Add(Color c1, Color c2)
        {
            int r = System.Math.Min(255, (int)c1.R + (int)c2.R);
            int g = System.Math.Min(255, (int)c1.G + (int)c2.G);
            int b = System.Math.Min(255, (int)c1.B + (int)c2.B);
            return Color.FromArgb(r, g, b);
        }
    }
}
