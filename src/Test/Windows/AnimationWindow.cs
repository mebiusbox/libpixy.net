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
using System.Drawing;
using System.Windows.Forms;

namespace Test.Windows
{
    public partial class AnimationWindow : Form
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private libpixy.net.Tools.UUID _uuid;
        private string _scriptName;
        private libpixy.net.Animation.FCurve _curve;
        private libpixy.net.Animation.FCurve _curveBackup;
        private bool _eventLocked = false;
        private bool _dirtyNodeList = true;
        private bool _resetGraphRuler = true;
        private int _currentFrame = 0;
        private TestNode _node = new TestNode();

        public AnimationWindow()
        {
            _logger.Trace("AnimationWindow.ctor");

            InitializeComponent();
            ClearTarget();
            MouseWheel += new MouseEventHandler(AnimationWindow_MouseWheel);
            uiCurveEditor.Document.Content.CurveColor = Color.Black;
            uiCurveEditor.Document.Content.PointColor = Color.Red;
            uiCurveEditor.Document.Content.Extrapolation = libpixy.net.Controls.CurveEditor.Extrapolation.Constant;

            this._node.ScriptName = "scene.node";
            this._node.Name = "Scene";
            this._node.Attributes.AddBool("object", true);
            this._node.Attributes.AddString("object.id.value", _node.UUID.ToString());
            this._node.Attributes.AddString("object.name.value", _node.Name);
            this._node.Attributes.AddBool("kine", true);
            this._node.Attributes.AddFloat("kine.local.pos.x.value", 0.0f);
            this._node.Attributes.AddFloat("kine.local.pos.y.value", 0.0f);
            this._node.Attributes.AddFloat("kine.local.pos.z.value", 0.0f);
            this._scriptName = "kine.local.pos.x";
            this._uuid = new libpixy.net.Tools.UUID("OFJOAIJFOIEJFO");
            this.BeginEditCurve();

            this.timer1.Enabled = true;
        }

        #region =========== Properties ==========
        #endregion

        #region =========== Public Methods ==========
        #endregion

        /// <summary>
        /// カーブの編集開始.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="scriptName"></param>
        public void BeginEditCurve(libpixy.net.Tools.UUID uuid, string scriptName)
        {
            _uuid = uuid;
            _scriptName = scriptName;
            BeginEditCurve();
        }

        #region =========== Private Methods ==========
        #endregion

        private void ClearTarget()
        {
            _scriptName = string.Empty;
            _curve = null;
            _curveBackup = null;
            _uuid = null;
        }

