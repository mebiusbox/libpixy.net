namespace libpixy.net.Controls
{
    partial class ImageStretchButton
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
            if (disposing)
            {
                if (this.m_overBackgroundImage != null)
                {
                    this.m_overBackgroundImage.Dispose();
                    this.m_overBackgroundImage = null;
                }

                if (this.m_pressedBackgroundImage != null)
                {
                    this.m_pressedBackgroundImage.Dispose();
                    this.m_pressedBackgroundImage = null;
                }

                if (this.m_switchBackgroundImage != null)
                {
                    this.m_switchBackgroundImage.Dispose();
                    this.m_switchBackgroundImage = null;
                }

                if (this.m_switchOverBackgroundImage != null)
                {
                    this.m_switchOverBackgroundImage.Dispose();
                    this.m_switchOverBackgroundImage = null;
                }

                if (this.m_switchImage != null)
                {
                    this.m_switchImage.Dispose();
                    this.m_switchImage = null;
                }
            }
            
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
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}
