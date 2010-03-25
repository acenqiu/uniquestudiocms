using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 非法权限异常
    /// </summary>
    public class InvalidPermissionException : ApplicationException
    {
        /// <summary>
        /// 初始化<see cref="InvalidPermissionException"/>类的实例
        /// </summary>
        public InvalidPermissionException()
        {
        }

        /// <summary>
        /// 以权限名称初始化<see cref="InvalidPermissionException"/>类的实例
        /// </summary>
        /// <remarks>
        /// <para>该重载构造的错误信息格式如下：</para>
        /// <para>您不具有以下权限，请与管理员联系：{permissionName}。</para></remarks>
        /// <param name="permissionName">权限名称</param>
        public InvalidPermissionException(string permissionName)
            : base(string.Format("您不具有以下权限，请与管理员联系：{0}。", permissionName))
        {

        }

        /// <summary>
        /// 以权限名称初始化<see cref="InvalidPermissionException"/>类的实例
        /// </summary>
        /// <remarks>
        /// <para>该重载构造的错误信息格式如下：</para>
        /// <para>您不具有以下权限，请与管理员联系：{permissionName}({permissionDescription})。</para></remarks>
        /// <param name="permissionName">权限名称</param>
        /// <param name="permissionDescription"></param>
        public InvalidPermissionException(string permissionName, string permissionDescription)
            : base(string.Format("您不具有以下权限，请与管理员联系：{0}({1})。", permissionName, permissionDescription))
        {

        }
    }
}
