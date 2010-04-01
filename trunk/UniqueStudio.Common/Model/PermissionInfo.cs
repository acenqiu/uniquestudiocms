//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示一个权限的实体类。
// 完成日期：2010年03月17日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个权限的实体类。
    /// </summary>
    public class PermissionInfo
    {
        private int permissionId;
        private int siteId;
        private string permissionName;
        private string description = string.Empty;
        private string provider;

        /// <summary>
        /// 初始化<see cref="PermissionInfo"/>类的实例。
        /// </summary>
        public PermissionInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以权限ID初始化<see cref="PermissionInfo"/>类的实例。
        /// </summary>
        /// <param name="permissionId">权限ID。</param>
        public PermissionInfo(int permissionId)
        {
            this.permissionId = permissionId;
        }

        /// <summary>
        /// 以权限名初始化<see cref="PermissionInfo"/>类的实例。
        /// </summary>
        /// <param name="permissionName">权限名。</param>
        public PermissionInfo(string permissionName)
        {
            this.permissionName = permissionName;
        }

        /// <summary>
        /// 以权限ID、权限名称描述初始化PermissionInfo类的实例。
        /// </summary>
        /// <param name="id">权限ID。</param>
        /// <param name="permissionName">权限名称。</param>
        /// <param name="description">描述。</param>
        public PermissionInfo(int permissionId, string permissionName, string description)
        {
            this.permissionId = permissionId;
            this.permissionName = permissionName;
            this.description = description;
        }

        /// <summary>
        /// 权限ID。
        /// </summary>
        public int PermissionId
        {
            get { return permissionId; }
            set { permissionId = value; }
        }

        /// <summary>
        /// 权限名称。
        /// </summary>
        public string PermissionName
        {
            get { return permissionName; }
            set { permissionName = value; }
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
        /// 权限提供者（内核，组件）。
        /// </summary>
        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }
    }
}
