using System;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示模块的实体类。
    /// </summary>
    [Serializable]
    public class ModuleInfo
    {
        private int moduleId;
        private string moduleName;
        private string displayName;
        private string moduleAuthor;
        private string description;
        private string classPath;
        private string assembly;
        private string workingPath;
        private string config;

        /// <summary>
        /// 初始化<see cref="ModuleInfo"/>类的实例。
        /// </summary>
        public ModuleInfo()
        {
        }

        /// <summary>
        /// 模块ID。
        /// </summary>
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        /// <summary>
        /// 模块名称。
        /// </summary>
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }
        /// <summary>
        /// 模块显示名。
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        /// <summary>
        /// 模块作者。
        /// </summary>
        public string ModuleAuthor
        {
            get { return moduleAuthor; }
            set { moduleAuthor = value; }
        }
        /// <summary>
        /// 模块说明。
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// 类名。
        /// </summary>
        public string ClassPath
        {
            get { return classPath; }
            set { classPath = value; }
        }
        /// <summary>
        /// 程序集名称。
        /// </summary>
        public string Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        }
        /// <summary>
        /// 模块工作路径。
        /// </summary>
        public string WorkingPath
        {
            get { return workingPath; }
            set { workingPath = value; }
        }
        /// <summary>
        /// 参数。
        /// </summary>
        public string Config
        {
            get { return config; }
            set { config = value; }
        }
    }
}
