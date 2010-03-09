using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 数据库操作异常
    /// </summary>
    public class DatabaseException:ApplicationException
    {
        public DatabaseException()
            :base("数据库出现了异常")
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
