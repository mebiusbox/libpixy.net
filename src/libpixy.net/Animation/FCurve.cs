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
    /// Ｆカーブのキー補間の種類
    /// </summary>
    public enum FCurveInterpolationType
    {
        /// <summary>
        /// 定数
        /// </summary>
        Constant = 0,

        /// <summary>
        /// 一次
        /// </summary>
        Linear,

        /// <summary>
        /// ３次ベジェ
        /// </summary>
        Cubic,

        /// <summary>
        /// 球面線形補間
        /// </summary>
        Spherical
    }

    /// <summary>
    /// 外挿
    /// </summary>
    public enum FCurveExtrapolationType
    {
        /// <summary>
        /// 定数
        /// </summary>
        Constant = 0,

        /// <summary>
        /// 周期
        /// </summary>
        Cycle,

        /// <summary>
        /// 周期（オフセット）
        /// </summary>
        CycleOffset
    }

    /// <summary>
    /// Ｆカーブ
    /// </summary>
    public class FCurve : Tools.BaseObject, ICloneable
    {
        #region Fields

        private FCurveInterpolationType m_type = FCurveInterpolationType.Linear;
        private FCurveExtrapolationType m_extrapolation = FCurveExtrapolationType.Constant;

        #endregion Fields

        #region Properties

        /// <summary>
        /// キーフレームコレクション
        /// </summary>
        public List<FCurveKey> Items = new List<FCurveKey>();

        /// <summary>
        /// 補間
        /// </summary>
        public FCurveInterpolationType Interporation
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// 外挿
        /// </summary>
        public FCurveExtrapolationType Extrapolation
        {
            get { return m_extrapolation; }
            set { m_extrapolation = value; }
        }

        /// <summary>
        /// 有効かどうか
        /// </summary>
        public bool Valid
        {
            get { return this.Items.Count > 0; }
        }

        /// <summary>
        /// 先頭
        /// </summary>
        private FCurveKey Head
        {
            get { return this.Items[0]; }
        }

        /// <summary>
        /// 末尾
        /// </summary>
        private FCurveKey Tail
        {
            get { return this.Items[this.Items.Count - 1]; }
        }

        #endregion Properties

        #region Attributes
        #endregion Attributes

        #region Constructor, Destructor

        /// <summary>
        /// Ctor
        /// </summary>
        public FCurve()
            : base("fcurve", "fcurve")
        {
        }

        #endregion Constructor, Destructor

        #region Method

        /// <summary>
        /// キー値の最小最大値を求める
        /// </summary>
        /// <returns>最小最大値を格納した配列 (0:最小値 1:最大値)</returns>
        public int[] GetFrameRange()
        {
            int[] range = new int[2];
            range[0] = int.MaxValue;
            range[1] = int.MinValue;
            foreach (FCurveKey key in this.Items)
            {
                if (range[0] > key.Frame)
                {
                    range[0] = key.Frame;
                }

                if (range[1] < key.Frame)
                {
                    range[1] = key.Frame;
                }
            }

            return range;
        }

        /// <summary>
        /// 値の最小最大値を求める
        /// </summary>
        /// <returns>最小最大値を格納した配列 (0:最小値 1:最大値)</returns>
        public float[] GetValueRange()
        {
            float[] range = new float[2];
            range[0] = float.MaxValue;
            range[1] = float.MinValue;
            foreach (FCurveKey key in this.Items)
            {
                if (range[0] > key.Value)
                {
                    range[0] = key.Value;
                }

                if (range[1] < key.Value)
                {
                    range[1] = key.Value;
                }
            }

            return range;
        }

        /// <summary>
        /// 値を取得
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public float Eval(int frame, float default_value)
        {
            System.Diagnostics.Debug.Assert(this.Items.Count != 0);

            if (this.Items.Count == 1)
            {
                return this.Head.Value;
            }

            if (frame < this.Head.Frame)
            {
                return EvalInnerExtrapolation(frame, default_value);
            }

            if (this.Tail.Frame <= frame)
            {
                return EvalOuterExtrapolation(frame, default_value);
            }

            return EvalInterpolation(frame, default_value, 0.0f);
        }

        /// <summary>
        /// 補間後の値を取得
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private float EvalInterpolation(int frame, float default_value, float offset)
        {
            for (int i = 0; i < this.Items.Count - 1; ++i)
            {
                if (this.Items[i].Frame <= frame && frame < this.Items[i + 1].Frame)
                {
                    if (this.Items[i].Interpolation == FCurveInterpolationType.Constant)
                    {
                        return this.Items[i].Value + offset;
                    }
                    else if (this.Items[i].Interpolation == FCurveInterpolationType.Linear)
                    {
                        float t = (float)(frame - this.Items[i].Frame) / (float)(this.Items[i + 1].Frame - this.Items[i].Frame);
                        return (this.Items[i + 1].Value - this.Items[i].Value) * t + this.Items[i].Value + offset;
                    }
                    else if (this.Items[i].Interpolation == FCurveInterpolationType.Cubic)
                    {
                        float t = (float)(frame - this.Items[i].Frame) / (float)(this.Items[i + 1].Frame - this.Items[i].Frame);
                        return Vecmath.Bezeir.Eval(
                            this.Items[i + 0].Value,
                            this.Items[i + 0].Value + this.Items[i + 0].RightTanY,
                            this.Items[i + 1].Value + this.Items[i + 1].LeftTanY,
                            this.Items[i + 1].Value,
                            t) + offset;
                    }
                    else if (this.Items[i].Interpolation == FCurveInterpolationType.Spherical)
                    {
                        //TODO
                    }

                    break;
                }
            }

            return default_value;
        }

        /// <summary>
        /// 内側の外挿を計算
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="default_value"></param>
        /// <returns></returns>
        private float EvalInnerExtrapolation(int frame, float default_value)
        {
            if (this.Extrapolation == FCurveExtrapolationType.Constant)
            {
                return this.Head.Value;
            }
            else if (this.Extrapolation == FCurveExtrapolationType.Cycle)
            {
                int range = this.Tail.Frame - this.Head.Frame;
                int newFrame = this.Head.Frame - frame;
                newFrame -= (newFrame / range) * range;
                return EvalInterpolation(newFrame + this.Head.Frame, default_value, 0.0f);
            }
            else if (this.Extrapolation == FCurveExtrapolationType.CycleOffset)
            {
                int range = this.Tail.Frame - this.Head.Frame;
                int newFrame = this.Head.Frame - frame;
                float offset = 0.0f;
                while (newFrame > range)
                {
                    newFrame -= range;
                    offset -= (this.Tail.Value - this.Head.Value);
                }

                return EvalInterpolation(newFrame + this.Head.Frame, default_value, offset);
            }
            else
            {
                return default_value;
            }
        }

        /// <summary>
        /// 外側の外挿を計算
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="default_value"></param>
        /// <returns></returns>
        private float EvalOuterExtrapolation(int frame, float default_value)
        {
            if (this.Extrapolation == FCurveExtrapolationType.Constant)
            {
                return this.Tail.Value;
            }
            else if (this.Extrapolation == FCurveExtrapolationType.Cycle)
            {
                int range = this.Tail.Frame - this.Head.Frame;
                int newFrame = frame - this.Tail.Frame;
                newFrame -= (newFrame / range) * range;
                return EvalInterpolation(newFrame + this.Head.Frame, default_value, 0.0f);
            }
            else if (this.Extrapolation == FCurveExtrapolationType.CycleOffset)
            {
                int range = this.Tail.Frame - this.Head.Frame;
                int newFrame = frame - this.Tail.Frame;
                float offset = this.Tail.Value - this.Head.Value;
                while (newFrame > range)
                {
                    newFrame -= range;
                    offset += this.Tail.Value - this.Head.Value;
                }

                return EvalInterpolation(newFrame + this.Head.Frame, default_value, offset);
            }
            else
            {
                return default_value;
            }
        }

        /// <summary>
        /// 指定のフレームが補間対象のフレームかどうか
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public bool ContainsKey(int frame)
        {
            if (this.Items.Count == 0)
            {
                return false;
            }

            return true;

#if false
            for (int i = 0; i < this.Items.Count - 1; ++i )
            {
                if (this.Items[i].Frame <= frame && frame < this.Items[i + 1].Frame)
                {
                    return true;
                }
            }

            return false;
#endif
        }

        /// <summary>
        /// 指定のフレームにキーがあるかどうか
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public bool ExistsKey(int frame)
        {
            for (int i = 0; i < this.Items.Count; ++i )
            {
                if (this.Items[i].Frame == frame)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// キーを追加する
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="value"></param>
        public void AddKey(int frame, float value, FCurveInterpolationType interpolation)
        {
            AddKey(frame, value, -5.0f, 0.0f, 5.0f, 0.0f, interpolation);
        }

        /// <summary>
        /// キーを追加する
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="value"></param>
        /// <param name="leftTanX"></param>
        /// <param name="leftTanY"></param>
        /// <param name="rightTanX"></param>
        /// <param name="rightTanY"></param>
        public void AddKey(
            int frame,
            float value,
            float leftTanX,
            float leftTanY,
            float rightTanX,
            float rightTanY,
            FCurveInterpolationType interpolation)
        {
            FCurveKey ck = new FCurveKey();
            ck.Frame = frame;
            ck.Value = value;
            ck.LeftTanX = leftTanX;
            ck.LeftTanY = leftTanY;
            ck.RightTanX = rightTanX;
            ck.RightTanY = rightTanY;
            ck.Interpolation = interpolation;
            AddKey(ck);
        }

        /// <summary>
        /// キーを追加
        /// </summary>
        /// <param name="key"></param>
        public void AddKey(FCurveKey key)
        {
            for (int i = 0; i < this.Items.Count; ++i)
            {
                if (key.Frame == this.Items[i].Frame)
                {
                    this.Items[i] = key;
                    return;
                }

                if (key.Frame < this.Items[i].Frame)
                {
                    this.Items.Insert(i, key);
                    return;
                }
            }

            this.Items.Add(key);
        }

        /// <summary>
        /// キーを削除
        /// </summary>
        /// <param name="frame"></param>
        public void RemoveKey(int frame)
        {
            for (int i=0; i<this.Items.Count; ++i)
            {
                if (this.Items[i].Frame == frame)
                {
                    this.Items.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 指定のキーフレームを取得
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public FCurveKey GetKey(int frame)
        {
            for (int i = 0; i < this.Items.Count; ++i)
            {
                if (this.Items[i].Frame == frame)
                {
                    return this.Items[i];
                }
            }

            return null;
        }

        #endregion Method

        #region Serialize

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public virtual void Serialize(XmlWriter w)
        {
            w.WriteElementString("script_name", this.ScriptName);
            w.WriteElementString("interpolation", this.Interporation.ToString());
            w.WriteElementString("extrapolation", this.Extrapolation.ToString());
            w.WriteStartElement("keys");
            w.WriteAttributeString("count", this.Items.Count.ToString());
            foreach (FCurveKey key in this.Items)
            {
                w.WriteStartElement("key");
                key.Serialize(w);
                w.WriteEndElement();
            }
            w.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public virtual void Deserialize(XmlReader r)
        {
            this.ScriptName = r.ReadElementString("script_name");
            string interpolation = r.ReadElementString("interpolation");
            string extrapolation = r.ReadElementString("extrapolation");
            if (interpolation == "Constant")
            {
                this.Interporation = FCurveInterpolationType.Constant;
            }
            else if (interpolation == "Linear")
            {
                this.Interporation = FCurveInterpolationType.Linear;
            }
            else if (interpolation == "Cubic")
            {
                this.Interporation = FCurveInterpolationType.Cubic;
            }
            else if (interpolation == "Spherical")
            {
                this.Interporation = FCurveInterpolationType.Spherical;
            }

            if (extrapolation == "Constant")
            {
                this.Extrapolation = FCurveExtrapolationType.Constant;
            }
            else if (extrapolation == "Cycle")
            {
                this.Extrapolation = FCurveExtrapolationType.Cycle;
            }
            else if (extrapolation == "CycleOffset")
            {
                this.Extrapolation = FCurveExtrapolationType.CycleOffset;
            }

            if (r.IsStartElement("keys"))
            {
                int count = Int32.Parse(r.GetAttribute("count"));
                r.ReadStartElement("keys");
                for (int i = 0; i < count; ++i)
                {
                    if (r.IsStartElement("key"))
                    {
                        r.ReadStartElement("key");

                        FCurveKey key = new FCurveKey();
                        key.Deserialize(r);
                        this.Items.Add(key);

                        r.ReadEndElement();
                    }
                }
                r.ReadEndElement();
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
            FCurve clone = new FCurve();
            base.Copy(clone);
            foreach (FCurveKey key in this.Items)
            {
                clone.Items.Add((FCurveKey)key.Clone());
            }

            clone.m_type = this.m_type;
            clone.m_extrapolation = this.m_extrapolation;
            return clone;
        }

        #endregion ICloneable
    }

    public class FCurveComparer : System.Collections.Generic.IComparer<libpixy.net.Animation.FCurve>
    {
        public int Compare(libpixy.net.Animation.FCurve x, libpixy.net.Animation.FCurve y)
        {
            return (int)x.Tag - (int)y.Tag;
        }
    }
}
