using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 权限的集合
    /// </summary>
    public class PermissionCollection:List<PermissionInfo>
	{
        /// <summary>
        /// 初始化<see cref="PermissionCollection"/>类的实例
        /// </summary>
        public PermissionCollection()
            :base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="PermissionCollection"/>类的实例
        /// </summary>
        /// <param name="capacity">集合容量</param>
        public PermissionCollection(int capacity)
            : base(capacity)
        {
        }
	}
}
