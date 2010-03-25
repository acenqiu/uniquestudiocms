using System;
using System.Collections.Generic;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个插件的实体类。
    /// </summary>
    [Serializable]
    public class PlugInInfo
    {
        private int plugInId;
        private string plugInName;
        private string displayName;
        private string plugInAuthor;
        private string description;
        private string classPath;
        private string assembly;
        private string plugInCategory=string.Empty;
        private int plugInOrdering;
        private string workingPath=string.Empty;
        private string config=string.Empty;
        private List<PlugInInstanceInfo> instances;

        /// <summary>
        /// 初始化PlugInInfo类的实例。
        /// </summary>
        public PlugInInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 插件ID。
        /// </summary>
        public int PlugInId
        {
            get { return plugInId; }
            set { plugInId = value; }
        }
        /// <summary>
        /// 插件名称。
        /// </summary>
        public string PlugInName
        {
            get { return plugInName; }
            set { plugInName = value; }
        }
        /// <summary>
        /// 插件后台显示名称。
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        /// <summary>
        /// 插件作者。
        /// </summary>
        public string PlugInAuthor
        {
            get { return plugInAuthor; }
            set { plugInAuthor = value; }
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
        /// 插件类别。
        /// </summary>
        public string PlugInCategory
        {
            get { return plugInCategory; }
            set { plugInCategory = value; }
        }
        /// <summary>
        /// 插件执行顺序。
        /// </summary>
        public int PlugInOrdering
        {
            get { return plugInOrdering; }
            set { plugInOrdering = value; }
        }
        /// <summary>
        /// 插件工作路径。
        /// </summary>
        public string WorkingPath
        {
            get { return workingPath; }
            set { workingPath = value; }
        }
        /// <summary>
        /// 插件参数。
        /// </summary>
        public string Config
        {
            get { return config; }
            set { config = value; }
        }
        /// <summary>
        /// 插件实例的集合。
        /// </summary>
        public List<PlugInInstanceInfo> Instances
        {
            get { return instances; }
            set { instances = value; }
        }
    }
}
