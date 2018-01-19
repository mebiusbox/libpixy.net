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
using System.Xml;

namespace Test
{
    /// <summary>
    /// ノード.
    /// </summary>
    public class TestNode : libpixy.net.Tools.BaseObject
    {
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        protected libpixy.net.Tools.Random m_random = new libpixy.net.Tools.Random();

        //..Properties

        /// <summary>
        /// アニメーションコレクション.
        /// </summary>
        public libpixy.net.Animation.Animations Animations = new libpixy.net.Animation.Animations();

        /// <summary>
        /// 属性コレクション.
        /// </summary>
        public libpixy.net.Attribute.Attributes Attributes = new libpixy.net.Attribute.Attributes();

        /// <summary>
        /// 子ノードコレクション.
        /// </summary>
        public List<TestNode> Items = new List<TestNode>();

        /// <summary>
        /// 
        /// </summary>
        public libpixy.net.Tools.UUID ParentUUID = null;

        /// <summary>
        /// 
        /// </summary>
        public libpixy.net.Tools.UUID OldUUID = null;

        /// <summary>
        /// 
        /// </summary>
        public TestNode[] SubNodes
        {
            get
            {
                List<TestNode> tmp = new List<TestNode>();
                foreach (TestNode node in Items)
                {
                    tmp.Add(node);
                    tmp.AddRange(node.SubNodes);
                }

                return tmp.ToArray();
            }
        }



        //..Methods



