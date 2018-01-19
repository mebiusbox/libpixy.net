namespace Test.Windows
{
    partial class AnimationWindow
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationWindow));
            libpixy.net.Controls.CurveEditor.Document document1 = new libpixy.net.Controls.CurveEditor.Document();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.uiKeyFrame = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.uiKeyValue = new System.Windows.Forms.ToolStripTextBox();
            this.uiAddKey = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.uiUnit = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel11 = new System.Windows.Forms.ToolStripLabel();
            this.uiTickLength = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel12 = new System.Windows.Forms.ToolStripLabel();
            this.uiTick = new System.Windows.Forms.ToolStripTextBox();
            this.uiCurveEditor = new libpixy.net.Controls.CurveEditor.CurveEditor();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.uiUndo = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.uiNode = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.uiParam = new System.Windows.Forms.ToolStripComboBox();
            this.uiRefreshParam = new System.Windows.Forms.ToolStripButton();
            this.uiShowParam = new System.Windows.Forms.ToolStripSplitButton();
            this.uiFrameCanvas = new System.Windows.Forms.ToolStripButton();
            this.uiLinkParam = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.uiInterpolation = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.uiExtrapolation = new System.Windows.Forms.ToolStripComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.uiCurveEditor);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1018, 292);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1018, 342);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.uiKeyFrame,
            this.toolStripLabel4,
            this.uiKeyValue,
            this.uiAddKey,
            this.toolStripLabel7,
            this.uiUnit,
            this.toolStripLabel11,
            this.uiTickLength,
            this.toolStripLabel12,
            this.uiTick});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1018, 25);
            this.toolStrip2.Stretch = true;
            this.toolStrip2.TabIndex = 0;
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel3.Text = "frame";
            // 
            // uiKeyFrame
            // 
            this.uiKeyFrame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiKeyFrame.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiKeyFrame.ForeColor = System.Drawing.Color.White;
            this.uiKeyFrame.Name = "uiKeyFrame";
            this.uiKeyFrame.Size = new System.Drawing.Size(50, 25);
            this.uiKeyFrame.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uiKeyFrame_KeyDown);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(35, 22);
            this.toolStripLabel4.Text = "value";
            // 
            // uiKeyValue
            // 
            this.uiKeyValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiKeyValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiKeyValue.ForeColor = System.Drawing.Color.White;
            this.uiKeyValue.Name = "uiKeyValue";
            this.uiKeyValue.Size = new System.Drawing.Size(100, 25);
            this.uiKeyValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uiKeyValue_KeyDown);
            // 
            // uiAddKey
            // 
            this.uiAddKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.uiAddKey.Image = global::Test.Properties.Resources.add;
            this.uiAddKey.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiAddKey.Name = "uiAddKey";
            this.uiAddKey.Size = new System.Drawing.Size(23, 22);
            this.uiAddKey.Text = "toolStripButton5";
            this.uiAddKey.Click += new System.EventHandler(this.uiAddKey_Click);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel7.Text = "単位";
            // 
            // uiUnit
            // 
            this.uiUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiUnit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiUnit.ForeColor = System.Drawing.Color.White;
            this.uiUnit.Name = "uiUnit";
            this.uiUnit.Size = new System.Drawing.Size(50, 25);
            this.uiUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uiUnit_KeyDown);
            // 
            // toolStripLabel11
            // 
            this.toolStripLabel11.Name = "toolStripLabel11";
            this.toolStripLabel11.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel11.Text = "間隔";
            // 
            // uiTickLength
            // 
            this.uiTickLength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiTickLength.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiTickLength.ForeColor = System.Drawing.Color.White;
            this.uiTickLength.Name = "uiTickLength";
            this.uiTickLength.Size = new System.Drawing.Size(50, 25);
            this.uiTickLength.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uiTickLength_KeyDown);
            // 
            // toolStripLabel12
            // 
            this.toolStripLabel12.Name = "toolStripLabel12";
            this.toolStripLabel12.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel12.Text = "目盛";
            // 
            // uiTick
            // 
            this.uiTick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiTick.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiTick.ForeColor = System.Drawing.Color.White;
            this.uiTick.Name = "uiTick";
            this.uiTick.Size = new System.Drawing.Size(50, 25);
            this.uiTick.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uiTick_KeyDown);
            // 
            // uiCurveEditor
            // 
            this.uiCurveEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.uiCurveEditor.CanvasOrigin = new System.Drawing.Point(0, 0);
            this.uiCurveEditor.CanvasScale = ((System.Drawing.PointF)(resources.GetObject("uiCurveEditor.CanvasScale")));
            this.uiCurveEditor.CurveDescColorH = 16;
            this.uiCurveEditor.CurveDescColorW = 30;
            this.uiCurveEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCurveEditor.Document = document1;
            this.uiCurveEditor.ExtrapolationColor = System.Drawing.Color.Gray;
            this.uiCurveEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiCurveEditor.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.uiCurveEditor.IndicatorColor = System.Drawing.Color.Red;
            this.uiCurveEditor.IndicatorPosition = ((System.Drawing.PointF)(resources.GetObject("uiCurveEditor.IndicatorPosition")));
            this.uiCurveEditor.Location = new System.Drawing.Point(0, 0);
            this.uiCurveEditor.Name = "uiCurveEditor";
            this.uiCurveEditor.ShowExtrapolation = true;
            this.uiCurveEditor.ShowIndicatorX = true;
            this.uiCurveEditor.ShowIndicatorY = false;
            this.uiCurveEditor.Size = new System.Drawing.Size(1018, 292);
            this.uiCurveEditor.TabIndex = 0;
            this.uiCurveEditor.TickColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.uiCurveEditor.XFormat = "0";
            this.uiCurveEditor.XOffset = 0F;
            this.uiCurveEditor.XSnap = true;
            this.uiCurveEditor.XStart = 0;
            this.uiCurveEditor.XTick = 5;
            this.uiCurveEditor.XTickLength = 10;
            this.uiCurveEditor.XTickWidth = 5;
            this.uiCurveEditor.XUnit = 1F;
            this.uiCurveEditor.XWidth = 30;
            this.uiCurveEditor.YFormat = "0";
            this.uiCurveEditor.YOffset = 0F;
            this.uiCurveEditor.YSnap = false;
            this.uiCurveEditor.YStart = 0;
            this.uiCurveEditor.YTick = 1;
            this.uiCurveEditor.YTickLength = 10;
            this.uiCurveEditor.YTickWidth = 5;
            this.uiCurveEditor.YUnit = 1F;
            this.uiCurveEditor.YWidth = 80;
            this.uiCurveEditor.CanvasMouseDown += new System.Windows.Forms.MouseEventHandler(this.uiCurveEditor_CanvasMouseDown);
            this.uiCurveEditor.CurveClicked += new System.EventHandler(this.uiCurveEditor_CurveClicked);
            this.uiCurveEditor.ControlPointSelected += new System.EventHandler(this.uiCurveEditor_ControlPointSelected);
            this.uiCurveEditor.ControlPointChanged += new System.EventHandler(this.uiCurveEditor_ControlPointChanged);
            this.uiCurveEditor.GraphChanged += new System.EventHandler(this.uiCurveEditor_GraphChanged);
            this.uiCurveEditor.IndicatorPositionChanged += new System.EventHandler(this.uiCurveEditor_IndicatorPositionChanged);
            this.uiCurveEditor.CurveDescriptionClicked += new libpixy.net.Controls.CurveEditor.CurveEditor.CurveDescriptionClickedEvent(this.uiCurveEditor_CurveDescriptionClicked);
            this.uiCurveEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AnimationWindow_KeyDown);
            this.uiCurveEditor.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.uiCurveEditor_PreviewKeyDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uiUndo,
            this.toolStripLabel1,
            this.uiNode,
            this.toolStripLabel2,
            this.uiParam,
            this.uiRefreshParam,
            this.uiShowParam,
            this.uiFrameCanvas,
            this.uiLinkParam,
            this.toolStripLabel8,
            this.uiInterpolation,
            this.toolStripLabel9,
            this.uiExtrapolation});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1018, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // uiUndo
            // 
            this.uiUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.uiUndo.Image = global::Test.Properties.Resources.arrow_return_180_left;
            this.uiUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiUndo.Name = "uiUndo";
            this.uiUndo.Size = new System.Drawing.Size(23, 22);
            this.uiUndo.Text = "戻す";
            this.uiUndo.Click += new System.EventHandler(this.uiUndo_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel1.Text = "node";
            // 
            // uiNode
            // 
            this.uiNode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiNode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiNode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiNode.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.uiNode.ForeColor = System.Drawing.Color.White;
            this.uiNode.Name = "uiNode";
            this.uiNode.Size = new System.Drawing.Size(200, 25);
            this.uiNode.SelectedIndexChanged += new System.EventHandler(this.uiNode_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(40, 22);
            this.toolStripLabel2.Text = "param";
            // 
            // uiParam
            // 
            this.uiParam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiParam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiParam.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.uiParam.ForeColor = System.Drawing.Color.White;
            this.uiParam.Name = "uiParam";
            this.uiParam.Size = new System.Drawing.Size(150, 25);
            this.uiParam.SelectedIndexChanged += new System.EventHandler(this.uiParam_SelectedIndexChanged);
            // 
            // uiRefreshParam
            // 
            this.uiRefreshParam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.uiRefreshParam.Image = global::Test.Properties.Resources.arrow_circle_double;
            this.uiRefreshParam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiRefreshParam.Name = "uiRefreshParam";
            this.uiRefreshParam.Size = new System.Drawing.Size(23, 22);
            this.uiRefreshParam.Text = "更新";
            this.uiRefreshParam.Click += new System.EventHandler(this.uiRefreshParam_Click);
            // 
            // uiShowParam
            // 
            this.uiShowParam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiShowParam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiShowParam.Name = "uiShowParam";
            this.uiShowParam.Size = new System.Drawing.Size(47, 22);
            this.uiShowParam.Text = "対象";
            this.uiShowParam.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.uiShowParam_DropDownItemClicked);
            // 
            // uiFrameCanvas
            // 
            this.uiFrameCanvas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.uiFrameCanvas.Image = global::Test.Properties.Resources.draw_wave;
            this.uiFrameCanvas.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiFrameCanvas.Name = "uiFrameCanvas";
            this.uiFrameCanvas.Size = new System.Drawing.Size(23, 22);
            this.uiFrameCanvas.Text = "toolStripButton1";
            this.uiFrameCanvas.ToolTipText = "Fit";
            this.uiFrameCanvas.Click += new System.EventHandler(this.uiFrameCanvas_Click);
            // 
            // uiLinkParam
            // 
            this.uiLinkParam.CheckOnClick = true;
            this.uiLinkParam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.uiLinkParam.Image = global::Test.Properties.Resources.chain;
            this.uiLinkParam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiLinkParam.Name = "uiLinkParam";
            this.uiLinkParam.Size = new System.Drawing.Size(23, 22);
            this.uiLinkParam.Text = "toolStripButton1";
            this.uiLinkParam.ToolTipText = "まとめて編集";
            this.uiLinkParam.Click += new System.EventHandler(this.uiLinkParam_Click);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel8.Text = "補間";
            // 
            // uiInterpolation
            // 
            this.uiInterpolation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiInterpolation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiInterpolation.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.uiInterpolation.ForeColor = System.Drawing.Color.White;
            this.uiInterpolation.Items.AddRange(new object[] {
            "Constant",
            "Linear",
            "Bezeir"});
            this.uiInterpolation.Name = "uiInterpolation";
            this.uiInterpolation.Size = new System.Drawing.Size(121, 25);
            this.uiInterpolation.SelectedIndexChanged += new System.EventHandler(this.uiInterpolation_SelectedIndexChanged);
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel9.Text = "外挿";
            // 
            // uiExtrapolation
            // 
            this.uiExtrapolation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.uiExtrapolation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiExtrapolation.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.uiExtrapolation.ForeColor = System.Drawing.Color.White;
            this.uiExtrapolation.Items.AddRange(new object[] {
            "Constant",
            "Cycle",
            "CycleOffset"});
            this.uiExtrapolation.Name = "uiExtrapolation";
            this.uiExtrapolation.Size = new System.Drawing.Size(121, 25);
            this.uiExtrapolation.SelectedIndexChanged += new System.EventHandler(this.uiExtrapolation_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // AnimationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 342);
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "AnimationWindow";
            this.Text = "Animation Editor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AnimationWindow_KeyDown);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox uiKeyFrame;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox uiKeyValue;
        private System.Windows.Forms.ToolStripButton uiAddKey;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripTextBox uiUnit;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox uiNode;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox uiParam;
        private System.Windows.Forms.ToolStripSplitButton uiShowParam;
        private libpixy.net.Controls.CurveEditor.CurveEditor uiCurveEditor;
        private System.Windows.Forms.ToolStripButton uiRefreshParam;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripComboBox uiInterpolation;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        private System.Windows.Forms.ToolStripComboBox uiExtrapolation;
        private System.Windows.Forms.ToolStripButton uiUndo;
        private System.Windows.Forms.ToolStripLabel toolStripLabel11;
        private System.Windows.Forms.ToolStripTextBox uiTickLength;
        private System.Windows.Forms.ToolStripLabel toolStripLabel12;
        private System.Windows.Forms.ToolStripTextBox uiTick;
        private System.Windows.Forms.ToolStripButton uiFrameCanvas;
        private System.Windows.Forms.ToolStripButton uiLinkParam;
        private System.Windows.Forms.Timer timer1;
    }
}
