using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 非法权限异常
    /// </summary>
    public class InvalidPermissionException :ApplicationException
    {
        /// <summary>
        /// 初始化<see cref="InvalidPermissionException"/>类的实例
        /// </summary>
        public InvalidPermissionException()
        {
        }

        /// <summary>
        /// 以错误信息初始化<see cref="InvalidPermissionException"/>类的实例
        /// </summary>
        /// <remarks>方法参数意义可能在后续版本中修改</remarks>
        /// <param name="message">错误信息</param>
        public InvalidPermissionException(string message)
            : base(message)
        {
        }
    }
}
