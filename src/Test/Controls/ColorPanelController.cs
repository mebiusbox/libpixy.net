using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Test.Controls
{
    /// <summary>
    /// カラーパネルコントローラ
    /// </summary>
    public class ColorPanelController
    {
        public enum ColorSpace
        {
            RGB,
            HLS,
            HSV
        };

        private ColorSpace m_colorSpace = ColorSpace.RGB;
        private ColorDialog m_colorDialog = new ColorDialog();

        #region EventHandler

        public event EventHandler<EventArgs> ValueChanged;

        /// <summary>
        /// 
        /// </summary>
        public void RaiseValueChanged()
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }

        #endregion EventHandler

        #region Fields

        private System.Windows.Forms.PictureBox m_pictureBox = null;
        private Glass.GlassButton m_button = null;
        private System.Windows.Forms.TextBox m_textBox1 = null;
        private System.Windows.Forms.TextBox m_textBox2 = null;
        private System.Windows.Forms.TextBox m_textBox3 = null;
        private System.Windows.Forms.TextBox m_textBox4 = null;
        private System.Windows.Forms.Label m_label1 = null;
        private System.Windows.Forms.Label m_label2 = null;
        private System.Windows.Forms.Label m_label3 = null;
        private libpixy.net.Controls.Standard.Slider m_slider1 = null;
        private libpixy.net.Controls.Standard.Slider m_slider2 = null;
        private libpixy.net.Controls.Standard.Slider m_slider3 = null;
        private libpixy.net.Controls.Standard.Slider m_slider4 = null;

        #endregion Fields

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="button"></param>
        /// <param name="textBox1"></param>
        /// <param name="textBox2"></param>
        /// <param name="textBox3"></param>
        /// <param name="label1"></param>
        /// <param name="label2"></param>
        /// <param name="label3"></param>
        /// <param name="slider1"></param>
        /// <param name="slider2"></param>
        /// <param name="slider3"></param>
        public ColorPanelController(
            System.Windows.Forms.PictureBox pictureBox,
            Glass.GlassButton button,
            System.Windows.Forms.TextBox textBox1,
            System.Windows.Forms.TextBox textBox2,
            System.Windows.Forms.TextBox textBox3,
            System.Windows.Forms.Label label1,
            System.Windows.Forms.Label label2,
            System.Windows.Forms.Label label3,
            libpixy.net.Controls.Standard.Slider slider1,
            libpixy.net.Controls.Standard.Slider slider2,
            libpixy.net.Controls.Standard.Slider slider3)
        {
            m_pictureBox = pictureBox;
            m_button = button;
            m_textBox1 = textBox1;
            m_textBox2 = textBox2;
            m_textBox3 = textBox3;
            m_label1 = label1;
            m_label2 = label2;
            m_label3 = label3;
            m_slider1 = slider1;
            m_slider2 = slider2;
            m_slider3 = slider3;

            m_pictureBox.Click += new EventHandler(pictureBox1_Click);
            m_button.Click += new EventHandler(button1_Click);
            m_slider1.ValueChanged += new EventHandler<EventArgs>(slider1_ValueChanged);
            m_slider2.ValueChanged += new EventHandler<EventArgs>(slider2_ValueChanged);
            m_slider3.ValueChanged += new EventHandler<EventArgs>(slider3_ValueChanged);
            m_textBox1.TextChanged += new EventHandler(m_textBox1_TextChanged);
            m_textBox2.TextChanged += new EventHandler(m_textBox2_TextChanged);
            m_textBox3.TextChanged += new EventHandler(m_textBox3_TextChanged);
            UpdateLabels();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="button"></param>
        /// <param name="textBox1"></param>
        /// <param name="textBox2"></param>
        /// <param name="textBox3"></param>
        /// <param name="textBox4"></paravbn>
        /// <param name="label1"></param>
        /// <param name="label2"></param>
        /// <param name="label3"></param>
        /// <param name="slider1"></param>
        /// <param name="slider2"></param>
        /// <param name="slider3"></param>
        /// <param name="slider4"></param>
        public ColorPanelController(
            System.Windows.Forms.PictureBox pictureBox,
            Glass.GlassButton button,
            System.Windows.Forms.TextBox textBox1,
            System.Windows.Forms.TextBox textBox2,
            System.Windows.Forms.TextBox textBox3,
            System.Windows.Forms.TextBox textBox4,
            System.Windows.Forms.Label label1,
            System.Windows.Forms.Label label2,
            System.Windows.Forms.Label label3,
            libpixy.net.Controls.Standard.Slider slider1,
            libpixy.net.Controls.Standard.Slider slider2,
            libpixy.net.Controls.Standard.Slider slider3,
            libpixy.net.Controls.Standard.Slider slider4)
        {
            m_pictureBox = pictureBox;
            m_button = button;
            m_textBox1 = textBox1;
            m_textBox2 = textBox2;
            m_textBox3 = textBox3;
            m_textBox4 = textBox4;
            m_label1 = label1;
            m_label2 = label2;
            m_label3 = label3;
            m_slider1 = slider1;
            m_slider2 = slider2;
            m_slider3 = slider3;
            m_slider4 = slider4;

            m_pictureBox.Click += new EventHandler(pictureBox1_Click);
            m_button.Click += new EventHandler(button1_Click);
            m_slider1.ValueChanged += new EventHandler<EventArgs>(slider1_ValueChanged);
            m_slider2.ValueChanged += new EventHandler<EventArgs>(slider2_ValueChanged);
            m_slider3.ValueChanged += new EventHandler<EventArgs>(slider3_ValueChanged);
            m_slider4.ValueChanged += new EventHandler<EventArgs>(slider4_ValueChanged);
            m_slider4.UpdateText();
            m_textBox1.TextChanged += new EventHandler(m_textBox1_TextChanged);
            m_textBox2.TextChanged += new EventHandler(m_textBox2_TextChanged);
            m_textBox3.TextChanged += new EventHandler(m_textBox3_TextChanged);
            m_textBox4.TextChanged += new EventHandler(m_textBox4_TextChanged);
            UpdateLabels();
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public float Red
        {
            get { return m_slider1.Value; }
            set
            {
                m_slider1.Value = value;
                UpdatePictureBoxFromSlider();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Green
        {
            get { return m_slider2.Value; }
            set
            { 
                m_slider2.Value = value;
                UpdatePictureBoxFromSlider();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Blue
        {
            get { return m_slider3.Value; }
            set 
            {
                m_slider3.Value = value;
                UpdatePictureBoxFromSlider();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Alpha
        {
            get { return m_slider4.Value; }
            set
            { 
                m_slider4.Value = value;
                UpdatePictureBoxFromSlider();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Red255
        {
            get { return m_pictureBox.BackColor.R; }
            set
            {
                m_pictureBox.BackColor = Color.FromArgb(Alpha255, value, Green255, Blue255);
                UpdateSliderFromPictureBox();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Green255
        {
            get { return m_pictureBox.BackColor.G; }
            set
            {
                m_pictureBox.BackColor = Color.FromArgb(Alpha255, Red255, value, Blue255);
                UpdateSliderFromPictureBox();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Blue255
        {
            get { return m_pictureBox.BackColor.B; }
            set
            {
                m_pictureBox.BackColor = Color.FromArgb(Alpha255, Red255, Green255, value);
                UpdateSliderFromPictureBox();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Alpha255
        {
            get { return m_pictureBox.BackColor.A; }
            set 
            {
                m_pictureBox.BackColor = Color.FromArgb(value, Red255, Green255, Blue255);
                UpdateSliderFromPictureBox();
            }
        }

        #endregion Properties

        #region Attributes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void SetColor(float r, float g, float b)
        {
            m_slider1.Value = r;
            m_slider2.Value = g;
            m_slider3.Value = b;
            UpdatePictureBoxFromSlider();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void SetColor(byte r, byte g, byte b)
        {
            m_pictureBox.BackColor = Color.FromArgb(m_pictureBox.BackColor.A, r, g, b);
            UpdateSliderFromPictureBox();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void SetColor(float a, float r, float g, float b)
        {
            m_slider1.Value = r;
            m_slider2.Value = g;
            m_slider3.Value = b;

            if (m_slider4 != null)
            {
                m_slider4.Value = a;
            }

            UpdatePictureBoxFromSlider();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void SetColor(byte a, byte r, byte g, byte b)
        {
            m_pictureBox.BackColor = Color.FromArgb(a, r, g, b);
            UpdateSliderFromPictureBox();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            m_pictureBox.BackColor = color;
            UpdateSliderFromPictureBox();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color GetColor()
        {
            return m_pictureBox.BackColor;
        }

        #endregion Attributes

        /// <summary>
        /// ピクチャボックスの背景色をスライダに反映
        /// </summary>
        public void UpdateSliderFromPictureBox()
        {
            switch (m_colorSpace)
            {
                case ColorSpace.RGB:
                    this.m_slider1.Value = (float)this.m_pictureBox.BackColor.R / 255.0f;
                    this.m_slider2.Value = (float)this.m_pictureBox.BackColor.G / 255.0f;
                    this.m_slider3.Value = (float)this.m_pictureBox.BackColor.B / 255.0f;
                    break;

                case ColorSpace.HLS:
                    libpixy.net.Vecmath.ColorSpace.HSL hsl = libpixy.net.Vecmath.ColorSpace.RGBtoHSL(this.m_pictureBox.BackColor);
                    this.m_slider1.Value = (float)hsl.Hue / 360.0f;
                    this.m_slider2.Value = (float)hsl.Luminance;
                    this.m_slider3.Value = (float)hsl.Saturation;
                    break;

                case ColorSpace.HSV:
                    libpixy.net.Vecmath.ColorSpace.HSB hsb = libpixy.net.Vecmath.ColorSpace.RGBtoHSB(this.m_pictureBox.BackColor);
                    this.m_slider1.Value = (float)hsb.Hue / 360.0f;
                    this.m_slider2.Value = (float)hsb.Saturation;
                    this.m_slider3.Value = (float)hsb.Brightness;
                    break;
            }

            this.m_slider1.UpdateText();
            this.m_slider2.UpdateText();
            this.m_slider3.UpdateText();

            if (this.m_slider4 != null)
            {
                this.m_slider4.Value = (float)this.m_pictureBox.BackColor.A / 255.0f;
                this.m_slider4.UpdateText();
            }
        }

        /// <summary>
        /// ピクチャボックスの背景色をスライダから反映
        /// </summary>
        public void UpdatePictureBoxFromSlider()
        {
            switch (m_colorSpace)
            {
                case ColorSpace.RGB:
                    this.m_pictureBox.BackColor = Color.FromArgb((int)(255 * m_slider1.Value), (int)(255 * m_slider2.Value), (int)(255 * m_slider3.Value));
                    break;

                case ColorSpace.HLS:
                    this.m_pictureBox.BackColor = libpixy.net.Vecmath.ColorSpace.HSLtoColor(m_slider1.Value * 360.0f, m_slider3.Value, m_slider2.Value);
                    break;

                case ColorSpace.HSV:
                    this.m_pictureBox.BackColor = libpixy.net.Vecmath.ColorSpace.HSBtoColor(m_slider1.Value * 360.0f, m_slider2.Value, m_slider3.Value);
                    break;
            }

            if (m_slider4 != null)
            {
                this.m_pictureBox.BackColor = Color.FromArgb(
                    (int)(255 * m_slider4.Value),
                    this.m_pictureBox.BackColor.R,
                    this.m_pictureBox.BackColor.G,
                    this.m_pictureBox.BackColor.B);
            }
        }

        private void UpdateLabels()
        {
            switch (m_colorSpace)
            {
                case ColorSpace.RGB:
                    this.m_button.Text = "RGB";
                    this.m_label1.Text = "R";
                    this.m_label2.Text = "G";
                    this.m_label3.Text = "B";
                    break;

                case ColorSpace.HLS:
                    this.m_button.Text = "HLS";
                    this.m_label1.Text = "H";
                    this.m_label2.Text = "L";
                    this.m_label3.Text = "S";
                    break;

                case ColorSpace.HSV:
                    this.m_button.Text = "HSV";
                    this.m_label1.Text = "H";
                    this.m_label2.Text = "S";
                    this.m_label3.Text = "V";
                    break;
            }
        }

        private void slider1_ValueChanged(object sender, EventArgs e)
        {
            UpdatePictureBoxFromSlider();
            RaiseValueChanged();
        }

        private void slider2_ValueChanged(object sender, EventArgs e)
        {
            UpdatePictureBoxFromSlider();
            RaiseValueChanged();
        }

        private void slider3_ValueChanged(object sender, EventArgs e)
        {
            UpdatePictureBoxFromSlider();
            RaiseValueChanged();
        }

        private void slider4_ValueChanged(object sender, EventArgs e)
        {
            UpdatePictureBoxFromSlider();
            RaiseValueChanged();
        }


        void m_textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdatePictureBoxFromSlider();
        }

        void m_textBox2_TextChanged(object sender, EventArgs e)
        {
            UpdatePictureBoxFromSlider();
        }

        void m_textBox3_TextChanged(object sender, EventArgs e)
        {
            UpdatePictureBoxFromSlider();
        }

        void m_textBox4_TextChanged(object sender, EventArgs e)
        {
            UpdatePictureBoxFromSlider();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (m_colorSpace)
            {
                case ColorSpace.RGB:
                    m_colorSpace = ColorSpace.HLS;
                    break;

                case ColorSpace.HLS:
                    m_colorSpace = ColorSpace.HSV;
                    break;

                case ColorSpace.HSV:
                    m_colorSpace = ColorSpace.RGB;
                    break;
            }

            UpdateLabels();
            UpdateSliderFromPictureBox();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (me != null)
            {
                if (me.Button == MouseButtons.Right)
                {
                    if (m_colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.m_pictureBox.BackColor = Color.FromArgb(
                        this.m_pictureBox.BackColor.A,
                        m_colorDialog.Color.R,
                        m_colorDialog.Color.G,
                        m_colorDialog.Color.B);
                        UpdateSliderFromPictureBox();
                        RaiseValueChanged();
                    }

                    return;
                }
            }

            AdobeColorPicker.frmColorPicker colorPicker = new AdobeColorPicker.frmColorPicker(this.m_pictureBox.BackColor);
            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                this.m_pictureBox.BackColor = colorPicker.PrimaryColor;
                UpdateSliderFromPictureBox();
                RaiseValueChanged();
            }
        }
    }
}
