namespace libpixy.net.Controls.Toolbox
{
    partial class Toolbox
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
            // Toolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "Toolbox";
            this.MouseLeave += new System.EventHandler(this.Toolbox_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Toolbox_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Toolbox_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Toolbox_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Toolbox_MouseUp);
            this.MouseEnter += new System.EventHandler(this.Toolbox_MouseEnter);
            this.ClientSizeChanged += new System.EventHandler(this.Toolbox_ClientSizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
