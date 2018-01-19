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
    public class Geom
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float DistanceToSegment(Vector2 p, Vector2 v1, Vector2 v2)
        {
            return (float)System.Math.Sqrt(DistanceToSegmentSq(p, v1, v2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float DistanceToSegmentSq(Vector2 p, Vector2 v1, Vector2 v2)
        {
#if false
            Vector2 w = p - v1;
            Vector2 t = v2 - v1;
            t.Normalize();
            float sn = Vector2.Dot(w,t);
            Vector2 pn = w - (t * sn);
            float d = pn.Length();
            float v1d = p.Distance(v1);
            float v2d = p.Distance(v2);
            if (d > v1d) { d = v1d; }
            if (d > v2d) { d = v2d; }
            return d;
#endif
            Vector2 ab = v2 - v1;
            Vector2 ac = p - v1;
            Vector2 bc = p - v2;
            float e = Vector2.Dot(ac, ab);
            if (e <= 0.0f)
            {
                return Vector2.Dot(ac, ac);
            }
            float f = Vector2.Dot(ab, ab);
            if (e >= f)
            {
                return Vector2.Dot(bc, bc);
            }
            return Vector2.Dot(ac, ac) - e * e / f;
        }

        /// <summary>
        /// 三角形の符号付面積の２倍を返す。結果は abc が反時計回りの場合に正
        /// abc が時計回りに場合に負、abc が縮退している場合にゼロ。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float Signed2DTriArea(Vector2 a, Vector2 b, Vector2 c)
        {
            return (a.X - c.X) * (b.Y - c.Y) - (a.Y - c.Y) * (b.X - c.X);
        }

        /// <summary>
        /// 線分 ab および cb が重なっているかどうかを判定。重なっていれば
        /// ab に沿った交差する t の値を返す
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int Test2DSegmentSegment(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out float t)
        {
            float a1 = Signed2DTriArea(a, b, d);
            float a2 = Signed2DTriArea(a, b, c);
            if (a1 * a2 < 0.0f)
            {
                float a3 = Signed2DTriArea(c, d, a);
                float a4 = a3 + a2 - a1;
                if (a3 * a4 < 0.0f)
                {
                    t = a3 / (a3 - a4);
                    return 1;
                }
            }

            t = 0.0f;
            return 0;
        }
    }
}
