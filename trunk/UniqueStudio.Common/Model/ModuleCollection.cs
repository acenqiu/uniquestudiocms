using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class ModuleCollection : List<ModuleInfo>
    {
        public ModuleCollection()
            : base()
        {
        }
    }
}
