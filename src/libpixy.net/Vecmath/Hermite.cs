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
    public class Hermite
    {
        /// <summary>
        /// エルミート曲線
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Eval(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;
            float b1 = 2 * t3 - 3 * t2 + 1;
            float b2 = t3 - 2 * t2 + t;
            float b3 = -2 * t3 + 3 * t2;
            float b4 = t3 - t2;
            return new Vector2(
                p0.X * b1 + p1.X * b2 + p2.X * b3 + p3.X * b4,
                p0.Y * b1 + p1.Y * b2 + p2.Y * b3 + p3.Y * b4);
        }

        /// <summary>
        /// エルミート曲線
        /// </summary>
        /// <param name="p"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Eval(Vector2[] p, float t)
        {
            return Eval(p[0], p[1], p[2], p[3], t);
        }

        /// <summary>
        /// エルミート曲線
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] Eval(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int resolution)
        {
            Vector2[] vret = new Vector2[resolution + 1];
            float t1, t2, t3;
            float b1, b2, b3, b4;
            for (int i = 0; i <= resolution; ++i)
            {
                t1 = (float)i / (float)resolution;
                t2 = t1 * t1;
                t3 = t2 * t1;
                b1 = 2 * t3 - 3 * t2 + 1;
                b2 = t3 - 2 * t2 + t1;
                b3 = -2 * t3 + 3 * t2;
                b4 = t3 - t2;
                vret[i] = new Vector2();
                vret[i].X = p0.X * b1 + p1.X * b2 + p2.X * b3 + p3.X * b4;
                vret[i].Y = p0.Y * b1 + p1.Y * b2 + p2.Y * b3 + p3.Y * b4;
            }

            return vret;
        }

        /// <summary>
        /// エルミート曲線
        /// </summary>
        /// <param name="p"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] Eval(Vector2[] p, int resolution)
        {
            return Eval(p[0], p[1], p[2], p[3], resolution);
        }
    }
}
