//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：权限的集合。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System.Collections.Generic;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 权限的集合。
    /// </summary>
    public class PermissionCollection : List<PermissionInfo>
    {
        /// <summary>
        /// 初始化<see cref="PermissionCollection"/>类的实例。
        /// </summary>
        public PermissionCollection()
            : base()
        {

        }

        /// <summary>
        /// 以集合容量初始化<see cref="PermissionCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public PermissionCollection(int capacity)
            : base(capacity)
        {

        }
    }
}
