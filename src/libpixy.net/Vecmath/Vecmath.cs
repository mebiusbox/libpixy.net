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
    public class Constant
    {
        public const float kPi = 3.14159265f;
        public const float k2Pi = kPi*2.0f;
        public const float kPiOver2 = kPi/2.0f;
        public const float kOneOverPi = 1.0f / kPi;
        public const float kOneOver2Pi = 1.0f / k2Pi;
        public const float kPiOver180 = kPi / 180.0f;
        public const float k180OverPi = 180.0f / kPi;
    };

    public static class Utils
    {
        public static float WrapPi(float theta)
        {
            theta += Constant.kPi;
            theta -= (float)Math.Floor(theta * Constant.kOneOver2Pi);
            theta -= Constant.kPi;
            return theta;
        }

        public static float SafeAcos(float x)
        {
            // Check limit conditions
            if (x <= -1.0f)
            {
                return Constant.kPi;
            }
            if (x >= 1.0f)
            {
                return 0.0f;
            }

            return (float)Math.Acos(x);
        }

        public static float DegToRad(float deg)
        {
            return deg * Constant.kPiOver180;
        }

        public static float RadToDeg(float rad)
        {
            return rad * Constant.k180OverPi;
        }

        public static float FovToZoom(float fov)
        {
            return 1.0f / (float)Math.Tan(fov * 0.5f);
        }

        public static float ZoomToFov(float zoom)
        {
            return 2.0f * (float)Math.Atan(1.0f / zoom);
        }

        public static void Swap(ref int a, ref int b)
        {
            int t = a;
            a = b;
            b = t;
        }

        public static void Swap(ref float a, ref float b)
        {
            float t = a;
            a = b;
            b = t;
        }

        public static void Swap(ref double a, ref double b)
        {
            double t = a;
            a = b;
            b = t;
        }

        public static Point GetCenter(Rectangle rc)
        {
            return new Point(rc.X + rc.Width / 2, rc.Y + rc.Height / 2);
        }

        public static int RoundUp(int value, int x)
        {
            if (x <= 1)
            {
                return value;
            }
            else
            {
                if (value % x == 0)
                {
                    return value;
                }
                else
                {
                    return (value / x + 1) * x;
                }
            }
        }

        public static int RoundDown(int value, int x)
        {
            if (x <= 1)
            {
                return value;
            }
            else
            {
                if (value % x == 0)
                {
                    return value;
                }
                else
                {
                    return (value / x - 1) * x;
                }
            }
        }

        public static int SnapGrid(int value, int gridsize)
        {
            if (gridsize <= 1)
            {
                return value;
            }
            else
            {
                int mod = Math.Abs(value % gridsize);
                if (mod == 0)
                {
                    return value;
                }

                if (value < 0)
                {
                    return gridsize * (value / gridsize - 1);
                }
                else
                {
                    return gridsize * (value / gridsize + 1);
                }
            }
        }

        public static string GetStringFromStack(Stack<string> stack)
        {
            string[] strArray = stack.ToArray();
            if (strArray.Length > 0)
            {
                string ret = strArray[0];
                for (int i = 1; i < strArray.Length; ++i)
                {
                    ret = strArray[i] + "." + ret;
                }
                return ret;
            }

            return "";
        }

        public static Color ColorModulate(Color c1, Color c2)
        {
            return Color.FromArgb(
                c1.A * c2.A / 255,
                c1.R * c2.R / 255,
                c1.G * c2.G / 255,
                c1.B * c2.B / 255);
        }

        public static void RaiseEvent(EventHandler ev, object sender)
        {
            if (ev != null)
            {
                ev(sender, EventArgs.Empty);
            }
        }

        public static void RaiseEvent(EventHandler ev, object sender, EventArgs e)
        {
            if (ev != null)
            {
                ev(sender, e);
            }
        }
    }
}
