using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    public class PermissionCollection:List<PermissionInfo>
	{
        public PermissionCollection()
            :base()
        {
        }

        public PermissionCollection(int capacity)
            : base(capacity)
        {
        }
	}
}
