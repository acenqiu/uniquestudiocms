using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示模块控件的实体类
    /// </summary>
    [Serializable]
    public class ModuleControlInfo
    {
        private string controlId;
        private int moduleId;
        private string moduleName;
        private bool isEnabled;
        private string layoutPath;
        private string parameters;

        /// <summary>
        /// 初始化<see cref="ModuleControlInfo"/>类的实例
        /// </summary>
        public ModuleControlInfo()
        {
        }

        /// <summary>
        /// 控件ID
        /// </summary>
        public string ControlId
        {
            get { return controlId; }
            set { controlId = value; }
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
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
        /// <summary>
        /// 样式模板路径
        /// </summary>
        public string LayoutPath
        {
            get { return layoutPath; }
            set { layoutPath = value; }
        }
        /// <summary>
        /// 参数
        /// </summary>
        /// <remarks>Xml格式</remarks>
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }
}
