//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示模块控件的实体类。
// 完成日期：2010年03月29日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示模块控件的实体类。
    /// </summary>
    [Serializable]
    public class ModuleControlInfo
    {
        private int controlId;
        private int siteId;
        private string controlName;
        private int moduleId;
        private string moduleName;
        private bool isEnabled;
        private string config;

        /// <summary>
        /// 初始化<see cref="ModuleControlInfo"/>类的实例。
        /// </summary>
        public ModuleControlInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 控件ID。
        /// </summary>
        public int ControlId
        {
            get { return controlId; }
            set { controlId = value; }
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
        /// 控件名称。
        /// </summary>
        public string ControlName
        {
            get { return controlName; }
            set { controlName = value; }
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
        /// 是否启用。
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
        /// <summary>
        /// 配置信息。
        /// </summary>
        public string Config
        {
            get { return config; }
            set { config = value; }
        }
    }
}
