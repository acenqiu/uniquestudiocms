using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    public class RoleCollection:List<RoleInfo>
    {
        public RoleCollection()
            : base()
        {
        }

        public RoleCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
