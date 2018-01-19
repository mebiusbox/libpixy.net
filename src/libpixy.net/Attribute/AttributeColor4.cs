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

namespace libpixy.net.Attribute
{
    /// <summary>
    /// 属性：カラー
    /// </summary>
    public class AttributeColor4 : Attribute
    {
        #region Fields

        public Vecmath.Color4 Value = Vecmath.Color4.White;

        #endregion Fields

        #region Properties
        #endregion Properties

        #region Attributes

        /// <summary>
        /// 属性の値設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public override void SetValue(string scriptName, float value)
        {
            if (this.ScriptName + ".r" == scriptName)
            {
                this.Value.R = value;
            }
            else if (this.ScriptName + ".g" == scriptName)
            {
                this.Value.G = value;
            }
            else if (this.ScriptName + ".b" == scriptName)
            {
                this.Value.B = value;
            }
            else if (this.ScriptName + ".a" == scriptName)
            {
                this.Value.A = value;
            }
        }

        /// <summary>
        /// 属性の値設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public override void SetValue(string scriptName, object value)
        {
            //base.SetValue(scriptName, value);
            this.Value = (Vecmath.Color4)value;
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public override object GetValue(string scriptName)
        {
            if (this.ScriptName == scriptName)
            {
                return this.Value;
            }
            else if (this.ScriptName + ".r" == scriptName)
            {
                return this.Value.R;
            }
            else if (this.ScriptName + ".g" == scriptName)
            {
                return this.Value.G;
            }
            else if (this.ScriptName + ".b" == scriptName)
            {
                return this.Value.B;
            }
            else if (this.ScriptName + ".a" == scriptName)
            {
                return this.Value.A;
            }

            return null;
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public override float GetValueAsFloat(string scriptName)
        {
            if (this.ScriptName + ".r" == scriptName)
            {
                return this.Value.R;
            }
            else if (this.ScriptName + ".g" == scriptName)
            {
                return this.Value.G;
            }
            else if (this.ScriptName + ".b" == scriptName)
            {
                return this.Value.B;
            }
            else if (this.ScriptName + ".a" == scriptName)
            {
                return this.Value.A;
            }

            return 0.0f;
        }

        #endregion Attributes

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeColor4() : base("AttributeColor4", "value")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public AttributeColor4(string scriptName) : base("AttributeColor4", scriptName)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public AttributeColor4(string scriptName, Vecmath.Color4 initial)
            : base("AttributeColor4", scriptName)
        {
            this.Value = initial;
        }

        #endregion Constructor, Destructor

        #region Serialize

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="w"></param>
        public override void Serialize(System.Xml.XmlWriter w)
        {
            base.Serialize(w);
            w.WriteElementString("r", this.Value.R.ToString());
            w.WriteElementString("g", this.Value.G.ToString());
            w.WriteElementString("b", this.Value.B.ToString());
            w.WriteElementString("a", this.Value.A.ToString());
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="r"></param>
        public override void Deserialize(System.Xml.XmlReader r)
        {
            base.Deserialize(r);
            this.Value.R = float.Parse(r.ReadElementString("r"));
            this.Value.G = float.Parse(r.ReadElementString("g"));
            this.Value.B = float.Parse(r.ReadElementString("b"));
            this.Value.A = float.Parse(r.ReadElementString("a"));
        }

        #endregion Serialize

        #region Cloneable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            AttributeColor4 clone = new AttributeColor4();
            base.Copy(clone);
            clone.Value = this.Value;
            return clone;
        }

        #endregion Cloneable
    }
}
