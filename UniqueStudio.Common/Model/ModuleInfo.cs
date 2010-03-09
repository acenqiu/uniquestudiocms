using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示模块的实体类
    /// </summary>
    [Serializable]
    public class ModuleInfo
    {
        private int moduleId;
        private string moduleName;
        private string displayName;
        private string moduleAuthor;
        private string description;
        private string installFilePath;
        private string parameters;

        /// <summary>
        /// 初始化<see cref="ModuleInfo"/>类的实例
        /// </summary>
        public ModuleInfo()
        {
        }

        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }
        /// <summary>
        /// 模块显示名
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        /// <summary>
        /// 模块作者
        /// </summary>
        public string ModuleAuthor
        {
            get { return moduleAuthor; }
            set { moduleAuthor = value; }
        }
        /// <summary>
        /// 模块说明
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// 模块安装文件路径
        /// </summary>
        public string InstallFilePath
        {
            get { return installFilePath; }
            set { installFilePath = value; }
        }
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }
}
