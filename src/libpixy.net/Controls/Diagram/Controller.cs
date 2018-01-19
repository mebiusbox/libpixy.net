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

namespace libpixy.net.Controls.Diagram
{
    /// <summary>
    /// コントローラ
    /// </summary>
    public class Controller
    {
        public Document Document { get; set; }

        #region Connection

        /// <summary>
        /// ノードを接続する。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="link"></param>
        public void Attach(Node node, Link link)
        {
            Port srcPort = link.Port1;
            Port dstPort = link.Port2;
            this.Document.Unlink(link);
            this.Document.Link(srcPort, node.ParentPort);
            this.Document.Link(node.ChildPort, dstPort);
        }

        /// <summary>
        /// ノードを分離する。その際に、分離ノードの親と子を接続しなおす。
        /// </summary>
        /// <param name="node"></param>
        public void Detach(Node node)
        {
            Port parentPort = null;
            bool parentConnected = node.ParentConnected;
            bool childConnected = node.ChildConnected;
            if (parentConnected)
            {
                if (node.ParentConnected)
                {
                    parentPort = node.ParentPort.Connections[0].Port1;
                    node.ParentPort.ClearConnection();
                }
            }

            if (childConnected)
            {
                List<Port> ports = new List<Port>();
                foreach (Link link in node.ChildPort.Connections)
                {
                    ports.Add(link.Port2);
                }

                node.ChildPort.ClearConnection();

                if (parentConnected && parentPort != null)
                {
                    foreach (Port port in ports)
                    {
                        this.Document.Link(parentPort, port);
                    }
                }

                ports = null;
            }
        }

        #endregion Connection

        #region Methods

        /// <summary>
        /// 全ノードのワークステートを初期化
        /// </summary>
        public void ClearWorkState()
        {
            foreach (Node node in this.Document.Nodes)
            {
                node.State.Work = false;
            }
        }

        /// <summary>
        /// 選択状態をクリア
        /// </summary>
        public void ClearNodeSelectState()
        {
            foreach (Node node in this.Document.SelectedNodes)
            {
                node.State.Select = false;
            }
        }

        /// <summary>
        /// 選択状態をクリア
        /// </summary>
        public void ClearSelectedNodes()
        {
            if (this.Document.SelectedNode != null)
            {
                ClearNodeSelectState();
                this.Document.ClearSelectedNodes();
                this.Document.RaiseChangedEvent(ChangedEventType.DESELECT_ALL, null);
            }
        }

        /// <summary>
        /// ノードの折り畳みを切り替える
        /// </summary>
        /// <param name="node"></param>
        public void ToggleNodeFolding(Node node)
        {
            ToggleNodeFolding(node, !node.Folded);
        }

        /// <summary>
        /// ノードの折り畳みを切り替える
        /// </summary>
        /// <param name="node"></param>
        public void ToggleNodeFolding(Node node, bool expand)
        {
            if (expand)
            {
                if (!node.Folded)
                {
                    node.Folded = true;
                    this.Document.RaiseChangedEvent(ChangedEventType.COLLAPSE_NODE, node);
                }
            }
            else
            {
                if (node.Folded)
                {
                    node.Folded = false;
                    this.Document.RaiseChangedEvent(ChangedEventType.EXPAND_NODE, node);
                }
            }
        }

        /// <summary>
        /// ノードを指定の位置に移動する
        /// </summary>
        /// <param name="node"></param>
        /// <param name="location"></param>
        public void MoveTo(Node node, Point location)
        {
            int dx = location.X - node.Location.X;
            int dy = location.Y - node.Location.Y;

            node.Location = location;
            node.ComputeBounds();

            // 出力ポートが折りたたんであったら、接続されているノードも相対移動
            if (node.DestinationFolded)
            {
                TraverseDestinationNodes(
                    node,
                    delegate(Node target, bool desitination)
                    {
                        target.Location = new Point(
                            target.Location.X + dx, target.Location.Y + dy);
                        target.ComputeBounds();
                    },
                    true);
            }

            // 子ポートが折りたたんであったら、接続されているノードも相対移動
            if (node.ChildFolded)
            {
                TraverseHierarchyNodes(
                    node,
                    delegate(Node target, bool destination)
                    {
                        target.Location = new Point(
                            target.Location.X + dx, target.Location.Y + dy);
                        target.ComputeBounds();
                    },
                    true,
                    true);
            }
        }

        /// <summary>
        /// 選択しているノードを削除する
        /// </summary>
        public void DeleteSelectedNodes()
        {
            if (this.Document.SelectedNodes != null)
            {
                foreach (Node node in this.Document.SelectedNodes)
                {
                    node.DisconnectAllPorts();
                }

                this.Document.RemoveAllNode(delegate(Node node) { return node.State.Select; });
                ClearSelectedNodes();
            }
        }



        #endregion Methods

        #region Traverse

        /// <summary>
        /// ノード走査ハンドラ
        /// </summary>
        /// <param name="node"></param>
        /// <param name="destination"></param>
        public delegate void NodeTraverseHandler(Node node, bool destination);

        /// <summary>
        /// 出力ポートに接続しているノードを走査
        /// </summary>
        /// <param name="node"></param>
        /// <param name="handler"></param>
        /// <param name="recursive"></param>
        public void TraverseDestinationNodes(Node node, NodeTraverseHandler handler, bool recursive)
        {
            if (node == null)
            {
                return;
            }

            if (node.DestinationPorts != null)
            {
                foreach (Port port in node.DestinationPorts)
                {
                    if (port.Connected)
                    {
                        foreach (Link link in port.Connections)
                        {
                            handler(link.Port2.Owner, true);
                            if (recursive)
                            {
                                TraverseHierarchyNodes(link.Port2.Owner, handler, recursive, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 子ポートに接続しているノードを走査
        /// </summary>
        /// <param name="node"></param>
        /// <param name="handler"></param>
        /// <param name="recursive"></param>
        /// <param name="destination"></param>
        public void TraverseHierarchyNodes(Node node, NodeTraverseHandler handler, bool recursive, bool destination)
        {
            if (destination)
            {
                TraverseDestinationNodes(node, handler, recursive);
            }

            if (node.ChildPort != null && node.ChildPort.Connected)
            {
                foreach (Link link in node.ChildPort.Connections)
                {
                    handler(link.Port2.Owner, true);

                    if (recursive)
                    {
                        TraverseHierarchyNodes(link.Port2.Owner, handler, recursive, destination);
                    }
                }
            }
        }

        #endregion Traverse
    }
}
