//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：非法权限异常。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Exceptions
{
    /// <summary>
    /// 非法权限异常。
    /// </summary>
    public class InvalidPermissionException : ApplicationException
    {
        /// <summary>
        /// 初始化<see cref="InvalidPermissionException"/>类的实例。
        /// </summary>
        public InvalidPermissionException()
        {
        }

        /// <summary>
        /// 以权限名称初始化<see cref="InvalidPermissionException"/>类的实例。
        /// </summary>
        /// <remarks>
        /// <para>该重载构造的错误信息格式如下：</para>
        /// <para>您不具有以下权限，请与管理员联系：{permissionName}。</para></remarks>
        /// <param name="permissionName">权限名称。</param>
        public InvalidPermissionException(string permissionName)
            : base(string.Format("您不具有以下权限，请与管理员联系：{0}。", permissionName))
        {

        }

        /// <summary>
        /// 以权限名称初始化<see cref="InvalidPermissionException"/>类的实例。
        /// </summary>
        /// <remarks>
        /// <para>该重载构造的错误信息格式如下：</para>
        /// <para>您不具有以下权限，请与管理员联系：{permissionName}({permissionDescription})。</para></remarks>
        /// <param name="permissionName">权限名称。</param>
        /// <param name="permissionDescription">权限描述信息。</param>
        public InvalidPermissionException(string permissionName, string permissionDescription)
            : base(string.Format("您不具有以下权限，请与管理员联系：{0}({1})。", permissionName, permissionDescription))
        {

        }
    }
}
