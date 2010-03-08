using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class ModuleControlCollection:List<ModuleControlInfo>
    {
        public ModuleControlCollection()
            : base()
        {
        }

        public ModuleControlCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
