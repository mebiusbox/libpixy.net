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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libpixy.net.Vecmath;

namespace libpixy.net.Controls.CurveEditor
{
    /// <summary>
    /// カーブエディタ
    /// </summary>
    public partial class CurveEditor : UserControl, libpixy.net.Tools.IDebugParamServer
    {
        #region Types

        /// <summary>
        /// カーソルの接触情報
        /// </summary>
        class Touch
        {
            public int ControlPointIndex = -1;
            public int CurveIndex = -1;
            public DragTarget Target = DragTarget.None;

            /// <summary>
            /// 
            /// </summary>
            public void Clear()
            {
                this.ControlPointIndex = -1;
                this.CurveIndex = -1;
                this.Target = DragTarget.None;
            }
        }

        /// <summary>
        /// マウス操作
        /// </summary>
        enum MouseOperation
        {
            None,
            DragPoint,
            DragHandle,
            DragCanvas,
            ZoomCanvas,
            DragIndicator,
            FrameCanvas,
            Selection
        }

        /// <summary>
        /// マウス情報
        /// </summary>
        class Mouse : Tool.Mouse
        {
            public bool Enter = false;
            public DragTarget DragTarget = DragTarget.Position;
            public int DragControlPointIndex = -1;
            public MouseOperation Operation = MouseOperation.None;
        }

        /// <summary>
        /// 定規情報
        /// </summary>
        struct RulerParam
        {
            public int start;//開始
            public int width;//幅
            public int tick;//目盛りの値を表示する分解数
            public int tickWidth;//目盛りの線幅
            public int tickPixel;//目盛の間隔
            public float unit;//単位
            public bool snap;//スナップ
            public string format;//目盛りの書式
            public float offset;//目盛のオフセット値
        }

        /// <summary>
        /// ２次元定規
        /// </summary>
        struct Ruler2D
        {
            public RulerParam x;//X軸
            public RulerParam y;//Y軸
        }

        #endregion Types

        #region Fields

        private Touch m_touch = new Touch();
        private Mouse m_mouse = new Mouse();
        private Tool.Canvas m_canvas = new Tool.Canvas();
        private Renderer m_renderer = new Renderer();

        #endregion

        #region Attributes

        private Document m_document = new Document();
        private Ruler2D m_ruler;//標準設定
        private Ruler2D m_rulerActual;//実際に表示する設定

        private Color m_gridColor = Color.FromArgb(164, 160, 160);
        private Color m_tickColor = Color.FromArgb(255, 0, 0);
        private Color m_extrapolationColor = Color.FromKnownColor(KnownColor.Gray);
        private bool m_showExtrapolation = false;
        private Color m_indicatorColor = Color.FromArgb(255, 128, 0);
        private PointF m_indicatorPosition;
        private bool m_showIndicatorX = false;
        private bool m_showIndicatorY = false;
        private Font m_font = new Font("Tahoma", 8, FontStyle.Bold);

        #endregion

        #region Public Events

        /// <summary>
        /// イベント：キャンバスがドラッグされた
        /// </summary>
        //public event EventHandler CanvasDragged;

        /// <summary>
        /// イベント：キャンバスがドラッグされている
        /// </summary>
        //public event EventHandler CanvasDragging;

        /// <summary>
        /// イベント：キャンバス上でマウスのボタンが押された
        /// </summary>
        public event MouseEventHandler CanvasMouseDown;

        /// <summary>
        /// イベント：キャンバス上でマウスのボタンを離した
        /// </summary>
        public event MouseEventHandler CanvasMouseUp;

        /// <summary>
        /// イベント：カーブにマウスが触れた
        /// </summary>
        //public event EventHandler CurveTouched;

        /// <summary>
        /// イベント：カーブをクリックした
        /// </summary>
        public event EventHandler CurveClicked;

        /// <summary>
        /// イベント：コントロールポイントにマウスが触れた
        /// </summary>
        //public event EventHandler ControlPointTouched;

        /// <summary>
        /// イベント：コントロールポイントをクリックした
        /// </summary>
        //public event EventHandler ControlPointClicked;

        /// <summary>
        /// イベント：コントロールポイントが選択された
        /// </summary>
        public event EventHandler ControlPointSelected;

        /// <summary>
        /// イベント：コントロールポイントが変更された
        /// </summary>
        public event EventHandler ControlPointChanged;

        /// <summary>
        /// イベント：グラフ更新中
        /// </summary>
        //public event EventHandler GraphChanging;

        /// <summary>
        /// イベント：グラフを更新した
        /// </summary>
        public event EventHandler GraphChanged;

        /// <summary>
        /// イベント：インディケーターがドラッグされた
        /// </summary>
        public event EventHandler IndicatorPositionChanged;

        public delegate void CurveDescriptionClickedEvent(object sender, string name);
        public event CurveDescriptionClickedEvent CurveDescriptionClicked;

        #endregion Public Events

        #region Properties

        /// <summary>
        /// ドキュメント
        /// </summary>
        public Document Document
        {
            get { return m_document; }
            set
            {
                m_document = value;
                m_document.Changed += new EventHandler(m_document_Changed);
            }
        }

