using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个类的信息的实体类。
    /// </summary>
    [Serializable]
    public class ClassInfo
    {
        private string classPath;
        private string assembly;

        /// <summary>
        /// 初始化ClassInfo类的实例。
        /// </summary>
        public ClassInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以类名、程序集初始化ClassInfo类的实例。
        /// </summary>
        /// <param name="classPath"></param>
        /// <param name="assembly"></param>
        public ClassInfo(string classPath, string assembly)
        {
            this.classPath = classPath;
            this.assembly = assembly;
        }

        /// <summary>
        /// 类名（完整路径）
        /// </summary>
        public string ClassPath
        {
            get { return classPath; }
            set { classPath = value; }
        }

        /// <summary>
        /// 程序集
        /// </summary>
        public string Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        }
    }
}
