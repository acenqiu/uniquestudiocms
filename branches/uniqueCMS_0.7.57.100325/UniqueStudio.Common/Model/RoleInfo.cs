//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示一个角色的实体类。
// 完成日期：2010年03月17日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个角色的实体类。
    /// </summary>
    public class RoleInfo
    {
        private int roleId;
        private int siteId;
        private string roleName;
        private string description = string.Empty;
        private int grade;
        private string siteName;
        private PermissionCollection permissions = null;
        private UserCollection users = null;

        /// <summary>
        /// 初始化<see cref="RoleInfo"/>类的实例。
        /// </summary>
        public RoleInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以角色ID初始化<see cref="RoleInfo"/>类的实例。
        /// </summary>
        /// <param name="roleId">角色ID。</param>
        public RoleInfo(int roleId)
        {
            this.roleId = roleId;
        }

        /// <summary>
        /// 以角色名称初始化<see cref="RoleInfo"/>类的实例。
        /// </summary>
        /// <param name="roleName">角色名称。</param>
        public RoleInfo(string roleName)
        {
            this.roleName = roleName;
        }

        /// <summary>
        /// 角色ID。
        /// </summary>
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }
        /// <summary>
        /// 网站ID。
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// 角色名称。
        /// </summary>
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }
        /// <summary>
        /// 描述。
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// 角色级别。
        /// </summary>
        /// <remarks>该属性暂时不使用。</remarks>
        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        /// <summary>
        /// 网站名称。
        /// </summary>
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }
        /// <summary>
        /// 该角色所具有的权限列表。
        /// </summary>
        public PermissionCollection Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }
        /// <summary>
        /// 该角色所具有的用户列表。
        /// </summary>
        public UserCollection Users
        {
            get { return users; }
            set { users = value; }
        }
    }
}
