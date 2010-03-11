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
        /// <summary>
        /// 初始化<see cref="UriProvider"/>类的实例
        /// </summary>
        public UriProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 返回Uri
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <returns>Uri</returns>
        public static Int64 GetNewUri(ResourceType type)
        {
            byte resourceType = (byte)type;
            string uriString = resourceType.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            return Convert.ToInt64(uriString);
        }

        /// <summary>
        /// 返回指定资源是否存在
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns>是否存在</returns>
        public static bool IsThisUriExist(Int64 uri)
        {
            return true;
        }

        /// <summary>
        /// 返回指定资源是否存在
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="type">资源类型</param>
        /// <returns>是否存在</returns>
        public static bool IsThisUriExist(Int64 uri, ResourceType type)
        {
            throw new NotImplementedException();
        }
    }
}
