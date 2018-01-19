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
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.Xml;

namespace libpixy.net.Controls.Diagram
{
    public partial class Canvas : UserControl
    {
        #region Fields

        /// <summary>
        /// マウス
        /// </summary>
        public class ToolMouse : Tool.Mouse
        {
            public ToolMouse() : base() { }
            public ToolNodeDragging DragNode = null;
            public ToolMouseDragging DragCanvas = null;
            public ToolMouseDragging DragNavi = null;
            public ToolMouseDragging ZoomCanvas = null;
            public bool MouseClickHook = false;
            public bool Handled = false;
        }

        private Document m_document;
        private Controller m_ctrl = new Controller();
        private ToolMouse m_mouse = new ToolMouse();
        private DrawContext m_drawContext = new DrawContext();
        private ToolPortLinking m_portLinking = null;
        private Tool.Canvas m_canvas = new libpixy.net.Controls.Tool.Canvas();
        private Rectangle m_naviArea = new Rectangle();
        private Canvas m_navigateTarget = null;
        private IDrawConnection m_drawConnection = new DrawCurvedConnection();
        //private IDrawConnection m_drawConnection = new DrawLineConnection();
        private bool m_navi = false;

        #endregion // Fields

        #region Public Events

        /// <summary>
        /// 
        /// </summary>
        public class DragDropLinkEventArgs
        {
            public libpixy.net.Controls.Diagram.Port Port = null;
            public Point loc;
        }

        public delegate void DragDropLinkHandler(DragDropLinkEventArgs e);
        public event DragDropLinkHandler DragDropLink;
        public event DragDropLinkHandler DragDropUnlink;

        public event EventHandler BeginSelection;
        public event EventHandler EndSelection;
        public event EventHandler NodeSelected;
        public event EventHandler CanvasClick;
        public event EventHandler CanvasScaleChanged;
        public event EventHandler CanvasDragged;
        public event EventHandler CanvasDragging;
        public event EventHandler BeginCanvasDrag;
        public event EventHandler NaviDragging;

        /// <summary>
        /// 
        /// </summary>
        private void RaiseNodeSelected()
        {
            if (NodeSelected != null)
            {
                NodeSelected(this, null);
            }
        }

        #endregion Public Events

        #region Properties

        /// <summary>
        /// グリッドサイズ
        /// </summary>
        public int GridSize { get; set; }

        /// <summary>
        /// グリッドにスナップ
        /// </summary>
        public bool SnapGrid = false;

        /// <summary>
        /// グリッドを表示
        /// </summary>
        public bool ShowGrid = true;

