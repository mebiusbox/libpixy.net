namespace libpixy.net.Controls.CurveEditor
{
    partial class CurveEditor
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
            // CurveEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CurveEditor";
            this.MouseLeave += new System.EventHandler(this.CurveEditor_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CurveEditor_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CurveEditor_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CurveEditor_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CurveEditor_MouseUp);
            this.MouseEnter += new System.EventHandler(this.CurveEditor_MouseEnter);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
