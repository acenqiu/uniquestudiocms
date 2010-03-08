using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个插件的实体类
    /// </summary>
    [Serializable]
    public class PlugInInfo
    {
        private int plugInId;
        private string plugInName;
        private string displayName;
        private string plugInAuthor;
        private string description;
        private bool isEnabled;
        private string classPath;
        private string assembly;
        private string plugInCategory=string.Empty;
        private int plugInOrdering;
        private string installFilePath=string.Empty;
        private string parameters=string.Empty;

        /// <summary>
        /// 初始化PlugInInfo类的实例
        /// </summary>
        public PlugInInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以类名、程序集初始化PlugInInfo类的实例
        /// </summary>
        /// <param name="classPath"></param>
        /// <param name="assembly"></param>
        public PlugInInfo(string classPath, string assembly)
        {
            this.classPath = classPath;
            this.assembly = assembly;
        }

        /// <summary>
        /// 插件ID
        /// </summary>
        public int PlugInId
        {
            get { return plugInId; }
            set { plugInId = value; }
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string PlugInName
        {
            get { return plugInName; }
            set { plugInName = value; }
        }

        /// <summary>
        /// 插件后台显示名称
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        /// <summary>
        /// 插件作者
        /// </summary>
        public string PlugInAuthor
        {
            get { return plugInAuthor; }
            set { plugInAuthor = value; }
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
        /// 插件是否启用
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassPath
        {
            get { return classPath; }
            set { classPath = value; }
        }

        /// <summary>
        /// 程序集名称
        /// </summary>
        public string Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        }

        /// <summary>
        /// 插件类别
        /// </summary>
        public string PlugInCategory
        {
            get { return plugInCategory; }
            set { plugInCategory = value; }
        }

        /// <summary>
        /// 插件执行顺序
        /// </summary>
        /// <remarks>
        /// 对同一分类排序，如果两个插件排序相同，以在数据库中的顺序为实际执行顺序
        /// </remarks>
        public int PlugInOrdering
        {
            get { return plugInOrdering; }
            set { plugInOrdering = value; }
        }

        /// <summary>
        /// 插件安装文件路径
        /// </summary>
        public string InstallFilePath
        {
            get { return installFilePath; }
            set { installFilePath = value; }
        }

        /// <summary>
        /// 插件参数
        /// </summary>
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }
}
