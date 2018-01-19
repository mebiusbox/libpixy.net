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
using System.Drawing;

namespace libpixy.net.Controls.CurveEditor
{
    /// <summary>
    /// 外挿
    /// </summary>
    public enum Extrapolation
    {
        Constant = 0,
        Cycle,
        CycleOffset
    }

    /// <summary>
    /// 曲線データ
    /// </summary>
    public class CurveContent
    {
        #region Properteis

        /// <summary>
        /// 
        /// </summary>
        public List<ControlPoint> Items = new List<ControlPoint>();

        /// <summary>
        /// 外挿
        /// </summary>
        public Extrapolation Extrapolation { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public System.Drawing.Color CurveColor { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public System.Drawing.Color PointColor { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public System.Drawing.Color ExtraColor { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ソート
        /// </summary>
        public void Sort()
        {
            this.Items.Sort(delegate(ControlPoint arg1, ControlPoint arg2)
            {
                return (arg1.Position.X.CompareTo(arg2.Position.X));
            });
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CurveContent()
        {
            this.CurveColor = System.Drawing.Color.Black;
            this.PointColor = System.Drawing.Color.Red;
            this.ExtraColor = System.Drawing.Color.Gray;
            this.Extrapolation = Extrapolation.Constant;
        }

        #endregion Constructor
    }

    /// <summary>
    /// マーカー
    /// </summary>
    public class Marker
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public PointF Position { get; set; }
        public bool XAxis { get; set; }
        public bool YAxis { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Marker()
        {
            this.Name = "";
            this.Color = Color.FromArgb(255, 0, 0);
            this.Position = new PointF(0, 0);
            this.XAxis = true;
            this.YAxis = false;
        }
    }

    /// <summary>
    /// ドキュメント
    /// </summary>
    public class Document
    {
        #region Properties

        /// <summary>
        /// 編集しない曲線データ（表示用）
        /// </summary>
        public List<CurveContent> SubContents = new List<CurveContent>();

        /// <summary>
        /// 編集データ
        /// </summary>
        public CurveContent Content = new CurveContent();

        /// <summary>
        /// 
        /// </summary>
        public List<int> SelectedItems = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        public List<int> SelectedCurves = new List<int>();

        /// <summary>
        /// マーカーリスト
        /// </summary>
        public List<Marker> Markers = new List<Marker>();

        #endregion Properties

        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Document()
        {
            this.Content.CurveColor = System.Drawing.Color.Black;
            this.Content.PointColor = System.Drawing.Color.Red;
            this.Content.Extrapolation = Extrapolation.Constant;
        }

        #endregion Constructor

        #region Public Events

        public event EventHandler Changed;

        /// <summary>
        /// 
        /// </summary>
        public void RaiseChanged()
        {
            if (this.Changed != null)
            {
                this.Changed(null, EventArgs.Empty);
            }
        }

        #endregion Public Events

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cp"></param>
        public void Add(ControlPoint cp)
        {
            this.Content.Items.Add(cp);
            RaiseChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Add(int x, int y)
        {
            ControlPoint cp = new ControlPoint();
            cp.Position.X = x;
            cp.Position.Y = y;
            Add(cp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ControlPoint IndexOf(int index)
        {
            if (index >= 0 && index < this.Content.Items.Count)
            {
                return this.Content.Items[index];
            }

            return null;
        }

        /// <summary>
        /// ソート
        /// </summary>
        public void Sort()
        {
            if (IsSelected())
            {
                ClearSelection();
            }

            this.Content.Sort();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            this.Content.Items.Clear();
            this.Content.Name = "";
            this.SelectedItems.Clear();
            this.SelectedCurves.Clear();
            RaiseChanged();

            System.Diagnostics.Debug.WriteLine("libpixy.net.Controls.CurveEditor.Document: Clear.");
        }

        /// <summary>
        /// 選択情報をクリア
        /// </summary>
        public void ClearSelection()
        {
            foreach (int index in this.SelectedItems)
            {
                this.Content.Items[index].Select = false;
                this.Content.Items[index].Handle1.Select = false;
                this.Content.Items[index].Handle2.Select = false;
            }

            foreach (int index in this.SelectedCurves)
            {
                this.Content.Items[index].Curve.Select = false;
            }

            this.SelectedItems.Clear();
            this.SelectedCurves.Clear();
        }

        /// <summary>
        /// 選択されているか
        /// </summary>
        /// <returns></returns>
        public bool IsSelected()
        {
            return IsSelectedPoint() || IsSelectedCurve();
        }

        /// <summary>
        /// コントロールポイントが選択されているか
        /// </summary>
        /// <returns></returns>
        public bool IsSelectedPoint()
        {
            return this.SelectedItems.Count > 0;
        }

        /// <summary>
        /// カーブが選択されているか
        /// </summary>
        /// <returns></returns>
        public bool IsSelectedCurve()
        {
            return this.SelectedCurves.Count > 0;
        }

        /// <summary>
        /// 指定の名前のコンテンツを検索
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public CurveContent FindContent(string scriptName)
        {
            if (this.Content.Name == scriptName)
            {
                return this.Content;
            }

            foreach (CurveContent content in this.SubContents)
            {
                if (content.Name == scriptName)
                {
                    return content;
                }
            }

            return null;
        }

        #endregion Public Methods
    }
}
