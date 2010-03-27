using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.ComContent.Model
{
    /// <summary>
    /// 附件集合
    /// </summary>
    [Serializable]
    public class EnclosureCollection : List<Enclosure>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EnclosureCollection()
            : base()
        {
        }
    }
}
