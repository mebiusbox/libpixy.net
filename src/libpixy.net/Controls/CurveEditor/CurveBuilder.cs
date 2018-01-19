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
using libpixy.net.Vecmath;

namespace libpixy.net.Controls.CurveEditor
{
    /// <summary>
    /// 
    /// </summary>
    public class CurveBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="i"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] Build(CurveContent content, int i, int resolution)
        {
            if (i < content.Items.Count)
            {
                if (content.Items[i].Curve != null)
                {
                    switch (content.Items[i].Curve.Type)
                    {
                        case CurveType.Constant:
                            return BuildConstant(content, i, resolution);
                            
                        case CurveType.Linear:
                            return BuildLinear(content, i, resolution);

                        case CurveType.CutmallRom:
                            return BuildCutmallRom(content, i, resolution);

                        case CurveType.Hermite:
                            return BuildHermite(content, i, resolution);

                        case CurveType.Bezeir:
                            return BuildBezeir(content, i, resolution);

                        case CurveType.BSpline:
                            return BuildBSpline(content, i, resolution);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="i"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] BuildConstant(CurveContent content, int i, int resolution)
        {
            int count = content.Items.Count;
            int i1 = i;
            int i2 = (i + 1 < count) ? i + 1 : count - 1;
            Vector2[] ret = new Vector2[2];
            ret[0] = new Vector2(content.Items[i1].Position.X, content.Items[i1].Position.Y);
            ret[1] = new Vector2(content.Items[i2].Position.X, content.Items[i1].Position.Y);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="i"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] BuildLinear(CurveContent content, int i, int resolution)
        {
            int count = content.Items.Count;
            int i1 = i;
            int i2 = (i + 1 < count) ? i + 1 : count - 1;
            Vector2[] ret = new Vector2[2];
            ret[0] = new Vector2(content.Items[i1].Position.X, content.Items[i1].Position.Y);
            ret[1] = new Vector2(content.Items[i2].Position.X, content.Items[i2].Position.Y);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="i"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] BuildCutmallRom(CurveContent content, int i, int resolution)
        {
            int count = content.Items.Count;
            int i0 = (i > 0) ? i - 1 : 0;
            int i1 = i;
            int i2 = (i + 1 < count) ? i + 1 : count - 1;
            int i3 = (i + 2 < count) ? i + 2 : count - 1;
            return Vecmath.CatmullRom.Eval(
                    content.Items[i0].Position,
                    content.Items[i1].Position,
                    content.Items[i2].Position,
                    content.Items[i3].Position,
                    resolution);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="i"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] BuildHermite(CurveContent content, int i, int resolution)
        {
            int count = content.Items.Count;
            int i1 = i;
            int i2 = (i + 1 < count) ? i + 1 : count - 1;
            return Vecmath.Hermite.Eval(
                    content.Items[i1].Position,
                    content.Items[i1].Handle1Position,
                    content.Items[i2].Position,
                    content.Items[i2].Handle2Position,
                    resolution);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="i"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] BuildBezeir(CurveContent content, int i, int resolution)
        {
            int count = content.Items.Count;
            int i0 = i;
            int i1 = (i + 1 < count) ? i + 1 : count - 1;
            return Vecmath.Bezeir.Eval(
                    content.Items[i0].Position,
                    content.Items[i0].Handle1Position,
                    content.Items[i1].Handle2Position,
                    content.Items[i1].Position,
                    resolution);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="i"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static Vector2[] BuildBSpline(CurveContent content, int i, int resolution)
        {
            int count = content.Items.Count;
            int i0 = i;
            int i1 = (i + 1 < count) ? i + 1 : count - 1;
            int i2 = (i + 2 < count) ? i + 2 : count - 1;
            int i3 = (i + 3 < count) ? i + 3 : count - 1;
            return Vecmath.BSpline.Eval(
                    content.Items[i0].Position,
                    content.Items[i1].Position,
                    content.Items[i2].Position,
                    content.Items[i3].Position,
                    resolution);
        }
    }
}
