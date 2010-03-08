using System;
using System.Collections.Generic;
using System.Resources;
using System.Reflection;
using System.Text;

namespace UniqueStudio.Common.Resource
{
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

        public static string Category
        {
            get
            {
                return ResourceManager.GetString("Category");
            }
        }
    }
}
