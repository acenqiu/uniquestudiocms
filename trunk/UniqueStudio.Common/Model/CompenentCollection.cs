using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class CompenentCollection:List<CompenentInfo>
    {
        public CompenentCollection()
            : base()
        {
        }
    }
}
