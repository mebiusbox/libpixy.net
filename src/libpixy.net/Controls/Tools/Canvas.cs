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

namespace libpixy.net.Controls.Tool
{
    /// <summary>
    /// キャンバス。
    /// キャンバス座標系 - 拡大縮小、移動前の座標系
    /// ビュー座標系 - 拡大縮小、移動後の座標系
    /// </summary>
    public class Canvas
    {
        #region Field
        #endregion Field

        #region Properties

        public Point Origin { get; set; }
        public PointF Scale { get; set; }

        #endregion // Properties

        #region Public Methods

        /// <summary>
        /// Ctor
        /// </summary>
        public Canvas()
        {
            this.Origin = new Point(0, 0);
            this.Scale = new PointF(1.0f, 1.0f);
        }

        /// <summary>
        /// キャンバス座標系でのＸ座標値を求める
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int ToCanvasX(int x)
        {
            return (int)((float)x / this.Scale.X) - this.Origin.X;
        }

        /// <summary>
        /// キャンバス座標系でのＹ座標値を求める
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int ToCanvasY(int y)
        {
            return (int)((float)y / this.Scale.Y) - this.Origin.Y;
        }

        /// <summary>
        /// キャンバス座標系での幅を求める
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int ToCanvasWidth(int w)
        {
            return (int)((float)w / this.Scale.X);
        }

        /// <summary>
        /// キャンバス座標系での高さを求める
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int ToCanvasHeight(int h)
        {
            return (int)((float)h / this.Scale.Y);
        }

        /// <summary>
        /// ビュー座標系でのＸ座標値を求める
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int ToViewX(int x)
        {
            return (int)((float)(x + this.Origin.X) * this.Scale.X);
        }

        /// <summary>
        /// ビュー座標系でのＹ座標値を求める
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int ToViewY(int y)
        {
            return (int)((float)(y + this.Origin.Y) * this.Scale.Y);
        }

        /// <summary>
        /// ビュー座標系での幅を求める
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int ToViewWidth(int w)
        {
            return (int)((float)w * this.Scale.X);
        }

        /// <summary>
        /// ビュー座標系での高さを求める
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int ToViewHeight(int h)
        {
            return (int)((float)h * this.Scale.Y);
        }

        /// <summary>
        /// ビュー座標系からキャンバス座標系に変換する
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Point PointToCanvas(Point pt)
        {
            float x = (float)pt.X / this.Scale.X;
            float y = (float)pt.Y / this.Scale.Y;
            return new Point((int)x - this.Origin.X, (int)y - this.Origin.Y);
        }

        /// <summary>
        /// キャンバス座標系からビュー座標系に変換する
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Point PointToView(Point pt)
        {
            float x = (float)(pt.X + this.Origin.X) * this.Scale.X;
            float y = (float)(pt.Y + this.Origin.Y) * this.Scale.Y;
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// ビュー座標系からキャンバス座標系に変換する
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Size SizeToCanvas(Size pt)
        {
            float x = (float)pt.Width / this.Scale.X;
            float y = (float)pt.Height / this.Scale.Y;
            return new Size((int)x, (int)y);
        }

        /// <summary>
        /// キャンバス座標系からビュー座標系に変換する
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Size SizeToView(Size pt)
        {
            float x = (float)pt.Width * this.Scale.X;
            float y = (float)pt.Height * this.Scale.Y;
            return new Size((int)x, (int)y);
        }

        /// <summary>
        /// ビュー座標系からキャンバス座標系に変換する
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public Rectangle RectToCanvas(Rectangle rc)
        {
            Point lt = PointToCanvas(new Point(rc.X, rc.Y));
            Point rb = PointToCanvas(new Point(rc.Right, rc.Bottom));
            return new Rectangle(lt.X, lt.Y, rb.X - lt.X, rb.Y - lt.Y);
        }

        /// <summary>
        /// キャンバス座標系からビュー座標系に変換する
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public Rectangle RectToView(Rectangle rc)
        {
            Point lt = PointToView(new Point(rc.X, rc.Y));
            Point rb = PointToView(new Point(rc.Right, rc.Bottom));
            return new Rectangle(lt.X, lt.Y, rb.X - lt.X, rb.Y - lt.Y);
        }

        /// <summary>
        /// 座標値をスナップする
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static Point PointSnap(Point pt, int gridsize)
        {
            return new Point(Snap(pt.X, gridsize), Snap(pt.Y, gridsize));
        }

        /// <summary>
        /// スナップ
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gridsize"></param>
        /// <returns></returns>
        public static int Snap(int value, int gridsize)
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

                if (mod < gridsize / 2)
                {
                    return (value / gridsize) * gridsize;
                }
                else
                {
                    return (value / gridsize + 1) * gridsize;
                }
            }
        }

