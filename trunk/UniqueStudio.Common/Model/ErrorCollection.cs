using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class ErrorCollection:List<ErrorInfo>
    {
        public ErrorCollection()
            :base()
        {
        }

        public ErrorCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
