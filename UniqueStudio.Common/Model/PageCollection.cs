using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    public class PageCollection:List<PageInfo>
    {
        public PageCollection()
            : base()
        {
        }

        public PageCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
