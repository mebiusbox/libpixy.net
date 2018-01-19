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
    /// 属性：行列
    /// </summary>
    public class AttributeMatrix : Attribute
    {
        #region Fields

        public Vecmath.Matrix4 Value = Vecmath.Matrix4.Unit;
        
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
            if (this.ScriptName + ".11" == scriptName)
            {
                this.Value.m11 = value;
            }
            else if (this.ScriptName + ".12" == scriptName)
            {
                this.Value.m12 = value;
            }
            else if (this.ScriptName + ".13" == scriptName)
            {
                this.Value.m13 = value;
            }
            else if (this.ScriptName + ".14" == scriptName)
            {
                this.Value.m14 = value;
            }
            else if (this.ScriptName + ".21" == scriptName)
            {
                this.Value.m21 = value;
            }
            else if (this.ScriptName + ".22" == scriptName)
            {
                this.Value.m22 = value;
            }
            else if (this.ScriptName + ".23" == scriptName)
            {
                this.Value.m23 = value;
            }
            else if (this.ScriptName + ".24" == scriptName)
            {
                this.Value.m24 = value;
            }
            else if (this.ScriptName + ".31" == scriptName)
            {
                this.Value.m31 = value;
            }
            else if (this.ScriptName + ".32" == scriptName)
            {
                this.Value.m32 = value;
            }
            else if (this.ScriptName + ".33" == scriptName)
            {
                this.Value.m33 = value;
            }
            else if (this.ScriptName + ".34" == scriptName)
            {
                this.Value.m34 = value;
            }
            else if (this.ScriptName + ".41" == scriptName)
            {
                this.Value.m41 = value;
            }
            else if (this.ScriptName + ".42" == scriptName)
            {
                this.Value.m42 = value;
            }
            else if (this.ScriptName + ".43" == scriptName)
            {
                this.Value.m43 = value;
            }
            else if (this.ScriptName + ".44" == scriptName)
            {
                this.Value.m44 = value;
            }
        }

        /// <summary>
        /// 属性の値を設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public override void SetValue(string scriptName, object value)
        {
            base.SetValue(scriptName, value);
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
            else if (this.ScriptName + ".11" == scriptName)
            {
                return this.Value.m11;
            }
            else if (this.ScriptName + ".12" == scriptName)
            {
                return this.Value.m12;
            }
            else if (this.ScriptName + ".13" == scriptName)
            {
                return this.Value.m13;
            }
            else if (this.ScriptName + ".14" == scriptName)
            {
                return this.Value.m14;
            }
            else if (this.ScriptName + ".21" == scriptName)
            {
                return this.Value.m21;
            }
            else if (this.ScriptName + ".22" == scriptName)
            {
                return this.Value.m22;
            }
            else if (this.ScriptName + ".23" == scriptName)
            {
                return this.Value.m23;
            }
            else if (this.ScriptName + ".24" == scriptName)
            {
                return this.Value.m24;
            }
            else if (this.ScriptName + ".31" == scriptName)
            {
                return this.Value.m31;
            }
            else if (this.ScriptName + ".32" == scriptName)
            {
                return this.Value.m32;
            }
            else if (this.ScriptName + ".33" == scriptName)
            {
                return this.Value.m33;
            }
            else if (this.ScriptName + ".34" == scriptName)
            {
                return this.Value.m34;
            }
            else if (this.ScriptName + ".41" == scriptName)
            {
                return this.Value.m41;
            }
            else if (this.ScriptName + ".42" == scriptName)
            {
                return this.Value.m42;
            }
            else if (this.ScriptName + ".43" == scriptName)
            {
                return this.Value.m43;
            }
            else if (this.ScriptName + ".44" == scriptName)
            {
                return this.Value.m44;
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
            if (this.ScriptName + ".11" == scriptName)
            {
                return this.Value.m11;
            }
            else if (this.ScriptName + ".12" == scriptName)
            {
                return this.Value.m12;
            }
            else if (this.ScriptName + ".13" == scriptName)
            {
                return this.Value.m13;
            }
            else if (this.ScriptName + ".14" == scriptName)
            {
                return this.Value.m14;
            }
            else if (this.ScriptName + ".21" == scriptName)
            {
                return this.Value.m21;
            }
            else if (this.ScriptName + ".22" == scriptName)
            {
                return this.Value.m22;
            }
            else if (this.ScriptName + ".23" == scriptName)
            {
                return this.Value.m23;
            }
            else if (this.ScriptName + ".24" == scriptName)
            {
                return this.Value.m24;
            }
            else if (this.ScriptName + ".31" == scriptName)
            {
                return this.Value.m31;
            }
            else if (this.ScriptName + ".32" == scriptName)
            {
                return this.Value.m32;
            }
            else if (this.ScriptName + ".33" == scriptName)
            {
                return this.Value.m33;
            }
            else if (this.ScriptName + ".34" == scriptName)
            {
                return this.Value.m34;
            }
            else if (this.ScriptName + ".41" == scriptName)
            {
                return this.Value.m41;
            }
            else if (this.ScriptName + ".42" == scriptName)
            {
                return this.Value.m42;
            }
            else if (this.ScriptName + ".43" == scriptName)
            {
                return this.Value.m43;
            }
            else if (this.ScriptName + ".44" == scriptName)
            {
                return this.Value.m44;
            }

            return 0.0f;
        }

        #endregion Attributes

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeMatrix() : base("AttributeMatrix", "value")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public AttributeMatrix(string scriptName) : base("AttributeMatrix", scriptName)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public AttributeMatrix(string scriptName, Vecmath.Matrix4 initial)
            : base("AttributeMatrix", scriptName)
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
            w.WriteElementString("m11", this.Value.m11.ToString());
            w.WriteElementString("m12", this.Value.m12.ToString());
            w.WriteElementString("m13", this.Value.m13.ToString());
            w.WriteElementString("m14", this.Value.m14.ToString());

            w.WriteElementString("m21", this.Value.m21.ToString());
            w.WriteElementString("m22", this.Value.m22.ToString());
            w.WriteElementString("m23", this.Value.m23.ToString());
            w.WriteElementString("m24", this.Value.m24.ToString());

            w.WriteElementString("m31", this.Value.m31.ToString());
            w.WriteElementString("m32", this.Value.m32.ToString());
            w.WriteElementString("m33", this.Value.m33.ToString());
            w.WriteElementString("m34", this.Value.m34.ToString());

            w.WriteElementString("m41", this.Value.m41.ToString());
            w.WriteElementString("m42", this.Value.m42.ToString());
            w.WriteElementString("m43", this.Value.m43.ToString());
            w.WriteElementString("m44", this.Value.m44.ToString());
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="r"></param>
        public override void Deserialize(System.Xml.XmlReader r)
        {
            base.Deserialize(r);
            this.Value.m11 = float.Parse(r.ReadElementString("m11"));
            this.Value.m12 = float.Parse(r.ReadElementString("m12"));
            this.Value.m13 = float.Parse(r.ReadElementString("m13"));
            this.Value.m14 = float.Parse(r.ReadElementString("m14"));

            this.Value.m21 = float.Parse(r.ReadElementString("m21"));
            this.Value.m22 = float.Parse(r.ReadElementString("m22"));
            this.Value.m23 = float.Parse(r.ReadElementString("m23"));
            this.Value.m24 = float.Parse(r.ReadElementString("m24"));

            this.Value.m31 = float.Parse(r.ReadElementString("m31"));
            this.Value.m32 = float.Parse(r.ReadElementString("m32"));
            this.Value.m33 = float.Parse(r.ReadElementString("m33"));
            this.Value.m34 = float.Parse(r.ReadElementString("m34"));

            this.Value.m41 = float.Parse(r.ReadElementString("m41"));
            this.Value.m42 = float.Parse(r.ReadElementString("m42"));
            this.Value.m43 = float.Parse(r.ReadElementString("m43"));
            this.Value.m44 = float.Parse(r.ReadElementString("m44"));
        }

        #endregion Serialize

        #region Cloneable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            AttributeMatrix clone = new AttributeMatrix();
            base.Copy(clone);
            clone.Value = this.Value;
            return clone;

        }

        #endregion Cloneable
    }
}