        /// <summary>
        /// キャンバスのスケール
        /// </summary>
        public PointF CanvasScale
        {
            get { return m_canvas.Scale; }
            set
            {
                m_canvas.Scale = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// キャンバスの原点
        /// </summary>
        public Point CanvasOrigin
        {
            get { return m_canvas.Origin; }
            set
            {
                m_canvas.Origin = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// プロット領域を取得
        /// </summary>
        /// <returns></returns>
        public Rectangle GetPlotArea()
        {
            return new Rectangle(m_ruler.y.width, 0, this.ClientSize.Width - m_ruler.y.width, this.ClientSize.Height - m_ruler.x.width);
        }

        /// <summary>
        /// グリッドの色
        /// </summary>
        public Color GridColor
        {
            get { return m_gridColor; }
            set { m_gridColor = value; }
        }

        /// <summary>
        /// 目盛り（カーソル位置）の色
        /// </summary>
        public Color TickColor
        {
            get { return m_tickColor; }
            set { m_tickColor = value; }
        }

        /// <summary>
        /// インジケータの色
        /// </summary>
        public Color IndicatorColor
        {
            get { return m_indicatorColor; }
            set { m_indicatorColor = value; }
        }

        /// <summary>
        /// インジケータの位置
        /// </summary>
        public PointF IndicatorPosition
        {
            get { return m_indicatorPosition; }
            set
            { 
                m_indicatorPosition = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// インジケータ(X軸)を表示するかどうか
        /// </summary>
        public bool ShowIndicatorX
        {
            get { return m_showIndicatorX; }
            set { m_showIndicatorX = value; }
        }

        /// <summary>
        /// インジケータ(Y軸)を表示するかどうか
        /// </summary>
        public bool ShowIndicatorY
        {
            get { return m_showIndicatorY; }
            set { m_showIndicatorY = value; }
        }

        /// <summary>
        /// 外挿色
        /// </summary>
        public Color ExtrapolationColor
        {
            get { return m_extrapolationColor; }
            set { m_extrapolationColor = value; }
        }

        /// <summary>
        /// 外挿を表示するかどうか
        /// </summary>
        public bool ShowExtrapolation
        {
            get { return m_showExtrapolation; }
            set { m_showExtrapolation = value; }
        }

        /// <summary>
        /// 定規（X軸）の分解度
        /// </summary>
        [Category("XRuler"), Browsable(true)]
        public int XTickLength
        {
            get { return m_ruler.x.tickPixel; }
            set 
            {
                m_ruler.x.tickPixel = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（X軸）の開始位置
        /// </summary>
        [Category("XRuler"), Browsable(true)]
        public int XStart
        {
            get { return m_ruler.x.start; }
            set
            {
                m_ruler.x.start = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（X軸）の幅
        /// </summary>
        [Category("XRuler"), Browsable(true)]
        public int XWidth
        {
            get { return m_ruler.x.width; }
            set
            {
                m_ruler.x.width = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（X軸）１目盛りの幅
        /// </summary>
        [Category("XRuler"), Browsable(true)]
        public int XTickWidth
        {
            get { return m_ruler.x.tickWidth; }
            set 
            {
                m_ruler.x.tickWidth = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（X軸）の単位
        /// </summary>
        [Category("XRuler"), Browsable(true)]
        public float XUnit
        {
            get { return m_ruler.x.unit; }
            set
            {
                m_ruler.x.unit = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（X軸）１目盛り分の量
        /// </summary>
        [Category("XRuler"), Browsable(true)]
        public int XTick
        {
            get { return m_ruler.x.tick; }
            set
            {
                m_ruler.x.tick = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（X軸）のスナップ状態
        /// </summary>
        [Category("XRuler"), Browsable(true)]
        public bool XSnap
        {
            get { return m_ruler.x.snap; }
            set
            {
                m_ruler.x.snap = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（X軸）の値表示書式
        /// </summary>
        [Category("XRuler"), Browsable(true)]
        public string XFormat
        {
            get { return m_ruler.x.format; }
            set
            {
                m_ruler.x.format = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（X軸）の１目盛りのピクセル数
        /// </summary>
        public float XOffset
        {
            get { return m_ruler.x.offset; }
            set 
            {
                foreach (ControlPoint cp in m_document.Content.Items)
                {
                    PointF tmp = CanvasToValue(cp.Position.ToPointF());
                    tmp.X += value;
                    cp.Position = new Vector2(ValueToCanvas(tmp));
                }

                m_ruler.x.offset = value;

                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）の分解度
        /// </summary>
        [Category("YRuler"), Browsable(true)]
        public int YTickLength
        {
            get { return m_ruler.y.tickPixel; }
            set
            {
                m_ruler.y.tickPixel = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）の開始位置
        /// </summary>
        [Category("YRuler"), Browsable(true)]
        public int YStart
        {
            get { return m_ruler.y.start; }
            set
            {
                m_ruler.y.start = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）の幅
        /// </summary>
        [Category("YRuler"), Browsable(true)]
        public int YWidth
        {
            get { return m_ruler.y.width; }
            set
            {
                m_ruler.y.width = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）１目盛りの幅
        /// </summary>
        [Category("YRuler"), Browsable(true)]
        public int YTickWidth
        {
            get { return m_ruler.y.tickWidth; }
            set 
            {
                m_ruler.y.tickWidth = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）の単位
        /// </summary>
        [Category("YRuler"), Browsable(true)]
        public float YUnit
        {
            get { return m_ruler.y.unit; }
            set
            {
                m_ruler.y.unit = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）１目盛り分の量
        /// </summary>
        [Category("YRuler"), Browsable(true)]
        public int YTick
        {
            get { return m_ruler.y.tick; }
            set
            {
                m_ruler.y.tick = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）のスナップ状態
        /// </summary>
        [Category("YRuler"), Browsable(true)]
        public bool YSnap
        {
            get { return m_ruler.y.snap; }
            set
            {
                m_ruler.y.snap = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）の値表示書式
        /// </summary>
        [Category("YRuler"), Browsable(true)]
        public string YFormat
        {
            get { return m_ruler.y.format; }
            set
            {
                m_ruler.y.format = value;
                this.CalcRulerActual();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 定規（Y軸）の１目盛りのピクセル数
        /// </summary>
        public float YOffset
        {
            get { return m_ruler.y.offset; }
            set
            {
                float offset = m_ruler.y.offset;

                foreach (ControlPoint cp in m_document.Content.Items)
                {
                    m_ruler.y.offset = offset;
                    PointF tmp = CanvasToValue(cp.Position.ToPointF());
                    m_ruler.y.offset = value;
                    cp.Position = new Vector2(ValueToCanvas(tmp));
                }

                m_ruler.y.offset = value;

                this.Invalidate();
            }
        }

        public int CurveDescColorW { get; set; }
        public int CurveDescColorH { get; set; }

        /// <summary>
        /// サブデータも同時に編集
        /// </summary>
        public bool EditSubContents = false;


        #endregion

        #region Constructor, Destructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CurveEditor()
        {
            this.CurveDescColorW = 30;
            this.CurveDescColorH = 16;
            
            this.m_ruler.x.width = 30;
            this.m_ruler.x.tickWidth = 5;
            this.m_ruler.x.tickPixel = 10;
            this.m_ruler.x.unit = 10.0f;
            this.m_ruler.x.tick = 1;
            this.m_ruler.x.snap = true;
            this.m_ruler.x.format = "0";
            this.m_ruler.x.offset = 0.0f;
            this.m_ruler.y.width = 80;
            this.m_ruler.y.tickWidth = 5;
            this.m_ruler.y.tickPixel = 10;
            this.m_ruler.y.unit = 10.0f;
            this.m_ruler.y.tick = 1;
            this.m_ruler.y.snap = false;
            this.m_ruler.y.format = "0";
            this.m_ruler.y.offset = 0.0f;
            this.m_rulerActual = m_ruler;
            InitializeComponent();
            this.BackColor = Color.FromArgb(172, 168, 168);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~CurveEditor()
        {
            m_font.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// クライアント座標系からプロット座標系
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point ClientToPlot(Point p)
        {
            Rectangle area = this.GetPlotArea();
            return new Point(p.X - area.Left, area.Bottom - p.Y);
        }

        /// <summary>
        /// クライアント座標系からプロット座標系
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Rectangle ClientToPlot(Rectangle rc)
        {
            Rectangle area = this.GetPlotArea();
            int x1 = rc.X - area.Left;
            int y1 = area.Bottom - rc.Y;
            int x2 = rc.Right - area.Left;
            int y2 = area.Bottom - rc.Bottom;
            return new Rectangle(x1, y1, x2-x1, y1-y2);
        }

        /// <summary>
        /// プロット座標系からクライアント座標系
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point PlotToClient(Point p)
        {
            Rectangle area = this.GetPlotArea();
            return new Point(p.X + area.Left, area.Bottom - p.Y);
        }

        /// <summary>
        /// プロット座標系からクライアント座標系
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Rectangle PlotToClient(Rectangle rc)
        {
            Rectangle area = this.GetPlotArea();
            int x1 = rc.X + area.Left;
            int y1 = area.Bottom - rc.Y;
            int x2 = rc.Right + area.Left;
            int y2 = area.Bottom - rc.Bottom;
            return new Rectangle(x1, y1, x2 - x1, y1 - y2);
        }

        /// <summary>
        /// クライアント座標系からキャンバス座標系（スナップ付き）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point ClientToCanvas(Point p, bool snap)
        {
            Point pt = m_canvas.PointToCanvas(ClientToPlot(p));

            if (snap)
            {
                if (m_ruler.x.snap)
                {
                    int tmp = (int)Math.Round((double)pt.X / (double)this.XTickLength);
                    pt.X = tmp * this.XTickLength;
                }

                if (m_ruler.y.snap)
                {
                    int tmp = (int)Math.Round((double)pt.Y / (double)this.YTickLength);
                    pt.Y = tmp * this.YTickLength;
                }
            }

            return pt;
        }

        /// <summary>
        /// クライアント座標系からキャンバス座標系（スナップ付き）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Rectangle ClientToCanvas(Rectangle rc, bool snap)
        {
            Point lt = ClientToCanvas(rc.Location, snap);
            Point rb = ClientToCanvas(new Point(rc.Right, rc.Bottom), snap);
            if (lt.X > rb.X)
            {
                int t = lt.X; lt.X = rb.X; rb.X = t;
            }
            if (lt.Y > rb.Y)
            {
                int t = lt.Y; lt.Y = rb.Y; rb.Y = t;
            }

            return new Rectangle(lt.X, lt.Y, rb.X - lt.X, rb.Y - lt.Y);
        }

        /// <summary>
        /// キャンバス座標系からクライアント座標系
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point CanvasToClient(Point p)
        {
            return PlotToClient(m_canvas.PointToView(p));
        }

        /// <summary>
        /// スナップ
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point Snap(Point p)
        {
            return CanvasToClient(ClientToCanvas(p, true));
        }

        /// <summary>
        /// クライアント座標からグラフの座標を求める
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PointF ClientToValue(Point p)
        {
            return CanvasToValue(ClientToCanvas(p, true));
        }

        /// <summary>
        /// キャンバス座標から値を取得
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public PointF CanvasToValue(Point cp)
        {
            float xvalue = (float)cp.X / (float)this.XTickLength;
            float yvalue = (float)cp.Y / (float)this.YTickLength;
            return new PointF(xvalue * m_ruler.x.unit + m_ruler.x.offset, yvalue * m_ruler.y.unit + m_ruler.y.offset);
        }

        /// <summary>
        /// キャンバス座標から値を取得
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public PointF CanvasToValue(PointF cp)
        {
            float xvalue = cp.X / (float)this.XTickLength;
            float yvalue = cp.Y / (float)this.YTickLength;
            return new PointF(xvalue * m_ruler.x.unit + m_ruler.x.offset, yvalue * m_ruler.y.unit + m_ruler.y.offset);
        }

        /// <summary>
        /// 値からクライアント座標を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Point ValueToClient(PointF value)
        {
            return CanvasToClient(ValueToCanvas(value));
        }

        /// <summary>
        /// 値からキャンバス座標を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Point ValueToCanvas(PointF value)
        {
            Point cp = new Point();
            cp.X = (int)((value.X - m_ruler.x.offset) / m_ruler.x.unit * (float)this.XTickLength);
            cp.Y = (int)((value.Y - m_ruler.y.offset) / m_ruler.y.unit * (float)this.YTickLength);
            return cp;
        }

        public void PreCalcPointPositions()
        {
            for (int i = 0; i < this.Document.Content.Items.Count; ++i)
            {
                this.Document.Content.Items[i].Value = new Vector2(CanvasToValue(this.Document.Content.Items[i].Position.ToPointF()));
            }
        }

        public void CalcPointPositions()
        {
            if (this.Document.Content.Items.Count > 0)
            {
                for (int i = 0; i < this.Document.Content.Items.Count; ++i)
                {
                    this.Document.Content.Items[i].Position = new Vector2(ValueToCanvas(this.Document.Content.Items[i].Value.ToPointF()));
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetCanvas()
        {
            m_canvas.Reset();
        }

        /// <summary>
        /// フレーム
        /// </summary>
        public void FrameCanvas()
        {
            List<ControlPoint> cplist = null;
            if (m_document.SelectedItems.Count > 0)
            {
                cplist = new List<ControlPoint>();
                foreach (int index in m_document.SelectedItems)
                {
                    cplist.Add(m_document.Content.Items[index]);
                }
            }
            else
            {
                cplist = m_document.Content.Items;
            }

            if (cplist.Count == 0)
            {
                return;
            }

            float x1 = float.MaxValue;
            float y1 = float.MaxValue;
            float x2 = float.MinValue;
            float y2 = float.MinValue;
            foreach (ControlPoint cp in cplist)
            {
                if (x1 > cp.Position.X)
                {
                    x1 = cp.Position.X;
                }
                if (y1 > cp.Position.Y)
                {
                    y1 = cp.Position.Y;
                }
                if (x2 < cp.Position.X)
                {
                    x2 = cp.Position.X;
                }
                if (y2 < cp.Position.Y)
                {
                    y2 = cp.Position.Y;
                }
            }

            Rectangle rcArea = new Rectangle((int)x1, (int)y1, (int)(x2-x1), (int)(y2-y1));
            Rectangle rcView = m_canvas.RectToView(rcArea);
            rcView.Inflate(20, 20);
            m_canvas.Frame(rcView, GetPlotArea());
            Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalcRulerActual()
        {
            this.m_rulerActual.x.start = m_ruler.x.start - (m_canvas.Origin.X / m_ruler.x.tickPixel);
            this.m_rulerActual.x.width = this.m_ruler.x.width;
            this.m_rulerActual.x.tickWidth = this.m_ruler.x.tickWidth;
            this.m_rulerActual.x.unit = this.m_ruler.x.unit;
            this.m_rulerActual.x.tick = this.m_ruler.x.tick;

            this.m_rulerActual.y.start = m_ruler.y.start - (m_canvas.Origin.Y / m_ruler.y.tickPixel);
            this.m_rulerActual.y.width = this.m_ruler.y.width;
            this.m_rulerActual.y.tickWidth = this.m_ruler.y.tickWidth;
            this.m_rulerActual.y.unit = this.m_ruler.y.unit;
            this.m_rulerActual.y.tick = this.m_ruler.y.tick;

#if false
            if (this.m_rulerActual.x.start <= m_ruler.x.min && this.m_canvas.Origin.X > -m_rulerActual.x.start * intervalX)
            {
                this.m_rulerActual.x.start = m_ruler.x.min;
                this.m_canvas.Origin = new Point(-m_rulerActual.x.start * intervalX, this.m_canvas.Origin.Y);
            }

            if (this.m_rulerActual.y.start <= m_ruler.y.min && this.m_canvas.Origin.Y > -m_rulerActual.y.start * intervalY)
            {
                this.m_rulerActual.y.start = m_ruler.y.min;
                this.m_canvas.Origin = new Point(this.m_canvas.Origin.X, -m_rulerActual.y.start * intervalY);
            }
#endif
        }

        #endregion

        #region Mouse Operation

        /// <summary>
        /// コントロールポイントをドラッグするかどうか調べる
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckDragControlPoint(MouseEventArgs e)
        {
            if (m_touch.ControlPointIndex != -1)
            {
                m_mouse.Drag.Location = e.Location;
                m_mouse.DragControlPointIndex = m_touch.ControlPointIndex;
                m_mouse.DragTarget = m_touch.Target;
                m_mouse.Drag.Valid = true;
                if (m_touch.Target == DragTarget.Position)
                {
                    m_mouse.Operation = MouseOperation.DragPoint;
                }
                else if (
                    m_touch.Target == DragTarget.Handle1 ||
                    m_touch.Target == DragTarget.Handle2)
                {
                    m_mouse.Operation = MouseOperation.DragHandle;
                }

                ControlPoint cp = m_document.Content.Items[m_touch.ControlPointIndex];
                if (cp.Select == false)
                {
                    m_document.ClearSelection();
                    m_document.SelectedItems.Add(m_touch.ControlPointIndex);

                    SelectCurves();
                }

                libpixy.net.Vecmath.Utils.RaiseEvent(this.ControlPointSelected, this);

                return true;
            }

            return false;
        }

        private void SelectCurves()
        {
            if (CurveClicked == null)
            {
                return;
            }

            foreach (int index in m_document.SelectedItems)
            {
                ControlPoint cp = m_document.Content.Items[index];
                if (cp.Curve != null)
                {
                    m_document.SelectedCurves.Add(index);
                    cp.Curve.Select = true;

                    if (index > 0)
                    {
                        cp = m_document.Content.Items[index - 1];
                        if (cp.Curve != null && m_document.SelectedCurves.IndexOf(index - 1) < 0)
                        {
                            m_document.SelectedCurves.Add(index - 1);
                            cp.Curve.Select = true;
                        }
                    }
                }
            }

            CurveClicked(this, EventArgs.Empty);
        }

        /// <summary>
        /// カーブに接触していたらイベントを起こす
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckCurveClicked(MouseEventArgs e)
        {
            if (m_touch.CurveIndex != -1)
            {
                if (this.CurveClicked != null)
                {
                    this.m_document.SelectedCurves.Add(m_touch.CurveIndex);
                    this.m_document.Content.Items[m_touch.CurveIndex].Curve.Select = true;
                    this.CurveClicked(this, EventArgs.Empty);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// カーブ詳細をクリックしたらイベントを起こす
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckCurveDescriptionClicked(MouseEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            Rectangle area = GetPlotArea();
            int x = area.Left;
            int y = area.Top + 2;

            if (string.IsNullOrEmpty(m_document.Content.Name) == false)
            {
                Rectangle rc = CalcDrawCurveDescriptionArea(g, x, y, area.Width, CurveDescColorH, m_document.Content);
                if (rc.Contains(e.Location))
                {
                    if (this.CurveDescriptionClicked != null)
                    {
                        this.CurveDescriptionClicked(this, m_document.Content.Name);
                    }

                    return true;
                }

                y += CurveDescColorH;
            }

            foreach (CurveContent content in m_document.SubContents)
            {
                Rectangle rc = CalcDrawCurveDescriptionArea(g, x, y, area.Width, CurveDescColorH, content);
                if (rc.Contains(e.Location))
                {
                    if (this.CurveDescriptionClicked != null)
                    {
                        this.CurveDescriptionClicked(this, content.Name);
                    }

                    return true;
                }

                y += CurveDescColorH;
            }

            return false;
        }

        /// <summary>
        /// コントロールポイントをドラッグするかどうか調べる
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckDragIndicator(MouseEventArgs e)
        {
            if (m_touch.ControlPointIndex == -1 && m_touch.CurveIndex == -1 && m_touch.Target == DragTarget.Indicator)
            {
                m_mouse.Drag.Location = e.Location;
                m_mouse.DragTarget = m_touch.Target;
                m_mouse.Drag.Valid = true;
                m_mouse.Operation = MouseOperation.DragIndicator;
                return true;
            }

            return false;
        }

        /// <summary>
        /// キャンバスをドラッグするか調べる
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckDragCanvas(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Control.ModifierKeys == Keys.Alt)
                {
                    m_mouse.Drag.Location = e.Location;
                    m_mouse.DragControlPointIndex = -1;
                    m_mouse.DragTarget = DragTarget.Canvas;
                    m_mouse.Drag.Valid = true;
                    m_mouse.Operation = MouseOperation.ZoomCanvas;
                }
                else
                {
                    m_mouse.Drag.Location = e.Location;
                    m_mouse.DragControlPointIndex = -1;
                    m_mouse.DragTarget = DragTarget.Canvas;
                    m_mouse.Drag.Valid = true;
                    m_mouse.Operation = MouseOperation.DragCanvas;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// キャンバスをズームするか調べる
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckZoomCanvas(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                m_mouse.Drag.Location = e.Location;
                m_mouse.DragControlPointIndex = -1;
                m_mouse.DragTarget = DragTarget.Canvas;
                m_mouse.Drag.Valid = true;
                m_mouse.Operation = MouseOperation.ZoomCanvas;
                return true;
            }

            return false;
        }

        /// <summary>
        /// ポイントのドラッグ処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool UpdateDragPoint(MouseEventArgs e)
        {
            if (m_mouse.Operation == MouseOperation.DragPoint)
            {
                Point loc = ClientToCanvas(e.Location, true);
                Point old = ClientToCanvas(m_mouse.OldLocation, true);
                int dx = loc.X - old.X;
                int dy = loc.Y - old.Y;

                /// 複数同時編集はコントロールポイントが１つのみ対応
                if (this.EditSubContents && m_document.SelectedItems.Count == 1)
                {
                    int index = m_document.SelectedItems[0];
                    foreach (CurveContent sub in m_document.SubContents)
                    {
                        for (int i = 0; i < sub.Items.Count; ++i)
                        {
                            if (sub.Items[i].Position.X == m_document.Content.Items[index].Position.X &&
                                sub.Items[i].Position.Y == m_document.Content.Items[index].Position.Y)
                            {
                                sub.Items[i].Position += new Vector2((float)dx, (float)dy);
                            }
                        }
                    }

                    m_document.Content.Items[index].Position += new Vector2((float)dx, (float)dy);
                }
                else
                {
                    foreach (int index in m_document.SelectedItems)
                    {
                        m_document.Content.Items[index].Position += new Vector2((float)dx, (float)dy);
                    }
                }

                libpixy.net.Vecmath.Utils.RaiseEvent(this.ControlPointChanged, this);

                return true;
            }

            return false;
        }

        /// <summary>
        /// キャンバスのドラッグ処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool UpdateDragCanvas(MouseEventArgs e)
        {
            if (m_mouse.Operation == MouseOperation.DragCanvas)
            {
                Point loc = ClientToCanvas(e.Location, false);
                Point old = ClientToCanvas(m_mouse.OldLocation, false);
                int dx = loc.X - old.X;
                int dy = loc.Y - old.Y;
                m_canvas.Origin = new Point(m_canvas.Origin.X + dx, m_canvas.Origin.Y + dy);
                this.CalcRulerActual();
                this.Invalidate();
                return true;
            }

            return false;
        }

        /// <summary>
        /// キャンバスのズーム処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool UpdateZoomCanvas(MouseEventArgs e)
        {
            if (m_mouse.Operation == MouseOperation.ZoomCanvas)
            {
                Point d = m_mouse.GetMoved();
                double s = Math.Sqrt((double)d.X * (double)d.X + (double)d.Y * (double)d.Y);
                if (d.X < 0 || d.Y < 0) s = -s;

                Rectangle rcPlot = GetPlotArea();
                m_canvas.Zoom(
                    libpixy.net.Vecmath.Utils.GetCenter(rcPlot),
                    new PointF(
                        Math.Min(10.0f, m_canvas.Scale.X + (float)s * 0.01f),
                        Math.Min(10.0f, m_canvas.Scale.Y + (float)s * 0.01f)));

                this.CalcRulerActual();
                this.Invalidate();
                return true;
            }

            return false;
        }

        /// <summary>
        /// インジケーターのドラッグ処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool UpdateDragIndicator(MouseEventArgs e)
        {
            if (m_mouse.Operation == MouseOperation.DragIndicator)
            {
                PointF loc = ClientToValue(e.Location);
                PointF old = ClientToValue(m_mouse.OldLocation);
                m_indicatorPosition.X += (loc.X - old.X);
                this.CalcRulerActual();
                this.Invalidate();

                if (this.IndicatorPositionChanged != null)
                {
                    this.IndicatorPositionChanged(this, EventArgs.Empty);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// ハンドルのドラッグ処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool UpdateDragHandle(MouseEventArgs e)
        {
            if (m_mouse.Operation == MouseOperation.DragHandle)
            {
                Point loc = ClientToCanvas(e.Location, false);
                Point old = ClientToCanvas(m_mouse.OldLocation, false);
                int dx = loc.X - old.X;
                int dy = loc.Y - old.Y;
                if (m_mouse.DragTarget == DragTarget.Handle1)
                {
                    ControlPoint cp = m_document.IndexOf(m_mouse.DragControlPointIndex);
                    cp.Handle1Position += new Vector2((float)dx, (float)dy);
                    if ((Control.ModifierKeys & Keys.Shift) == 0)
                    {
                        libpixy.net.Vecmath.Vector2 dir = cp.Position - cp.Handle1Position;
                        cp.Handle2Position = cp.Position + dir;
                    }
                }
                else if (m_mouse.DragTarget == DragTarget.Handle2)
                {
                    ControlPoint cp = m_document.IndexOf(m_mouse.DragControlPointIndex);
                    cp.Handle2Position += new Vector2((float)dx, (float)dy);
                    if ((Control.ModifierKeys & Keys.Shift) == 0)
                    {
                        libpixy.net.Vecmath.Vector2 dir = cp.Position - cp.Handle2Position;
                        cp.Handle1Position = cp.Position + dir;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// ドラッグ更新
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool UpdateDrag(MouseEventArgs e)
        {
            if (m_mouse.Drag.Valid)
            {
                if (UpdateDragPoint(e))
                {
                    return true;
                }

                if (UpdateDragCanvas(e))
                {
                    return true;
                }

                if (UpdateZoomCanvas(e))
                {
                    return true;
                }

                if (UpdateDragHandle(e))
                {
                    return true;
                }

                if (UpdateDragIndicator(e))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// プロット画面のカーソル更新
        /// </summary>
        /// <param name="e"></param>
        private void UpdatePlotCursor(MouseEventArgs e)
        {
            Rectangle area = this.GetPlotArea();
            if (area.Contains(e.Location))
            {
                m_mouse.Enter = true;
            }
            else
            {
                m_mouse.Enter = false;
            }
        }

        /// <summary>
        /// 選択領域を更新
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool UpdateSelection(MouseEventArgs e)
        {
            if (m_mouse.Selection.Valid)
            {
                m_mouse.Selection.EndPos = e.Location;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 選択を開始するか調べる
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckSelection(MouseEventArgs e)
        {
            if (m_touch.ControlPointIndex != -1 || m_touch.CurveIndex != -1 || m_touch.Target == DragTarget.Indicator)
            {
                return false;
            }

            if (m_mouse.Selection.Valid)
            {
                return true;
            }

            if (m_mouse.Press)
            {
                m_mouse.Selection.Valid = true;
                m_mouse.Selection.StartPos = e.Location;
                m_mouse.Selection.EndPos = e.Location;
                return true;
            }

            return false;
        }

        /// <summary>
        /// ドラッグを終了する
        /// </summary>
        /// <returns></returns>
        private bool EndDrag()
        {
            if (m_mouse.Drag.Valid)
            {
                if (m_mouse.DragTarget == DragTarget.Position ||
                    m_mouse.DragTarget == DragTarget.Handle1 ||
                    m_mouse.DragTarget == DragTarget.Handle2)
                {
                    if (this.GraphChanged != null)
                    {
                        this.GraphChanged(this, EventArgs.Empty);
                    }
                }

                m_mouse.Drag.Valid = false;
                m_mouse.Operation = MouseOperation.None;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 選択を終了する
        /// </summary>
        /// <returns></returns>
        private bool EndSelection()
        {
            if (m_mouse.Selection.Valid)
            {
                m_mouse.Selection.Valid = false;
                m_mouse.Operation = MouseOperation.None;

                Rectangle rc = ClientToCanvas(m_mouse.Selection.GetRect(), false);

                if (m_mouse.ModifierKeys == Keys.Alt)
                {
                    m_canvas.Frame(m_canvas.RectToView(rc), GetPlotArea());
                    Invalidate();
                }
                else
                {
                    for (int i = 0; i < m_document.Content.Items.Count; ++i)
                    {
                        if (rc.Contains(m_document.Content.Items[i].Position.ToPoint()))
                        {
                            m_document.SelectedItems.Add(i);
                            m_document.Content.Items[i].Select = true;
                        }
                    }

                    SelectCurves();

                    libpixy.net.Vecmath.Utils.RaiseEvent(this.ControlPointSelected, this);
                }
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// 接触情報をクリア
        /// </summary>
        private void ClearTouch()
        {
            if (m_touch.Target != DragTarget.None)
            {
                if (m_touch.ControlPointIndex != -1)
                {
                    m_document.Content.Items[m_touch.ControlPointIndex].Handle1.Touch = false;
                    m_document.Content.Items[m_touch.ControlPointIndex].Handle2.Touch = false;
                    m_document.Content.Items[m_touch.ControlPointIndex].Touch = false;
                    m_touch.ControlPointIndex = -1;
                }

                if (m_touch.CurveIndex != -1)
                {
                    m_document.Content.Items[m_touch.CurveIndex].Curve.Touch = false;
                    m_touch.CurveIndex = -1;
                }

                m_touch.Target = DragTarget.None;
            }
        }

        /// <summary>
        /// コントロールポイントに接触しているか調べる
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CheckTouchControlPoint(int x, int y)
        {
            Point p = this.ClientToCanvas(new Point(x, y), false);
            int old = m_touch.ControlPointIndex;
            float d = float.MaxValue;
            m_touch.ControlPointIndex = -1;
            Vector2 loc = new Vector2((float)p.X, (float)p.Y);
            for (int i = 0; i < this.m_document.Content.Items.Count; ++i)
            {
                float t = m_document.Content.Items[i].Position.Distance(loc);
                if (d > t && t < 4.0f)
                {
                    d = t;
                    m_touch.ControlPointIndex = i;
                    m_touch.Target = DragTarget.Position;
                    m_document.Content.Items[i].Touch = true;
                }
                else
                {
                    t = Vecmath.Vector2.Distance(m_document.Content.Items[i].Handle1Position, loc);
                    if (d > t && t < 4.0f)
                    {
                        d = t;
                        m_touch.ControlPointIndex = i;
                        m_touch.Target = DragTarget.Handle1;
                        m_document.Content.Items[i].Handle1.Touch = true;
                    }
                    else
                    {
                        t = Vecmath.Vector2.Distance(m_document.Content.Items[i].Handle2Position, loc);
                        if (d > t && t < 4.0f)
                        {
                            d = t;
                            m_touch.ControlPointIndex = i;
                            m_touch.Target = DragTarget.Handle2;
                            m_document.Content.Items[i].Handle2.Touch = true;
                        }
                    }
                }
            }

            if (old != m_touch.ControlPointIndex)
            {
                this.Invalidate();
            }
        }

        /// <summary>
        /// カーブに接触しているか調べる
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CheckTouchCurve(int x, int y)
        {
            int num = m_document.Content.Items.Count;
            if (num <= 1)
            {
                return;
            }

            if (m_touch.ControlPointIndex != -1)
            {
                return;
            }

            int old = m_touch.CurveIndex;
            m_touch.CurveIndex = -1;

            const float Bounds = 3.0f;
            float d = float.MaxValue;
            float t;
            Point p = this.ClientToCanvas(new Point(x, y), false);
            Vector2 loc = new Vector2((float)p.X, (float)p.Y);

            for (int i = 0; i < num - 1; ++i)
            {
                Vector2[] point = CurveBuilder.Build(m_document.Content, i, 16);
                for (int j = 0; j < point.Length - 1; ++j)
                {
                    t = Vecmath.Geom.DistanceToSegment(loc, point[j], point[j + 1]);
                    if (d > t && t < Bounds)
                    {
                        d = t;
                        m_touch.CurveIndex = i;
                    }
                }
            }

            if (old != m_touch.CurveIndex)
            {
                if (m_touch.CurveIndex != -1)
                {
                    m_document.Content.Items[m_touch.CurveIndex].Curve.Touch = true;
                    m_touch.Target = DragTarget.Curve;
                }

                this.Invalidate();
            }
        }

        /// <summary>
        /// カーブに接触しているか調べる
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CheckTouchIndicator(int x, int y)
        {
            if (m_touch.ControlPointIndex != -1 || m_touch.CurveIndex != -1)
            {
                return;
            }

            const float Bounds = 3.0f;
            Point p = ValueToClient(m_indicatorPosition);
            Vector2 loc = new Vector2((float)x, (float)y);
            Vector2 pt1 = new Vector2((float)p.X, 0.0f);
            Vector2 pt2 = new Vector2((float)p.X, (float)(this.ClientSize.Height - m_rulerActual.x.width));
            float t = Vecmath.Geom.DistanceToSegment(loc, pt1, pt2);
            if (t < Bounds)
            {
                m_touch.Target = DragTarget.Indicator;
                this.Invalidate();
            }
        }

        #endregion Mouse Operation

        #region Draw Methods

        /// <summary>
        /// キャンバス内の描画を開始する
        /// </summary>
        /// <param name="g"></param>
        private void BeginDrawInCanvas(Graphics g)
        {
            m_canvas.SetTransform(g);
            g.ScaleTransform(1.0f, -1.0f, System.Drawing.Drawing2D.MatrixOrder.Append);//Y軸反転
            g.TranslateTransform(this.GetPlotArea().Left, this.GetPlotArea().Bottom, System.Drawing.Drawing2D.MatrixOrder.Append);
        }

        /// <summary>
        /// 背景を描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawBackground(Graphics g)
        {
            using (Brush brush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }
        }

        /// <summary>
        /// グリッド描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawGrid(Graphics g)
        {
            Ruler2D ruler = m_rulerActual;
            Rectangle area = this.GetPlotArea();

            bool drawCenterX = false;
            bool drawCenterY = false;
            int centerX = 0;
            int centerY = 0;

            using (Pen pen = new Pen(this.GridColor, 1))
            {
                // x-axis
                {
                    int len = this.XTickLength;
                    int x = 0;
                    int i = ruler.x.start;
                    for (; ; )
                    {
                        x = this.CanvasToClient(new Point(i * len, 0)).X;
                        if (x > area.Right)
                        {
                            break;
                        }

                        g.DrawLine(pen, x, 0, x, area.Bottom - 1);
                        if (i == 0)
                        {
                            drawCenterX = true;
                            centerX = x;
                        }

                        ++i;
                    }
                }

                // y-axis
                {
                    int len = this.YTickLength;
                    int y = 0;
                    int i = ruler.y.start;
                    for (; ; )
                    {
                        y = CanvasToClient(new Point(0, i * len)).Y;
                        if (y < area.Top)
                        {
                            break;
                        }

                        g.DrawLine(pen, area.Left + 1, y, area.Right, y);
                        if (i == 0)
                        {
                            drawCenterY = true;
                            centerY = y;
                        }

                        i++;
                    }
                }
            }

            if (drawCenterX || drawCenterY)
            {
                using (Pen centerPen = new Pen(Color.Gray, 1))
                {
                    if (drawCenterX)
                    {
                        g.DrawLine(centerPen, centerX, 0, centerX, area.Bottom - 1);
                    }

                    if (drawCenterY)
                    {
                        g.DrawLine(centerPen, area.Left + 1, centerY, area.Right, centerY);
                    }
                }
            }
        }

        /// <summary>
        /// 目盛り描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawRuler(Graphics g)
        {
            DrawRulerVertBack(g);
            DrawRulerHorzBack(g);
            DrawRulerVert(g);
            DrawRulerHorz(g);
        }

        /// <summary>
        /// 目盛りの背景を描画(X-Axis)
        /// </summary>
        /// <param name="g"></param>
        private void DrawRulerHorzBack(Graphics g)
        {
            Rectangle area = new Rectangle(0, this.ClientRectangle.Height - m_ruler.x.width, this.ClientRectangle.Width, m_ruler.y.width);
            using (Brush brush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(brush, area);
            }

            using (Pen pen = new Pen(Brushes.Black, 1))
            {
                g.DrawLine(pen, m_ruler.y.width, area.Top, area.Right, area.Top);
            }
        }

        /// <summary>
        /// 目盛りを描画(X-Axis)
        /// </summary>
        /// <param name="g"></param>
        private void DrawRulerHorz(Graphics g)
        {
            Ruler2D ruler = m_rulerActual;
            Rectangle area = this.GetPlotArea();
            Pen pen1 = new Pen(this.ForeColor, 1);

            // Measure
            int len = this.XTickLength;
            int x = 0;
            int i = ruler.x.start;
            for (;;)
            {
                x = this.CanvasToClient(new Point(i * len, 0)).X;
                if (x > area.Right)
                {
                    break;
                }
                if (x > area.Left)
                {
                    g.DrawLine(pen1, x, area.Bottom, x, area.Bottom + ruler.x.tickWidth);

                    if ((i % ruler.x.tick) == 0)
                    {
                        float value = (float)i * ruler.x.unit + m_ruler.x.offset;
                        DrawValue(g, this.Font, this.ForeColor, x, area.Bottom + ruler.x.tickWidth, value.ToString(m_ruler.x.format), false, true);
                    }
                }
                ++i;
            }

            pen1.Dispose();
        }

        /// <summary>
        /// 目盛りの背景を描画(Y-Axis)
        /// </summary>
        /// <param name="g"></param>
        private void DrawRulerVertBack(Graphics g)
        {
            Rectangle area = new Rectangle(0, 0, m_ruler.y.width, this.ClientRectangle.Height);
            using (Brush brush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(brush, area);
            }

            using (Pen pen = new Pen(Brushes.Black, 1))
            {
                g.DrawLine(pen, area.Right, area.Top, area.Right, area.Bottom);
            }
        }

        /// <summary>
        /// 目盛りを描画(Y-Axis)
        /// </summary>
        /// <param name="g"></param>
        private void DrawRulerVert(Graphics g)
        {
            Ruler2D ruler = m_rulerActual;
            Rectangle area = this.GetPlotArea();
            Pen pen1 = new Pen(this.ForeColor, 1);

            // Measure
            int len = this.YTickLength;
            int y = 0;
            int i = ruler.y.start;
            for (; ; )
            {
                y = CanvasToClient(new Point(0, i * len)).Y;
                if (y < area.Top)
                {
                    break;
                }
                if (y < area.Bottom)
                {
                    g.DrawLine(pen1, area.Left - ruler.y.tickWidth, y, area.Left, y);

                    if ((i % ruler.y.tick) == 0)
                    {
                        float value = (float)i * ruler.y.unit + m_ruler.y.offset;
                        DrawValue(g, this.Font, this.ForeColor, area.Left - ruler.y.tickWidth, y, value.ToString(m_ruler.y.format), false, false);
                    }
                }
                i++;
            }

            pen1.Dispose();
        }

        /// <summary>
        /// 目盛りの値を描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        /// <param name="vertical"></param>
        private void DrawValue(Graphics g, Font font, Color color, int x, int y, string value, bool vertical, bool vertAlignment)
        {
            // The sizing operation is common to all options
            StringFormat format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
            if (vertical)
            {
                format.FormatFlags |= StringFormatFlags.DirectionVertical;
            }

            SizeF size = g.MeasureString(value, font, 100, format);
            Point drawingPoint;
            if (vertAlignment)
            {
                drawingPoint = new Point(x - Convert.ToInt32(Math.Ceiling(size.Width)) / 2, y);
            }
            else
            {
                drawingPoint = new Point(x - Convert.ToInt32(Math.Ceiling(size.Width)), y - Convert.ToInt32(Math.Ceiling(size.Height)) / 2);
            }

            // The drawstring function is common to all operations
            g.DrawString(value, font, new SolidBrush(color), drawingPoint, format);
        }

        /// <summary>
        /// 曲線描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="content"></param>
        /// <param name="drawHandle"></param>
        private void DrawContent(Graphics g, CurveContent content, bool drawHandle)
        {
            DrawCurves(g, content);
            DrawExtrapolationCurves(g, content);
            DrawControlPoints(g, content, drawHandle);
        }

        /// <summary>
        /// 曲線描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawCurves(Graphics g, CurveContent content)
        {
            m_renderer.DrawCurves(g, content);
        }

        /// <summary>
        /// 曲線描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawExtrapolationCurves(Graphics g, CurveContent content)
        {
            if (this.ShowExtrapolation == false)
            {
                return;
            }

            if (content.Items.Count <= 1)
            {
                return;
            }

            using (Pen pen = new Pen(content.ExtraColor, 1))
            {
                Point start_canvas = content.Items[0].Position.ToPoint();
                Point end_canvas = content.Items[content.Items.Count - 1].Position.ToPoint();
                Point start = CanvasToClient(start_canvas);
                Point end = CanvasToClient(end_canvas);
                Rectangle area = GetPlotArea();

                if (content.Extrapolation == Extrapolation.Constant)
                {
                    Point area_lt = ClientToCanvas(new Point(area.Left, area.Top), false);
                    Point area_rt = ClientToCanvas(new Point(area.Right, area.Bottom), false);

                    if (area.Left < start.X)
                    {
                        g.DrawLine(pen, area_lt.X, start_canvas.Y, start_canvas.X, start_canvas.Y);
                    }

                    if (end.X < area.Right)
                    {
                        g.DrawLine(pen, end_canvas.X, end_canvas.Y, area_rt.X, end_canvas.Y);
                    }
                }
                else if (content.Extrapolation == Extrapolation.Cycle)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    if (start.X > 0)
                    {
                        float dx = content.Items[content.Items.Count - 1].Position.X - content.Items[0].Position.X;
                        float ofx = -dx;

                        int x = start.X;
                        while (x > 0)
                        {
                            x -= (end.X - start.X);
                            for (int i = 0; i < content.Items.Count - 1; ++i)
                            {
                                Vector2[] point = CurveBuilder.Build(m_document.Content, i, 16);
                                for (int j = 0; j < point.Length - 1; ++j)
                                {
                                    g.DrawLine(pen, new PointF(ofx + point[j].X, point[j].Y), new PointF(ofx + point[j + 1].X, point[j + 1].Y));
                                }
                            }
                            ofx -= dx;
                        }
                    }
                    if (end.X < area.Right)
                    {
                        float dx = content.Items[content.Items.Count - 1].Position.X - content.Items[0].Position.X;
                        float ofx = dx;
                        int x = end.X;
                        while (x < area.Right)
                        {
                            for (int i = 0; i < content.Items.Count - 1; ++i)
                            {
                                Vector2[] point = CurveBuilder.Build(m_document.Content, i, 16);
                                for (int j = 0; j < point.Length - 1; ++j)
                                {
                                    g.DrawLine(pen, new PointF(ofx + point[j].X, point[j].Y), new PointF(ofx + point[j + 1].X, point[j + 1].Y));
                                }
                            }
                            x += (end.X - start.X);
                            ofx += dx;
                        }
                    }
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                }
                else if (content.Extrapolation == Extrapolation.CycleOffset)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    if (start.X > 0)
                    {
                        float dx = content.Items[content.Items.Count - 1].Position.X - content.Items[0].Position.X;
                        float dy = content.Items[content.Items.Count - 1].Position.Y - content.Items[0].Position.Y;
                        float ofx = -dx;
                        float ofy = -dy;
                        int x = start.X;
                        while (x > 0)
                        {
                            x -= (end.X - start.X);
                            for (int i = 0; i < content.Items.Count - 1; ++i)
                            {
                                Vector2[] point = CurveBuilder.Build(m_document.Content, i, 16);
                                for (int j = 0; j < point.Length - 1; ++j)
                                {
                                    g.DrawLine(
                                        pen,
                                        new PointF(point[j + 0].X + ofx, point[j + 0].Y + ofy),
                                        new PointF(point[j + 1].X + ofx, point[j + 1].Y + ofy));
                                }
                            }
                            ofx -= dx;
                            ofy -= dy;
                        }
                    }
                    if (end.X < area.Right)
                    {
                        float dx = content.Items[content.Items.Count - 1].Position.X - content.Items[0].Position.X;
                        float dy = content.Items[content.Items.Count - 1].Position.Y - content.Items[0].Position.Y;
                        float ofx = dx;
                        float ofy = dy;
                        int x = end.X;
                        while (x < area.Right)
                        {
                            for (int i = 0; i < content.Items.Count - 1; ++i)
                            {
                                Vector2[] point = CurveBuilder.Build(m_document.Content, i, 16);
                                for (int j = 0; j < point.Length - 1; ++j)
                                {
                                    g.DrawLine(
                                        pen,
                                        new PointF(point[j + 0].X + ofx, point[j + 0].Y + ofy),
                                        new PointF(point[j + 1].X + ofx, point[j + 1].Y + ofy));
                                }
                            }
                            ofx += dx;
                            ofy += dy;
                            x += (end.X - start.X);
                        }
                    }
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                }
            }
        }

        /// <summary>
        /// コントロールポイントを描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawControlPoints(Graphics g, CurveContent content, bool drawHandle)
        {
            m_renderer.DrawControlPoints(g, content, drawHandle);
        }

        /// <summary>
        /// カーソル位置を目盛り上に描画する
        /// </summary>
        /// <param name="g"></param>
        private void DrawCursor(Graphics g)
        {
            if (m_mouse.Enter)
            {
                m_canvas.ResetTransform(g);

                Ruler2D ruler = m_rulerActual;
                Rectangle area = this.GetPlotArea();
                Point p = Snap(m_mouse.Location);
                using (Pen pen = new Pen(this.TickColor, 3))
                {
                    g.DrawLine(pen, area.Left - ruler.y.tickWidth, p.Y, area.Left, p.Y);
                    g.DrawLine(pen, p.X, area.Bottom + 1, p.X, area.Bottom + ruler.x.tickWidth + 1);
                }

                PointF value = this.ClientToValue(m_mouse.Location);
                DrawValue(g, this.Font, this.TickColor, p.X, area.Bottom + ruler.x.tickWidth, value.X.ToString(m_ruler.x.format), false, true);
                DrawValue(g, this.Font, this.TickColor, area.Left - ruler.y.tickWidth, p.Y, value.Y.ToString(m_ruler.y.format), false, false);
            }
        }

        /// <summary>
        /// 選択領域を描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawSelection(Graphics g)
        {
            if (this.m_mouse.Selection.Valid)
            {
                Rectangle rc = this.m_mouse.Selection.GetRect();
                Brush brush = new SolidBrush(Color.FromArgb(64, 0, 0, 255));
                g.FillRectangle(brush, rc);
                g.DrawRectangle(Pens.Blue, rc);
            }
        }

        /// <summary>
        /// インジケータ描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawIndicator(Graphics g)
        {
            if (m_showIndicatorX || m_showIndicatorY)
            {
                m_canvas.ResetTransform(g);

                Color col = this.IndicatorColor;
                if (m_touch.Target == DragTarget.Indicator)
                {
                    col = Color.FromArgb(255, 255, 0);
                }
                Point p = ValueToClient(m_indicatorPosition);
                Rectangle area = this.GetPlotArea();
                using (Pen pen = new Pen(col, 1))
                {
                    if (m_showIndicatorX && area.Left <= p.X && p.X < area.Right)
                    {
                        g.DrawLine(pen, p.X, area.Top, p.X, area.Bottom);
                    }

                    if (m_showIndicatorY && area.Top <= p.Y && p.Y < area.Bottom)
                    {
                        g.DrawLine(pen, area.Left+1, p.Y, area.Right, p.Y);
                    }
                }
            }
        }

        /// <summary>
        /// マーカー描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawMarkers(Graphics g)
        {
            m_canvas.ResetTransform(g);

            foreach (Marker marker in m_document.Markers)
            {
                if (marker.XAxis == false && marker.YAxis == false)
                {
                    continue;
                }

                Point p = ValueToClient(marker.Position);
                Rectangle area = this.GetPlotArea();
                if (area.Left <= p.X && p.X < area.Right)
                {
                    using (Pen pen = new Pen(marker.Color, 1))
                    {
                        if (marker.XAxis)
                        {
                            g.DrawLine(pen, p.X, area.Top, p.X, area.Bottom);
                        }

                        if (marker.YAxis)
                        {
                            g.DrawLine(pen, area.Left + 1, p.Y, area.Right, p.Y);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 表示しているカーブの詳細を表示する
        /// </summary>
        /// <param name="g"></param>
        private void DrawCurveDescription(Graphics g)
        {
            Rectangle area = GetPlotArea();

            int x = area.Left;
            int y = area.Top + 2;

            if (string.IsNullOrEmpty(m_document.Content.Name) == false)
            {
                DrawCurveDescription(g, x, y, area.Width, CurveDescColorH, m_document.Content);
            }

            y += CurveDescColorH;

            foreach (CurveContent content in m_document.SubContents)
            {
                DrawCurveDescription(g, x, y, area.Width, CurveDescColorH, content);
                y += CurveDescColorH;
            }
        }

        /// <summary>
        /// 表示しているカーブの詳細を表示する
        /// </summary>
        /// <param name="g"></param>
        private void DrawCurveDescription(Graphics g, int x, int y, int w, int h, CurveContent content)
        {
            //Rectangle area = CalcDrawCurveDescriptionArea(g, x, y, w, h, content);
            //using (Brush brush = new SolidBrush(Color.FromArgb(0,255,0)))
            //{
            //    g.FillRectangle(brush, area);
            //}

            int margin = 4;
            Rectangle rc = new Rectangle(x + w - CurveDescColorW - margin, y+2, CurveDescColorW, h-4);
            using (Brush brush = new SolidBrush(content.CurveColor))
            {
                g.FillRectangle(brush, rc);
            }

            using (Pen pen = new Pen(Color.Black, 1))
            {
                g.DrawRectangle(pen, rc);
            }

            using (Brush brush = new SolidBrush(Color.Black))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Far;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(content.Name, m_font, brush, new RectangleF(x, y, w - CurveDescColorW - margin*2, h), sf);
            }
        }

        private Rectangle CalcDrawCurveDescriptionArea(Graphics g, int x, int y, int w, int h, CurveContent content)
        {
            int margin = 4;
            int x2 = x + w - CurveDescColorW - margin;
            int y2 = y;
            int w2 = CurveDescColorW;
            int h2 = h;
            int area_right = x2 + w2;
            int text_right = x + w - CurveDescColorW - margin * 2;
            SizeF size = g.MeasureString(content.Name, m_font);
            int text_left = text_right - (int)size.Width;
            return new Rectangle(text_left, y2, area_right - text_left, h2);
        }

        #endregion Draw Method
        
        #region IDebugParamServer implement

        private event libpixy.net.Tools.DebugParamEvent m_debugParamEvent;

        private void RaiseDebugParamChanged(string name)
        {
            if (m_debugParamEvent != null)
            {
                m_debugParamEvent(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<libpixy.net.Tools.DebugParam> QueryParam(string pattern)
        {
            List<libpixy.net.Tools.DebugParam> items = new List<libpixy.net.Tools.DebugParam>();

            libpixy.net.Tools.WildcardSelector selector = new libpixy.net.Tools.WildcardSelector(pattern);

            if (selector.Match("ruler.actual.x.start"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("ruler.actual.x.start", m_rulerActual.x.start));
            }
            if (selector.Match("ruler.actual.y.start"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("ruler.actual.y.start", m_rulerActual.y.start));
            }


            if (selector.Match("canvas.origin.x"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("canvas.origin.x", this.m_canvas.Origin.X));
            }
            if (selector.Match("canvas.origin.y"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("canvas.origin.y", this.m_canvas.Origin.Y));
            }
            if (selector.Match("canvas.scale.x"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("canvas.scale.x", this.m_canvas.Scale.X));
            }
            if (selector.Match("canvas.scale.y"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("canvas.scale.y", this.m_canvas.Scale.Y));
            }
            if (selector.Match("canvas.cursor.x"))
            {
                Point cp = ClientToCanvas(m_mouse.Location, false);
                items.Add(new libpixy.net.Tools.DebugParam("canvas.cursor.x", cp.X));
            }
            if (selector.Match("canvas.cursor.y"))
            {
                Point cp = ClientToCanvas(m_mouse.Location, false);
                items.Add(new libpixy.net.Tools.DebugParam("canvas.cursor.y", cp.Y));
            }
            if (selector.Match("canvas.len.x"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("canvas.len.x", this.XTickLength));
            }
            if (selector.Match("canvas.len.y"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("canvas.len.y", this.YTickLength));
            }
            if (selector.Match("canvas.client.x"))
            {
                Point cp = ClientToCanvas(m_mouse.Location, true);
                Point pp = CanvasToClient(cp);
                items.Add(new libpixy.net.Tools.DebugParam("canvas.client.x", pp.X));
            }
            if (selector.Match("canvas.client.y"))
            {
                Point cp = ClientToCanvas(m_mouse.Location, true);
                Point pp = CanvasToClient(cp);
                items.Add(new libpixy.net.Tools.DebugParam("canvas.client.y", pp.Y));
            }
            if (selector.Match("canvas.value.x"))
            {
                PointF evaluate = ClientToValue(m_mouse.Location);
                items.Add(new libpixy.net.Tools.DebugParam("canvas.value.x", evaluate.X));
            }
            if (selector.Match("canvas.value.y"))
            {
                PointF evaluate = ClientToValue(m_mouse.Location);
                items.Add(new libpixy.net.Tools.DebugParam("canvas.value.y", evaluate.Y));
            }

            if (selector.Match("mouse.enter"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.enter", m_mouse.Enter));
            }
            if (selector.Match("mouse.down"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.down", m_mouse.Press));
            }
            if (selector.Match("mouse.location.x"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.location.x", m_mouse.Location.X));
            }
            if (selector.Match("mouse.location.y"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.location.y", m_mouse.Location.Y));
            }
            if (selector.Match("mouse.old_location.x"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.old_location.x", m_mouse.OldLocation.X));
            }
            if (selector.Match("mouse.old_location.y"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.old_location.y", m_mouse.OldLocation.Y));
            }
            if (selector.Match("mouse.buttons"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.buttons", m_mouse.Button.ToString()));
            }
            if (selector.Match("mouse.operation"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.operation", m_mouse.Operation.ToString()));
            }

            if (selector.Match("mouse.selection.start_pos.x"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.selection.start_pos.x", m_mouse.Selection.StartPos.X));
            }
            if (selector.Match("mouse.selection.start_pos.y"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.selection.start_pos.y", m_mouse.Selection.StartPos.Y));
            }
            if (selector.Match("mouse.selection.end_pos.x"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.selection.end_pos.x", m_mouse.Selection.EndPos.X));
            }
            if (selector.Match("mouse.selection.end_pos.y"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.selection.end_pos.y", m_mouse.Selection.EndPos.Y));
            }
            if (selector.Match("mouse.selection.valid"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.selection.valid", m_mouse.Selection.Valid));
            }

            if (selector.Match("mouse.drag.location.x"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.drag.location.x", m_mouse.Drag.Location.X));
            }
            if (selector.Match("mouse.drag.location.y"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.drag.location.y", m_mouse.Drag.Location.Y));
            }
            if (selector.Match("mouse.drag.valid"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.drag.valid", m_mouse.Drag.Valid));
            }
            if (selector.Match("mouse.drag.target"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.drag.target", m_mouse.DragTarget.ToString()));
            }
            if (selector.Match("mouse.drag.control_point_index"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("mouse.drag.control_point_index", m_mouse.DragControlPointIndex));
            }

            if (selector.Match("touch.control_point"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("touch.control_point", this.m_touch.ControlPointIndex));
            }

            if (selector.Match("touch.curve"))
            {
                items.Add(new libpixy.net.Tools.DebugParam("touch.curve", this.m_touch.CurveIndex));
            }

            return items;
        }

        public void Connect(libpixy.net.Tools.DebugParamEvent ev)
        {
            m_debugParamEvent += ev;
        }

        public void Disconnect(libpixy.net.Tools.DebugParamEvent ev)
        {
            m_debugParamEvent -= ev;
        }


        #endregion IDebugParamServer implement

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurveEditor_Paint(object sender, PaintEventArgs e)
        {
            Region oldRegion = e.Graphics.Clip;

            e.Graphics.ResetTransform();

            DrawBackground(e.Graphics);
            DrawGrid(e.Graphics);

            this.BeginDrawInCanvas(e.Graphics);

            foreach (CurveContent content in m_document.SubContents)
            {
                DrawContent(e.Graphics, content, false);
            }

            DrawContent(e.Graphics, m_document.Content, true);

            e.Graphics.ResetTransform();

            DrawRuler(e.Graphics);
            DrawCursor(e.Graphics);
            DrawIndicator(e.Graphics);
            DrawMarkers(e.Graphics);
            DrawSelection(e.Graphics);
            DrawCurveDescription(e.Graphics);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurveEditor_MouseDown(object sender, MouseEventArgs e)
        {
            m_mouse.Press = true;
            m_mouse.Button = e.Button;
            m_mouse.OldLocation = m_mouse.Location;
            m_mouse.Location = e.Location;
            m_mouse.ModifierKeys = Control.ModifierKeys;

            if (CheckDragControlPoint(e))
            {
                return;
            }

            m_document.ClearSelection();

            if (CheckCurveClicked(e))
            {
                return;
            }

            if (CheckDragCanvas(e))
            {
                return;
            }

            if (CheckZoomCanvas(e))
            {
                return;
            }

            if (CheckDragIndicator(e))
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (this.CanvasMouseDown != null)
                {
                    this.CanvasMouseDown(sender, e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurveEditor_MouseUp(object sender, MouseEventArgs e)
        {
            m_mouse.Press = false;
            m_mouse.Button = MouseButtons.None;

            if (EndDrag())
            {
                return;
            }

            if (EndSelection())
            {
                return;
            }

            if (CheckCurveDescriptionClicked(e))
            {
                return;
            }

            if (this.CanvasMouseUp != null)
            {
                this.CanvasMouseUp(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurveEditor_MouseMove(object sender, MouseEventArgs e)
        {
            m_mouse.OldLocation = m_mouse.Location;
            m_mouse.Location = e.Location;

            ClearTouch();

            if (UpdateDrag(e) == false)
            {
                if (UpdateSelection(e) == false)
                {
                    CheckTouchControlPoint(e.X, e.Y);
                    CheckTouchCurve(e.X, e.Y);
                    CheckTouchIndicator(e.X, e.Y);
                    CheckSelection(e);
                }
            }

            UpdatePlotCursor(e);
            this.Invalidate();
        }

        private void CurveEditor_MouseEnter(object sender, EventArgs e)
        {
            m_mouse.Enter = true;
            this.Invalidate();
        }

        private void CurveEditor_MouseLeave(object sender, EventArgs e)
        {
            m_mouse.Enter = false;
            this.Invalidate();
        }

        void m_document_Changed(object sender, EventArgs e)
        {
            EndDrag();
            EndSelection();
            m_touch.Clear();
        }

        #endregion Event Handlers
    }
}
