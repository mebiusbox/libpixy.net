namespace libpixy.net.Controls.Diagram
{
    partial class Canvas
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
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Canvas";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseMove);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseUp);
            this.ClientSizeChanged += new System.EventHandler(this.NodeTree_ClientSizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
