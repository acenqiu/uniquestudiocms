using System;
using System.Collections.Generic;
using System.Resources;
using System.Reflection;
using System.Text;

namespace UniqueStudio.Common.Resource
{
    /// <summary>
    /// 系统资源
    /// </summary>
    /// <remarks>当前该类处于测试状态</remarks>
    public class Res
    {
        private static ResourceManager resourceManager;

        private static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceManager, null))
                {
                    //获取用户设置
                    resourceManager = new ResourceManager("UniqueStudio.Common.Resource.Zh-cn",
                                                        Assembly.Load("UniqueStudio.Common"));
                }
                return resourceManager;
            }
        }

        /// <summary>
        /// 分类
        /// </summary>
        public static string Category
        {
            get
            {
                return ResourceManager.GetString("Category");
            }
        }
    }
}
