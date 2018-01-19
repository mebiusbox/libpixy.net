namespace Test.Controls
{
    partial class ParameterBase
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
            this.uiMenuRemoveAllKey = new System.Windows.Forms.ToolStripMenuItem();
            this.uiMenuAnimationEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.uiContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uiContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiMenuRemoveAllKey
            // 
            this.uiMenuRemoveAllKey.Name = "uiMenuRemoveAllKey";
            this.uiMenuRemoveAllKey.Size = new System.Drawing.Size(183, 22);
            this.uiMenuRemoveAllKey.Text = "Remove All Key";
            // 
            // uiMenuAnimationEditor
            // 
            this.uiMenuAnimationEditor.Name = "uiMenuAnimationEditor";
            this.uiMenuAnimationEditor.Size = new System.Drawing.Size(183, 22);
            this.uiMenuAnimationEditor.Text = "Animation Editor...";
            // 
            // uiContextMenu
            // 
            this.uiContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uiMenuRemoveAllKey,
            this.uiMenuAnimationEditor});
            this.uiContextMenu.Name = "uiContextMenu";
            this.uiContextMenu.Size = new System.Drawing.Size(184, 48);
            this.uiContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.uiContextMenu_ItemClicked);
            // 
            // ParameterBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.uiContextMenu;
            this.DoubleBuffered = true;
            this.Name = "ParameterBase";
            this.Load += new System.EventHandler(this.ParameterBase_Load);
            this.uiContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem uiMenuRemoveAllKey;
        private System.Windows.Forms.ToolStripMenuItem uiMenuAnimationEditor;
        private System.Windows.Forms.ContextMenuStrip uiContextMenu;


    }
}
