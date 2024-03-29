﻿//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示一个类的信息的实体类。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

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
        /// <param name="classPath">类名。</param>
        /// <param name="assembly">程序集。</param>
        public ClassInfo(string classPath, string assembly)
        {
            this.classPath = classPath;
            this.assembly = assembly;
        }

        /// <summary>
        /// 类名（完整路径）。
        /// </summary>
        public string ClassPath
        {
            get { return classPath; }
            set { classPath = value; }
        }

        /// <summary>
        /// 程序集。
        /// </summary>
        public string Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        }
    }
}