        private bool CheckTarget()
        {
            if (string.IsNullOrEmpty(_scriptName))
            {
                return false;
            }

            if (_curve != null && _uuid != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 定規更新
        /// </summary>
        private void ResetGraphRuler()
        {
            if (_resetGraphRuler == false)
            { 
                return; 
            }

            uiCurveEditor.ResetCanvas();

            if (CheckTarget() == false) 
            {
                return; 
            }

            uiCurveEditor.YOffset = 0.0f;
            uiCurveEditor.YTickLength = 10;
            uiCurveEditor.YTick = 1;

            libpixy.net.Tools.ScopeName scope = new libpixy.net.Tools.ScopeName(_curve.ScriptName);
            if (scope.Match("*.rot.*", libpixy.net.Tools.ScopeName.MatchMode.Wildcard))
            {
                Rectangle rc = uiCurveEditor.GetPlotArea();
                int h = (rc.Height * 90) / 100;
                uiCurveEditor.YTickLength = h / 36;
                uiCurveEditor.YStart = 0;
                uiCurveEditor.YUnit = 10f;
                uiCurveEditor.YFormat = "0";
                ////uiCurveEditor.YTickWidth = 0;
            }
            else if (
                scope.Match("*.r", libpixy.net.Tools.ScopeName.MatchMode.Wildcard) ||
                scope.Match("*.g", libpixy.net.Tools.ScopeName.MatchMode.Wildcard) ||
                scope.Match("*.b", libpixy.net.Tools.ScopeName.MatchMode.Wildcard) ||
                scope.Match("*.a", libpixy.net.Tools.ScopeName.MatchMode.Wildcard))
            {
                Rectangle rc = uiCurveEditor.GetPlotArea();
                int h = (rc.Height * 90) / 100;
                uiCurveEditor.YTickLength = h / 10;
                uiCurveEditor.YStart = 0;
                uiCurveEditor.YUnit = 0.1f;
                uiCurveEditor.YFormat = "0.0";
            }
            else
            {
                Rectangle rc = uiCurveEditor.GetPlotArea();
                int h = (rc.Height * 90) / 100;
                uiCurveEditor.YTickLength = h / 10;
                uiCurveEditor.YStart = 0;

                float value = 0.0f;
                foreach (libpixy.net.Animation.FCurveKey key in _curve.Items)
                {
                    if (Math.Abs(key.GetValue()) > 0.0f)
                    {
                        value = Math.Abs(key.GetValue());
                    }
                }

                if (value < 0.0001f)
                {
                    uiCurveEditor.YFormat = "0.00000";
                    uiCurveEditor.YUnit = 0.00001f;
                }
                else if (value < 0.001f)
                {
                    uiCurveEditor.YFormat = "0.0000";
                    uiCurveEditor.YUnit = 0.0001f;
                }
                else if (value < 0.01f)
                {
                    uiCurveEditor.YFormat = "0.000";
                    uiCurveEditor.YUnit = 0.001f;
                }
                else if (value < 0.1f)
                {
                    uiCurveEditor.YFormat = "0.00";
                    uiCurveEditor.YUnit = 0.01f;
                }
                else if (value < 1.0f)
                {
                    uiCurveEditor.YFormat = "0.0";
                    uiCurveEditor.YUnit = 0.1f;
                }
                else
                {
                    uiCurveEditor.YFormat = "0";
                    uiCurveEditor.YUnit = 1.0f;
                }
            }

            //uiUnit.Text = uiCurveEditor.YUnit.ToString(App.Sys.FloatValueFormat);
            uiTickLength.Text = uiCurveEditor.YTickLength.ToString();
            uiTick.Text = uiCurveEditor.YTick.ToString();
        }

        /// <summary>
        /// キーフレームアニメーションから曲線データを作成する
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        private libpixy.net.Controls.CurveEditor.CurveContent CreateContent(libpixy.net.Animation.FCurve curve, bool force)
        {
            if (force == false && (curve == null || curve.Valid == false))
            {
                return null;
            }

            libpixy.net.Controls.CurveEditor.CurveContent content = new libpixy.net.Controls.CurveEditor.CurveContent();
            content.Name = curve.ScriptName;

            switch (curve.Extrapolation)
            {
                case libpixy.net.Animation.FCurveExtrapolationType.Constant:
                    content.Extrapolation = libpixy.net.Controls.CurveEditor.Extrapolation.Constant;
                    break;

                case libpixy.net.Animation.FCurveExtrapolationType.Cycle:
                    content.Extrapolation = libpixy.net.Controls.CurveEditor.Extrapolation.Cycle;
                    break;

                case libpixy.net.Animation.FCurveExtrapolationType.CycleOffset:
                    content.Extrapolation = libpixy.net.Controls.CurveEditor.Extrapolation.CycleOffset;
                    break;
            }

            foreach (libpixy.net.Animation.FCurveKey key in curve.Items)
            {
                PointF value = new PointF(key.Frame, (float)key.Value);
                PointF handle1 = new PointF(key.Frame + key.RightTanX, key.Value + key.RightTanY);
                PointF handle2 = new PointF(key.Frame + key.LeftTanX, key.Value + key.LeftTanY);
                Point p = uiCurveEditor.ValueToCanvas(value);
                Point ph1 = uiCurveEditor.ValueToCanvas(handle1);
                Point ph2 = uiCurveEditor.ValueToCanvas(handle2);

                libpixy.net.Controls.CurveEditor.ControlPoint cp = new libpixy.net.Controls.CurveEditor.ControlPoint();
                cp.Position = new libpixy.net.Vecmath.Vector2(p.X, p.Y);
                cp.Handle1Position = new libpixy.net.Vecmath.Vector2(ph1.X, ph1.Y);
                cp.Handle2Position = new libpixy.net.Vecmath.Vector2(ph2.X, ph2.Y);

                switch (key.Interpolation)
                {
                    case libpixy.net.Animation.FCurveInterpolationType.Constant:
                        cp.Curve = new libpixy.net.Controls.CurveEditor.Curve();
                        cp.Curve.Type = libpixy.net.Controls.CurveEditor.CurveType.Constant;
                        break;

                    case libpixy.net.Animation.FCurveInterpolationType.Linear:
                        cp.Curve = new libpixy.net.Controls.CurveEditor.Curve();
                        cp.Curve.Type = libpixy.net.Controls.CurveEditor.CurveType.Linear;
                        break;

                    case libpixy.net.Animation.FCurveInterpolationType.Cubic:
                        cp.Curve = new libpixy.net.Controls.CurveEditor.Curve();
                        cp.Curve.Type = libpixy.net.Controls.CurveEditor.CurveType.Bezeir;
                        break;

                    case libpixy.net.Animation.FCurveInterpolationType.Spherical:
                        // TODO: 
                        break;
                }

                content.Items.Add(cp);
            }

            return content;
        }

        private void ResetGraph()
        {
            uiCurveEditor.Document.SubContents.Clear();
            UpdateGraph();
            UpdateMarker();

            foreach (ToolStripItem item in uiShowParam.DropDownItems)
            {
                if (item.Image != null && item.Text != _scriptName)
                {
                    AddCurve(item.Text);
                }
            }
        }

        /// <summary>
        /// Ｆカーブデータをアニメーションエディタの編集データに設定
        /// </summary>
        private void UpdateGraph()
        {
            if (CheckTarget() == false)
            {
                return; 
            }

            uiCurveEditor.Document.Clear();

            libpixy.net.Controls.CurveEditor.CurveContent content = CreateContent(_curve, true);
            if (content != null)
            {
                libpixy.net.Tools.ScopeName scope = new libpixy.net.Tools.ScopeName(_scriptName);
                if (_scriptName.EndsWith(".x") || _scriptName.EndsWith(".r"))
                {
                    content.CurveColor = Color.FromArgb(128, 255, 0, 0);
                }
                else if (_scriptName.EndsWith(".y") || _scriptName.EndsWith(".g"))
                {
                    content.CurveColor = Color.FromArgb(128, 0, 255, 0);
                }
                else if (_scriptName.EndsWith(".z") || _scriptName.EndsWith(".b"))
                {
                    content.CurveColor = Color.FromArgb(128, 0, 0, 255);
                }

                uiCurveEditor.Document.Content = content;
                uiCurveEditor.Invalidate();
            }
        }

        /// <summary>
        /// アニメーションエディタ上の編集データをＦカーブデータに変換
        /// </summary>
        private void UpdateFCurve()
        {
            if (CheckTarget() == false)
            {
                return;
            }

            UpdateFCurve(uiCurveEditor.Document.Content, _curve);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="curve"></param>
        private void UpdateFCurve(
            libpixy.net.Controls.CurveEditor.CurveContent content,
            libpixy.net.Animation.FCurve curve)
        {
            //if (Tool.Preconditions.CheckNull(content, "content", _logger))
            //{
            //    return;
            //}

            //if (Tool.Preconditions.CheckNull(curve, "curve", _logger))
            //{
            //    return;
            //}

            curve.Items.Clear();

            foreach (libpixy.net.Controls.CurveEditor.ControlPoint cp in content.Items)
            {
                PointF value = uiCurveEditor.CanvasToValue(new PointF(cp.Position.X, cp.Position.Y));
                PointF handle1 = uiCurveEditor.CanvasToValue(new PointF(cp.Handle1Position.X, cp.Handle1Position.Y));
                PointF handle2 = uiCurveEditor.CanvasToValue(new PointF(cp.Handle2Position.X, cp.Handle2Position.Y));
                libpixy.net.Animation.FCurveKey key = new libpixy.net.Animation.FCurveKey();
                key.Frame = (int)value.X;
                key.Value = value.Y;
                key.LeftTanX = handle2.X - value.X;
                key.LeftTanY = handle2.Y - value.Y;
                key.RightTanX = handle1.X - value.X;
                key.RightTanY = handle1.Y - value.Y;

                switch (cp.Curve.Type)
                {
                    case libpixy.net.Controls.CurveEditor.CurveType.Constant:
                        key.Interpolation = libpixy.net.Animation.FCurveInterpolationType.Constant;
                        break;

                    case libpixy.net.Controls.CurveEditor.CurveType.Linear:
                        key.Interpolation = libpixy.net.Animation.FCurveInterpolationType.Linear;
                        break;

                    case libpixy.net.Controls.CurveEditor.CurveType.Bezeir:
                        key.Interpolation = libpixy.net.Animation.FCurveInterpolationType.Cubic;
                        break;

                    default:
                        key.Interpolation = libpixy.net.Animation.FCurveInterpolationType.Linear;
                        break;
                }

                curve.AddKey(key);
            }
        }

        /// <summary>
        /// ノード名リスト用の名前を生成する
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string MakeCombinedNodeName(TestNode node)
        {
            string name = string.Format("{0}.{1}", node.Name, node.UUID.ToString());
            string nodename = node.Attributes.GetValueAsString("node.name");
            if (string.IsNullOrEmpty(nodename) == false)
            {
                name += "." + nodename;
            }

            return name;
        }

        /// <summary>
        /// ノード名リストをリセット
        /// </summary>
        private void ResetNodeNameList()
        {
            uiNode.Items.Clear();
            uiParam.Items.Clear();
            _dirtyNodeList = false;
        }

        /// <summary>
        /// 指定の属性が存在するかチェックし、存在すればノードパラメータリストに登録する
        /// </summary>
        /// <param name="scriptName"></param>
        private void AddNodeParam(TestNode node, string scriptName)
        {
            if (node.Attributes.Exists(scriptName + ".value"))
            {
                uiParam.Items.Add(scriptName);
                uiShowParam.DropDownItems.Add(scriptName);
            }
        }

        /// <summary>
        /// 指定の属性が存在するかチェックし、存在すればノードパラメータリストに登録する
        /// </summary>
        /// <param name="scriptName"></param>
        private void AddNodeVectorParam(TestNode node, string scriptName)
        {
            if (node.Attributes.Exists(scriptName + ".x.value"))
            {
                uiParam.Items.Add(scriptName + ".x");
                uiParam.Items.Add(scriptName + ".y");
                uiParam.Items.Add(scriptName + ".z");
                uiShowParam.DropDownItems.Add(scriptName + ".x");
                uiShowParam.DropDownItems.Add(scriptName + ".y");
                uiShowParam.DropDownItems.Add(scriptName + ".z");
            }
        }

        /// <summary>
        /// 指定の属性が存在するかチェックし、存在すればノードパラメータリストに登録する
        /// </summary>
        /// <param name="scriptName"></param>
        private void AddNodeColorParam(TestNode node, string scriptName)
        {
            if (node.Attributes.Exists(scriptName + ".r.value"))
            {
                uiParam.Items.Add(scriptName + ".r");
                uiParam.Items.Add(scriptName + ".g");
                uiParam.Items.Add(scriptName + ".b");
                uiParam.Items.Add(scriptName + ".a");
                uiShowParam.DropDownItems.Add(scriptName + ".r");
                uiShowParam.DropDownItems.Add(scriptName + ".g");
                uiShowParam.DropDownItems.Add(scriptName + ".b");
                uiShowParam.DropDownItems.Add(scriptName + ".a");
            }
        }

        /// <summary>
        /// ノードパラメータリストをリセット
        /// </summary>
        private void ResetNodeParamList()
        {
            uiParam.Items.Clear();
            uiShowParam.DropDownItems.Clear();

            TestNode node = GetCurrentNode();
            if (node == null)
            {
                return;
            }

            AddNodeVectorParam(node, "kine.local.pos");
            AddNodeVectorParam(node, "kine.local.rot");
            AddNodeVectorParam(node, "kine.local.scl");
        }

        /// <summary>
        /// 現在アクティブなノードを取得する
        /// </summary>
        /// <returns></returns>
        private TestNode GetCurrentNode()
        {
            //if (uiNode.SelectedItem == null)
            //{
            //    return null;
            //}

            //string nodeName = (string)uiNode.SelectedItem;
            //string[] names = nodeName.Split(new char[] { '.' });
            //if (names.Length < 2)
            //{
            //    return null;
            //}

            //libpixy.net.Tools.UUID uuid = new libpixy.net.Tools.UUID();
            //if (uuid.Parse(names[1]) == false)
            //{
            //    return null;
            //}

            return this._node;
        }

        /// <summary>
        /// 現在アクティブなノードを取得する
        /// </summary>
        /// <returns></returns>
        private string GetCurrentParamName()
        {
            if (uiParam.SelectedItem == null)
            {
                return null;
            }

            return (string)uiParam.SelectedItem;
        }

        /// <summary>
        /// カーブ編集開始
        /// </summary>
        private void BeginEditCurve()
        {
            if (string.IsNullOrEmpty(_scriptName))
            {
                return;
            }

            _eventLocked = true;

            bool xyz = false;
            bool rgba = false;
            if (_scriptName.EndsWith(".x") ||
                _scriptName.EndsWith(".y") ||
                _scriptName.EndsWith(".z"))
            {
                xyz = true;
            }
            else if (
                _scriptName.EndsWith(".r") ||
                _scriptName.EndsWith(".g") ||
                _scriptName.EndsWith(".b") ||
                _scriptName.EndsWith(".a"))
            {
                rgba = true;
            }

            TestNode node = _node;
            if (node != null)
            {
                _curve = node.Animations.Find(_scriptName);
            }

            if (_curve == null)
            {
                _curve = new libpixy.net.Animation.FCurve();
                _curve.ScriptName = _scriptName;
                node.Animations.Items.Add(_curve);
            }

            // Backup
            _curveBackup = (libpixy.net.Animation.FCurve)_curve.Clone();

            string nodeName = MakeCombinedNodeName(node);
            if (_dirtyNodeList)
            {
                ResetNodeNameList();
            }

            SelectNode(nodeName);
            ResetNodeParamList();
            SelectNodeParam(_scriptName);
            SetEditNodeParam(_scriptName, true);
            uiInterpolation.SelectedIndex = 0;
            uiExtrapolation.SelectedIndex = (int)_curve.Extrapolation;
            uiCurveEditor.Document.SubContents.Clear();
            ResetGraphRuler();
            UpdateGraph();
            UpdateMarker();

            // XYZ, RGBA の要素なら、他の要素も表示する
            if (xyz)
            {
                string body = _scriptName.Substring(0, _scriptName.Length - 2);
                ShowNodeParam(body + ".x", true);
                ShowNodeParam(body + ".y", true);
                ShowNodeParam(body + ".z", true);
                ShowNodeParam(body + ".x.value", true);
                ShowNodeParam(body + ".y.value", true);
                ShowNodeParam(body + ".z.value", true);
            }
            else if (rgba)
            {
                string body = _scriptName.Substring(0, _scriptName.Length - 2);
                ShowNodeParam(body + ".r", true);
                ShowNodeParam(body + ".g", true);
                ShowNodeParam(body + ".b", true);
                ShowNodeParam(body + ".a", true);
                ShowNodeParam(body + ".r.value", true);
                ShowNodeParam(body + ".g.value", true);
                ShowNodeParam(body + ".b.value", true);
                ShowNodeParam(body + ".a.value", true);
            }

            _eventLocked = false;
        }

        /// <summary>
        /// ノードを選択する
        /// </summary>
        /// <param name="nodeName"></param>
        private void SelectNode(string nodeName)
        {
            for (int i = 0; i < uiNode.Items.Count; ++i)
            {
                if ((string)uiNode.Items[i] == nodeName)
                {
                    uiNode.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// ノードパラメータを選択する
        /// </summary>
        /// <param name="scriptName"></param>
        private void SelectNodeParam(string scriptName)
        {
            for (int i = 0; i < uiParam.Items.Count; ++i)
            {
                if ((string)uiParam.Items[i] == scriptName)
                {
                    uiParam.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        ///  パラメータを表示する
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="show"></param>
        private void SetEditNodeParam(string scriptName, bool edit)
        {
            for (int i = 0; i < uiShowParam.DropDownItems.Count; ++i)
            {
                if (uiShowParam.DropDownItems[i].Text == scriptName)
                {
                    if (edit)
                    {
                        uiShowParam.DropDownItems[i].Image = Properties.Resources.pencil;
                    }
                    else
                    {
                        uiShowParam.DropDownItems[i].Image = null;
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///  パラメータを表示する
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="show"></param>
        private void ToggleNodeParam(string scriptName)
        {
            if (uiCurveEditor.Document.Content.Name == scriptName)
            {
                return;
            }

            for (int i = 0; i < uiShowParam.DropDownItems.Count; ++i)
            {
                if (uiShowParam.DropDownItems[i].Text == scriptName)
                {
                    if (uiShowParam.DropDownItems[i].Image == null)
                    {
                        uiShowParam.DropDownItems[i].Image = Properties.Resources.tick;
                        AddCurve(scriptName);
                    }
                    else
                    {
                        uiShowParam.DropDownItems[i].Image = null;
                        RemoveCurve(scriptName);
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///  パラメータを表示する
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="show"></param>
        private void ShowNodeParam(string scriptName, bool show)
        {
            for (int i = 0; i < uiShowParam.DropDownItems.Count; ++i)
            {
                if (uiShowParam.DropDownItems[i].Text == scriptName)
                {
                    if (show)
                    {
                        if (uiShowParam.DropDownItems[i].Image == null)
                        {
                            if (AddCurve(scriptName))
                            {
                                uiShowParam.DropDownItems[i].Image = Properties.Resources.tick;
                            }
                        }
                    }
                    else
                    {
                        if (uiShowParam.DropDownItems[i].Image != null)
                        {
                            uiShowParam.DropDownItems[i].Image = null;
                            RemoveCurve(scriptName);
                        }
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 曲線コンテンツを追加する
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        private bool AddCurve(string scriptName)
        {
            TestNode node = GetCurrentNode();
            if (node == null)
            {
                return false;
            }

            libpixy.net.Controls.CurveEditor.CurveContent content = uiCurveEditor.Document.FindContent(scriptName);
            if (content != null)
            {
                return true;
            }

            Color[] color = new Color[] 
            {
                Color.FromArgb(128, 0, 0, 192),
                Color.FromArgb(128, 0, 0, 128),
                Color.FromArgb(128, 0, 0, 64),
                Color.FromArgb(128, 0, 192, 0),
                Color.FromArgb(128, 0, 128, 0),
                Color.FromArgb(128, 0, 64, 0),
                Color.FromArgb(128, 0, 255, 255),
                Color.FromArgb(128, 0, 192, 255),
                Color.FromArgb(128, 0, 128, 255),
                Color.FromArgb(128, 0, 64, 255),
                Color.FromArgb(128, 0, 255, 192),
                Color.FromArgb(128, 0, 255, 128),
                Color.FromArgb(128, 0, 255, 64),
                Color.FromArgb(128, 255, 0, 255),
                Color.FromArgb(128, 255, 0, 192),
                Color.FromArgb(128, 255, 0, 128),
                Color.FromArgb(128, 255, 0, 64)
            };

            libpixy.net.Animation.FCurve curve = node.Animations.Find(scriptName);
            if (curve != null)
            {
                content = CreateContent(curve, false);
                if (content != null)
                {
                    if (curve.ScriptName.EndsWith(".x")
                        || curve.ScriptName.EndsWith(".x.value")
                        || curve.ScriptName.EndsWith(".r")
                        || curve.ScriptName.EndsWith(".r.value"))
                    {
                        content.CurveColor = Color.FromArgb(128, 255, 0, 0);
                    }
                    else if (curve.ScriptName.EndsWith(".y")
                        || curve.ScriptName.EndsWith(".y.value")
                        || curve.ScriptName.EndsWith(".g")
                        || curve.ScriptName.EndsWith(".g.value"))
                    {
                        content.CurveColor = Color.FromArgb(128, 0, 255, 0);
                    }
                    else if (curve.ScriptName.EndsWith(".z")
                        || curve.ScriptName.EndsWith(".z.value")
                        || curve.ScriptName.EndsWith(".b")
                        || curve.ScriptName.EndsWith(".b.value"))
                    {
                        content.CurveColor = Color.FromArgb(128, 0, 0, 255);
                    }
                    else
                    {
                        if (uiCurveEditor.Document.SubContents.Count < color.Length)
                        {
                            content.CurveColor = color[uiCurveEditor.Document.SubContents.Count];
                        }
                        else
                        {
                            content.CurveColor = Color.FromArgb(128, 0, 0, 0);
                        }
                    }

                    content.PointColor = Color.FromArgb(128, 128, 128, 128);
                    content.ExtraColor = content.CurveColor;

                    uiCurveEditor.Document.SubContents.Add(content);
                    uiCurveEditor.Invalidate();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 指定の曲線を非表示にする
        /// </summary>
        /// <param name="scriptName"></param>
        private void RemoveCurve(string scriptName)
        {
            foreach (libpixy.net.Controls.CurveEditor.CurveContent content in uiCurveEditor.Document.SubContents)
            {
                if (content.Name == scriptName)
                {
                    uiCurveEditor.Document.SubContents.Remove(content);
                    break;
                }
            }
        }

        /// <summary>
        /// マーカーを更新する
        /// </summary>
        private void UpdateMarker()
        {
            uiCurveEditor.Document.Markers.Clear();
            TestNode node = GetCurrentNode();
            if (node == null)
            {
                return;
            }

            int time1 = 0;
            int time2 = 10;
            int time3 = 20;

            libpixy.net.Controls.CurveEditor.Marker marker1 = new libpixy.net.Controls.CurveEditor.Marker();
            marker1.Name = "time1";
            marker1.Position = new PointF(1.0f + time1, 0.0f);
            marker1.Color = Color.FromArgb(64, 0, 0, 255);
            uiCurveEditor.Document.Markers.Add(marker1);

            libpixy.net.Controls.CurveEditor.Marker marker2 = new libpixy.net.Controls.CurveEditor.Marker();
            marker2.Name = "time2";
            marker2.Position = new PointF(1.0f + time2, 0.0f);
            marker2.Color = Color.FromArgb(64, 255, 0, 255);
            uiCurveEditor.Document.Markers.Add(marker2);

            libpixy.net.Controls.CurveEditor.Marker marker3 = new libpixy.net.Controls.CurveEditor.Marker();
            marker3.Name = "time3";
            marker3.Position = new PointF(1.0f + time3, 0.0f);
            marker3.Color = Color.FromArgb(64, 255, 0, 0);
            uiCurveEditor.Document.Markers.Add(marker3);
        }

        private void RemoveSelectedKey()
        {
            foreach (int index in uiCurveEditor.Document.SelectedItems)
            {
                PointF value = uiCurveEditor.CanvasToValue(uiCurveEditor.Document.Content.Items[index].Position.ToPoint());
                _curve.RemoveKey((int)value.X);
            }

            UpdateGraph();
        }

        #region =========== UI Events ==========
        #endregion

        private void AnimationWindow_KeyDown(object sender, KeyEventArgs e)
        {
//            _logger.Trace("AnimationWindow_KeyDown");
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedKey();
            }
            //else
            //{
            //    Utils.ProcessKeyDown(e, App.Doc, null, null);
            //}
        }

        private void AnimationWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            Rectangle rc = uiCurveEditor.GetPlotArea();
            Point p = uiCurveEditor.PlotToClient(new Point(rc.Width / 2, rc.Height / 2));
            Point canvas_old = uiCurveEditor.ClientToCanvas(p, false);
            PointF val = uiCurveEditor.ClientToValue(p);
            uiCurveEditor.PreCalcPointPositions();

            if (e.Delta > 0)
            {
                int len = uiCurveEditor.YTickLength;
                uiCurveEditor.YTickLength = Math.Max(len - 2, 5);
            }
            else
            {
                uiCurveEditor.YTickLength += 2;
            }

            Point canvas_new = uiCurveEditor.ValueToCanvas(val);
            Point org = uiCurveEditor.CanvasOrigin;
            Point new_org = new Point(org.X, org.Y - (canvas_new.Y - canvas_old.Y));
            uiCurveEditor.CanvasOrigin = new_org;
            uiCurveEditor.CalcPointPositions();
        }

        private void uiRefreshParam_Click(object sender, EventArgs e)
        {
            ResetNodeNameList();
        }

        private void uiRefreshGraph_Click(object sender, EventArgs e)
        {
            uiCurveEditor.Document.SubContents.Clear();
            UpdateGraph();
            UpdateMarker();

            foreach (ToolStripItem item in uiShowParam.DropDownItems)
            {
                if (item.Image != null && item.Text != _scriptName)
                {
                    AddCurve(item.Text);
                }
            }
        }

        private void uiZoomActual_Click(object sender, EventArgs e)
        {
            uiCurveEditor.CanvasScale = new PointF(1.0f, 1.0f);
        }

        private void uiZoomIn_Click(object sender, EventArgs e)
        {
            uiCurveEditor.CanvasScale = new PointF(
               Math.Max(uiCurveEditor.CanvasScale.X + 1.0f, 0.1f),
               Math.Max(uiCurveEditor.CanvasScale.Y + 1.0f, 0.1f));
        }

        private void uiZoomOut_Click(object sender, EventArgs e)
        {
            uiCurveEditor.CanvasScale = new PointF(
                Math.Max(uiCurveEditor.CanvasScale.X - 1.0f, 0.1f),
                Math.Max(uiCurveEditor.CanvasScale.Y - 1.0f, 0.1f));
        }

        private void uiCurveEditor_CanvasMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control)
            {
                Point pt = uiCurveEditor.ClientToCanvas(e.Location, true);
                libpixy.net.Controls.CurveEditor.ControlPoint cp = new libpixy.net.Controls.CurveEditor.ControlPoint();
                cp.Position = new libpixy.net.Vecmath.Vector2(pt.X, pt.Y);
                cp.Curve.Type = libpixy.net.Controls.CurveEditor.CurveType.Linear;
                uiCurveEditor.Document.Add(cp);
                uiCurveEditor.Invalidate();
                UpdateFCurve();
                UpdateGraph();
            }
        }

        private void uiCurveEditor_ControlPointChanged(object sender, EventArgs e)
        {
            uiCurveEditor_ControlPointSelected(sender, e);
        }

        private void uiCurveEditor_ControlPointSelected(object sender, EventArgs e)
        {
            if (uiCurveEditor.Document.SelectedItems.Count == 1)
            {
                int index = uiCurveEditor.Document.SelectedItems[0];
                libpixy.net.Vecmath.Vector2 pos = uiCurveEditor.Document.Content.Items[index].Position;
                PointF value = uiCurveEditor.CanvasToValue(new PointF(pos.X, pos.Y));
                uiKeyFrame.Text = value.X.ToString();
                //uiKeyValue.Text = value.Y.ToString(App.Sys.FloatValueFormat);
            }
            else
            {
                uiKeyFrame.Text = string.Empty;
                uiKeyValue.Text = string.Empty;
            }
        }

        private void uiCurveEditor_CurveClicked(object sender, EventArgs e)
        {
            _eventLocked = true;

            if (uiCurveEditor.Document.SelectedCurves.Count > 0)
            {
                switch (uiCurveEditor.Document.Content.Items[uiCurveEditor.Document.SelectedCurves[0]].Curve.Type)
                {
                    case libpixy.net.Controls.CurveEditor.CurveType.Constant:
                        uiInterpolation.SelectedIndex = 0;
                        break;

                    case libpixy.net.Controls.CurveEditor.CurveType.Linear:
                        uiInterpolation.SelectedIndex = 1;
                        break;

                    case libpixy.net.Controls.CurveEditor.CurveType.Bezeir:
                        uiInterpolation.SelectedIndex = 2;
                        break;
                }
            }

            _eventLocked = false;
        }

        private void uiCurveEditor_GraphChanged(object sender, EventArgs e)
        {
            _logger.Trace("uiCurveEditor_GraphChanged");

            UpdateFCurve();
        }

        private void uiCurveEditor_IndicatorPositionChanged(object sender, EventArgs e)
        {
            _logger.Trace("uiCurveEditor>IndicatorPositionChanged");
            ////App.Doc.PlayCtrl.CurrentFrame = uiCurveEditor.IndicatorPosition.X;
        }

        private void uiCurveEditor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                float shift = (e.Modifiers == Keys.Shift) ? 10.0f : 1.0f;

                if (uiCurveEditor.Document.SelectedItems.Count > 0)
                {
                    foreach (int index in uiCurveEditor.Document.SelectedItems)
                    {
                        PointF value = uiCurveEditor.CanvasToValue(
                            uiCurveEditor.Document.Content.Items[index].Position.ToPointF());
                        value.X -= shift;
                        uiCurveEditor.Document.Content.Items[index].Position = new libpixy.net.Vecmath.Vector2(
                            uiCurveEditor.ValueToCanvas(value));
                    }

                    UpdateFCurve();
                    uiCurveEditor.Invalidate();
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                float shift = (e.Modifiers == Keys.Shift) ? 10.0f : 1.0f;

                if (uiCurveEditor.Document.SelectedItems.Count > 0)
                {
                    foreach (int index in uiCurveEditor.Document.SelectedItems)
                    {
                        PointF value = uiCurveEditor.CanvasToValue(
                            uiCurveEditor.Document.Content.Items[index].Position.ToPointF());
                        value.X += shift;
                        uiCurveEditor.Document.Content.Items[index].Position = new libpixy.net.Vecmath.Vector2(
                            uiCurveEditor.ValueToCanvas(value));
                    }

                    UpdateFCurve();
                    uiCurveEditor.Invalidate();
                }
            }
        }

        private void uiInterpolation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_eventLocked)
            {
                _logger.Trace("event locked.");
                return;
            }

            libpixy.net.Controls.CurveEditor.CurveType type = libpixy.net.Controls.CurveEditor.CurveType.Constant;
            switch (uiInterpolation.SelectedIndex)
            {
                case 0:
                    // Constant
                    break;

                case 1:
                    type = libpixy.net.Controls.CurveEditor.CurveType.Linear;
                    break;

                case 2:
                    type = libpixy.net.Controls.CurveEditor.CurveType.Bezeir;
                    break;
            }

            foreach (int index in uiCurveEditor.Document.SelectedCurves)
            {
                uiCurveEditor.Document.Content.Items[index].Curve.Type = type;
            }

            UpdateFCurve();
        }

        private void uiExtrapolation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_eventLocked)
            {
                _logger.Trace("event locked.");
                return;
            }

            if (_curve != null)
            {
                _curve.Extrapolation = (libpixy.net.Animation.FCurveExtrapolationType)uiExtrapolation.SelectedIndex;
                UpdateGraph();
            }
        }

        private void uiNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_eventLocked)
            {
                _logger.Trace("event locked.");
                return;
            }

            ResetNodeParamList();
        }

        private void uiParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_eventLocked)
            {
                _logger.Trace("event locked.");
                return;
            }

            string paramName = GetCurrentParamName();
            if (string.IsNullOrEmpty(paramName))
            {
                return;
            }

            TestNode node = GetCurrentNode();
            if (node == null)
            {
                return;
            }

            BeginEditCurve(node.UUID, paramName);
        }

        private void uiShowParam_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToggleNodeParam(e.ClickedItem.Text);
        }

        private void uiAddKey_Click(object sender, EventArgs e)
        {
            int keyFrame = 0;
            float keyValue = 0.0f;
            if (int.TryParse(uiKeyFrame.Text, out keyFrame) && float.TryParse(uiKeyValue.Text, out keyValue))
            {
                libpixy.net.Controls.CurveEditor.ControlPoint cp = new libpixy.net.Controls.CurveEditor.ControlPoint();
                Point pt = uiCurveEditor.ValueToCanvas(new PointF((float)keyFrame, keyValue));
                cp.Position = new libpixy.net.Vecmath.Vector2(pt.X, pt.Y);
                uiCurveEditor.Document.Add(cp);
                uiCurveEditor.Document.Sort();
                uiCurveEditor.Invalidate();
            }
        }

        private void uiUndo_Click(object sender, EventArgs e)
        {
            _curve.Items.Clear();

            foreach (libpixy.net.Animation.FCurveKey key in _curveBackup.Items)
            {
                _curve.Items.Add((libpixy.net.Animation.FCurveKey)key.Clone());
            }

            _curve.Extrapolation = _curveBackup.Extrapolation;

            UpdateGraph();
        }

        private void uiFrameCanvas_Click(object sender, EventArgs e)
        {
            uiCurveEditor.FrameCanvas();
        }

        private void uiCurveEditor_CurveDescriptionClicked(object sender, string name)
        {
            if (_scriptName != name)
            {
                _resetGraphRuler = false;
                BeginEditCurve(_uuid, name);
                _resetGraphRuler = true;
            }
        }

        private void uiLinkParam_Click(object sender, EventArgs e)
        {
            uiCurveEditor.EditSubContents = uiLinkParam.Checked;
        }

        private void uiKeyFrame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (uiCurveEditor.Document.SelectedItems.Count == 1)
                {
                    int keyFrame = 0;
                    if (int.TryParse(uiKeyFrame.Text, out keyFrame))
                    {
                        int index = uiCurveEditor.Document.SelectedItems[0];
                        libpixy.net.Vecmath.Vector2 pos = uiCurveEditor.Document.Content.Items[index].Position;
                        PointF value = uiCurveEditor.CanvasToValue(pos.ToPointF());
                        value.X = (float)keyFrame;
                        libpixy.net.Vecmath.Vector2 newpos = new libpixy.net.Vecmath.Vector2(uiCurveEditor.ValueToCanvas(value));

                        if (uiLinkParam.Checked)
                        {
                            foreach (libpixy.net.Controls.CurveEditor.CurveContent content in uiCurveEditor.Document.SubContents)
                            {
                                foreach (libpixy.net.Controls.CurveEditor.ControlPoint cp in content.Items)
                                {
                                    if (cp.Position.X == pos.X)
                                    {
                                        cp.Position = newpos;
                                        break;
                                    }
                                }
                            }
                        }

                        uiCurveEditor.Document.Content.Items[index].Position = newpos;
                        uiCurveEditor.Invalidate();
                        UpdateFCurve();
                    }
                }
            }
        }

        private void uiKeyValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (uiCurveEditor.Document.SelectedItems.Count > 0)
                {
                    float keyValue = 0.0f;
                    if (float.TryParse(uiKeyValue.Text, out keyValue))
                    {
                        foreach (int index in uiCurveEditor.Document.SelectedItems)
                        {
                            libpixy.net.Vecmath.Vector2 pos = uiCurveEditor.Document.Content.Items[index].Position;
                            PointF value = uiCurveEditor.CanvasToValue(pos.ToPointF());
                            value.Y = (float)keyValue;
                            uiCurveEditor.Document.Content.Items[index].Position = new libpixy.net.Vecmath.Vector2(uiCurveEditor.ValueToCanvas(value));
                        }

                        uiCurveEditor.Invalidate();
                        UpdateFCurve();
                    }
                }
            }
        }

        private void uiUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int value;
                if (int.TryParse(uiUnit.Text, out value))
                {
                    if (value > 0)
                    {
                        uiCurveEditor.YUnit = (float)value;
                        uiCurveEditor.YFormat = "0";
                        ResetGraph();
                    }
                }
                else
                {
                    float fvalue;
                    if (float.TryParse(uiUnit.Text, out fvalue))
                    {
                        if (fvalue > 0.0f)
                        {
                            uiCurveEditor.YUnit = fvalue;
                            if (fvalue < 0.0001f)
                            {
                                uiCurveEditor.YFormat = "0.00000";
                            }
                            else if (fvalue < 0.001f)
                            {
                                uiCurveEditor.YFormat = "0.0000";
                            }
                            else if (fvalue < 0.01f)
                            {
                                uiCurveEditor.YFormat = "0.000";
                            }
                            else if (fvalue < 0.1f)
                            {
                                uiCurveEditor.YFormat = "0.00";
                            }
                            else if (fvalue < 1.0f)
                            {
                                uiCurveEditor.YFormat = "0.0";
                            }

                            ResetGraph();
                        }
                    }
                }
            }
        }

        private void uiTickLength_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int value;
                if (int.TryParse(uiTickLength.Text, out value))
                {
                    if (value > 0)
                    {
                        uiCurveEditor.YTickLength = value;
                        ResetGraph();
                    }
                }
            }
        }

        private void uiTick_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int value;
                if (int.TryParse(uiTick.Text, out value))
                {
                    if (value > 0)
                    {
                        uiCurveEditor.YTick = value;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this._currentFrame++;
            if (this._currentFrame > 100)
            {
                this._currentFrame = 0;
            }

            uiCurveEditor.IndicatorPosition = new PointF(this._currentFrame, 0.0f);
        }
    }
}
