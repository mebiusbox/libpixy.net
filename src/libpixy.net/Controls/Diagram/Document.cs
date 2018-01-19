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
using System.Xml;
using System.Drawing;

namespace libpixy.net.Controls.Diagram
{
    /// <summary>
    /// ドキュメント
    /// </summary>
    public class Document
    {
        #region Fields

        private List<Node> m_nodes;
        private List<Node> m_selectedNodes;
        private libpixy.net.Tools.PUID m_puid = new libpixy.net.Tools.PUID();
        public bool ModifiedFlag = false;

        #endregion // Fileds

        #region Public Methods

        public event ChangedEvent Changed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void RaiseChangedEvent(ChangedEventType type, Object data)
        {
            switch (type)
            {
                case ChangedEventType.CLEAR:
                    this.ModifiedFlag = false;
                    break;
                case ChangedEventType.COLLAPSE_NODE:
                case ChangedEventType.CONNECT_PORT:
                case ChangedEventType.DISCONNECT_PORT:
                case ChangedEventType.EXPAND_NODE:
                case ChangedEventType.NEW_NODE:
                case ChangedEventType.REMOVE_NODE:
                    this.ModifiedFlag = true;
                    break;
            }

            if (Changed != null)
            {
                Changed(this, new ChangedEventArgs(type, data));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        public void Update()
        {
            RaiseChangedEvent(ChangedEventType.UPDATE, null);
        }

        #endregion Public Methods

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Document()
        {
            m_nodes = new List<Node>();
            m_selectedNodes = new List<Node>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// ドキュメントをクリア
        /// </summary>
        public void Clear()
        {
            m_nodes.Clear();
            m_selectedNodes.Clear();
            m_puid.Reset();
            RaiseChangedEvent(ChangedEventType.CLEAR, null);
        }

        #endregion Public Methods

        #region Properties

        /// <summary>
        /// ノードコレクション
        /// </summary>
        public List<Node> Nodes
        {
            get { return m_nodes; }
        }

        /// <summary>
        /// 選択されているノードコレクション
        /// </summary>
        public List<Node> SelectedNodes
        {
            get { return m_selectedNodes; }
        }

        /// <summary>
        /// ノードの数
        /// </summary>
        public int NodeCount
        {
            get { return m_nodes.Count; }
        }

        /// <summary>
        /// ノードが選択されているかどうか
        /// </summary>
        public bool IsNodeSelected
        {
            get { return (m_selectedNodes.Count > 0); }
        }

        /// <summary>
        /// 選択しているノードの数
        /// </summary>
        public int SelectedCount
        {
            get { return m_selectedNodes.Count; }
        }

        /// <summary>
        /// 選択されている最初のノードを取得
        /// </summary>
        public Node SelectedNode
        {
            get
            {
                if (m_selectedNodes.Count > 0)
                {
                    return m_selectedNodes[0];
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Node operation

        /// <summary>
        /// ノードを追加
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(Node node)
        {
            node.Id = m_puid.GetNext();
            m_nodes.Add(node);

            RaiseChangedEvent(ChangedEventType.NEW_NODE, new Node[1] { node });
        }

        /// <summary>
        /// ノードを追加
        /// </summary>
        /// <param name="node"></param>
        public void AddNodes(IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                node.Id = m_puid.GetNext();
            }

            m_nodes.AddRange(nodes);

            RaiseChangedEvent(ChangedEventType.NEW_NODE, nodes);
        }

        /// <summary>
        /// ノードを削除
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(Node node)
        {
            m_nodes.Remove(node);
            node.Id = 0;

            RaiseChangedEvent(ChangedEventType.REMOVE_NODE, new Node[1] { node });
        }

        /// <summary>
        /// ノードを削除
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNodes(IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                m_nodes.Remove(node);
                node.Id = 0;
            }

            RaiseChangedEvent(ChangedEventType.REMOVE_NODE, nodes);
        }

        /// <summary>
        /// ノードを削除
        /// </summary>
        /// <param name="pred"></param>
        public void RemoveAllNode(Predicate<Node> pred)
        {
            m_nodes.RemoveAll(pred);
        }

        /// <summary>
        /// ノードを選択
        /// </summary>
        /// <param name="node"></param>
        public void SelectNode(Node node)
        {
            node.State.Select = true;
            m_selectedNodes.Add(node);

            RaiseChangedEvent(ChangedEventType.SELECT_NODE, new Node[1] { node });
        }

        /// <summary>
        /// ノードを選択
        /// </summary>
        public void SelectNodes(IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                node.State.Select = true;
            }

            m_selectedNodes.AddRange(nodes);

            RaiseChangedEvent(ChangedEventType.SELECT_NODE, nodes);
        }

        /// <summary>
        /// ノードクリア
        /// </summary>
        public void ClearNodes()
        {
            m_nodes.Clear();
            RaiseChangedEvent(ChangedEventType.CLEAR, m_nodes);
        }

        /// <summary>
        /// 選択ノード情報をクリア
        /// </summary>
        public void ClearSelectedNodes()
        {
            m_selectedNodes.Clear();
            RaiseChangedEvent(ChangedEventType.SELECT_NODE, null);
        }

        /// <summary>
        /// ノードを検索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Node FindNodeById(int id)
        {
            foreach (Node node in this.Nodes)
            {
                if (node.Id == id)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// ノードを検索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Node FindNodeByOldId(int id)
        {
            foreach (Node node in this.Nodes)
            {
                if (node.OldId == id)
                {
                    return node;
                }
            }

            return null;
        }


        #endregion

        #region Link

        /// <summary>
        /// ノード間の接続を解除する
        /// </summary>
        /// <param name="link"></param>
        public void Unlink(Link link)
        {
            link.Port1.ClearConnection();
            link.Port2.ClearConnection();

            RaiseChangedEvent(ChangedEventType.DISCONNECT_PORT, link);
        }

        /// <summary>
        /// ノード間を接続する
        /// </summary>
        /// <param name="srcPort"></param>
        /// <param name="dstPort"></param>
        public void Link(Port srcPort, Port dstPort)
        {
            Link link = new Link();
            link.Port1 = srcPort;
            link.Port2 = dstPort;

            if (srcPort.EnableMultiConnection)
            {
                srcPort.AddConnection(link);
            }
            else
            {
                srcPort.SetConnection(link);
            }

            if (dstPort.EnableMultiConnection)
            {
                dstPort.AddConnection(link);
            }
            else
            {
                dstPort.SetConnection(link);
            }

            RaiseChangedEvent(ChangedEventType.CONNECT_PORT, link);
        }

        #endregion Link

        #region Serialize

        public delegate libpixy.net.Controls.Diagram.Node DeserializeNodeContent(string nodeName);
        //private libpixy.net.Controls.Diagram.Node ReadNode = null;
        //private ConnectionInfo ReadConnection = null;

        /// <summary>
        /// 
        /// </summary>
        private class ConnectionInfo
        {
            public int NodeId1 = 0;
            public int NodeId2 = 0;
            public int NodePortIndex1 = 0;
            public int NodePortIndex2 = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public virtual void Serialize(XmlWriter w)
        {
            w.WriteStartElement("libpixy.net_Controls_Diagram_Document");
            w.WriteAttributeString("version", "1.0");

            SerializeNodes(w);
            SerializeConnections(w);

            w.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public void SerializeNodes(XmlWriter w)
        {
            w.WriteStartElement("Nodes");
            w.WriteAttributeString("count", string.Format("{0}", this.NodeCount));

            foreach (Node node in this.Nodes)
            {
                w.WriteStartElement("Node");
                w.WriteElementString("TypeName", node.GetType().ToString());
                node.Serialize(w);
                w.WriteEndElement();
            }

            w.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public void SerializeConnections(XmlWriter w)
        {
            SerializeHierarchyConnections(w);
            SerializeStreamConnections(w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public void SerializeHierarchyConnections(XmlWriter w)
        {
            int count = 0;

            foreach (Node node in this.Nodes)
            {
                if (node.ParentPort != null)
                {
                    foreach (Link link in node.ParentPort.Connections)
                    {
                        count++;
                    }
                }
            }

            w.WriteStartElement("HierarchyConnections");
            w.WriteAttributeString("count", string.Format("{0}", count));

            foreach (Node node in this.Nodes)
            {
                if (node.ParentPort != null)
                {
                    foreach (Link link in node.ParentPort.Connections)
                    {
                        w.WriteStartElement("Connection");

                        w.WriteElementString("Node1", link.Port1.Owner.Id.ToString());
                        w.WriteElementString("Node2", link.Port2.Owner.Id.ToString());

                        w.WriteEndElement();
                    }
                }
            }

            w.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public void SerializeStreamConnections(XmlWriter w)
        {
            int count = 0;

            foreach (Node node in this.Nodes)
            {
                foreach (Port port in node.DestinationPorts)
                {
                    foreach (Link link in port.Connections)
                    {
                        count++;
                    }
                }
            }

            w.WriteStartElement("Connections");
            w.WriteAttributeString("count", string.Format("{0}", count));

            foreach (Node node in this.Nodes)
            {
                foreach (Port port in node.DestinationPorts)
                {
                    foreach (Link link in port.Connections)
                    {
                        w.WriteStartElement("Connection");

                        w.WriteElementString("Node1", link.Port1.Owner.Id.ToString());
                        w.WriteElementString("Node2", link.Port2.Owner.Id.ToString());
                        w.WriteElementString("NodePort1", link.Port1.Text);
                        w.WriteElementString("NodePort2", link.Port2.Text);

                        w.WriteEndElement();
                    }
                }
            }

            w.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public virtual void Deserialize(XmlReader r, DeserializeNodeContent handler)
        {
            if (r.IsStartElement("libpixy.net_Controls_Diagram_Document") == false)
            {
                return;
            }

            r.ReadStartElement("libpixy.net_Controls_Diagram_Document");

            if (r.IsStartElement("Nodes") && r.IsEmptyElement == false)
            {
                int count = Int32.Parse(r.GetAttribute("count"));
                r.ReadStartElement("Nodes");
                DeserializeNodes(r, count, handler);
                r.ReadEndElement();
            }

            if (r.IsStartElement("HierarchyConnections"))
            {
                int count = Int32.Parse(r.GetAttribute("count"));
                if (r.IsEmptyElement)
                {
                    r.Skip();
                }
                else
                {
                    r.ReadStartElement("HierarchyConnections");
                    DeserializeHierarchyConnections(r, count);
                    r.ReadEndElement();
                }
            }

            if (r.IsStartElement("Connections"))
            {
                int count = Int32.Parse(r.GetAttribute("count"));
                if (r.IsEmptyElement)
                {
                    r.Skip();
                }
                else
                {
                    r.ReadStartElement("Connections");
                    DeserializeStreamConnections(r, count);
                    r.ReadEndElement();
                }
            }

            r.ReadEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void DeserializeNodes(XmlReader r, int count, DeserializeNodeContent handler)
        {
            for (int i = 0; i < count; ++i)
            {
                if (r.IsStartElement("Node"))
                {
                    r.ReadStartElement("Node");
                    string persistString = r.ReadElementString("TypeName");

                    Node newNode = null;

                    if (handler != null)
                    {
                        newNode = handler(persistString);
                    }

                    if (newNode == null)
                    {
                        newNode = new Node();
                    }

                    newNode.Deserialize(r);
                    newNode.ComputeBounds();
                    AddNode(newNode);
                    r.ReadEndElement();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void DeserializeHierarchyConnections(XmlReader r, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                if (r.IsStartElement("Connection"))
                {
                    r.ReadStartElement("Connection");
                    int nodeId1 = Int32.Parse(r.ReadElementString("Node1"));
                    int nodeId2 = Int32.Parse(r.ReadElementString("Node2"));

                    libpixy.net.Controls.Diagram.Node node1 = null;
                    libpixy.net.Controls.Diagram.Node node2 = null;
                    foreach (libpixy.net.Controls.Diagram.Node node in this.Nodes)
                    {
                        if (node.OldId == nodeId1)
                        {
                            node1 = node;
                        }
                        else if (node.OldId == nodeId2)
                        {
                            node2 = node;
                        }
                    }

                    if (node1 != null && node2 != null)
                    {
                        this.Link(node1.ChildPort, node2.ParentPort);
                    }
                    r.ReadEndElement();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void DeserializeStreamConnections(XmlReader r, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                if (r.IsStartElement("Connection"))
                {
                    r.ReadStartElement("Connection");
                    int nodeId1 = Int32.Parse(r.ReadElementString("Node1"));
                    int nodeId2 = Int32.Parse(r.ReadElementString("Node2"));
                    string nodePort1 = r.ReadElementString("NodePort1");
                    string nodePort2 = r.ReadElementString("NodePort2");

                    libpixy.net.Controls.Diagram.Node node1 = null;
                    libpixy.net.Controls.Diagram.Node node2 = null;
                    foreach (libpixy.net.Controls.Diagram.Node node in this.Nodes)
                    {
                        if (node.OldId == nodeId1)
                        {
                            node1 = node;
                        }
                        else if (node.OldId == nodeId2)
                        {
                            node2 = node;
                        }
                    }

                    if (node1 != null && node2 != null)
                    {
                        Port port1 = node1.GetPort(nodePort1);
                        Port port2 = node2.GetPort(nodePort2);
                        if (port1 != null && port2 != null)
                        {
                            this.Link(port1, port2);
                        }
                    }

                    r.ReadEndElement();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 変更イベントのタイプ
    /// </summary>
    public enum ChangedEventType
    {
        NEW_NODE,
        REMOVE_NODE,
        SELECT_NODE,
        DESELECT_ALL,
        COLLAPSE_NODE,
        EXPAND_NODE,
        CONNECT_PORT,
        DISCONNECT_PORT,
        CLEAR,
        UPDATE
    }

    /// <summary>
    /// 変更イベントの引数
    /// </summary>
    public class ChangedEventArgs
    {
        public ChangedEventType Type;
        public Object Data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public ChangedEventArgs(
            ChangedEventType type,
            Object data)
        {
            this.Type = type;
            this.Data = data;
        }
    }

    /// <summary>
    /// 変更イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void ChangedEvent(object sender, ChangedEventArgs args);
}
