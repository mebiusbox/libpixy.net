﻿namespace libpixy.net.Controls.Standard
{
    partial class FrameButton
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
            // BitmapButton
            // 
            this.MouseLeave += new System.EventHandler(this.BitmapButton_MouseLeave);
            this.Click += new System.EventHandler(this.BitmapButton_Click);
            this.MouseEnter += new System.EventHandler(this.BitmapButton_MouseEnter);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
