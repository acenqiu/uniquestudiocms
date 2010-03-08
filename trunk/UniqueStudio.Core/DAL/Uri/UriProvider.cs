using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.DAL.Uri
{
    /// <summary>
    /// 提供管理Uri的相关方法
    /// </summary>
    public class UriProvider
    {
        public UriProvider()
        {
        }

        /// <summary>
        /// 获取Uri
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <returns>Uri</returns>
        public static Int64 GetNewUri(ResourceType type)
        {
            byte resourceType = (byte)type;
            string uriString = resourceType.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            return Convert.ToInt64(uriString);
        }

        public static bool IsThisUriExist(Int64 uri)
        {
            return true;
        }

        public static bool IsThisUriExist(Int64 uri, ResourceType type)
        {
            throw new NotImplementedException();
        }
    }
}