        /// <summary>
        /// C'tor.
        /// </summary>
        public TestNode()
        {
            this.ScriptName = "base.node";
            this.Name = "TestNode";
            this.Attributes.ValueChanged += new EventHandler<libpixy.net.Attribute.Attributes.ValueChangedEventArgs>(Attributes_ValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        public TestNode(string scriptName)
        {
            this.Name = "TestNode";
            this.ScriptName = scriptName;
            this.Attributes.ValueChanged += new EventHandler<libpixy.net.Attribute.Attributes.ValueChangedEventArgs>(Attributes_ValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        ~TestNode()
        {
            //_logger.Trace(string.Format("~SparkNode scriptName[{0}] UUID[{1}]", this.ScriptName, this.UUID.ToString()));
        }

        /// <summary>
        /// クローン生成.
        /// </summary>
        /// <returns></returns>
        public virtual TestNode Clone()
        {
            TestNode node = new TestNode();

            this.Copy(node);
            foreach (TestNode subnode in Items)
            {
                node.Items.Add(subnode.Clone());
            }
            node.Attributes.SetValue("object.id.value", node.UUID.ToString());

            return node;
        }

        /// <summary>
        /// コピー.
        /// </summary>
        /// <param name="dst"></param>
        public void Copy(TestNode dst)
        {
            base.Copy(dst);
            dst.Attributes = (libpixy.net.Attribute.Attributes)this.Attributes.Clone();
            dst.Animations = (libpixy.net.Animation.Animations)this.Animations.Clone();
        }

        /// <summary>
        /// 属性をダンプ出力
        /// </summary>
        public void DumpAttributes()
        {
            foreach (libpixy.net.Attribute.Attribute attr in this.Attributes.Items)
            {
                System.Diagnostics.Debug.WriteLine("<attr>");
                System.Diagnostics.Debug.WriteLine("  name = " + attr.Name);
                System.Diagnostics.Debug.WriteLine("  scriptName = " + attr.ScriptName);
                System.Diagnostics.Debug.WriteLine("  value = " + attr.ToString());
                System.Diagnostics.Debug.WriteLine("</attr>");
            }
        }

 
        public int GetNodeCount()
        {
            int cnt = 0;
            foreach (TestNode node in Items)
            {
                cnt += node.GetNodeCount() + 1;
            }

            return cnt;
        }

        public void RemoveInvalidAnimations()
        {
            if (this.Animations.Items.Count == 0)
            {
                return;
            }

            libpixy.net.Animation.Animations tmp = new libpixy.net.Animation.Animations();
            foreach (libpixy.net.Animation.FCurve curve in this.Animations.Items)
            {
                if (curve.Items.Count > 0)
                {
                    tmp.Items.Add(curve);
                }
            }

            this.Animations = tmp;
        }

         /// <summary>
        /// シリアライズ.
        /// </summary>
        /// <param name="w"></param>
        public virtual void Serialize(XmlWriter w)
        {
            w.WriteStartElement("Node");
            w.WriteAttributeString("name", this.Name);
            w.WriteAttributeString("scriptName", this.ScriptName);
            w.WriteAttributeString("uuid", this.UUID.ToString());
            if (this.ParentUUID != null)
            {
                w.WriteAttributeString("puuid", this.ParentUUID.ToString());
            }
            w.WriteStartElement("Attributes");
            w.WriteAttributeString("count", this.Attributes.Items.Count.ToString());
            foreach (libpixy.net.Attribute.Attribute attr in this.Attributes.Items.Values)
            {
                w.WriteStartElement("Attribute");
                w.WriteAttributeString("name", attr.Name);
                w.WriteAttributeString("scriptName", attr.ScriptName);
                attr.Serialize(w);
                w.WriteEndElement();//Attribute
            }
            w.WriteEndElement();//Attributes

            int count = 0;
            foreach (libpixy.net.Animation.FCurve curve in this.Animations.Items)
            {
                if (curve.Valid)
                {
                    count++;
                }
            }

            w.WriteStartElement("Animations");
            w.WriteAttributeString("count", count.ToString());
            foreach (libpixy.net.Animation.FCurve curve in this.Animations.Items)
            {
                if (curve.Valid)
                {
                    w.WriteStartElement("FCurve");
                    curve.Serialize(w);
                    w.WriteEndElement();//FCurve
                }
            }
            w.WriteEndElement();//Animations
            w.WriteEndElement();//Node
        }

        /// <summary>
        /// デシリアライズ.
        /// </summary>
        /// <param name="r"></param>
        public virtual void Deserialize(XmlReader r)
        {
            if (r.IsStartElement("Node") == false)
            {
                return;
            }

            this.Name = r.GetAttribute("name");
            this.ScriptName = r.GetAttribute("scriptName");
            if (this.ScriptName == "emitter.node")
            {
                this.Items.Clear();
            }

            this.UUID = new libpixy.net.Tools.UUID(r.GetAttribute("uuid"));
            string puuid = r.GetAttribute("puuid");
            if (string.IsNullOrEmpty(puuid) == false)
            {
                this.ParentUUID = new libpixy.net.Tools.UUID(r.GetAttribute("puuid"));
            }

            r.ReadStartElement("Node");

            if (r.IsStartElement("Attributes"))
            {
                int count = Int32.Parse(r.GetAttribute("count"));
                if (r.IsEmptyElement)
                {
                    r.Skip();
                }
                else
                {
                    r.ReadStartElement("Attributes");
                    for (int i = 0; i < count; ++i)
                    {
                        if (r.IsStartElement("Attribute"))
                        {
                            string name = r.GetAttribute("name");
                            string scriptName = r.GetAttribute("scriptName");

                            r.ReadStartElement("Attribute");
                            libpixy.net.Attribute.Attribute attr = this.Attributes.Find(scriptName);
                            if (attr != null)
                            {
                                attr.Deserialize(r);
                            }
                            else
                            {
                                if (name == "AttributeBoolean")
                                {
                                    libpixy.net.Attribute.AttributeBoolean newAttr = new libpixy.net.Attribute.AttributeBoolean();
                                    newAttr.Deserialize(r);
                                    newAttr.ScriptName = scriptName;
                                    this.Attributes.Items.Add(scriptName, newAttr);
                                }
                                else if (name == "AttributeInteger")
                                {
                                    libpixy.net.Attribute.AttributeInteger newAttr = new libpixy.net.Attribute.AttributeInteger();
                                    newAttr.Deserialize(r);
                                    newAttr.ScriptName = scriptName;
                                    this.Attributes.Items.Add(scriptName, newAttr);
                                }
                                else if (name == "AttributeFloat")
                                {
                                    libpixy.net.Attribute.AttributeFloat newAttr = new libpixy.net.Attribute.AttributeFloat();
                                    newAttr.Deserialize(r);
                                    newAttr.ScriptName = scriptName;
                                    this.Attributes.Items.Add(scriptName, newAttr);
                                }
                                else if (name == "AttributeString")
                                {
                                    libpixy.net.Attribute.AttributeString newAttr = new libpixy.net.Attribute.AttributeString();
                                    newAttr.Deserialize(r);
                                    newAttr.ScriptName = scriptName;
                                    this.Attributes.Items.Add(scriptName, newAttr);
                                }
                                else if (name == "AttributeVector3")
                                {
                                    libpixy.net.Attribute.AttributeVector3 newAttr = new libpixy.net.Attribute.AttributeVector3();
                                    newAttr.Deserialize(r);
                                    newAttr.ScriptName = scriptName;
                                    this.Attributes.Items.Add(scriptName, newAttr);
                                }
                                else if (name == "AttributeColor4")
                                {
                                    libpixy.net.Attribute.AttributeColor4 newAttr = new libpixy.net.Attribute.AttributeColor4();
                                    newAttr.Deserialize(r);
                                    newAttr.ScriptName = scriptName;
                                    this.Attributes.Items.Add(scriptName, newAttr);
                                }
                                else if (name == "AttributeMatrix")
                                {
                                    libpixy.net.Attribute.AttributeMatrix newAttr = new libpixy.net.Attribute.AttributeMatrix();
                                    newAttr.Deserialize(r);
                                    newAttr.ScriptName = scriptName;
                                    this.Attributes.Items.Add(scriptName, newAttr);
                                }
                            }

                            r.ReadEndElement();// Attribute
                        }
                    }
                    r.ReadEndElement();// Attributes
                }
            }

            if (r.IsStartElement("Animations"))
            {
                int count = Int32.Parse(r.GetAttribute("count"));
                if (r.IsEmptyElement)
                {
                    r.Skip();
                }
                else
                {
                    r.ReadStartElement("Animations");
                    for (int i = 0; i < count; ++i)
                    {
                        if (r.IsStartElement("FCurve"))
                        {
                            r.ReadStartElement("FCurve");
                            libpixy.net.Animation.FCurve curve = new libpixy.net.Animation.FCurve();
                            curve.Deserialize(r);
                            this.Animations.Items.Add(curve);
                            r.ReadEndElement();
                        }
                    }

                    r.ReadEndElement();// Animations
                }
            }

            r.ReadEndElement();// Node
        }



        //..Events



        /// <summary>
        /// 属性が変更された.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Attributes_ValueChanged(object sender, libpixy.net.Attribute.Attributes.ValueChangedEventArgs e)
        {
            RaiseAttributeChanged(e.ScriptName);
        }

        /// <summary>
        /// 属性が変更されたときのイベント引数.
        /// </summary>
        public class AttributeChangedEventArgs : EventArgs
        {
            public TestNode Node { get; set; }
            public string ScriptName { get; set; }
            public AttributeChangedEventArgs(TestNode node, string scriptName)
            {
                this.Node = node;
                this.ScriptName = scriptName;
            }
        }

        /// <summary>
        /// 属性が変更された.
        /// </summary>
        public event EventHandler<AttributeChangedEventArgs> AttributeChanged;

        /// <summary>
        /// 属性変更を通知する.
        /// </summary>
        /// <param name="scriptName"></param>
        private void RaiseAttributeChanged(string scriptName)
        {
            if (this.AttributeChanged != null)
            {
                this.AttributeChanged(this, new AttributeChangedEventArgs(this, scriptName));
            }
        }
    }
}
