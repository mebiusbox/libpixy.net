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
using System.Collections;
using System.Linq;
using System.Text;

namespace libpixy.net.Attribute
{
    /// <summary>
    /// 属性コンテナ
    /// </summary>
    public class Attributes : ICloneable
    {
        #region Fields
        private bool m_lockValueChanged = false;
        #endregion Fields

        #region Properteis

        //public List<Attribute> Items = new List<Attribute>();
        public Hashtable Items = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        public class ValueChangedEventArgs : EventArgs
        {
            public string ScriptName { get; set; }

            public ValueChangedEventArgs(string scriptName)
            {
                this.ScriptName = scriptName;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public Attributes()
        {
        }

        #endregion Ctor
        
        #region EventHandler

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        public void RaiseValueChanged(string scriptName)
        {
            if (ValueChanged != null && m_lockValueChanged == false)
            {
                ValueChanged(this, new ValueChangedEventArgs(scriptName));
            }
        }

        #endregion EventHandler

        #region Attributes

        public object this[string scriptName]
        {
            get { return Items[scriptName]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void SetValue(string scriptName, int value)
        {
            object obj = Items[scriptName];
            if (obj != null)
            {
                ((Attribute)obj).SetValue(scriptName, value);
                RaiseValueChanged(scriptName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void SetValue(string scriptName, float value)
        {
            object obj = Items[scriptName];
            if (obj != null)
            {
                ((Attribute)obj).SetValue(scriptName, value);
                RaiseValueChanged(scriptName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void SetValue(string scriptName, bool value)
        {
            object obj = Items[scriptName];
            if (obj != null)
            {
                ((Attribute)obj).SetValue(scriptName, value);
                RaiseValueChanged(scriptName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void SetValue(string scriptName, string value)
        {
            object obj = Items[scriptName];
            if (obj != null)
            {
                ((Attribute)obj).SetValue(scriptName, value);
                RaiseValueChanged(scriptName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void SetValue(string scriptName, object value)
        {
            object obj = Items[scriptName];
            if (obj != null)
            {
                ((Attribute)obj).SetValue(scriptName, value);
                RaiseValueChanged(scriptName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public object GetValue(string scriptName)
        {
            object attr = Items[scriptName];
            if (attr != null)
            {
                return ((Attribute)attr).GetValue(scriptName);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public int GetValueAsInt(string scriptName)
        {
            object attr = Items[scriptName];
            if (attr != null)
            {
                return ((Attribute)attr).GetValueAsInt(scriptName);
            }

            return 0;
        }

        /// <summary>
        /// 属性の値を取得。属性が存在しなければ defaultValue を返す。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetValueAsInt(string scriptName, int defaultValue)
        {
            if (Exists(scriptName))
            {
                return GetValueAsInt(scriptName);
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public float GetValueAsFloat(string scriptName)
        {
            object attr = Items[scriptName];
            if (attr != null)
            {
                return ((Attribute)attr).GetValueAsFloat(scriptName);
            }

            return 0.0f;
        }

        /// <summary>
        /// 属性の値を取得。属性が存在しなければ defaultValue を返す。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float GetValueAsFloat(string scriptName, float defaultValue)
        {
            if (Exists(scriptName))
            {
                return GetValueAsFloat(scriptName);
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public bool GetValueAsBool(string scriptName)
        {
            object attr = Items[scriptName];
            if (attr != null)
            {
                return ((Attribute)attr).GetValueAsBool(scriptName);
            }

            return false;
        }

        /// <summary>
        /// 属性の値を取得。属性が存在しなければ defaultValue を返す。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetValueAsBool(string scriptName, bool defaultValue)
        {
            if (Exists(scriptName))
            {
                return GetValueAsBool(scriptName);
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public string GetValueAsString(string scriptName)
        {
            object attr = Items[scriptName];
            if (attr != null)
            {
                return ((Attribute)attr).GetValueAsString(scriptName);
            }

            return string.Empty;
        }

        /// <summary>
        /// 属性の値を取得。属性が存在しなければ defaultValue を返す。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetValueAsString(string scriptName, string defaultValue)
        {
            if (Exists(scriptName))
            {
                return GetValueAsString(scriptName);
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public Vecmath.Vector3 GetValueAsVector3(string scriptName)
        {
            object attr = Items[scriptName];
            if (attr != null)
            {
                return ((AttributeVector3)attr).Value;
            }

            return Vecmath.Vector3.Zero;
        }

        /// <summary>
        /// 属性の値を取得。属性が存在しなければ defaultValue を返す。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Vecmath.Vector3 GetValueAsVector3(string scriptName, Vecmath.Vector3 defaultValue)
        {
            if (Exists(scriptName))
            {
                return GetValueAsVector3(scriptName);
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public Vecmath.Color4 GetValueAsColor4(string scriptName)
        {
            object attr = Items[scriptName];
            if (attr != null)
            {
                return ((AttributeColor4)attr).Value;
            }

            return Vecmath.Color4.Black;
        }

        /// <summary>
        /// 属性の値を取得。属性が存在しなければ defaultValue を返す。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Vecmath.Color4 GetValueAsColor4(string scriptName, Vecmath.Color4 defaultValue)
        {
            if (Exists(scriptName))
            {
                return GetValueAsColor4(scriptName);
            }

            return defaultValue;
        }

        #endregion Attributes

        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public bool Exists(string scriptName)
        {
            return this.Items.ContainsKey(scriptName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public bool Exists(Tools.Selector selector)
        {
            foreach (Attribute attr in Items.Values)
            {
                if (selector.Match(attr.ScriptName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Attribute Find(string scriptName)
        {
            return (Attribute)Items[scriptName];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public List<Attribute> Select(Tools.Selector selector)
        {
            List<Attribute> items = new List<Attribute>();
            foreach (Attribute attr in Items.Values)
            {
                if (selector.Match(attr.ScriptName))
                {
                    items.Add(attr);
                }
            }

            return items;
        }

        /// <summary>
        /// 属性を削除
        /// </summary>
        /// <param name="scriptName"></param>
        public void Remove(string scriptName)
        {
            Items.Remove(scriptName);
        }

        /// <summary>
        /// 属性を削除
        /// </summary>
        /// <param name="selector"></param>
        public void Remove(Tools.Selector selector)
        {
            Hashtable clone = new Hashtable();

            foreach (Attribute attr in Items.Values)
            {
                if (selector.Match(attr.ScriptName))
                {
                    //nop
                }
                else
                {
                    clone.Add(attr.ScriptName, attr);
                }
            }

            this.Items = clone;
        }

        /// <summary>
        /// 属性を除去
        /// </summary>
        /// <param name="scriptName"></param>
        public void RemoveElse(string scriptName)
        {
            Hashtable clone = new Hashtable();

            foreach (Attribute attr in Items.Values)
            {
                if (attr.ScriptName != attr.ScriptName)
                {
                    //nop
                }
                else
                {
                    clone.Add(attr.ScriptName, attr);
                }
            }

            this.Items = clone;
        }

        /// <summary>
        /// 属性を除去
        /// </summary>
        /// <param name="selector"></param>
        public void RemoveElse(Tools.Selector selector)
        {
            Hashtable clone = new Hashtable();

            foreach (Attribute attr in Items.Values)
            {
                if (selector.Match(attr.ScriptName))
                {
                    clone.Add(attr.ScriptName, attr);
                }
            }

            this.Items = clone;
        }

        #endregion Method

        #region Helper

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void AddBool(string scriptName, bool value)
        {
            Attribute attr = Find(scriptName);
            if (attr != null)
            {
                attr.SetValue(scriptName, value);
                return;
            }

            this.Items.Add(scriptName, new AttributeBoolean(scriptName, value));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void AddInt(string scriptName, int value)
        {
            Attribute attr = Find(scriptName);
            if (attr != null)
            {
                attr.SetValue(scriptName, value);
                return;
            }

            this.Items.Add(scriptName, new AttributeInteger(scriptName, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void AddFloat(string scriptName, float value)
        {
            Attribute attr = Find(scriptName);
            if (attr != null)
            {
                attr.SetValue(scriptName, value);
                return;
            }

            this.Items.Add(scriptName, new AttributeFloat(scriptName, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void AddString(string scriptName, string value)
        {
            Attribute attr = Find(scriptName);
            if (attr != null)
            {
                attr.SetValue(scriptName, value);
                return;
            }

            this.Items.Add(scriptName, new AttributeString(scriptName, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void AddVector3(string scriptName, Vecmath.Vector3 value)
        {
            Attribute attr = Find(scriptName);
            if (attr != null)
            {
                attr.SetValue(scriptName, value);
                return;
            }

            this.Items.Add(scriptName, new AttributeVector3(scriptName, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="value"></param>
        public void AddColor4(string scriptName, Vecmath.Color4 value)
        {
            Attribute attr = Find(scriptName);
            if (attr != null)
            {
                attr.SetValue(scriptName, value);
                return;
            }

            this.Items.Add(scriptName, new AttributeColor4(scriptName, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="scriptName"></param>
        /// <param name="m"></param>
        public void AddMatrix(string scriptName, Vecmath.Matrix4 value)
        {
            Attribute attr = Find(scriptName);
            if (attr != null)
            {
                attr.SetValue(scriptName, value);
                return;
            }

            this.Items.Add(scriptName, new AttributeMatrix(scriptName, value));
        }

        #endregion Helper

        #region ICloneable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Attributes clone = new Attributes();
            foreach (Attribute attr in this.Items.Values)
            {
                clone.Items.Add(attr.ScriptName, (Attribute)attr.Clone());
            }

            return clone;
        }
        #endregion ICloneable
    }
}
