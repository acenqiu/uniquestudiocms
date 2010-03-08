using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class MenuCollection:List<MenuInfo>
    {
        public MenuCollection()
            : base()
        {
        }

        public MenuCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
