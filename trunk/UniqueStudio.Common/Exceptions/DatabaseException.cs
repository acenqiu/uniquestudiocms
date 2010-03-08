using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Exceptions
{
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
