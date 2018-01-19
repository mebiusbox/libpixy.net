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
using System.Drawing.Drawing2D;

namespace libpixy.net.Controls.Diagram
{
    /// <summary>
    /// コネクション描画（曲線）
    /// </summary>
    public class DrawCurvedConnection : IDrawConnection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="arrow"></param>
        public void Draw(Graphics g, Port p1, Port p2, bool arrow)
        {
            Draw(g, p1, p2, System.Drawing.Color.Black, 2, arrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="arrow"></param>
        public void Draw(Graphics g, Port p1, Port p2, Color c, int w, bool arrow)
        {
            int angle1 = GetAngle(p1);
            int angle2 = GetAngle(p2);
            libpixy.net.Controls.Diagram.Draw.DrawCurve(
                g,
                p1.ConnectionPoint, angle1,
                p2.ConnectionPoint, angle2,
                c,
                w,
                arrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="arrow"></param>
        public void Draw(Graphics g, Port p1, Point p2, bool arrow)
        {
            int angle1 = GetAngle(p1);
            int angle2 = GetAngle(p1, p2);
            libpixy.net.Controls.Diagram.Draw.DrawCurve(
                g,
                p1.ConnectionPoint, angle1,
                p2, angle2,
                System.Drawing.Color.Black,
                2, arrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="angle"></param>
        private int GetAngle(Port port)
        {
            int angle = 0;
            switch (port.Alignment)
            {
                case PortAlignment.Left:
                    angle = 180;
                    break;

                case PortAlignment.Top:
                    angle = 270;
                    break;

                case PortAlignment.Right:
                    angle = 0;
                    break;

                case PortAlignment.Bottom:
                    angle = 90;
                    break;
            }

            return angle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="angle"></param>
        private int GetAngle(Port p1, Point p2)
        {
            int angle = 0;
            int len = Math.Abs(p1.ConnectionPoint.X - p2.X);
            if (p1.ConnectionPoint.X < p2.X)
            {
                angle = 180;
            }
            else
            {
                angle = 0;
            }

            if (Math.Abs(p1.ConnectionPoint.Y - p2.Y) > len)
            {
                if (p1.ConnectionPoint.Y < p2.Y)
                {
                    angle = 270;
                }
                else
                {
                    angle = 90;
                }
            }

            return angle;
        }

        /// <summary>
        /// 交差判定
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        public bool Intersect(Port p1, Port p2, Rectangle rc)
        {
            int angle1 = GetAngle(p1);
            int angle2 = GetAngle(p2);

            libpixy.net.Vecmath.Vector2[] pt = new libpixy.net.Vecmath.Vector2[4];
            pt[0] = new libpixy.net.Vecmath.Vector2(p1.ConnectionPoint.X, p1.ConnectionPoint.Y);
            pt[3] = new libpixy.net.Vecmath.Vector2(p2.ConnectionPoint.X, p2.ConnectionPoint.Y);
            pt[1] = new libpixy.net.Vecmath.Vector2(pt[0].X, pt[0].Y);
            pt[2] = new libpixy.net.Vecmath.Vector2(pt[3].X, pt[3].Y);

            float dx = pt[0].X - pt[3].X;
            float dy = pt[0].Y - pt[3].Y;
            int len = (int)(Math.Sqrt(dx * dx + dy * dy) / 3.0);
            if (len > 100) len = 100;

            pt[1].X += (float)(Math.Cos((double)angle1 * Math.PI / 180.0) * (double)len);
            pt[1].Y += (float)(Math.Sin((double)angle1 * Math.PI / 180.0) * (double)len);
            pt[2].X += (float)(Math.Cos((double)angle2 * Math.PI / 180.0) * (double)len);
            pt[2].Y += (float)(Math.Sin((double)angle2 * Math.PI / 180.0) * (double)len);

            libpixy.net.Vecmath.Vector2 v1 = new libpixy.net.Vecmath.Vector2((float)rc.Left, (float)rc.Top);
            libpixy.net.Vecmath.Vector2 v2 = new libpixy.net.Vecmath.Vector2((float)rc.Right, (float)rc.Top);
            libpixy.net.Vecmath.Vector2 v3 = new libpixy.net.Vecmath.Vector2((float)rc.Right, (float)rc.Bottom);
            libpixy.net.Vecmath.Vector2 v4 = new libpixy.net.Vecmath.Vector2((float)rc.Left, (float)rc.Bottom);

            float t = 0.0f;
            libpixy.net.Vecmath.Vector2[] positions = libpixy.net.Vecmath.Bezeir.Eval(pt[0], pt[1], pt[2], pt[3], 8);
            for (int i=0; i<positions.Length-1; ++i)
            {
                if (libpixy.net.Vecmath.Geom.Test2DSegmentSegment(v1, v2, positions[i], positions[i + 1], out t) == 1)
                {
                    return true;
                }
                if (libpixy.net.Vecmath.Geom.Test2DSegmentSegment(v2, v3, positions[i], positions[i + 1], out t) == 1)
                {
                    return true;
                }
                if (libpixy.net.Vecmath.Geom.Test2DSegmentSegment(v3, v4, positions[i], positions[i + 1], out t) == 1)
                {
                    return true;
                }
                if (libpixy.net.Vecmath.Geom.Test2DSegmentSegment(v4, v1, positions[i], positions[i + 1], out t) == 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
