using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Exceptions
{
    public class InvalidPermissionException :ApplicationException
    {
        public InvalidPermissionException()
        {
        }

        public InvalidPermissionException(string message)
            : base(message)
        {
        }

        public InvalidPermissionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
