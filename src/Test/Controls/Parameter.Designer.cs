namespace Test.Controls
{
    partial class Parameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Parameter));
            this.uiLabel = new System.Windows.Forms.Label();
            this.uiLink = new libpixy.net.Controls.Standard.FrameButton();
            this.uiCurve = new libpixy.net.Controls.Standard.FrameButton();
            this.uiValuePanel = new libpixy.net.Controls.Standard.ImageStretchPanel();
            this.uiValue = new System.Windows.Forms.TextBox();
            this.uiValuePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiLabel
            // 
            this.uiLabel.AutoSize = true;
            this.uiLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiLabel.ForeColor = System.Drawing.Color.White;
            this.uiLabel.Location = new System.Drawing.Point(18, 0);
            this.uiLabel.Name = "uiLabel";
            this.uiLabel.Size = new System.Drawing.Size(36, 13);
            this.uiLabel.TabIndex = 434;
            this.uiLabel.Text = "Label:";
            // 
            // uiLink
            // 
            this.uiLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiLink.Checkable = true;
            this.uiLink.Checked = false;
            this.uiLink.CheckedImage = global::Test.Properties.Resources.stock_link_mini;
            this.uiLink.CheckedInnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.uiLink.CheckedOuterColor = System.Drawing.Color.Black;
            this.uiLink.Image = global::Test.Properties.Resources.stock_link_mini;
            this.uiLink.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.uiLink.Location = new System.Drawing.Point(90, 0);
            this.uiLink.Name = "uiLink";
            this.uiLink.OuterColor = System.Drawing.Color.Black;
            this.uiLink.Rounded = false;
            this.uiLink.Size = new System.Drawing.Size(16, 16);
            this.uiLink.TabIndex = 600;
            this.uiLink.TabStop = false;
            this.uiLink.Text = "bitmapButton170";
            this.uiLink.Visible = false;
            this.uiLink.Click += new System.EventHandler(this.uiLink_Click);
            // 
            // uiCurve
            // 
            this.uiCurve.Checkable = false;
            this.uiCurve.Checked = false;
            this.uiCurve.CheckedImage = ((System.Drawing.Image)(resources.GetObject("uiCurve.CheckedImage")));
            this.uiCurve.CheckedInnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.uiCurve.CheckedOuterColor = System.Drawing.Color.Black;
            this.uiCurve.Image = null;
            this.uiCurve.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.uiCurve.Location = new System.Drawing.Point(0, 0);
            this.uiCurve.Name = "uiCurve";
            this.uiCurve.OuterColor = System.Drawing.Color.Black;
            this.uiCurve.Rounded = false;
            this.uiCurve.Size = new System.Drawing.Size(16, 16);
            this.uiCurve.TabIndex = 438;
            this.uiCurve.TabStop = false;
            this.uiCurve.Text = "bitmapButton170";
            this.uiCurve.Click += new System.EventHandler(this.uiCurve_Click);
            // 
            // uiValuePanel
            // 
            this.uiValuePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiValuePanel.BackgroundImage = global::Test.Properties.Resources.bordered_textbox;
            this.uiValuePanel.Controls.Add(this.uiValue);
            this.uiValuePanel.ForeColor = System.Drawing.Color.White;
            this.uiValuePanel.Location = new System.Drawing.Point(110, 0);
            this.uiValuePanel.Name = "uiValuePanel";
            this.uiValuePanel.PartLeftWidth = 5;
            this.uiValuePanel.PartLowerHeight = 5;
            this.uiValuePanel.PartRightWidth = 5;
            this.uiValuePanel.PartUpperHeight = 5;
            this.uiValuePanel.Size = new System.Drawing.Size(70, 16);
            this.uiValuePanel.StretchMode = libpixy.net.Controls.ImageStretchTools.StretchModes.repeat;
            this.uiValuePanel.TabIndex = 433;
            // 
            // uiValue
            // 
            this.uiValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.uiValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.uiValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiValue.ForeColor = System.Drawing.Color.White;
            this.uiValue.Location = new System.Drawing.Point(3, 1);
            this.uiValue.Name = "uiValue";
            this.uiValue.Size = new System.Drawing.Size(66, 14);
            this.uiValue.TabIndex = 38;
            this.uiValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uiValue_KeyDown);
            this.uiValue.ModifiedChanged += new System.EventHandler(this.uiValue_ModifiedChanged);
            // 
            // Parameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.Controls.Add(this.uiLink);
            this.Controls.Add(this.uiCurve);
            this.Controls.Add(this.uiLabel);
            this.Controls.Add(this.uiValuePanel);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "Parameter";
            this.Size = new System.Drawing.Size(184, 16);
            this.Load += new System.EventHandler(this.Parameter_Load);
            this.uiValuePanel.ResumeLayout(false);
            this.uiValuePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private libpixy.net.Controls.Standard.FrameButton uiCurve;
        private System.Windows.Forms.Label uiLabel;
        private libpixy.net.Controls.Standard.ImageStretchPanel uiValuePanel;
        private System.Windows.Forms.TextBox uiValue;
        private libpixy.net.Controls.Standard.FrameButton uiLink;
    }
}
