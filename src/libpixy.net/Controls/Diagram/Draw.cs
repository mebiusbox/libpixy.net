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
    public class Draw
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="p"></param>
        public static void DrawRoundedRectangle(Graphics g, Rectangle r, int d, Pen p)
        {
            System.Drawing.Drawing2D.SmoothingMode mode = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawArc(p, r.X, r.Y, d, d, 180, 90);
            g.DrawLine(p, r.X + d / 2, r.Y, r.X + r.Width - d / 2, r.Y);
            g.DrawArc(p, r.X + r.Width - d, r.Y, d, d, 270, 90);
            g.DrawLine(p, r.X, r.Y + d / 2, r.X, r.Y + r.Height - d / 2);
            g.DrawLine(p, r.X + r.Width, r.Y + d / 2, r.X + r.Width, r.Y + r.Height - d / 2);
            g.DrawLine(p, r.X + d / 2, r.Y + r.Height, r.X + r.Width - d / 2, r.Y + r.Height);
            g.DrawArc(p, r.X, r.Y + r.Height - d, d, d, 90, 90);
            g.DrawArc(p, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            g.SmoothingMode = mode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="c"></param>
        /// <param name="width"></param>
        public static void DrawRoundedRectangle(Graphics g, Rectangle r, int d, Color c, int width)
        {
            using (Pen pen = new Pen(c, width))
            {
                DrawRoundedRectangle(g, r, d, pen);
            }
        }

        /// <summary>
        /// 角丸四角形を描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="b"></param>
        public static void FillRoundedRectangle(Graphics g, Rectangle r, int d, Brush b)
        {
            // anti alias distorts fill so remove it.
            System.Drawing.Drawing2D.SmoothingMode mode = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.FillPie(b, r.X, r.Y, d, d, 180, 90);
            g.FillPie(b, r.X + r.Width - d, r.Y, d, d, 270, 90);
            g.FillPie(b, r.X, r.Y + r.Height - d, d, d, 90, 90);
            g.FillPie(b, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            g.FillRectangle(b, r.X + d / 2, r.Y, r.Width - d, d / 2);
            g.FillRectangle(b, r.X, r.Y + d / 2, r.Width, r.Height - d);
            g.FillRectangle(b, r.X + d / 2, r.Y + r.Height - d / 2, r.Width - d, d / 2);
            g.SmoothingMode = mode;
        }

        /// <summary>
        /// 角丸四角形を描画（d が小さい場合の特別バージョン）
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="b"></param>
        public static void FillRoundedRectangleSmall(Graphics g, Rectangle r, int d, Brush b)
        {
            using (Pen pen = new Pen(b, 1))
            {
                int x1 = r.X;
                int y1 = r.Y;
                int x2 = r.Right-1;
                int y2 = r.Bottom-1;

                for (int i = 0; i < d; ++i)
                {
                    g.DrawLine(pen, x1 + d - i, y1 + i, x2 - (d - i), y1 + i);
                    g.DrawLine(pen, x1 + d - i, y2 - i, x2 - (d - i), y2 - i);
                }

                g.FillRectangle(b, x1, y1+d, r.Width, r.Height - d*2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="c"></param>
        public static void FillRoundedRectangle(Graphics g, Rectangle r, int d, Color c)
        {
            using (SolidBrush brush = new SolidBrush(c))
            {
                FillRoundedRectangle(g, r, d, brush);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="d"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="angle"></param>
        public static void GradientRoundedRectangle(Graphics g, Rectangle r, int d, Color c1, Color c2, float angle)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(r, c1, c2, angle))
            {
                FillRoundedRectangle(g, r, d, brush);
            }
        }

        /// <summary>
        /// 吹き出し描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="p"></param>
        /// <param name="w"></param>
        /// <param name="c"></param>
        public static void DrawBalloon(Graphics g, Rectangle r, Point p, int w, Color c)
        {
            using (Brush brush = new SolidBrush(c))
            {
                g.FillRectangle(brush, r);

                Point[] pt = new Point[3];
                pt[0] = new Point(p.X - w / 2, r.Bottom - 1);
                pt[1] = p;
                pt[2] = new Point(p.X + w / 2, r.Bottom - 1);

                System.Drawing.Drawing2D.SmoothingMode mode = g.SmoothingMode;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillPolygon(brush, pt);
                g.SmoothingMode = mode;
            }
        }

        /// <summary>
        /// 吹き出し描画（境界線付き）
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="p"></param>
        /// <param name="w"></param>
        /// <param name="c"></param>
        /// <param name="bw"></param>
        /// <param name="bc"></param>
        public static void DrawBalloon(Graphics g, Rectangle r, Point p, int w, Color c, int bw, Color bc)
        {
            using (Brush brush = new SolidBrush(bc))
            {
                Rectangle br = new Rectangle(r.Location, r.Size);
                br.Inflate(bw, bw);
                g.FillRectangle(brush, br);

                Point[] pt = new Point[3];
                pt[0] = new Point(p.X - w / 2 - bw, r.Bottom - 1);
                pt[1] = new Point(p.X, p.Y + bw);
                pt[2] = new Point(p.X + w / 2 + bw, r.Bottom - 1);

                System.Drawing.Drawing2D.SmoothingMode mode = g.SmoothingMode;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillPolygon(brush, pt);
                g.SmoothingMode = mode;
            }

            DrawBalloon(g, r, p, w, c);
        }

        /// <summary>
        /// 吹き出し描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="p"></param>
        /// <param name="w"></param>
        /// <param name="c"></param>
        public static void DrawRoundedBalloon(Graphics g, Rectangle r, Point p, int w, Color c, int d)
        {
            using (Brush brush = new SolidBrush(c))
            {
                if (d < 3)
                {
                    FillRoundedRectangleSmall(g, r, d, brush);
                }
                else
                {
                    FillRoundedRectangle(g, r, d, brush);
                }

                Point[] pt = new Point[3];
                pt[0] = new Point(p.X - w / 2, r.Bottom - 1);
                pt[1] = p;
                pt[2] = new Point(p.X + w / 2, r.Bottom - 1);

                System.Drawing.Drawing2D.SmoothingMode mode = g.SmoothingMode;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillPolygon(brush, pt);
                g.SmoothingMode = mode;
            }
        }

        /// <summary>
        /// 吹き出し描画（境界線付き）
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="p"></param>
        /// <param name="w"></param>
        /// <param name="c"></param>
        /// <param name="bw"></param>
        /// <param name="bc"></param>
        public static void DrawRoundedBalloon(Graphics g, Rectangle r, Point p, int w, Color c, int d, int bw, Color bc)
        {
            using (Brush brush = new SolidBrush(bc))
            {
                Rectangle br = new Rectangle(r.Location, r.Size);
                br.Inflate(bw, bw);

                if (d < 3)
                {
                    FillRoundedRectangleSmall(g, br, d, brush);
                }
                else
                {
                    FillRoundedRectangle(g, br, d, brush);
                }

                Point[] pt = new Point[3];
                pt[0] = new Point(p.X - w / 2 - bw, r.Bottom - 1);
                pt[1] = new Point(p.X, p.Y + bw);
                pt[2] = new Point(p.X + w / 2 + bw, r.Bottom - 1);

                System.Drawing.Drawing2D.SmoothingMode mode = g.SmoothingMode;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillPolygon(brush, pt);
                g.SmoothingMode = mode;
            }

            DrawRoundedBalloon(g, r, p, w, c, d);
        }

        /// <summary>
        /// 
        /// </summary>
        public enum CurveDir
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        public static void DrawCurve(Graphics gfx, Point pt1, Point pt2, Color clr, int width, CurveDir dir)
        {
            Point[] pt = new Point[4];
            pt[0] = pt1;
            pt[3] = pt2;
            pt[1] = pt[0];
            pt[2] = pt[3];

            int dx = pt2.X - pt1.X;
            int dy = pt2.Y - pt1.Y;
            int len = (int)(Math.Sqrt(dx * dx + dy * dy) / 3.0);
            if (len > 100) len = 100;

            if (dir == CurveDir.Horizontal)
            {
                pt[1].Offset(len, 0);
                pt[2].Offset(-len, 0);
            }
            else
            {
                pt[1].Offset(0, len);
                pt[2].Offset(0, -len);
            }

            using (Pen penArrow = new Pen(clr, width))
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.DrawBeziers(penArrow, pt);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        public static void DrawCurveAndArrow(
            Graphics gfx, Point pt1, Point pt2, Color clr, int width, CurveDir dir)
        {
            Point[] pt = new Point[4];
            pt[0] = pt1;
            pt[3] = pt2;
            pt[1] = pt[0];
            pt[2] = pt[3];

            int dx = pt2.X - pt1.X;
            int dy = pt2.Y - pt1.Y;
            int len = (int)(Math.Sqrt(dx * dx + dy * dy) / 3.0);
            if (len > 100) len = 100;

            if (dir == CurveDir.Horizontal)
            {
                pt[1].Offset(len, 0);
                pt[2].Offset(-len, 0);
            }
            else
            {
                pt[1].Offset(0, len);
                pt[2].Offset(0, -len);
            }

            using (Pen penArrow = new Pen(clr, width))
            {
                penArrow.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                penArrow.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5.0f, 5.0f);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.DrawBeziers(penArrow, pt);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        public static void DrawCurve(
            Graphics gfx, Point pt1, int angle1, Point pt2, int angle2, Color clr, int width, bool arrow)
        {
            Point[] pt = new Point[4];
            pt[0] = pt1;
            pt[3] = pt2;
            pt[1] = pt[0];
            pt[2] = pt[3];

            int dx = pt2.X - pt1.X;
            int dy = pt2.Y - pt1.Y;
            int len = (int)(Math.Sqrt(dx * dx + dy * dy) / 3.0);
            if (len > 100) len = 100;

            pt[1].X += (int)(Math.Cos((double)angle1 * Math.PI / 180.0) * (double)len);
            pt[1].Y += (int)(Math.Sin((double)angle1 * Math.PI / 180.0) * (double)len);
            pt[2].X += (int)(Math.Cos((double)angle2 * Math.PI / 180.0) * (double)len);
            pt[2].Y += (int)(Math.Sin((double)angle2 * Math.PI / 180.0) * (double)len);

            using (Pen penArrow = new Pen(clr, width))
            {
                if (arrow)
                {
                    penArrow.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    penArrow.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5.0f, 5.0f);
                }

                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.DrawBeziers(penArrow, pt);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        public static void DrawConnection(Graphics g, Point pt1, Point pt2, CurveDir dir)
        {
            DrawCurve(g, pt1, pt2, Color.Black, 2, dir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="port"></param>
        public static void DrawPortCircle(Graphics g, Port port)
        {
            int radius = 5;
            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(port.PortColor),
                    port.ConnectionPoint.X - radius,
                    port.ConnectionPoint.Y - radius,
                    radius * 2, radius * 2);
                g.DrawEllipse(pen,
                    port.ConnectionPoint.X - radius,
                    port.ConnectionPoint.Y - radius,
                    radius * 2, radius * 2);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
        }
    }
}
