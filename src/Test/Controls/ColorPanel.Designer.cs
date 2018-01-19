namespace Test.Controls
{
    partial class ColorPanel
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
            this.colorLabel4 = new System.Windows.Forms.Label();
            this.colorLabel3 = new System.Windows.Forms.Label();
            this.colorLabel2 = new System.Windows.Forms.Label();
            this.colorLabel1 = new System.Windows.Forms.Label();
            this.colorSpaceButton = new Glass.GlassButton();
            this.colorPictureBox = new System.Windows.Forms.PictureBox();
            this.colorSlider4 = new libpixy.net.Controls.Standard.Slider();
            this.colorTextBox4 = new System.Windows.Forms.TextBox();
            this.colorSlider3 = new libpixy.net.Controls.Standard.Slider();
            this.colorTextBox3 = new System.Windows.Forms.TextBox();
            this.colorSlider2 = new libpixy.net.Controls.Standard.Slider();
            this.colorTextBox2 = new System.Windows.Forms.TextBox();
            this.colorTextBox1 = new System.Windows.Forms.TextBox();
            this.colorSlider1 = new libpixy.net.Controls.Standard.Slider();
            ((System.ComponentModel.ISupportInitialize)(this.colorPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // colorLabel4
            // 
            this.colorLabel4.AutoSize = true;
            this.colorLabel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorLabel4.ForeColor = System.Drawing.Color.White;
            this.colorLabel4.Location = new System.Drawing.Point(48, 50);
            this.colorLabel4.Name = "colorLabel4";
            this.colorLabel4.Size = new System.Drawing.Size(15, 13);
            this.colorLabel4.TabIndex = 21;
            this.colorLabel4.Text = "A";
            // 
            // colorLabel3
            // 
            this.colorLabel3.AutoSize = true;
            this.colorLabel3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorLabel3.ForeColor = System.Drawing.Color.White;
            this.colorLabel3.Location = new System.Drawing.Point(48, 34);
            this.colorLabel3.Name = "colorLabel3";
            this.colorLabel3.Size = new System.Drawing.Size(14, 13);
            this.colorLabel3.TabIndex = 22;
            this.colorLabel3.Text = "B";
            // 
            // colorLabel2
            // 
            this.colorLabel2.AutoSize = true;
            this.colorLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorLabel2.ForeColor = System.Drawing.Color.White;
            this.colorLabel2.Location = new System.Drawing.Point(48, 18);
            this.colorLabel2.Name = "colorLabel2";
            this.colorLabel2.Size = new System.Drawing.Size(15, 13);
            this.colorLabel2.TabIndex = 20;
            this.colorLabel2.Text = "G";
            // 
            // colorLabel1
            // 
            this.colorLabel1.AutoSize = true;
            this.colorLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorLabel1.ForeColor = System.Drawing.Color.White;
            this.colorLabel1.Location = new System.Drawing.Point(48, 3);
            this.colorLabel1.Name = "colorLabel1";
            this.colorLabel1.Size = new System.Drawing.Size(15, 13);
            this.colorLabel1.TabIndex = 19;
            this.colorLabel1.Text = "R";
            // 
            // colorSpaceButton
            // 
            this.colorSpaceButton.BackColor = System.Drawing.Color.DimGray;
            this.colorSpaceButton.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.colorSpaceButton.InnerBorderColor = System.Drawing.Color.DimGray;
            this.colorSpaceButton.Location = new System.Drawing.Point(4, 46);
            this.colorSpaceButton.Name = "colorSpaceButton";
            this.colorSpaceButton.OuterBorderColor = System.Drawing.Color.Black;
            this.colorSpaceButton.ShineColor = System.Drawing.Color.DimGray;
            this.colorSpaceButton.Size = new System.Drawing.Size(40, 18);
            this.colorSpaceButton.TabIndex = 18;
            this.colorSpaceButton.Text = "RGB";
            // 
            // colorPictureBox
            // 
            this.colorPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPictureBox.Location = new System.Drawing.Point(3, 3);
            this.colorPictureBox.Name = "colorPictureBox";
            this.colorPictureBox.Size = new System.Drawing.Size(41, 40);
            this.colorPictureBox.TabIndex = 17;
            this.colorPictureBox.TabStop = false;
            // 
            // colorSlider4
            // 
            this.colorSlider4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.colorSlider4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorSlider4.Edit = this.colorTextBox4;
            this.colorSlider4.ForeColor = System.Drawing.Color.Gray;
            this.colorSlider4.Location = new System.Drawing.Point(129, 49);
            this.colorSlider4.Lower = 0F;
            this.colorSlider4.Name = "colorSlider4";
            this.colorSlider4.Size = new System.Drawing.Size(100, 12);
            this.colorSlider4.TabIndex = 12;
            this.colorSlider4.Upper = 1F;
            this.colorSlider4.Value = 0.8F;
            // 
            // colorTextBox4
            // 
            this.colorTextBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.colorTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.colorTextBox4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorTextBox4.ForeColor = System.Drawing.Color.White;
            this.colorTextBox4.Location = new System.Drawing.Point(63, 48);
            this.colorTextBox4.Name = "colorTextBox4";
            this.colorTextBox4.Size = new System.Drawing.Size(60, 15);
            this.colorTextBox4.TabIndex = 14;
            // 
            // colorSlider3
            // 
            this.colorSlider3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.colorSlider3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorSlider3.Edit = this.colorTextBox3;
            this.colorSlider3.ForeColor = System.Drawing.Color.Gray;
            this.colorSlider3.Location = new System.Drawing.Point(129, 34);
            this.colorSlider3.Lower = 0F;
            this.colorSlider3.Name = "colorSlider3";
            this.colorSlider3.Size = new System.Drawing.Size(100, 12);
            this.colorSlider3.TabIndex = 9;
            this.colorSlider3.Upper = 1F;
            this.colorSlider3.Value = 0.8F;
            // 
            // colorTextBox3
            // 
            this.colorTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.colorTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.colorTextBox3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorTextBox3.ForeColor = System.Drawing.Color.White;
            this.colorTextBox3.Location = new System.Drawing.Point(63, 33);
            this.colorTextBox3.Name = "colorTextBox3";
            this.colorTextBox3.Size = new System.Drawing.Size(60, 15);
            this.colorTextBox3.TabIndex = 13;
            // 
            // colorSlider2
            // 
            this.colorSlider2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.colorSlider2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorSlider2.Edit = this.colorTextBox2;
            this.colorSlider2.ForeColor = System.Drawing.Color.Gray;
            this.colorSlider2.Location = new System.Drawing.Point(129, 19);
            this.colorSlider2.Lower = 0F;
            this.colorSlider2.Name = "colorSlider2";
            this.colorSlider2.Size = new System.Drawing.Size(100, 12);
            this.colorSlider2.TabIndex = 10;
            this.colorSlider2.Upper = 1F;
            this.colorSlider2.Value = 0.8F;
            // 
            // colorTextBox2
            // 
            this.colorTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.colorTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.colorTextBox2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorTextBox2.ForeColor = System.Drawing.Color.White;
            this.colorTextBox2.Location = new System.Drawing.Point(63, 18);
            this.colorTextBox2.Name = "colorTextBox2";
            this.colorTextBox2.Size = new System.Drawing.Size(60, 15);
            this.colorTextBox2.TabIndex = 16;
            // 
            // colorTextBox1
            // 
            this.colorTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.colorTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.colorTextBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorTextBox1.ForeColor = System.Drawing.Color.White;
            this.colorTextBox1.Location = new System.Drawing.Point(63, 3);
            this.colorTextBox1.Name = "colorTextBox1";
            this.colorTextBox1.Size = new System.Drawing.Size(60, 15);
            this.colorTextBox1.TabIndex = 15;
            // 
            // colorSlider1
            // 
            this.colorSlider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.colorSlider1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorSlider1.Edit = this.colorTextBox1;
            this.colorSlider1.ForeColor = System.Drawing.Color.Gray;
            this.colorSlider1.Location = new System.Drawing.Point(129, 4);
            this.colorSlider1.Lower = 0F;
            this.colorSlider1.Name = "colorSlider1";
            this.colorSlider1.Size = new System.Drawing.Size(100, 12);
            this.colorSlider1.TabIndex = 11;
            this.colorSlider1.Upper = 1F;
            this.colorSlider1.Value = 0.8F;
            // 
            // ColorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.colorLabel4);
            this.Controls.Add(this.colorLabel3);
            this.Controls.Add(this.colorLabel2);
            this.Controls.Add(this.colorLabel1);
            this.Controls.Add(this.colorSpaceButton);
            this.Controls.Add(this.colorPictureBox);
            this.Controls.Add(this.colorSlider4);
            this.Controls.Add(this.colorSlider3);
            this.Controls.Add(this.colorTextBox4);
            this.Controls.Add(this.colorSlider2);
            this.Controls.Add(this.colorTextBox3);
            this.Controls.Add(this.colorTextBox1);
            this.Controls.Add(this.colorSlider1);
            this.Controls.Add(this.colorTextBox2);
            this.Name = "ColorPanel";
            this.Size = new System.Drawing.Size(235, 68);
            ((System.ComponentModel.ISupportInitialize)(this.colorPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label colorLabel4;
        private System.Windows.Forms.Label colorLabel3;
        private System.Windows.Forms.Label colorLabel2;
        private System.Windows.Forms.Label colorLabel1;
        private Glass.GlassButton colorSpaceButton;
        private System.Windows.Forms.PictureBox colorPictureBox;
        private libpixy.net.Controls.Standard.Slider colorSlider4;
        private System.Windows.Forms.TextBox colorTextBox4;
        private libpixy.net.Controls.Standard.Slider colorSlider3;
        private System.Windows.Forms.TextBox colorTextBox3;
        private libpixy.net.Controls.Standard.Slider colorSlider2;
        private System.Windows.Forms.TextBox colorTextBox2;
        private System.Windows.Forms.TextBox colorTextBox1;
        private libpixy.net.Controls.Standard.Slider colorSlider1;
    }
}
