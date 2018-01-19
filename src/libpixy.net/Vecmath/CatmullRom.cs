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
    public class CatmullRom
    {
        /// <summary>
        /// スプライン曲線(Catmull-Ram曲線)
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Eval(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            Vector2 v0 = (p2 - p0) * 0.5f;
            Vector2 v1 = (p3 - p1) * 0.5f;
            float t2 = t * t;
            float t3 = t2 * t;
            return (2.0f * p1 - 2.0f * p2 + v0 + v1) * t3 + (-3.0f * p1 + 3.0f * p2 - 2.0f * v0 - v1) * t2 + v0 * t + p1;
        }

        /// <summary>
        /// スプライン曲線(Catmull-Ram曲線)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Eval(Vector2[] p, float t)
        {
            return Eval(p[0], p[1], p[2], p[3], t);
        }

        /// <summary>
        /// スプライン曲線(Catmull-Ram曲線)
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] Eval(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int resolution)
        {
            Vector2 v0 = (p2 - p0) * 0.5f;
            Vector2 v1 = (p3 - p1) * 0.5f;
            Vector2[] vret = new Vector2[resolution + 1];
            float t1;
            float t2;
            float t3;
            for (int i = 1; i < resolution; ++i)
            {
                t1 = (float)i / (float)resolution;
                t2 = t1 * t1;
                t3 = t2 * t1;
                vret[i] = (2.0f * p1 - 2.0f * p2 + v0 + v1) * t3 + (-3.0f * p1 + 3.0f * p2 - 2.0f * v0 - v1) * t2 + v0 * t1 + p1;
            }

            vret[0] = p1;
            vret[resolution] = p2;

            return vret;
        }

        /// <summary>
        /// スプライン曲線(Catmull-Ram曲線)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] Eval(Vector2[] p, int resolution)
        {
            return Eval(p[0], p[1], p[2], p[3], resolution);
        }

        /// <summary>
        /// スプライン曲線(Catmull-Ram曲線)
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Eval(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Vector3 v0 = (p2 - p0) * 0.5f;
            Vector3 v1 = (p3 - p1) * 0.5f;
            float t2 = t * t;
            float t3 = t2 * t;
            return (2.0f * p1 - 2.0f * p2 + v0 + v1) * t3 + (-3.0f * p1 + 3.0f * p2 - 2.0f * v0 - v1) * t2 + v0 * t + p1;
        }

        /// <summary>
        /// スプライン曲線(Catmull-Ram曲線)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Eval(Vector3[] p, float t)
        {
            return Eval(p[0], p[1], p[2], p[3], t);
        }

        /// <summary>
        /// スプライン曲線(Catmull-Ram曲線)
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector3[] Eval(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int resolution)
        {
            Vector3 v0 = (p2 - p0) * 0.5f;
            Vector3 v1 = (p3 - p1) * 0.5f;
            Vector3[] vret = new Vector3[resolution + 1];
            float t1;
            float t2;
            float t3;
            for (int i = 1; i < resolution; ++i)
            {
                t1 = (float)i / (float)resolution;
                t2 = t1 * t1;
                t3 = t2 * t1;
                vret[i] = (2.0f * p1 - 2.0f * p2 + v0 + v1) * t3 + (-3.0f * p1 + 3.0f * p2 - 2.0f * v0 - v1) * t2 + v0 * t1 + p1;
            }

            vret[0] = p1;
            vret[resolution] = p2;

            return vret;
        }

        /// <summary>
        /// スプライン曲線(Catmull-Ram曲線)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector3[] Eval(Vector3[] p, int resolution)
        {
            return Eval(p[0], p[1], p[2], p[3], resolution);
        }
    }
}
