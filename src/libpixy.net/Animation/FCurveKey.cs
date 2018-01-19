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

namespace libpixy.net.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public enum FCurveKeyValue
    {
        Value = 0,
        LeftTanX,
        LeftTanY,
        RightTanX,
        RightTanY
    }

    /// <summary>
    /// キー
    /// </summary>
    public class FCurveKey : Tools.BaseObject, ICloneable 
    {
        #region Fields

        private int m_frame = 0;
        private float[] m_value = new float[5];
        private FCurveInterpolationType m_type = FCurveInterpolationType.Cubic;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 補間タイプ
        /// </summary>
        public FCurveInterpolationType Interpolation
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// フレーム
        /// </summary>
        public int Frame
        {
            get { return m_frame; }
            set { m_frame = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Value
        {
            get { return m_value[0]; }
            set { m_value[0] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float LeftTanX
        {
            get { return m_value[1]; }
            set { m_value[1] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float LeftTanY
        {
            get { return m_value[2]; }
            set { m_value[2] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float RightTanX
        {
            get { return m_value[3]; }
            set { m_value[3] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float RightTanY
        {
            get { return m_value[4]; }
            set { m_value[4] = value; }
        }

        #endregion Properties

        #region Constructor, Destructor

        /// <summary>
        /// Ctor
        /// </summary>
        public FCurveKey()
            : base("key", "key")
        {
        }

        #endregion Constructor, Destructor

        #region Attributes

        /// <summary>
        /// 値を取得
        /// </summary>
        /// <returns></returns>
        public float GetValue()
        {
            return m_value[0];
        }

        /// <summary>
        /// 値を取得
        /// </summary>
        /// <param name="i">0:value, 1:Left tan X, 2:Left tan Y, 3:Right tan X, 4:Right tan Y</param>
        /// <returns></returns>
        public float GetValue(int i)
        {
            System.Diagnostics.Debug.Assert(i < 5);
            System.Diagnostics.Debug.Assert(i >= 0);
            return m_value[i];
        }

        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="i">0:value, 1:Left tan X, 2:Left tan Y, 3:Right tan X, 4:Right tan Y</param>
        /// <param name="value"></param>
        public void SetValue(int i, float value)
        {
            System.Diagnostics.Debug.Assert(i < 5);
            System.Diagnostics.Debug.Assert(i >= 0);
            m_value[i] = value;
        }

        #endregion Attributes

        #region Serialize

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public virtual void Serialize(XmlWriter w)
        {
            w.WriteElementString("script_name", this.ScriptName);
            w.WriteElementString("frame", this.Frame.ToString());
            w.WriteElementString("value", this.Value.ToString());
            w.WriteElementString("left_tan_x", this.LeftTanX.ToString());
            w.WriteElementString("left_tan_y", this.LeftTanY.ToString());
            w.WriteElementString("right_tan_x", this.RightTanX.ToString());
            w.WriteElementString("right_tan_y", this.RightTanY.ToString());
            w.WriteElementString("interpolation", this.Interpolation.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public virtual void Deserialize(XmlReader r)
        {
            this.ScriptName = r.ReadElementString("script_name");
            this.Frame = Int32.Parse(r.ReadElementString("frame"));
            this.Value = float.Parse(r.ReadElementString("value"));
            this.LeftTanX = float.Parse(r.ReadElementString("left_tan_x"));
            this.LeftTanY = float.Parse(r.ReadElementString("left_tan_y"));
            this.RightTanX = float.Parse(r.ReadElementString("right_tan_x"));
            this.RightTanY = float.Parse(r.ReadElementString("right_tan_y"));

            string interpolation = r.ReadElementString("interpolation");
            if (interpolation == "Constant")
            {
                this.Interpolation = FCurveInterpolationType.Constant;
            }
            else if (interpolation == "Linear")
            {
                this.Interpolation = FCurveInterpolationType.Linear;
            }
            else if (interpolation == "Cubic")
            {
                this.Interpolation = FCurveInterpolationType.Cubic;
            }
            else if (interpolation == "Spherical")
            {
                this.Interpolation = FCurveInterpolationType.Spherical;
            }
        }

        #endregion Serialize

        #region ICloneable

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            FCurveKey clone = new FCurveKey();
            clone.m_frame = this.m_frame;
            m_value.CopyTo(clone.m_value, 0);
            clone.m_type = this.m_type;
            return clone;
        }

        #endregion ICloneable
    }
}
