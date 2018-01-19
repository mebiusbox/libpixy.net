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
    /// コネクション描画（直線）
    /// </summary>
    public class DrawLineConnection : IDrawConnection
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
            int len = Math.Abs(p2.ConnectionPoint.Y - p1.ConnectionPoint.Y);
            int bump = (len > 10) ? 10 : len;

            Point dp1 = new Point(p1.ConnectionPoint.X, p1.ConnectionPoint.Y);
            Point dp2 = new Point(p2.ConnectionPoint.X, p2.ConnectionPoint.Y);
            switch (p1.Alignment)
            {
                case PortAlignment.Left:
                    dp1.X -= bump;
                    break;

                case PortAlignment.Top:
                    dp1.Y -= bump;
                    break;

                case PortAlignment.Right:
                    dp1.X += bump;
                    break;

                case PortAlignment.Bottom:
                    dp1.Y += bump;
                    break;
            }

            switch (p2.Alignment)
            {
                case PortAlignment.Left:
                    dp2.X -= bump;
                    break;

                case PortAlignment.Top:
                    dp2.Y -= bump;
                    break;

                case PortAlignment.Right:
                    dp2.X += bump;
                    break;

                case PortAlignment.Bottom:
                    dp2.Y += bump;
                    break;
            }

            using (Pen pen = new Pen(Brushes.Black, 2))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if ((p1.Alignment == PortAlignment.Left || p1.Alignment == PortAlignment.Right) &&
                    (p2.Alignment == PortAlignment.Top || p2.Alignment == PortAlignment.Bottom))
                {
                    Point q = new Point(dp2.X, dp1.Y);
                    g.DrawLine(pen, dp1, q);
                    g.DrawLine(pen, dp2, q);
                }
                else
                {
                    Point q1 = new Point(dp1.X, p1.ConnectionPoint.Y + len / 2);
                    Point q2 = new Point(dp2.X, p2.ConnectionPoint.Y - len / 2);
                    g.DrawLine(pen, dp1, q1);
                    g.DrawLine(pen, dp2, q2);
                    g.DrawLine(pen, q1, q2);
                }

                g.DrawLine(pen, p1.ConnectionPoint, dp1);

                if (arrow)
                {
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5.0f, 5.0f);
                }

                g.DrawLine(pen, p2.ConnectionPoint, dp2);
                g.SmoothingMode = SmoothingMode.Default;
            }
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
            int len = Math.Abs(p2.Y - p1.ConnectionPoint.Y);
            int bump = (len > 10) ? 10 : len;
            Point dp1 = new Point(p1.ConnectionPoint.X, p1.ConnectionPoint.Y);
            Point dp2 = new Point(p2.X, p2.Y);
            switch (p1.Alignment)
            {
                case PortAlignment.Left:
                    dp1.X -= bump;
                    break;

                case PortAlignment.Top:
                    dp1.Y -= bump;
                    break;

                case PortAlignment.Right:
                    dp1.X += bump;
                    break;

                case PortAlignment.Bottom:
                    dp1.Y += bump;
                    break;
            }

            using (Pen pen = new Pen(Brushes.Black, 2))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if (p1.Alignment == PortAlignment.Left || p1.Alignment == PortAlignment.Right)
                {
                    Point q = new Point(dp2.X, dp1.Y);
                    g.DrawLine(pen, dp1, q);
                    g.DrawLine(pen, dp2, q);
                }
                else
                {
                    Point q1 = new Point(dp1.X, p1.ConnectionPoint.Y + len / 2);
                    Point q2 = new Point(dp2.X, p2.Y - len / 2);
                    g.DrawLine(pen, dp1, q1);
                    g.DrawLine(pen, dp2, q2);
                    g.DrawLine(pen, q1, q2);
                }

                g.DrawLine(pen, p1.ConnectionPoint, dp1);

                if (arrow)
                {
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5.0f, 5.0f);
                }

                g.DrawLine(pen, p2, dp2);
                g.SmoothingMode = SmoothingMode.Default;
            }
        }

        public bool Intersect(Port p1, Port p2, Rectangle rc)
        {
            // 未実装
            System.Diagnostics.Debug.Assert(false);
            return false;
        }
    }
}