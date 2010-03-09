using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 角色的集合
    /// </summary>
    public class RoleCollection:List<RoleInfo>
    {
        /// <summary>
        /// 初始化<see cref="RoleCollection"/>类的实例
        /// </summary>
        public RoleCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量<see cref="RoleCollection"/>类的实例
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public RoleCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
