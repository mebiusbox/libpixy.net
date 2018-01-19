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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libpixy.net.Attribute
{
    /// <summary>
    /// 
    /// </summary>
    public class AttributeInteger : Attribute
    {
        #region Fields

        public int Value = 0;

        #endregion Fields

        #region Properties
        #endregion Properties

        #region Attributes

        /// <summary>
        /// 属性の値設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public override void SetValue(string scriptName, int value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public override object GetValue(string scriptName)
        {
            return this.Value;
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public override int GetValueAsInt(string scriptName)
        {
            return this.Value;
        }

        #endregion Attributes

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeInteger() : base("AttributeInteger", "value")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public AttributeInteger(string scriptName) : base("AttributeInteger", scriptName)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public AttributeInteger(string scriptName, int initial)
            : base("AttributeInteger", scriptName)
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
            w.WriteElementString("value", this.Value.ToString());
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="r"></param>
        public override void Deserialize(System.Xml.XmlReader r)
        {
            base.Deserialize(r);
            this.Value = Int32.Parse(r.ReadElementString("value"));
        }

        #endregion Serialize

        #region Cloneable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            AttributeInteger clone = new AttributeInteger();
            base.Copy(clone);
            clone.Value = this.Value;
            return clone;

        }

        #endregion Cloneable

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
