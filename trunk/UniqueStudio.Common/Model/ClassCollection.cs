using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class ClassCollection:List<ClassInfo>
    {
        public ClassCollection()
            : base()
        {
        }

        public ClassCollection(int capacity)
            :base(capacity)
        {
        }
    }
}
