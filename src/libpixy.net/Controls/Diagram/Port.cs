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
using System.Xml;

namespace libpixy.net.Controls.Diagram
{
    /// <summary>
    /// 
    /// </summary>
    public enum PortStream
    {
        In = 0,
        Out,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PortAlignment
    {
        Left, Top, Right, Bottom
    }

    /// <summary>
    /// ポート
    /// </summary>
    public class Port : ICloneable
    {
        #region Fields

        public string Text;
        public string Type;
        public PortStream Stream;
        public PortAlignment Alignment;
        public Point ConnectionPoint;
        public Rectangle Bounds;
        public List<Link> Connections;
        public Node Owner;
        public bool EnableMultiConnection;
        public Color SingleColor;
        public Color MultiColor;

        #endregion Fields

        #region Ctor, Dtor

        /// <summary>
        /// 
        /// </summary>
        public Port()
        {
            Connections = new List<Link>();
            EnableMultiConnection = false;
            SingleColor = Color.Yellow;
            MultiColor = Color.Cyan;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Color PortColor
        {
            get
            {
                if (EnableMultiConnection)
                {
                    return MultiColor;
                }
                else
                {
                    return SingleColor;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Connected
        {
            get { return Connections.Count > 0; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void ClearConnection()
        {
            foreach (Link link in Connections)
            {
                if (link.Port1 == this)
                {
                    link.Port2.RemoveConnection(link);
                }
                else
                {
                    link.Port1.RemoveConnection(link);
                }
            }

            Connections.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        public void SetConnection(Link link)
        {
            ClearConnection();
            Connections.Add(link);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        public void AddConnection(Link link)
        {
            Connections.Add(link);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        public void RemoveConnection(Link link)
        {
            Connections.Remove(link);
        }

        #endregion Methods

        #region Serialize

        public static string PortStreamToString(PortStream strm)
        {
            if (strm == PortStream.In)
            {
                return "in";
            }
            else if (strm == PortStream.Out)
            {
                return "out";
            }

            throw new ArgumentException();
        }

        public static PortStream StringToPortStream(string str)
        {
            if (str == "in")
            {
                return PortStream.In;
            }
            else if (str == "out")
            {
                return PortStream.Out;
            }

            throw new ArgumentException();
        }

        public static string PortAlignmentToString(PortAlignment a)
        {
            switch (a)
            {
                case PortAlignment.Left:    return "left";
                case PortAlignment.Top:     return "top";
                case PortAlignment.Right:   return "right";
                case PortAlignment.Bottom:  return "bottom";
            }

            throw new ArgumentException();
        }

        public static PortAlignment StringToPortAlignment(string str)
        {
            switch (str)
            {
                case "left": return PortAlignment.Left;
                case "top": return PortAlignment.Top;
                case "right": return PortAlignment.Right;
                case "bottom": return PortAlignment.Bottom;
            }

            throw new ArgumentException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public void Serialize(XmlWriter w)
        {
            w.WriteElementString("Text", this.Text);
            w.WriteElementString("Type", this.Type);
            w.WriteElementString("PortStream", PortStreamToString(this.Stream));
            w.WriteElementString("PortAlignment", PortAlignmentToString(this.Alignment));
            w.WriteElementString("EnableMultiConnection", this.EnableMultiConnection.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void Deserialize(XmlReader r)
        {
            this.Text = r.ReadElementString("Text");
            this.Type = r.ReadElementString("Type");
            this.Stream = StringToPortStream(r.ReadElementString("PortStream"));
            this.Alignment = StringToPortAlignment(r.ReadElementString("PortAlignment"));
            this.EnableMultiConnection = Convert.ToBoolean(r.ReadElementString("EnableMultiConnection"));
        }

        #endregion Serialize

        #region IClonable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Port port = new Port();
            port.Text = this.Text;
            port.Type = this.Type;
            port.Stream = this.Stream;
            port.Alignment = this.Alignment;
            port.ConnectionPoint = this.ConnectionPoint;
            port.Bounds = this.Bounds;
            port.Owner = null;
            port.EnableMultiConnection = this.EnableMultiConnection;
            port.SingleColor = this.SingleColor;
            port.MultiColor = this.MultiColor;
            return port;
        }

        #endregion IClonable
    }

    /// <summary>
    /// ポートコレクション
    /// </summary>
    public class PortCollection : List<Port>
    {
        public Port this[string name]
        {
            get
            {
                foreach (Port port in this)
                {
                    if (port.Text == name)
                    {
                        return port;
                    }
                }

                return null;
            }
        }
    }
}
