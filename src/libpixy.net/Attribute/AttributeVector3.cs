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
    /// 属性：ベクトル
    /// </summary>
    public class AttributeVector3 : Attribute
    {
        #region Fields

        public Vecmath.Vector3 Value = new Vecmath.Vector3(0.0f, 0.0f, 0.0f);

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
            if (this.ScriptName + ".x" == scriptName)
            {
                this.Value.X = value;
            }
            else if (this.ScriptName + ".y" == scriptName)
            {
                this.Value.Y = value;
            }
            else if (this.ScriptName + ".z" == scriptName)
            {
                this.Value.Z = value;
            }
        }

        /// <summary>
        /// 属性の値を設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public override void SetValue(string scriptName, object value)
        {
            //base.SetValue(scriptName, value);
            this.Value = (Vecmath.Vector3)value;
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
            else if (this.ScriptName + ".x" == scriptName)
            {
                return this.Value.X;
            }
            else if (this.ScriptName + ".y" == scriptName)
            {
                return this.Value.Y;
            }
            else if (this.ScriptName + ".z" == scriptName)
            {
                return this.Value.Z;
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
            if (this.ScriptName + ".x" == scriptName)
            {
                return this.Value.X;
            }
            else if (this.ScriptName + ".y" == scriptName)
            {
                return this.Value.Y;
            }
            else if (this.ScriptName + ".z" == scriptName)
            {
                return this.Value.Z;
            }

            return 0.0f;
        }

        #endregion Attributes

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeVector3() : base("AttributeVector3", "value")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public AttributeVector3(string scriptName) : base("AttributeVector3", scriptName)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public AttributeVector3(string scriptName, Vecmath.Vector3 initial)
            : base("AttributeVector3", scriptName)
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
            w.WriteElementString("x", this.Value.X.ToString());
            w.WriteElementString("y", this.Value.Y.ToString());
            w.WriteElementString("z", this.Value.Z.ToString());
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="r"></param>
        public override void Deserialize(System.Xml.XmlReader r)
        {
            base.Deserialize(r);
            this.Value.X = float.Parse(r.ReadElementString("x"));
            this.Value.Y = float.Parse(r.ReadElementString("y"));
            this.Value.Z = float.Parse(r.ReadElementString("z"));
        }

        #endregion Serialize

        #region Cloneable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            AttributeVector3 clone = new AttributeVector3();
            base.Copy(clone);
            clone.Value = new Vecmath.Vector3(this.Value.X, this.Value.Y, this.Value.Z);
            return clone;
        }

        #endregion Cloneable
    }
}
