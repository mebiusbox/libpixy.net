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

namespace libpixy.net.Attribute
{
    /// <summary>
    /// 属性が見つからなかったときに発生する例外
    /// </summary>
    public class NotFoundAttributeException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptName"></param>
        public NotFoundAttributeException(string scriptName)
            : base(scriptName)
        {
        }
    }

    /// <summary>
    /// 属性
    /// </summary>
    public class Attribute : Tools.BaseObject
    {
        #region Fields
        #endregion Fields

        #region Properties
        #endregion Properties

        #region Attributes
        #endregion Attributes

        #region Constructor, Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected Attribute() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        protected Attribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scriptName"></param>
        protected Attribute(string name, string scriptName)
            : base(name, scriptName)
        {
        }

        #endregion Constructor, Destructor

        #region Methods

        /// <summary>
        /// 属性の値設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string scriptName, int value)
        {
            //stub
        }

        /// <summary>
        /// 属性の値設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string scriptName, float value)
        {
            //stub
        }

        /// <summary>
        /// 属性の値を設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string scriptName, object value)
        {
            //stub
        }

        /// <summary>
        /// 属性の値設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string scriptName, bool value)
        {
            //stub
        }

        /// <summary>
        /// 属性の値設定
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string scriptName, string value)
        {
            //stub
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public virtual object GetValue(string scriptName)
        {
            throw new NotFoundAttributeException(scriptName);
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public virtual int GetValueAsInt(string scriptName)
        {
            throw new NotFoundAttributeException(scriptName);
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public virtual float GetValueAsFloat(string scriptName)
        {
            throw new NotFoundAttributeException(scriptName);
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public virtual bool GetValueAsBool(string scriptName)
        {
            throw new NotFoundAttributeException(scriptName);
        }

        /// <summary>
        /// 属性の値取得
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public virtual string GetValueAsString(string scriptName)
        {
            throw new NotFoundAttributeException(scriptName);
        }

        #endregion Methods

        #region Serialize

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public virtual void Serialize(XmlWriter w)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public virtual void Deserialize(XmlReader r)
        {
        }
        
        #endregion Serialize

        #region ICloneable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            Attribute clone = new Attribute();
            base.Copy(clone);
            return clone;
        }

        #endregion ICloneable
    }
}
