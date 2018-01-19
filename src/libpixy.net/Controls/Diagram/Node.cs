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
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Xml;

namespace libpixy.net.Controls.Diagram
{
    /// <summary>
    /// ノード
    /// </summary>
    public class Node : ICloneable
    {
        #region Constants

        public const int NAME_HEIGHT = 30;
        public const int EXTNAME_HEIGHT = 12;
        public const int PORT_HEIGHT = 15;
        public const int PORT_WIDTH = 20;
        public const int NODE_WIDTH = 150;

        #endregion Constants

        #region Field

        public Point Location;
        public string Text;
        public string SubText;
        public string Label;
        private bool m_folding;
        private bool m_destFolding;
        private bool m_childFolding;
        public Port ParentPort = null;
        public Port ChildPort = null;
        public PortCollection SourcePorts;
        public PortCollection DestinationPorts;
        public Rectangle Bounds;
        public int Id = 0;
        public int OldId = 0;
        public NodeState State = new NodeState();

        #endregion

        #region Enums

        /// <summary>
        /// ノード状態
        /// </summary>
        public class NodeState
        {
            public bool Select = false;//選択状態
            public bool Hit = false;//ヒット状態
            public bool Hide = false;//非表示状態
            public bool Invalid = false;//無効状態
            public bool Work = false;//作業用
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// ノードのタイプ
        /// </summary>
        /// <returns></returns>
        public virtual string Type()
        {
            return this.Name;
        }

        /// <summary>
        /// ノード名
        /// </summary>
        public string Name
        {
            get { return Text; }
            set { Text = value; }
        }

        /// <summary>
        /// 背景カラー
        /// </summary>
        public Color BackColor;

        /// <summary>
        /// 前景カラー
        /// </summary>
        public Color ForeColor;

        /// <summary>
        /// 分割ラインカラー
        /// </summary>
        public Color SeparateColor;

        /// <summary>
        /// サブテキストカラー
        /// </summary>
        public Color SubTextColor;

        /// <summary>
        /// 折り畳み
        /// </summary>
        public bool Folded
        {
            get { return m_folding; }
            set
            {
                m_folding = value;
                ComputeBounds();
            }
        }

        /// <summary>
        /// 出力ポートの折り畳み
        /// </summary>
        public bool DestinationFolded
        {
            get { return m_destFolding; }
            set
            {
                m_destFolding = value;
            }
        }

        /// <summary>
        /// 子ポートの折り畳み
        /// </summary>
        public bool ChildFolded
        {
            get { return m_childFolding; }
            set
            {
                m_childFolding = value;
            }
        }

        /// <summary>
        /// 親と接続されているか
        /// </summary>
        public bool ParentConnected
        {
            get
            {
                if (this.ParentPort == null)
                {
                    return false;
                }

                return this.ParentPort.Connected;
            }
        }

        /// <summary>
        /// 子と接続されているか
        /// </summary>
        public bool ChildConnected
        {
            get
            {
                if (this.ChildPort == null)
                {
                    return false;
                }

                return this.ChildPort.Connected;
            }
        }

