using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示组件的实体类
    /// </summary>
    [Serializable]
    public class CompenentInfo
    {
        private int compenentId;
        private string compenentName;
        private string displayName;
        private string compenentAuthor;
        private string description;
        private string installFilePath;

        /// <summary>
        /// 初始化<see cref="CompenentInfo"/>类的实例
        /// </summary>
        public CompenentInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 组件ID
        /// </summary>
        public int CompenentId
        {
            get { return compenentId; }
            set { compenentId = value; }
        }
        /// <summary>
        /// 组件名称
        /// </summary>
        public string CompenentName
        {
            get { return compenentName; }
            set { compenentName = value; }
        }
        /// <summary>
        /// 组件显示名
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        /// <summary>
        /// 组件作者
        /// </summary>
        public string CompenentAuthor
        {
            get { return compenentAuthor; }
            set { compenentAuthor = value; }
        }
        /// <summary>
        /// 组件描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// 安装文件路径
        /// </summary>
        public string InstallFilePath
        {
            get { return installFilePath; }
            set { installFilePath = value; }
        }
    }
}
