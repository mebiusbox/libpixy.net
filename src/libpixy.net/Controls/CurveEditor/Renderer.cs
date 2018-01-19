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
using libpixy.net.Vecmath;

namespace libpixy.net.Controls.CurveEditor
{
    /// <summary>
    /// レンダラー
    /// </summary>
    class Renderer
    {
        #region Fields

        private PenSet m_penHandleLine = new PenSet();
        private PenSet m_penCurve = new PenSet();
        private BrushSet m_brushControlPoint = new BrushSet();
        private BrushSet m_brushHandle = new BrushSet();

        private ColorSet m_handleLineColor = new ColorSet();
        private ColorSet m_curveColor = new ColorSet();
        private ColorSet m_controlPointColor = new ColorSet();
        private ColorSet m_handleColor = new ColorSet();

        #endregion Fields

        #region Properties

        public float PointSize { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Renderer()
        {
            this.PointSize = 6.0f;
            m_handleLineColor.Base = Color.FromArgb(72,72,72);
            m_handleLineColor.Touch = Color.FromArgb(72,72,72);
            m_handleLineColor.Select = Color.FromArgb(72, 72, 72);
            m_curveColor.Base = Color.Black;
            m_curveColor.Touch = Color.Pink;
            m_curveColor.Select = Color.FromArgb(255, 255, 0);
            m_controlPointColor.Base = Color.Red;
            m_controlPointColor.Touch = Color.Pink;
            m_controlPointColor.Select = Color.FromArgb(255, 255, 0);
            m_handleColor.Base = Color.Gray;
            m_handleColor.Touch = Color.Pink;
            m_handleColor.Select = Color.FromArgb(255, 255, 0);
        }

        #endregion Constructor

        #region Private Methods

        /// <summary>
        /// 描画を開始する。
        /// </summary>
        private void BeginDraw()
        {
            m_penHandleLine.Setup(m_handleLineColor);
            m_penCurve.Setup(m_curveColor);
            m_brushControlPoint.Setup(m_controlPointColor);
            m_brushHandle.Setup(m_handleColor);
        }

        /// <summary>
        /// コントロールポイント描画
        /// </summary>
        /// <param name="pt"></param>
        private void DrawControlPoint(Graphics g, CurveContent content, int i)
        {
            Brush brush = m_brushControlPoint.Base;
            if (content.Items[i].Select)
            {
                brush = m_brushControlPoint.Select;
            }
            else if (content.Items[i].Touch)
            {
                brush = m_brushControlPoint.Touch;
            }

            g.FillRectangle(
                    brush,
                    content.Items[i].Position.X - PointSize * 0.5f,
                    content.Items[i].Position.Y - PointSize * 0.5f,
                    PointSize,
                    PointSize);
        }

        /// <summary>
        /// コントロールポイント描画
        /// </summary>
        /// <param name="pt"></param>
        private void DrawHandlePoint(Graphics g, Brush brush, Pen pen, Vector2 pos)
        {
            g.FillEllipse(
                    brush,
                    pos.X - PointSize * 0.5f,
                    pos.Y - PointSize * 0.5f,
                    PointSize,
                    PointSize);

            g.DrawEllipse(
                pen,
                pos.X - PointSize * 0.5f,
                pos.Y - PointSize * 0.5f,
                PointSize,
                PointSize);

#if false
            g.FillRectangle(
                brush,
                pos.X - PointSize * 0.5f,
                pos.Y - PointSize * 0.5f,
                PointSize,
                PointSize);
#endif
        }

        /// <summary>
        /// ハンドル描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="doc"></param>
        /// <param name="i"></param>
        private void DrawHandles(Graphics g, CurveContent content, int i)
        {
            Pen pen = m_penHandleLine.Base;
            if (content.Items[i].Handle1.Touch)
            {
                pen = m_penHandleLine.Touch;
            }

            g.DrawLine(
                pen,
                content.Items[i].Position.X,
                content.Items[i].Position.Y,
                content.Items[i].Handle1Position.X,
                content.Items[i].Handle1Position.Y);

            Brush brush = m_brushHandle.Base;
            if (content.Items[i].Handle1.Touch)
            {
                brush = m_brushHandle.Touch;
            }

            DrawHandlePoint(g, brush, pen, content.Items[i].Handle1Position);

            pen = m_penHandleLine.Base;
            if (content.Items[i].Handle2.Touch)
            {
                pen = m_penHandleLine.Touch;
            }

            g.DrawLine(
                pen,
                content.Items[i].Position.X,
                content.Items[i].Position.Y,
                content.Items[i].Handle2Position.X,
                content.Items[i].Handle2Position.Y);

            brush = m_brushHandle.Base;
            if (content.Items[i].Handle2.Touch)
            {
                brush = m_brushHandle.Touch;
            }

            DrawHandlePoint(g, brush, pen, content.Items[i].Handle2Position);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="doc"></param>
        public void DrawControlPoints(
            Graphics g,
            CurveContent content,
            bool drawHandle)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            m_controlPointColor.Base = content.PointColor;
            m_brushControlPoint.Clear();
            BeginDraw();

            for (int i = 0; i < content.Items.Count; ++i)
            {
                if (drawHandle)
                {
                    DrawHandles(g, content, i);
                }

                DrawControlPoint(g, content, i);
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="doc"></param>
        public void DrawCurves(Graphics g, CurveContent content)
        {
            m_curveColor.Base = content.CurveColor;
            m_penCurve.Clear();
            BeginDraw();

            for (int i = 0; i < content.Items.Count - 1; ++i)
            {
                DrawCurve(g, content, i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="doc"></param>
        /// <param name="index"></param>
        public void DrawCurve(
            Graphics g,
            CurveContent content,
            int index)
        {
            System.Diagnostics.Debug.Assert(index < content.Items.Count);
            System.Diagnostics.Debug.Assert(content.Items[index] != null);

            BeginDraw();

            Pen pen = m_penCurve.Base;
            if (content.Items[index].Curve.Select)
            {
                pen = m_penCurve.Select;
            }
            else if (content.Items[index].Curve.Touch)
            {
                pen = m_penCurve.Touch;
            }

            Vector2[] point = CurveBuilder.Build(content, index, 16);
            DrawCurveMain(g, pen, point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="point"></param>
        public void DrawCurveMain(Graphics g, Pen pen, Vector2[] point)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            for (int i = 0; i < point.Length - 1; ++i)
            {
                g.DrawLine(pen, new PointF(point[i].X, point[i].Y), new PointF(point[i + 1].X, point[i + 1].Y));
            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
        }

        #endregion Public Methods
    }
}
