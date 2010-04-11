//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：数据库操作异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 数据库操作异常。
    /// </summary>
    public class DatabaseException : ApplicationException
    {
        public DatabaseException()
            : base("数据库出现了异常。")
        {
        }

        public DatabaseException(string message)
            : base(message)
        {
        }

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
