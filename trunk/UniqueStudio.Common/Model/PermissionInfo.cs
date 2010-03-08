using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个权限的实体类。
    /// </summary>
    public class PermissionInfo
    {
        private int id;
        private string permissionName;
        private string description = string.Empty;
        private string provider;

        /// <summary>
        /// 初始化PermissionInfo类的实例。
        /// </summary>
        public PermissionInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以权限名初始化<see cref="PermissionInfo"/>类的实例
        /// </summary>
        /// <param name="permissionName">权限名</param>
        public PermissionInfo(string permissionName)
        {
            this.permissionName = permissionName;
        }

        /// <summary>
        /// 以权限ID、权限名称描述初始化PermissionInfo类的实例。
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <param name="permissionName">权限名称</param>
        /// <param name="description">描述</param>
        public PermissionInfo(int id, string permissionName, string description)
        {
            this.id = id;
            this.permissionName = permissionName;
            this.description = description;
        }

        /// <summary>
        /// 权限ID
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName
        {
            get { return permissionName; }
            set { permissionName = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 权限提供者（内核，组件）
        /// </summary>
        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }
    }
}