        /// <summary>
        /// pt を中心にズーム
        /// </summary>
        /// <param name="pt">ビュー座標系</param>
        public void Zoom(Point pt, float sx, float sy)
        {
            Point vpos1 = PointToCanvas(pt);
            this.Scale = new PointF(Math.Max(sx, 0.1f), Math.Max(sy, 0.1f));
            Point vpos2 = PointToCanvas(pt);
            Point origin = this.Origin;
            origin.Offset(vpos2.X - vpos1.X, vpos2.Y - vpos1.Y);
            this.Origin = origin;
        }

        /// <summary>
        /// pt を中心にズーム
        /// </summary>
        /// <param name="pt">ビュー座標系</param>
        /// <param name="scale">スケール</param>
        public void Zoom(Point pt, PointF scale)
        {
            Zoom(pt, scale.X, scale.Y);
        }

        /// <summary>
        /// フレーミング
        /// </summary>
        /// <param name="rcFrame"></param>
        /// <param name="rcClient"></param>
        public void Frame(Rectangle rcFrame, Rectangle rcClient)
        {
            Rectangle rcFrameCanvas = RectToCanvas(rcFrame);
            Rectangle rcClientCanvas = RectToCanvas(rcClient);

            Point center = PointToCanvas(libpixy.net.Vecmath.Utils.GetCenter(rcFrame));
            float ratioX = (float)rcClientCanvas.Width / (float)rcFrameCanvas.Width;
            float ratioY = (float)rcClientCanvas.Height / (float)rcFrameCanvas.Height;
            if (ratioX < ratioY)
            {
                this.Scale = new PointF(this.Scale.X * ratioX, this.Scale.Y * ratioX);
            }
            else
            {
                this.Scale = new PointF(this.Scale.X * ratioY, this.Scale.Y * ratioY);
            }

            Point newCenter = PointToCanvas(libpixy.net.Vecmath.Utils.GetCenter(rcClient));
            this.Origin = new Point(this.Origin.X - (center.X - newCenter.X), this.Origin.Y - (center.Y - newCenter.Y));
        }

        /// <summary>
        /// 移動、拡大縮小の値をリセットする
        /// </summary>
        public void Reset()
        {
            this.Origin = new Point(0, 0);
            this.Scale = new PointF(1.0f, 1.0f);
        }

        /// <summary>
        /// 変換行列をリセット
        /// </summary>
        /// <param name="g"></param>
        public void ResetTransform(Graphics g)
        {
            g.ResetTransform();
        }

        /// <summary>
        /// 変換行列を設定
        /// </summary>
        /// <param name="g"></param>
        public void SetTransform(Graphics g)
        {
            // 移動→拡大
            g.ResetTransform();
            g.TranslateTransform(
                this.Origin.X, this.Origin.Y,
                System.Drawing.Drawing2D.MatrixOrder.Append);
            g.ScaleTransform(
                this.Scale.X, this.Scale.Y,
                System.Drawing.Drawing2D.MatrixOrder.Append);
        }

        #endregion // Public Methods
    }
}
