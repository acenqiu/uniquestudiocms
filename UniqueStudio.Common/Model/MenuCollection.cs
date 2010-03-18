//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：菜单的集合。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 菜单的集合。
    /// </summary>
    [Serializable]
    public class MenuCollection:List<MenuInfo>
    {
        /// <summary>
        /// 初始化<see cref="MenuCollection"/>类的实例。
        /// </summary>
        public MenuCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="MenuCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public MenuCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