        /// <summary>
        /// キャンバスのスケール
        /// </summary>
        [Browsable(true)]
        public PointF CanvasScale
        {
            get { return m_canvas.Scale; }

            set
            {
                m_canvas.Zoom(libpixy.net.Vecmath.Utils.GetCenter(this.ClientRectangle), value);
                Invalidate();

                if (CanvasScaleChanged != null)
                {
                    CanvasScaleChanged(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// ナビゲーションモードの設定
        /// </summary>
        [Browsable(true)]
        public bool NavigationMode
        {
            get { return m_navi; }
            set { m_navi = value; }
        }

        /// <summary>
        /// 表示エリア
        /// </summary>
        [Browsable(false)]
        public Rectangle DispArea
        {
            get
            {
                Point lt = PointToCanvas(new Point(0, 0));
                Point rb = PointToCanvas(new Point(this.ClientSize.Width, this.ClientSize.Height));
                return new Rectangle(lt.X, lt.Y, rb.X - lt.X, rb.Y - lt.Y);
            }
        }

        /// <summary>
        /// ナビゲージョン領域
        /// </summary>
        [Browsable(false)]
        public Rectangle NaviArea
        {
            get { return m_naviArea; }
            set { m_naviArea = value; }
        }

        /// <summary>
        /// ナビゲーション対象
        /// </summary>
        [Browsable(false)]
        public Canvas NavigateTarget
        {
            get { return m_navigateTarget; }
            set
            {
                if (m_navigateTarget != null)
                {
                    m_navigateTarget.CanvasScaleChanged -= NaviTarget_CanvasScaleChanged;
                    m_navigateTarget.CanvasDragging -= NaviTarget_CanvasDragging;
                    m_navigateTarget.NaviDragging -= NaviTarget_NaviDragging;
                }

                m_navigateTarget = value;

                if (m_navigateTarget != null)
                {
                    m_navigateTarget.CanvasScaleChanged += NaviTarget_CanvasScaleChanged;
                    m_navigateTarget.CanvasDragging += NaviTarget_CanvasDragging;
                    m_navigateTarget.NaviDragging += NaviTarget_NaviDragging;
                }
            }
        }

        /// <summary>
        /// ドキュメント
        /// </summary>
        [Browsable(true)]
        public Document Document
        {
            get { return m_document; }
            set
            {
                m_document = value;
                m_document.Changed += new ChangedEvent(m_document_Changed);
                m_ctrl.Document = m_document;
            }
        }

        /// <summary>
        /// マウス情報
        /// </summary>
        [Browsable(false)]
        public ToolMouse MouseInfo { get { return m_mouse; } }

        /// <summary>
        /// ノードの色
        /// </summary>
        public Color FocusNodeColor = Color.FromArgb(128, 0, 0, 255);

        /// <summary>
        /// ノードの選択色
        /// </summary>
        public Color SelectNodeColor = Color.Yellow;

        /// <summary>
        /// ノードの接触色
        /// </summary>
        public Color HitNodeColor = Color.FromArgb(128, 0, 0, 255);

        /// <summary>
        /// ノードの選択枠色
        /// </summary>
        public Color SelectionColor = Color.FromArgb(255, 0, 0, 255);

        /// <summary>
        /// ナビゲーション領域の色
        /// </summary>
        public Color NaviAreaColor = Color.Red;

        /// <summary>
        /// グリッドの背景色
        /// </summary>
        public Color GridBackColor = Color.FromArgb(172, 168, 168);

        /// <summary>
        /// グリッドの前景色
        /// </summary>
        public Color GridForeColor = Color.FromArgb(164, 160, 160);

        #endregion // Properties

        #region Constructor / Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Canvas()
        {
            this.GridSize = 20;
            InitializeComponent();

            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint, true);
            this.DoubleBuffered = true;

            m_document = new Document();
            m_ctrl.Document = m_document;

            this.NavigationMode = false;
        }

        /// <summary>
        /// 
        /// </summary>
        ~Canvas()
        {
            m_drawContext.Dispose();
            m_drawContext = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            m_document.Clear();
            m_canvas.Origin = new Point(0, 0);
            m_canvas.Scale = new PointF(1.0f, 1.0f);
        }

        /// <summary>
        /// ノードを描画する前の準備
        /// </summary>
        public void UpdateNodeState()
        {
            if (this.Document.NodeCount == 0)
            {
                return;
            }

            foreach (Node node in this.Document.Nodes)
            {
                node.State.Work = false;
                node.State.Hide = false;
            }

            foreach (Node node in this.Document.Nodes)
            {
                if (node.State.Work)
                {
                    continue;
                }

                if (node.DestinationFolded)
                {
                    foreach (Port port in node.DestinationPorts)
                    {
                        HideNodes(port, true);
                    }
                }

                if (node.ChildFolded)
                {
                    HideNodes(node.ChildPort, true);
                }

                node.State.Work = true;
            }
        }

        /// <summary>
        /// ノードを非表示にする
        /// </summary>
        /// <param name="port"></param>
        /// <param name="recursive"></param>
        private void HideNodes(Port port, bool recursive)
        {
            if (port == null)
            {
                return;
            }

            if (port.Connected == false)
            {
                return;
            }

            foreach (Link link in port.Connections)
            {
                link.Port2.Owner.State.Hide = true;
                HideNode(link.Port2.Owner, true);
            }
        }

        /// <summary>
        /// ノードを非表示にする
        /// </summary>
        /// <param name="port"></param>
        /// <param name="recursive"></param>
        private void HideNode(Node node, bool recursive)
        {
            node.State.Hide = true;
            node.State.Work = true;

            if (node.DestinationPorts != null && node.DestinationPorts.Count > 0)
            {
                foreach (Port port in node.DestinationPorts)
                {
                    HideNodes(port, true);
                }
            }

            if (node.ChildPort != null)
            {
                HideNodes(node.ChildPort, true);
            }
        }

        #endregion // Methods

        #region Draw

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            m_canvas.ResetTransform(e.Graphics);

            ClearBackground(e.Graphics);
            DrawGrid(e.Graphics);

            m_canvas.SetTransform(e.Graphics);

            //PrepareDrawNodes();
            DrawNodeConnections(e.Graphics);
            DrawNodes(e.Graphics);
            DrawDragLink(e.Graphics);
            DrawNodeSelection(e.Graphics);
            DrawNodeLabel(e.Graphics);
            DrawFrameSelection(e.Graphics);
            DrawConnectNode(e.Graphics);

            m_canvas.ResetTransform(e.Graphics);

            DrawNaviArea(e.Graphics);

            //base.OnPaint(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        /// <summary>
        /// 
        /// </summary>
        public class DrawItemEventArgs
        {
            public Graphics Graphics = null;
            public Node Item = null;
            public Color BackColor;
            public Color ForeColor;
            public Color SeparateColor;
            public Color SubTextColor;
            public DrawContext Context = null;
            public bool DrawDefault = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public class DrawConnectionEventArgs
        {
            public Graphics Graphics = null;
            public Link Item = null;
            public Color BackColor;
            public Color ForeColor;
            public bool DrawDefault = false;
        }

        public delegate void DrawItemHandler(DrawItemEventArgs e);
        public delegate void DrawConnectionItemHandler(DrawConnectionEventArgs e);

        public event DrawItemHandler DrawItem;
        public event DrawItemHandler PostDrawItem;
        public event DrawConnectionItemHandler DrawConnectionItem;

        public enum ConnectionAlignment
        {
            Left, Up, Right, Bottom
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawNodes(Graphics g)
        {
            foreach (Node node in this.Document.Nodes)
            {
                if (node.State.Hide == false)
                {
                    DrawNode(node, g);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawNodeLabel(Graphics g)
        {
            Font fontLabel = new Font("Tahoma", 9);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            foreach (Node node in this.Document.Nodes)
            {
                if (string.IsNullOrEmpty(node.Label))
                {
                    continue;
                }

                if (node.State.Hide)
                {
                    continue;
                }

                Rectangle rc = node.Bounds;

                int width = rc.Width/2 - 8;// 8 = radius
                SizeF size = g.MeasureString(node.Label, fontLabel);
                System.Drawing.Size ext = new Size((int)size.Width + 4, (int)size.Height);
                int labelH = ext.Height + 4;//margin
                Rectangle rcLabel = new Rectangle(rc.Left + width / 2 - ext.Width / 2, rc.Top - labelH - 6, ext.Width, labelH);
                Point ptBalloon = new Point(rc.Left + width / 2, rc.Top);
                if (width < rcLabel.Width)
                {
                    rcLabel.Offset((width - rcLabel.Width)/2, 0);
                }

                int bw = (rcLabel.Width < 16) ? rcLabel.Width / 2 : 16;
                libpixy.net.Controls.Diagram.Draw.DrawBalloon(g, rcLabel, ptBalloon, bw, node.BackColor);
                using (Brush b = new SolidBrush(node.ForeColor))
                {
                    g.DrawString(node.Label, fontLabel, b, rcLabel, sf);
                }
            }

            fontLabel.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawDragLink(Graphics g)
        {
            if (m_portLinking != null)
            {
                m_drawConnection.Draw(g, m_portLinking.Port, PointToCanvas(this.PointToClient(Cursor.Position)), true);

#if false
                if (m_portLinking.Port.Type == "node")
                {
                    Draw.DrawCurveAndArrow(
                        g,
                        m_portLinking.Port.ConnectionPoint,
                        PointToView(this.PointToClient(Cursor.Position)),
                        Color.Black,
                        2,
                        Draw.CurveDir.Vertical);
                }
                else
                {
                    Draw.DrawCurveAndArrow(
                        g,
                        m_portLinking.Port.ConnectionPoint,
                        PointToView(this.PointToClient(Cursor.Position)),
                        Color.Black,
                        2,
                        Draw.CurveDir.Horizontal);
                }
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawNodeSelection(Graphics g)
        {
            if (this.IsNodeSelectionDragging())
            {
                Rectangle rc = m_mouse.Selection.GetRect();
                using (Brush brush = new SolidBrush(Color.FromArgb(
                    64,
                    this.SelectionColor.R,
                    this.SelectionColor.G,
                    this.SelectionColor.B)))
                {
                    g.FillRectangle(brush, rc);
                }

                using (Pen pen = new Pen(this.SelectionColor))
                {
                    g.DrawRectangle(pen, rc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawFrameSelection(Graphics g)
        {
            if (IsCanvasFraming())
            {
                Rectangle rc = m_mouse.Selection.GetRect();
                using (Brush brush = new SolidBrush(Color.FromArgb(64,255,255,0)))
                {
                    g.FillRectangle(brush, rc);
                }

                using (Pen pen = new Pen(Color.FromArgb(255,255,0)))
                {
                    g.DrawRectangle(pen, rc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawNaviArea(Graphics g)
        {
            if (NavigationMode)
            {
                using (Pen pen = new Pen(this.NaviAreaColor))
                {
                    Rectangle rc = m_canvas.RectToView(m_naviArea);
                    g.DrawRectangle(pen, rc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void ClearBackground(Graphics g)
        {
            using (Brush brush = new SolidBrush(this.GridBackColor))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void DrawGrid(Graphics g)
        {
            if (!ShowGrid)
            {
                return;
            }

            Pen pen = new Pen(this.GridForeColor);

            Point vpos1 = PointToCanvas(new Point(0, 0));
            Point vpos2 = PointToCanvas(new Point(this.ClientSize.Width, this.ClientSize.Height));

            int cw = libpixy.net.Vecmath.Utils.RoundUp(vpos2.X - vpos1.X, GridSize);
            int ch = libpixy.net.Vecmath.Utils.RoundUp(vpos2.Y - vpos1.Y, GridSize);
            int divx = cw / GridSize + 2;
            int divy = ch / GridSize + 2;
            cw = divx * GridSize;
            ch = divy * GridSize;

            Point[] pt1 = new Point[divx * 2];
            Point[] pt2 = new Point[divy * 2];

            int orgX = libpixy.net.Vecmath.Utils.RoundDown(vpos1.X, GridSize);
            int orgY = libpixy.net.Vecmath.Utils.RoundDown(vpos1.Y, GridSize);
            int sw = 0;
            int i = 0;
            for (int x = 0; x < divx; ++x)
            {
                pt1[i + sw].X = orgX + x * GridSize;
                pt1[i + sw].Y = orgY;
                pt1[i + 1 - sw].X = orgX + x * GridSize;
                pt1[i + 1 - sw].Y = orgY + ch;
                pt1[i + 0] = PointToView(pt1[i + 0]);
                pt1[i + 1] = PointToView(pt1[i + 1]);
                i += 2;
                sw = 1 - sw;
            }

            i = 0;
            sw = 0;
            for (int y = 0; y < divy; ++y)
            {
                pt2[i + sw].X = orgX;
                pt2[i + sw].Y = orgY + y * GridSize;
                pt2[i + 1 - sw].X = orgX + cw;
                pt2[i + 1 - sw].Y = orgY + y * GridSize;
                pt2[i + 0] = PointToView(pt2[i + 0]);
                pt2[i + 1] = PointToView(pt2[i + 1]);
                i += 2;
                sw = 1 - sw;
            }

            g.DrawLines(pen, pt1);
            g.DrawLines(pen, pt2);

            pen.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gfx"></param>
        private void DrawNodeConnections(Graphics gfx)
        {
            foreach (Node node in m_document.Nodes)
            {
                DrawNodeConnection(gfx, node);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="node"></param>
        private void DrawNodeConnection(Graphics gfx, Node node)
        {
            if (node.State.Hide)
            {
                return;
            }

            DrawConnectionEventArgs args = new DrawConnectionEventArgs();
            args.Graphics = gfx;

            // Draw Children
            if (node.ChildPort != null && node.ChildPort.Connected && node.ChildFolded == false)
            {
                args.DrawDefault = true;
                foreach (Link link in node.ChildPort.Connections)
                {
                    args.Item = link;
                    if (DrawConnectionItem != null)
                    {
                        DrawConnectionItem(args);
                    }

                    if (args.DrawDefault)
                    {
                        m_drawConnection.Draw(gfx, link.Port1, link.Port2, false);
                    }
                }
            }

            if (node.DestinationFolded)
            {
                return;
            }

            // Draw link
            foreach (Port port in node.DestinationPorts)
            {
                if (port.Connected)
                {
                    foreach (Link link in port.Connections)
                    {
                        args.DrawDefault = true;
                        if (this.DrawConnectionItem != null)
                        {
                            args.Item = link;
                            DrawConnectionItem(args);
                        }

                        if (args.DrawDefault)
                        {
                            m_drawConnection.Draw(gfx, link.Port1, link.Port2, false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="g"></param>
        private void DrawNode(Node node, Graphics g)
        {
            DrawItemEventArgs e = new DrawItemEventArgs();
            e.Item = node;
            e.Graphics = g;
            e.BackColor = node.BackColor;
            e.ForeColor = node.ForeColor;
            e.SeparateColor = node.SeparateColor;
            e.SubTextColor = node.SubTextColor;
            e.DrawDefault = true;
            if (this.DrawItem != null)
            {
                DrawItem(e);
            }

            if (!e.DrawDefault)
            {
                return;
            }

            m_drawContext.Reset(e.ForeColor, e.BackColor, e.SeparateColor);
            e.Context = m_drawContext;

            Rectangle rc = node.Bounds;

            // 折り畳んだ子ノードの描画
            if (node.ChildFolded && node.ChildConnected)
            {
                Rectangle rcChild = new Rectangle(0, 0, 20, 12);
                rcChild.Offset(libpixy.net.Vecmath.Utils.GetCenter(rc).X-10, rc.Bottom + 15);
                Draw.FillRoundedRectangle(g, rcChild, 10, e.BackColor);
                Draw.DrawRoundedRectangle(g, rcChild, 10, System.Drawing.Color.Black, 2);

                Point p1 = new Point(libpixy.net.Vecmath.Utils.GetCenter(rc).X, rc.Bottom);
                Point p2 = new Point(p1.X, rcChild.Top);
                libpixy.net.Controls.Diagram.Draw.DrawCurve(
                    g, p1, 90, p2, 270, System.Drawing.Color.Black, 2, false);
            }

            // 折り畳んだ出力ノードの描画
            if (node.DestinationFolded && node.DestinationConnected)
            {
                Rectangle rcChild = new Rectangle(0, 0, 20, 12);
                rcChild.Offset(rc.Right + 10, libpixy.net.Vecmath.Utils.GetCenter(rc).Y - 6);
                Draw.FillRoundedRectangle(g, rcChild, 10, e.BackColor);
                Draw.DrawRoundedRectangle(g, rcChild, 10, System.Drawing.Color.Black, 2);

                Point p1 = new Point(rc.Right, libpixy.net.Vecmath.Utils.GetCenter(rc).Y);
                Point p2 = new Point(rcChild.Left, p1.Y);
                libpixy.net.Controls.Diagram.Draw.DrawCurve(
                    g, p1, 0, p2, 180, System.Drawing.Color.Black, 2, false);
            }

            // Draw background
            Draw.FillRoundedRectangle(g, rc, 10, e.BackColor);

            // Draw frame
            if (node.State.Select)
            {
                Rectangle frame = new Rectangle(rc.Left, rc.Top, rc.Width, rc.Height);
                frame.Inflate(2, 2);
                Draw.DrawRoundedRectangle(g, frame, 10, this.SelectNodeColor, 2);
            }
            else if (node.State.Hit)
            {
                Rectangle frame = new Rectangle(rc.Left, rc.Top, rc.Width, rc.Height);
                frame.Inflate(2, 2);
                Draw.DrawRoundedRectangle(g, frame, 10, this.HitNodeColor, 4);
            }

            // Draw border
            Draw.DrawRoundedRectangle(g, rc, 10, Color.Black, 2);

            // Draw text
            rc.Height = Node.NAME_HEIGHT;
            g.DrawString(node.Text, m_drawContext.fontHead, m_drawContext.foreBrush, rc, m_drawContext.sf);

            // Draw SubText
            if (string.IsNullOrEmpty(node.SubText) == false)
            {
                using (Brush brSubText = new SolidBrush(e.SubTextColor))
                {
                    int y = libpixy.net.Vecmath.Utils.GetCenter(rc).Y + m_drawContext.fontHead.Height / 2;
                    Rectangle rcSubText = new Rectangle(rc.X, y, rc.Width, Node.EXTNAME_HEIGHT);
                    g.DrawString(node.SubText, m_drawContext.fontSubText, brSubText, rcSubText, m_drawContext.sf);
                    rc.Height += Node.EXTNAME_HEIGHT;
                }
            }

            if (node.Folded == false)
            {
                if (node.SourcePorts.Count > 0 || node.DestinationPorts.Count > 0)
                {
                    g.DrawLine(m_drawContext.linePen, rc.Left, rc.Bottom, rc.Right, rc.Bottom);
                }
            }

            // Draw folding button
            g.FillRectangle(Brushes.White, node.FoldingButtonBounds);
            g.DrawRectangle(Pens.Black, node.FoldingButtonBounds);
            g.DrawLine(Pens.Black,
                new Point(node.FoldingButtonBounds.Left + 2, libpixy.net.Vecmath.Utils.GetCenter(node.FoldingButtonBounds).Y),
                new Point(node.FoldingButtonBounds.Right - 2, libpixy.net.Vecmath.Utils.GetCenter(node.FoldingButtonBounds).Y));
            if (node.Folded)
            {
                g.DrawLine(Pens.Black,
                    new Point(libpixy.net.Vecmath.Utils.GetCenter(node.FoldingButtonBounds).X, node.FoldingButtonBounds.Top + 2),
                    new Point(libpixy.net.Vecmath.Utils.GetCenter(node.FoldingButtonBounds).X, node.FoldingButtonBounds.Bottom - 2));
            }

            // Draw destination folding button
            g.FillRectangle(Brushes.White, node.DestFoldingButtonBounds);
            g.DrawRectangle(Pens.Black, node.DestFoldingButtonBounds);
            g.DrawLine(Pens.Black,
                new Point(node.DestFoldingButtonBounds.Left + 2, libpixy.net.Vecmath.Utils.GetCenter(node.DestFoldingButtonBounds).Y),
                new Point(node.DestFoldingButtonBounds.Right - 2, libpixy.net.Vecmath.Utils.GetCenter(node.DestFoldingButtonBounds).Y));
            if (node.DestinationFolded)
            {
                g.DrawLine(Pens.Black,
                    new Point(libpixy.net.Vecmath.Utils.GetCenter(node.DestFoldingButtonBounds).X, node.DestFoldingButtonBounds.Top + 2),
                    new Point(libpixy.net.Vecmath.Utils.GetCenter(node.DestFoldingButtonBounds).X, node.DestFoldingButtonBounds.Bottom - 2));
            }

            if (node.Folded)
            {
                if (node.ParentPort != null)
                {
                    Draw.DrawPortCircle(g, node.ParentPort);
                }

                if (node.ChildPort != null)
                {
                    Draw.DrawPortCircle(g, node.ChildPort);
                }

                if (node.SourcePorts.Count > 0)
                {
                    Draw.DrawPortCircle(g, node.SourcePorts[0]);
                }

                if (node.DestinationPorts.Count > 0)
                {
                    Draw.DrawPortCircle(g, node.DestinationPorts[0]);
                }

                return;
            }

            // Draw port
            rc.Height = Node.PORT_HEIGHT;
            rc.Y += node.NodeNameHeight;

            // margin
            rc.X += 8;
            rc.Width -= 16;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            foreach (Port port in node.SourcePorts)
            {
                g.DrawString(port.Text, m_drawContext.fontPort, m_drawContext.foreBrush, rc, sf);
                rc.Y += Node.PORT_HEIGHT;

                Draw.DrawPortCircle(g, port);
            }

            if (node.SourcePorts.Count > 0)
            {
                g.DrawLine(m_drawContext.linePen, node.Bounds.Left, rc.Top, node.Bounds.Right, rc.Top);
            }

            sf.Alignment = StringAlignment.Far;
            foreach (Port port in node.DestinationPorts)
            {
                g.DrawString(port.Text, m_drawContext.fontPort, m_drawContext.foreBrush, rc, sf);
                rc.Y += Node.PORT_HEIGHT;

                Draw.DrawPortCircle(g, port);
            }

            if (node.ParentPort != null)
            {
                Draw.DrawPortCircle(g, node.ParentPort);
            }

            if (node.ChildPort != null)
            {
                Draw.DrawPortCircle(g, node.ChildPort);
            }

            if (this.PostDrawItem != null)
            {
                PostDrawItem(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="g"></param>
        private void DrawConnectNode(Graphics g)
        {
            if (this.Document.IsNodeSelected == false) {
                return;
            }

            if (this.IsNodeDragging() == false) {
                return;
            }

            if (m_mouse.DragNode.ConnectMode == false) {
                return;
            }

            Color c = Color.FromArgb(64,255,255,255);
            if (m_mouse.DragNode.ConnectLink != null) {
                c = Color.FromArgb(128,255,255,0);
            }

            Node node = this.Document.SelectedNode;
            Color foreColor = libpixy.net.Vecmath.Utils.ColorModulate(node.ForeColor, c);
            Color backColor = libpixy.net.Vecmath.Utils.ColorModulate(node.BackColor, c);
            Color sepColor = libpixy.net.Vecmath.Utils.ColorModulate(node.SeparateColor, c);
            m_drawContext.Reset(foreColor, backColor, sepColor);
            Rectangle rc = node.Bounds;
            rc.Location = m_mouse.DragNode.ConnectPoint;

            // Draw background
            Draw.FillRoundedRectangle(g, rc, 10, backColor);

            // Draw border
            Draw.DrawRoundedRectangle(g, rc, 10, foreColor, 2);

            // Draw text
            rc.Height = Node.NAME_HEIGHT;
            g.DrawString(node.Text, m_drawContext.fontHead, m_drawContext.foreBrush, rc, m_drawContext.sf);
        }

        #endregion // Draw

        #region Mouse

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramView_MouseDown(object sender, MouseEventArgs e)
        {
            m_mouse.Location = Cursor.Position;
            m_mouse.Button = e.Button;
            m_mouse.Press = true;
            m_mouse.ModifierKeys = Control.ModifierKeys;
            m_mouse.Handled = false;
            if (m_navi)
            {
                Navi_MouseDown(sender, e);
            }
            else
            {
                Edit_MouseDown(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramView_MouseMove(object sender, MouseEventArgs e)
        {
            m_mouse.OldLocation = m_mouse.Location;
            m_mouse.Location = e.Location;
            m_mouse.MouseClickHook = false;
            if (m_navi)
            {
                Navi_MouseMove(sender, e);
            }
            else
            {
                Edit_MouseMove(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiagramView_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_navi)
            {
                Navi_MouseUp(sender, e);
            }
            else
            {
                Edit_MouseUp(sender, e);
            }
            m_mouse.Press = false;
            m_mouse.Button = MouseButtons.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_MouseDown(object sender, MouseEventArgs e)
        {
            Point loc = m_canvas.PointToCanvas(e.Location);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    BeginDragNode(loc);
                    break;

                case MouseButtons.Right:
                    break;

                case MouseButtons.Middle:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsNodeDragging())
            {
                DragNode();
            }
            else if (IsPortLinking())
            {
                //nop
            }
            else if (IsCanvasFraming())
            {
                UpdateFrameCanvas();
            }
            else if (IsNodeSelectionDragging())
            {
                DragNodeSelection();
            }
            else if (IsCanvasDragging())
            {
                DragCanvas();
            }
            else if (IsCanvasZooming())
            {
                ZoomCanvas();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                ZoomCanvas();
            }
            else
            {
                if (MouseInfo.Press)
                {
                    if (MouseInfo.Button == MouseButtons.Left)
                    {
                        if (MouseInfo.ModifierKeys == Keys.Alt)
                        {
                            BeginFrameCanvas();
                        }
                        else
                        {
                            BeginDragNodeSelection(e.Location);
                        }
                    }
                    else if (MouseInfo.Button == MouseButtons.Right)
                    {
                        if (MouseInfo.ModifierKeys == Keys.Alt)
                        {
                            BeginZoomCanvas();
                        }
                        else
                        {
                            BeginDragCanvas();
                        }
                    }
                    else if (MouseInfo.Button == MouseButtons.Middle)
                    {
                        BeginZoomCanvas();
                    }
                }
            }

            // Hit Check

            Point pt = PointToCanvas(e.Location);
            foreach (Node node in Document.Nodes)
            {
                if (node.HitTest(pt))
                {
                    node.State.Hit = true;
                }
                else
                {
                    node.State.Hit = false;
                }
            }

            Invalidate();

#if false// Debug
            Point loc = PointToView(e.Location);
            Graphics g = CreateGraphics();
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, this.ClientSize.Width, 20));
            g.DrawString(String.Format("{0},{1} ({2},{3})", e.Location.X, e.Location.Y, loc.X, loc.Y),
                this.Font, Brushes.Black, new PointF(0.0f, 0.0f));
            g.Dispose();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_MouseUp(object sender, MouseEventArgs e)
        {
            bool nodeDragging = IsNodeDragging();
            bool nodeSelDragging = IsNodeSelectionDragging();
            bool canvasDragging = IsCanvasDragging();
            bool canvasZooming = IsCanvasZooming();
            bool canvasFraming = IsCanvasFraming();

            EndDragNode();
            EndDragNodeSelection();
            EndDragCanvas();
            EndZoomCanvas();
            EndFrameCanvas();

            if (nodeSelDragging)
            {
                RaiseNodeSelected();
                return;
            }

            if (!nodeDragging && !canvasDragging && !canvasFraming)
            {
                m_ctrl.ClearSelectedNodes();

                if (CanvasClick != null)
                {
                    CanvasClick(null, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Navi_MouseDown(object sender, MouseEventArgs e)
        {
            Point loc = PointToCanvas(e.Location);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    BeginDragNavi(loc);
                    break;

                case MouseButtons.Right:
                    BeginDragCanvas();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Navi_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragNavi();
            }
            else if (e.Button == MouseButtons.Right)
            {
                DragCanvas();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Navi_MouseUp(object sender, MouseEventArgs e)
        {
            EndDragNavi();
            EndDragCanvas();
        }

        #endregion Mouse

        #region Mouse Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        private bool BeginDragNode(Point loc)
        {
            Node node = PickNode(loc);
            if (node == null)
            {
                return false;
            }

            // ノード折り畳みチェック
            if (node.FoldingButtonBounds.Contains(loc))
            {
                m_ctrl.ToggleNodeFolding(node);
                m_mouse.MouseClickHook = true;
                UpdateNodeState();
                return true;
            }

            // 出力ポートお折り畳みチェック
            if (node.DestFoldingButtonBounds.Contains(loc))
            {
                if (node.DestinationFolded)
                {
                    node.DestinationFolded = false;
                }
                else
                {
                    node.DestinationFolded = true;
                }

                m_mouse.MouseClickHook = true;

                UpdateNodeState();
                Invalidate();
                return true;
            }

            // リンク開始チェック
            Port port = node.HitPort(loc);
            if (port != null)
            {
                if (port.Stream == PortStream.In && port.Connected)
                {
                    Link link = port.Connections[0];
                    link.Port1.RemoveConnection(link);
                    link.Port2.RemoveConnection(link);
                    m_portLinking = new ToolPortLinking();
                    m_portLinking.Position = Cursor.Position;
                    m_portLinking.Port = link.Port1;
                    m_portLinking.Connected = true;
                    return true;
                }
                else if (port.Stream == PortStream.Out)
                {
                    m_portLinking = new ToolPortLinking();
                    m_portLinking.Position = Cursor.Position;
                    m_portLinking.Port = port;
                    m_portLinking.Connected = false;
                    return true;
                }
            }

            // ノードを１つ選択した状態で、Shiftキー押しながらノードをクリックすると階層リンクする
            if (m_document.SelectedCount == 1 && m_document.SelectedNode != node && Control.ModifierKeys == Keys.Shift)
            {
                if (m_document.SelectedNode.ParentPort != null && node.ChildPort != null)
                {
                    m_document.SelectedNode.DisconnectParent();
                    m_document.Link(node.ChildPort, m_document.SelectedNode.ParentPort);
                    return true;
                }
            }

            // ノードのドラッグ開始
            m_mouse.DragNode = new ToolNodeDragging();
            m_mouse.DragNode.StartPosition = Cursor.Position;
            m_mouse.DragNode.EndPosition = Cursor.Position;

            if (node.State.Select)
            {
                //nop
            }
            else
            {
                m_ctrl.ClearSelectedNodes();
                m_document.SelectNode(node);
                RaiseNodeSelected();
            }

            // コネクトモード発動チェック
            if (Control.ModifierKeys == Keys.Shift)
            {
                m_mouse.DragNode.ConnectMode = true;
            }

            // 全ノードの位置を保存
            foreach (Node snode in this.Document.Nodes)
            {
                m_mouse.DragNode.NodeLocations[snode] = snode.Location;
            }

            return true;
        }

        /// <summary>
        /// ノードドラッグ終了
        /// </summary>
        private void EndDragNode()
        {
            EndDragNode_Connect();
            EndDragNode_Port();
            m_mouse.DragNode = null;
        }

        /// <summary>
        /// ノードドラッグ（コネクトモード）終了
        /// </summary>
        private void EndDragNode_Connect()
        {
            if (m_mouse.DragNode == null || m_mouse.DragNode.ConnectMode == false || this.Document.IsNodeSelected == false)
            {
                return;
            }

            Node node = this.Document.SelectedNode;

            if (m_mouse.DragNode.ConnectLink != null)
            {
                // 自分とのコネクションは無視
                if (node.ExistsConnection(m_mouse.DragNode.ConnectLink))
                {
                    return;
                }

                m_ctrl.Detach(node);
                m_ctrl.Attach(node, m_mouse.DragNode.ConnectLink);
                m_ctrl.MoveTo(node, m_mouse.DragNode.ConnectPoint);
                m_mouse.DragNode.ConnectLink = null;
                m_mouse.DragNode.ConnectMode = false;
            }
            else
            {
                m_ctrl.Detach(node);
                m_ctrl.MoveTo(node, m_mouse.DragNode.ConnectPoint);
                m_mouse.DragNode.ConnectMode = false;
            }
        }

        /// <summary>
        /// ノードドラッグ（リンク）終了
        /// </summary>
        private void EndDragNode_Port()
        {
            if (m_portLinking == null)
            {
                return;
            }

            bool connect = false;
            Point loc = PointToCanvas(PointToClient(Cursor.Position));
            foreach (Node node in this.Document.Nodes)
            {
                if (node.State.Hide)
                {
                    continue;
                }

                Port port = node.HitPort(loc);
                if (port != null &&//ポートにカーソルがあっているかどうか
                    port.Stream == PortStream.In && //入力ポートか
                    port.Type == m_portLinking.Port.Type && //データタイプは同一か
                    node != m_portLinking.Port.Owner)//自分自身のポートは無効
                {
                    m_document.Link(m_portLinking.Port, port);
                    connect = true;
                    break;
                }

                // ポートがノードタイプなら、当たり範囲をノード全体にする
                if (m_portLinking.Port.Type == "node")
                {
                    if (node.HitTest(loc) && node.ParentPort != null && node != m_portLinking.Port.Owner)
                    {
                        m_document.Link(m_portLinking.Port, node.ParentPort);
                        connect = true;
                        break;
                    }
                }
            }

            if (!connect)
            {
                if (m_portLinking.Connected)
                {
                    // リンク解除
                    if (this.DragDropUnlink != null)
                    {
                        DragDropLinkEventArgs e = new DragDropLinkEventArgs();
                        e.Port = m_portLinking.Port;
                        e.loc = PointToClient(Cursor.Position);
                        this.DragDropUnlink(e);
                    }
                }
                else if (this.DragDropLink != null)
                {
                    DragDropLinkEventArgs e = new DragDropLinkEventArgs();
                    e.Port = m_portLinking.Port;
                    e.loc = PointToClient(Cursor.Position);
                    this.DragDropLink(e);
                }
            }

            m_portLinking = null;

            Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DragNode()
        {
            if (m_mouse.DragNode == null) {
                return;
            }

            if (m_document.IsNodeSelected == false) {
                return;
            }

            if (m_mouse.DragNode.ConnectMode) {
                DragNode_ConnectMode();
            }
            else {
                DragNode_NormalMode();
            }

            m_document.RaiseChangedEvent(ChangedEventType.UPDATE, null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DragNode_ConnectMode()
        {
            Point ptS = PointToCanvas(m_mouse.DragNode.StartPosition);
            Point ptE = PointToCanvas(Cursor.Position);
            Point diff = new Point(ptE.X - ptS.X, ptE.Y - ptS.Y);

            m_ctrl.ClearWorkState();

            Node node = this.Document.SelectedNode;
            Rectangle bounds = new Rectangle(node.Bounds.Location, node.Bounds.Size);
            bounds.Location = m_mouse.DragNode.ConnectPoint;

            Point loc = (Point)m_mouse.DragNode.NodeLocations[node];
            m_mouse.DragNode.ConnectPoint = PointSnapGrid(
                new Point(loc.X + diff.X, loc.Y + diff.Y));

            m_mouse.DragNode.ConnectLink = null;
            foreach (Node target in this.Document.Nodes)
            {
                if (target.ParentConnected)
                {
                    foreach (Link link in target.ParentPort.Connections)
                    {
                        if (m_drawConnection.Intersect(link.Port1, link.Port2, bounds))
                        {
                            m_mouse.DragNode.ConnectLink = link;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DragNode_NormalMode()
        {
            Point ptS = PointToCanvas(m_mouse.DragNode.StartPosition);
            Point ptE = PointToCanvas(Cursor.Position);
            Point diff = new Point(ptE.X - ptS.X, ptE.Y - ptS.Y);

            m_ctrl.ClearWorkState();

            foreach (Node node in this.Document.SelectedNodes)
            {
                Point loc = (Point)m_mouse.DragNode.NodeLocations[node];
                node.Location = PointSnapGrid(
                    new Point(loc.X + diff.X, loc.Y + diff.Y));
                node.ComputeBounds();
                node.State.Work = true;

                if (m_mouse.ModifierKeys == Keys.Control || node.DestinationFolded)
                {
                    m_ctrl.TraverseDestinationNodes(
                        node,
                        delegate(Node target, bool destination)
                        {
                            loc = (Point)m_mouse.DragNode.NodeLocations[target];
                            target.Location = PointSnapGrid(
                                new Point(loc.X + diff.X, loc.Y + diff.Y));
                            target.ComputeBounds();
                            target.State.Work = true;
                        },
                        true
                    );
                }

                if (m_mouse.ModifierKeys == Keys.Control || node.ChildFolded)
                {
                    m_ctrl.TraverseHierarchyNodes(
                        node,
                        delegate(Node target, bool destination)
                        {
                            loc = (Point)m_mouse.DragNode.NodeLocations[target];
                            target.Location = PointSnapGrid(
                                new Point(loc.X + diff.X, loc.Y + diff.Y));
                            target.ComputeBounds();
                            target.State.Work = true;
                        },
                        true,
                        true
                    );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsPortLinking()
        {
            return (m_portLinking != null) ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsNodeDragging()
        {
            return (m_mouse.DragNode != null) ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BeginDragCanvas()
        {
            m_mouse.DragCanvas = new ToolMouseDragging();
            m_mouse.DragCanvas.StartPosition = Cursor.Position;

            if (BeginCanvasDrag != null)
            {
                BeginCanvasDrag(this, new EventArgs());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndDragCanvas()
        {
            if (m_mouse.DragCanvas != null)
            {
                m_mouse.DragCanvas = null;
            }

            if (CanvasDragged != null)
            {
                CanvasDragged(this, new EventArgs());
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        private void DragCanvas()
        {
            if (m_mouse.DragCanvas != null)
            {
                Point ptS = PointToCanvas(m_mouse.DragCanvas.StartPosition);
                Point ptE = PointToCanvas(Cursor.Position);
                Point org = m_canvas.Origin;
                org.Offset(ptE.X - ptS.X, ptE.Y - ptS.Y);
                m_canvas.Origin = org;
                m_mouse.DragCanvas.StartPosition = Cursor.Position;

                if (CanvasDragging != null)
                {
                    CanvasDragging(this, new EventArgs());
                }

                Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsCanvasDragging()
        {
            return (m_mouse.DragCanvas != null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BeginZoomCanvas()
        {
            m_mouse.ZoomCanvas = new ToolMouseDragging();
            m_mouse.ZoomCanvas.StartPosition = Cursor.Position;
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndZoomCanvas()
        {
            if (m_mouse.ZoomCanvas != null)
            {
                m_mouse.ZoomCanvas = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        public void ZoomCanvas(float sx, float sy)
        {
            this.CanvasScale = new PointF(m_canvas.Scale.X + sx, m_canvas.Scale.Y + sy);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ZoomCanvas()
        {
            if (m_mouse.ZoomCanvas != null)
            {
                Point ptS = m_mouse.ZoomCanvas.StartPosition;
                Point ptE = Cursor.Position;
                int dx = ptE.X - ptS.X;
                int dy = ptE.Y - ptS.Y;
                float d = (float)Math.Sqrt(dx * dx + dy * dy) * 0.01f;
                if (dx < 0 || dy < 0) { d = -d; }
                this.ZoomCanvas(d, d);
                m_mouse.ZoomCanvas.StartPosition = Cursor.Position;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsCanvasZooming()
        {
            return (m_mouse.ZoomCanvas != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        private void BeginFrameCanvas()
        {
            m_mouse.Selection.StartPos = m_canvas.PointToCanvas(PointToClient(Cursor.Position));
            m_mouse.Selection.EndPos = m_canvas.PointToCanvas(PointToClient(Cursor.Position));
            m_mouse.Selection.Valid = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndFrameCanvas()
        {
            if (m_mouse.Selection.Valid)
            {
                if (m_mouse.ModifierKeys == Keys.Alt)
                {
                    m_canvas.Frame(m_canvas.RectToView(m_mouse.Selection.GetRect()), this.ClientRectangle);
                    m_mouse.Selection.Clear();
                    Invalidate();
                    libpixy.net.Vecmath.Utils.RaiseEvent(this.CanvasScaleChanged, this);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateFrameCanvas()
        {
            if (IsCanvasFraming())
            {
                m_mouse.Selection.EndPos = m_canvas.PointToCanvas(this.PointToClient(Cursor.Position));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsCanvasFraming()
        {
            return m_mouse.Selection.Valid && m_mouse.ModifierKeys == Keys.Alt;
        }

        /// <summary>
        /// キャンバスをフレーム
        /// </summary>
        public void FrameCanvas()
        {
            int x1 = int.MaxValue;
            int y1 = int.MaxValue;
            int x2 = int.MinValue;
            int y2 = int.MinValue;

            IEnumerable<libpixy.net.Controls.Diagram.Node> nodes = null;
            if (m_document.SelectedNode != null)
            {
                nodes = m_document.SelectedNodes;
            }
            else
            {
                nodes = m_document.Nodes;
            }

            foreach (libpixy.net.Controls.Diagram.Node node in nodes)
            {
                Rectangle rc = node.Bounds;
                if (x1 > rc.Left) {
                    x1 = rc.Left;
                }
                if (y1 > rc.Top) {
                    y1 = rc.Top;
                }
                if (x2 < rc.Right) {
                    x2 = rc.Right;
                }
                if (y2 < rc.Bottom) {
                    y2 =rc.Bottom;
                }
            }

            Rectangle rcArea = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            Rectangle rcView = m_canvas.RectToView(rcArea);
            rcView.Inflate(20, 20);
            m_canvas.Frame(rcView, this.ClientRectangle);
            libpixy.net.Vecmath.Utils.RaiseEvent(this.CanvasScaleChanged, this);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        private void BeginDragNodeSelection(Point pt)
        {
            m_ctrl.ClearSelectedNodes();

            m_mouse.Selection.StartPos = m_canvas.PointToCanvas(pt);
            m_mouse.Selection.EndPos = m_canvas.PointToCanvas(pt);
            m_mouse.Selection.Valid = true;

            if (this.BeginSelection != null)
            {
                this.BeginSelection(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndDragNodeSelection()
        {
            if (m_mouse.Selection.Valid && m_mouse.ModifierKeys != Keys.Alt)
            {
                m_mouse.Selection.Clear();

                Invalidate();

                if (this.EndSelection != null)
                {
                    this.EndSelection(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DragNodeSelection()
        {
            if (m_mouse.Selection.Valid && m_mouse.ModifierKeys != Keys.Alt)
            {
                //m_ctrl.ClearSelectedNodes();

                List<Node> nodes = new List<Node>();

                m_mouse.Selection.EndPos = m_canvas.PointToCanvas(this.PointToClient(Cursor.Position));
                Rectangle area = m_mouse.Selection.GetRect();
                foreach (Node node in this.Document.Nodes)
                {
                    if (node.State.Hide)
                    {
                        continue;
                    }

                    if (area.IntersectsWith(node.Bounds))
                    {
                        nodes.Add(node);
                    }
                }

                bool update = false;
                foreach (Node node in nodes)
                {
                    if (m_document.SelectedNodes.Contains(node) == false)
                    {
                        update = true;
                        break;
                    }
                }

                if (update)
                {
                    m_ctrl.ClearSelectedNodes();
                    m_document.SelectNodes(nodes);
                    RaiseNodeSelected();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsNodeSelectionDragging()
        {
            return m_mouse.Selection.Valid && m_mouse.ModifierKeys != Keys.Alt;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BeginDragNavi(Point pt)
        {
            if (m_naviArea.Contains(pt))
            {
                m_mouse.DragNavi = new ToolMouseDragging();
                m_mouse.DragNavi.StartPosition = Cursor.Position;
                Cursor.Current = Cursors.Hand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndDragNavi()
        {
            if (m_mouse.DragNavi != null)
            {
                Cursor.Current = Cursors.Default;
                m_mouse.DragNavi = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DragNavi()
        {
            if (m_mouse.DragNavi != null)
            {
                Point ptS = PointToCanvas(m_mouse.DragNavi.StartPosition);
                Point ptE = PointToCanvas(Cursor.Position);
                m_mouse.DragNavi.StartPosition = Cursor.Position;
                m_naviArea.Offset(ptE.X - ptS.X, ptE.Y - ptS.Y);

                if (NaviDragging != null)
                {
                    NaviDragging(this, new EventArgs());
                }

                Invalidate();
            }
        }

        #endregion Mouse Methods

        #region Node

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        public void AddNode(Node node)
        {
            node.Location = PointSnapGrid(PointToCanvas(node.Location));
            node.ComputeBounds();
            Document.AddNode(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        private Node PickNode(Point loc)
        {
            foreach (Node node in this.Document.Nodes)
            {
                if (node.State.Hide)
                {
                    continue;
                }

                if (node.Bounds.Contains(loc))
                {
                    return node;
                }

                if (node.HitPort(loc) != null)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// ノードを削除する
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(Node node)
        {
            if (node == null)
            { 
                return; 
            }

            node.DisconnectAllPorts();
            m_document.RemoveNode(node);
        }

        /// <summary>
        /// ノードを削除する
        /// </summary>
        /// <param name="id"></param>
        public void RemoveNode(int id)
        {
            RemoveNode(m_document.FindNodeById(id));
        }

        /// <summary>
        /// ノードを削除する
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNodes(IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                RemoveNode(node);
            }
        }

        /// <summary>
        /// ノードを選択する
        /// </summary>
        /// <param name="node"></param>
        public void SelectNode(Node node)
        {
            m_ctrl.ClearSelectedNodes();
            m_document.SelectNode(node);
            RaiseNodeSelected();
        }

        /// <summary>
        /// ノードを選択する
        /// </summary>
        /// <param name="node"></param>
        public void SelectNodes(Node[] nodes)
        {
            m_ctrl.ClearSelectedNodes();
            m_document.SelectNodes(nodes);
            RaiseNodeSelected();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SyncNaviTarget()
        {
            if (CanvasScaleChanged != null)
            {
                CanvasScaleChanged(this, new EventArgs());
            }

            if (CanvasDragging != null)
            {
                CanvasDragging(this, new EventArgs());
            }
        }

        #endregion // Misc

        #region Coordinate transform

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Point PointToView(Point pt)
        {
            return m_canvas.PointToView(pt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Point PointToCanvas(Point pt)
        {
            return m_canvas.PointToCanvas(pt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        private Point PointSnapGrid(Point pt)
        {
            if (SnapGrid)
            {
                return new Point(
                    libpixy.net.Vecmath.Utils.SnapGrid(pt.X, GridSize),
                    libpixy.net.Vecmath.Utils.SnapGrid(pt.Y, GridSize));
            }

            return pt;
        }

        #endregion // Coordinate Transform

        #region EventHandler

        /// <summary>
        /// 
        /// </summary>
        public void m_document_Changed(object sender, ChangedEventArgs args)
        {
            switch (args.Type)
            {
                case ChangedEventType.UPDATE:
                case ChangedEventType.NEW_NODE:
                case ChangedEventType.REMOVE_NODE:
                case ChangedEventType.EXPAND_NODE:
                case ChangedEventType.COLLAPSE_NODE:
                case ChangedEventType.CLEAR:
                    Invalidate();
                    break;

                case ChangedEventType.DESELECT_ALL:
                case ChangedEventType.SELECT_NODE:
                    Invalidate();
                    RaiseNodeSelected();
                    break;

                case ChangedEventType.CONNECT_PORT:
                case ChangedEventType.DISCONNECT_PORT:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NaviTarget_CanvasScaleChanged(object sender, EventArgs e)
        {
            NaviArea = m_navigateTarget.DispArea;
            Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NaviTarget_CanvasDragging(object sender, EventArgs e)
        {
            NaviArea = m_navigateTarget.DispArea;
            Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NaviTarget_NaviDragging(object sender, EventArgs e)
        {
            m_canvas.Origin = new Point(
                -m_navigateTarget.NaviArea.X,
                -m_navigateTarget.NaviArea.Y);
            Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeTree_ClientSizeChanged(object sender, EventArgs e)
        {
            SyncNaviTarget();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (m_mouse.MouseClickHook)
            {
                m_mouse.MouseClickHook = false;
                return;
            }

            Point loc = PointToCanvas(e.Location);
            Node node = PickNode(loc);
            if (node != null)
            {
                node.ChildFolded = !node.ChildFolded;
                UpdateNodeState();
                Invalidate();
            }
        }

        #endregion

        #region Tool

        /// <summary>
        /// 
        /// </summary>
        public class ToolNodeDragging
        {
            public Point StartPosition;
            public Point EndPosition;
            public Hashtable NodeLocations;
            public bool ConnectMode;
            public Point ConnectPoint;
            public Link ConnectLink;

            /// <summary>
            /// 
            /// </summary>
            public ToolNodeDragging()
            {
                this.NodeLocations = new Hashtable();
                this.ConnectMode = false;
                this.ConnectLink = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ToolMouseDragging
        {
            public Point StartPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        public class ToolPortLinking
        {
            public Point Position;
            public Port Port;
            public bool Connected;
        }

        #endregion // Tool

        #region Serialize

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public virtual void Serialize(XmlWriter w)
        {
            w.WriteStartElement("Mirage_Controls_Diagram_Canvas");
            w.WriteAttributeString("version", "1.0");

            w.WriteElementString("GridSize", this.GridSize.ToString());
            w.WriteElementString("ShowGrid", this.ShowGrid.ToString());
            w.WriteElementString("SnapGrid", this.SnapGrid.ToString());
            w.WriteElementString("CanvasOrgX", m_canvas.Origin.X.ToString());
            w.WriteElementString("CanvasOrgY", m_canvas.Origin.Y.ToString());
            w.WriteElementString("CanvasScaleX", m_canvas.Scale.X.ToString());
            w.WriteElementString("CanvasScaleY", m_canvas.Scale.Y.ToString());

            w.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public virtual void Deserialize(XmlReader r)
        {
            if (r.IsStartElement("Mirage_Controls_Diagram_Canvas") == false)
            {
                return;
            }

            r.ReadStartElement("Mirage_Controls_Diagram_Canvas");
            this.GridSize = Int32.Parse(r.ReadElementString("GridSize"));
            this.ShowGrid = bool.Parse(r.ReadElementString("ShowGrid"));
            this.SnapGrid = bool.Parse(r.ReadElementString("SnapGrid"));
            this.m_canvas.Origin = new Point(
                Int32.Parse(r.ReadElementString("CanvasOrgX")),
                Int32.Parse(r.ReadElementString("CanvasOrgY")));
            this.m_canvas.Scale = new PointF(
                float.Parse(r.ReadElementString("CanvasScaleX")),
                float.Parse(r.ReadElementString("CanvasScaleY")));
            r.ReadEndElement();

            UpdateNodeState();
        }

        #endregion

        #region NativeMethods

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        #endregion NativeMethods
    }
}