        /// <summary>
        /// 出力ポートと接続されているか
        /// </summary>
        public bool DestinationConnected
        {
            get
            {
                if (this.DestinationPorts == null)
                {
                    return false;
                }

                foreach (Port port in this.DestinationPorts)
                {
                    if (port.Connected)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 折り畳みボタン領域
        /// </summary>
        [Browsable(false)]
        public Rectangle FoldingButtonBounds
        {
            get
            {
                return new Rectangle(
                    Location.X + 8,
                    Location.Y + NAME_HEIGHT / 2 - 5,
                    10, 10);
            }
        }

        /// <summary>
        /// 出力ボタン領域
        /// </summary>
        [Browsable(false)]
        public Rectangle DestFoldingButtonBounds
        {
            get
            {
                return new Rectangle(
                    Bounds.Right - 8 - 10,
                    Location.Y + NAME_HEIGHT / 2 - 5,
                    10, 10);
            }
        }

        /// <summary>
        /// ノードの名前領域の高さ
        /// </summary>
        [Browsable(false)]
        public int NodeNameHeight
        {
            get
            {
                if (string.IsNullOrEmpty(this.SubText))
                {
                    return NAME_HEIGHT;
                }
                else
                {
                    return NAME_HEIGHT + EXTNAME_HEIGHT;
                }
            }
        }

        /// <summary>
        /// タグ
        /// </summary>
        public object Tag;

        #endregion

        #region Ctor, Dtor

        /// <summary>
        /// 
        /// </summary>
        public Node()
        {
            SourcePorts = new PortCollection();
            DestinationPorts = new PortCollection();
            Bounds = new Rectangle();
            //BackColor = Color.FromArgb(138,155,223);
            BackColor = Color.FromArgb(212, 154, 96);
            ForeColor = Color.Black;
            SeparateColor = Color.Black;
            SubTextColor = Color.FromArgb(255, 255, 0);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ノードの折り畳みを設定する
        /// </summary>
        /// <param name="node"></param>
        /// <param name="collapse"></param>
        public static void ExpandNode(Node node, bool collapse)
        {
            if (node.DestinationPorts != null)
            {
                node.DestinationFolded = collapse;

                foreach (Port port in node.DestinationPorts)
                {
                    foreach (Link link in port.Connections)
                    {
                        ExpandNode(link.Port2.Owner, collapse);
                    }
                }
            }

            if (node.ChildPort != null)
            {
                node.ChildFolded = collapse;

                foreach (Link link in node.ChildPort.Connections)
                {
                    ExpandNode(link.Port2.Owner, collapse);
                }
            }

            node.Folded = collapse;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// ノードの折り畳みを設定する
        /// </summary>
        /// <param name="collapse"></param>
        public void Expand(bool collapse)
        {
            ExpandNode(this, collapse);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ComputeBounds()
        {
            if (Folded)
            {
                Bounds = new Rectangle(
                    Location.X,
                    Location.Y,
                    NODE_WIDTH,
                    NodeNameHeight);

                if (ParentPort != null)
                {
                    ParentPort.Bounds = new Rectangle(Location.X + Bounds.Width / 2, Bounds.Top, PORT_WIDTH, PORT_HEIGHT);
                    ParentPort.ConnectionPoint = new Point(Location.X + Bounds.Width / 2, Bounds.Top);
                }

                if (ChildPort != null)
                {
                    ChildPort.Bounds = new Rectangle(Location.X + Bounds.Width / 2, Bounds.Bottom, PORT_WIDTH, PORT_HEIGHT);
                    ChildPort.ConnectionPoint = new Point(Location.X + Bounds.Width / 2, Bounds.Bottom);
                }

                for (int i = 0; i < SourcePorts.Count; ++i)
                {
                    SourcePorts[i].Bounds = new Rectangle(0, 0, 0, 0);
                    SourcePorts[i].ConnectionPoint = new Point(Bounds.Left, Location.Y + Bounds.Height / 2);
                }

                for (int i = 0; i < DestinationPorts.Count; ++i)
                {
                    DestinationPorts[i].Bounds = new Rectangle(0, 0, 0, 0);
                    DestinationPorts[i].ConnectionPoint = new Point(Bounds.Right, Location.Y + Bounds.Height / 2);
                }

                return;
            }

            Bounds = new Rectangle(
                Location.X,
                Location.Y,
                NODE_WIDTH,
                NodeNameHeight + PORT_HEIGHT * (SourcePorts.Count + DestinationPorts.Count));

            Rectangle rc = Bounds;
            rc.Y = rc.Y + NodeNameHeight;
            rc.Width = NODE_WIDTH / 3;
            rc.Height = PORT_HEIGHT;

            if (ParentPort != null)
            {
                ParentPort.Bounds = new Rectangle(Location.X + Bounds.Width / 2 - PORT_WIDTH / 2, Bounds.Top - PORT_HEIGHT / 2, PORT_WIDTH, PORT_HEIGHT);
                ParentPort.ConnectionPoint = new Point(Location.X + Bounds.Width / 2, Bounds.Top);
            }

            if (ChildPort != null)
            {
                ChildPort.Bounds = new Rectangle(Location.X + Bounds.Width / 2 - PORT_WIDTH / 2, Bounds.Bottom - PORT_HEIGHT / 2, PORT_WIDTH, PORT_HEIGHT);
                ChildPort.ConnectionPoint = new Point(Location.X + Bounds.Width / 2, Bounds.Bottom);
            }

            for (int i = 0; i < SourcePorts.Count; ++i)
            {
                SourcePorts[i].Bounds = rc;
                SourcePorts[i].ConnectionPoint = new Point(rc.X, rc.Y + rc.Height / 2);
                rc.Y += PORT_HEIGHT;
            }

            rc.X = Bounds.Right - rc.Width;
            for (int i = 0; i < DestinationPorts.Count; ++i)
            {
                DestinationPorts[i].Bounds = rc;
                DestinationPorts[i].ConnectionPoint = new Point(rc.Right, rc.Y + rc.Height / 2);
                rc.Y += PORT_HEIGHT;
            }
        }

        /// <summary>
        /// ノードの領域内かどうか
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool HitTest(Point loc)
        {
            return Bounds.Contains(loc);
        }

        #endregion Public Methods

        #region Port

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="multi"></param>
        public void AddSourcePort(string text, string type, bool multi)
        {
            Port port = new Port();
            port.Text = text;
            port.Type = type;
            port.Owner = this;
            port.EnableMultiConnection = multi;
            port.Stream = PortStream.In;
            port.Alignment = PortAlignment.Left;
            SourcePorts.Add(port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="multi"></param>
        public void AddDestinationPort(string text, string type, bool multi)
        {
            Port port = new Port();
            port.Text = text;
            port.Type = type;
            port.Owner = this;
            port.EnableMultiConnection = multi;
            port.Stream = PortStream.Out;
            port.Alignment = PortAlignment.Right;
            DestinationPorts.Add(port);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect(Link link)
        {
            if (this.ParentConnected)
            {
                if (ParentPort.Connections.Contains(link))
                {
                    ParentPort.Connections.Remove(link);
                    return;
                }
            }

            if (this.DestinationConnected)
            {
                foreach (Port dport in DestinationPorts)
                {
                    if (dport.Connections.Contains(link))
                    {
                        dport.Connections.Remove(link);
                        return;
                    }
                }
            }

            if (this.ChildConnected)
            {
                if (ChildPort.Connections.Contains(link))
                {
                    ChildPort.Connections.Remove(link);
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect(Port port)
        {
            if (this.ParentConnected)
            {
                foreach (Link link in ParentPort.Connections)
                {
                    if (link.Port2 == port)
                    {
                        ParentPort.Connections.Remove(link);
                        return;
                    }
                }
            }

            if (this.DestinationConnected)
            {
                foreach (Port dport in DestinationPorts)
                {
                    foreach (Link link in dport.Connections)
                    {
                        if (link.Port2 == port)
                        {
                            dport.Connections.Remove(link);
                            return;
                        }
                    }
                }
            }

            if (this.ChildConnected)
            {
                foreach (Link link in ChildPort.Connections)
                {
                    if (link.Port2 == port)
                    {
                        ChildPort.Connections.Remove(link);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect(Node node)
        {
            if (this.ParentConnected)
            {
                foreach (Link link in ParentPort.Connections)
                {
                    if (link.Port2.Owner == node)
                    {
                        ParentPort.Connections.Remove(link);
                        return;
                    }
                }
            }

            if (this.DestinationConnected)
            {
                foreach (Port dport in DestinationPorts)
                {
                    foreach (Link link in dport.Connections)
                    {
                        if (link.Port2.Owner == node)
                        {
                            dport.Connections.Remove(link);
                            return;
                        }
                    }
                }
            }

            if (this.ChildConnected)
            {
                foreach (Link link in ChildPort.Connections)
                {
                    if (link.Port2.Owner == node)
                    {
                        ChildPort.Connections.Remove(link);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisconnectAllPorts()
        {
            DisconnectSrcDst();
            DisconnectParent();
            DisconnectChildren();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisconnectSrcDst()
        {
            foreach (Port port in SourcePorts)
            {
                port.ClearConnection();
            }

            foreach (Port port in DestinationPorts)
            {
                port.ClearConnection();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisconnectParent()
        {
            if (ParentPort != null)
            {
                ParentPort.ClearConnection();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisconnectChildren()
        {
            if (ChildPort != null)
            {
                ChildPort.ClearConnection();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenParentPort()
        {
            ParentPort = new Port();
            ParentPort.Owner = this;
            ParentPort.Type = "node";
            ParentPort.Text = "parent";
            ParentPort.Stream = PortStream.In;
            ParentPort.Alignment = PortAlignment.Top;
            ParentPort.EnableMultiConnection = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenChildrenPort()
        {
            ChildPort = new Port();
            ChildPort.Owner = this;
            ChildPort.Type = "node";
            ChildPort.Text = "children";
            ChildPort.Stream = PortStream.Out;
            ChildPort.Alignment = PortAlignment.Bottom;
            ChildPort.EnableMultiConnection = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenHierarchicalPorts()
        {
            OpenParentPort();
            OpenChildrenPort();
            ComputeBounds();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Port HitPort(Point loc)
        {
            if (this.ParentPort != null && this.ParentPort.Bounds.Contains(loc))
            {
                return this.ParentPort;
            }

            if (this.ChildPort != null && this.ChildPort.Bounds.Contains(loc))
            {
                return this.ChildPort;
            }

            Port ret = HitSourcePort(loc);
            if (ret != null)
            {
                return ret;
            }

            return HitDestinationPort(loc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Port HitSourcePort(Point loc)
        {
            foreach (Port port in SourcePorts)
            {
                if (port.Bounds.Contains(loc))
                {
                    return port;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Port HitDestinationPort(Point loc)
        {
            foreach (Port port in DestinationPorts)
            {
                if (port.Bounds.Contains(loc))
                {
                    return port;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Port GetPort(string label)
        {
            if (this.ParentPort != null && this.ParentPort.Text == label)
            {
                return this.ParentPort;
            }

            if (this.ChildPort != null && this.ChildPort.Text == label)
            {
                return this.ChildPort;
            }

            Port ret = GetSourcePort(label);
            if (ret != null)
            {
                return ret;
            }

            return GetDestinationPort(label);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Port GetSourcePort(string label)
        {
            foreach (Port port in SourcePorts)
            {
                if (port.Text == label)
                {
                    return port;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Port GetDestinationPort(string label)
        {
            foreach (Port port in DestinationPorts)
            {
                if (port.Text == label)
                {
                    return port;
                }
            }

            return null;
        }

        /// <summary>
        /// 指定のコネクションを持っているかどうか
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool ExistsConnection(Link link)
        {
            if (ExistsParentConnection(link))
            {
                return true;
            }

            if (ExistsDestinationConnection(link))
            {
                return true;
            }

            if (ExistsChildConnection(link))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 親ポートに指定のコネクションがあるかどうか
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool ExistsParentConnection(Link link)
        {
            if (this.ParentConnected)
            {
                return this.ParentPort.Connections.Contains(link);
            }

            return false;
        }

        /// <summary>
        /// 出力ポートに指定のコネクションがあるかどうか
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool ExistsDestinationConnection(Link link)
        {
            if (this.DestinationConnected)
            {
                foreach (Port port in this.DestinationPorts)
                {
                    if (port.Connections.Contains(link))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 子ポートに指定のコネクションがあるかどうか
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool ExistsChildConnection(Link link)
        {
            if (this.ChildConnected)
            {
                return this.ChildPort.Connections.Contains(link);
            }

            return false;
        }

        #endregion

        #region Serialize

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public virtual void Serialize(XmlWriter w)
        {
            w.WriteElementString("Id", this.Id.ToString());
            w.WriteElementString("LocationX", this.Location.X.ToString());
            w.WriteElementString("LocationY", this.Location.Y.ToString());
            w.WriteElementString("Text", this.Text);
            w.WriteElementString("SubText", this.SubText);
            w.WriteElementString("Label", this.Label);
            w.WriteElementString("Expand", this.Folded.ToString());
            w.WriteElementString("ChildExpand", this.ChildFolded.ToString());
            w.WriteElementString("DestinationExpand", this.DestinationFolded.ToString());
            w.WriteElementString("BackColor", string.Format("{0},{1},{2},{3}", this.BackColor.R, this.BackColor.G,this.BackColor.B, this.BackColor.A));
            w.WriteElementString("ForeColor", string.Format("{0},{1},{2},{3}", this.ForeColor.R, this.ForeColor.G,this.ForeColor.B, this.ForeColor.A));
            w.WriteElementString("SeparateColor", string.Format("{0},{1},{2},{3}", this.SeparateColor.R, this.SeparateColor.G,this.SeparateColor.B, this.SeparateColor.A));
            w.WriteElementString("SubTextColor", string.Format("{0},{1},{2},{3}", this.SubTextColor.R, this.SubTextColor.G, this.SubTextColor.B, this.SubTextColor.A));

            if (this.ParentPort == null)
            {
                w.WriteElementString("ParentPort", "");
            }
            else
            {
                w.WriteStartElement("ParentPort");
                this.ParentPort.Serialize(w);
                w.WriteEndElement();
            }

            if (this.ChildPort == null)
            {
                w.WriteElementString("ChildPort", "");
            }
            else
            {
                w.WriteStartElement("ChildPort");
                this.ChildPort.Serialize(w);
                w.WriteEndElement();
            }

            w.WriteElementString("SourcePortCount", string.Format("{0}", this.SourcePorts.Count));
            for (int i = 0; i < this.SourcePorts.Count; ++i)
            {
                w.WriteStartElement(string.Format("SourcePort-{0}", i));
                this.SourcePorts[i].Serialize(w);
                w.WriteEndElement();
            }

            w.WriteElementString("DestinationPortCount", string.Format("{0}", this.DestinationPorts.Count));
            for (int i = 0; i < this.DestinationPorts.Count; ++i)
            {
                w.WriteStartElement(string.Format("DestinationPort-{0}", i));
                this.DestinationPorts[i].Serialize(w);
                w.WriteEndElement();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public virtual void Deserialize(XmlReader r)
        {
            if (r.IsStartElement("Id"))
            {
                this.OldId = Int32.Parse(r.ReadElementString("Id"));
            }
            if (r.IsStartElement("LocationX"))
            {
                this.Location.X = Int32.Parse(r.ReadElementString("LocationX"));
            }
            if (r.IsStartElement("LocationY"))
            {
                this.Location.Y = Int32.Parse(r.ReadElementString("LocationY"));
            }
            if (r.IsStartElement("Text"))
            {
                this.Text = r.ReadElementString("Text");
            }
            if (r.IsStartElement("SubText"))
            {
                this.SubText = r.ReadElementString("SubText");
            }
            if (r.IsStartElement("Label"))
            {
                this.Label = r.ReadElementString("Label");
            }
            if (r.IsStartElement("Expand"))
            {
                this.Folded = Boolean.Parse(r.ReadElementString("Expand"));
            }
            if (r.IsStartElement("ChildExpand"))
            {
                this.ChildFolded = Boolean.Parse(r.ReadElementString("ChildExpand"));
            }
            if (r.IsStartElement("DestinationExpand"))
            {
                this.DestinationFolded = Boolean.Parse(r.ReadElementString("DestinationExpand"));
            }
            if (r.IsStartElement("BackColor"))
            {
                string[] backColor = r.ReadElementString("BackColor").Split(',');
                this.BackColor = System.Drawing.Color.FromArgb(
                    Int32.Parse(backColor[3]),
                    Int32.Parse(backColor[0]),
                    Int32.Parse(backColor[1]),
                    Int32.Parse(backColor[2]));
            }
            if (r.IsStartElement("ForeColor"))
            {
                string[] foreColor = r.ReadElementString("ForeColor").Split(',');
                this.ForeColor = System.Drawing.Color.FromArgb(
                    Int32.Parse(foreColor[3]),
                    Int32.Parse(foreColor[0]),
                    Int32.Parse(foreColor[1]),
                    Int32.Parse(foreColor[2]));
            }
            if (r.IsStartElement("SplitColor"))
            {
                string[] splitColor = r.ReadElementString("SplitColor").Split(',');
                this.SeparateColor = System.Drawing.Color.FromArgb(
                    Int32.Parse(splitColor[3]),
                    Int32.Parse(splitColor[0]),
                    Int32.Parse(splitColor[1]),
                    Int32.Parse(splitColor[2]));
            }
            else if (r.IsStartElement("SeparateColor"))
            {
                string[] splitColor = r.ReadElementString("SeparateColor").Split(',');
                this.SeparateColor = System.Drawing.Color.FromArgb(
                    Int32.Parse(splitColor[3]),
                    Int32.Parse(splitColor[0]),
                    Int32.Parse(splitColor[1]),
                    Int32.Parse(splitColor[2]));
            }
            if (r.IsStartElement("SubTextColor"))
            {
                string[] color = r.ReadElementString("SubTextColor").Split(',');
                this.SubTextColor = System.Drawing.Color.FromArgb(
                    Int32.Parse(color[3]),
                    Int32.Parse(color[0]),
                    Int32.Parse(color[1]),
                    Int32.Parse(color[2]));
            }


            if (r.IsStartElement("ParentPort"))
            {
                if (r.IsEmptyElement)
                {
                    r.Read();
                }
                else
                {
                    r.ReadStartElement("ParentPort");
                    this.ParentPort = new Port();
                    this.ParentPort.Deserialize(r);
                    this.ParentPort.Owner = this;
                    r.ReadEndElement();
                }
            }

            if (r.IsStartElement("ChildPort"))
            {
                if (r.IsEmptyElement)
                {
                    r.Read();
                }
                else
                {
                    r.ReadStartElement("ChildPort");
                    this.ChildPort = new Port();
                    this.ChildPort.Deserialize(r);
                    this.ChildPort.Owner = this;
                    r.ReadEndElement();
                }
            }

            if (r.IsStartElement("SourcePortCount"))
            {
                int portCount = Int32.Parse(r.ReadElementString("SourcePortCount"));
                for (int i = 0; i < portCount; ++i)
                {
                    r.ReadStartElement(string.Format("SourcePort-{0}", i));
                    Port newPort = new Port();
                    newPort.Deserialize(r);
                    newPort.Owner = this;
                    this.SourcePorts.Add(newPort);
                    r.ReadEndElement();
                }
            }
            if (r.IsStartElement("DestinationPortCount"))
            {
                int portCount = Int32.Parse(r.ReadElementString("DestinationPortCount"));
                for (int i = 0; i < portCount; ++i)
                {
                    r.ReadStartElement(string.Format("DestinationPort-{0}", i));
                    Port newPort = new Port();
                    newPort.Deserialize(r);
                    newPort.Owner = this;
                    this.DestinationPorts.Add(newPort);
                    r.ReadEndElement();
                }
            }
        }

        #endregion

        #region Implements IClonable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Node node = new Node();
            node.Location = this.Location;
            node.Text = this.Text;
            node.SubText = this.SubText;
            node.Label = this.Label;
            node.Folded = this.Folded;
            node.DestinationFolded = this.DestinationFolded;
            node.ChildFolded = this.ChildFolded;
            node.Id = this.Id;
            node.OldId = this.OldId;

            if (this.ParentPort != null)
            {
                node.OpenParentPort();
            }
            else
            {
                node.ParentPort = null;
            }

            if (this.ChildPort != null)
            {
                node.OpenChildrenPort();
            }
            else
            {
                node.ChildPort = null;
            }

            foreach (Port port in this.SourcePorts)
            {
                Port clone = (Port)port.Clone();
                clone.Owner = node;
                node.SourcePorts.Add(clone);
            }

            foreach (Port port in this.DestinationPorts)
            {
                Port clone = (Port)port.Clone();
                clone.Owner = node;
                node.DestinationPorts.Add(clone);
            }

            //node.Bounds = this.Bounds;
            node.ComputeBounds();
            node.State = new NodeState();
            node.Name = this.Name;
            node.ForeColor = this.ForeColor;
            node.BackColor = this.BackColor;
            node.SeparateColor = this.SeparateColor;
            node.SubTextColor = this.SubTextColor;
            return node;
        }

        #endregion IClonable

        #region Helper

        /// <summary>
        /// ノードのクローンを生成する
        /// </summary>
        public static List<Node> Clones(IEnumerable<Node> nodes)
        {
            int count = 0;
            foreach (Node node in nodes)
            {
                count++;
                break;
            }

            if (count == 0)
            {
                return null;
            }

            List<Node> source = new List<Node>();
            Node.GetExpandedNodes(nodes, ref source);

            List<Node> clones = new List<Node>();
            List<int> idlist = new List<int>();
            Point location = new Point(int.MaxValue, int.MaxValue);

            // クローン作成
            foreach (Node node in source)
            {
                // ルートはコピーから除外
                if (node.Name == "Root")
                {
                    continue;
                }

                clones.Add((Node)node.Clone());
                idlist.Add(node.Id);

                if (location.X > node.Location.X)
                {
                    location.X = node.Location.X;
                }

                if (location.Y > node.Location.Y)
                {
                    location.Y = node.Location.Y;
                }
            }

            // ノードの位置を再計算
            foreach (Node node in clones)
            {
                node.Location.X -= location.X;
                node.Location.Y -= location.Y;
            }

            // コネクションをコピー
            int i = 0;
            foreach (Node node in source)
            {
                if (node.ChildPort != null)
                {
                    foreach (Link link in node.ChildPort.Connections)
                    {
                        if (idlist.Contains(link.Port2.Owner.Id))
                        {
                            Node childNode = (Node)link.Port2.Owner;
                            Link newLink = new Link();
                            newLink.Port1 = clones[i].ChildPort;
                            newLink.Port2 = clones[idlist.IndexOf(childNode.Id)].ParentPort;
                            newLink.Port1.Connections.Add(newLink);
                            newLink.Port2.Connections.Add(newLink);
                        }
                    }
                }

                int dstPortCount = node.DestinationPorts.Count;
                for (int dstPortIndex = 0; dstPortIndex < dstPortCount; ++dstPortIndex)
                {
                    Port dstPort = node.DestinationPorts[dstPortIndex];
                    foreach (Link link in dstPort.Connections)
                    {
                        if (idlist.Contains(link.Port2.Owner.Id))
                        {
                            int srcPortIndex = link.Port2.Owner.SourcePorts.IndexOf(link.Port2);
                            if (srcPortIndex != -1)
                            {
                                Link newLink = new Link();
                                newLink.Port1 = clones[i].DestinationPorts[dstPortIndex];
                                newLink.Port2 = clones[idlist.IndexOf(link.Port2.Owner.Id)].SourcePorts[srcPortIndex];
                                newLink.Port1.Connections.Add(newLink);
                                newLink.Port2.Connections.Add(newLink);
                            }
                            else if (link.Port2.Equals(link.Port2.Owner.ParentPort))
                            {
                                Link newLink = new Link();
                                newLink.Port1 = clones[i].DestinationPorts[dstPortIndex];
                                newLink.Port2 = clones[idlist.IndexOf(link.Port2.Owner.Id)].ParentPort;
                                newLink.Port1.Connections.Add(newLink);
                                newLink.Port2.Connections.Add(newLink);
                            }
                        }
                    }
                }

                ++i;
            }

            return clones;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void GetExpandedNodes(Node node, ref List<Node> output)
        {
            if (node.DestinationConnected)
            {
                foreach (Port port in node.DestinationPorts)
                {
                    if (port.Connected)
                    {
                        foreach (Link link in port.Connections)
                        {
                            if (output.Contains(link.Port2.Owner) == false)
                            {
                                output.Add(link.Port2.Owner);
                                GetExpandedNodes(link.Port2.Owner, ref output);
                            }
                        }
                    }
                }
            }

            if (node.ChildConnected)
            {
                foreach (Link link in node.ChildPort.Connections)
                {
                    if (output.Contains(link.Port2.Owner) == false)
                    {
                        output.Add(link.Port2.Owner);
                        GetExpandedNodes(link.Port2.Owner, ref output);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void GetExpandedNodes(IEnumerable<Node> nodes, ref List<Node> output)
        {
            foreach (Node node in nodes)
            {
                if (output.Contains(node))
                {
                    continue;
                }

                output.Add(node);
                GetExpandedNodes(node, ref output);
            }
        }


        #endregion Helper
    }
}
