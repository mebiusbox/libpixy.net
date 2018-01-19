using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace libpixy.net.Controls.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class ToolboxGroup : IToolboxItem
    {
        public Boolean Expand = true;
        public System.Collections.Generic.List<ToolboxButton> Buttons;


        public ToolboxGroup()
        {
            Buttons = new List<ToolboxButton>();
        }

        public ToolboxButton GetByName(String text)
        {
            foreach (ToolboxButton btn in Buttons)
            {
                if (btn.Text == text)
                {
                    return btn;
                }
            }

            return null;
        }

        public void ClearState()
        {
            Focus = false;
            Select = false;

            foreach (ToolboxButton btn in Buttons)
            {
                btn.Focus = false;
                btn.Select = false;
            }
        }

        public Rectangle GetExpandBoxRect()
        {
            Rectangle rcBox = new Rectangle();
            rcBox.X = Bounds.X + 3;
            rcBox.Y = Bounds.Y + Bounds.Height / 2 - 5;
            rcBox.Width = 10;
            rcBox.Height = 10;
            return rcBox;
        }

        public Rectangle GetLabelRect()
        {
            Rectangle rcLabel = new Rectangle();
            rcLabel.X = Bounds.X + 16;
            rcLabel.Y = Bounds.Y;
            rcLabel.Width = Bounds.Right - rcLabel.X;
            rcLabel.Height = Bounds.Bottom - rcLabel.Y;
            rcLabel.Inflate(-Toolbox.PAD_X, -Toolbox.PAD_Y);
            return rcLabel;
        }
    }
}
