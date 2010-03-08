using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class MenuItemCollection:List<MenuItemInfo>
    {
        public MenuItemCollection()
            : base()
        {
        }

        public MenuItemCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
