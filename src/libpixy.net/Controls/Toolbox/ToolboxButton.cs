using System;
using System.Collections.Generic;
using System.Text;

namespace libpixy.net.Controls.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class ToolboxButton : IToolboxItem
    {
        public ToolboxButton()
        {
        }

        public ToolboxButton(String text)
        {
            Text = text;
        }

        public ToolboxButton(String text, int imageIndex)
        {
            Text = text;
            ImageIndex = imageIndex;
        }
    }
}
