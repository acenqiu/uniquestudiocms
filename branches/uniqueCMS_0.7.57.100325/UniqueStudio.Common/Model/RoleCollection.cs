//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：角色的集合。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System.Collections.Generic;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 角色的集合。
    /// </summary>
    public class RoleCollection:List<RoleInfo>
    {
        /// <summary>
        /// 初始化<see cref="RoleCollection"/>类的实例。
        /// </summary>
        public RoleCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量<see cref="RoleCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public RoleCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
