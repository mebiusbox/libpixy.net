using System;
using System.Collections.Generic;
using System.Text;

namespace libpixy.net.Controls.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class ToolboxEventArgs
    {
        public IToolboxItem Item;

        public ToolboxEventArgs()
        {
        }

        public ToolboxEventArgs(IToolboxItem item)
        {
            Item = item;
        }
    }
}
