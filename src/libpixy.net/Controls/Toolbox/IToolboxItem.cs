using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace libpixy.net.Controls.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class IToolboxItem
    {
        public Boolean m_focus = false;
        public Boolean m_select = false;
        public String m_text;
        public int m_imageIndex = -1;
        public Rectangle m_bounds;
        private Rectangle m_nameBounds = new Rectangle(-1, -1, -1, -1);

        public IToolboxItem()
        {
            m_bounds = new Rectangle();
        }

        #region Properties

        public Boolean Focus
        {
            get { return m_focus; }
            set { m_focus = value; }
        }

        public Boolean Select
        {
            get { return m_select; }
            set { m_select = value; }
        }

        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        public int ImageIndex
        {
            get { return m_imageIndex; }
            set { m_imageIndex = value; }
        }

        public Rectangle Bounds
        {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

        public Rectangle NameBounds
        {
            get { return m_nameBounds; }
            set { m_nameBounds = value; }
        }

        #endregion
    }
}
