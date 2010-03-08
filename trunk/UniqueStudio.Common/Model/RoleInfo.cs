using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个角色的实体类。
    /// </summary>
    public class RoleInfo
    {
        private int id;
        private string roleName;
        private string description = string.Empty;
        private int grade;
        private PermissionCollection permissions = null;
        private UserCollection users = null;

        /// <summary>
        /// 初始化一个<see cref="RoleInfo"/>类的实例。
        /// </summary>
        public RoleInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以角色名初始化<see cref="RoleInfo"/>类的实例。
        /// </summary>
        /// <param name="roleName"></param>
        public RoleInfo(string roleName)
        {
            this.roleName = roleName;
        }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
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
        /// 角色级别
        /// </summary>
        /// <remarks>
        /// 该属性暂时不使用</remarks>
        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        /// <summary>
        /// 该角色所具有的权限列表
        /// </summary>
        public PermissionCollection Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }

        /// <summary>
        /// 该角色所具有的用户列表
        /// </summary>
        public UserCollection Users
        {
            get { return users; }
            set { users = value; }
        }
    }
}
